namespace Wms.Areas.Arrival.Query.InputPurchase
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
    using Wms.Areas.Arrival.Models;
    using Wms.Areas.Arrival.ViewModels.InputPurchase;
    using Wms.Areas.Log.Models;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Arrival.ViewModels.InputPurchase.InputPurchase01SearchConditions;

    public class InputPurchaseQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertArrInputPurchase01(InputPurchase01SearchConditions condition)
        {
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_ARR_INPUT_PURCHASE01 (
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
                        ,   PO_ID
                        ,   CATEGORY_NAME1
                        ,   ITEM_ID
                        ,   ITEM_NAME
                        ,   ITEM_SKU_QTY
                        ,   PACKING_PLAN_QTY
                        ,   PACKING_RESULT_QTY
                        ,   ARRIVE_PLAN_QTY
                        ,   RESULT_QTY
                        ,   ARRIVAL_STATUS
                        ,   ARRIVAL_STATUS_NAME
                    )
                    WITH
                        ARRIVE_DATA AS (
                            SELECT
                                    MAX(TAP.ARRIVE_PLAN_DATE) ARRIVE_PLAN_DATE
                                ,   MAX(TAP.VENDOR_ID) VENDOR_ID
                                ,   TAP.INVOICE_NO
                                ,   MAX(TAP.PO_ID) PO_ID
                                ,   MAX(TAP.ITEM_ID) ITEM_ID
                                ,   MAX(TAP.ITEM_SKU_ID) ITEM_SKU_ID
                                ,   COUNT(DISTINCT(TAP.ITEM_SKU_ID)) ITEM_SKU_QTY
                                ,   SUM(TAP.ARRIVE_PLAN_QTY) ARRIVE_PLAN_QTY
                                ,   SUM(NVL(TAR.RESULT_QTY,0)) RESULT_QTY
                                ,   MAX(NVL(TAR.ARRIVAL_STATUS,1)) ARRIVAL_STATUS
                                ,   TAP.CENTER_ID
                                ,   TAP.SHIPPER_ID
                            FROM
                                    T_ARRIVE_PLANS TAP
                            LEFT JOIN
                                    T_ARRIVE_RESULTS TAR
                            ON
                                    TAP.SHIPPER_ID = TAR.SHIPPER_ID
                                AND TAP.CENTER_ID = TAR.CENTER_ID
                                AND TAP.INVOICE_NO = TAR.INVOICE_NO
                                AND TAP.INVOICE_SEQ = TAR.INVOICE_SEQ
                            WHERE
                                    TAP.SHIPPER_ID = :SHIPPER_ID
                            GROUP BY
                                    TAP.SHIPPER_ID
                                ,   TAP.CENTER_ID
                                ,   TAP.INVOICE_NO
                    )
                    ,   PACKING_ARRIVE_DATA AS (
                            SELECT
                                    PLANS.SHIPPER_ID
                                ,   PLANS.CENTER_ID
                                ,   PLANS.INVOICE_NO
                                ,   COUNT(DISTINCT TRIM(PLANS.BOX_NO)) PACKING_PLAN_QTY
                                ,   COUNT(DISTINCT TRIM(RESULTS.BOX_NO)) PACKING_RESULT_QTY
                            FROM
                                    T_PACKING_ARRIVE_PLANS PLANS
                            LEFT JOIN
                                    T_PACKING_ARRIVE_RESULTS RESULTS
                            ON
                                    PLANS.SHIPPER_ID = RESULTS.SHIPPER_ID
                                AND PLANS.CENTER_ID = RESULTS.CENTER_ID
                                AND PLANS.INVOICE_NO = RESULTS.INVOICE_NO
                                AND PLANS.INVOICE_SEQ = RESULTS.INVOICE_SEQ
                            WHERE
                                    PLANS.SHIPPER_ID = :SHIPPER_ID
                            GROUP BY
                                    PLANS.SHIPPER_ID
                                ,   PLANS.CENTER_ID
                                ,   PLANS.INVOICE_NO
                    )
                    ,   ARRIVAL_STATUS AS (
                            SELECT
                                    MG.SHIPPER_ID
                                ,   MG.GEN_CD
                                ,   MG.GEN_NAME ARRIVAL_STATUS_NAME
                            FROM
                                    M_GENERALS MG
                            WHERE
                                    MG.REGISTER_DIVI_CD = '1'
                                AND MG.CENTER_ID = '@@@'
                                AND MG.GEN_DIV_CD = 'ARRIVAL_STATUS'
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
                        ,   ROWNUM
                        ,   ARRIVE.CENTER_ID
                        ,   ARRIVE.ARRIVE_PLAN_DATE
                        ,   ARRIVE.VENDOR_ID
                        ,   MV.VENDOR_NAME1 VENDOR_NAME
                        ,   ARRIVE.INVOICE_NO
                        ,   ARRIVE.PO_ID
                        ,   MIC.CATEGORY_NAME1
                        ,   ARRIVE.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   ARRIVE.ITEM_SKU_QTY
                        ,   CASE
                                WHEN PACKING_ARRIVE.PACKING_PLAN_QTY = 0 THEN NULL
                                ELSE PACKING_ARRIVE.PACKING_PLAN_QTY
                            END AS PACKING_PLAN_QTY
                        ,   CASE
                                WHEN PACKING_ARRIVE.PACKING_RESULT_QTY = 0 THEN NULL
                                ELSE PACKING_ARRIVE.PACKING_RESULT_QTY
                            END AS PACKING_RESULT_QTY
                        ,   CASE
                                WHEN ARRIVE.ARRIVE_PLAN_QTY = 0 THEN NULL
                                ELSE ARRIVE.ARRIVE_PLAN_QTY
                            END AS ARRIVE_PLAN_QTY
                        ,   CASE
                                WHEN ARRIVE.RESULT_QTY = 0 THEN NULL
                                ELSE ARRIVE.RESULT_QTY
                            END AS RESULT_QTY
                        ,   ARRIVE.ARRIVAL_STATUS
                        ,   ARRIVAL_STATUS.ARRIVAL_STATUS_NAME 
                    FROM 
                            ARRIVE_DATA ARRIVE
                    LEFT JOIN
                            PACKING_ARRIVE_DATA PACKING_ARRIVE
                    ON
                            ARRIVE.SHIPPER_ID = PACKING_ARRIVE.SHIPPER_ID
                        AND ARRIVE.CENTER_ID = PACKING_ARRIVE.CENTER_ID
                        AND ARRIVE.INVOICE_NO = PACKING_ARRIVE.INVOICE_NO
                    LEFT JOIN
                            M_VENDORS MV
                    ON
                            ARRIVE.SHIPPER_ID = MV.SHIPPER_ID
                        AND ARRIVE.VENDOR_ID = MV.VENDOR_ID
                    LEFT JOIN
                            M_ITEM_SKU MIS
                    ON
                            ARRIVE.SHIPPER_ID = MIS.SHIPPER_ID
                        AND ARRIVE.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                    LEFT JOIN
                            M_ITEM_CATEGORIES4 MIC
                    ON
                            MIS.SHIPPER_ID = MIC.SHIPPER_ID
                        AND MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1
                        AND MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2
                        AND MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3
                        AND MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4
                    LEFT JOIN
                            ARRIVAL_STATUS
                    ON
                            ARRIVE.SHIPPER_ID = ARRIVAL_STATUS.SHIPPER_ID
                        AND TO_CHAR(ARRIVE.ARRIVAL_STATUS) = ARRIVAL_STATUS.GEN_CD
                    ");
                    query.Append(" WHERE ARRIVE.SHIPPER_ID   = :SHIPPER_ID ");
                    query.Append("   AND ARRIVE.ARRIVAL_STATUS NOT IN (4,5) "); // 4:入荷確定済と5:実績送信済は表示しない

                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertArrInputPurchase01");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

                    // Add search condition
                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.Append(" AND ARRIVE.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

                    // 入荷予定日日(検索・From-To)
                    if (condition.ArrivePlanDateFrom != null)
                    {
                        query.Append(" AND ARRIVE.ARRIVE_PLAN_DATE >= :ARRIVE_PLAN_DATE_FROM ");
                        parameters.Add(":ARRIVE_PLAN_DATE_FROM", condition.ArrivePlanDateFrom);
                    }
                    if (condition.ArrivePlanDateTo != null)
                    {
                        query.Append(" AND ARRIVE.ARRIVE_PLAN_DATE <= :ARRIVE_PLAN_DATE_TO ");
                        parameters.Add(":ARRIVE_PLAN_DATE_TO", condition.ArrivePlanDateTo);
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

                    // 仕入先
                    if (!string.IsNullOrEmpty(condition.VendorId))
                    {
                        query.Append(" AND ARRIVE.VENDOR_ID LIKE :VENDOR_ID ");
                        parameters.Add(":VENDOR_ID", condition.VendorId + "%");
                    }

                    // 仕入先名
                    if (string.IsNullOrEmpty(condition.VendorId) && !string.IsNullOrEmpty(condition.VendorName))
                    {
                        query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                        parameters.Add(":VENDOR_NAME1", condition.VendorName + "%");
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

                    // 状況
                    if (!string.IsNullOrEmpty(condition.ArrivalStatus))
                    {
                        query.Append(" AND TO_CHAR(ARRIVE.ARRIVAL_STATUS) = :ARRIVAL_STATUS ");
                        parameters.Add(":ARRIVAL_STATUS", condition.ArrivalStatus);
                    }

                    // 納品書番号
                    if (!string.IsNullOrEmpty(condition.InvoiceNo))
                    {
                        query.Append(" AND ARRIVE.INVOICE_NO LIKE :INVOICE_NO ");
                        parameters.Add(":INVOICE_NO", condition.InvoiceNo + "%");
                    }

                    // 発注番号
                    if (!string.IsNullOrEmpty(condition.PoId))
                    {
                        query.Append(" AND ARRIVE.PO_ID LIKE :PO_ID ");
                        parameters.Add(":PO_ID", condition.PoId + "%");
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.Append(" AND ARRIVE.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "InsertArrInputPurchase01");
                    trans.Rollback();
                    return false;
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
        public IPagedList<InputPurchase01ResultRow> InputPurchase01GetData(InputPurchase01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT
                        *
                FROM
                        WW_ARR_INPUT_PURCHASE01
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<InputPurchase01ResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case InputPurchase01Sort.ArrivePlanDateVendorIdInvoiceNo:
                    switch (condition.Sort)
                    {
                        case InputPurchase01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,VENDOR_ID DESC,INVOICE_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,VENDOR_ID ASC,INVOICE_NO ASC ");
                            break;
                    }

                    break;
                case InputPurchase01Sort.VendorNameInvoiceNo:
                    switch (condition.Sort)
                    {
                        case InputPurchase01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY VENDOR_NAME DESC,INVOICE_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY VENDOR_NAME ASC,INVOICE_NO ASC ");
                            break;
                    }

                    break;
                case InputPurchase01Sort.ItemIdInvoiceNo:
                    switch (condition.Sort)
                    {
                        case InputPurchase01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ITEM_ID DESC,INVOICE_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ITEM_ID ASC,INVOICE_NO ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case InputPurchase01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,VENDOR_ID DESC,ITEM_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,VENDOR_ID ASC,ITEM_ID ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var inputPurchase01s = MvcDbContext.Current.Database.Connection.Query<InputPurchase01ResultRow>(query.ToString(), parameters);
            var arrInputPurchase01s = MvcDbContext.Current.ArrInputPurchase01s.Where(x => x.Seq == condition.Seq && x.ShipperId == Common.Profile.User.ShipperId);
            condition.ItemSkuQtySum = arrInputPurchase01s.Select(x => x.ItemSkuQty).Sum();
            condition.ArrivePlanQtySum = arrInputPurchase01s.Select(x => x.ArrivePlanQty).Sum();
            condition.ResultQtySum = arrInputPurchase01s.Select(x => x.ResultQty).Sum();
            condition.InvoicePlanQtySum = arrInputPurchase01s.Select(x => x.InvoiceNo).Count();
            condition.InvoiceResultQtySum = arrInputPurchase01s.Where(x => x.ResultQty != null).Select(x => x.InvoiceNo).Count();

            // Excute paging
            return new StaticPagedList<InputPurchase01ResultRow>(inputPurchase01s, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public InputPurchase02SearchConditions InsertArrInputPurchase02(SelectedInputPurchase01ViewModel selected, bool fromPurchaseReference)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    var rtn = new InputPurchase02SearchConditions();
                    rtn.FromPurchaseReference = fromPurchaseReference;

                    if (selected.Seq == 0 && selected.LineNo == 0)
                    {
                        rtn.CenterId = selected.CenterId;
                        rtn.Seq = new BaseQuery().GetWorkId();
                        rtn.ArrivePlanDate = selected.ArrivePlanDate;
                        rtn.VendorId = selected.VendorId;
                        rtn.VendorName = selected.VendorName;
                        rtn.InvoiceNo = selected.InvoiceNo;
                        rtn.PoId = selected.PoId;
                    }
                    else
                    {
                        var inputPurchase01 = MvcDbContext.Current.ArrInputPurchase01s.Where(x => x.Seq == selected.Seq && x.LineNo == selected.LineNo && x.ShipperId == Common.Profile.User.ShipperId).FirstOrDefault();
                        rtn.Seq = new BaseQuery().GetWorkId();
                        rtn.CenterId = inputPurchase01.CenterId;
                        rtn.ArrivePlanDate = inputPurchase01.ArrivePlanDate;
                        rtn.VendorId = inputPurchase01.VendorId;
                        rtn.VendorName = inputPurchase01.VendorName;
                        rtn.InvoiceNo = inputPurchase01.InvoiceNo;
                        rtn.PoId = inputPurchase01.PoId;
                    }
                    
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_ARR_INPUT_PURCHASE02 (
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
                            ,   PO_ID
                            ,   INVOICE_SEQ
                            ,   PLAN_BOX_NO
                            ,   RESULT_BOX_NO
                            ,   CASE_CLASS
                            ,   TCDC_CLASS
                            ,   CATEGORY_ID1
                            ,   CATEGORY_NAME1
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_COLOR_NAME
                            ,   ITEM_SIZE_ID
                            ,   ITEM_SIZE_NAME
                            ,   JAN
                            ,   ITEM_SKU_ID
                            ,   PACKING_PLAN_QTY
                            ,   ARRIVE_PLAN_QTY
                            ,   PACKING_RESULT_QTY
                            ,   RESULT_UPDATE_COUNT
                            ,   JAN_QTY
                            ,   PLAN_BOX_SEQ
                            ,   RESULT_BOX_SEQ
                            ,   INPUT_RESULT_QTY
                            ,   DISP_BOX_NO
                        )
                        WITH
                            PACKING_ARRIVE_PLAN_JAN_QTY AS (
                                SELECT
                                        SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   INVOICE_NO
                                    ,   INVOICE_SEQ
                                    ,   BOX_NO
                                    ,   COUNT(DISTINCT(JAN)) JAN_QTY
                                FROM
                                        T_PACKING_ARRIVE_PLANS
                                WHERE
                                        SHIPPER_ID = :SHIPPER_ID
                                GROUP BY
                                        SHIPPER_ID
                                    ,   CENTER_ID
                                    ,   INVOICE_NO
                                    ,   INVOICE_SEQ
                                    ,   BOX_NO
                        )
                        ,   PACKING_ARRIVE AS (
                                SELECT
                                        NVL(PLAN.SHIPPER_ID,RESULT.SHIPPER_ID) SHIPPER_ID
                                    ,   NVL(PLAN.CENTER_ID,RESULT.CENTER_ID) CENTER_ID
                                    ,   NVL(PLAN.INVOICE_NO,RESULT.INVOICE_NO) INVOICE_NO
                                    ,   NVL(PLAN.INVOICE_SEQ,RESULT.INVOICE_SEQ) INVOICE_SEQ
                                    ,   PLAN.BOX_NO PLAN_BOX_NO
                                    ,   RESULT.BOX_NO RESULT_BOX_NO
                                    ,   RESULT.CASE_CLASS
                                    ,   RESULT.TCDC_CLASS
                                    ,   PLAN.PACKING_PLAN_QTY
                                    ,   RESULT.PACKING_RESULT_QTY
                                    ,   PLAN.BOX_SEQ PLAN_BOX_SEQ
                                    ,   RESULT.BOX_SEQ RESULT_BOX_SEQ
                                    ,   RESULT.DISP_BOX_NO DISP_BOX_NO
                                FROM
                                        T_PACKING_ARRIVE_PLANS PLAN
                                FULL OUTER JOIN --予定なしかつ実績ありのデータも取得する
                                        T_PACKING_ARRIVE_RESULTS RESULT
                                ON
                                        PLAN.SHIPPER_ID = RESULT.SHIPPER_ID
                                    AND PLAN.CENTER_ID = RESULT.CENTER_ID
                                    AND PLAN.INVOICE_NO = RESULT.INVOICE_NO
                                    AND PLAN.INVOICE_SEQ = RESULT.INVOICE_SEQ
                                    AND PLAN.BOX_NO = RESULT.BOX_NO
                        )
                        SELECT
                                SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   0
                            ,   TAP.SHIPPER_ID
                            ,   :SEQ
                            ,   ROWNUM
                            ,   TAP.CENTER_ID
                            ,   TAP.ARRIVE_PLAN_DATE
                            ,   TAP.VENDOR_ID
                            ,   MV.VENDOR_NAME1 VENDOR_NAME
                            ,   TAP.INVOICE_NO
                            ,   TAP.PO_ID
                            ,   TAP.INVOICE_SEQ
                            ,   PA.PLAN_BOX_NO
                            ,   PA.RESULT_BOX_NO
                            ,   PA.CASE_CLASS
                            ,   PA.TCDC_CLASS
                            ,   MIS.CATEGORY_ID1
                            ,   MIC.CATEGORY_NAME1
                            ,   TAP.ITEM_ID
                            ,   MIS.ITEM_NAME
                            ,   TAP.ITEM_COLOR_ID
                            ,   MC.ITEM_COLOR_NAME
                            ,   TAP.ITEM_SIZE_ID
                            ,   MS.ITEM_SIZE_NAME
                            ,   TAP.JAN
                            ,   TAP.ITEM_SKU_ID
                            ,   PA.PACKING_PLAN_QTY
                            ,   TAP.ARRIVE_PLAN_QTY
                            ,   PA.PACKING_RESULT_QTY
                            ,   TAR.UPDATE_COUNT RESULT_UPDATE_COUNT
                            ,   PAP.JAN_QTY
                            ,   PA.PLAN_BOX_SEQ
                            ,   PA.RESULT_BOX_SEQ
                            ,   PA.PACKING_RESULT_QTY AS INPUT_RESULT_QTY
                            ,   PA.DISP_BOX_NO
                        FROM
                                T_ARRIVE_PLANS TAP
                        LEFT JOIN
                                T_ARRIVE_RESULTS TAR
                        ON
                                TAP.SHIPPER_ID = TAR.SHIPPER_ID
                            AND TAP.CENTER_ID = TAR.CENTER_ID
                            AND TAP.INVOICE_NO = TAR.INVOICE_NO
                            AND TAP.INVOICE_SEQ = TAR.INVOICE_SEQ
                        LEFT JOIN
                                PACKING_ARRIVE PA
                        ON
                                TAP.SHIPPER_ID = PA.SHIPPER_ID
                            AND TAP.CENTER_ID = PA.CENTER_ID
                            AND TAP.INVOICE_NO = PA.INVOICE_NO
                            AND TAP.INVOICE_SEQ = PA.INVOICE_SEQ
                        LEFT JOIN
                                M_VENDORS MV
                        ON
                                TAP.SHIPPER_ID = MV.SHIPPER_ID
                            AND TAP.VENDOR_ID = MV.VENDOR_ID
                        LEFT JOIN
                                M_ITEM_SKU MIS
                        ON
                                TAP.SHIPPER_ID = MIS.SHIPPER_ID
                            AND TAP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                        LEFT JOIN
                                M_COLORS MC
                        ON
                                TAP.SHIPPER_ID = MC.SHIPPER_ID
                            AND TAP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                        LEFT JOIN
                                M_SIZES MS
                        ON
                                TAP.SHIPPER_ID = MS.SHIPPER_ID
                            AND TAP.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                        LEFT JOIN
                                M_ITEM_CATEGORIES4 MIC
                        ON
                                MIS.SHIPPER_ID = MIC.SHIPPER_ID
                            AND MIS.CATEGORY_ID1 = MIC.CATEGORY_ID1
                            AND MIS.CATEGORY_ID2 = MIC.CATEGORY_ID2
                            AND MIS.CATEGORY_ID3 = MIC.CATEGORY_ID3
                            AND MIS.CATEGORY_ID4 = MIC.CATEGORY_ID4
                        LEFT JOIN
                                PACKING_ARRIVE_PLAN_JAN_QTY PAP
                        ON
                                PA.SHIPPER_ID = PAP.SHIPPER_ID
                            AND PA.CENTER_ID = PAP.CENTER_ID
                            AND PA.INVOICE_NO = PAP.INVOICE_NO
                            AND PA.INVOICE_SEQ = PAP.INVOICE_SEQ
                            AND PA.PLAN_BOX_NO = PAP.BOX_NO
                        WHERE
                                TAP.SHIPPER_ID = :SHIPPER_ID
                            AND TAP.CENTER_ID = :CENTER_ID
                            AND TAP.INVOICE_NO = :INVOICE_NO
                    ");
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertArrInputPurchase02");
                    parameters.Add(":SEQ", rtn.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", rtn.CenterId);
                    parameters.Add(":INVOICE_NO", rtn.InvoiceNo);

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);

                    trans.Commit();
                    return rtn;
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "InsertArrInputPurchase02");
                    return null;
                }
            }
        }

        /// <summary>
        /// 仕入入荷実績入力ワーク02から対象のデータを取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public List<InputPurchase02ResultRow> InputPurchase02GetData(InputPurchase02SearchConditions condition)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();

                StringBuilder query = new StringBuilder(@"
                    WITH
                        PLAN_QTY AS ( --予定梱包番号単位で梱包予定または梱包実績があるデータの予定数を取得
                            SELECT
                                    ROW_NUMBER() OVER (PARTITION BY WW.SHIPPER_ID , WW.CENTER_ID , WW.INVOICE_NO , WW.INVOICE_SEQ , WW.PLAN_BOX_NO ORDER BY WW.TCDC_CLASS ASC , WW.CASE_CLASS ASC) ROWNUMBER
                                ,   WW.SHIPPER_ID
                                ,   WW.CENTER_ID
                                ,   WW.INVOICE_NO
                                ,   WW.INVOICE_SEQ
                                ,   WW.PLAN_BOX_NO
                                ,   WW.CASE_CLASS
                                ,   WW.TCDC_CLASS
                                ,   WW.RESULT_BOX_NO
                                ,   WW.ARRIVE_PLAN_QTY
                                ,   CASE
                                        WHEN WW.PLAN_BOX_NO IS NOT NULL AND WW.RESULT_BOX_NO IS NULL THEN 0
                                        WHEN WW.PLAN_BOX_NO IS NOT NULL AND WW.RESULT_BOX_NO IS NOT NULL THEN 1
                                        WHEN WW.PLAN_BOX_NO IS NULL AND WW.RESULT_BOX_NO IS NOT NULL THEN 2
                                    END AS PACKING_STATUS
                                ,   WW.DISP_BOX_NO
                            FROM
                                    WW_ARR_INPUT_PURCHASE02 WW
                            WHERE
                                    WW.SHIPPER_ID = :SHIPPER_ID
                                AND WW.SEQ = :SEQ
                    )
                    ,   FIRST_PLAN_QTY AS ( --1行目を取得
                            SELECT
                                    SHIPPER_ID
                                ,   CENTER_ID
                                ,   INVOICE_NO
                                ,   INVOICE_SEQ
                                ,   PLAN_BOX_NO
                                ,   ARRIVE_PLAN_QTY
                                ,   PACKING_STATUS
                                ,   DISP_BOX_NO
                            FROM
                                    PLAN_QTY
                            WHERE
                                    ROWNUMBER = 1
                    )
                    SELECT
                            WW.UPDATE_COUNT
                        ,   WW.SHIPPER_ID
                        ,   WW.SEQ
                        ,   WW.LINE_NO
                        ,   WW.CENTER_ID
                        ,   WW.ARRIVE_PLAN_DATE
                        ,   WW.VENDOR_ID
                        ,   WW.VENDOR_NAME
                        ,   WW.INVOICE_NO
                        ,   WW.PO_ID
                        ,   WW.INVOICE_SEQ
                        ,   CASE
                                WHEN WW.DISP_BOX_NO IS NULL THEN WW.PLAN_BOX_NO
                                WHEN TRIM(WW.DISP_BOX_NO) IS NOT NULL THEN WW.DISP_BOX_NO
                                WHEN WW.DISP_BOX_NO = ' ' AND WW.TCDC_CLASS = 1 THEN CAST('(TC)' AS NVARCHAR2(72))
                                WHEN WW.DISP_BOX_NO = ' ' AND WW.TCDC_CLASS = 2 THEN CAST('(DCバラ)' AS NVARCHAR2(72))
                                ELSE NULL
                            END BOX_NO
                        ,   WW.PLAN_BOX_NO
                        ,   WW.RESULT_BOX_NO
                        ,   WW.CASE_CLASS
                        ,   WW.TCDC_CLASS
                        ,   WW.CATEGORY_ID1
                        ,   WW.CATEGORY_NAME1
                        ,   WW.ITEM_ID
                        ,   WW.ITEM_NAME
                        ,   WW.ITEM_COLOR_ID
                        ,   WW.ITEM_COLOR_NAME
                        ,   WW.ITEM_SIZE_ID
                        ,   WW.ITEM_SIZE_NAME
                        ,   WW.JAN
                        ,   WW.ITEM_SKU_ID
                        ,   WW.PACKING_PLAN_QTY
                        ,   WW.ARRIVE_PLAN_QTY
                        ,   CASE
                                --予定梱包番号番号単位でTCDC区分、荷姿区分の昇順にしたとき
                                --1行目がTCバラの場合、2行目以降はブランク
                                WHEN PLAN_QTY.ROWNUMBER > 1 AND FIRST_PLAN_QTY.DISP_BOX_NO = ' ' THEN NULL
                                --1行目が梱包予定なしの場合、2行目以降はブランク
                                WHEN PLAN_QTY.ROWNUMBER > 1 AND FIRST_PLAN_QTY.PACKING_STATUS = 2 THEN NULL
                                --予定梱包番号単位ですべて梱包予定なしの場合、入荷予定TBL.予定数を表示
                                WHEN PLAN_QTY.ROWNUMBER IS NULL THEN WW.ARRIVE_PLAN_QTY
                                --上記以外（1行目のとき・2行目以降で1行目が梱包予定ありのときetc）
                                ELSE NVL(WW.PACKING_PLAN_QTY,WW.ARRIVE_PLAN_QTY)
                            END AS PLAN_QTY
                        ,   WW.PACKING_RESULT_QTY
                        ,   WW.RESULT_UPDATE_COUNT
                        ,   WW.JAN_QTY
                        ,   WW.PLAN_BOX_SEQ
                        ,   WW.RESULT_BOX_SEQ
                        ,   WW.INPUT_RESULT_QTY
                    FROM
                            WW_ARR_INPUT_PURCHASE02 WW
                    LEFT OUTER JOIN
                            PLAN_QTY
                    ON
                            WW.SHIPPER_ID = PLAN_QTY.SHIPPER_ID
                        AND WW.CENTER_ID = PLAN_QTY.CENTER_ID
                        AND WW.INVOICE_NO = PLAN_QTY.INVOICE_NO
                        AND WW.INVOICE_SEQ = PLAN_QTY.INVOICE_SEQ
                        AND NVL(WW.PLAN_BOX_NO,'0') = NVL(PLAN_QTY.PLAN_BOX_NO,'0')
                        AND NVL(WW.DISP_BOX_NO,'0') = NVL(PLAN_QTY.DISP_BOX_NO,'0')
                        AND NVL(WW.CASE_CLASS,0) = NVL(PLAN_QTY.CASE_CLASS,0)
                        AND NVL(WW.TCDC_CLASS,0) =  NVL(PLAN_QTY.TCDC_CLASS,0)
                    LEFT OUTER JOIN
                            FIRST_PLAN_QTY
                    ON
                            WW.SHIPPER_ID = FIRST_PLAN_QTY.SHIPPER_ID
                        AND WW.CENTER_ID = FIRST_PLAN_QTY.CENTER_ID
                        AND WW.INVOICE_NO = FIRST_PLAN_QTY.INVOICE_NO
                        AND WW.INVOICE_SEQ = FIRST_PLAN_QTY.INVOICE_SEQ
                        AND NVL(WW.PLAN_BOX_NO,'0') = NVL(FIRST_PLAN_QTY.PLAN_BOX_NO,'0')
                    WHERE
                            WW.SHIPPER_ID = :SHIPPER_ID
                        AND WW.SEQ = :SEQ
                    ORDER BY
                            WW.INVOICE_SEQ ASC
                        ,   WW.PLAN_BOX_NO ASC
                        ,   WW.TCDC_CLASS ASC
                        ,   WW.CASE_CLASS ASC
                        ,   WW.RESULT_BOX_NO ASC
                ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // Fill data to memory
                var arrInputPurchase02s = MvcDbContext.Current.Database.Connection.Query<InputPurchase02ResultRow>(query.ToString(), parameters);
                condition.ItemSkuQtySum = arrInputPurchase02s.Select(x => x.ItemSkuId).Distinct().Count();
                condition.ArrivePlanQtySum = arrInputPurchase02s.Select(x => x.PlanQty).Sum();
                condition.ResultQtySum = arrInputPurchase02s.Select(x => x.PackingResultQty).Sum();
                // Excute paging
                return MvcDbContext.Current.Database.Connection.Query<InputPurchase02ResultRow>(query.ToString(), parameters).ToList();
            }
            catch (Exception ex) //デバッグ用
            {
                Mvc.Common.AppError.PutLogREF(ex, "InputPurchase02GetData");
                return null;
            }
        }

        /// <summary>
        /// ワーク02更新
        /// </summary>
        /// <param name="inputPurchase02s"></param>
        /// <returns></returns>
        public bool UpdateArrInputPurchase02(IList<InputPurchase02ResultRow> inputPurchase02s)
        {
            try
            {
                var dbContext = MvcDbContext.Current;
                using (var trans = dbContext.Database.BeginTransaction())
                {
                    foreach (var u in inputPurchase02s)
                    {
                        var inputPurchase02 =
                       dbContext.ArrInputPurchase02s
                       .Where(m => m.ShipperId == u.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                       .SingleOrDefault();
                        if (inputPurchase02 == null)
                        {
                            return false;
                        }

                        inputPurchase02.SetBaseInfoUpdate();
                        inputPurchase02.InputResultQty = u.InputResultQty;

                        //更新
                        try
                        {
                            //save data to DB
                            dbContext.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return false;
                        }
                    }

                    trans.Commit();
                }
                //return
                return true;
            }
            catch (Exception ex) //デバッグ用
            {
                Mvc.Common.AppError.PutLogREF(ex, "UpdateArrInputPurchase02");
                return false;
            }
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void UpdateInputPurchase(InputPurchase02Result result, int isConfirmed, out ProcedureStatus status, out string message)
        {
            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var param = new DynamicParameters();
                    param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
                    param.Add("IN_CENTER_ID", result.InputPurchase02s.FirstOrDefault().CenterId, DbType.String, ParameterDirection.Input);
                    param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
                    param.Add("IN_SEQ", result.InputPurchase02s.FirstOrDefault().Seq, DbType.Int32, ParameterDirection.Input);
                    param.Add("IN_IS_CONFIRMED", isConfirmed, DbType.Int32, ParameterDirection.Input);
                    param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                    var db = MvcDbContext.Current.Database;

                    db.Connection.Execute(
                        "PK_W_ARR_INPUTPURCHASE02.UPDATE_RESULT",
                        param,
                        commandType: CommandType.StoredProcedure);

                    status = param.Get<ProcedureStatus>("OUT_STATUS");
                    message = param.Get<string>("OUT_MESSAGE");

                    if (status == ProcedureStatus.Success)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                    }
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "UpdateInputPurchase");
                    trans.Rollback();
                    status = ProcedureStatus.Error;
                    message = "error";
                }
            }
        }

        /// <summary>
        /// 入荷状況取得（「入荷確定済」「実績送信済」は表示しない）
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListArriveStatus()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        GEN_CD AS VALUE
                    ,   GEN_NAME AS TEXT
                FROM
                        M_GENERALS
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = '@@@'
                    AND REGISTER_DIVI_CD = '1'
                    AND GEN_DIV_CD  = 'ARRIVAL_STATUS'
                    AND GEN_CD NOT IN ('4','5') --4:入荷確定済、5:実績送信済
                ORDER BY
                        ORDER_NO
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // 全レコード数を取得
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
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
    }
}