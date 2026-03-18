namespace Wms.Areas.Ship.Query.EcAllocationCancel
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.EcAllocationCancel;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.EcAllocationCancel.EcAllocationCancelSearchConditions;

    public class EcAllocationCancelQuery
    {
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<EcAllocationCancelResultRow> GetData(EcAllocationCancelSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT T.SHIP_REQUEST_DATE
                      ,T.SHIP_INSTRUCT_ID
                      ,T.ORDER_DATE
                      ,MT.TRANSPORTER_NAME
                      ,T.ARRIVE_REQUEST_DATE
                      ,T.DATA_DATE
                      ,T.ORDER_QTY
                      ,T.ITEM_SKU_QTY
                  FROM (
                    SELECT MAX(TE.SHIP_REQUEST_DATE) SHIP_REQUEST_DATE
                          ,TE.SHIP_INSTRUCT_ID
                          ,MAX(TE.ORDER_DATE) ORDER_DATE
                          ,MAX(TE.TRANSPORTER_ID) TRANSPORTER_ID
                          ,MAX(TE.ARRIVE_REQUEST_DATE) ARRIVE_REQUEST_DATE
                          ,MAX(TE.MAKE_DATE) DATA_DATE
                          ,SUM(TE.ORDER_QTY) ORDER_QTY
                          ,COUNT(DISTINCT(TE.ITEM_SKU_ID)) ITEM_SKU_QTY
                          ,TE.CENTER_ID
                          ,TE.SHIPPER_ID
                      FROM T_ECSHIPS TE
                     INNER JOIN M_ITEM_SKU MIS
                        ON TE.SHIPPER_ID = MIS.SHIPPER_ID
                       AND TE.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                     INNER JOIN M_COLORS MC
                        ON TE.SHIPPER_ID   = MC.SHIPPER_ID
                       AND TE.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                     INNER JOIN M_SIZES MS
                        ON TE.SHIPPER_ID   = MS.SHIPPER_ID
                       AND TE.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                     WHERE TE.BATCH_NO IN(
                           SELECT TAI.ALLOC_NO 
                             FROM T_ALLOC_INFO TAI
                            WHERE TAI.SHIPPER_ID = :SHIPPER_ID
                              AND TAI.CENTER_ID = :CENTER_ID
                              AND SHIP_KIND = 3
                              AND TAI.ALLOC_GROUP_NO = :ALLOC_GROUP_NO)
                       AND TE.SHIPPER_ID = :SHIPPER_ID
                       AND TE.CENTER_ID = :CENTER_ID
                     GROUP BY TE.SHIP_INSTRUCT_ID,TE.CENTER_ID,TE.SHIPPER_ID ) T
                  LEFT JOIN M_TRANSPORTERS MT
                    ON T.SHIPPER_ID   = MT.SHIPPER_ID
                   AND T.TRANSPORTER_ID = MT.TRANSPORTER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":ALLOC_GROUP_NO", condition.AllocGroupNo);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<EcAllocationCancelResultRow>(query.ToString(), parameters).Count();
            var all = MvcDbContext.Current.Database.Connection.Query<EcAllocationCancelResultRow>(query.ToString(), parameters);

            // Sort function
            switch (condition.SortKey)
            {
                case EcAllocationCancelSortKey.ShipInstructId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY T.SHIP_INSTRUCT_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY T.SHIP_INSTRUCT_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY T.SHIP_REQUEST_DATE DESC,T.ORDER_DATE DESC,T.SHIP_INSTRUCT_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY T.SHIP_REQUEST_DATE ASC,T.ORDER_DATE ASC,T.SHIP_INSTRUCT_ID ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var EcAllocationCancels = MvcDbContext.Current.Database.Connection.Query<EcAllocationCancelResultRow>(query.ToString(), parameters);
            condition.OrderQtySum = all.Count();
            condition.ItemSkuQtySum = all.Select(x => x.ItemSkuQty).Sum();
            condition.PlanQtySum = all.Select(x => x.OrderQty).Sum();
            // Excute paging
            return new StaticPagedList<EcAllocationCancelResultRow>(EcAllocationCancels, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void EcAllocationCancel(EcAllocationCancelSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_ALLOC_GROUP_NO", searchConditions.AllocGroupNo, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_ECAllOC_CANCEL",
                param,
                commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            if (status == ProcedureStatus.Success)
            {
                message = string.Format(EcAllocationCancelResource.SUC_UPDATE, searchConditions.AllocGroupNo);
            }
            else
            {
                message = param.Get<string>("OUT_MESSAGE");
            }
        }

        /// <summary>
        /// バッチNo取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListBatchNos(string centerId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                SELECT TAI.ALLOC_GROUP_NO VALUE
                      ,TAI.ALLOC_GROUP_NO || ':' || MAX(TAI.BATCH_NAME) TEXT
                  FROM T_ALLOC_INFO TAI
                  LEFT JOIN (
                             SELECT MAX(TP.PIC_STATUS) PIC_STATUS
                                   ,TP.BATCH_NO
                                   ,TP.CENTER_ID
                                   ,TP.SHIPPER_ID
                               FROM T_PIC TP
                              GROUP BY TP.BATCH_NO,TP.CENTER_ID,TP.SHIPPER_ID ) TP
                    ON TAI.SHIPPER_ID = TP.SHIPPER_ID
                   AND TAI.CENTER_ID = TP.CENTER_ID
                   AND TAI.ALLOC_NO = TP.BATCH_NO
                 WHERE TAI.SHIPPER_ID = :SHIPPER_ID
                   AND TAI.CENTER_ID = :CENTER_ID
                   AND TAI.SHIP_KIND = 3
                   AND (TP.PIC_STATUS = 0 OR TP.SHIPPER_ID IS NULL)
                 GROUP BY TAI.ALLOC_GROUP_NO,TAI.CENTER_ID,TAI.SHIPPER_ID
                 ORDER BY MAX(TAI.ALLOC_DATE) DESC
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", string.IsNullOrWhiteSpace(centerId) ?Profile.User.CenterId : centerId);

            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }
    }
}