namespace Wms.Areas.Returns.Query.PurchaseReturns
{
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
    using Wms.Areas.Returns.Models;
    using Wms.Areas.Returns.Resources;
    using Wms.Areas.Returns.ViewModels.PurchaseReturns;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Returns.ViewModels.PurchaseReturns.PurchaseReturnsSearchConditions;

    public class PurchaseReturnsQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertPurchaseReturns(PurchaseReturnsSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    var sql = $@"
                        INSERT INTO WW_RET_PURCHASE_RETURNS (
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
                            ,   ITEM_SKU_ID
                            ,   LOCATION_CD
                            ,   BUYING_PRICE
                            ,   INVOICE_NO
                            ,   ARRIVE_QTY
                        )
                        WITH
                            STOCK_DATA AS (
                                SELECT
                                        TS.SHIPPER_ID
                                    ,   TS.CENTER_ID
                                    ,   TS.ITEM_SKU_ID
                                    ,   TS.LOCATION_CD
                                    ,   TS.STOCK_QTY
                                FROM
                                        T_STOCKS TS
                                WHERE
                                        TS.SHIPPER_ID = :SHIPPER_ID
                                    AND TS.CENTER_ID = :CENTER_ID
                                    AND TS.LOCATION_CD = SF_GET_FIXED_LOCATION(:SHIPPER_ID, :CENTER_ID, :MAKE_USER_ID, 'MKH')
                                    {((condition.ItemId != null)? "AND TS.ITEM_ID LIKE :ITEM_ID || '%'" : null)}                    
                                    {((condition.Jan != null)? "AND TS.JAN LIKE :JAN || '%'" : null)}
                                    {((condition.ItemSkuId != null)? "AND TS.ITEM_SKU_ID LIKE :ITEM_SKU_ID || '%'" : null)}
                            )
                            ,ARRIVE_RESULT_DATA AS (
                                SELECT
                                        ARR.*
                                    ,   ROW_NUMBER() OVER(
                                                    PARTITION BY 
                                                            ARR.SHIPPER_ID
                                                        ,   ARR.CENTER_ID
                                                        ,   ARR.ITEM_SKU_ID
                                                    ORDER BY
                                                            ARR.CONFIRM_DATE DESC
                                                ) AS ORDER_NO
                                FROM (
                                    SELECT
                                            TAR.SHIPPER_ID
                                        ,   TAR.CENTER_ID
                                        ,   TAR.INVOICE_NO
                                        ,   TAR.ITEM_SKU_ID
                                        ,   TAR.RESULT_QTY
                                        ,   TAR.CONFIRM_DATE
                                        ,   TAP.NORMAL_BUYING_PRICE
                                    FROM
                                            T_ARRIVE_RESULTS TAR
                                    INNER JOIN
                                            STOCK_DATA STK
                                    ON
                                            STK.ITEM_SKU_ID = TAR.ITEM_SKU_ID
                                        AND STK.CENTER_ID = TAR.CENTER_ID
                                        AND STK.SHIPPER_ID = TAR.SHIPPER_ID
                                    INNER JOIN
                                            T_ARRIVE_PLANS TAP
                                    ON
                                            TAP.INVOICE_NO = TAR.INVOICE_NO
                                        AND TAP.INVOICE_SEQ = TAR.INVOICE_SEQ
                                        AND TAP.CENTER_ID = TAR.CENTER_ID
                                        AND TAP.SHIPPER_ID = TAR.SHIPPER_ID
                                    WHERE
                                            TAR.SHIPPER_ID = :SHIPPER_ID
                                        AND TAR.CENTER_ID = :CENTER_ID
                                        AND TAR.IF_STATE = 2                   --実績送信済

                                    UNION ALL

                                    SELECT
                                            TAR.SHIPPER_ID
                                        ,   TAR.CENTER_ID
                                        ,   TAR.INVOICE_NO
                                        ,   TAR.ITEM_SKU_ID
                                        ,   TAR.RESULT_QTY
                                        ,   TAR.CONFIRM_DATE
                                        ,   TAP.NORMAL_BUYING_PRICE
                                    FROM
                                            A_ARRIVE_RESULTS TAR
                                    INNER JOIN
                                            STOCK_DATA STK
                                    ON
                                            STK.ITEM_SKU_ID = TAR.ITEM_SKU_ID
                                        AND STK.CENTER_ID = TAR.CENTER_ID
                                        AND STK.SHIPPER_ID = TAR.SHIPPER_ID
                                    INNER JOIN
                                            A_ARRIVE_PLANS TAP
                                    ON
                                            TAP.INVOICE_NO = TAR.INVOICE_NO
                                        AND TAP.INVOICE_SEQ = TAR.INVOICE_SEQ
                                        AND TAP.CENTER_ID = TAR.CENTER_ID
                                        AND TAP.SHIPPER_ID = TAR.SHIPPER_ID
                                    WHERE
                                            TAR.SHIPPER_ID = :SHIPPER_ID
                                        AND TAR.CENTER_ID = :CENTER_ID
                                ) ARR
                            )
                        SELECT
                                SYSTIMESTAMP
                            ,   :MAKE_USER_ID
                            ,   'PurchaseReturns'
                            ,   SYSTIMESTAMP
                            ,   :MAKE_USER_ID
                            ,   'PurchaseReturns'
                            ,   0
                            ,   '{Common.Profile.User.ShipperId}'
                            ,   {condition.Seq}
                            ,   ROWNUM
                            ,   TS.CENTER_ID
                            ,   TS.ITEM_SKU_ID
                            ,   TS.LOCATION_CD
                            ,   AR.NORMAL_BUYING_PRICE
                            ,   AR.INVOICE_NO
                            ,   AR.RESULT_QTY
                        FROM
                                STOCK_DATA TS
                        INNER JOIN
                                ARRIVE_RESULT_DATA AR
                        ON
                                AR.ITEM_SKU_ID = TS.ITEM_SKU_ID
                            AND AR.CENTER_ID = TS.CENTER_ID
                            AND AR.SHIPPER_ID = TS.SHIPPER_ID
                            AND AR.ORDER_NO = 1                                
                    ";

                    parameters.AddDynamicParams(
                        new
                        {
                            SHIPPER_ID = Common.Profile.User.ShipperId,
                            CENTER_ID = condition.CenterId,
                            MAKE_USER_ID = Common.Profile.User.UserId,
                            ITEM_ID = condition.ItemId,
                            JAN = condition.Jan,
                            ITEM_SKU_ID = condition.ItemSkuId
                        });

                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Execute(sql, parameters);
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
        /// GetData from Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<PurchaseReturnsResultRow> GetPurchaseReturns(PurchaseReturnsSearchConditions condition)
        {
            try
            {
                string order;
                // Sort function
                switch (condition.SortKey)
                {
                    case DataSortKey.Sku:
                        switch (condition.Sort)
                        {
                            case PurchaseReturnsSearchConditions.AscDescSort.Desc:
                                order = "ORDER BY TS.ITEM_SKU_ID DESC";
                                break;

                            default:
                                order = "ORDER BY TS.ITEM_SKU_ID ASC";
                                break;
                        }

                        break;
                    default:
                        switch (condition.Sort)
                        {
                            case PurchaseReturnsSearchConditions.AscDescSort.Desc:
                                order = "ORDER BY TS.JAN DESC ";
                                break;

                            default:
                                order = "ORDER BY TS.JAN ASC";
                                break;
                        }

                        break;
                }
                DynamicParameters parameters = new DynamicParameters();
                var sql = $@"
                    WITH
                        WORK_DATA AS (
                            SELECT
                                    *
                            FROM
                                    WW_RET_PURCHASE_RETURNS PR
                            WHERE
                                    PR.SEQ = :SEQ
                                AND PR.SHIPPER_ID = :SHIPPER_ID
                                AND PR.CENTER_ID = :CENTER_ID
                        )
                        ,ARRIVE_TUNE_DATA AS (
                            SELECT
                                    TMR.SHIPPER_ID
                                ,   TMR.CENTER_ID
                                ,   TMR.INVOICE_NO
                                ,   TMR.ITEM_SKU_ID
                                ,   SUM(TMR.RETURN_QTY) AS TUNE_QTY
                            FROM
                                    T_MKRETURN_RESULTS TMR
                            INNER JOIN
                                    WORK_DATA WK
                            ON
                                    WK.ITEM_SKU_ID = TMR.ITEM_SKU_ID
                                AND WK.INVOICE_NO = TMR.INVOICE_NO
                                AND WK.CENTER_ID = TMR.CENTER_ID
                                AND WK.SHIPPER_ID = TMR.SHIPPER_ID
                            WHERE
                                    TMR.SHIPPER_ID = :SHIPPER_ID
                                AND TMR.CENTER_ID = :CENTER_ID
                                AND TMR.RETUEN_CLASS = 1        --仕入訂正
                            GROUP BY
                                    TMR.SHIPPER_ID
                                ,   TMR.CENTER_ID
                                ,   TMR.INVOICE_NO
                                ,   TMR.ITEM_SKU_ID
                        )
                        ,RETURN_DATA AS (
                            SELECT
                                    TMR.SHIPPER_ID
                                ,   TMR.CENTER_ID
                                ,   TMR.INVOICE_NO
                                ,   TMR.ITEM_SKU_ID
                                ,   SUM(TMR.RETURN_QTY) AS RETURN_QTY
                            FROM
                                    T_MKRETURN_RESULTS TMR
                            INNER JOIN
                                    WORK_DATA WK
                            ON
                                    WK.ITEM_SKU_ID = TMR.ITEM_SKU_ID
                                AND WK.INVOICE_NO = TMR.INVOICE_NO
                                AND WK.CENTER_ID = TMR.CENTER_ID
                                AND WK.SHIPPER_ID = TMR.SHIPPER_ID
                            WHERE
                                    TMR.SHIPPER_ID = :SHIPPER_ID
                                AND TMR.CENTER_ID = :CENTER_ID
                                AND TMR.RETUEN_CLASS = 0        --仕入先返品
                            GROUP BY
                                    TMR.SHIPPER_ID
                                ,   TMR.CENTER_ID
                                ,   TMR.INVOICE_NO
                                ,   TMR.ITEM_SKU_ID
                        )
                    SELECT
                            ROW_NUMBER() OVER (
                                            {order}
                                        ) AS LINE_NO
                        ,   CTGRI.CATEGORY_NAME1
                        ,   TS.ITEM_SKU_ID
                        ,   TS.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   TS.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   TS.ITEM_SIZE_ID
                        ,   MS.ITEM_SIZE_NAME
                        ,   TS.JAN
                        ,   MIS.NORMAL_BUYING_PRICE
                        ,   TS.STOCK_QTY
                        ,   PR.INVOICE_NO
                        ,   PR.BUYING_PRICE
                        ,   PR.ARRIVE_QTY + NVL(AT.TUNE_QTY, 0) - NVL(AR.RETURN_QTY, 0) AS RETURNABLE_QTY
                        ,   NVL(PR.RETURN_QTY, LEAST(TS.STOCK_QTY, PR.ARRIVE_QTY + NVL(AT.TUNE_QTY, 0) - NVL(AR.RETURN_QTY, 0))) AS RETURN_QTY
                        ,   PR.ARRIVE_QTY
                        ,   PR.SEQ
                    FROM 
                            WORK_DATA PR
                    INNER JOIN 
                            T_STOCKS TS
                    ON 
                            PR.ITEM_SKU_ID = TS.ITEM_SKU_ID
                        AND PR.LOCATION_CD = TS.LOCATION_CD
                        AND PR.CENTER_ID = TS.CENTER_ID
                        AND PR.SHIPPER_ID = TS.SHIPPER_ID
                    LEFT OUTER JOIN 
                            M_ITEM_SKU MIS
                    ON 
                            TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                        AND TS.SHIPPER_ID = MIS.SHIPPER_ID
                    LEFT OUTER JOIN 
                            M_COLORS MC
                    ON 
                            TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                        AND TS.SHIPPER_ID = MC.SHIPPER_ID
                    LEFT OUTER JOIN 
                            M_SIZES MS
                    ON 
                            TS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                        AND TS.SHIPPER_ID = MS.SHIPPER_ID
                    LEFT OUTER JOIN 
                            M_ITEM_CATEGORIES4 CTGRI
                    ON  
                            CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                        AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                        AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                        AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                        AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                    LEFT OUTER JOIN
                            ARRIVE_TUNE_DATA AT
                    ON
                            AT.ITEM_SKU_ID = PR.ITEM_SKU_ID
                        AND AT.INVOICE_NO = PR.INVOICE_NO
                        AND AT.CENTER_ID = PR.CENTER_ID
                        AND AT.SHIPPER_ID = PR.SHIPPER_ID
                    LEFT OUTER JOIN
                            RETURN_DATA AR
                    ON
                            AR.ITEM_SKU_ID = PR.ITEM_SKU_ID
                        AND AR.INVOICE_NO = PR.INVOICE_NO
                        AND AR.CENTER_ID = PR.CENTER_ID
                        AND AR.SHIPPER_ID = PR.SHIPPER_ID
                    {order}
                ";

                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", condition.CenterId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<PurchaseReturnsResultRow>(sql, parameters).Count();

                sql += " OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY";

                parameters.AddDynamicParams(
                    new {
                        PAGE_SIZE = condition.PageSize,
                        OFFSET = (condition.Page - 1) * condition.PageSize
                    });

                // 2.検索・ワーク作成
                var Result =  MvcDbContext.Current.Database.Connection.Query<PurchaseReturnsResultRow>(sql, parameters).ToList();

                // Excute paging
                return new StaticPagedList<PurchaseReturnsResultRow>(Result, condition.Page, condition.PageSize, totalCount);
            }
            catch (DbUpdateException ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "GetReturnInfo");
                throw;
            }
        }
        /// <summary>
        /// GetData from Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public int CheckVendorCount(PurchaseReturnsSearchConditions condition)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                StringBuilder query = new StringBuilder(@"
                    SELECT
                           COUNT(*)
                      FROM T_STOCKS TS
                      LEFT JOIN M_ITEM_SKU MIS
                        ON TS.SHIPPER_ID = MIS.SHIPPER_ID
                       AND TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                     WHERE
                           TS.SHIPPER_ID = :SHIPPER_ID
                       AND TS.CENTER_ID = :CENTER_ID
                       AND TS.LOCATION_CD = (SELECT SF_GET_FIXED_LOCATION(:SHIPPER_ID,:CENTER_ID,:MAKE_USER_ID,'MKH') FROM DUAL)
                ");
                parameters.Add(":CENTER_ID", condition.CenterId);
                parameters.Add(":MAKE_USER_ID", Common.Profile.User.UserId);
                // 仕分先
                if (!string.IsNullOrEmpty(condition.VendorId))
                {
                    query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :VENDOR_ID ");
                    parameters.Add(":VENDOR_ID", condition.VendorId + "%");
                }

