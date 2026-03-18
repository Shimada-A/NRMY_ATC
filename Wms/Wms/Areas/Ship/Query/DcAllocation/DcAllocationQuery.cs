namespace Wms.Areas.Ship.Query.DcAllocation
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.ServiceModel.Security;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Dapper;
    using Microsoft.Ajax.Utilities;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Models;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.DcAllocation;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.DcAllocation.DcAllocationSearchConditions;

    public class DcAllocationQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpDcAllocation(DcAllocationSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;

                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;

                    // SKU一覧
                    if (condition.ResultType == Ship.ViewModels.DcAllocation.ResultTypes.Sku)
                    {
                        query = new StringBuilder(@"
                        INSERT INTO WW_SHP_DC_ALLOCATION(
                               MAKE_DATE
                              ,MAKE_USER_ID
                              ,MAKE_PROGRAM_NAME
                              ,UPDATE_DATE
                              ,UPDATE_USER_ID
                              ,UPDATE_PROGRAM_NAME
                              ,UPDATE_COUNT
                              ,SHIPPER_ID
                              ,SEQ
                              ,LINE_NO
                              ,IS_CHECK
                              ,CENTER_ID
                              ,CENTER_NAME
                              ,SHIP_PLAN_DATE
                              ,EMERGENCY_CLASS_NAME
                              ,SHIP_INSTRUCT_ID
                              ,SHIP_INSTRUCT_SEQ
                              ,ITEM_ID
                              ,ITEM_NAME
                              ,ITEM_COLOR_ID
                              ,ITEM_COLOR_NAME
                              ,ITEM_SIZE_ID
                              ,ITEM_SIZE_NAME
                              ,JAN
                              ,SHIP_TO_STORE_ID
                              ,SHIP_TO_STORE_NAME
                              ,SHIP_TO_STORE_CLASS_NAME
                              ,TRANSPORTER_NAME
                              ,SHIP_TO_QTY
                              ,INSTRUCT_QTY
                              ,ALLOC_DATE
                              ,ITEM_SKU_ID
                              ,INSTRUCT_CLASS_NAME
                              ,SALES_CLASS
                              ,SALES_CLASS_NAME
                              ,OFF_RATE
                              ,BRAND_ID
                            )
                        SELECT MAKE_DATE
                              ,MAKE_USER_ID
                              ,MAKE_PROGRAM_NAME
                              ,UPDATE_DATE
                              ,UPDATE_USER_ID
                              ,UPDATE_PROGRAM_NAME
                              ,UPDATE_COUNT
                              ,SHIPPER_ID
                              ,SEQ
                              ,ROWNUM LINE_NO
                              ,IS_CHECK
                              ,CENTER_ID
                              ,CENTER_NAME
                              ,SHIP_PLAN_DATE
                              ,EMERGENCY_CLASS_NAME
                              ,SHIP_INSTRUCT_ID
                              ,SHIP_INSTRUCT_SEQ
                              ,ITEM_ID
                              ,ITEM_NAME
                              ,ITEM_COLOR_ID
                              ,ITEM_COLOR_NAME
                              ,ITEM_SIZE_ID
                              ,ITEM_SIZE_NAME
                              ,JAN
                              ,SHIP_TO_STORE_ID
                              ,SHIP_TO_STORE_NAME
                              ,SHIP_TO_STORE_CLASS_NAME
                              ,TRANSPORTER_NAME
                              ,SHIP_TO_QTY
                              ,INSTRUCT_QTY
                              ,ALLOC_DATE
                              ,ITEM_SKU_ID
                              ,INSTRUCT_CLASS_NAME
                              ,SALES_CLASS
                              ,SALES_CLASS_NAME
                              ,OFF_RATE
                              ,BRAND_ID
                        FROM(
                        SELECT
                             " + "SYSTIMESTAMP " + " AS MAKE_DATE" +
                             ", '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                             ",'DcAllocation'" + " AS MAKE_PROGRAM_NAME" +
                             ",SYSTIMESTAMP " + "AS UPDATE_DATE" +
                             ", '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                             ",'DcAllocation'" + " AS UPDATE_PROGRAM_NAME" +
                             ",0" + " AS UPDATE_COUNT" +
                             "," + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                             "," + condition.Seq + " AS SEQ");
                        query.Append(@"
                              ,0 AS IS_CHECK
                              ,TS.CENTER_ID
                              ,NULL AS CENTER_NAME
                              ,MAX(TS.SHIP_PLAN_DATE) SHIP_PLAN_DATE
                              ,MAX(MG.GEN_NAME) EMERGENCY_CLASS_NAME
                              ,TS.SHIP_INSTRUCT_ID
                              ,0 AS SHIP_INSTRUCT_SEQ
                              ,MAX(TS.ITEM_ID) ITEM_ID
                              ,MAX(TS.ITEM_NAME) ITEM_NAME
                              ,MAX(TS.ITEM_COLOR_ID) ITEM_COLOR_ID
                              ,MAX(MC.ITEM_COLOR_NAME) ITEM_COLOR_NAME
                              ,MAX(TS.ITEM_SIZE_ID) ITEM_SIZE_ID
                              ,MAX(MIS.ITEM_SIZE_NAME) ITEM_SIZE_NAME
                              ,MAX(TS.JAN) JAN
                              ,NULL AS SHIP_TO_STORE_ID
                              ,NULL AS SHIP_TO_STORE_NAME
                              ,NULL AS SHIP_TO_STORE_CLASS_NAME
                              ,NULL AS TRANSPORTER_NAME
                              ,COUNT(DISTINCT(TS.SHIP_TO_STORE_ID)) SHIP_TO_QTY
                              ,SUM(TS.INSTRUCT_QTY) INSTRUCT_QTY
                              ,MAX(TS.MAKE_DATE) ALLOC_DATE
                              ,TS.ITEM_SKU_ID
                              ,MAX(GEN1.GEN_NAME) INSTRUCT_CLASS_NAME
                              ,MAX(TS.SALES_CLASS) SALES_CLASS
                              ,MAX(GEN2.GEN_NAME) SALES_CLASS_NAME
                              ,MAX(TS.OFF_RATE) OFF_RATE
                              ,MAX(MIS.BRAND_ID) BRAND_ID
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
                          LEFT JOIN M_BRANDS MB
                            ON MIS.SHIPPER_ID   = MB.SHIPPER_ID
                           AND MIS.BRAND_ID = MB.BRAND_ID
                          LEFT JOIN M_VENDORS MV
                            ON MIS.SHIPPER_ID   = MV.SHIPPER_ID
                           AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                          LEFT JOIN M_GENERALS GEN1
                            ON GEN1.SHIPPER_ID = TS.SHIPPER_ID
                           AND GEN1.CENTER_ID = '@@@'
                           AND GEN1.GEN_DIV_CD = 'INSTRUCT_CLASS'
                           AND GEN1.REGISTER_DIVI_CD = '1'
                           AND GEN1.GEN_CD = TO_CHAR(TS.INSTRUCT_CLASS)
                          LEFT JOIN M_GENERALS GEN2
                            ON GEN2.SHIPPER_ID = TS.SHIPPER_ID
                           AND GEN2.CENTER_ID = '@@@'
                           AND GEN2.GEN_DIV_CD = 'SALE_CLASS'
                           AND GEN2.REGISTER_DIVI_CD = '1'
                           AND GEN2.GEN_CD = TO_CHAR(TS.SALES_CLASS)
                         WHERE TS.SHIP_KIND = 2
                           AND TS.ALLOC_FLAG = 0
                           AND TS.DELI_SHIWAKE_CD <> ' '
                           AND TS.STOP_FLAG = 0
                           AND TS.SHIPPER_ID = :SHIPPER_ID
                           AND TS.INSTRUCT_CLASS = :INSTRUCT_CLASS
                         ");
                        parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                        parameters.Add(":INSTRUCT_CLASS", condition.InstructKbn);

                        // Add search condition
                        // センター
                        if (!string.IsNullOrEmpty(condition.CenterId))
                        {
                            query.Append(" AND TS.CENTER_ID = :CENTER_ID ");
                            parameters.Add(":CENTER_ID", condition.CenterId);
                        }

                        // 受信日時(From-To)
                        if (condition.AllocDateFrom != null)
                        {
                            query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') >= :ALLOC_DATE_TIME_FROM ");
                            parameters.Add(":ALLOC_DATE_TIME_FROM", condition.AllocTimeFrom == null ?
                                                                    string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " 00:00:00" :
                                                                    string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " " + condition.AllocTimeFrom);
                        }

                        if (condition.AllocDateTo != null)
                        {
                            query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') <= :ALLOC_DATE_TIME_TO ");
                            parameters.Add(":ALLOC_DATE_TIME_TO", condition.AllocTimeTo == null ?
                                                                    string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " 23:59:59" :
                                                                    string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " " + condition.AllocTimeTo);
                        }

                        // 出荷予定日(From-To)
                        if (condition.ShipPlanDateFrom != null)
                        {
                            query.Append(" AND TS.SHIP_PLAN_DATE >= :SHIP_PLAN_DATE_FROM ");
                            parameters.Add(":SHIP_PLAN_DATE_FROM", condition.ShipPlanDateFrom);
                        }

                        if (condition.ShipPlanDateTo != null)
                        {
                            query.Append(" AND TS.SHIP_PLAN_DATE <= :SHIP_PLAN_DATE_TO ");
                            parameters.Add(":SHIP_PLAN_DATE_TO", condition.ShipPlanDateTo);
                        }

                        // 納品日
                        if (condition.DeliveryDate != null)
                        {
                            query.Append(" AND TS.DELIVERY_DATE = :DELIVERY_DATE ");
                            parameters.Add(":DELIVERY_DATE", condition.DeliveryDate);
                        }

                        // 出荷先区分
                        if (!string.IsNullOrEmpty(condition.ShipToClass))
                        {
                            query.Append(" AND TS.STORE_CLASS = :STORE_CLASS ");
                            parameters.Add(":STORE_CLASS", condition.ShipToClass);
                        }

                        // 出荷先
                        if (!string.IsNullOrEmpty(condition.ShipToId))
                        {
                            string[] shipToIds = (condition.ShipToId ?? condition.ShipToId).Split(',');
                            query.Append(" AND TS.SHIP_TO_STORE_ID IN :SHIP_TO_STORE_ID ");
                            parameters.Add(":SHIP_TO_STORE_ID", shipToIds);
                        }

                        // 配送業者
                        if (!string.IsNullOrEmpty(condition.TransporterId))
                        {
                            string[] transporterIds = (condition.TransporterId ?? condition.TransporterId).Split(',');
                            query.Append(" AND TS.TRANSPORTER_ID IN :TRANSPORTER_ID ");
                            parameters.Add(":TRANSPORTER_ID", transporterIds);
                        }

                        // 配送エリア
                        if (!string.IsNullOrEmpty(condition.TransporterArea))
                        {
                            string[] transporterAreas = (condition.TransporterArea ?? condition.TransporterArea).Split(',');
                            query.Append(" AND TS.PREF_ID IN :PREF_ID ");
                            parameters.Add(":PREF_ID", transporterAreas);
                        }

                        // 事業部
                        if (!string.IsNullOrEmpty(condition.DivisionId))
                        {
                            query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                            parameters.Add(":DIVISION_ID", condition.DivisionId);
                        }

                        // ブランド
                        if (!string.IsNullOrEmpty(condition.BrandId))
                        {
                            query.Append(" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                            parameters.Add(":BRAND_ID", condition.BrandId + "%");
                        }

                        // ブランド名
                        if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
                        {
                            query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                            parameters.Add(":BRAND_NAME", condition.BrandName + "%");
                        }

                        // 代表仕入先
                        if (!string.IsNullOrEmpty(condition.MainVendorId))
                        {
                            query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :MAIN_VENDOR_ID ");
                            parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId + "%");
                        }

                        // 代表仕入先名
                        if (string.IsNullOrEmpty(condition.MainVendorId) && !string.IsNullOrEmpty(condition.MainVendorName))
                        {
                            query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                            parameters.Add(":VENDOR_NAME1", condition.MainVendorName + "%");
                        }

                        // 分類
                        if (!string.IsNullOrEmpty(condition.CategoryId1))
                        {
                            query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                            parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId2))
                        {
                            query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                            parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId3))
                        {
                            query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                            parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId4))
                        {
                            query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                            parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                        }

                        // アイテムコード
                        if (!string.IsNullOrEmpty(condition.ItemCode))
                        {
                            query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                            parameters.Add(":ITEM_CODE", condition.ItemCode);
                        }

                        // 品番
                        if (!string.IsNullOrEmpty(condition.ItemId))
                        {
                            query.Append(" AND TS.ITEM_ID LIKE :ITEM_ID ");
                            parameters.Add(":ITEM_ID", condition.ItemId + "%");
                        }

                        // JAN
                        if (!string.IsNullOrEmpty(condition.Jan))
                        {
                            query.Append(" AND TS.JAN LIKE :JAN ");
                            parameters.Add(":JAN", condition.Jan + "%");
                        }

                        // SKU
                        if (!string.IsNullOrEmpty(condition.ItemSkuId))
                        {
                            query.Append(" AND TS.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                            parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                        }

                        // 出荷指示ID(From-To)
                        if (!string.IsNullOrEmpty(condition.ShipInstructIdFrom))
                        {
                            query.Append(" AND TS.SHIP_INSTRUCT_ID >= :SHIP_INSTRUCT_ID_FROM ");
                            parameters.Add(":SHIP_INSTRUCT_ID_FROM", condition.ShipInstructIdFrom);
                        }

                        if (!string.IsNullOrEmpty(condition.ShipInstructIdTo))
                        {
                            query.Append(" AND TS.SHIP_INSTRUCT_ID <= :SHIP_INSTRUCT_ID_TO ");
                            parameters.Add(":SHIP_INSTRUCT_ID_TO", condition.ShipInstructIdTo);
                        }

                        // 緊急
                        if (!string.IsNullOrEmpty(condition.EmergencyClass))
                        {
                            query.Append(" AND TS.EMERGENCY_CLASS = :EMERGENCY_CLASS ");
                            parameters.Add(":EMERGENCY_CLASS", condition.EmergencyClass);
                        }

                        // 売上区分
                        if (!string.IsNullOrEmpty(condition.SalesClass))
                        {
                            query.Append(" AND TS.SALES_CLASS = :SALES_CLASS ");
                            parameters.Add(":SALES_CLASS", condition.SalesClass);
                        }

                        query.Append(@" GROUP BY TS.SHIP_INSTRUCT_ID,TS.ITEM_SKU_ID,TS.CENTER_ID,TS.SHIPPER_ID ) ");

                    }// 指示明細
                    else
                    {
                        query = new StringBuilder(@"
                        INSERT INTO WW_SHP_DC_ALLOCATION(
                               MAKE_DATE
                              ,MAKE_USER_ID
                              ,MAKE_PROGRAM_NAME
                              ,UPDATE_DATE
                              ,UPDATE_USER_ID
                              ,UPDATE_PROGRAM_NAME
                              ,UPDATE_COUNT
                              ,SHIPPER_ID
                              ,SEQ
                              ,LINE_NO
                              ,IS_CHECK
                              ,CENTER_ID
                              ,CENTER_NAME
                              ,SHIP_PLAN_DATE
                              ,EMERGENCY_CLASS_NAME
                              ,SHIP_INSTRUCT_ID
                              ,SHIP_INSTRUCT_SEQ
                              ,ITEM_ID
                              ,ITEM_NAME
                              ,ITEM_COLOR_ID
                              ,ITEM_COLOR_NAME
                              ,ITEM_SIZE_ID
                              ,ITEM_SIZE_NAME
                              ,JAN
                              ,SHIP_TO_STORE_ID
                              ,SHIP_TO_STORE_NAME
                              ,SHIP_TO_STORE_CLASS_NAME
                              ,TRANSPORTER_NAME
                              ,SHIP_TO_QTY
                              ,INSTRUCT_QTY
                              ,ALLOC_DATE
                              ,ITEM_SKU_ID
                              ,INSTRUCT_CLASS_NAME
                              ,SALES_CLASS
                              ,SALES_CLASS_NAME
                              ,OFF_RATE
                              ,VIA_CENTER_ID
                              ,VIA_CENTER_NAME
                              ,BRAND_ID
                            )
                        SELECT MAKE_DATE
                              ,MAKE_USER_ID
                              ,MAKE_PROGRAM_NAME
                              ,UPDATE_DATE
                              ,UPDATE_USER_ID
                              ,UPDATE_PROGRAM_NAME
                              ,UPDATE_COUNT
                              ,SHIPPER_ID
                              ,SEQ
                              ,ROWNUM LINE_NO
                              ,IS_CHECK
                              ,CENTER_ID
                              ,CENTER_NAME
                              ,SHIP_PLAN_DATE
                              ,EMERGENCY_CLASS_NAME
                              ,SHIP_INSTRUCT_ID
                              ,SHIP_INSTRUCT_SEQ
                              ,ITEM_ID
                              ,ITEM_NAME
                              ,ITEM_COLOR_ID
                              ,ITEM_COLOR_NAME
                              ,ITEM_SIZE_ID
                              ,ITEM_SIZE_NAME
                              ,JAN
                              ,SHIP_TO_STORE_ID
                              ,SHIP_TO_STORE_NAME
                              ,SHIP_TO_STORE_CLASS_NAME
                              ,TRANSPORTER_NAME
                              ,SHIP_TO_QTY
                              ,INSTRUCT_QTY
                              ,ALLOC_DATE
                              ,ITEM_SKU_ID
                              ,INSTRUCT_CLASS_NAME
                              ,SALES_CLASS
                              ,SALES_CLASS_NAME
                              ,OFF_RATE
                              ,VIA_CENTER_ID
                              ,VIA_CENTER_NAME
                              ,BRAND_ID
                        FROM(
                        SELECT
                             " + "SYSTIMESTAMP " + " AS MAKE_DATE" +
                             ", '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                             ",'DcAllocation'" + " AS MAKE_PROGRAM_NAME" +
                             ",SYSTIMESTAMP " + "AS UPDATE_DATE" +
                             ", '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                             ",'DcAllocation'" + " AS UPDATE_PROGRAM_NAME" +
                             ",0" + " AS UPDATE_COUNT" +
                             "," + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                             "," + condition.Seq + " AS SEQ");
                        query.Append(@"
                              ,0 AS IS_CHECK
                              ,TS.CENTER_ID
                              ,NULL AS CENTER_NAME
                              ,TS.SHIP_PLAN_DATE
                              ,MG.GEN_NAME EMERGENCY_CLASS_NAME
                              ,TS.SHIP_INSTRUCT_ID
                              ,TS.SHIP_INSTRUCT_SEQ
                              ,TS.ITEM_ID
                              ,TS.ITEM_NAME
                              ,TS.ITEM_COLOR_ID
                              ,MC.ITEM_COLOR_NAME
                              ,TS.ITEM_SIZE_ID
                              ,MIS.ITEM_SIZE_NAME
                              ,NULL AS JAN
                              ,TS.SHIP_TO_STORE_ID
                              ,VSTS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                              ,MG1.GEN_NAME SHIP_TO_STORE_CLASS_NAME
                              ,MT.TRANSPORTER_NAME
                              ,NULL AS SHIP_TO_QTY
                              ,TS.INSTRUCT_QTY
                              ,NULL AS ALLOC_DATE
                              ,TS.ITEM_SKU_ID
                              ,GEN1.GEN_NAME INSTRUCT_CLASS_NAME
                              ,TS.SALES_CLASS
                              ,GEN2.GEN_NAME SALES_CLASS_NAME
                              ,TS.OFF_RATE
                              ,MST.CONTROL_CENTER_ID VIA_CENTER_ID
                              ,MCE.CENTER_NAME1 VIA_CENTER_NAME
                              ,MIS.BRAND_ID
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
                          LEFT JOIN V_SHIP_TO_STORES VSTS
                            ON VSTS.SHIPPER_ID = TS.SHIPPER_ID
                           AND VSTS.SHIP_TO_STORE_ID = TS.SHIP_TO_STORE_ID
                          LEFT JOIN M_GENERALS MG1
                            ON TS.SHIPPER_ID = MG1.SHIPPER_ID
                           AND MG1.REGISTER_DIVI_CD = '1'
                           AND MG1.CENTER_ID = '@@@'
                           AND MG1.GEN_DIV_CD = 'STORE_CLASS'
                           AND TS.STORE_CLASS = MG1.GEN_CD
                          LEFT JOIN M_TRANSPORTERS MT
                            ON TS.SHIPPER_ID   = MT.SHIPPER_ID
                           AND TS.TRANSPORTER_ID = MT.TRANSPORTER_ID
                          LEFT JOIN M_BRANDS MB
                            ON MIS.SHIPPER_ID   = MB.SHIPPER_ID
                           AND MIS.BRAND_ID = MB.BRAND_ID
                          LEFT JOIN M_VENDORS MV
                            ON MIS.SHIPPER_ID   = MV.SHIPPER_ID
                           AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                          LEFT JOIN M_GENERALS GEN1
                            ON GEN1.SHIPPER_ID = TS.SHIPPER_ID
                           AND GEN1.CENTER_ID = '@@@'
                           AND GEN1.GEN_DIV_CD = 'INSTRUCT_CLASS'
                           AND GEN1.REGISTER_DIVI_CD = '1'
                           AND GEN1.GEN_CD = TO_CHAR(TS.INSTRUCT_CLASS)
                          LEFT JOIN M_GENERALS GEN2
                            ON GEN2.SHIPPER_ID = TS.SHIPPER_ID
                           AND GEN2.CENTER_ID = '@@@'
                           AND GEN2.GEN_DIV_CD = 'SALE_CLASS'
                           AND GEN2.REGISTER_DIVI_CD = '1'
                           AND GEN2.GEN_CD = TO_CHAR(TS.SALES_CLASS)
                          LEFT JOIN M_STORES MST
                            ON MST.SHIPPER_ID = TS.SHIPPER_ID
                           AND MST.STORE_ID = TS.SHIP_TO_STORE_ID
                          LEFT JOIN M_CENTERS MCE
                            ON MCE.SHIPPER_ID = TS.SHIPPER_ID
                           AND MCE.CENTER_ID = MST.CONTROL_CENTER_ID
                         WHERE TS.SHIP_KIND = 2
                           AND TS.ALLOC_FLAG = 0
                           AND TS.DELI_SHIWAKE_CD <> ' '
                           AND TS.STOP_FLAG = 0
                           AND TS.SHIPPER_ID = :SHIPPER_ID
                           AND TS.INSTRUCT_CLASS = :INSTRUCT_CLASS
                         ");
                        parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                        parameters.Add(":INSTRUCT_CLASS", condition.InstructKbn);

                        // Add search condition
                        // センター
                        if (!string.IsNullOrEmpty(condition.CenterId))
                        {
                            query.Append(" AND TS.CENTER_ID = :CENTER_ID ");
                            parameters.Add(":CENTER_ID", condition.CenterId);
                        }

                        // 受信日時(From-To)
                        if (condition.AllocDateFrom != null)
                        {
                            query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') >= :ALLOC_DATE_TIME_FROM ");
                            parameters.Add(":ALLOC_DATE_TIME_FROM", condition.AllocTimeFrom == null ?
                                                                    string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " 00:00:00" :
                                                                    string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " " + condition.AllocTimeFrom);
                        }

                        if (condition.AllocDateTo != null)
                        {
                            query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') <= :ALLOC_DATE_TIME_TO ");
                            parameters.Add(":ALLOC_DATE_TIME_TO", condition.AllocTimeTo == null ?
                                                                    string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " 23:59:59" :
                                                                    string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " " + condition.AllocTimeTo);
                        }

                        // 出荷予定日(From-To)
                        if (condition.ShipPlanDateFrom != null)
                        {
                            query.Append(" AND TS.SHIP_PLAN_DATE >= :SHIP_PLAN_DATE_FROM ");
                            parameters.Add(":SHIP_PLAN_DATE_FROM", condition.ShipPlanDateFrom);
                        }

                        if (condition.ShipPlanDateTo != null)
                        {
                            query.Append(" AND TS.SHIP_PLAN_DATE <= :SHIP_PLAN_DATE_TO ");
                            parameters.Add(":SHIP_PLAN_DATE_TO", condition.ShipPlanDateTo);
                        }

                        // 納品日
                        if (condition.DeliveryDate != null)
                        {
                            query.Append(" AND TS.DELIVERY_DATE = :DELIVERY_DATE ");
                            parameters.Add(":DELIVERY_DATE", condition.DeliveryDate);
                        }

                        // 出荷先区分
                        if (!string.IsNullOrEmpty(condition.ShipToClass))
                        {
                            query.Append(" AND TS.STORE_CLASS = :STORE_CLASS ");
                            parameters.Add(":STORE_CLASS", condition.ShipToClass);
                        }

                        // 出荷先
                        if (!string.IsNullOrEmpty(condition.ShipToId))
                        {
                            string[] shipToIds = (condition.ShipToId ?? condition.ShipToId).Split(',');
                            query.Append(" AND TS.SHIP_TO_STORE_ID IN :SHIP_TO_STORE_ID ");
                            parameters.Add(":SHIP_TO_STORE_ID", shipToIds);
                        }

                        // 配送業者
                        if (!string.IsNullOrEmpty(condition.TransporterId))
                        {
                            string[] transporterIds = (condition.TransporterId ?? condition.TransporterId).Split(',');
                            query.Append(" AND TS.TRANSPORTER_ID IN :TRANSPORTER_ID ");
                            parameters.Add(":TRANSPORTER_ID", transporterIds);
                        }

                        // 配送エリア
                        if (!string.IsNullOrEmpty(condition.TransporterArea))
                        {
                            string[] transporterAreas = (condition.TransporterArea ?? condition.TransporterArea).Split(',');
                            query.Append(" AND TS.PREF_ID IN :PREF_ID ");
                            parameters.Add(":PREF_ID", transporterAreas);
                        }

                        // 事業部
                        if (!string.IsNullOrEmpty(condition.DivisionId))
                        {
                            query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                            parameters.Add(":DIVISION_ID", condition.DivisionId);
                        }

                        // ブランド
                        if (!string.IsNullOrEmpty(condition.BrandId))
                        {
                            query.Append(" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                            parameters.Add(":BRAND_ID", condition.BrandId + "%");
                        }

                        // ブランド名
                        if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
                        {
                            query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                            parameters.Add(":BRAND_NAME", condition.BrandName + "%");
                        }

                        // 代表仕入先
                        if (!string.IsNullOrEmpty(condition.MainVendorId))
                        {
                            query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :MAIN_VENDOR_ID ");
                            parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId + "%");
                        }

                        // 代表仕入先名
                        if (string.IsNullOrEmpty(condition.MainVendorId) && !string.IsNullOrEmpty(condition.MainVendorName))
                        {
                            query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                            parameters.Add(":VENDOR_NAME1", condition.MainVendorName + "%");
                        }

                        // 分類
                        if (!string.IsNullOrEmpty(condition.CategoryId1))
                        {
                            query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                            parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId2))
                        {
                            query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                            parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId3))
                        {
                            query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                            parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId4))
                        {
                            query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                            parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                        }

                        // アイテムコード
                        if (!string.IsNullOrEmpty(condition.ItemCode))
                        {
                            query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                            parameters.Add(":ITEM_CODE", condition.ItemCode);
                        }

                        // 品番
                        if (!string.IsNullOrEmpty(condition.ItemId))
                        {
                            query.Append(" AND TS.ITEM_ID LIKE :ITEM_ID ");
                            parameters.Add(":ITEM_ID", condition.ItemId + "%");
                        }

                        // JAN
                        if (!string.IsNullOrEmpty(condition.Jan))
                        {
                            query.Append(" AND TS.JAN LIKE :JAN ");
                            parameters.Add(":JAN", condition.Jan + "%");
                        }

                        // SKU
                        if (!string.IsNullOrEmpty(condition.ItemSkuId))
                        {
                            query.Append(" AND TS.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                            parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                        }

                        // 出荷指示ID(From-To)
                        if (!string.IsNullOrEmpty(condition.ShipInstructIdFrom))
                        {
                            query.Append(" AND TS.SHIP_INSTRUCT_ID >= :SHIP_INSTRUCT_ID_FROM ");
                            parameters.Add(":SHIP_INSTRUCT_ID_FROM", condition.ShipInstructIdFrom);
                        }

                        if (!string.IsNullOrEmpty(condition.ShipInstructIdTo))
                        {
                            query.Append(" AND TS.SHIP_INSTRUCT_ID <= :SHIP_INSTRUCT_ID_TO ");
                            parameters.Add(":SHIP_INSTRUCT_ID_TO", condition.ShipInstructIdTo);
                        }

                        // 緊急
                        if (!string.IsNullOrEmpty(condition.EmergencyClass))
                        {
                            query.Append(" AND TS.EMERGENCY_CLASS = :EMERGENCY_CLASS ");
                            parameters.Add(":EMERGENCY_CLASS", condition.EmergencyClass);
                        }

                        // 売上区分
                        if (!string.IsNullOrEmpty(condition.SalesClass))
                        {
                            query.Append(" AND TS.SALES_CLASS = :SALES_CLASS ");
                            parameters.Add(":SALES_CLASS", condition.SalesClass);
                        }
                        query.Append(@" ) ");
                    }
                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
                return true;
            }
        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpDcAllocationDeatil(DcAllocationSearchConditions condition)
        {
            var keys = MvcDbContext.Current.ShpDcAllocations.Where(x => x.IsCheck && x.Seq == condition.Seq).Select(x => new { x.ShipperId, x.CenterId, x.ShipInstructId, x.ItemSkuId }).ToList();

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
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
                      ,VSTS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                      ,MG1.GEN_NAME SHIP_TO_STORE_CLASS_NAME
                      ,MT.TRANSPORTER_NAME 
                      ,TS.INSTRUCT_QTY
                      ,TS.ITEM_SKU_ID
                      ,TS.CENTER_ID
                      ,GEN1.GEN_NAME INSTRUCT_CLASS_NAME
                      ,TS.SALES_CLASS
                      ,GEN2.GEN_NAME SALES_CLASS_NAME
                      ,TS.OFF_RATE
                      ,MST.CONTROL_CENTER_ID VIA_CENTER_ID
                      ,MCE.CENTER_NAME1 VIA_CENTER_NAME
                      ,MIS.BRAND_ID
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
                  LEFT JOIN V_SHIP_TO_STORES VSTS
                    ON VSTS.SHIPPER_ID = TS.SHIPPER_ID
                   AND VSTS.SHIP_TO_STORE_ID = TS.SHIP_TO_STORE_ID
                  LEFT JOIN M_GENERALS MG1
                    ON TS.SHIPPER_ID = MG1.SHIPPER_ID
                   AND MG1.REGISTER_DIVI_CD = '1'
                   AND MG1.CENTER_ID = '@@@'
                   AND MG1.GEN_DIV_CD = 'STORE_CLASS'
                   AND TS.STORE_CLASS = MG1.GEN_CD
                  LEFT JOIN M_TRANSPORTERS MT
                    ON TS.SHIPPER_ID   = MT.SHIPPER_ID
                   AND TS.TRANSPORTER_ID = MT.TRANSPORTER_ID
                  LEFT JOIN M_BRANDS MB
                    ON MIS.SHIPPER_ID   = MB.SHIPPER_ID
                   AND MIS.BRAND_ID = MB.BRAND_ID
                  LEFT JOIN M_VENDORS MV
                    ON MIS.SHIPPER_ID   = MV.SHIPPER_ID
                   AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                  LEFT JOIN M_GENERALS GEN1
                    ON GEN1.SHIPPER_ID = TS.SHIPPER_ID
                   AND GEN1.CENTER_ID = '@@@'
                   AND GEN1.GEN_DIV_CD = 'INSTRUCT_CLASS'
                   AND GEN1.REGISTER_DIVI_CD = '1'
                   AND GEN1.GEN_CD = TO_CHAR(TS.INSTRUCT_CLASS)
                  LEFT JOIN M_GENERALS GEN2
                    ON GEN2.SHIPPER_ID = TS.SHIPPER_ID
                   AND GEN2.CENTER_ID = '@@@'
                   AND GEN2.GEN_DIV_CD = 'SALE_CLASS'
                   AND GEN2.REGISTER_DIVI_CD = '1'
                   AND GEN2.GEN_CD = TO_CHAR(TS.SALES_CLASS)
                  LEFT JOIN M_STORES MST
                    ON MST.SHIPPER_ID = TS.SHIPPER_ID
                   AND MST.STORE_ID = TS.SHIP_TO_STORE_ID
                  LEFT JOIN M_CENTERS MCE
                    ON MCE.SHIPPER_ID = TS.SHIPPER_ID
                   AND MCE.CENTER_ID = MST.CONTROL_CENTER_ID
                 WHERE TS.SHIP_KIND = 2
                   AND TS.ALLOC_FLAG = 0
                   AND TS.DELI_SHIWAKE_CD <> ' '
                   AND TS.STOP_FLAG = 0
                   AND TS.SHIPPER_ID = :SHIPPER_ID
                   AND TS.INSTRUCT_CLASS = :INSTRUCT_CLASS
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":INSTRUCT_CLASS", condition.InstructKbn);

            // Add search condition
            // センター
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND TS.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            // 受信日時(From-To)
            if (condition.AllocDateFrom != null)
            {
                query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') >= :ALLOC_DATE_TIME_FROM ");
                parameters.Add(":ALLOC_DATE_TIME_FROM", condition.AllocTimeFrom == null ?
                                                        string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " 00:00:00" :
                                                        string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " " + condition.AllocTimeFrom);
            }

            if (condition.AllocDateTo != null)
            {
                query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') <= :ALLOC_DATE_TIME_TO ");
                parameters.Add(":ALLOC_DATE_TIME_TO", condition.AllocTimeTo == null ?
                                                        string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " 23:59:59" :
                                                        string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " " + condition.AllocTimeTo);
            }

            // 出荷予定日(From-To)
            if (condition.ShipPlanDateFrom != null)
            {
                query.Append(" AND TS.SHIP_PLAN_DATE >= :SHIP_PLAN_DATE_FROM ");
                parameters.Add(":SHIP_PLAN_DATE_FROM", condition.ShipPlanDateFrom);
            }

            if (condition.ShipPlanDateTo != null)
            {
                query.Append(" AND TS.SHIP_PLAN_DATE <= :SHIP_PLAN_DATE_TO ");
                parameters.Add(":SHIP_PLAN_DATE_TO", condition.ShipPlanDateTo);
            }

            // 納品日
            if (condition.DeliveryDate != null)
            {
                query.Append(" AND TS.DELIVERY_DATE = :DELIVERY_DATE ");
                parameters.Add(":DELIVERY_DATE", condition.DeliveryDate);
            }

            // 出荷先区分
            if (!string.IsNullOrEmpty(condition.ShipToClass))
            {
                query.Append(" AND TS.STORE_CLASS = :STORE_CLASS ");
                parameters.Add(":STORE_CLASS", condition.ShipToClass);
            }

            // 出荷先
            if (!string.IsNullOrEmpty(condition.ShipToId))
            {
                string[] shipToIds = (condition.ShipToId ?? condition.ShipToId).Split(',');
                query.Append(" AND TS.SHIP_TO_STORE_ID IN :SHIP_TO_STORE_ID ");
                parameters.Add(":SHIP_TO_STORE_ID", shipToIds);
            }

            // 配送業者
            if (!string.IsNullOrEmpty(condition.TransporterId))
            {
                string[] transporterIds = (condition.TransporterId ?? condition.TransporterId).Split(',');
                query.Append(" AND TS.TRANSPORTER_ID IN :TRANSPORTER_ID ");
                parameters.Add(":TRANSPORTER_ID", transporterIds);
            }

            // 配送エリア
            if (!string.IsNullOrEmpty(condition.TransporterArea))
            {
                string[] transporterAreas = (condition.TransporterArea ?? condition.TransporterArea).Split(',');
                query.Append(" AND TS.PREF_ID IN :PREF_ID ");
                parameters.Add(":PREF_ID", transporterAreas);
            }

            // 事業部
            if (!string.IsNullOrEmpty(condition.DivisionId))
            {
                query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                parameters.Add(":DIVISION_ID", condition.DivisionId);
            }

            // ブランド
            if (!string.IsNullOrEmpty(condition.BrandId))
            {
                query.Append(" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                parameters.Add(":BRAND_ID", condition.BrandId + "%");
            }

            // ブランド名
            if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
            {
                query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                parameters.Add(":BRAND_NAME", condition.BrandName + "%");
            }

            // 代表仕入先
            if (!string.IsNullOrEmpty(condition.MainVendorId))
            {
                query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :MAIN_VENDOR_ID ");
                parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId + "%");
            }

            // 代表仕入先名
            if (string.IsNullOrEmpty(condition.MainVendorId) && !string.IsNullOrEmpty(condition.MainVendorName))
            {
                query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                parameters.Add(":VENDOR_NAME1", condition.MainVendorName + "%");
            }

            // 分類
            if (!string.IsNullOrEmpty(condition.CategoryId1))
            {
                query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId2))
            {
                query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId3))
            {
                query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId4))
            {
                query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
            }

            // アイテムコード
            if (!string.IsNullOrEmpty(condition.ItemCode))
            {
                query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                parameters.Add(":ITEM_CODE", condition.ItemCode);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND TS.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND TS.JAN LIKE :JAN ");
                parameters.Add(":JAN", condition.Jan + "%");
            }

            // SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND TS.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
            }

            // 出荷指示ID(From-To)
            if (!string.IsNullOrEmpty(condition.ShipInstructIdFrom))
            {
                query.Append(" AND TS.SHIP_INSTRUCT_ID >= :SHIP_INSTRUCT_ID_FROM ");
                parameters.Add(":SHIP_INSTRUCT_ID_FROM", condition.ShipInstructIdFrom);
            }
            if (!string.IsNullOrEmpty(condition.ShipInstructIdTo))
            {
                query.Append(" AND TS.SHIP_INSTRUCT_ID <= :SHIP_INSTRUCT_ID_TO ");
                parameters.Add(":SHIP_INSTRUCT_ID_TO", condition.ShipInstructIdTo);
            }

            // 緊急
            if (!string.IsNullOrEmpty(condition.EmergencyClass))
            {
                query.Append(" AND TS.EMERGENCY_CLASS = :EMERGENCY_CLASS ");
                parameters.Add(":EMERGENCY_CLASS", condition.EmergencyClass);
            }

            // 売上区分
            if (!string.IsNullOrEmpty(condition.SalesClass))
            {
                query.Append(" AND TS.SALES_CLASS = :SALES_CLASS ");
                parameters.Add(":SALES_CLASS", condition.SalesClass);
            }

            // 1.ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            // 2.検索・ワーク作成
            var result = MvcDbContext.Current.Database.Connection.Query<ShpDcAllocation>(query.ToString(), parameters);
            var shpDcAllocationDetails = from row in result
                          where keys.Select(x => x.CenterId + x.ShipInstructId + x.ItemSkuId).Contains(row.CenterId + row.ShipInstructId + row.ItemSkuId)
                          select row;
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var row in shpDcAllocationDetails.Select((v, i) => new { v, i }))
                {
                    var shpDcAllocation = new ShpDcAllocation();
                    shpDcAllocation.SetBaseInfoInsert();
                    shpDcAllocation.Seq = condition.Seq;
                    shpDcAllocation.LineNo = row.i + 1;
                    shpDcAllocation.IsCheck = false;
                    shpDcAllocation.CenterId = row.v.CenterId;
                    shpDcAllocation.ShipPlanDate = row.v.ShipPlanDate;
                    shpDcAllocation.EmergencyClassName = row.v.EmergencyClassName;
                    shpDcAllocation.ShipInstructId = row.v.ShipInstructId;
                    shpDcAllocation.ShipInstructSeq = row.v.ShipInstructSeq;
                    shpDcAllocation.ItemId = row.v.ItemId;
                    shpDcAllocation.ItemName = row.v.ItemName;
                    shpDcAllocation.ItemColorId = row.v.ItemColorId;
                    shpDcAllocation.ItemColorName = row.v.ItemColorName;
                    shpDcAllocation.ItemSizeId = row.v.ItemSizeId;
                    shpDcAllocation.ItemSizeName = row.v.ItemSizeName;
                    shpDcAllocation.ShipToStoreId = row.v.ShipToStoreId;
                    shpDcAllocation.ShipToStoreName = row.v.ShipToStoreName;
                    shpDcAllocation.ShipToStoreClassName = row.v.ShipToStoreClassName;
                    shpDcAllocation.TransporterName = row.v.TransporterName;
                    shpDcAllocation.InstructQty = row.v.InstructQty;
                    shpDcAllocation.ItemSkuId = row.v.ItemSkuId;
                    shpDcAllocation.InstructClassName = row.v.InstructClassName;
                    shpDcAllocation.SalesClassName = row.v.SalesClassName;
                    shpDcAllocation.OffRate = row.v.OffRate;
                    MvcDbContext.Current.ShpDcAllocations.Add(shpDcAllocation);
                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                    {
                        return false;
                    }
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<DcAllocationResultRow> GetData(DcAllocationSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            // SKU一覧
            if (condition.ResultType == Ship.ViewModels.DcAllocation.ResultTypes.Sku)
            {
                StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_DC_ALLOCATION
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<DcAllocationResultRow>(query.ToString(), parameters).Count();

                // Sort function
                switch (condition.SortKey)
                {
                    case DcAllocationSortKey.SkuInstructId:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID DESC,SHIP_INSTRUCT_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID ASC,SHIP_INSTRUCT_ID ASC ");
                                break;
                        }

                        break;

                    default:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE DESC,SHIP_INSTRUCT_ID DESC,ITEM_SKU_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE ASC,SHIP_INSTRUCT_ID ASC,ITEM_SKU_ID ASC ");
                                break;
                        }

                        break;
                }

                query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
                parameters.Add(":PAGE_SIZE", condition.PageSize);

                // Choose data corresponding on each page
                parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

                // Fill data to memory
                var dcAllocations = MvcDbContext.Current.Database.Connection.Query<DcAllocationResultRow>(query.ToString(), parameters);
                var shpDcAllocations = MvcDbContext.Current.ShpDcAllocations.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
                condition.InstructIdSum = shpDcAllocations.Select(x => x.ShipInstructId).Distinct().Count();
                condition.ItemSkuSum = shpDcAllocations.Select(x => x.ItemSkuId).Distinct().Count();
                condition.PlanQtySum = shpDcAllocations.Select(x => x.InstructQty).Sum();
                condition.SelectedCnt = MvcDbContext.Current.ShpDcAllocations.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

                // Excute paging
                return new StaticPagedList<DcAllocationResultRow>(dcAllocations, condition.Page, condition.PageSize, totalCount);
            }

            // 指示明細
            else
            {
                StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_DC_ALLOCATION
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<DcAllocationResultRow>(query.ToString(), parameters).Count();

                // Sort function
                switch (condition.DetailSortKey)
                {
                    case DcAllocationDetailSortKey.SkuInstructIdDetailId:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID DESC,SHIP_INSTRUCT_ID DESC,SHIP_INSTRUCT_SEQ DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ITEM_SKU_ID ASC,SHIP_INSTRUCT_ID ASC,SHIP_INSTRUCT_SEQ ASC ");
                                break;
                        }

                        break;

                    case DcAllocationDetailSortKey.ShipIdSkuInstructIdDetailId:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY SHIP_TO_STORE_ID DESC,ITEM_SKU_ID DESC,SHIP_INSTRUCT_ID DESC,SHIP_INSTRUCT_SEQ DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY SHIP_TO_STORE_ID ASC,ITEM_SKU_ID ASC,SHIP_INSTRUCT_ID ASC,SHIP_INSTRUCT_SEQ ASC ");
                                break;
                        }

                        break;

                    default:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE DESC,SHIP_INSTRUCT_ID DESC,ITEM_SKU_ID DESC,SHIP_INSTRUCT_SEQ DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY SHIP_PLAN_DATE ASC,SHIP_INSTRUCT_ID ASC,ITEM_SKU_ID ASC,SHIP_INSTRUCT_SEQ ASC ");
                                break;
                        }

                        break;
                }

                query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
                parameters.Add(":PAGE_SIZE", condition.PageSize);

                // Choose data corresponding on each page
                parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

                // Fill data to memory
                var dcAllocations = MvcDbContext.Current.Database.Connection.Query<DcAllocationResultRow>(query.ToString(), parameters);
                var shpDcAllocations = MvcDbContext.Current.ShpDcAllocations.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
                condition.ItemSkuSum = shpDcAllocations.Select(x => x.ItemSkuId).Distinct().Count();
                condition.DetailSum = shpDcAllocations.Count();
                condition.PlanQtySum = shpDcAllocations.Select(x => x.InstructQty).Sum();
                condition.SelectedCnt = MvcDbContext.Current.ShpDcAllocations.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

                // Excute paging
                return new StaticPagedList<DcAllocationResultRow>(dcAllocations, condition.Page, condition.PageSize, totalCount);
            }
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool UpdateShpDcAllocation(IList<SelectedDcAllocationViewModel> ShpDcAllocations)
        {
            var sql = $@"
                UPDATE WW_SHP_DC_ALLOCATION
                SET
                        UPDATE_DATE = SYSDATE
                    ,   UPDATE_USER_ID = :USER_ID
                    ,   UPDATE_PROGRAM_NAME = :PROGRAM_NAME
                    ,   UPDATE_COUNT = UPDATE_COUNT + 1
                    ,   IS_CHECK = :IS_CHECK
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND SEQ = :SEQ
                    AND LINE_NO = :LINE_NO
            ";

            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in ShpDcAllocations)
                {
                    try
                    {
                        MvcDbContext.Current.Database.Connection.Execute(sql,
                            new
                            {
                                USER_ID = Profile.User.UserId,
                                PROGRAM_NAME = BaseModel.GetProgramId(),
                                IS_CHECK = u.IsCheck ? 1 : 0,
                                SHIPPER_ID = Profile.User.ShipperId,
                                SEQ = u.Seq,
                                LINE_NO = u.LineNo
                            });
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        trans.Rollback();
                        return false;
                    }
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool ShpDcAllocationAllChange(DcAllocationSearchConditions conditions, bool check)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;
                    query = new StringBuilder(@"
                        UPDATE  WW_SHP_DC_ALLOCATION
                        SET
                                UPDATE_DATE = :UPDATE_DATE
                            ,   UPDATE_USER_ID = :UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME = 'DcAllocation'
                            ,   UPDATE_COUNT = UPDATE_COUNT + 1
                            ,   IS_CHECK = :IS_CHECK
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                    ");
                    parameters.Add(":UPDATE_DATE", DateTimeOffset.Now);
                    parameters.Add(":UPDATE_USER_ID", Profile.User.UserId);
                    parameters.Add(":IS_CHECK", (check == true) ? 1 : 0 );
                    parameters.Add(":SEQ", conditions.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }

            conditions.SelectedCnt = MvcDbContext.Current.ShpDcAllocations
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void AllocationUpdate(DcAllocationSearchConditions searchConditions)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BATCH_NAME", searchConditions.AllocationName, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_LIST_TYPE", searchConditions.ResultType, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_SLIP_DATE", searchConditions.SlipDate, DbType.Date, ParameterDirection.Input);
            param.Add("IN_VIA_CENTER_FLAG", searchConditions.ChkViaCenter ? 1 : 0, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_PICK_KIND", (int)searchConditions.PickKind, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_CHECKED_STOCKOUT", searchConditions.CheckedStockoutFlag, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_PICKED_FLAG", (searchConditions.PickKind == PickClass.Total && searchConditions.IsTotalPicked) ? 1 : 0, DbType.Int32, ParameterDirection.Input);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_DCAllOC_MAIN",
                param,
                commandType: CommandType.StoredProcedure);
        }


        ///// <summary>
        ///// 実績更新
        ///// </summary>
        //public static async Task<OutProcedureModel> AllocationUpdateAsync(CurrentBagForCpuTask CurrentBagForCpuTask, DcAllocationSearchConditions searchConditions)
        //{

        //    var param = new DynamicParameters();
        //    param.Add("IN_SHIPPER_ID", CurrentBagForCpuTask.ShipperId, DbType.String, ParameterDirection.Input);
        //    param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
        //    param.Add("IN_USER_ID", CurrentBagForCpuTask.UserId, DbType.String, ParameterDirection.Input);
        //    param.Add("IN_BATCH_NAME", searchConditions.AllocationName, DbType.String, ParameterDirection.Input);
        //    param.Add("IN_WORK_ID", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
        //    param.Add("IN_LIST_TYPE", searchConditions.ResultType, DbType.Int32, ParameterDirection.Input);

        //    await CurrentBagForCpuTask.CurrentMvcDbContext.Database.Connection.ExecuteAsync(
        //         "SP_W_SHP_DCAllOC_MAIN",
        //         param,
        //         commandType: CommandType.StoredProcedure).ConfigureAwait(true);

        //    var outProcedureModel = new OutProcedureModel();

        //    return outProcedureModel;
        //}

        /// <summary>
        /// 伝票日付入力チェック
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AllocationSlipDateCheck(DcAllocationSearchConditions condition, out string message)
        {
            DynamicParameters parameters0 = new DynamicParameters();
            StringBuilder query0;
            query0 = new StringBuilder(@"
                SELECT
                    CLOSED_MONTH
                FROM
                    M_SHIPPERS
                WHERE
                    SHIPPER_ID = :SHIPPER_ID
                AND LAST_DAY(TO_DATE(CLOSED_MONTH, 'YYYYMM')) < :SLIP_DATE
            ");
            parameters0.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters0.Add(":SLIP_DATE", condition.SlipDate);
            var result0 = MvcDbContext.Current.Database.Connection.Query<DcAllocationSearchConditions>(query0.ToString(), parameters0);
            var result = result0
                                .Select(m => new
                                {
                                    m.SlipDate,
                                    m.ClosedMonth
                                })
                                .SingleOrDefault();

            if (result == null)
            {
                message = DcAllocationResource.SlipDateMessage;
                return false;
            }

            message = "";
            return true;
        }

        /// <summary>
        /// 出荷レーン間口マスタチェック
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AllocationFrontageStoreCheck(DcAllocationSearchConditions condition, out string message)
        {
            var shpDcAllocations = MvcDbContext.Current.ShpDcAllocations.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == condition.Seq && x.IsCheck).ToList();
            foreach (var cModel in shpDcAllocations.Select((v, i) => new { v, i }))
            {
                if (condition.DcAllocations.Where(x => x.Seq == cModel.v.Seq && x.LineNo == cModel.v.LineNo && x.IsCheck).Count() == 1)
                {
                    if (!MvcDbContext.Current.ShipFrontages.Where(x => x.ShipperId == cModel.v.ShipperId
                                                                 && x.CenterId == cModel.v.CenterId
                                                                 && x.StoreId == cModel.v.ShipToStoreId
                                                                 && x.BrandId == cModel.v.BrandId).Any())
                    {
                        message = string.Format(DcAllocationResource.FrontageStoreIdMessage, cModel.v.BrandId, cModel.v.ShipToStoreId);
                        return false;
                    }
                }
            }
            message = "";
            return true;
        }

        /// <summary>
        /// オフ率混在チェック
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool AllocationOffRaateCheck(DcAllocationSearchConditions conditions, out string message)
        {
            var records = MvcDbContext.Current.ShpDcAllocations.Where(x => x.ShipperId == Profile.User.ShipperId && x.Seq == conditions.Seq && x.IsCheck).DistinctBy(x => x.OffRate);

            if (records.Count() > 1)
            {
                message = DcAllocationResource.OffRateMixMessage;
                return false;
            }

            message = "";
            return true;
        }

        /// <summary>
        /// 引当実行時入力チェック
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool AllocationInputCheck(DcAllocationSearchConditions condition, out string message)
        {
            var checkMessage = "";

            // 伝票日付チェック
            if (!AllocationSlipDateCheck(condition, out checkMessage))
            {
                message = checkMessage;
                return false;
            }
            
            //if (condition.PickKind == PickClass.Total)
            //{
            //    // レーン間口マスタの登録店舗チェック
            //    if (!AllocationFrontageStoreCheck(condition, out checkMessage))
            //    {
            //        message = checkMessage;
            //        return false;
            //    }
            //}

            //オフ率混在チェック
            if (!AllocationOffRaateCheck(condition, out checkMessage))
            {
                message = checkMessage;
                return false;
            }

            message = "";
            return true;
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListDivisions()
        {
            return MvcDbContext.Current.Divisions
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.DivisionId,
                    Text = m.DivisionId + "：" + m.DivisionName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListTransporters()
        {
            return MvcDbContext.Current.Transporters
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.TransporterId.ToString(),
                    Text = m.TransporterName
                })
                .OrderBy(m => m.Text);
        }

        /// <summary>
        /// 分類1データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys1()
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId )
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId1.ToString(),
                    Text = m.CategoryId1.ToString() + "：" + m.CategoryName1
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類2データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys2(string categoryId1 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2.ToString(),
                    Text = m.CategoryId2.ToString() + "：" + m.CategoryName2
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類3データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys3(string categoryId1 = "", string categoryId2 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId3.ToString(),
                    Text = m.CategoryId3.ToString() + "：" + m.CategoryName3
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類4データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys4(string categoryId1 = "", string categoryId2 = "", string categoryId3 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2)
                && (categoryId3 == null ? 1 == 1 : m.CategoryId3 == categoryId3))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId4.ToString(),
                    Text = m.CategoryId4.ToString() + "：" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// アイテムコードデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return MvcDbContext.Current.Items
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.ItemCode,
                    Text = m.ItemCode + "：" + m.ItemCodeName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 引当進捗ワークデータ取得
        /// </summary>
        /// <returns>引当進捗ワークデータ</returns>
        public AllocStatus GetAllocStatus(long Seq)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@" select * from W_ALLOC_STATUS where SHIPPER_ID = :SHIPPER_ID and SEQ = :SEQ
                ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":SEQ", Seq);
            var allocStatus = MvcDbContext.Current.Database.Connection.Query<AllocStatus>(query.ToString(), parameters).FirstOrDefault();
            return allocStatus;
        }
    }
}