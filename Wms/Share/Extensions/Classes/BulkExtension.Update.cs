namespace Share.Extensions.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Oracle.ManagedDataAccess.Client;

    public static partial class BulkExtension
    {
        /// <summary>
        /// 複数のEnittyをまとめて更新します。
        /// </summary>
        /// <typeparam name="T">Entityの型</typeparam>
        /// <param name="connection">DBコネクション</param>
        /// <param name="entities">対象のデータ</param>
        /// <returns>更新したレコード数</returns>
        /// <remarks>
        /// 対象のEntityをDbSetから取得する場合はAsNoTrackingで取得してください。
        /// このメソッドを実行した後SaveChangesを呼び出すと更新したレコードを変更しようとしてDbUpdateConcurrencyExceptionが発生します。
        /// </remarks>
        public static int BulkUpdate<T>(this OracleConnection connection, IEnumerable<T> entities)
        {
            if (!CheckArg(entities)) return 0;

            var cmd = CreateCommand(connection, MakeUpdateSql(entities.First().GetType()), entities.Count());
            SetAllParameters(cmd, entities);

            // sql発行
            return cmd.ExecuteNonQuery();
        }

        public static string MakeUpdateSql(Type t)
        {
            var tableName = GetTableName(t);

            var sql = new StringBuilder($"UPDATE {tableName} SET");
            sql.AppendLine();
            var where = new StringBuilder("WHERE");
            where.AppendLine();
            var keyprops = GetKeyProperties(t);
            var valueProps = GetPropertiesWithoutNotmap(t);

            var count = 0;
            var kCount = 0;
            foreach (var prop in valueProps)
            {
                var index = count + kCount;
                var colName = GetStrictColName(prop);
                var value = $"{colName} = :p{index}";
                if (keyprops.Contains(prop))
                {
                    // keyはwhere句
                    if (kCount > 0)
                        where.Append("AND ");
                    where.AppendLine(value);
                    kCount++;
                }
                else
                {
                    // keyじゃなければvalue
                    if (count > 0)
                        sql.Append(",");
                    sql.AppendLine(value);
                    count++;
                }
            }

            return $"{sql.ToString()} {where.ToString()}";
        }
    }
}