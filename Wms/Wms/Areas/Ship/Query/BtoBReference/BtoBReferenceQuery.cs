namespace Wms.Areas.Ship.Query.BtoBReference
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
    using Wms.Areas.Ship.ViewModels.BtoBReference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.BtoBReference.BtoBReference01SearchConditions;
    using static Wms.Areas.Ship.ViewModels.BtoBReference.BtoBReference02SearchConditions;

    public class BtoBReferenceQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpBtoBReference01(BtoBReference01SearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;

                    // サブSQL生成
                    string packingSQL;
                    // B
                    if (string.IsNullOrWhiteSpace(condition.ShipBoxStatus) && condition.ShipBoxStatusOld)
                    {
                        packingSQL = @"
                                    SELECT
                                            ITEM_SKU_ID
                                        ,   RESULT_QTY
                                        ,   BOX_NO
                                        ,   SHIP_TO_STORE_CLASS
                                        ,   SHIP_TO_STORE_ID
                                        ,   BATCH_NO
                                        ,   IF_STATE_ERP
                                        ,   KAKU_FLAG
                                        ,   NOUHIN_PRN_FLAG
                                        ,   DELI_PRN_FLAG
                                        ,   CASE_KAKU_FLAG
                                        ,   KAKU_DATE
                                        ,   SHIPPER_ID
                                        ,   CENTER_ID
                                        ,   SHIP_INSTRUCT_ID
                                        ,   SHIP_INSTRUCT_SEQ
                                        ,   TRANSPORTER_ID
                                        ,   ITEM_ID
                                        ,   JAN
                                        ,   DELI_NO
                                        ,   DELI_NO2
                                        ,   NOUHIN_NO
                                        ,   0 AS PAST_FLAG  --累積フラグ
                                    FROM
                                            T_SHIP_PACKING_INFO
                                    WHERE
                                            CENTER_ID = :CENTER_ID
                                        AND SHIPPER_ID = :SHIPPER_ID
                                        AND EC_FLAG = 0

                                    UNION ALL

                                    SELECT
                                            ITEM_SKU_ID
                                        ,   RESULT_QTY
                                        ,   BOX_NO
                                        ,   SHIP_TO_STORE_CLASS
                                        ,   SHIP_TO_STORE_ID
                                        ,   BATCH_NO
                                        ,   IF_STATE_ERP
                                        ,   KAKU_FLAG
                                        ,   NOUHIN_PRN_FLAG
                                        ,   DELI_PRN_FLAG
                                        ,   CASE_KAKU_FLAG
                                        ,   KAKU_DATE
                                        ,   SHIPPER_ID
                                        ,   CENTER_ID
                                        ,   SHIP_INSTRUCT_ID
                                        ,   SHIP_INSTRUCT_SEQ
                                        ,   TRANSPORTER_ID
                                        ,   ITEM_ID
                                        ,   JAN 
                                        ,   DELI_NO
                                        ,   DELI_NO2
                                        ,   NOUHIN_NO
                                        ,   1 AS PAST_FLAG  --累積フラグ
                                    FROM
                                            A_SHIP_PACKING_INFO
                                    WHERE
                                            CENTER_ID = :CENTER_ID
                                        AND SHIPPER_ID = :SHIPPER_ID
                                        AND EC_FLAG = 0
                        ";
                    }
                    // C
                    else if (condition.ShipBoxStatus == "5" && !condition.ShipBoxStatusOld)
                    {
                        packingSQL = @"
                                    SELECT
                                            ITEM_SKU_ID
                                        ,   RESULT_QTY
                                        ,   BOX_NO
                                        ,   SHIP_TO_STORE_CLASS
                                        ,   SHIP_TO_STORE_ID
                                        ,   BATCH_NO
                                        ,   IF_STATE_ERP
                                        ,   KAKU_FLAG
                                        ,   NOUHIN_PRN_FLAG
                                        ,   DELI_PRN_FLAG
                                        ,   CASE_KAKU_FLAG
                                        ,   KAKU_DATE
                                        ,   SHIPPER_ID
                                        ,   CENTER_ID
                                        ,   SHIP_INSTRUCT_ID
                                        ,   SHIP_INSTRUCT_SEQ
                                        ,   TRANSPORTER_ID
                                        ,   ITEM_ID
                                        ,   JAN 
                                        ,   DELI_NO
                                        ,   DELI_NO2
                                        ,   NOUHIN_NO
                                        ,   1 AS PAST_FLAG  --累積フラグ
                                    FROM
                                            A_SHIP_PACKING_INFO
                                    WHERE
                                            CENTER_ID = :CENTER_ID
                                        AND SHIPPER_ID = :SHIPPER_ID
                                        AND EC_FLAG = 0
                        ";
                    }
                    // A
                    else
                    {
                        packingSQL = @"
                                SELECT
                                        ITEM_SKU_ID
                                    ,   RESULT_QTY
                                    ,   BOX_NO
                                    ,   SHIP_TO_STORE_CLASS
                                    ,   SHIP_TO_STORE_ID
                                    ,   BATCH_NO
                                    ,   IF_STATE_ERP
                                    ,   KAKU_FLAG
                                    ,   NOUHIN_PRN_FLAG
                                    ,   DELI_PRN_FLAG
                                    ,   CASE_KAKU_FLAG
                                    ,   KAKU_DATE
                                    ,   SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   TRANSPORTER_ID
                                    ,   ITEM_ID
                                    ,   JAN
                                    ,   DELI_NO
                                    ,   DELI_NO2
                                    ,   NOUHIN_NO
                                    ,   0 AS PAST_FLAG  --累積フラグ
                                FROM
                                        T_SHIP_PACKING_INFO
                                WHERE
                                        CENTER_ID = :CENTER_ID
                                    AND SHIPPER_ID = :SHIPPER_ID
                                    AND EC_FLAG = 0
                        ";
                    }

                    string shipSQL;
                    shipSQL = @"
                                SELECT
                                        SHIP_PLAN_DATE
                                    ,   SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   MAKE_DATE
                                    ,   INSTRUCT_CLASS
                                FROM
                                        T_SHIPS
                                WHERE
                                        CENTER_ID = :CENTER_ID
                                    AND SHIPPER_ID = :SHIPPER_ID
                    ";
                    // B // C
                    if ((string.IsNullOrWhiteSpace(condition.ShipBoxStatus) && condition.ShipBoxStatusOld) || (condition.ShipBoxStatus == "5" && !condition.ShipBoxStatusOld))
                    {
                        shipSQL += @"
                                UNION ALL

                                SELECT
                                        SHIP_PLAN_DATE
                                    ,   SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SHIP_INSTRUCT_ID
                                    ,   SHIP_INSTRUCT_SEQ
                                    ,   MAKE_DATE
                                    ,   INSTRUCT_CLASS
                                FROM
                                        A_SHIPS
                                WHERE
                                        CENTER_ID = :CENTER_ID
                                    AND SHIPPER_ID = :SHIPPER_ID
                        ";
                    }

                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder($@"
                        INSERT INTO WW_SHP_BTO_B_REFERENCE01 (
                                MAKE_DATE
                            ,   MAKE_USER_ID 
                            ,   MAKE_PROGRAM_NAME
                            ,   UPDATE_DATE 
                            ,   UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME
                            ,   UPDATE_COUNT
                            ,   SHIPPER_ID
                            ,   SEQ
                            ,   LINE_NO
                            ,   IS_CHECK
                            ,   BOX_NO
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_TO_STORE_NAME
                            ,   TRANSPORTER_NAME
                            ,   RESULT_QTY
                            ,   BATCH_NO
                            ,   SHIP_PLAN_DATE
                            ,   SHIP_STATUS_NAME
                            ,   KAKU_DATE
                            ,   ITEM_SKU_SUM
                            ,   DETAIL_SUM
                            ,   RESULT_QTY_SUM
                            ,   CENTER_ID
                        )
                        WITH
                                SELECTED_SHIP_PACKING AS ({packingSQL})
                            ,   SELECTED_SHIP_DATA AS ({shipSQL})
                        SELECT
                                SYSTIMESTAMP AS MAKE_DATE
                            ,   '{Common.Profile.User.UserId}' AS MAKE_USER_ID 
                            ,   '{nameof(BtoBReference)}' AS MAKE_PROGRAM_NAME
                            ,   SYSTIMESTAMP AS UPDATE_DATE 
                            ,   '{Common.Profile.User.UserId}' AS UPDATE_USER_ID
                            ,   '{nameof(BtoBReference)}' AS UPDATE_PROGRAM_NAME
                            ,   0 AS UPDATE_COUNT
                            ,   TSPI.SHIPPER_ID
                            ,   {condition.Seq} AS SEQ
                            ,   ROW_NUMBER() OVER(ORDER BY TSPI.BOX_NO, TSPI.SHIP_TO_STORE_ID, TSPI.CENTER_ID, TSPI.SHIPPER_ID) AS LINE_NO
                            ,   0 AS IS_CHECK
                            ,   TSPI.BOX_NO
                            ,   TSPI.SHIP_TO_STORE_ID
                            ,   MAX(TSPI.SHIP_TO_STORE_NAME1) SHIP_TO_STORE_NAME
                            ,   MAX(TSPI.TRANSPORTER_NAME) TRANSPORTER_NAME
                            ,   SUM(TSPI.RESULT_QTY) RESULT_QTY
                            ,   MAX(TSPI.BATCH_NO) || (CASE WHEN COUNT(DISTINCT(TSPI.BATCH_NO)) > 1 THEN '　{BtoBReferenceResource.Other}' ELSE '' END) BATCH_NO
                            ,   TO_CHAR(MAX(TSPI.SHIP_PLAN_DATE),'yyyy/MM/dd') || (CASE WHEN COUNT(DISTINCT(TSPI.SHIP_PLAN_DATE)) > 1 THEN '　{BtoBReferenceResource.Other}' ELSE '' END) SHIP_PLAN_DATE
                            ,   CASE
                                    WHEN MAX(TSPI.PAST_FLAG) = 1 THEN '{BtoBReferenceResource.Daily}'
                                    WHEN MAX(TSPI.IF_STATE_ERP) = 2 THEN '{BtoBReferenceResource.Send}'
                                    WHEN MAX(TSPI.KAKU_FLAG) = 1 THEN '{BtoBReferenceResource.Confirm}'
                                    WHEN MAX(TSPI.NOUHIN_PRN_FLAG) = 9 THEN '{BtoBReferenceResource.Print}'
                                    WHEN MAX(TSPI.NOUHIN_PRN_FLAG) = 1 THEN '{BtoBReferenceResource.RePrint}'
                                    WHEN MAX(TSPI.NOUHIN_PRN_FLAG) = 8 AND (MAX(TSPI.DELI_PRN_FLAG) = 1 OR MAX(TSPI.DELI_PRN_FLAG) = 2) THEN '{BtoBReferenceResource.InvoicePrint}'
                                    WHEN MAX(TSPI.CASE_KAKU_FLAG) = 1 THEN '{BtoBReferenceResource.CaseKaku}'
                                    ELSE '{BtoBReferenceResource.Shipping}'
                                END AS SHIP_STATUS_NAME
                            ,   MAX(TSPI.KAKU_DATE) KAKU_DATE
                            ,   MAX(TSPI.ITEM_SKU_SUM) AS ITEM_SKU_SUM
                            ,   MAX(TSPI.DETAIL_SUM) AS DETAIL_SUM
                            ,   MAX(TSPI.RESULT_QTY_SUM) AS RESULT_QTY_SUM
                            ,   TSPI.CENTER_ID
                        FROM (
                            SELECT
                                    TSPI.SHIPPER_ID
                                ,   TSPI.CENTER_ID
                                ,   TSPI.BOX_NO
                                ,   TSPI.SHIP_TO_STORE_ID
                                ,   TSPI.RESULT_QTY
                                ,   TSPI.BATCH_NO
                                ,   TSPI.PAST_FLAG
                                ,   TSPI.IF_STATE_ERP
                                ,   TSPI.KAKU_FLAG
                                ,   TSPI.NOUHIN_PRN_FLAG
                                ,   TSPI.DELI_PRN_FLAG
                                ,   TSPI.CASE_KAKU_FLAG
                                ,   TSPI.KAKU_DATE
                                ,   TS.SHIP_PLAN_DATE
                                ,   VSTS.SHIP_TO_STORE_NAME1
                                ,   MT.TRANSPORTER_NAME
                                ,   COUNT(DISTINCT(TSPI.ITEM_SKU_ID)) OVER () AS ITEM_SKU_SUM
                                ,   COUNT(DISTINCT(TSPI.SHIP_INSTRUCT_ID || TSPI.SHIP_INSTRUCT_SEQ)) OVER () AS DETAIL_SUM
                                ,   SUM(TSPI.RESULT_QTY) OVER () AS RESULT_QTY_SUM
                            FROM
                                    SELECTED_SHIP_PACKING TSPI
                            LEFT JOIN
                                    SELECTED_SHIP_DATA TS
                            ON
                                    TSPI.SHIPPER_ID = TS.SHIPPER_ID
                                AND TSPI.CENTER_ID = TS.CENTER_ID
                                AND TSPI.SHIP_INSTRUCT_ID = TS.SHIP_INSTRUCT_ID
                                AND TSPI.SHIP_INSTRUCT_SEQ = TS.SHIP_INSTRUCT_SEQ
                            LEFT JOIN
                                    M_ITEM_SKU MIS
                            ON
                                    TSPI.SHIPPER_ID = MIS.SHIPPER_ID
                                AND TSPI.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                            LEFT JOIN
                                    M_BRANDS MB
                            ON
                                    MIS.SHIPPER_ID   = MB.SHIPPER_ID
                                AND MIS.BRAND_ID = MB.BRAND_ID
                            LEFT JOIN
                                    M_VENDORS MV
                            ON
                                    MIS.SHIPPER_ID   = MV.SHIPPER_ID
                                AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                            LEFT JOIN
                                    V_SHIP_TO_STORES VSTS
                            ON 
                                    TSPI.SHIPPER_ID = VSTS.SHIPPER_ID
                                AND TSPI.SHIP_TO_STORE_ID  = VSTS.SHIP_TO_STORE_ID
                            LEFT JOIN
                                    M_TRANSPORTERS MT
                            ON
                                    TSPI.SHIPPER_ID   = MT.SHIPPER_ID
                                AND TSPI.TRANSPORTER_ID = MT.TRANSPORTER_ID
                            WHERE
                                    TSPI.SHIPPER_ID = :SHIPPER_ID
                                AND TSPI.CENTER_ID = :CENTER_ID
                    ");
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);

                    // 受信日時(From-To)
                    if (condition.MakeDateFrom != null)
                    {
                        query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') >= :MAKE_DATE_FROM ");
                        parameters.Add(":MAKE_DATE_FROM", condition.MakeTimeFrom == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateFrom) + " 00:00:00" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateFrom) + " " + condition.MakeTimeFrom);
                    }

                    if (condition.MakeDateTo != null)
                    {
                        query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') <= :MAKE_DATE_TO ");
                        parameters.Add(":MAKE_DATE_TO", condition.MakeTimeTo == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateTo) + " 23:59:59" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.MakeDateTo) + " " + condition.MakeTimeTo);
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

                    // 出荷確定日(From-To)
                    if (condition.KakuDateFrom != null)
                    {
                        query.Append(" AND TSPI.KAKU_DATE >= :KAKU_DATE_FROM ");
                        parameters.Add(":KAKU_DATE_FROM", condition.KakuDateFrom);
                    }

                    if (condition.KakuDateTo != null)
                    {
                        query.Append(" AND TSPI.KAKU_DATE <= :KAKU_DATE_TO ");
                        parameters.Add(":KAKU_DATE_TO", condition.KakuDateTo);
                    }

                    // 状態
                    if (condition.ShipBoxStatus == "5") //日次済
                    {
                        query.Append(@" AND TSPI.PAST_FLAG = 1 ");
                    }
                    else if (condition.ShipBoxStatus == "4") //送信済
                    {
                        query.Append(" AND TSPI.IF_STATE_ERP = 2 AND TSPI.PAST_FLAG = 0 ");
                    }
                    else if (condition.ShipBoxStatus == "3") //確定済
                    {
                        query.Append(" AND TSPI.KAKU_FLAG = 1 AND TSPI.PAST_FLAG = 0 AND TSPI.IF_STATE_ERP <> 2 ");
                    }
                    else if (condition.ShipBoxStatus == "2") //納品書発行済
                    {
                        query.Append(@" AND TSPI.KAKU_FLAG <> 1 AND TSPI.PAST_FLAG = 0 AND TSPI.IF_STATE_ERP <> 2
                                        AND ((TSPI.NOUHIN_PRN_FLAG = 8 AND TSPI.DELI_PRN_FLAG <> 0) OR TSPI.NOUHIN_PRN_FLAG = 9) ");
                    }
                    else if (condition.ShipBoxStatus == "6") //納品書再発行待ち
                    {
                        query.Append(@" AND TSPI.KAKU_FLAG <> 1
                                        AND TSPI.IF_STATE_ERP <> 2
                                        AND TSPI.NOUHIN_PRN_FLAG = 1 ");
                    }
                    else if (condition.ShipBoxStatus == "1") //出荷中
                    {
                        query.Append(@" AND TSPI.PAST_FLAG = 0
                                        AND TSPI.IF_STATE_ERP <> 2
                                        AND TSPI.KAKU_FLAG <> 1
                                        AND (   TSPI.NOUHIN_PRN_FLAG = 0
                                            OR  (TSPI.NOUHIN_PRN_FLAG = 8 AND TSPI.DELI_PRN_FLAG = 0) ) ");
                    }
                    else if (condition.ShipBoxStatus == "7") //カートン確定済
                    {
                        query.Append(@" AND TSPI.CASE_KAKU_FLAG = 1 ");
                        
                    }

                    // 指示区分
                    if (!string.IsNullOrEmpty(condition.InstructClass))
                    {
                        query.Append(" AND TS.INSTRUCT_CLASS = :INSTRUCT_CLASS ");
                        parameters.Add(":INSTRUCT_CLASS", condition.InstructClass);
                    }

                    // アイテム
                    if (!string.IsNullOrEmpty(condition.ItemCode))
                    {
                        query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                        parameters.Add(":ITEM_CODE", condition.ItemCode);
                    }

                    // 出荷先区分
                    if (!string.IsNullOrEmpty(condition.StoreClass))
                    {
                        query.Append(" AND TSPI.SHIP_TO_STORE_CLASS = :SHIP_TO_STORE_CLASS ");
                        parameters.Add(":SHIP_TO_STORE_CLASS", condition.StoreClass);
                    }

                    // 出荷先
                    if (!string.IsNullOrEmpty(condition.ShipToStoreId))
                    {
                        query.Append(" AND TSPI.SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID ");
                        parameters.Add(":SHIP_TO_STORE_ID", condition.ShipToStoreId);
                    }

                    // 配送業者
                    if (!string.IsNullOrEmpty(condition.TransporterId))
                    {
                        query.Append(" AND TSPI.TRANSPORTER_ID = :TRANSPORTER_ID ");
                        parameters.Add(":TRANSPORTER_ID", condition.TransporterId);
                    }

                    // 出荷指示ID
                    if (!string.IsNullOrEmpty(condition.ShipInstructId))
                    {
                        query.Append(" AND TSPI.SHIP_INSTRUCT_ID LIKE :SHIP_INSTRUCT_ID ");
                        parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId + "%");
                    }

                    // バッチNo
                    if (!string.IsNullOrEmpty(condition.BatchNo))
                    {
                        query.Append(" AND TSPI.BATCH_NO = :BATCH_NO ");
                        parameters.Add(":BATCH_NO", condition.BatchNo);
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

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.Append(" AND TSPI.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.Append(" AND TSPI.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    // SKU
                    if (!string.IsNullOrEmpty(condition.ItemSkuId))
                    {
                        query.Append(" AND TSPI.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                        parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                    }

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo))
                    {
                        query.Append(" AND TSPI.BOX_NO = :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo);
                    }

                    //送り状No
                    if (!string.IsNullOrEmpty(condition.DeliNo))
                    {
                        query.Append(" AND NVL(TSPI.DELI_NO2, TSPI.DELI_NO) = :DELI_NO ");
                        parameters.Add(":DELI_NO", condition.DeliNo);
                    }

                    //納品書No
                    if(!string.IsNullOrEmpty(condition.NouhinNo))
                    {
                        query.Append("AND TSPI.NOUHIN_NO = :NOUHIN_NO");
                        parameters.Add(":NOUHIN_NO", condition.NouhinNo);
                    }
                    query.Append(@" 
                        ) TSPI
                        GROUP BY 
                                TSPI.BOX_NO
                            ,   TSPI.SHIP_TO_STORE_ID
                            ,   TSPI.CENTER_ID 
                            ,   TSPI.SHIPPER_ID
                    ");

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
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<BtoBReference01ResultRow> BtoBReference01GetData(BtoBReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            var query = new StringBuilder(@"
                SELECT
                        PACK.*
                FROM
                        WW_SHP_BTO_B_REFERENCE01 PACK
                WHERE
                        PACK.SHIPPER_ID = :SHIPPER_ID
                    AND PACK.SEQ = :SEQ
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<ShpBtoBReference>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case BtoBReference01SortKey.ShipToStoreIdBoxNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY PACK.SHIP_TO_STORE_ID DESC, PACK.BOX_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY PACK.SHIP_TO_STORE_ID ASC, PACK.BOX_NO ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY PACK.BOX_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY PACK.BOX_NO ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var BtoBReference01s = MvcDbContext.Current.Database.Connection.Query<ShpBtoBReference>(query.ToString(), parameters);

            if (BtoBReference01s.Any()) 
            { 
                condition.ItemSkuSum = BtoBReference01s.First().ItemSkuSum;
                condition.DetailSum = BtoBReference01s.First().DetailSum;
                condition.ResultQtySum = BtoBReference01s.First().ResultQtySum;
            }
            condition.SelectedCnt = MvcDbContext.Current.ShpBtoBReference01s.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

            var shpBtoBReference01s = BtoBReference01s.Select(x => new BtoBReference01ResultRow()
            {
                IsCheck = x.IsCheck,
                BoxNo = x.BoxNo,
                ShipToStoreId = x.ShipToStoreId,
                ShipToStoreName = x.ShipToStoreName,
                TransporterName = x.TransporterName,
                ResultQty = x.ResultQty,
                BatchNo = x.BatchNo,
                ShipPlanDate = x.ShipPlanDate,
                ShipStatusName = x.ShipStatusName,
                KakuDate = x.KakuDate,
                Seq = x.Seq,
                LineNo = x.LineNo
            });
            // Excute paging
            return new StaticPagedList<BtoBReference01ResultRow>(shpBtoBReference01s, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool UpdateShpBtoBReference(IList<SelectedBtoBReference01ViewModel> References)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in References)
                {
                    // 在庫明細
                    var reference01 = MvcDbContext.Current.ShpBtoBReference01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (reference01 == null)
                    {
                        return false;
                    }

                    reference01.SetBaseInfoUpdate();
                    reference01.IsCheck = u.IsCheck;
                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
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
        /// <param name="References"></param>
        /// <returns></returns>
        public bool ShpBtoBReferenceAllChange(BtoBReference01SearchConditions conditions, bool check)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;
                    query = new StringBuilder(@"
                        MERGE INTO WW_SHP_BTO_B_REFERENCE01 WW
                        USING (
                            WITH
                                SELECTES_WORK_DATA AS (
                                    SELECT
                                            WK.ROWID AS TARGETID
                                        ,   WK.*
                                    FROM
                                            WW_SHP_BTO_B_REFERENCE01 WK
                                    WHERE
                                            WK.SEQ = :SEQ
                                        AND WK.SHIPPER_ID = :SHIPPER_ID
                            )
                            SELECT
                                    SLD.*
                            FROM
                                    SELECTES_WORK_DATA SLD
                            LEFT OUTER JOIN
                                    M_STORE_INV INV
                            ON
                                    INV.STORE_ID = SLD.SHIP_TO_STORE_ID
                                AND INV.CENTER_ID = SLD.CENTER_ID
                                AND INV.SHIPPER_ID = SLD.SHIPPER_ID
                            WHERE
                                    (SLD.SHIP_STATUS_NAME = :NOUHIN_ZUMI OR SLD.SHIP_STATUS_NAME = :OKURI_ZUMI)
                                AND INV.STORE_ID IS NULL
                        ) TGT
                        ON(
                                WW.ROWID = TGT.TARGETID
                        )
                        WHEN MATCHED THEN
                            UPDATE SET
                                    WW.UPDATE_DATE = :UPDATE_DATE
                                ,   WW.UPDATE_USER_ID = :UPDATE_USER_ID
                                ,   WW.UPDATE_PROGRAM_NAME = 'BtoBReference'
                                ,   WW.UPDATE_COUNT = UPDATE_COUNT + 1
                                ,   WW.IS_CHECK = :IS_CHECK
                    ");
                    parameters.Add(":SEQ", conditions.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":NOUHIN_ZUMI", BtoBReferenceResource.Print);
                    parameters.Add(":OKURI_ZUMI", BtoBReferenceResource.InvoicePrint);
                    parameters.Add(":UPDATE_DATE", DateTimeOffset.Now);
                    parameters.Add(":UPDATE_USER_ID", Profile.User.UserId);
                    parameters.Add(":IS_CHECK", (check == true) ? 1 : 0);
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }
                trans.Commit();
            }

            conditions.SelectedCnt = MvcDbContext.Current.ShpBtoBReference01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 出荷確定
        /// </summary>
        public void ShipConfirm(long seq, string center_id, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", center_id, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", seq, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_CONFIRMPROGRESSPACKING_ACTUAL",
                param,
                commandType: CommandType.StoredProcedure);

            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IEnumerable<BtoBReference02ResultRow> GetDetailData(BtoBReference02SearchConditions condition)
        {
            var BtoBReference01 = MvcDbContext.Current.ShpBtoBReference01s.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == condition.Seq && x.LineNo == condition.LineNo).FirstOrDefault();
            DynamicParameters parameters = new DynamicParameters();
            var query = new StringBuilder();
            // B（過去分含む）
            if (condition.Parten == "B")
            {
                query.Append(@"
                    WITH
                        SELECTED_PACKING_DATA AS (
                            SELECT
                                    BOX_NO
                                ,   SHIP_TO_STORE_ID
                                ,   DELI_NO
                                ,   DELI_NO2
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   BATCH_NO
                                ,   ITEM_SKU_ID
                                ,   ITEM_ID
                                ,   ITEM_NAME
                                ,   ITEM_COLOR_ID
                                ,   ITEM_SIZE_ID
                                ,   JAN
                                ,   RESULT_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   TRANSPORTER_ID
                                ,   NOUHIN_PRN_USER_ID
                                ,   KEN_USER_ID
                                ,   TO_CHAR(KEN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS KEN_DATE
                                ,   TO_CHAR(NOUHIN_PRN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS NOUHIN_PRN_DATE
                                ,   TO_CHAR(KAKU_DATE, 'YYYY/MM/DD') AS KAKU_DATE
                                ,   KAKU_USER_ID
                                ,   TO_CHAR(CASE_KAKU_DATE, 'YYYY/MM/DD HH24:MI:SS') AS CASE_KAKU_DATE
                                ,   CASE_KAKU_USER_ID
                            FROM
                                    T_SHIP_PACKING_INFO
                            WHERE
                                    BOX_NO = :BOX_NO
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                                AND EC_FLAG = 0
                            UNION
                            SELECT
                                    BOX_NO
                                ,   SHIP_TO_STORE_ID
                                ,   DELI_NO
                                ,   DELI_NO2
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   BATCH_NO
                                ,   ITEM_SKU_ID
                                ,   ITEM_ID
                                ,   ITEM_NAME
                                ,   ITEM_COLOR_ID
                                ,   ITEM_SIZE_ID
                                ,   JAN
                                ,   RESULT_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   TRANSPORTER_ID
                                ,   NOUHIN_PRN_USER_ID
                                ,   KEN_USER_ID
                                ,   TO_CHAR(KEN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS KEN_DATE
                                ,   TO_CHAR(NOUHIN_PRN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS NOUHIN_PRN_DATE
                                ,   TO_CHAR(KAKU_DATE, 'YYYY/MM/DD') AS KAKU_DATE
                                ,   KAKU_USER_ID
                                ,   TO_CHAR(CASE_KAKU_DATE, 'YYYY/MM/DD HH24:MI:SS') AS CASE_KAKU_DATE
                                ,   CASE_KAKU_USER_ID
                            FROM
                                    A_SHIP_PACKING_INFO
                            WHERE
                                    BOX_NO = :BOX_NO
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                                AND EC_FLAG = 0
                    )
                    ,   SELECTED_SHIP_DATA AS(
                            SELECT
                                    DELI_SHIWAKE_CD
                                ,   SHIP_PLAN_DATE
                                ,   ALLOC_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                            FROM
                                    T_SHIPS
                            WHERE
                                    CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                            UNION
                            SELECT
                                    DELI_SHIWAKE_CD
                                ,   SHIP_PLAN_DATE
                                ,   ALLOC_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                            FROM
                                    A_SHIPS
                            WHERE
                                    CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                    )
                ");
            }
            // C(過去分含むかつ日次済み
            else if (condition.Parten == "C")
            {
                query.Append(@"
                    WITH
                        SELECTED_PACKING_DATA AS (
                            SELECT
                                    BOX_NO
                                ,   SHIP_TO_STORE_ID
                                ,   DELI_NO
                                ,   DELI_NO2
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   BATCH_NO
                                ,   ITEM_SKU_ID
                                ,   ITEM_ID
                                ,   ITEM_NAME
                                ,   ITEM_COLOR_ID
                                ,   ITEM_SIZE_ID
                                ,   JAN
                                ,   RESULT_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   TRANSPORTER_ID
                                ,   NOUHIN_PRN_USER_ID
                                ,   KEN_USER_ID
                                ,   TO_CHAR(KEN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS KEN_DATE
                                ,   TO_CHAR(NOUHIN_PRN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS NOUHIN_PRN_DATE
                                ,   TO_CHAR(KAKU_DATE, 'YYYY/MM/DD') AS KAKU_DATE
                                ,   KAKU_USER_ID
                                ,   TO_CHAR(CASE_KAKU_DATE, 'YYYY/MM/DD HH24:MI:SS') AS CASE_KAKU_DATE
                                ,   CASE_KAKU_USER_ID
                            FROM
                                    A_SHIP_PACKING_INFO
                            WHERE
                                    BOX_NO = :BOX_NO
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                                AND EC_FLAG = 0
                    )
                    ,   SELECTED_SHIP_DATA AS(
                            SELECT
                                    DELI_SHIWAKE_CD
                                ,   SHIP_PLAN_DATE
                                ,   ALLOC_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                            FROM
                                    A_SHIPS
                            WHERE
                                    CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                    )
                ");
            }
            // A
            else
            {
                query.Append(@"
                    WITH
                        SELECTED_PACKING_DATA AS (
                            SELECT
                                    BOX_NO
                                ,   SHIP_TO_STORE_ID
                                ,   DELI_NO
                                ,   DELI_NO2
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                                ,   BATCH_NO
                                ,   ITEM_SKU_ID
                                ,   ITEM_ID
                                ,   ITEM_NAME
                                ,   ITEM_COLOR_ID
                                ,   ITEM_SIZE_ID
                                ,   JAN
                                ,   RESULT_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   TRANSPORTER_ID
                                ,   NOUHIN_PRN_USER_ID
                                ,   KEN_USER_ID
                                ,   TO_CHAR(KEN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS KEN_DATE
                                ,   TO_CHAR(NOUHIN_PRN_DATE, 'YYYY/MM/DD HH24:MI:SS') AS NOUHIN_PRN_DATE
                                ,   TO_CHAR(KAKU_DATE, 'YYYY/MM/DD') AS KAKU_DATE
                                ,   KAKU_USER_ID
                                ,   TO_CHAR(CASE_KAKU_DATE, 'YYYY/MM/DD HH24:MI:SS') AS CASE_KAKU_DATE
                                ,   CASE_KAKU_USER_ID
                            FROM
                                    T_SHIP_PACKING_INFO
                            WHERE
                                    BOX_NO = :BOX_NO
                                AND CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                                AND EC_FLAG = 0
                    )
                    ,   SELECTED_SHIP_DATA AS(
                            SELECT
                                    DELI_SHIWAKE_CD
                                ,   SHIP_PLAN_DATE
                                ,   ALLOC_QTY
                                ,   SHIPPER_ID
                                ,   CENTER_ID
                                ,   SHIP_INSTRUCT_ID
                                ,   SHIP_INSTRUCT_SEQ
                            FROM
                                    T_SHIPS
                            WHERE
                                    CENTER_ID = :CENTER_ID
                                AND SHIPPER_ID = :SHIPPER_ID
                                AND SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                    )
                ");
            }
            query.Append(@"
                ,   TARGET_PACK_DATA AS (
                        SELECT
                                PACK.*
                            ,   SHIP.DELI_SHIWAKE_CD
                            ,   SHIP.SHIP_PLAN_DATE
                            ,   SHIP.ALLOC_QTY
                        FROM
                                SELECTED_PACKING_DATA PACK
                        LEFT OUTER JOIN
                                SELECTED_SHIP_DATA SHIP
                        ON
                                SHIP.SHIP_INSTRUCT_ID = PACK.SHIP_INSTRUCT_ID
                            AND SHIP.SHIP_INSTRUCT_SEQ = PACK.SHIP_INSTRUCT_SEQ
                            AND SHIP.CENTER_ID = PACK.CENTER_ID
                            AND SHIP.SHIPPER_ID = PACK.SHIPPER_ID
                )
                SELECT
                        TGT.BOX_NO
                    ,   TGT.SHIP_TO_STORE_ID
                    ,   VSTS.SHIP_TO_STORE_NAME1 SHIP_TO_STORE_NAME
                    ,   NVL(TGT.DELI_NO2, TGT.DELI_NO) DELI_NO
                    ,   MT.TRANSPORTER_NAME
                    ,   TGT.NOUHIN_PRN_DATE
                    ,   MU1.USER_NAME NOUHIN_PRN_USER_NAME
                    ,   TGT.KEN_DATE
                    ,   MU2.USER_NAME KEN_USER_NAME
                    ,   TGT.CASE_KAKU_DATE
                    ,   MU3.USER_NAME AS CASE_KAKU_USER
                    ,   TGT.KAKU_DATE
                    ,   USER_KAKU.USER_NAME AS KAKU_USER_NAME
                    ,   TGT.DELI_SHIWAKE_CD
                    ,   TGT.SHIP_INSTRUCT_ID
                    ,   TGT.SHIP_INSTRUCT_SEQ
                    ,   TGT.SHIP_PLAN_DATE
                    ,   TGT.BATCH_NO
                    ,   MIC.CATEGORY_NAME1
                    ,   TGT.ITEM_SKU_ID
                    ,   TGT.ITEM_ID
                    ,   TGT.ITEM_NAME
                    ,   TGT.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   TGT.ITEM_SIZE_ID
                    ,   MIS.ITEM_SIZE_NAME
                    ,   TGT.JAN
                    ,   TGT.ALLOC_QTY
                    ,   TGT.RESULT_QTY
                FROM
                        TARGET_PACK_DATA TGT
            ");
            query.Append(@"
                LEFT JOIN
                        M_ITEM_SKU MIS
                ON
                        TGT.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                    AND TGT.SHIPPER_ID = MIS.SHIPPER_ID
                LEFT JOIN
                        M_BRANDS MB
                ON
                        MIS.BRAND_ID = MB.BRAND_ID
                    AND MIS.SHIPPER_ID = MB.SHIPPER_ID
                LEFT JOIN
                        M_VENDORS MV
                ON
                        MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                    AND MIS.SHIPPER_ID   = MV.SHIPPER_ID
                LEFT JOIN
                        V_SHIP_TO_STORES VSTS
                ON
                        TGT.SHIP_TO_STORE_ID  = VSTS.SHIP_TO_STORE_ID
                    AND TGT.SHIPPER_ID = VSTS.SHIPPER_ID
                LEFT JOIN
                        M_TRANSPORTERS MT
                ON
                        TGT.TRANSPORTER_ID = MT.TRANSPORTER_ID
                    AND TGT.SHIPPER_ID   = MT.SHIPPER_ID
                LEFT JOIN
                        M_USERS MU1
                ON
                        TGT.NOUHIN_PRN_USER_ID = MU1.USER_ID
                    AND TGT.SHIPPER_ID   = MU1.SHIPPER_ID
                LEFT JOIN
                        M_USERS MU2
                ON
                        TGT.KEN_USER_ID = MU2.USER_ID
                    AND TGT.SHIPPER_ID   = MU2.SHIPPER_ID
                LEFT JOIN
                        M_USERS MU3
                ON
                        TGT.CASE_KAKU_USER_ID = MU3.USER_ID
                    AND TGT.SHIPPER_ID   = MU3.SHIPPER_ID
                LEFT OUTER JOIN
                        M_USERS USER_KAKU
                ON
                        TGT.KAKU_USER_ID = USER_KAKU.USER_ID
                    AND TGT.SHIPPER_ID   = USER_KAKU.SHIPPER_ID
                LEFT JOIN
                        M_COLORS MC
                ON
                        TGT.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                    AND TGT.SHIPPER_ID   = MC.SHIPPER_ID
                LEFT JOIN
                        M_ITEM_CATEGORIES4 MIC
                ON
                        MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1
                    AND MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2
                    AND MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3
                    AND MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4
                    AND MIS.SHIPPER_ID   = MIC.SHIPPER_ID

                ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", BtoBReference01.CenterId);
            parameters.Add(":BOX_NO", BtoBReference01.BoxNo);
            parameters.Add(":SHIP_TO_STORE_ID", BtoBReference01.ShipToStoreId);

            // Sort function
            switch (condition.SortKey)
            {
                case BtoBReference02SortKey.SkuShipInstructIdSeq:
                    query.AppendLine(@"ORDER BY TGT.ITEM_SKU_ID ASC,TGT.SHIP_INSTRUCT_ID ASC,TGT.SHIP_INSTRUCT_SEQ ASC ");

                    break;
                default:
                    query.AppendLine(@"ORDER BY TGT.SHIP_INSTRUCT_ID ASC,TGT.SHIP_INSTRUCT_SEQ ASC ");

                    break;
            }
            return MvcDbContext.Current.Database.Connection.Query<BtoBReference02ResultRow>(query.ToString(), parameters);
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListDivisions()
        {
            return MvcDbContext.Current.Divisions
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.DivisionId,
                    Text = m.DivisionId + ":" + m.DivisionName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// アイテムデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return MvcDbContext.Current.Items
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.ItemCode,
                    Text = m.ItemCode + ":" + m.ItemCodeName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類1データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys1()
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId1,
                    Text = m.CategoryId1.ToString() + ":" + m.CategoryName1
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2,
                    Text = m.CategoryId2.ToString() + ":" + m.CategoryName2
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId3,
                    Text = m.CategoryId3.ToString() + ":" + m.CategoryName3
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2)
                && (categoryId3 == null ? 1 == 1 : m.CategoryId3 == categoryId3))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId4,
                    Text = m.CategoryId4.ToString() + ":" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 配送業者データ取得
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
                .OrderBy(m => m.Value);
        }
    }
}