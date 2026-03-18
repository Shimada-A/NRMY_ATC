namespace Wms.Areas.Ship.Query.DcAllocation
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Ship.ViewModels.DcAllocation;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.DcAllocation.DcAllocationSearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、引当エラー(商品別)のデータを作る。</returns>
        public IEnumerable<DcAllocationItemReport> DcAllocationItemListing(DcAllocationSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TS.CENTER_ID
                      ,TS.BATCH_NO
                      ,MAX(TAI.BATCH_NAME) BATCH_NAME
                      ,MIS.CATEGORY_ID1
                      ,MAX(MIC1.CATEGORY_NAME1) CATEGORY_NAME1
                      ,MAX(TS.ITEM_ID) ITEM_ID
                      ,MAX(TS.ITEM_NAME) ITEM_NAME
                      ,MAX(TS.ITEM_COLOR_ID) ITEM_COLOR_ID
                      ,MAX(MC.ITEM_COLOR_NAME) ITEM_COLOR_NAME
                      ,MAX(TS.ITEM_SIZE_ID) ITEM_SIZE_ID
                      ,MAX(MIS.ITEM_SIZE_NAME) ITEM_SIZE_NAME
                      ,MAX(TS.JAN) JAN
                      ,SUM(TS.INSTRUCT_QTY) INSTRUCT_QTY
                      ,SUM(TS.ALLOC_QTY) ALLOC_QTY
                  FROM T_SHIPS TS
                  LEFT JOIN T_ALLOC_INFO TAI
                    ON TS.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TS.CENTER_ID = TAI.CENTER_ID
                   AND TS.BATCH_NO = TAI.ALLOC_NO
                  LEFT JOIN M_ITEM_SKU MIS
                    ON TS.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                  LEFT JOIN M_DIVISIONS MD
                    ON MIS.SHIPPER_ID = MD.SHIPPER_ID
                   AND MIS.DIVISION_ID = MD.DIVISION_ID
                  LEFT JOIN (SELECT SHIPPER_ID
                                   ,CATEGORY_ID1
                                   ,MAX(CATEGORY_NAME1) CATEGORY_NAME1
                               FROM M_ITEM_CATEGORIES4
                              GROUP BY SHIPPER_ID,CATEGORY_ID1) MIC1
                    ON MIS.SHIPPER_ID   = MIC1.SHIPPER_ID
                   AND MIS.CATEGORY_ID1 = MIC1.CATEGORY_ID1
                  LEFT JOIN M_COLORS MC
                    ON TS.SHIPPER_ID   = MC.SHIPPER_ID
                   AND TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN M_SIZES MS
                    ON TS.SHIPPER_ID   = MS.SHIPPER_ID
                   AND TS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                 WHERE TS.SHIP_KIND = 2
                   AND TS.ALLOC_FLAG = 1
                   AND TS.ALLOC_ERR_FLAG = 1
                   AND TS.CENTER_ID = :CENTER_ID
                   AND TS.SHIPPER_ID = :SHIPPER_ID
                 GROUP BY TS.BATCH_NO,MIS.CATEGORY_ID1,TS.ITEM_SKU_ID,TS.CENTER_ID,TS.SHIPPER_ID
                 ORDER BY TS.BATCH_NO,MIS.CATEGORY_ID1,TS.ITEM_SKU_ID ");
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<DcAllocationItemReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、引当エラー(注文別)のデータを作る。</returns>
        public IEnumerable<DcAllocationShipInstructReport> DcAllocationShipInstructListing(DcAllocationSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT TS.CENTER_ID
                      ,TS.SHIP_TO_STORE_ID
                      ,VSTS.SHIP_TO_STORE_NAME1 STORE_NAME1
                      ,TS.BATCH_NO
                      ,TAI.BATCH_NAME
                      ,MIS.CATEGORY_ID1
                      ,MIC1.CATEGORY_NAME1
                      ,TS.ITEM_ID
                      ,TS.ITEM_NAME
                      ,TS.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TS.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,TS.JAN
                      ,TS.SHIP_INSTRUCT_ID
                      ,TS.SHIP_INSTRUCT_SEQ
                      ,TS.INSTRUCT_QTY
                      ,TS.ALLOC_QTY
                  FROM T_SHIPS TS
                  LEFT JOIN V_SHIP_TO_STORES VSTS
                    ON VSTS.SHIPPER_ID = TS.SHIPPER_ID
                   AND VSTS.SHIP_TO_STORE_ID = TS.SHIP_TO_STORE_ID
                  LEFT JOIN T_ALLOC_INFO TAI
                    ON TS.SHIPPER_ID = TAI.SHIPPER_ID
                   AND TS.CENTER_ID = TAI.CENTER_ID
                   AND TS.BATCH_NO = TAI.ALLOC_NO
                  LEFT JOIN M_ITEM_SKU MIS
                    ON TS.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                  LEFT JOIN M_DIVISIONS MD
                    ON MIS.SHIPPER_ID = MD.SHIPPER_ID
                   AND MIS.DIVISION_ID = MD.DIVISION_ID
                  LEFT JOIN (SELECT SHIPPER_ID
                                   ,CATEGORY_ID1
                                   ,MAX(CATEGORY_NAME1) CATEGORY_NAME1
                               FROM M_ITEM_CATEGORIES4
                              GROUP BY SHIPPER_ID,CATEGORY_ID1) MIC1
                    ON MIS.SHIPPER_ID   = MIC1.SHIPPER_ID
                   AND MIS.CATEGORY_ID1 = MIC1.CATEGORY_ID1
                  LEFT JOIN M_COLORS MC
                    ON TS.SHIPPER_ID   = MC.SHIPPER_ID
                   AND TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN M_SIZES MS
                    ON TS.SHIPPER_ID   = MS.SHIPPER_ID
                   AND TS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                 WHERE TS.SHIP_KIND = 2
                   AND TS.ALLOC_FLAG = 1
                   AND TS.ALLOC_ERR_FLAG = 1
                   AND TS.CENTER_ID = :CENTER_ID
                   AND TS.SHIPPER_ID = :SHIPPER_ID
                 ORDER BY TS.SHIP_TO_STORE_ID,TS.BATCH_NO,MIS.CATEGORY_ID1,TS.SHIP_INSTRUCT_ID,TS.SHIP_INSTRUCT_SEQ ");
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<DcAllocationShipInstructReport>(query.ToString(), parameters);
        }
    }
}