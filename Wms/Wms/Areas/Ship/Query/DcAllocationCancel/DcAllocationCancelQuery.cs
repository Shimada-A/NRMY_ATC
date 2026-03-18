namespace Wms.Areas.Ship.Query.DcAllocationCancel
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
    using Wms.Areas.Ship.ViewModels.DcAllocationCancel;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.DcAllocationCancel.DcAllocationCancelSearchConditions;

    public class DcAllocationCancelQuery
    {
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<DcAllocationCancelResultRow> GetData(DcAllocationCancelSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT TS.SHIP_PLAN_DATE
                      ,MG.GEN_NAME EMERGENCY_CLASS_NAME
                      ,TS.SHIP_INSTRUCT_ID
                      ,TS.SHIP_INSTRUCT_SEQ
                      ,TS.ITEM_ID
                      ,TS.ITEM_NAME
                      ,TS.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TS.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TS.SHIP_TO_STORE_ID
                      ,MST.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                      ,MG1.GEN_NAME SHIP_TO_STORE_CLASS_NAME
                      ,MT.TRANSPORTER_NAME 
                      ,TS.INSTRUCT_QTY
                      ,TS.ITEM_SKU_ID
                      ,TS.ALLOC_QTY
                  FROM T_SHIPS TS
                  LEFT JOIN M_GENERALS MG
                    ON TS.SHIPPER_ID = MG.SHIPPER_ID
                   AND MG.REGISTER_DIVI_CD = '1'
                   AND MG.CENTER_ID = '@@@'
                   AND MG.GEN_DIV_CD = 'EMERGENCY_CLASS'
                   AND TO_CHAR(TS.EMERGENCY_CLASS) = MG.GEN_CD
                  LEFT JOIN M_ITEM_SKU MIS
                    ON TS.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                  LEFT JOIN M_COLORS MC
                    ON TS.SHIPPER_ID   = MC.SHIPPER_ID
                   AND TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN M_SIZES MS
                    ON TS.SHIPPER_ID   = MS.SHIPPER_ID
                   AND TS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                  LEFT JOIN V_SHIP_TO_STORES MST
                    ON TS.SHIPPER_ID   = MST.SHIPPER_ID
                   AND TS.SHIP_TO_STORE_ID = MST.SHIP_TO_STORE_ID
                  LEFT JOIN M_GENERALS MG1
                    ON TS.SHIPPER_ID = MG1.SHIPPER_ID
                   AND MG1.REGISTER_DIVI_CD = '1'
                   AND MG1.CENTER_ID = '@@@'
                   AND MG1.GEN_DIV_CD = 'STORE_CLASS'
                   AND TS.STORE_CLASS = MG1.GEN_CD
                  LEFT JOIN M_TRANSPORTERS MT
                    ON TS.SHIPPER_ID   = MT.SHIPPER_ID
                   AND TS.TRANSPORTER_ID = MT.TRANSPORTER_ID
                 WHERE TS.SHIP_KIND = 2
                   AND TS.ALLOC_FLAG = 1
                   AND TS.SHIPPER_ID = :SHIPPER_ID
                   AND TS.CENTER_ID = :CENTER_ID
                   AND TS.BATCH_NO = :BATCH_NO
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":BATCH_NO", condition.BatchNo);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<DcAllocationCancelResultRow>(query.ToString(), parameters).Count();
            var all = MvcDbContext.Current.Database.Connection.Query<DcAllocationCancelResultRow>(query.ToString(), parameters);

            // Sort function
            switch (condition.SortKey)
            {
                case DcAllocationCancelSortKey.SkuInstructIdDetailId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TS.ITEM_SKU_ID DESC,TS.SHIP_INSTRUCT_ID DESC,TS.SHIP_INSTRUCT_SEQ DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TS.ITEM_SKU_ID ASC,TS.SHIP_INSTRUCT_ID ASC,TS.SHIP_INSTRUCT_SEQ ASC");
                            break;
                    }

                    break;

                case DcAllocationCancelSortKey.ShipIdSkuInstructIdDetailId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TS.SHIP_TO_STORE_ID DESC,TS.ITEM_SKU_ID DESC,TS.SHIP_INSTRUCT_ID DESC,TS.SHIP_INSTRUCT_SEQ DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TS.SHIP_TO_STORE_ID ASC,TS.ITEM_SKU_ID ASC,TS.SHIP_INSTRUCT_ID ASC,TS.SHIP_INSTRUCT_SEQ ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TS.SHIP_PLAN_DATE DESC,TS.SHIP_INSTRUCT_ID DESC,TS.ITEM_SKU_ID DESC,TS.SHIP_INSTRUCT_SEQ DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TS.SHIP_PLAN_DATE ASC,TS.SHIP_INSTRUCT_ID ASC,TS.ITEM_SKU_ID ASC,TS.SHIP_INSTRUCT_SEQ ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var dcAllocationCancels = MvcDbContext.Current.Database.Connection.Query<DcAllocationCancelResultRow>(query.ToString(), parameters);
            condition.DetailSum = all.Count();
            condition.ItemSkuSum = all.Select(x => x.ItemSkuId).Distinct().Count();
            condition.PlanQtySum = all.Select(x => x.InstructQty).Sum();
            // Excute paging
            return new StaticPagedList<DcAllocationCancelResultRow>(dcAllocationCancels, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void DcAllocationCancel(DcAllocationCancelSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BATCH_NO", searchConditions.BatchNo, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_DCAllOC_CANCEL",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
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
                WITH
                    PIC_DATA AS (
                        SELECT 
                                MAX(TP.PIC_STATUS) PIC_STATUS
                            ,   TP.BATCH_NO
                            ,   TP.CENTER_ID
                            ,   TP.SHIPPER_ID
                        FROM 
                                T_PIC TP
                        WHERE
                                TP.SHIPPER_ID = :SHIPPER_ID
                            AND TP.CENTER_ID = :CENTER_ID
                        GROUP BY 
                                TP.BATCH_NO
                            ,   TP.CENTER_ID
                            ,   TP.SHIPPER_ID
                    )
                    ,ORD_PIC_DATA AS (
                        SELECT 
                                MAX(TOP.PIC_STATUS) PIC_STATUS
                            ,   TOP.BATCH_NO
                            ,   TOP.CENTER_ID
                            ,   TOP.SHIPPER_ID
                        FROM 
                                T_ORDER_PIC TOP
                        WHERE
                                TOP.SHIPPER_ID = :SHIPPER_ID
                            AND TOP.CENTER_ID = :CENTER_ID
                        GROUP BY 
                                TOP.BATCH_NO
                            ,   TOP.CENTER_ID
                            ,   TOP.SHIPPER_ID
                    )
                SELECT 
                        TAI.ALLOC_NO VALUE
                    ,   TAI.ALLOC_NO || ':' || TAI.BATCH_NAME TEXT
                FROM 
                        T_ALLOC_INFO TAI
                LEFT JOIN 
                        PIC_DATA TP
                ON 
                        TAI.SHIPPER_ID = TP.SHIPPER_ID
                    AND TAI.CENTER_ID = TP.CENTER_ID
                    AND TAI.ALLOC_NO = TP.BATCH_NO
                LEFT JOIN
                        ORD_PIC_DATA TOP
                ON 
                        TAI.SHIPPER_ID = TOP.SHIPPER_ID
                    AND TAI.CENTER_ID = TOP.CENTER_ID
                    AND TAI.ALLOC_NO = TOP.BATCH_NO
                WHERE 
                        TAI.SHIPPER_ID = :SHIPPER_ID
                    AND TAI.CENTER_ID = :CENTER_ID
                    AND TAI.SHIP_KIND = 2
                    AND (TP.PIC_STATUS = 0 OR TP.SHIPPER_ID IS NULL)
                    AND (TOP.PIC_STATUS = 0 OR TOP.SHIPPER_ID IS NULL)
                ORDER BY 
                        TAI.ALLOC_DATE DESC
                    ,   TAI.ALLOC_NO
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", string.IsNullOrWhiteSpace(centerId) ?Profile.User.CenterId : centerId);

            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }
    }
}