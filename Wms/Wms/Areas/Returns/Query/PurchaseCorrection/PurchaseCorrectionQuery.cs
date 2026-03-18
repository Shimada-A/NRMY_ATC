namespace Wms.Areas.Returns.Query.PurchaseCorrection
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
    using Wms.Areas.Returns.ViewModels.CaseSearchModal;
    using Wms.Areas.Returns.ViewModels.InvoiceSearchModal;
    using Wms.Areas.Returns.ViewModels.JanSearchModal;
    using Wms.Areas.Returns.ViewModels.LocationSearchModal;
    using Wms.Areas.Returns.ViewModels.PurchaseCorrection;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Returns.ViewModels.PurchaseCorrection.PurchaseCorrectionSearchConditions;

    public class PurchaseCorrectionQuery
    {
        /// <summary>
        /// JANモーダル検索結果取得
        /// </summary>
        /// <returns>検索結果</returns>
        public List<JanViewModel> ListingJan(PurchaseCorrectionSearchConditions searchCondition, ref int TotalItemCount)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                        MIS.ITEM_ID
                    ,   MIS.ITEM_NAME
                    ,   MIS.ITEM_SKU_ID
                    ,   MIS.ITEM_COLOR_ID
                    ,   MIS.ITEM_SIZE_ID
                    ,   MIS.JAN
                    ,   MC.ITEM_COLOR_NAME
                    ,   MS.ITEM_SIZE_NAME
                    ,   TAR.INVOICE_NO
                    ,   TO_CHAR(TAR.ARRIVE_DATE, 'YYYY/MM/DD') ARRIVE_DATE
                    ,   TO_CHAR(TAR.CONFIRM_DATE, 'YYYY/MM/DD HH24:MI:SS') CONFIRM_DATE
                    ,   MV.VENDOR_ID
                    ,   MV.VENDOR_NAME1
                FROM
                        (
                        SELECT
                                TAR_SUB.SHIPPER_ID
                            ,   TAR_SUB.ITEM_SKU_ID
                            ,   TAR_SUB.CENTER_ID
                            ,   MAX(TAR_SUB.INVOICE_NO) AS INVOICE_NO
                            ,   MAX(TAR_SUB.CONFIRM_DATE) AS CONFIRM_DATE
                            ,   MAX(TAP_SUB.ARRIVE_PLAN_DATE) ARRIVE_DATE
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
                            AND TAR_SUB.INVOICE_NO = :INVOICE_NO
                        GROUP BY
                                TAR_SUB.SHIPPER_ID
                            ,   TAR_SUB.CENTER_ID
                            ,   TAR_SUB.ITEM_SKU_ID
                        UNION ALL
                        SELECT
                                TAR_SUB.SHIPPER_ID
                            ,   TAR_SUB.ITEM_SKU_ID
                            ,   TAR_SUB.CENTER_ID
                            ,   MAX(TAR_SUB.INVOICE_NO) AS INVOICE_NO
                            ,   MAX(TAR_SUB.CONFIRM_DATE) AS CONFIRM_DATE
                            ,   MAX(TAP_SUB.ARRIVE_PLAN_DATE) ARRIVE_DATE
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
                            AND TAR_SUB.INVOICE_NO = :INVOICE_NO
                        GROUP BY
                                TAR_SUB.SHIPPER_ID
                            ,   TAR_SUB.CENTER_ID
                            ,   TAR_SUB.ITEM_SKU_ID
                        ) TAR
                INNER JOIN
                        M_ITEM_SKU MIS
                ON
                        TAR.SHIPPER_ID = MIS.SHIPPER_ID
                    AND TAR.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT OUTER JOIN
                        M_VENDORS MV
                ON 
                        MIS.SHIPPER_ID = MV.SHIPPER_ID
                    AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                ORDER BY
                        MIS.ITEM_SIZE_ID

            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", searchCondition.CenterId);
            parameters.Add(":INVOICE_NO", searchCondition.InvoiceNo);


            // 全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<JanViewModel>(sql.ToString(), parameters).Count();
            TotalItemCount = totalCount;

            return MvcDbContext.Current.Database.Connection.Query<JanViewModel>(sql.ToString(), parameters).ToList();

        }

        /// <summary>
        /// 納品書モーダル検索結果取得
        /// </summary>
        /// <returns>検索結果</returns>
        public List<InvoiceViewModel> ListingInvoice(PurchaseCorrectionSearchConditions searchCondition, ref int TotalItemCount)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                        MIS.ITEM_ID
                    ,   MIS.ITEM_NAME
                    ,   MIS.ITEM_SKU_ID
                    ,   MIS.ITEM_COLOR_ID
                    ,   MIS.ITEM_SIZE_ID
                    ,   MIS.JAN
                    ,   MC.ITEM_COLOR_NAME
                    ,   MS.ITEM_SIZE_NAME
                    ,   TAR.INVOICE_NO
                    ,   TO_CHAR(TAR.ARRIVE_DATE, 'YYYY/MM/DD') ARRIVE_DATE
                    ,   TO_CHAR(TAR.CONFIRM_DATE, 'YYYY/MM/DD HH24:MI:SS') CONFIRM_DATE
                FROM
                        (
                        SELECT
                                TAR_SUB.SHIPPER_ID
                            ,   TAR_SUB.CENTER_ID
                            ,   TAR_SUB.INVOICE_NO
                            ,   MAX(TAR_SUB.CONFIRM_DATE) CONFIRM_DATE
                            ,   MAX(TAR_SUB.ITEM_SKU_ID) ITEM_SKU_ID
                            ,   MAX(TAP_SUB.ARRIVE_PLAN_DATE) ARRIVE_DATE
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
                            AND TAR_SUB.JAN = :JAN
                            AND TAR_SUB.IF_STATE = 2                   --実績送信済
                            AND TAR_SUB.CONFIRM_DATE >= TRUNC(LAST_DAY(ADD_MONTHS(SYSDATE,-2))+1)    --先月1日以降
                        GROUP BY
                                TAR_SUB.SHIPPER_ID
                            ,   TAR_SUB.CENTER_ID
                            ,   TAR_SUB.INVOICE_NO
                        UNION ALL
                        SELECT
                                TAR_SUB.SHIPPER_ID
                            ,   TAR_SUB.CENTER_ID
                            ,   TAR_SUB.INVOICE_NO
                            ,   MAX(TAR_SUB.CONFIRM_DATE) CONFIRM_DATE
                            ,   MAX(TAR_SUB.ITEM_SKU_ID) ITEM_SKU_ID
                            ,   MAX(TAP_SUB.ARRIVE_PLAN_DATE) ARRIVE_DATE
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
                            AND TAR_SUB.JAN = :JAN
                            AND TAR_SUB.CONFIRM_DATE >= TRUNC(LAST_DAY(ADD_MONTHS(SYSDATE,-2))+1)    --先月1日以降
                        GROUP BY
                                TAR_SUB.SHIPPER_ID
                            ,   TAR_SUB.CENTER_ID
                            ,   TAR_SUB.INVOICE_NO
                        ) TAR
                INNER JOIN
                        M_ITEM_SKU MIS
                ON
                        TAR.SHIPPER_ID = MIS.SHIPPER_ID
                    AND TAR.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                ORDER BY
                        TAR.CONFIRM_DATE
                    ,   TAR.INVOICE_NO

            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", searchCondition.CenterId);
            parameters.Add(":JAN", searchCondition.Jan);


            // 全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<InvoiceViewModel>(sql.ToString(), parameters).Count();
            TotalItemCount = totalCount;

            return MvcDbContext.Current.Database.Connection.Query<InvoiceViewModel>(sql.ToString(), parameters).ToList();

        }

        /// <summary>
        /// ロケーションモーダル検索結果取得
        /// </summary>
        /// <returns>検索結果</returns>
        public List<LocationViewModel> ListingLocation(PurchaseCorrectionSearchConditions searchCondition, ref int TotalItemCount)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                        MIS.ITEM_ID
                    ,   MIS.ITEM_NAME
                    ,   MIS.ITEM_SKU_ID
                    ,   MIS.ITEM_COLOR_ID
                    ,   MIS.ITEM_SIZE_ID
                    ,   MIS.JAN
                    ,   MC.ITEM_COLOR_NAME
                    ,   MS.ITEM_SIZE_NAME
                    ,   TS.LOCATION_CD
                    ,   MLC.LOCATION_NAME AS LOCATION_CLASS
                    ,   TS.GRADE_ID
                    ,   TS.STOCK_QTY
                    ,   TS.ALLOC_QTY
                    ,   TS.STOCK_QTY - TS.ALLOC_QTY AS KANOU_QTY
                    ,   ML.CASE_CLASS
                FROM
                        T_STOCKS TS
                INNER JOIN
                        M_ITEM_SKU MIS
                ON
                        TS.SHIPPER_ID = MIS.SHIPPER_ID
                    AND TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                INNER JOIN
                        M_LOCATIONS ML
                ON 
                        TS.SHIPPER_ID = ML.SHIPPER_ID
                    AND TS.CENTER_ID = ML.CENTER_ID
                    AND TS.LOCATION_CD = ML.LOCATION_CD
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT JOIN
                        M_LOCATION_CLASSES MLC
                ON
                        ML.SHIPPER_ID = MLC.SHIPPER_ID
                    AND ML.LOCATION_CLASS = MLC.LOCATION_CLASS
                WHERE
                        TS.SHIPPER_ID = :SHIPPER_ID
                    AND TS.CENTER_ID = :CENTER_ID
                    AND TS.JAN = :JAN
                ORDER BY  TS.LOCATION_CD
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", searchCondition.CenterId);
            parameters.Add(":JAN", searchCondition.Jan);
            parameters.Add(":INVOICE_NO", searchCondition.InvoiceNo);


            // 全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<LocationViewModel>(sql.ToString(), parameters).Count();
            TotalItemCount = totalCount;

            return MvcDbContext.Current.Database.Connection.Query<LocationViewModel>(sql.ToString(), parameters).ToList();

        }

        /// <summary>
        /// ケースNoモーダル検索結果取得
        /// </summary>
        /// <returns>検索結果</returns>
        public List<CaseViewModel> ListingCase(PurchaseCorrectionSearchConditions searchCondition, ref int TotalItemCount)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                        MIS.ITEM_ID
                    ,   MIS.ITEM_NAME
                    ,   MIS.ITEM_SKU_ID
                    ,   MIS.ITEM_COLOR_ID
                    ,   MIS.ITEM_SIZE_ID
                    ,   MIS.JAN
                    ,   MC.ITEM_COLOR_NAME
                    ,   MS.ITEM_SIZE_NAME
                    ,   TS.LOCATION_CD
                    ,   MLC.LOCATION_NAME AS LOCATION_CLASS
                    ,   TS.GRADE_ID
                    ,   TS.BOX_NO
                    ,   TS.STOCK_QTY
                    ,   TS.ALLOC_QTY
                    ,   TS.STOCK_QTY - TS.ALLOC_QTY AS KANOU_QTY
                    ,   TS.INVOICE_NO
                FROM
                        T_PACKAGE_STOCKS TS
                INNER JOIN
                        M_ITEM_SKU MIS
                ON
                        TS.SHIPPER_ID = MIS.SHIPPER_ID
                    AND TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                INNER JOIN
                        M_LOCATIONS ML
                ON 
                        TS.SHIPPER_ID = ML.SHIPPER_ID
                    AND TS.CENTER_ID = ML.CENTER_ID
                    AND TS.LOCATION_CD = ML.LOCATION_CD
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT JOIN
                        M_LOCATION_CLASSES MLC
                ON
                        ML.SHIPPER_ID = MLC.SHIPPER_ID
                    AND ML.LOCATION_CLASS = MLC.LOCATION_CLASS
                WHERE
                        TS.SHIPPER_ID = :SHIPPER_ID
                    AND TS.CENTER_ID = :CENTER_ID
                    AND TS.JAN = :JAN
                    AND TS.LOCATION_CD = :LOCATION_CD
                    AND TS.INVOICE_NO = :INVOICE_NO
                ORDER BY  TS.BOX_NO
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", searchCondition.CenterId);
            parameters.Add(":JAN", searchCondition.Jan);
            parameters.Add(":LOCATION_CD", searchCondition.LocationCd);
            parameters.Add(":INVOICE_NO", searchCondition.InvoiceNo);

            // 全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<CaseViewModel>(sql.ToString(), parameters).Count();
            TotalItemCount = totalCount;

            return MvcDbContext.Current.Database.Connection.Query<CaseViewModel>(sql.ToString(), parameters).ToList();
        }

        /// <summary>
        /// Jan存在チェック
        /// </summary>
        /// <param name="jan"></param>
        /// <returns></returns>
        public bool ExistJan(string jan)
        {
            bool result = false;

            StringBuilder query = new StringBuilder(@"
                                SELECT COUNT(1)
                                  FROM M_ITEM_SKU MIS
                                 WHERE MIS.SHIPPER_ID = :SHIPPER_ID
                                   AND MIS.JAN = :JAN");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":JAN", jan);
            object outResult = MvcDbContext.Current.Database.Connection.ExecuteScalar(query.ToString(), parameters);
            if (outResult != null)
            {
                if (int.Parse(outResult.ToString()) != 0)
                {
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// 納品書番号存在チェック
        /// </summary>
        /// <param name="invoiceNo"></param>
        /// <returns></returns>
        public bool ExistInvoiceNo(string centerId, string jan, string invoiceNo)
        {
            bool result = false;

            StringBuilder query = new StringBuilder(@"
                SELECT
                        COUNT(1)
                FROM
                        (
                        SELECT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   INVOICE_NO
                            ,   MAX(ARRIVE_DATE) ARRIVE_DATE
                            ,   MAX(CONFIRM_DATE) CONFIRM_DATE
                            ,   MAX(ITEM_SKU_ID) ITEM_SKU_ID
                        FROM
                                T_ARRIVE_RESULTS
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                             ");
            if (jan.Length > 0)
            {
                query.Append("  AND JAN = :JAN");
            }
            query.Append(@" AND INVOICE_NO = :INVOICE_NO
                            AND IF_STATE = 2                   --実績送信済
                            AND CONFIRM_DATE >= TRUNC(LAST_DAY(ADD_MONTHS(SYSDATE,-2))+1)    --先月1日以降
                        GROUP BY
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   INVOICE_NO
                        UNION ALL
                        SELECT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   INVOICE_NO
                            ,   MAX(ARRIVE_DATE) ARRIVE_DATE
                            ,   MAX(CONFIRM_DATE) CONFIRM_DATE
                            ,   MAX(ITEM_SKU_ID) ITEM_SKU_ID
                        FROM
                                A_ARRIVE_RESULTS
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                             ");
            if (jan.Length > 0)
            {
                query.Append("  AND JAN = :JAN");
            }
            query.Append(@"
                            AND INVOICE_NO = :INVOICE_NO
                            AND CONFIRM_DATE >= TRUNC(LAST_DAY(ADD_MONTHS(SYSDATE,-2))+1)    --先月1日以降
                        GROUP BY
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   INVOICE_NO
                        ) TAR");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            parameters.Add(":INVOICE_NO", invoiceNo);
            if (jan.Length > 0)
            {
                parameters.Add(":JAN", jan);
            }

            object outResult = MvcDbContext.Current.Database.Connection.ExecuteScalar(query.ToString(), parameters);
            if (outResult != null)
            {
                if (int.Parse(outResult.ToString()) != 0)
                {
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// ロケーション存在チェック
        /// </summary>
        /// <param name="locationCd"></param>
        /// <returns></returns>
        public bool ExistLocationCd(string centerId, string jan, string locationCd)
        {
            bool result = false;

            StringBuilder query = new StringBuilder(@"
                SELECT COUNT(1)
                FROM 
                        T_STOCKS TS
                WHERE
                        TS.SHIPPER_ID = :SHIPPER_ID
                    AND TS.CENTER_ID = :CENTER_ID
                    AND TS.LOCATION_CD = :LOCATION_CD
                    AND TS.JAN = :JAN
            ");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            parameters.Add(":JAN", jan);
            parameters.Add(":LOCATION_CD", locationCd);
            object outResult = MvcDbContext.Current.Database.Connection.ExecuteScalar(query.ToString(), parameters);
            if (outResult != null)
            {
                if (int.Parse(outResult.ToString()) != 0)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// ロケーション存在チェック
        /// </summary>
        /// <param name="locationCd"></param>
        /// <returns></returns>
        public bool ChkCaseLocationCd(string centerId,  string locationCd)
        {
            bool result = false;

            StringBuilder query = new StringBuilder(@"
                SELECT COUNT(1)
                FROM 
                        M_LOCATIONS
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = :CENTER_ID
                    AND LOCATION_CD = :LOCATION_CD
                    AND CASE_CLASS <> 2
            ");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            parameters.Add(":LOCATION_CD", locationCd);
            object outResult = MvcDbContext.Current.Database.Connection.ExecuteScalar(query.ToString(), parameters);
            if (outResult != null)
            {
                if (int.Parse(outResult.ToString()) != 0)
                {
                    //バラ以外をTRUE
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// ロケーション選択時在庫チェック
        /// </summary>
        /// <param name="locationCd"></param>
        /// <returns></returns>
        public int GetStockQty(string jan, string locationCd)
        {
            StringBuilder query = new StringBuilder(@"
                SELECT STOCK_QTY
                FROM 
                        T_STOCKS
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND JAN = :JAN
                    AND LOCATION_CD = :LOCATION_CD
            ");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":JAN", jan);
            parameters.Add(":LOCATION_CD", locationCd);

            return MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).FirstOrDefault();
        }
        /// <summary>
        /// ケースNo存在チェック
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public bool ExistBoxNo(string centerId, string jan, string locationCd, string boxNo,string invoiceNo)
        {
            bool result = false;

            StringBuilder query = new StringBuilder(@"
                    SELECT COUNT(1)
                    FROM 
                            T_PACKAGE_STOCKS TS
                    WHERE
                            TS.SHIPPER_ID = :SHIPPER_ID
                        AND TS.CENTER_ID = :CENTER_ID
                        AND TS.JAN = :JAN
                        AND TS.INVOICE_NO = :INVOICE_NO
                        AND TS.LOCATION_CD = :LOCATION_CD
                        AND TS.BOX_NO = :BOX_NO
            ");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            parameters.Add(":JAN", jan);
            parameters.Add(":INVOICE_NO", invoiceNo);
            parameters.Add(":LOCATION_CD", locationCd);
            parameters.Add(":BOX_NO", boxNo);
            object outResult = MvcDbContext.Current.Database.Connection.ExecuteScalar(query.ToString(), parameters);
            if (outResult != null)
            {
                if (int.Parse(outResult.ToString()) != 0)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// ケース選択時在庫チェック
        /// </summary>
        /// <param name="locationCd"></param>
        /// <returns></returns>
        public int GetCaseStockQty(string centerId, string jan, string locationCd, string boxNo, string invoiceNo)
        {
            StringBuilder query = new StringBuilder(@"
                    SELECT STOCK_QTY
                    FROM 
                            T_PACKAGE_STOCKS TS
                    WHERE
                            TS.SHIPPER_ID = :SHIPPER_ID
                        AND TS.CENTER_ID = :CENTER_ID
                        AND TS.JAN = :JAN
                        AND TS.INVOICE_NO = :INVOICE_NO
                        AND TS.LOCATION_CD = :LOCATION_CD
                        AND TS.BOX_NO = :BOX_NO
            ");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            parameters.Add(":JAN", jan);
            parameters.Add(":INVOICE_NO", invoiceNo);
            parameters.Add(":LOCATION_CD", locationCd);
            parameters.Add(":BOX_NO", boxNo);

            return MvcDbContext.Current.Database.Connection.Query<int>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// バラ在庫存在チェック
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public bool ExistBaraStock(string centerId, string jan, string locationCd, string invoiceNo)
        {
            bool result = false;

            //固定ロケーションの場合
            var GenCd = GetLocGenCd(locationCd);

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                    SELECT COUNT(1)
                    FROM 
                            T_PACKAGE_STOCKS TS
                    WHERE
                            TS.SHIPPER_ID = :SHIPPER_ID
                        AND TS.CENTER_ID = :CENTER_ID
                        AND TS.JAN = :JAN
                        AND TS.LOCATION_CD = :LOCATION_CD
                        AND TS.BOX_NO = ' '
            ");
            //DC入荷仮ロケもしくはTC入荷仮ロケの場合のみ納品書番号をみる
            if (GenCd == "NYK-DC1" || GenCd == "NYK-TC1")
            {
                query.Append(@" AND TS.INVOICE_NO = :INVOICE_NO ");
                parameters.Add(":INVOICE_NO", invoiceNo);
            }

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            parameters.Add(":JAN", jan);
            parameters.Add(":LOCATION_CD", locationCd);
            object outResult = MvcDbContext.Current.Database.Connection.ExecuteScalar(query.ToString(), parameters);
            if (outResult != null)
            {
                if (int.Parse(outResult.ToString()) != 0)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 固定ロケーションの汎用コード取得
        /// </summary>
        /// <param name="locationCd"></param>
        /// <returns></returns>
        public string GetLocGenCd(string locationCd)
        {
            StringBuilder queryloc = new StringBuilder(@"
                SELECT
                        GEN_CD
                FROM
                        M_GENERALS
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = '@@@'
                    AND REGISTER_DIVI_CD = '1'
                    AND GEN_DIV_CD = 'FIXED_LOCATION_CD'
                    AND GEN_NAME = :LOCATION_CD
            ");
            DynamicParameters locparameters = new DynamicParameters();
            locparameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            locparameters.Add(":LOCATION_CD", locationCd);
            return MvcDbContext.Current.Database.Connection.Query<string>(queryloc.ToString(), locparameters).FirstOrDefault();
        }

        /// <summary>
        /// TC入出荷仕分状況チェック(TC入荷仮ロケ用)
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool CheckStocKSortStatus(PurchaseCorrectionSearchConditions condition, out int status, out string message)
        {
            var GenCd = GetLocGenCd(condition.LocationCd);

            //入荷仮ロケ
            if (GenCd == "NYK-TC1")
            {
                StringBuilder query = new StringBuilder(@"
                SELECT
                        MAX(SORT_STATUS) AS SORT_STATUS
                FROM
                        T_PACKAGE_STOCKS
                WHERE
                        JAN = :JAN
                    AND LOCATION_CD = :LOCATION_CD
                    AND INVOICE_NO = :INVOICE_NO
                    AND CENTER_ID = :CENTER_ID
                    AND SHIPPER_ID = :SHIPPER_ID
                ");

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(":JAN", condition.Jan);
                parameters.Add(":LOCATION_CD", condition.LocationCd);
                parameters.Add(":INVOICE_NO", condition.InvoiceNo);
                parameters.Add(":CENTER_ID", condition.CenterId);
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                var sort_status = MvcDbContext.Current.Database.Connection.Query<int?>(query.ToString(), parameters).FirstOrDefault();
                if (sort_status != null && condition.SaiQty > 0 && sort_status > 1)
                {
                    status = -101;
                    message = PurchaseCorrectionResource.SortStatusAllocComErr;
                    return false;
                }
                if (sort_status != null && condition.SaiQty < 0 && sort_status == 2)
                {
                    status = -102;
                    message = PurchaseCorrectionResource.SortStatusAllocErr;
                    return false;
                }
                if (sort_status != null && condition.SaiQty < 0 && sort_status == 3 )
                {
                    status = -103;
                    message = PurchaseCorrectionResource.SortStatusMsg;
                    return false;
                }
            }
            status = 0;
            message = "";
            return true;
        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertPurchaseCorrection(PurchaseCorrectionSearchConditions condition, int isConfirmed, out int status, out string message)
        {
            int wk_status = 0;
            string wK_message = "";
            if(isConfirmed != 1 && CheckStocKSortStatus(condition, out wk_status, out wK_message) == false)
            {
                status = wk_status;
                message = wK_message;
                return false;
            }
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    // 1.ワークID採番
                    if (condition.Seq == 0)
                    {
                        condition.Seq = new BaseQuery().GetWorkId();
                    }
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_RET_PURCHASE_CORRECTION(
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
                              ,CENTER_ID
                              ,RETURN_QTY
                            ");
                    //仕入先名
                    if (condition.LocationCd != null)
                    {
                        query.Append(" ,LOCATION_CD ");
                    }

                    //JAN
                    if (condition.Jan != null)
                    {
                        query.Append(" ,JAN ");
                    }

                    //納品書番号
                    if (condition.InvoiceNo != null)
                    {
                        query.Append(" ,INVOICE_NO ");
                    }

                    //ケースNo
                    if (condition.BoxNo != null)
                    {
                        query.Append(" ,BOX_NO ");
                    }

                    query.Append(@")
                        VALUES(
                               SYSTIMESTAMP
                              ,:MAKE_USER_ID 
                              ,'PurchaseCorrection'
                              ,SYSTIMESTAMP 
                              ,:MAKE_USER_ID
                              ,'PurchaseCorrection'
                              ,0
                              ,:SHIPPER_ID
                              ,:SEQ
                              ,NVL((SELECT MAX(LINE_NO) + 1 FROM WW_RET_PURCHASE_CORRECTION WHERE SEQ = :SEQ AND SHIPPER_ID = :SHIPPER_ID AND CENTER_ID = :CENTER_ID),1)
                              ,:CENTER_ID
                              ,:RETURN_QTY
                    ");

                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    parameters.Add(":MAKE_USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":RETURN_QTY", condition.SaiQty);

                    //ロケ
                    if (condition.LocationCd != null)
                    {
                        query.Append(" ,:LOCATION_CD ");
                        parameters.Add(":LOCATION_CD", condition.LocationCd);
                    }

                    //JAN
                    if (condition.Jan != null)
                    {
                        query.Append(" ,:JAN ");
                        parameters.Add(":JAN", condition.Jan);
                    }

                    //納品書番号
                    if (condition.InvoiceNo != null)
                    {
                        query.Append(" ,:INVOICE_NO ");
                        parameters.Add(":INVOICE_NO", condition.InvoiceNo);
                    }

                    //ケースNo
                    if (condition.BoxNo != null)
                    {
                        query.Append(" ,:BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo);
                    }
                    query.Append(" ) ");
                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    trans.Rollback();
                    status = -1;
                    message = "";
                    return false;
                }
                trans.Commit();
                status = 0;
                message = "";
                return true;
            }
        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool DeletePurchaseCorrection(PurchaseCorrectionSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        DELETE FROM
                            WW_RET_PURCHASE_CORRECTION
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                            AND SEQ = :SEQ
                            AND LINE_NO = :LINE_NO
                     ");

                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":LINE_NO", condition.LineNo);

                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    trans.Rollback();
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
        public IList<PurchaseCorrectionResultRow> GetPurchaseCorrection(PurchaseCorrectionSearchConditions condition)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                StringBuilder query = new StringBuilder(@"
                    SELECT
                            WW.LINE_NO
                        ,   TAR.VENDOR_ID AS VENDOR_ID
                        ,   MV.VENDOR_NAME1 AS VENDOR_NAME
                        ,   CTGRI.CATEGORY_NAME1
                        ,   MIS.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   MIS.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   MIS.ITEM_SIZE_ID
                        ,   MS.ITEM_SIZE_NAME
                        ,   MIS.JAN
                        ,   WW.BOX_NO
                        ,   WW.LOCATION_CD
                        ,   WW.INVOICE_NO
                        ,   PLANS.NORMAL_BUYING_PRICE             --下代
                        ,   PLANS.PURCHASE_BUYING_PRICE           --W下代
                        ,   PLANS.NORMAL_SELLING_PRICE_EX_TAX     --上代
                        ,   WW.RETURN_QTY AS SAI_QTY                      --差異数
                        ,   WW.SEQ
                      FROM WW_RET_PURCHASE_CORRECTION WW
                      INNER JOIN M_ITEM_SKU MIS
                        ON WW.SHIPPER_ID = MIS.SHIPPER_ID
                       AND WW.JAN = MIS.JAN
                      LEFT JOIN (
                            SELECT
                                NORMAL_BUYING_PRICE             --下代
                            ,   PURCHASE_BUYING_PRICE           --W下代
                            ,   NORMAL_SELLING_PRICE_EX_TAX     --上代
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   INVOICE_NO
                            ,   JAN
                            FROM
                                T_ARRIVE_PLANS
                            WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                            UNION ALL
                            SELECT
                                NORMAL_BUYING_PRICE             --下代
                            ,   PURCHASE_BUYING_PRICE           --W下代
                            ,   NORMAL_SELLING_PRICE_EX_TAX     --上代
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                            ,   INVOICE_NO
                            ,   JAN
                            FROM
                                A_ARRIVE_PLANS
                            WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND CENTER_ID = :CENTER_ID
                        ) PLANS
                        ON WW.SHIPPER_ID = PLANS.SHIPPER_ID
                       AND WW.CENTER_ID = PLANS.CENTER_ID
                       AND WW.INVOICE_NO = PLANS.INVOICE_NO
                       AND WW.JAN = PLANS.JAN
                      LEFT JOIN (
                            SELECT
                                    TAR.SHIPPER_ID
                                ,   TAR.CENTER_ID
                                ,   TAR.INVOICE_NO
                                ,   TAR.JAN
                                ,   TAR.VENDOR_ID
                            FROM
                                    T_ARRIVE_RESULTS TAR
                            WHERE
                                    TAR.SHIPPER_ID = :SHIPPER_ID
                                AND TAR.CENTER_ID = :CENTER_ID
                            UNION ALL
                            SELECT
                                    AAR.SHIPPER_ID
                                ,   AAR.CENTER_ID
                                ,   AAR.INVOICE_NO
                                ,   AAR.JAN
                                ,   AAR.VENDOR_ID
                            FROM
                                    A_ARRIVE_RESULTS AAR
                            WHERE
                                    AAR.SHIPPER_ID = :SHIPPER_ID
                                AND AAR.CENTER_ID = :CENTER_ID
                      ) TAR
                      ON
                            WW.SHIPPER_ID = TAR.SHIPPER_ID
                        AND WW.CENTER_ID = TAR.CENTER_ID
                        AND WW.INVOICE_NO = TAR.INVOICE_NO
                        AND WW.JAN = TAR.JAN
                      LEFT JOIN M_COLORS MC
                        ON MIS.SHIPPER_ID = MC.SHIPPER_ID
                       AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                      LEFT JOIN M_SIZES MS
                        ON MIS.SHIPPER_ID = MS.SHIPPER_ID
                       AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                      LEFT JOIN M_ITEM_CATEGORIES4 CTGRI
                        ON CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                       AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                       AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                       AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                       AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                      LEFT JOIN M_VENDORS MV
                        ON TAR.SHIPPER_ID = MV.SHIPPER_ID
                       AND TAR.VENDOR_ID = MV.VENDOR_ID
                     WHERE
                           WW.SHIPPER_ID = :SHIPPER_ID
                       AND WW.CENTER_ID = :CENTER_ID
                       AND WW.SEQ = :SEQ
                    ORDER BY
                            WW.LINE_NO
                ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", condition.CenterId);
                parameters.Add(":SEQ", condition.Seq);

                // 2.検索・ワーク作成
                return MvcDbContext.Current.Database.Connection.Query<PurchaseCorrectionResultRow>(query.ToString(), parameters).ToList();

            }
            catch (DbUpdateException ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "GetReturnInfo");
                throw;
            }
        }

        /// <summary>
        /// 仕入訂正確定ストアド実行処理
        /// </summary>
        public void UpdRetPurchaseCorrection(PurchaseCorrectionInputViewModel InputResult, out ProcedureStatus status, out string message, out string returnId)
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
            "SP_W_RET_PURCHASE_CORRECTION_UPD",
            param,
            commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
            returnId = param.Get<string>("OUT_RETURN_ID");
        }
    }
}