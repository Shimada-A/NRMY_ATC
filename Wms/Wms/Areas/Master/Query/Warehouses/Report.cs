namespace Wms.Areas.Master.Query.Warehouses
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.Warehouses;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;
    using static Wms.Areas.Master.ViewModels.Warehouses.WarehousesSearchCondition;

    public class Report : BaseQuery
    {
        /// <summary>
        /// エクセルに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<ViewModels.Warehouses.Report> Listing(WarehousesSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       TO_CHAR(MW.MAKE_DATE,'YYYY/MM/DD') AS MAKE_DATE
                      ,TO_CHAR(MW.UPDATE_DATE,'YYYY/MM/DD') AS UPDATE_DATE
                      ,MW.CENTER_ID
                      ,MW.CENTER_CLASS
                      ,MW.CENTER_NAME1
                      ,MW.CENTER_SHORT_NAME
                      ,MW.CENTER_ZIP
                      ,MW.CENTER_PREF_NAME
                      ,MW.CENTER_CITY_NAME
                      ,MW.CENTER_ADDRESS1
                      ,MW.CENTER_ADDRESS2
                      ,MW.CENTER_ADDRESS3
                      ,MW.CENTER_TEL
                      ,MW.CENTER_FAX
                      ,MP.PREF_ID
                      ,CHANNEL_ID
                      ,CHANNEL_NAME
                      ,MW.WMS_CLASS
                      ,MW.BRAND_WORK_CLASS
                      ,MW.INVOICE_CENTER_NAME
                      ,MW.DELETE_FLAG
                  FROM M_CENTERS MW
                  LEFT JOIN M_PREFS MP
                    ON MP.SHIPPER_ID = MW.SHIPPER_ID
                   AND MP.PREF_NAME = MW.CENTER_PREF_NAME
                 WHERE MW.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND MW.CENTER_ID LIKE :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId + "%");
            }

            if (!string.IsNullOrEmpty(condition.CenterName))
            {
                query.Append(" AND MW.CENTER_NAME1 LIKE :CENTER_NAME ");
                parameters.Add(":CENTER_NAME", condition.CenterName + "%");
            }

            if (!string.IsNullOrEmpty(condition.CenterAddress))
            {
                query.Append(@" AND (MW.CENTER_PREF_NAME
                                 ||  MW.CENTER_CITY_NAME
                                 ||  MW.CENTER_ADDRESS1
                                 ||  MW.CENTER_ADDRESS2
                                 ||  MW.CENTER_ADDRESS3 LIKE :CENTER_ADDRESS)");
                parameters.Add(":CENTER_ADDRESS", "%" + condition.CenterAddress + "%");
            }

            if (!string.IsNullOrEmpty(condition.CenterTel))
            {
                query.Append(" AND REPLACE(MW.CENTER_TEL,'-') LIKE :CENTER_TEL ");
                parameters.Add(":CENTER_TEL", condition.CenterTel + "%");
            }

            // Sort function
            switch (condition.SortKey)
            {
                case WarehousesSortKey.CenterName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MW.CENTER_NAME1 DESC,MW.CENTER_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MW.CENTER_NAME1 ASC,MW.CENTER_ID ASC");
                            break;
                    }

                    break;

                default:

                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MW.CENTER_ID DESC,MW.CENTER_NAME1 DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MW.CENTER_ID ASC,MW.CENTER_NAME1 ASC");
                            break;
                    }

                    break;
            }

            return MvcDbContext.Current.Database.Connection.Query<ViewModels.Warehouses.Report>(query.ToString(), parameters).ToList();
        }
    }
}