using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Wms.Areas.Master.Models;

namespace Wms.Areas.DataEx.Models.OutputGeneralData
{
    public class OutputLayoutSqlGenerator
    {
        /// <summary>
        /// 出力用のSqlを作成する
        /// </summary>
        /// <param name="layout">対象のテーブル情報</param>
        /// <param name="details">Select句の情報</param>
        /// <param name="conditions">Where句の情報</param>
        /// <returns></returns>
        public static string CreateSql(Layout layout,List<LayoutDetail> details,List<LayoutCondition> conditions)
        {
            var table = layout.ObjectId;
            var select = CreateSelect(details);
            var where = CreateWhere(conditions);
            var order = CreateSortOrder(conditions);

            return $@"
                Select
                    {select}
                From
                    {table} TBL
                Where
                    {where}
                Order by
                    {order}
            ";
        }

        /// <summary>
        /// Select句のクエリ作成
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        private static string CreateSelect(List<LayoutDetail> details)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                ROW_NUMBER() OVER(
                  ORDER BY  TBL.SHIPPER_ID
                )
            ");
            foreach (var item in details)
                sb.AppendLine(item.ConvertQuery());
            return sb.ToString();
        }

        /// <summary>
        /// 条件文のクエリ作成
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private static string CreateWhere(List<LayoutCondition> conditions)
        {
            var sb = new StringBuilder();
            sb.AppendLine("TBL.SHIPPER_ID = :SHIPPER_ID");
            sb.AppendLine("AND TBL.CENTER_ID = :CENTER_ID");
            foreach (var item in conditions.Where(x => x.ConditionClass != (byte)Common.ConditionClass.None))
                sb.AppendLine($"AND {item.ConvertQuery()}");

            return sb.ToString();
        }

        /// <summary>
        /// ソート順のクエリ作成
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private static string CreateSortOrder(List<LayoutCondition> conditions)
        {
            
            var sb = new StringBuilder("TBL.SHIPPER_ID ASC");
            // ソート順位が入っているものだけを設定するようにする
            foreach (var item in  conditions.Where(x => x.SortOrder.HasValue))
            {
                sb.Append($", {item.ConvertOrder()}");
            }

            return sb.ToString();
        }
    }
}