                // 仕分先名
                if (string.IsNullOrEmpty(condition.VendorId) && !string.IsNullOrEmpty(condition.VendorName))
                {
                    query.Append(" AND MIS.VENDOR_NAME LIKE :VENDOR_NAME ");
                    parameters.Add(":VENDOR_NAME", condition.VendorName + "%");
                }
                query.AppendLine("GROUP BY MIS.MAIN_VENDOR_ID");

                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query(query.ToString(), parameters).Count();

                // 2.検索・ワーク作成
                return totalCount;
            }
            catch (DbUpdateException ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "GetReturnInfo");
                throw;
            }
        }
        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool UpdatePurchaseReturns(PurchaseReturnsInputViewModel InputResult)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    for(int i = 0; i < InputResult.Results.PurchaseReturns.Count; i++)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        StringBuilder query = new StringBuilder(@"
                            UPDATE WW_RET_PURCHASE_RETURNS
                               SET
                                   UPDATE_DATE = SYSTIMESTAMP
                                  ,UPDATE_USER_ID = :UPDATE_USER_ID
                                  ,UPDATE_PROGRAM_NAME = 'PurchaseReturns'
                                  ,UPDATE_COUNT = UPDATE_COUNT + 1
                                  ,BUYING_PRICE = :BUYING_PRICE
                                  ,RETURN_QTY = :RETURN_QTY
                                  ,INVOICE_NO = :INVOICE_NO
                                  ,ARRIVE_QTY = :ARRIVE_QTY
                             WHERE SEQ = :SEQ
                               AND SHIPPER_ID = :SHIPPER_ID
                               AND CENTER_ID = :CENTER_ID
                               AND ITEM_SKU_ID = :ITEM_SKU_ID
                        ");
                        parameters.Add(":SEQ", InputResult.Results.PurchaseReturns[i].Seq);
                        parameters.Add("BUYING_PRICE", InputResult.Results.PurchaseReturns[i].BuyingPrice);
                        parameters.Add("RETURN_QTY", InputResult.Results.PurchaseReturns[i].ReturnQty);
                        parameters.Add("INVOICE_NO", InputResult.Results.PurchaseReturns[i].InvoiceNo);
                        parameters.Add("ARRIVE_QTY", InputResult.Results.PurchaseReturns[i].ArriveQty);
                        parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                        parameters.Add(":CENTER_ID", InputResult.SearchConditions.CenterId);
                        parameters.Add(":UPDATE_USER_ID", Common.Profile.User.UserId);
                        parameters.Add(":ITEM_SKU_ID", InputResult.Results.PurchaseReturns[i].ItemSkuId);

                        // 2.検索・ワーク作成
                        var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                    }

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
        /// 仕入返品確定ストアド実行処理
        /// </summary>
        public void UpdRetPurchaseReturn(PurchaseReturnsInputViewModel InputResult, out ProcedureStatus status, out string message,out string returnId)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", InputResult.SearchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_SEQ", InputResult.SearchConditions.Seq, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);
            param.Add("OUT_RETURN_ID", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
            "SP_W_RET_PURCHASE_RETURNS_UPD",
            param,
            commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
            returnId = param.Get<string>("OUT_RETURN_ID");
        }

        /// <summary>
        /// 納品書モーダル検索結果取得
        /// </summary>
        /// <returns></returns>
        public InvoiceViewModel ListingInvoice(long seq, string centerId, string itemSkuId)
        {
            var sql = $@"
                SELECT
                        MIS.ITEM_ID
                    ,   MIS.ITEM_NAME
                    ,   MIS.ITEM_SKU_ID
                    ,   MIS.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   MIS.ITEM_SIZE_ID
                    ,   MS.ITEM_SIZE_NAME
                    ,   MIS.JAN
                FROM
                        M_ITEM_SKU MIS
                LEFT OUTER JOIN 
                        M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT OUTER JOIN 
                        M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                WHERE
                        MIS.SHIPPER_ID = :SHIPPER_ID
                    AND MIS.ITEM_SKU_ID = :ITEM_SKU_ID
            ";

            var result = MvcDbContext.Current.Database.Connection.Query<InvoiceViewModel>(
                sql,
                new
                {
                    SHIPPER_ID = Common.Profile.User.ShipperId,
                    ITEM_SKU_ID = itemSkuId
                }).FirstOrDefault();

            if (result == null) return null;

            sql = $@"
                WITH
                        WORK_DATA AS (
                            SELECT
                                    *
                            FROM
                                    WW_RET_PURCHASE_RETURNS PR
                            WHERE
                                    PR.SEQ = :SEQ
                                AND PR.SHIPPER_ID = :SHIPPER_ID
                                AND PR.CENTER_ID = :CENTER_ID
                                AND PR.ITEM_SKU_ID = :ITEM_SKU_ID
                        )
                    ,   ARRIVE_RESULT_DATA AS (
                            SELECT
                                    TAR_SUB.SHIPPER_ID
                                ,   TAR_SUB.CENTER_ID
                                ,   TAR_SUB.ITEM_SKU_ID
                                ,   TAR_SUB.INVOICE_NO
                                ,   MAX(TAR_SUB.VENDOR_ID) AS VENDOR_ID
                                ,   MAX(TAR_SUB.CONFIRM_DATE) AS CONFIRM_DATE
                                ,   MAX(TAP_SUB.ARRIVE_PLAN_DATE) AS ARRIVE_DATE
                                ,   MAX(TAP_SUB.NORMAL_BUYING_PRICE) AS NORMAL_BUYING_PRICE
                                ,   SUM(TAR_SUB.RESULT_QTY) AS ARRIVE_QTY
                            FROM
                                    T_ARRIVE_RESULTS TAR_SUB
                            INNER JOIN
                                    T_ARRIVE_PLANS TAP_SUB
                            ON
                                    TAR_SUB.SHIPPER_ID = TAP_SUB.SHIPPER_ID
                                AND TAR_SUB.CENTER_ID = TAP_SUB.CENTER_ID
                                AND TAR_SUB.INVOICE_NO = TAP_SUB.INVOICE_NO
                                AND TAR_SUB.INVOICE_SEQ = TAP_SUB.INVOICE_SEQ
                            WHERE
                                    TAR_SUB.SHIPPER_ID = :SHIPPER_ID
                                AND TAR_SUB.CENTER_ID = :CENTER_ID
                                AND TAR_SUB.ITEM_SKU_ID = :ITEM_SKU_ID
                                AND TAR_SUB.IF_STATE = 2                   --実績送信済
                            GROUP BY
                                    TAR_SUB.SHIPPER_ID
                                ,   TAR_SUB.CENTER_ID
                                ,   TAR_SUB.ITEM_SKU_ID
                                ,   TAR_SUB.INVOICE_NO

                            UNION ALL

                            SELECT
                                    TAR_SUB.SHIPPER_ID
                                ,   TAR_SUB.CENTER_ID
                                ,   TAR_SUB.ITEM_SKU_ID
                                ,   TAR_SUB.INVOICE_NO
                                ,   MAX(TAR_SUB.VENDOR_ID) AS VENDOR_ID
                                ,   MAX(TAR_SUB.CONFIRM_DATE) AS CONFIRM_DATE
                                ,   MAX(TAP_SUB.ARRIVE_PLAN_DATE) AS ARRIVE_DATE
                                ,   MAX(TAP_SUB.NORMAL_BUYING_PRICE) AS NORMAL_BUYING_PRICE
                                ,   SUM(TAR_SUB.RESULT_QTY) AS ARRIVE_QTY
                            FROM
                                    A_ARRIVE_RESULTS TAR_SUB
                            INNER JOIN
                                    A_ARRIVE_PLANS TAP_SUB
                            ON
                                    TAR_SUB.SHIPPER_ID = TAP_SUB.SHIPPER_ID
                                AND TAR_SUB.CENTER_ID = TAP_SUB.CENTER_ID
                                AND TAR_SUB.INVOICE_NO = TAP_SUB.INVOICE_NO
                                AND TAR_SUB.INVOICE_SEQ = TAP_SUB.INVOICE_SEQ
                            WHERE
                                    TAR_SUB.SHIPPER_ID = :SHIPPER_ID
                                AND TAR_SUB.CENTER_ID = :CENTER_ID
                                AND TAR_SUB.ITEM_SKU_ID = :ITEM_SKU_ID
                            GROUP BY
                                    TAR_SUB.SHIPPER_ID
                                ,   TAR_SUB.CENTER_ID
                                ,   TAR_SUB.ITEM_SKU_ID
                                ,   TAR_SUB.INVOICE_NO
                        )
                    ,   ARRIVE_TUNE_DATA AS (
                            SELECT
                                    TMR.SHIPPER_ID
                                ,   TMR.CENTER_ID
                                ,   TMR.INVOICE_NO
                                ,   SUM(TMR.RETURN_QTY) AS TUNE_QTY
                            FROM
                                    T_MKRETURN_RESULTS TMR
                            WHERE
                                    TMR.SHIPPER_ID = :SHIPPER_ID
                                AND TMR.CENTER_ID = :CENTER_ID
                                AND TMR.ITEM_SKU_ID = :ITEM_SKU_ID
                                AND TMR.RETUEN_CLASS = 1        --仕入訂正
                            GROUP BY
                                    TMR.SHIPPER_ID
                                ,   TMR.CENTER_ID
                                ,   TMR.INVOICE_NO
                        )
                    ,   RETURN_DATA AS (
                            SELECT
                                    TMR.SHIPPER_ID
                                ,   TMR.CENTER_ID
                                ,   TMR.INVOICE_NO
                                ,   SUM(TMR.RETURN_QTY) AS RETURN_QTY
                            FROM
                                    T_MKRETURN_RESULTS TMR
                            WHERE
                                    TMR.SHIPPER_ID = :SHIPPER_ID
                                AND TMR.CENTER_ID = :CENTER_ID
                                AND TMR.ITEM_SKU_ID = :ITEM_SKU_ID
                                AND TMR.RETUEN_CLASS = 0        --仕入先返品
                            GROUP BY
                                    TMR.SHIPPER_ID
                                ,   TMR.CENTER_ID
                                ,   TMR.INVOICE_NO
                        )
                SELECT
                        TAR.VENDOR_ID
                    ,   MV.VENDOR_NAME1 AS VENDOR_NAME
                    ,   TO_CHAR(TAR.CONFIRM_DATE, 'YYYY/MM/DD') AS CONFIRM_DATE
                    ,   TAR.INVOICE_NO
                    ,   TO_CHAR(TAR.ARRIVE_DATE, 'YYYY/MM/DD') AS ARRIVE_DATE
                    ,   TAR.NORMAL_BUYING_PRICE
                    ,   TAR.ARRIVE_QTY
                    ,   TAR.ARRIVE_QTY + NVL(AT.TUNE_QTY, 0) - NVL(AR.RETURN_QTY, 0) AS RETURNABLE_QTY
                    ,   CASE
                            WHEN WK.INVOICE_NO IS NULL THEN 0
                            ELSE 1
                        END AS IS_SELECTED
                FROM
                        ARRIVE_RESULT_DATA TAR
                LEFT OUTER JOIN
                        M_VENDORS MV
                ON
                        TAR.VENDOR_ID = MV.VENDOR_ID
                    AND TAR.SHIPPER_ID = MV.SHIPPER_ID
                LEFT OUTER JOIN
                        ARRIVE_TUNE_DATA AT
                ON
                        AT.INVOICE_NO = TAR.INVOICE_NO
                    AND AT.CENTER_ID = TAR.CENTER_ID
                    AND AT.SHIPPER_ID = TAR.SHIPPER_ID
                LEFT OUTER JOIN
                        RETURN_DATA AR
                ON
                        AR.INVOICE_NO = TAR.INVOICE_NO
                    AND AR.CENTER_ID = TAR.CENTER_ID
                    AND AR.SHIPPER_ID = TAR.SHIPPER_ID
                LEFT OUTER JOIN
                        WORK_DATA WK
                ON
                        WK.ITEM_SKU_ID = TAR.ITEM_SKU_ID
                    AND WK.INVOICE_NO = TAR.INVOICE_NO
                    AND WK.CENTER_ID = TAR.CENTER_ID
                    AND WK.SHIPPER_ID = TAR.SHIPPER_ID
                ORDER BY
                        TAR.CONFIRM_DATE DESC
                    ,   TAR.INVOICE_NO
            ";

            result.InvoiceResults = MvcDbContext.Current.Database.Connection.Query<InvoiceResultRow>(
                sql,
                new
                {
                    SHIPPER_ID = Common.Profile.User.ShipperId,
                    CENTER_ID = centerId,
                    ITEM_SKU_ID = itemSkuId,
                    SEQ = seq
                }).ToList();

            return result;
        }

    }
}