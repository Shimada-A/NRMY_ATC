namespace Wms.Areas.Arrival.Query.PurchaseReference
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Areas.Arrival.ViewModels.PurchaseReference;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Arrival.ViewModels.PurchaseReference.PurchaseReference01SearchConditions;
    using static Wms.Areas.Arrival.ViewModels.PurchaseReference.PurchaseReference02SearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PurchaseReference01Report> PurchaseReference01Listing(PurchaseReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT
                        SEQ
                    ,   MAX(LINE_NO) AS LINE_NO
                    ,   ARRIVE_PLAN_DATE
                    ,   BRAND_ID
                    ,   MAX(BRAND_NAME) AS BRAND_NAME
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
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

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

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseReference01Report>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PurchaseReference02DetailReport> PurchaseReference02DetailListing(PurchaseReference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT ARRIVE_PLAN_DATE
                      ,BRAND_ID
                      ,BRAND_NAME
                      ,VENDOR_ID
                      ,VENDOR_NAME
                      ,INVOICE_NO
                      ,PO_ID
                      ,CATEGORY_NAME1
                      ,ITEM_ID
                      ,ITEM_NAME
                      ,ITEM_COLOR_ID
                      ,ITEM_COLOR_NAME
                      ,ITEM_SIZE_ID
                      ,ITEM_SIZE_NAME
                      ,ARRIVE_PLAN_QTY
                      ,RESULT_QTY
                      ,DIFFERENCE_QTY
                      ,ARRIVAL_STATUS
                      ,ARRIVAL_STATUS_NAME
                      ,CONFIRM_DATE
                      ,IF_RUN_DATE
                  FROM WW_ARR_PUR_REF02
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Sort function
            switch (condition.DetailSort)
            {
                case PurchaseReference02DetailSort.ArrivePlanDateVendorIdSku:
                    switch (condition.Sort)
                    {
                        case PurchaseReference02SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,VENDOR_ID DESC,ITEM_SKU_ID DESC,INVOICE_NO DESC,PO_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,VENDOR_ID ASC,ITEM_SKU_ID ASC,INVOICE_NO ASC,PO_ID ASC ");
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
                default:
                    switch (condition.Sort)
                    {
                        case PurchaseReference02SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,VENDOR_ID DESC,INVOICE_NO DESC,PO_ID DESC,ITEM_SKU_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,VENDOR_ID ASC,INVOICE_NO ASC,PO_ID ASC,ITEM_SKU_ID ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseReference02DetailReport>(query.ToString(), parameters);
        }
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PurchaseReference02BoxNoReport> PurchaseReference02BoxNoListing(PurchaseReference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT ARRIVE_PLAN_DATE
                      ,BRAND_ID
                      ,BRAND_NAME
                      ,VENDOR_ID
                      ,VENDOR_NAME
                      ,INVOICE_NO
                      ,PO_ID
                      ,CATEGORY_NAME1
                      ,ITEM_ID
                      ,ITEM_NAME
                      ,ITEM_COLOR_ID
                      ,ITEM_COLOR_NAME
                      ,ITEM_SIZE_ID
                      ,ITEM_SIZE_NAME
                      ,BOX_NO
                      ,ARRIVE_PLAN_QTY
                      ,RESULT_QTY
                      ,DIFFERENCE_QTY
                      ,ARRIVAL_STATUS
                      ,ARRIVAL_STATUS_NAME
                      ,CONFIRM_DATE
                      ,IF_RUN_DATE
                  FROM WW_ARR_PUR_REF02
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Sort function
            switch (condition.BoxSort)
            {
                case PurchaseReference02BoxSort.ArrivePlanDateVendorIdSkuBoxNo:
                    switch (condition.Sort)
                    {
                        case PurchaseReference02SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,VENDOR_ID DESC,ITEM_SKU_ID DESC,INVOICE_NO DESC,PO_ID DESC,BOX_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,VENDOR_ID ASC,ITEM_SKU_ID ASC,INVOICE_NO ASC,PO_ID ASC,BOX_NO ASC ");
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
                default:
                    switch (condition.Sort)
                    {
                        case PurchaseReference02SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE DESC,VENDOR_ID DESC,INVOICE_NO DESC,PO_ID DESC,ITEM_SKU_ID DESC,BOX_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ARRIVE_PLAN_DATE ASC,VENDOR_ID ASC,INVOICE_NO ASC,PO_ID ASC,ITEM_SKU_ID ASC,BOX_NO ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseReference02BoxNoReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 入荷仕分リスト(JAN入り)に出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<PurchaseReferenceCsv> GetPurchaseReferenceList(PurchaseReference02SearchConditions condition, bool isJan)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder($@"
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
                                T_SHIPS
                        GROUP BY
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   SHIP_INSTRUCT_ID
                            ,   ITEM_SKU_ID
                )
                SELECT TAP.CENTER_ID
                      ,MCE.CENTER_NAME1 CENTER_NAME
                      ,TO_CHAR(TAP.ARRIVE_PLAN_DATE,'yyyy/MM/dd') ARRIVE_PLAN_DATE
                      ,TAP.VENDOR_ID
                      ,MV.VENDOR_NAME1 VENDOR_NAME
                      ,TAP.INVOICE_NO
                      ,MIS.DIVISION_ID
                      ,MD.DIVISION_NAME
                      ,TAP.ITEM_SKU_ID
                      ,TAP.ITEM_ID
                      ,MIS.ITEM_NAME
                      ,MIS.BRAND_ID
                      ,MB.BRAND_NAME
                      ,TAP.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,TAP.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,SUBSTR(TAP.JAN,1,7) JAN1
                      ,SUBSTR(TAP.JAN,8,6) JAN2
                      ,NVL(TAP.ARRIVE_PLAN_QTY,0) ARRIVE_PLAN_QTY
                      ,TAP.JAN
                      ,CASE
                           WHEN TAR.ARRIVAL_STATUS >= 4 THEN TAR.CONFIRM_DATE
                           ELSE NULL
                       END AS CONFIRM_DATE
                      ,TAR.IF_RUN_DATE
                      ,NVL(TAR.RESULT_QTY,0) AS ARRIVE_RESULT_QTY
                      ,DENSE_RANK() OVER (PARTITION BY TAP.ARRIVE_PLAN_DATE, TAP.VENDOR_ID, TAP.INVOICE_NO ORDER BY TAP.ITEM_ID) AS ITEM_SEQ
                      ,{(isJan ? "1" : "0")} AS JAN_FLAG
                  FROM T_ARRIVE_PLANS TAP
                  LEFT JOIN T_ARRIVE_RESULTS TAR
                    ON TAP.SHIPPER_ID = TAR.SHIPPER_ID
                   AND TAP.CENTER_ID = TAR.CENTER_ID
                   AND TAP.INVOICE_NO = TAR.INVOICE_NO
                   AND TAP.ITEM_SKU_ID = TAR.ITEM_SKU_ID
                  LEFT JOIN SHIP
                    ON TAP.SHIPPER_ID = SHIP.SHIPPER_ID
                   AND TAP.CENTER_ID = SHIP.CENTER_ID
                   AND TAP.INVOICE_NO = SHIP.SHIP_INSTRUCT_ID
                   AND TAP.ITEM_SKU_ID = SHIP.ITEM_SKU_ID
                  LEFT JOIN M_VENDORS MV
                    ON TAP.SHIPPER_ID = MV.SHIPPER_ID
                   AND TAP.VENDOR_ID = MV.VENDOR_ID
                   AND MV.DELETE_FLAG = 0
                  LEFT JOIN M_ITEM_SKU MIS
                    ON TAP.SHIPPER_ID = MIS.SHIPPER_ID
                   AND TAP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                   AND MIS.DELETE_FLAG = 0
                  LEFT JOIN M_COLORS MC
                    ON TAP.SHIPPER_ID = MC.SHIPPER_ID
                   AND TAP.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                   AND MC.DELETE_FLAG = 0
                  LEFT JOIN M_SIZES MS
                    ON TAP.SHIPPER_ID = MS.SHIPPER_ID
                   AND TAP.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                   AND MS.DELETE_FLAG = 0
                  LEFT JOIN M_CENTERS MCE
                    ON TAP.SHIPPER_ID = MCE.SHIPPER_ID
                   AND TAP.CENTER_ID = MCE.CENTER_ID
                  LEFT JOIN M_BRANDS MB
                    ON MIS.SHIPPER_ID = MB.SHIPPER_ID
                   AND MIS.BRAND_ID = MB.BRAND_ID
                   AND MB.DELETE_FLAG = 0
                  LEFT JOIN M_DIVISIONS MD
                    ON MIS.SHIPPER_ID = MD.SHIPPER_ID
                   AND MIS.DIVISION_ID = MD.DIVISION_ID
                 WHERE TAP.SHIPPER_ID = :SHIPPER_ID
                   AND TAP.CENTER_ID = :CENTER_ID
                   AND TAP.CANCEL_FLAG = 0
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            // Add search condition

            // 入荷予定日(検索・From-To)
            if (condition.ArrivePlanDateFrom != null)
            {
                query.Append(" AND TAP.ARRIVE_PLAN_DATE >= :ARRIVE_PLAN_DATE_FROM ");
                parameters.Add(":ARRIVE_PLAN_DATE_FROM", condition.ArrivePlanDateFrom);
            }
            if (condition.ArrivePlanDateTo != null)
            {
                query.Append(" AND TAP.ARRIVE_PLAN_DATE <= :ARRIVE_PLAN_DATE_TO ");
                parameters.Add(":ARRIVE_PLAN_DATE_TO", condition.ArrivePlanDateTo);
            }

            // 実績確定日(検索・From-To)
            if (condition.ConfirmDateFrom != null)
            {
                query.Append(" AND TAR.ARRIVAL_STATUS >= 4 ");
                query.Append(" AND TRUNC(TAR.CONFIRM_DATE,'DD') >= :CONFIRM_DATE_FROM ");
                parameters.Add(":CONFIRM_DATE_FROM", condition.ConfirmDateFrom);
            }
            if (condition.ConfirmDateTo != null)
            {
                query.Append(" AND TAR.ARRIVAL_STATUS >= 4 ");
                query.Append(" AND TRUNC(TAR.CONFIRM_DATE,'DD') <= :CONFIRM_DATE_TO ");
                parameters.Add(":CONFIRM_DATE_TO", condition.ConfirmDateTo);
            }

            // 事業部
            if (!string.IsNullOrEmpty(condition.DivisionId))
            {
                query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                parameters.Add(":DIVISION_ID", condition.DivisionId);
            }

            // ブランド
            if (condition.BrandId.Any())
            {
                if (!string.IsNullOrEmpty(condition.BrandId[0]))
                {
                    query.Append(" AND MIS.BRAND_ID IN :BRAND_ID ");
                    //var brandList = condition.BrandId.Split(',');
                    parameters.Add(":BRAND_ID", condition.BrandId);
                }
            }

            // 仕入先
            if (!string.IsNullOrEmpty(condition.VendorId))
            {
                query.Append(" AND TAP.VENDOR_ID LIKE :VENDOR_ID ");
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

            if (!string.IsNullOrEmpty(condition.ItemCode))
            {
                query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                parameters.Add(":ITEM_CODE", condition.ItemCode);
            }

            // 状況
            if (!string.IsNullOrEmpty(condition.ArrivalStatus))
            {
                query.Append(" AND TO_CHAR(NVL(TAR.ARRIVAL_STATUS,1)) = :ARRIVAL_STATUS ");
                parameters.Add(":ARRIVAL_STATUS", condition.ArrivalStatus);
            }

            // 納品書番号
            if (!string.IsNullOrEmpty(condition.InvoiceNo))
            {
                query.Append(" AND TAP.INVOICE_NO LIKE :INVOICE_NO ");
                parameters.Add(":INVOICE_NO", condition.InvoiceNo + "%");
            }

            // 発注番号
            if (!string.IsNullOrEmpty(condition.PoId))
            {
                query.Append(" AND TAP.PO_ID LIKE :PO_ID ");
                parameters.Add(":PO_ID", condition.PoId + "%");
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND TAP.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND TAP.JAN LIKE :JAN ");
                parameters.Add(":JAN", condition.Jan + "%");
            }

            // ケースNo
            if (!string.IsNullOrEmpty(condition.BoxNo))
            {
                query.Append(@" 
                        AND EXISTS (
                                SELECT
                                        *
                                FROM
                                        T_PACKING_ARRIVE_PLANS PACKING_PLAN
                                WHERE
                                        PACKING_PLAN.SHIPPER_ID = TAP.SHIPPER_ID
                                    AND PACKING_PLAN.CENTER_ID = TAP.CENTER_ID
                                    AND PACKING_PLAN.INVOICE_NO = TAP.INVOICE_NO
                                    AND PACKING_PLAN.ITEM_SKU_ID = TAP.ITEM_SKU_ID
                                    AND PACKING_PLAN.BOX_NO LIKE :BOX_NO
                            ) ");
                parameters.Add(":BOX_NO", condition.BoxNo + "%");
            }
            query.Append(@" ORDER BY TAP.ARRIVE_PLAN_DATE
                                    ,TAP.VENDOR_ID
                                    ,TAP.INVOICE_NO
                                    ,TAP.ITEM_ID
                                    ,TAP.INVOICE_SEQ
                        ");

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseReferenceCsv>(query.ToString(), parameters);
        }

        /// <summary>
        /// 仕入梱包リスト(JAN入り)に出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<PurchaseReferencePackingCsv> GetPurchaseReferenceListPacking(PurchaseReference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        ARR_PLAN.CENTER_ID
                    ,   ARR_PLAN.CENTER_NAME
                    ,   ARR_PLAN.ARRIVE_PLAN_DATE
                    ,   ARR_PLAN.VENDOR_ID
                    ,   ARR_PLAN.VENDOR_NAME
                    ,   TPAP.BOX_NO
                    ,   TPAP.INVOICE_NO
                    ,   ARR_PLAN.JAN
                    ,   ARR_PLAN.ITEM_SKU_ID
                    ,   ARR_PLAN.ITEM_NAME
                    ,   NVL(TPAP.PACKING_PLAN_QTY,0) AS PACKING_PLAN_QTY
                    ,   ARR_PLAN.CONFIRM_DATE
                    ,   ARR_PLAN.IF_RUN_DATE
                    ,   TPAR.DISP_BOX_NO
                FROM
                        T_PACKING_ARRIVE_PLANS TPAP
                INNER JOIN
                    (   SELECT TAP.CENTER_ID
                              ,TAP.SHIPPER_ID
                              ,MCE.CENTER_NAME1 CENTER_NAME
                              ,TO_CHAR(TAP.ARRIVE_PLAN_DATE,'yyyy/MM/dd') ARRIVE_PLAN_DATE
                              ,TAP.VENDOR_ID
                              ,MV.VENDOR_NAME1 VENDOR_NAME
                              ,TAP.INVOICE_NO
                              ,TAP.ITEM_SKU_ID
                              ,MIS.ITEM_NAME
                              ,TAP.JAN
                              ,TAP.INVOICE_SEQ
                              ,CASE
                                   WHEN TAR.ARRIVAL_STATUS >= 4 THEN TAR.CONFIRM_DATE
                                   ELSE NULL
                               END AS CONFIRM_DATE
                              ,TAR.IF_RUN_DATE
                          FROM T_ARRIVE_PLANS TAP
                          LEFT JOIN T_ARRIVE_RESULTS TAR
                            ON TAP.SHIPPER_ID = TAR.SHIPPER_ID
                           AND TAP.CENTER_ID = TAR.CENTER_ID
                           AND TAP.INVOICE_NO = TAR.INVOICE_NO
                           AND TAP.ITEM_SKU_ID = TAR.ITEM_SKU_ID
                          LEFT JOIN M_VENDORS MV
                            ON TAP.SHIPPER_ID = MV.SHIPPER_ID
                           AND TAP.VENDOR_ID = MV.VENDOR_ID
                           AND MV.DELETE_FLAG = 0
                          LEFT JOIN M_ITEM_SKU MIS
                            ON TAP.SHIPPER_ID = MIS.SHIPPER_ID
                           AND TAP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                           AND MIS.DELETE_FLAG = 0
                          LEFT JOIN M_CENTERS MCE
                            ON TAP.SHIPPER_ID = MCE.SHIPPER_ID
                           AND TAP.CENTER_ID = MCE.CENTER_ID
                          LEFT JOIN M_BRANDS MB
                            ON MIS.SHIPPER_ID = MB.SHIPPER_ID
                           AND MIS.BRAND_ID = MB.BRAND_ID
                           AND MB.DELETE_FLAG = 0
                         WHERE TAP.SHIPPER_ID = :SHIPPER_ID
                           AND TAP.CENTER_ID = :CENTER_ID
                           AND TAP.CANCEL_FLAG = 0
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);

            // Add search condition

            // 入荷予定日日(検索・From-To)
            if (condition.ArrivePlanDateFrom != null)
            {
                query.Append(" AND TAP.ARRIVE_PLAN_DATE >= :ARRIVE_PLAN_DATE_FROM ");
                parameters.Add(":ARRIVE_PLAN_DATE_FROM", condition.ArrivePlanDateFrom);
            }
            if (condition.ArrivePlanDateTo != null)
            {
                query.Append(" AND TAP.ARRIVE_PLAN_DATE <= :ARRIVE_PLAN_DATE_TO ");
                parameters.Add(":ARRIVE_PLAN_DATE_TO", condition.ArrivePlanDateTo);
            }

            // 実績確定日(検索・From-To)
            if (condition.ConfirmDateFrom != null)
            {
                query.Append(" AND TAR.ARRIVAL_STATUS >= 4 ");
                query.Append(" AND TRUNC(TAR.CONFIRM_DATE,'DD') >= :CONFIRM_DATE_FROM ");
                parameters.Add(":CONFIRM_DATE_FROM", condition.ConfirmDateFrom);
            }
            if (condition.ConfirmDateTo != null)
            {
                query.Append(" AND TAR.ARRIVAL_STATUS >= 4 ");
                query.Append(" AND TRUNC(TAR.CONFIRM_DATE,'DD') <= :CONFIRM_DATE_TO ");
                parameters.Add(":CONFIRM_DATE_TO", condition.ConfirmDateTo);
            }

            // 事業部
            if (!string.IsNullOrEmpty(condition.DivisionId))
            {
                query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                parameters.Add(":DIVISION_ID", condition.DivisionId);
            }

            // ブランド
            if (condition.BrandId.Any())
            {
                if (!string.IsNullOrEmpty(condition.BrandId[0]))
                {
                    query.Append(" AND MIS.BRAND_ID IN :BRAND_ID ");
                    //var brandList = condition.BrandId.Split(',');
                    parameters.Add(":BRAND_ID", condition.BrandId);
                }
            }

            //// ブランド名
            //if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
            //{
            //    query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
            //    parameters.Add(":BRAND_NAME", condition.BrandName + "%");
            //}

            // 仕入先
            if (!string.IsNullOrEmpty(condition.VendorId))
            {
                query.Append(" AND TAP.VENDOR_ID LIKE :VENDOR_ID ");
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

            if (!string.IsNullOrEmpty(condition.ItemCode))
            {
                query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                parameters.Add(":ITEM_CODE", condition.ItemCode);
            }

            // 状況
            if (!string.IsNullOrEmpty(condition.ArrivalStatus))
            {
                query.Append(" AND TO_CHAR(NVL(TAR.ARRIVAL_STATUS,1)) = :ARRIVAL_STATUS ");
                parameters.Add(":ARRIVAL_STATUS", condition.ArrivalStatus);
            }

            // 納品書番号
            if (!string.IsNullOrEmpty(condition.InvoiceNo))
            {
                query.Append(" AND TAP.INVOICE_NO LIKE :INVOICE_NO ");
                parameters.Add(":INVOICE_NO", condition.InvoiceNo + "%");
            }

            // 発注番号
            if (!string.IsNullOrEmpty(condition.PoId))
            {
                query.Append(" AND TAP.PO_ID LIKE :PO_ID ");
                parameters.Add(":PO_ID", condition.PoId + "%");
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND TAP.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND TAP.JAN LIKE :JAN ");
                parameters.Add(":JAN", condition.Jan + "%");
            }

            // ケースNo
            if (!string.IsNullOrEmpty(condition.BoxNo))
            {
                query.Append(@" 
                        AND EXISTS (
                                SELECT
                                        *
                                FROM
                                        T_PACKING_ARRIVE_PLANS PACKING_PLAN
                                WHERE
                                        PACKING_PLAN.SHIPPER_ID = TAP.SHIPPER_ID
                                    AND PACKING_PLAN.CENTER_ID = TAP.CENTER_ID
                                    AND PACKING_PLAN.INVOICE_NO = TAP.INVOICE_NO
                                    AND PACKING_PLAN.ITEM_SKU_ID = TAP.ITEM_SKU_ID
                                    AND PACKING_PLAN.BOX_NO LIKE :BOX_NO
                            ) ");
                parameters.Add(":BOX_NO", condition.BoxNo + "%");
            }
            query.Append(@"
                    ) ARR_PLAN
                ON
                        TPAP.SHIPPER_ID = ARR_PLAN.SHIPPER_ID
                    AND TPAP.CENTER_ID = ARR_PLAN.CENTER_ID
                    AND TPAP.INVOICE_NO = ARR_PLAN.INVOICE_NO
                    AND TPAP.ITEM_SKU_ID = ARR_PLAN.ITEM_SKU_ID
                INNER JOIN
                        T_PACKING_ARRIVE_RESULTS TPAR
                    ON  TPAR.SHIPPER_ID = TPAP.SHIPPER_ID
                    AND TPAR.CENTER_ID = TPAP.CENTER_ID
                    AND TPAR.BOX_NO = TPAP.BOX_NO
                    AND TPAR.BOX_SEQ = TPAP.BOX_SEQ
                    AND TPAR.INVOICE_NO = TPAP.INVOICE_NO
                    AND TPAR.INVOICE_SEQ = TPAP.INVOICE_SEQ
                    AND TPAR.CASE_CLASS = 1
                    AND TPAR.TCDC_CLASS = 2
                ORDER BY 
                        ARR_PLAN.ARRIVE_PLAN_DATE
                    ,   ARR_PLAN.VENDOR_ID
                    ,   TPAP.BOX_NO
                    ,   ARR_PLAN.INVOICE_NO
                    ,   ARR_PLAN.ITEM_SKU_ID
                ");

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseReferencePackingCsv>(query.ToString(), parameters);
        }
    }
}