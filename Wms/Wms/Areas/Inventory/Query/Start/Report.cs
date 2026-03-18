namespace Wms.Areas.Inventory.Query.Start
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Inventory.Resources;
    using Wms.Areas.Inventory.ViewModels.Start;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Inventory.ViewModels.Start.StartSearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<StartReport> StartListing(StartSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT '' AS CENTER_ID
                      ,'' AS LOCATION_CD
                  FROM DUAL
            ");

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<StartReport>(query.ToString());
        }

            /// <summary>
            /// アップロードされたデータのImport
            /// </summary>
            /// <param name="report"></param>
            /// <returns></returns>
            public void InsertWW_INV_START_01(IEnumerable<ViewModels.Start.StartReport> report, out string message,out long workId)
        {
            var dbContext = MvcDbContext.Current;
            var nullitems = report.Where(x => string.IsNullOrWhiteSpace(x.CenterId) || string.IsNullOrWhiteSpace(x.LocationCd));
            workId = GetWorkId();
            if (nullitems.Any())
            {
                message = StartResource.ERR_NOT_MATCH;
            }
            else
            {
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    foreach (var u in report.Select((v, i) => new { v, i }))
                    {
                        var wwInvStart01 = new Models.InvStart_01
                        {
                            Seq = workId,
                            LineNo = u.i + 1,
                            CenterId = u.v.CenterId,
                            LocationCd = u.v.LocationCd
                        };
                        wwInvStart01.SetBaseInfoInsert();
                        dbContext.InvStart_01s.Add(wwInvStart01);
                        try
                        {
                            dbContext.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                        }
                    }

                    trans.Commit();
                }
                message = string.Empty;
            }
        }
    }
}