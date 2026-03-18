namespace Wms.Areas.Arrival.Query.PurchaseReference
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Wms.Areas.Arrival.Models;
    using Wms.Areas.Arrival.ViewModels.PurchaseReference;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Arrival.ViewModels.PurchaseReference.PurchaseReference01SearchConditions;
    using static Wms.Areas.Arrival.ViewModels.PurchaseReference.PurchaseReference02SearchConditions;

    public class PurchaseReferenceQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertArrPurchaseReference01(PurchaseReference01SearchConditions condition)
        {
            // 1.ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            // 2.ワーク作成
            var result = AddArrPurRef01(condition, false);
            // 過去分を含む場合
            if (result && condition.ContainsArchive)
            {
                result = AddArrPurRef01(condition, true);
            }
            return result;
        }

        /// <summary>
        /// 仕入入荷進捗照会ワーク01にデータを登録する
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <param name="archiveTable">累積テーブルを取得するかどうか</param>
        /// <returns></returns>
        private bool AddArrPurRef01(PurchaseReference01SearchConditions condition, bool archiveTable)
        {
            string arriveTableName;
            string packingArriveTableName;
            List<int> lineNoList = MvcDbContext.Current.ArrPurRef01s
                .Where(x => x.Seq == condition.Seq && x.ShipperId == Common.Profile.User.ShipperId)
                .Select(x => (int)x.LineNo).ToList();

            if (archiveTable)
            {
                arriveTableName = "V_A_ARRIVE ARRIVE";
                packingArriveTableName = "V_A_PACKING_ARRIVE PACKING_ARRIVE";
            }
            else
            {
                arriveTableName = "V_ARRIVE ARRIVE";
                packingArriveTableName = "V_PACKING_ARRIVE PACKING_ARRIVE";
            }

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_ARR_PUR_REF01 (
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
                        ,   CENTER_ID
                        ,   ARRIVE_PLAN_DATE
                        ,   VENDOR_ID
                        ,   VENDOR_NAME
                        ,   PLAN_INVOICE_NO
                        ,   RESULT_INVOICE_NO
                        ,   CASE_PLAN_QTY
                        ,   CASE_RESULT_QTY
                        ,   ITEM_ID
                        ,   ITEM_SKU_ID
                        ,   ARRIVE_PLAN_QTY
                        ,   RESULT_QTY
                        ,   REMAINDER_QTY
                        ,   DIFFERENCE_PLUS
                        ,   DIFFERENCE_MINUS
                        ,   BRAND_ID
                        ,   BRAND_NAME
                        ,   BRAND_SHORT_NAME
                    )
                    WITH
                        PACKING_ARRIVE AS (
                            SELECT
                                    NVL(PACKING_ARRIVE.SHIPPER_ID,ARRIVE.SHIPPER_ID) AS SHIPPER_ID
                                ,   NVL(PACKING_ARRIVE.CENTER_ID,ARRIVE.CENTER_ID) AS CENTER_ID
                                ,   NVL(PACKING_ARRIVE.INVOICE_NO,ARRIVE.INVOICE_NO) AS INVOICE_NO
                                ,   NVL(PACKING_ARRIVE.ITEM_SKU_ID,ARRIVE.ITEM_SKU_ID) AS ITEM_SKU_ID
                                ,   COUNT(DISTINCT(TRIM(PACKING_ARRIVE.PLAN_BOX_NO))) AS CASE_PLAN_QTY
                                ,   COUNT(DISTINCT(TRIM(PACKING_ARRIVE.RESULT_BOX_NO))) AS CASE_RESULT_QTY
                                ,   SUM(PACKING_ARRIVE.PACKING_REMAINDER_QTY) AS PACKING_REMAINDER_QTY
                                ,   SUM(PACKING_ARRIVE.PACKING_PLAN_QTY) AS PACKING_PLAN_QTY
                                ,   SUM(PACKING_ARRIVE.PACKING_RESULT_QTY) AS PACKING_RESULT_QTY
                            FROM
                    ");
                    query.AppendLine(@"                    " + arriveTableName);
                    query.AppendLine(@"
                            LEFT OUTER JOIN
                    ");
                    query.AppendLine(@"                    " + packingArriveTableName);
                    query.AppendLine(@"
                            ON
                                    PACKING_ARRIVE.SHIPPER_ID = ARRIVE.SHIPPER_ID
                                AND PACKING_ARRIVE.CENTER_ID = ARRIVE.CENTER_ID
                                AND PACKING_ARRIVE.INVOICE_NO = ARRIVE.INVOICE_NO
                                AND PACKING_ARRIVE.ITEM_SKU_ID = ARRIVE.ITEM_SKU_ID
                            WHERE
                                    NVL(PACKING_ARRIVE.PACKING_STATUS,0) < 2
                            GROUP BY
                                    NVL(PACKING_ARRIVE.SHIPPER_ID,ARRIVE.SHIPPER_ID)
                                ,   NVL(PACKING_ARRIVE.CENTER_ID,ARRIVE.CENTER_ID)
                                ,   NVL(PACKING_ARRIVE.INVOICE_NO,ARRIVE.INVOICE_NO)
                                ,   NVL(PACKING_ARRIVE.ITEM_SKU_ID,ARRIVE.ITEM_SKU_ID)
                    )
                    ,   TARGET_CASE_QTY AS (
                            SELECT
                                    PACKING_ARRIVE.SHIPPER_ID AS SHIPPER_ID
                                ,   PACKING_ARRIVE.CENTER_ID AS CENTER_ID
                                ,   PACKING_ARRIVE.INVOICE_NO AS INVOICE_NO
                                ,   COUNT(DISTINCT(TRIM(PACKING_ARRIVE.PLAN_BOX_NO))) AS CASE_PLAN_QTY
                                ,   COUNT(DISTINCT(TRIM(PACKING_ARRIVE.RESULT_BOX_NO))) AS CASE_RESULT_QTY
                            FROM
                    ");
                    query.AppendLine(@"                    " + packingArriveTableName);
                    query.AppendLine(@"
                            WHERE
                                    NVL(PACKING_ARRIVE.PACKING_STATUS,0) < 2
                            GROUP BY
                                    PACKING_ARRIVE.SHIPPER_ID
                                ,   PACKING_ARRIVE.CENTER_ID
                                ,   PACKING_ARRIVE.INVOICE_NO
                    )
                    SELECT
                            SYSTIMESTAMP
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   SYSTIMESTAMP
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   0
                        ,   ARRIVE.SHIPPER_ID
                        ,   :SEQ
                        ,   ROWNUM + :MAX_LINE_NO
                        ,   ARRIVE.CENTER_ID
                        ,   ARRIVE.ARRIVE_PLAN_DATE
                        ,   ARRIVE.VENDOR_ID
                        ,   ARRIVE.VENDOR_NAME1 AS VENDOR_NAME
                        ,   ARRIVE.INVOICE_NO AS PLAN_INVOICE_NO
                        ,   CASE
                                WHEN ARRIVE.RESULT_QTY > 0 THEN ARRIVE.INVOICE_NO
                                ELSE NULL
                            END AS RESULT_INVOICE_NO
                        ,   TARGET_CASE_QTY.CASE_PLAN_QTY
                        ,   CASE
                                WHEN TARGET_CASE_QTY.CASE_RESULT_QTY = 0 THEN NULL
                                ELSE TARGET_CASE_QTY.CASE_RESULT_QTY
                            END AS CASE_RESULT_QTY
                        ,   ARRIVE.ITEM_ID
                        ,   ARRIVE.ITEM_SKU_ID
                        ,   ARRIVE.ARRIVE_PLAN_QTY
                        ,   ARRIVE.RESULT_QTY
                        ,   CASE
                                WHEN PACKING_ARRIVE.SHIPPER_ID IS NULL THEN ARRIVE.ARRIVE_REMAINDER_QTY
                                ELSE PACKING_ARRIVE.PACKING_REMAINDER_QTY
                            END AS REMAINDER_QTY
                        ,   CASE
                                WHEN ARRIVE.ARRIVE_DIFFERENCE_QTY > 0 THEN ARRIVE.ARRIVE_DIFFERENCE_QTY
                                ELSE NULL
                            END AS DIFFERENCE_PLUS
                        ,   CASE
                                WHEN ARRIVE.ARRIVE_DIFFERENCE_QTY < 0 THEN ARRIVE.ARRIVE_DIFFERENCE_QTY
                                ELSE NULL
                            END AS DIFFERENCE_MINUS
                        ,   ARRIVE.BRAND_ID
                        ,   ARRIVE.BRAND_NAME
                        ,   ARRIVE.BRAND_SHORT_NAME
                    FROM ");
                    query.AppendLine("                        " + arriveTableName);
                    query.AppendLine(@"                LEFT JOIN
                            PACKING_ARRIVE
                    ON
                            PACKING_ARRIVE.SHIPPER_ID = ARRIVE.SHIPPER_ID
                        AND PACKING_ARRIVE.CENTER_ID = ARRIVE.CENTER_ID
                        AND PACKING_ARRIVE.INVOICE_NO = ARRIVE.INVOICE_NO
                        AND PACKING_ARRIVE.ITEM_SKU_ID = ARRIVE.ITEM_SKU_ID
                    LEFT JOIN
                            TARGET_CASE_QTY
                    ON
                            TARGET_CASE_QTY.SHIPPER_ID = ARRIVE.SHIPPER_ID
                        AND TARGET_CASE_QTY.CENTER_ID = ARRIVE.CENTER_ID
                        AND TARGET_CASE_QTY.INVOICE_NO = ARRIVE.INVOICE_NO
                    WHERE
                            ARRIVE.SHIPPER_ID = :SHIPPER_ID
                        AND ARRIVE.CANCEL_FLAG = 0");

                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "AddArrPurRef01");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":MAX_LINE_NO", lineNoList.Count > 0 ? lineNoList.Max() : 0);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

                    // Add search condition
                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.AppendLine("                    AND ARRIVE.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

                    // 入荷予定日(検索・From-To)
                    if (condition.ArrivePlanDateFrom != null)
                    {
                        query.AppendLine("                    AND ARRIVE.ARRIVE_PLAN_DATE >= :ARRIVE_PLAN_DATE_FROM ");
                        parameters.Add(":ARRIVE_PLAN_DATE_FROM", condition.ArrivePlanDateFrom);
                    }
                    if (condition.ArrivePlanDateTo != null)
                    {
                        query.AppendLine("                    AND ARRIVE.ARRIVE_PLAN_DATE <= :ARRIVE_PLAN_DATE_TO ");
                        parameters.Add(":ARRIVE_PLAN_DATE_TO", condition.ArrivePlanDateTo);
                    }

                    // 実績確定日(検索・From-To)
                    if (condition.ConfirmDateFrom != null)
                    {
                        query.AppendLine("                    AND ARRIVE.ARRIVAL_STATUS >= 4 ");
                        query.AppendLine("                    AND TRUNC(ARRIVE.CONFIRM_DATE,'DD') >= :CONFIRM_DATE_FROM ");
                        parameters.Add(":CONFIRM_DATE_FROM", condition.ConfirmDateFrom);
                    }
                    if (condition.ConfirmDateTo != null)
                    {
                        query.AppendLine("                    AND ARRIVE.ARRIVAL_STATUS >= 4 ");
                        query.AppendLine("                    AND TRUNC(ARRIVE.CONFIRM_DATE,'DD') <= :CONFIRM_DATE_TO ");
                        parameters.Add(":CONFIRM_DATE_TO", condition.ConfirmDateTo);
                    }

                    // 事業部
                    if (!string.IsNullOrEmpty(condition.DivisionId))
                    {
                        query.AppendLine("                    AND ARRIVE.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionId);
                    }

                    //ブランド
                    if (condition.BrandId.Any())
                    {
                        if (!string.IsNullOrEmpty(condition.BrandId[0]))
                        {
                            query.AppendLine("                    AND ARRIVE.BRAND_ID IN :BRAND_ID ");
                            //var brandList = condition.BrandId.Split(',');
                            parameters.Add(":BRAND_ID", condition.BrandId);
                        }
                    }

                    // 仕入先
                    if (!string.IsNullOrEmpty(condition.VendorId))
                    {
                        query.AppendLine("                    AND ARRIVE.VENDOR_ID LIKE :VENDOR_ID ");
                        parameters.Add(":VENDOR_ID", condition.VendorId + "%");
                    }

                    // 仕入先名
                    if (string.IsNullOrEmpty(condition.VendorId) && !string.IsNullOrEmpty(condition.VendorName))
                    {
                        query.AppendLine("                    AND ARRIVE.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                        parameters.Add(":VENDOR_NAME1", condition.VendorName + "%");
                    }

                    // 納品書番号
                    if (!string.IsNullOrEmpty(condition.InvoiceNo))
                    {
                        query.AppendLine("                    AND ARRIVE.INVOICE_NO LIKE :INVOICE_NO ");
                        parameters.Add(":INVOICE_NO", condition.InvoiceNo + "%");
                    }

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// 仕入入荷進捗照会ワーク01に登録されたデータを取得する
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<PurchaseReference01ResultRow> PurchaseReference01GetData(PurchaseReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT
                        SEQ
                    ,   MAX(LINE_NO) AS LINE_NO
                    ,   ARRIVE_PLAN_DATE
                    ,   BRAND_ID
                    ,   BRAND_SHORT_NAME
                    ,   VENDOR_ID
                    ,   MAX(VENDOR_NAME) AS VENDOR_NAME
                    ,   MAX(CASE_PLAN_QTY) AS CASE_PLAN_QTY
                    ,   MAX(CASE_RESULT_QTY) AS CASE_RESULT_QTY
                    ,   COUNT(DISTINCT(ITEM_ID)) ITEM_QTY
                    ,   COUNT(DISTINCT(ITEM_SKU_ID)) ITEM_SKU_QTY
                    ,   SUM(ARRIVE_PLAN_QTY) AS ARRIVE_PLAN_QTY
                    ,   SUM(RESULT_QTY) AS RESULT_QTY
                    ,   SUM(REMAINDER_QTY) AS REMAINDER_QTY
                    ,   SUM(DIFFERENCE_PLUS) AS DIFFERENCE_PLUS
                    ,   SUM(DIFFERENCE_MINUS) AS DIFFERENCE_MINUS
                    ,   COUNT(DISTINCT(PLAN_INVOICE_NO)) AS INVOICE_PLAN_QTY
                    ,   COUNT(DISTINCT(RESULT_INVOICE_NO)) AS INVOICE_RESULT_QTY
                FROM
                        WW_ARR_PUR_REF01
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND SEQ = :SEQ
                GROUP BY
                        SEQ
                    ,   SHIPPER_ID
                    ,   CENTER_ID
                    ,   ARRIVE_PLAN_DATE
                    ,   VENDOR_ID
                    ,   BRAND_ID
                    ,   BRAND_SHORT_NAME
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<PurchaseReference01ResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case PurchaseReference01SortKey.ArrivePlanDateVendorId:
                    switch (condition.Sort)
                    {
                        case PurchaseReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,BRAND_ID DESC,VENDOR_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,BRAND_ID ASC,VENDOR_ID ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case PurchaseReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY BRAND_ID DESC,VENDOR_ID DESC,ARRIVE_PLAN_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY BRAND_ID ASC,VENDOR_ID ASC,ARRIVE_PLAN_DATE ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var purchaseReference01s = MvcDbContext.Current.Database.Connection.Query<PurchaseReference01ResultRow>(query.ToString(), parameters);
            var arrPurRef01s = GetDetailData01(condition);
            condition.ItemSkuQtySum = arrPurRef01s.ItemSkuQtySum;
            condition.ArrivePlanQtySum = arrPurRef01s.ArrivePlanQtySum ?? 0;
            condition.ResultQtySum = arrPurRef01s.ResultQtySum ?? 0;
            condition.PlanSlipNoSum = arrPurRef01s.PlanSlipNoSum;
            condition.ResultSlipNoSum = arrPurRef01s.ResultSlipNoSum;

            // Excute paging
            return new StaticPagedList<PurchaseReference01ResultRow>(purchaseReference01s, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 明細ヘッダ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public PurchaseReference01SearchConditions GetDetailData01(PurchaseReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
            SELECT
                    COUNT(DISTINCT(WW.ITEM_SKU_ID)) AS ITEM_SKU_QTY_SUM
                ,   SUM(WW.ARRIVE_PLAN_QTY) AS ARRIVE_PLAN_QTY_SUM
                ,   SUM(WW.RESULT_QTY) AS RESULT_QTY_SUM
                ,   COUNT(DISTINCT(WW.PLAN_INVOICE_NO)) AS PLAN_SLIP_NO_SUM
                ,   COUNT(DISTINCT(WW.RESULT_INVOICE_NO)) AS RESULT_SLIP_NO_SUM
            FROM
                    WW_ARR_PUR_REF01 WW
            WHERE
                    WW.SHIPPER_ID = :SHIPPER_ID
                AND WW.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            return MvcDbContext.Current.Database.Connection.Query<PurchaseReference01SearchConditions>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertArrPurchaseReference02(PurchaseReference02SearchConditions condition)
        {
            // 1.ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            // 2.ワーク作成
            var result = AddArrPurRef02(condition, false);
            // 過去分を含む場合
            if (result && condition.ContainsArchive)
            {
                result = AddArrPurRef02(condition, true);
            }
            return result;
        }

        /// <summary>
        /// 仕入入荷進捗照会ワーク02にデータを登録する
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <param name="archiveTable">累積テーブルを取得するかどうか</param>
        /// <returns></returns>
        private bool AddArrPurRef02(PurchaseReference02SearchConditions condition, bool archiveTable)
        {
            string arriveTableName;
            string packingArriveArchiveTableName;
            string packingArriveTableName;
            string shipTableName;
            List<int> lineNoList = MvcDbContext.Current.ArrPurRef02s
                .Where(x => x.Seq == condition.Seq && x.ShipperId == Common.Profile.User.ShipperId)
                .Select(x => (int)x.LineNo).ToList();

            if (archiveTable)
            {
                arriveTableName = "V_A_ARRIVE ARRIVE";
                packingArriveArchiveTableName = "V_A_PACKING_ARRIVE PACKING_ARRIVE";
                packingArriveTableName = "A_PACKING_ARRIVE_PLANS PACKING_PLAN";
                shipTableName = "A_SHIPS SHIP";
            }
            else
            {
                arriveTableName = "V_ARRIVE ARRIVE";
                packingArriveArchiveTableName = "V_PACKING_ARRIVE PACKING_ARRIVE";
                packingArriveTableName = "T_PACKING_ARRIVE_PLANS PACKING_PLAN";
                shipTableName = "T_SHIPS SHIP";
            }

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    var query = new StringBuilder();

                    query.Append(@"
                        INSERT INTO WW_ARR_PUR_REF02 (
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
                            ,   CENTER_ID
                            ,   ARRIVE_PLAN_DATE
                            ,   VENDOR_ID
                            ,   VENDOR_NAME
                            ,   INVOICE_NO
                            ,   INVOICE_SEQ
                            ,   PO_ID
                            ,   CATEGORY_NAME1
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_SKU_ID
                            ,   ITEM_COLOR_ID
                            ,   ITEM_COLOR_NAME
                            ,   ITEM_SIZE_ID
                            ,   ITEM_SIZE_NAME
                            ,   JAN
                            ,   BOX_NO
                            ,   ARRIVE_PLAN_QTY
                            ,   RESULT_QTY
                            ,   DIFFERENCE_QTY
                            ,   ARRIVAL_STATUS
                            ,   ARRIVAL_STATUS_NAME
                            ,   WMS_INSTRUCT_QTY
                            ,   STORAGE_PLAN_QTY
                            ,   CONFIRM_DATE
                            ,   IF_RUN_DATE
                            ,   BRAND_ID
                            ,   BRAND_NAME
                            ,   BRAND_SHORT_NAME
                        )
                    ");

                    // 明細
                    if (condition.ResultType == ResultTypes.Detail)
                    {
                        query.Append(@"
                        WITH
                            SHIP AS (
                                SELECT
                                        SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SHIP_INSTRUCT_ID
                                    ,   ITEM_SKU_ID
                                    ,   SUM(WMS_INSTRUCT_QTY) AS WMS_INSTRUCT_QTY
                                    ,   SUM(INSTRUCT_QTY) AS INSTRUCT_QTY
                                FROM
                        ");
                        query.AppendLine("                " + shipTableName);
                        query.AppendLine(@"                        GROUP BY
                                        SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   SHIP_INSTRUCT_ID
                                    ,   ITEM_SKU_ID
                        )
                        SELECT
                                SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   0
                            ,   ARRIVE.SHIPPER_ID
                            ,   :SEQ
                            ,   ROWNUM + :MAX_LINE_NO
                            ,   ARRIVE.CENTER_ID
                            ,   ARRIVE.ARRIVE_PLAN_DATE
                            ,   ARRIVE.VENDOR_ID
                            ,   ARRIVE.VENDOR_NAME1 AS VENDOR_NAME
                            ,   ARRIVE.INVOICE_NO
                            ,   ARRIVE.INVOICE_SEQ
                            ,   ARRIVE.PO_ID
                            ,   ARRIVE.CATEGORY_NAME1
                            ,   ARRIVE.ITEM_ID
                            ,   ARRIVE.ITEM_NAME
                            ,   ARRIVE.ITEM_SKU_ID
                            ,   ARRIVE.ITEM_COLOR_ID
                            ,   ARRIVE.ITEM_COLOR_NAME
                            ,   ARRIVE.ITEM_SIZE_ID
                            ,   ARRIVE.ITEM_SIZE_NAME
                            ,   ARRIVE.JAN
                            ,   '' BOX_NO
                            ,   ARRIVE.ARRIVE_PLAN_QTY
                            ,   ARRIVE.RESULT_QTY
                            ,   CASE
                                    WHEN ARRIVE.RESULT_QTY IS NULL THEN NULL
                                    ELSE ARRIVE.RESULT_QTY - ARRIVE.ARRIVE_PLAN_QTY
                                END AS DIFFERENCE_QTY
                            ,   ARRIVE.ARRIVAL_STATUS
                            ,   ARRIVE.ARRIVAL_STATUS_NAME
                            ,   NVL(SHIP.WMS_INSTRUCT_QTY,0) AS WMS_INSTRUCT_QTY
                            ,   ARRIVE.ARRIVE_PLAN_QTY - NVL(SHIP.INSTRUCT_QTY,0) AS STORAGE_PLAN_QTY
                            ,   CASE
                                    WHEN ARRIVAL_STATUS >= 4 THEN ARRIVE.CONFIRM_DATE
                                    ELSE NULL
                                END AS CONFIRM_DATE
                            ,   ARRIVE.IF_RUN_DATE
                            ,   ARRIVE.BRAND_ID
                            ,   ARRIVE.BRAND_NAME
                            ,   ARRIVE.BRAND_SHORT_NAME
                        FROM ");
                        query.AppendLine(@"                        " + arriveTableName);
                        query.AppendLine(@"                LEFT JOIN
                                SHIP
                        ON
                                ARRIVE.SHIPPER_ID = SHIP.SHIPPER_ID
                            AND ARRIVE.CENTER_ID = SHIP.CENTER_ID
                            AND ARRIVE.INVOICE_NO = SHIP.SHIP_INSTRUCT_ID
                            AND ARRIVE.ITEM_SKU_ID = SHIP.ITEM_SKU_ID
                        WHERE
                                ARRIVE.SHIPPER_ID = :SHIPPER_ID
                            AND ARRIVE.CANCEL_FLAG = 0
                            ");

                        // ケースNo
                        if (!string.IsNullOrEmpty(condition.BoxNo))
                        {
                            query.Append(@" 
                                    AND EXISTS (
                                            SELECT
                                                    *
                                            FROM
                            ");
                            query.AppendLine(@"    " + packingArriveTableName);
                            query.AppendLine(@"
                                            WHERE
                                                    PACKING_PLAN.SHIPPER_ID = ARRIVE.SHIPPER_ID
                                                AND PACKING_PLAN.CENTER_ID = ARRIVE.CENTER_ID
                                                AND PACKING_PLAN.INVOICE_NO = ARRIVE.INVOICE_NO
                                                AND PACKING_PLAN.ITEM_SKU_ID = ARRIVE.ITEM_SKU_ID
                                                AND PACKING_PLAN.BOX_NO LIKE :BOX_NO
                                        ) ");
                            parameters.Add(":BOX_NO", condition.BoxNo + "%");
                        }

                    }
                    //ケース別
                    else
                    {
                        query.Append(@"
                            SELECT
                                    SYSTIMESTAMP
                                ,   :USER_ID
                                ,   :PROGRAM_NAME
                                ,   SYSTIMESTAMP
                                ,   :USER_ID
                                ,   :PROGRAM_NAME
                                ,   0
                                ,   AR.SHIPPER_ID
                                ,   :SEQ
                                ,   ROWNUM + :MAX_LINE_NO
                                ,   AR.CENTER_ID
                                ,   AR.ARRIVE_PLAN_DATE
                                ,   AR.VENDOR_ID
                                ,   AR.VENDOR_NAME
                                ,   AR.INVOICE_NO
                                ,   AR.INVOICE_SEQ
                                ,   AR.PO_ID
                                ,   AR.CATEGORY_NAME1
                                ,   AR.ITEM_ID
                                ,   AR.ITEM_NAME
                                ,   AR.ITEM_SKU_ID
                                ,   AR.ITEM_COLOR_ID
                                ,   AR.ITEM_COLOR_NAME
                                ,   AR.ITEM_SIZE_ID
                                ,   AR.ITEM_SIZE_NAME
                                ,   AR.JAN
                                ,   NVL(NVL(AR.DISP_BOX_NO, AR.PLAN_BOX_NO), N' ') AS BOX_NO
                                ,   CASE
                                        WHEN AR.LINE_NO = 1 THEN
                                            CASE
                                                WHEN AR.PACKING_PLAN_QTY IS NOT NULL THEN AR.PACKING_PLAN_QTY
                                                ELSE AR.ARRIVE_PLAN_QTY
                                            END
                                        ELSE 
                                            NULL 
                                    END                           AS ARRIVE_PLAN_QTY
                                ,   AR.PACKING_RESULT_QTY                       AS RESULT_QTY
                                ,   CASE
                                        WHEN AR.LINE_NO = 1 THEN AR.DIFF_QTY
                                        ELSE NULL 
                                    END                           AS DIFFERENCE_QTY
                                ,   AR.ARRIVAL_STATUS
                                ,   AR.ARRIVAL_STATUS_NAME
                                ,   0 AS WMS_INSTRUCT_QTY
                                ,   0 AS STORAGE_PLAN_QTY
                                ,   AR.CONFIRM_DATE
                                ,   AR.IF_RUN_DATE
                                ,   AR.BRAND_ID
                                ,   AR.BRAND_NAME
                                ,   AR.BRAND_SHORT_NAME
                            FROM (
                                SELECT
                                        ARRIVE.SHIPPER_ID
                                    ,   ARRIVE.CENTER_ID
                                    ,   ARRIVE.ARRIVE_PLAN_DATE
                                    ,   ARRIVE.VENDOR_ID
                                    ,   ARRIVE.VENDOR_NAME1 AS VENDOR_NAME
                                    ,   ARRIVE.INVOICE_NO
                                    ,   ARRIVE.INVOICE_SEQ
                                    ,   ARRIVE.PO_ID
                                    ,   ARRIVE.CATEGORY_NAME1
                                    ,   ARRIVE.ITEM_ID
                                    ,   ARRIVE.ITEM_NAME
                                    ,   ARRIVE.ITEM_SKU_ID
                                    ,   ARRIVE.ITEM_COLOR_ID
                                    ,   ARRIVE.ITEM_COLOR_NAME
                                    ,   ARRIVE.ITEM_SIZE_ID
                                    ,   ARRIVE.ITEM_SIZE_NAME
                                    ,   ARRIVE.JAN
                                    ,   PACKING_ARRIVE.DISP_BOX_NO
                                    ,   PACKING_ARRIVE.PLAN_BOX_NO
                                    ,   ARRIVE.ARRIVE_PLAN_QTY
                                    ,   ARRIVE.RESULT_QTY
                                    ,   PACKING_ARRIVE.PACKING_PLAN_QTY
                                    ,   PACKING_ARRIVE.PACKING_RESULT_QTY
                                    ,   ROW_NUMBER() OVER(
                                                    PARTITION BY
                                                            ARRIVE.SHIPPER_ID
                                                        ,   ARRIVE.CENTER_ID
                                                        ,   ARRIVE.INVOICE_NO
                                                        ,   ARRIVE.PO_ID
                                                        ,   ARRIVE.ITEM_SKU_ID
                                                        ,   PACKING_ARRIVE.PLAN_BOX_NO  --梱包予定が無い場合は無視してまとめる
                                                    ORDER BY
                                                            PACKING_ARRIVE.TCDC_CLASS
                                                        ,   PACKING_ARRIVE.CASE_CLASS
                                        )                                               AS LINE_NO  --行番号:1が梱包予定のケースNo/SKU単位で先頭に来るデータ(予定数と差異数を表示するため)
                                    ,   SUM(PACKING_ARRIVE.PACKING_RESULT_QTY) OVER(
                                                        PARTITION BY
                                                                ARRIVE.SHIPPER_ID
                                                            ,   ARRIVE.CENTER_ID
                                                            ,   ARRIVE.INVOICE_NO
                                                            ,   ARRIVE.PO_ID
                                                            ,   ARRIVE.ITEM_SKU_ID
                                                            ,   PACKING_ARRIVE.PLAN_BOX_NO  --梱包予定がある場合はケース単位、無い場合はSKU単位でまとめる
                                                    )
                                        - (CASE
                                                WHEN PACKING_ARRIVE.PLAN_BOX_NO IS NOT NULL THEN PACKING_ARRIVE.PACKING_PLAN_QTY
                                                ELSE ARRIVE.ARRIVE_PLAN_QTY
                                            END)                                        AS DIFF_QTY --[差分] = 実績数-予定数
                                    ,   ARRIVE.ARRIVAL_STATUS
                                    ,   ARRIVE.ARRIVAL_STATUS_NAME
                                    ,   CASE
                                            WHEN ARRIVAL_STATUS >= 4 THEN ARRIVE.CONFIRM_DATE
                                            ELSE NULL
                                        END AS CONFIRM_DATE
                                    ,   ARRIVE.IF_RUN_DATE
                                    ,   ARRIVE.BRAND_ID
                                    ,   ARRIVE.BRAND_NAME
                                    ,   ARRIVE.BRAND_SHORT_NAME
                                FROM
                        ");
                        query.AppendLine(@"                " + arriveTableName);
                        query.AppendLine(@"                        LEFT JOIN ");
                        query.AppendLine(@"                                " + packingArriveArchiveTableName);
                        query.AppendLine(@"                        ON
                                        ARRIVE.SHIPPER_ID = PACKING_ARRIVE.SHIPPER_ID
                                    AND ARRIVE.CENTER_ID = PACKING_ARRIVE.CENTER_ID
                                    AND ARRIVE.INVOICE_NO = PACKING_ARRIVE.INVOICE_NO
                                    AND ARRIVE.ITEM_SKU_ID = PACKING_ARRIVE.ITEM_SKU_ID
                                WHERE
                                        ARRIVE.SHIPPER_ID = :SHIPPER_ID 
                                    AND ARRIVE.CANCEL_FLAG = 0 ");


                        // Add search condition
                        //// LineNo
                        //if (condition.LineNo != 0)
                        //{
                        //    var arrPurRef02 = MvcDbContext.Current.ArrPurRef02s.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == condition.Seq && x.LineNo == condition.LineNo).FirstOrDefault();
                        //    query.AppendLine(@"                    AND ARRIVE.INVOICE_NO = :INVOICE_NO
                        //                        AND ARRIVE.INVOICE_SEQ = :INVOICE_SEQ");
                        //    parameters.Add(":INVOICE_NO", arrPurRef02.InvoiceNo);
                        //    parameters.Add(":INVOICE_SEQ", arrPurRef02.InvoiceSeq);
                        //}

                        // ケースNo
                        if (!string.IsNullOrEmpty(condition.BoxNo))
                        {
                            query.AppendLine(@"                    AND PACKING_ARRIVE.PLAN_BOX_NO LIKE :BOX_NO ");
                            parameters.Add(":BOX_NO", condition.BoxNo + "%");
                        }
                    }

                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "AddArrPurRef02");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":MAX_LINE_NO", lineNoList.Count > 0 ? lineNoList.Max() : 0);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.AppendLine(@"                    AND ARRIVE.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

                    // 入荷予定日(検索・From-To)
                    if (condition.ArrivePlanDateFrom != null)
                    {
                        query.AppendLine(@"                    AND ARRIVE.ARRIVE_PLAN_DATE >= :ARRIVE_PLAN_DATE_FROM ");
                        parameters.Add(":ARRIVE_PLAN_DATE_FROM", condition.ArrivePlanDateFrom);
                    }
                    if (condition.ArrivePlanDateTo != null)
                    {
                        query.AppendLine(@"                    AND ARRIVE.ARRIVE_PLAN_DATE <= :ARRIVE_PLAN_DATE_TO ");
                        parameters.Add(":ARRIVE_PLAN_DATE_TO", condition.ArrivePlanDateTo);
                    }

                    // 実績確定日(検索・From-To)
                    if (condition.ConfirmDateFrom != null)
                    {
                        query.AppendLine(@"                    AND ARRIVE.ARRIVAL_STATUS >= 4 ");
                        query.AppendLine(@"                    AND TRUNC(ARRIVE.CONFIRM_DATE,'DD') >= :CONFIRM_DATE_FROM ");
                        parameters.Add(":CONFIRM_DATE_FROM", condition.ConfirmDateFrom);
                    }
                    if (condition.ConfirmDateTo != null)
                    {
                        query.AppendLine(@"                    AND ARRIVE.ARRIVAL_STATUS >= 4 ");
                        query.AppendLine(@"                    AND TRUNC(ARRIVE.CONFIRM_DATE,'DD') <= :CONFIRM_DATE_TO ");
                        parameters.Add(":CONFIRM_DATE_TO", condition.ConfirmDateTo);
                    }

                    // 事業部
                    if (!string.IsNullOrEmpty(condition.DivisionId))
                    {
                        query.AppendLine(@"                    AND ARRIVE.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionId);
                    }

                    // ブランド
                    if (condition.BrandId.Any())
                    {
                        if (!string.IsNullOrEmpty(condition.BrandId[0]))
                        {
                            query.AppendLine(@"                    AND ARRIVE.BRAND_ID IN :BRAND_ID ");
                            //var brandList = condition.BrandId.Split(',');
                            parameters.Add(":BRAND_ID", condition.BrandId);
                        }
                    }

                    // 仕入先
                    if (!string.IsNullOrEmpty(condition.VendorId))
                    {
                        query.AppendLine(@"                    AND ARRIVE.VENDOR_ID LIKE :VENDOR_ID ");
                        parameters.Add(":VENDOR_ID", condition.VendorId + "%");
                    }

                    // 仕入先名
                    if (string.IsNullOrEmpty(condition.VendorId) && !string.IsNullOrEmpty(condition.VendorName))
                    {
                        query.AppendLine(@"                    AND ARRIVE.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                        parameters.Add(":VENDOR_NAME1", condition.VendorName + "%");
                    }

                    // 分類
                    if (!string.IsNullOrEmpty(condition.CategoryId1))
                    {
                        query.AppendLine(@"                    AND ARRIVE.CATEGORY_ID1 = :CATEGORY_ID1 ");
                        parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId2))
                    {
                        query.AppendLine(@"                    AND ARRIVE.CATEGORY_ID2 = :CATEGORY_ID2 ");
                        parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId3))
                    {
                        query.AppendLine(@"                    AND ARRIVE.CATEGORY_ID3 = :CATEGORY_ID3 ");
                        parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId4))
                    {
                        query.AppendLine(@"                    AND ARRIVE.CATEGORY_ID4 = :CATEGORY_ID4 ");
                        parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                    }

                    //アイテムコード
                    if (!string.IsNullOrEmpty(condition.ItemCode))
                    {
                        query.AppendLine(@"                    AND ARRIVE.ITEM_CODE = :ITEM_CODE ");
                        parameters.Add(":ITEM_CODE", condition.ItemCode);
                    }

                    // 状況
                    if (!string.IsNullOrEmpty(condition.ArrivalStatus))
                    {
                        query.AppendLine(@"                    AND TO_CHAR(ARRIVE.ARRIVAL_STATUS) = :ARRIVAL_STATUS ");
                        parameters.Add(":ARRIVAL_STATUS", condition.ArrivalStatus);
                    }

                    // 納品書番号
                    if (!string.IsNullOrEmpty(condition.InvoiceNo))
                    {
                        query.AppendLine(@"                    AND ARRIVE.INVOICE_NO LIKE :INVOICE_NO ");
                        parameters.Add(":INVOICE_NO", condition.InvoiceNo + "%");
                    }

                    // 発注番号
                    if (!string.IsNullOrEmpty(condition.PoId))
                    {
                        query.AppendLine(@"                    AND ARRIVE.PO_ID LIKE :PO_ID ");
                        parameters.Add(":PO_ID", condition.PoId + "%");
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.AppendLine(@"                    AND ARRIVE.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.AppendLine(@"                    AND ARRIVE.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    // ケースNo別のみ
                    if (condition.ResultType == ResultTypes.BoxNo)
                    {
                        query.AppendLine(@" ) AR ");
                    }
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// 仕入入荷進捗照会ワーク02を取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<PurchaseReference02ResultRow> PurchaseReference02GetData(PurchaseReference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_ARR_PUR_REF02
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<PurchaseReference02ResultRow>(query.ToString(), parameters).Count();

            // 明細
            if (condition.ResultType == ResultTypes.Detail)
            {
                // Sort function
                switch (condition.DetailSort)
                {
                    case PurchaseReference02DetailSort.ArrivePlanDateVendorIdSku:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,BRAND_ID DESC,VENDOR_ID DESC,ITEM_SKU_ID DESC,INVOICE_NO DESC,PO_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,BRAND_ID ASC,VENDOR_ID ASC,ITEM_SKU_ID ASC,INVOICE_NO ASC,PO_ID ASC ");
                                break;
                        }

                        break;
                    case PurchaseReference02DetailSort.VendorIdInvoiceNoSku:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY BRAND_ID DESC,VENDOR_ID DESC,ARRIVE_PLAN_DATE DESC,INVOICE_NO DESC,PO_ID DESC,ITEM_SKU_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY BRAND_ID ASC,VENDOR_ID ASC,ARRIVE_PLAN_DATE ASC,INVOICE_NO ASC,PO_ID ASC,ITEM_SKU_ID ASC ");
                                break;
                        }

                        break;
                    case PurchaseReference02DetailSort.VendorIdSkuInvoiceNo:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY BRAND_ID DESC,VENDOR_ID DESC,ARRIVE_PLAN_DATE DESC,ITEM_SKU_ID DESC,INVOICE_NO DESC,PO_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY BRAND_ID ASC,VENDOR_ID ASC,ARRIVE_PLAN_DATE ASC,ITEM_SKU_ID ASC,INVOICE_NO ASC,PO_ID ASC ");
                                break;
                        }

                        break;
                    case PurchaseReference02DetailSort.ArrivePlanDateVendorIdInvoice:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,BRAND_ID DESC,VENDOR_ID DESC,INVOICE_NO DESC,PO_ID DESC,ITEM_SKU_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,BRAND_ID ASC,VENDOR_ID ASC,INVOICE_NO ASC,PO_ID ASC,ITEM_SKU_ID ASC ");
                                break;
                        }

                        break;
                    default:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,ITEM_SKU_ID DESC,ITEM_ID DESC,VENDOR_ID DESC,INVOICE_NO DESC,PO_ID DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,ITEM_SKU_ID ASC,ITEM_ID ASC,VENDOR_ID ASC,INVOICE_NO ASC,PO_ID ASC ");
                                break;
                        }

                        break;
                }
            }
            else
            {
                // Sort function
                switch (condition.BoxSort)
                {
                    case PurchaseReference02BoxSort.ArrivePlanDateVendorIdSkuBoxNo:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,BRAND_ID DESC,VENDOR_ID DESC,ITEM_SKU_ID DESC,INVOICE_NO DESC,PO_ID DESC,BOX_NO DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,BRAND_ID ASC,VENDOR_ID ASC,ITEM_SKU_ID ASC,INVOICE_NO ASC,PO_ID ASC,BOX_NO ASC ");
                                break;
                        }

                        break;
                    case PurchaseReference02BoxSort.VendorIdInvoiceNoSkuBoxNo:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY BRAND_ID DESC,VENDOR_ID DESC,ARRIVE_PLAN_DATE DESC,INVOICE_NO DESC,PO_ID DESC,ITEM_SKU_ID DESC,BOX_NO DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY BRAND_ID ASC,VENDOR_ID ASC,ARRIVE_PLAN_DATE ASC,INVOICE_NO ASC,PO_ID ASC,ITEM_SKU_ID ASC,BOX_NO ASC ");
                                break;
                        }

                        break;
                    case PurchaseReference02BoxSort.VendorIdSkuInvoiceNoBoxNo:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY BRAND_ID DESC,VENDOR_ID DESC,ARRIVE_PLAN_DATE DESC,ITEM_SKU_ID DESC,INVOICE_NO DESC,PO_ID DESC,BOX_NO DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY BRAND_ID ASC,VENDOR_ID ASC,ARRIVE_PLAN_DATE ASC,ITEM_SKU_ID ASC,INVOICE_NO ASC,PO_ID ASC,BOX_NO ASC ");
                                break;
                        }

                        break;
                    case PurchaseReference02BoxSort.ArrivePlanDateVendorIdInvoiceBoxNo:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,BRAND_ID DESC,VENDOR_ID DESC,INVOICE_NO DESC,PO_ID DESC,ITEM_SKU_ID DESC,BOX_NO DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,BRAND_ID ASC,VENDOR_ID ASC,INVOICE_NO ASC,PO_ID ASC,ITEM_SKU_ID ASC,BOX_NO ASC ");
                                break;
                        }

                        break;
                    default:
                        switch (condition.Sort)
                        {
                            case PurchaseReference02SearchConditions.AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,ITEM_SKU_ID DESC,ITEM_ID DESC,VENDOR_ID DESC,INVOICE_NO DESC,PO_ID DESC,BOX_NO DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,ITEM_SKU_ID ASC,ITEM_ID ASC,VENDOR_ID ASC,INVOICE_NO ASC,PO_ID ASC,BOX_NO ASC ");
                                break;
                        }

                        break;
                }
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var purchaseReference02s = MvcDbContext.Current.Database.Connection.Query<PurchaseReference02ResultRow>(query.ToString(), parameters);
            var arrPurRef02s = MvcDbContext.Current.ArrPurRef02s.Where(x => x.Seq == condition.Seq && x.ShipperId == Common.Profile.User.ShipperId);
            condition.ItemSkuQtySum = arrPurRef02s.Select(x => x.ItemSkuId).Distinct().Count();
            condition.ArrivePlanQtySum = arrPurRef02s.Select(x => x.ArrivePlanQty).Sum() ?? 0;
            condition.ResultQtySum = arrPurRef02s.Select(x => x.ResultQty).Sum() ?? 0;
            condition.PlanSlipNoSum = arrPurRef02s.Select(x => x.InvoiceNo).Distinct().Count();
            condition.ResultSlipNoSum = arrPurRef02s.Where(x => x.ArrivalStatus != "1").Select(x => x.InvoiceNo).Distinct().Count();

            // Excute paging
            return new StaticPagedList<PurchaseReference02ResultRow>(purchaseReference02s, condition.Page, condition.PageSize, totalCount);
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
        /// 格付データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListGrades()
        {
            return MvcDbContext.Current.Grades
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.GradeId,
                    Text = m.GradeName
                })
                .OrderBy(m => m.Text);
        }

        /// <summary>
        /// ブランドデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListBrands()
        {
            return MvcDbContext.Current.Brands
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.BrandId,
                    Text = m.BrandName
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
    }
}