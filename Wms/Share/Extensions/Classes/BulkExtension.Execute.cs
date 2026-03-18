namespace Share.Extensions.Classes
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Web.Routing;
    using Dapper;
    using Oracle.ManagedDataAccess.Client;

    public class BulkExecuteResult
    {
        public OracleCommand Command;
        public int ResultCount;
    }

    public static partial class BulkExtension
    {
        /// <summary>
        /// 指定のsqlを実行
        /// entityの型に定義されているproperty nameを使用し、sqlパラメータにバインドする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static BulkExecuteResult BulkExecute<T>(this OracleConnection connection, string sql, IEnumerable<T> entities)
        {
            if (!CheckArg(entities)) return null;

            var cmd = CreateCommand(connection, sql, entities.Count());
            SetAllParameters(cmd, entities, ParamName.UpperSnake);

            var res = new BulkExecuteResult
            {
                ResultCount = cmd.ExecuteNonQuery(),
                Command = cmd
            };
            return res;
        }

        public static BulkExecuteResult BulkExecute(this OracleConnection connection, string sql, IEnumerable<DynamicParameters> parameters)
        {
            if (!CheckArg(parameters)) return null;

            var cmd = CreateCommand(connection, sql, parameters.Count());
            SetDynamicParam(cmd, parameters);
            var res = new BulkExecuteResult
            {
                ResultCount = cmd.ExecuteNonQuery(),
                Command = cmd
            };
            return res;
        }

        private static void SetDynamicParam(OracleCommand cmd, IEnumerable<DynamicParameters> entities)
        {
            var list = new Dictionary<string, List<object>>();
            var paramInfo = new List<ParameterInfo>();
            var use = new List<bool>();

            foreach (var item in entities)
            {
                var dict = DynamicParamToDictionary(item);
                if (paramInfo.Count() == 0)
                {
                    for (int i = 0; i < dict.Count(); i++)
                    {
                        paramInfo.Add(new ParameterInfo());
                    }

                    use.AddRange(Enumerable.Repeat(false, dict.Count()));
                }

                var cont = 0;
                foreach (var pair in dict)
                {
                    if (!use[cont])
                    {
                        list.Add(pair.Key, new List<object>());
                        paramInfo[cont] = pair.Value;
                        use[cont] = true;
                    }

                    list[pair.Key].Add(pair.Value.Value);
                    cont++;
                }
            }

            var paramList = new List<List<object>>(paramInfo.Count());
            for (int i = 0; i < paramInfo.Count(); i++)
            {
                paramList.Add(list[paramInfo[i].Name]);
            }

            foreach (var param in new OraparamFactory(paramInfo, paramList, string.Empty).CreateParameters(ParamName.Default))
                cmd.Parameters.Add(param);
        }

        private static Dictionary<string, ParameterInfo> DynamicParamToDictionary(DynamicParameters entity)
        {
            var templateFieldInfo = typeof(DynamicParameters).GetField("templates", BindingFlags.NonPublic | BindingFlags.Instance);

            var dict = new Dictionary<string, ParameterInfo>();
            var templateField = templateFieldInfo.GetValue(entity) as List<object>;
            foreach (var item in templateField)
            {
                var routeValueDict = new RouteValueDictionary(item);
                foreach (var pair in routeValueDict)
                {
                    if (dict.ContainsKey(pair.Key)) dict[pair.Key].Value = pair.Value;
                    else dict.Add(pair.Key, new ParameterInfo { Name = pair.Key, Value = pair.Value });
                    if (pair.Value == null) dict[pair.Key].PropertyType = string.Empty.GetType();
                    else dict[pair.Key].PropertyType = pair.Value.GetType();
                }
            }

            var paramFieldInfo = typeof(DynamicParameters).GetField("parameters", BindingFlags.NonPublic | BindingFlags.Instance);
            var paramField = paramFieldInfo.GetValue(entity) as IDictionary;
            var valueType = typeof(DynamicParameters).GetNestedType("ParamInfo", BindingFlags.NonPublic);
            var valueProp = valueType.GetProperty("Value");
            var sizeProp = valueType.GetProperty("Size");
            var direcProp = valueType.GetProperty("ParameterDirection");
            var dbTypeProp = valueType.GetProperty("DbType");
            foreach (var item in paramField.Keys.Cast<string>())
            {
                if (!dict.ContainsKey(item)) dict.Add(item, new ParameterInfo());
                dict[item].Name = item;
                dict[item].Value = valueProp.GetValue(paramField[item]);
                dict[item].Size = sizeProp.GetValue(paramField[item]) as int?;
                dict[item].Direction = direcProp.GetValue(paramField[item]) as ParameterDirection?;
                dict[item].DbType = dbTypeProp.GetValue(paramField[item]) as DbType?;
                if (dict[item].Value == null) dict[item].PropertyType = string.Empty.GetType();
                else dict[item].PropertyType = dict[item].Value.GetType();
            }

            return dict;
        }
    }
}