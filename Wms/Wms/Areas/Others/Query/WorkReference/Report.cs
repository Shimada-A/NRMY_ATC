namespace Wms.Areas.Others.Query.WorkReference
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Others.ViewModels.WorkReference;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Others.ViewModels.WorkReference.WorkReferenceSearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<WorkReferenceReport> WorkReferenceListing(WorkReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
            SELECT
                    *
            FROM
                    WW_OTH_WORK_PERF WW
            WHERE
                    WW.SHIPPER_ID = :SHIPPER_ID
                AND WW.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Sort function
            switch (condition.SortKey)
            {
                case WorkReferenceSortKey.ProcessingTypeWorkStartDate:
                    switch (condition.Sort)
                    {
                        case WorkReferenceSearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY PROCESSING_TYPE DESC,WORK_START_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY PROCESSING_TYPE ASC,WORK_START_DATE ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case WorkReferenceSearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WORK_START_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WORK_START_DATE ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<WorkReferenceReport>(query.ToString(), parameters);
        }
    }
}