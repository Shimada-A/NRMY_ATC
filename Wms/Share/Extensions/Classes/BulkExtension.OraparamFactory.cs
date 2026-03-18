namespace Share.Extensions.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using Oracle.ManagedDataAccess.Client;

    public static partial class BulkExtension
    {
        private class ParameterInfo
        {
            public string Name { get; set; }

            public Type PropertyType { get; set; }

            public object Value { get; set; }

            public DbType? DbType { get; set; }

            public int? Size { get; set; }

            public ParameterDirection? Direction { get; set; }

            public ParameterInfo() { }

            public ParameterInfo(PropertyInfo prop)
            {
                this.Name = prop.Name;
                this.PropertyType = prop.PropertyType;
            }
        }

        private enum ParamName
        {
            UpperSnake,
            Default,
            Index
        }

        private class OraparamFactory
        {
            // 動的にObjectからのキャストが必要なため、GenericMethodをrefrectionで呼び出す
            private MethodInfo _setMethodInfo = typeof(OraparamFactory).GetMethod("SetOraParamValue", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

            private readonly List<ParameterInfo> _props;
            private readonly List<List<object>> _itemlists;
            private readonly string _paramPrefix;

            public OraparamFactory(List<ParameterInfo> props, List<List<object>> itemlists, string paramPrefix)
            {
                this._props = props;
                this._itemlists = itemlists;
                this._paramPrefix = paramPrefix;
            }

            public OraparamFactory(PropertyInfo[] props, List<List<object>> itemlists, string paramPrefix)
                : this(props.Select(x => new ParameterInfo(x)).ToList(), itemlists, paramPrefix)
            {
            }

            public IEnumerable<OracleParameter> CreateParameters(ParamName paramnameIsPropertyName = ParamName.Index)
            {
                for (int i = 0; i < this._props.Count(); i++)
                {
                    var oraParam = new OracleParameter();
                    if (paramnameIsPropertyName == ParamName.UpperSnake)
                        oraParam.ParameterName = GetStrictColName(this._props[i]);
                    else if (paramnameIsPropertyName == ParamName.Default)
                        oraParam.ParameterName = this._props[i].Name;
                    else
                        oraParam.ParameterName = $"{this._paramPrefix}{i}";

                    // oraParam.Size = _props[i].Size ?? 0;
                    oraParam.Size = this._itemlists[i].Count();

                    if (this._props[i].Direction.HasValue)
                    {
                        oraParam.Direction = this._props[i].Direction.Value;
                        if (this._props[i].Size.HasValue)
                        {
                            oraParam.ArrayBindSize = Enumerable.Repeat(this._props[i].Size.Value, this._itemlists[i].Count()).ToArray();
                        }
                    }

                    if (this._props[i].DbType.HasValue) oraParam.DbType = this._props[i].DbType.Value;

                    // 特殊な変換が必要な処理でなければgenericsのセット処理に流す
                    if (!this.SetOraParamValueWithConvert(oraParam, this._props[i].PropertyType, this._itemlists[i]))
                        this._setMethodInfo.MakeGenericMethod(this._props[i].PropertyType).Invoke(this, new object[] { oraParam, this._itemlists[i] });

                    yield return oraParam;
                }
            }

            private bool SetOraParamValueWithConvert(OracleParameter param, Type valueType, List<object> items)
            {
                // enum
                if (valueType.IsEnum)
                {
                    param.OracleDbType = OracleDbType.Int32;

                    // 定義されている型でセット処理に流す
                    this._setMethodInfo.MakeGenericMethod(Enum.GetUnderlyingType(valueType)).Invoke(this, new object[] { param, items });
                }

                // enum?
                var underlyingType = Nullable.GetUnderlyingType(valueType);
                if (underlyingType != null && underlyingType.IsEnum)
                {
                    param.Value = Activator.CreateInstance(Enum.GetUnderlyingType(underlyingType));
                    param.Value = items.ToArray();
                }

                if (valueType == typeof(DateTimeOffset))
                {
                    param.Value = items.Select(x => ((DateTimeOffset)x).DateTime).ToArray();
                }

                if (valueType == typeof(DateTimeOffset?))
                {
                    param.IsNullable = true;
                    param.Value = default(DateTime);
                    param.Value = items.Select(x => ((DateTimeOffset?)x).HasValue ? ((DateTimeOffset?)x).Value.DateTime as DateTime? : null).ToArray();
                }

                if (valueType == typeof(bool))
                {
                    param.OracleDbType = OracleDbType.Byte;

                    // boolはbyteにコンバートして渡す
                    param.Value = items.Select(x => Convert.ToByte(x)).ToArray();
                }

                return param.Value != null;
            }

            private void SetOraParamValue<T>(OracleParameter param, List<object> items)
            {
                var underlyingType = Nullable.GetUnderlyingType(typeof(T));
                if (underlyingType != null)
                {
                    // nullable型の場合、直接配列をセットすると、nullがあった場合にエラーになるため、
                    // デフォルト値を一度セットしてパラメータの型を確定する(妥協案)
                    param.Value = Activator.CreateInstance(underlyingType);
                    param.IsNullable = true;
                }
                else
                {
                    param.Value = default(T);
                }

                var arr = items.Select(x => (T)x).ToArray();
                param.Value = arr;
            }
        }
    }
}