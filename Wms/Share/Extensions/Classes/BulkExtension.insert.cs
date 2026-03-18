// Mvc.Extensions.Classes.BulkExtension.SetParameters
namespace Share.Extensions.Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Oracle.ManagedDataAccess.Client;

    public static partial class BulkExtension
    {
        public static int BulkInsert<T>(this OracleConnection connection, IEnumerable<T> entities)
        {
            if (!CheckArg(entities)) return 0;

            var cmd = CreateCommand(connection, MakeInsertSql(entities.First().GetType()), entities.Count());
            SetAllParameters(cmd, entities);

            return cmd.ExecuteNonQuery();
        }

        private static OracleCommand CreateCommand(OracleConnection connection, string sql, int entityCount)
        {
            var cmd = connection.CreateCommand();
            var count = entityCount;
            SetUpCmdForArrayBind(cmd, sql, count);
            return cmd;
        }

        private static void SetUpCmdForArrayBind(OracleCommand cmd, string sql, int count)
        {
            cmd.CommandText = sql;
            cmd.BindByName = true;
            cmd.ArrayBindCount = count;
        }

        private static void SetAllParameters<T>(OracleCommand cmd, IEnumerable<T> entities, ParamName userPropName = ParamName.Index)
        {
            var t = entities.First().GetType();
            SetParameters(cmd, entities, GetPropertiesWithoutNotmap(t), userPropName: userPropName);
        }

        private static void SetParameters<T>(OracleCommand cmd, IEnumerable<T> entities, PropertyInfo[] props, string pname = "p", ParamName userPropName = ParamName.Index)
        {
            var list = new List<List<object>>(props.Length);
            var entityCount = entities.Count();
            for (int i = 0; i < props.Length; i++)
                list.Add(new List<object>(entityCount));

            foreach (var entity in entities)
            {
                AddEntityValues(list, props, entity);
            }

            foreach (var param in new OraparamFactory(props, list, pname).CreateParameters(userPropName))
                cmd.Parameters.Add(param);
        }

        private static void AddEntityValues<T>(List<List<object>> list, PropertyInfo[] props, T entity)
        {
            var index = 0;
            foreach (var prop in props)
                list[index++].Add(prop.GetValue(entity));
        }

        private static PropertyInfo[] GetPropertiesWithoutNotmap(Type t)
        {
            return t.GetProperties().Where(x =>
            {
                var prop = x.GetCustomAttribute(typeof(NotMappedAttribute));
                return prop == null;
            }).ToArray();
        }

        private static string MakeInsertSql(Type t)
        {
            var tableName = GetTableName(t);
            var sql = new StringBuilder($"INSERT INTO {tableName} (");
            var param = new StringBuilder("VALUES (");
            var count = 0;
            foreach (var prop in GetPropertiesWithoutNotmap(t))
            {
                if (count > 0)
                {
                    sql.Append(",");
                    param.Append(",");
                }

                sql.AppendLine(GetStrictColName(prop));
                param.AppendLine($":p{count}");
                count++;
            }

            sql.AppendLine(")");
            param.AppendLine(")");
            return $"{sql.ToString()}{param.ToString()}";
        }

        private static string GetTableName(Type t)
        {
            var table = t.GetCustomAttributes(typeof(TableAttribute), false).First() as TableAttribute;
            return table.Name;
        }

        private static string GetStrictColName(PropertyInfo info)
        {
            return GetStrictColName(info.Name);
        }

        private static string GetStrictColName(ParameterInfo info)
        {
            return GetStrictColName(info.Name);
        }

        private static string GetStrictColName(string name)
        {
            var arr = name.ToCharArray();
            var result = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                if (i > 0 && char.IsUpper(arr[i])) result.Append('_');
                result.Append(char.ToUpper(arr[i]));
            }

            return result.ToString();
        }

        private static bool CheckArg<T>(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities is null");
            if (entities.Count() == 0) return false;
            var first = entities.First();
            return true;
        }
    }
}