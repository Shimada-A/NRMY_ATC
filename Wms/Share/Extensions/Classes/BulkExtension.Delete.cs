namespace Share.Extensions.Classes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Oracle.ManagedDataAccess.Client;

    public static partial class BulkExtension
    {
        public static int BulkDelete<T>(this OracleConnection connection, IEnumerable<T> entities)
        {
            if (!CheckArg(entities)) return 0;

            var cmd = CreateCommand(connection, MakeDeleteSql(entities.First().GetType()), entities.Count());
            SetKeyParameters(cmd, entities);

            return cmd.ExecuteNonQuery();
        }

        private static void SetKeyParameters<T>(OracleCommand cmd, IEnumerable<T> entities)
        {
            var t = entities.First().GetType();
            SetParameters(cmd, entities, GetKeyProperties(t));
        }

        private static PropertyInfo[] GetKeyProperties(Type t)
        {
            return t.GetProperties().Where(x =>
            {
                return x.GetCustomAttribute(typeof(KeyAttribute)) != null;
            }).ToArray();
        }

        private static string MakeDeleteSql(Type t)
        {
            var tableName = GetTableName(t);
            var sql = new StringBuilder($"DELETE FROM {tableName} WHERE");
            sql.AppendLine();
            var count = 0;
            var keyprops = GetKeyProperties(t);
            if (keyprops.Length == 0)
                throw new Exception("Error : Entity has not table keys");

            foreach (var prop in keyprops)
            {
                if (count > 0)
                {
                    sql.Append("AND ");
                }

                sql.AppendLine($"{GetStrictColName(prop)} = :p{count}");
                count++;
            }

            return sql.ToString();
        }
    }
}