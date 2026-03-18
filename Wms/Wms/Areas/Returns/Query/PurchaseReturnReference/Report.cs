namespace Wms.Areas.Returns.Query.PurchaseReturnReference
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Returns.ViewModels.PurchaseCorrection;
    using Wms.Areas.Returns.ViewModels.PurchaseReturnReference;
    using Wms.Areas.Returns.ViewModels.PurchaseReturns;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Returns.ViewModels.PurchaseReturnReference.PurchaseReturnReference01SearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PurchaseReturnReference01Report> PurchaseReturnReference01Listing(PurchaseReturnReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            string order;
            switch (condition.SortKey)
            {
                case PurchaseReturnReference01SortKey.ReturnId:
                    switch (condition.Sort)
                    {
                        case PurchaseReturnReference01SearchConditions.AscDescSort.Desc:
                            order = "ORDER BY RETURN_ID DESC ";
                            break;

                        default:
                            order = "ORDER BY RETURN_ID ASC ";
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case PurchaseReturnReference01SearchConditions.AscDescSort.Desc:
                            order = "ORDER BY WW.VENDOR_ID DESC,WW.RETURN_ID DESC ";
                            break;

                        default:
                            order = "ORDER BY WW.VENDOR_ID ASC,WW.RETURN_ID ASC ";
                            break;
                    }

                    break;
            }

            StringBuilder query = new StringBuilder(@"
                 SELECT
                        ROW_NUMBER() OVER ( ");
            query.AppendLine(order);
            query.AppendLine(@"
                        ) AS LINE_NO
                    ,   WW.VENDOR_ID
                    ,   MV.VENDOR_NAME1    AS VENDOR_NAME
                    ,   CTGRI.CATEGORY_NAME1 AS CATEGORY_NAME
                    ,   WW.ITEM_ID
                    ,   MIS.ITEM_NAME
                    ,   WW.ITEM_COLOR_ID
                    ,   MC.ITEM_COLOR_NAME
                    ,   WW.ITEM_SIZE_ID
                    ,   MS.ITEM_SIZE_NAME
                    ,   MIS.ITEM_SKU_ID
                    ,   WW.JAN
                    ,   WW.RETURN_QTY
                    ,   WW.NORMAL_BUYING_PRICE
                    ,   WW.PURCHASE_BUYING_PRICE
                    ,   WW.NORMAL_SELLING_PRICE_EX_TAX
                    ,   WW.INVOICE_NO
                    ,   CASE WW.RETUEN_CLASS WHEN 0 THEN '仕入先返品' ELSE '仕入訂正' END AS RETURN_CLASS_NAME
                    ,   WW.SHIPPER_ID
                    ,   WW.CENTER_ID
                    ,   WW.RETURN_ID
                    ,   TO_CHAR(WW.ARRIVE_DATE, 'YYYY/MM/DD')  ARRIVE_DATE        --返品登録日
                    ,   WW.INPUT_USER_ID        --返品実績者ID
                    ,   WW.INPUT_USER_NAME      --返品実績者名
                FROM
                        WW_RET_PURCHASE_REFERENCE WW
                INNER JOIN M_ITEM_SKU MIS
                ON 
                        WW.SHIPPER_ID = MIS.SHIPPER_ID
                    AND WW.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT JOIN M_ITEM_CATEGORIES4 CTGRI
                ON
                        CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                    AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                    AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                    AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                    AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                LEFT JOIN M_VENDORS MV
                ON
                       WW.SHIPPER_ID = MV.SHIPPER_ID
                    AND WW.VENDOR_ID = MV.VENDOR_ID
                WHERE
                        WW.SHIPPER_ID = :SHIPPER_ID
                    AND WW.CENTER_ID = :CENTER_ID
                    AND WW.SEQ = :SEQ
            ");
            query.AppendLine(order);
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseReturnReference01Report>(query.ToString(), parameters);
        }

        /// <summary>
        /// 帳票に出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        public IEnumerable<PurchaseReturnsReport> GetResultRowListReturn(PurchaseReturnReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                 SELECT
                            :USER_ID || '　' || MU.USER_NAME PRINT_USER
                        ,   TO_CHAR(WW.ARRIVE_DATE, 'YYYY/MM/DD') AS ARRIVE_DATE
                        ,   WW.RETURN_ID
                        ,   MCS.CENTER_ZIP
                        ,   MCS.CENTER_PREF_NAME || MCS.CENTER_CITY_NAME || MCS.CENTER_ADDRESS1 || CENTER_ADDRESS2 AS CENTER_ADDRESS
                        ,   MCS.CENTER_TEL
                        ,   MCS.CENTER_NAME1 AS CENTER_NAME
                        ,   CTGRI.CATEGORY_ID1
                        ,   CTGRI.CATEGORY_NAME1
                        ,   WW.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   WW.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   WW.ITEM_SIZE_ID
                        ,   MS.ITEM_SIZE_NAME
                        ,   SUBSTR(WW.JAN,1,7) JAN1
                        ,   SUBSTR(WW.JAN,8,6) JAN2
                        ,   WW.NORMAL_BUYING_PRICE * WW.RETURN_QTY AS NORMAL_BUYING_PRICE
                        ,   WW.RETURN_QTY
                        ,   WW.VENDOR_ID || MV.VENDOR_NAME1 AS VENDOR_NAME
                        ,   TO_CHAR(SYSDATE, 'YYYY/MM/DD HH24:MI:SS') AS PRINT_DATE
                FROM
                        WW_RET_PURCHASE_REFERENCE WW
                INNER JOIN
                        M_USERS MU
                ON 
                        WW.SHIPPER_ID   = MU.SHIPPER_ID
                    AND :USER_ID = MU.USER_ID
                INNER JOIN
                        M_CENTERS MCS
                    ON
                        WW.SHIPPER_ID   = MCS.SHIPPER_ID
                    AND WW.CENTER_ID   = MCS.CENTER_ID
                INNER JOIN M_ITEM_SKU MIS
                ON 
                        WW.SHIPPER_ID = MIS.SHIPPER_ID
                    AND WW.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT JOIN M_ITEM_CATEGORIES4 CTGRI
                ON
                        CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                    AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                    AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                    AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                    AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                LEFT JOIN M_VENDORS MV
                ON
                       WW.SHIPPER_ID = MV.SHIPPER_ID
                    AND WW.VENDOR_ID = MV.VENDOR_ID
                WHERE
                        WW.SHIPPER_ID = :SHIPPER_ID
                    AND WW.CENTER_ID = :CENTER_ID
                    AND WW.SEQ = :SEQ
                    AND WW.RETURN_ID = :RETURN_ID
                ORDER BY
                        WW.VENDOR_ID
                    ,   MIS.CATEGORY_ID1
                    ,   ITEM_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);
            parameters.Add(":RETURN_ID", condition.HidReturnId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseReturnsReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 帳票に出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        public IEnumerable<PurchaseCorrectionReport> GetResultRowListCorrection(PurchaseReturnReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                 SELECT
                            :USER_ID || '　' || MU.USER_NAME PRINT_USER
                        ,   TO_CHAR(ARRIVE_DATE, 'YYYY/MM/DD') AS ARRIVE_DATE
                        ,   :RETURN_ID AS RETURN_ID
                        ,   MCS.CENTER_ZIP
                        ,   MCS.CENTER_PREF_NAME || MCS.CENTER_CITY_NAME || MCS.CENTER_ADDRESS1 || CENTER_ADDRESS2 AS CENTER_ADDRESS
                        ,   MCS.CENTER_TEL
                        ,   MCS.CENTER_NAME1 AS CENTER_NAME
                        ,   CTGRI.CATEGORY_ID1
                        ,   CTGRI.CATEGORY_NAME1
                        ,   WW.VENDOR_ID AS VENDOR_ID
                        ,   MV.VENDOR_NAME1 AS VENDOR_NAME
                        ,   MIS.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   MIS.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   MIS.ITEM_SIZE_ID
                        ,   MS.ITEM_SIZE_NAME
                        ,   SUBSTR(MIS.JAN,1,7) JAN1
                        ,   SUBSTR(MIS.JAN,8,6) JAN2
                        ,   '' AS BOX_NO
                        ,   '' AS LOCATION_CD
                        ,   WW.INVOICE_NO
                        ,   WW.NORMAL_BUYING_PRICE             --下代
                        ,   WW.PURCHASE_BUYING_PRICE           --W下代
                        ,   WW.NORMAL_SELLING_PRICE_EX_TAX     --上代
                        ,   WW.RETURN_QTY                      --差異数
                        ,   TO_CHAR(SYSDATE, 'YYYY/MM/DD HH24:MI:SS') AS PRINT_DATE
                FROM
                        WW_RET_PURCHASE_REFERENCE WW
                INNER JOIN
                        M_USERS MU
                ON 
                        WW.SHIPPER_ID   = MU.SHIPPER_ID
                    AND :USER_ID = MU.USER_ID
                INNER JOIN
                        M_CENTERS MCS
                    ON
                        WW.SHIPPER_ID   = MCS.SHIPPER_ID
                    AND WW.CENTER_ID   = MCS.CENTER_ID
                INNER JOIN M_ITEM_SKU MIS
                ON 
                        WW.SHIPPER_ID = MIS.SHIPPER_ID
                    AND WW.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                LEFT JOIN M_COLORS MC
                ON
                        MIS.SHIPPER_ID = MC.SHIPPER_ID
                    AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                LEFT JOIN M_SIZES MS
                ON
                        MIS.SHIPPER_ID = MS.SHIPPER_ID
                    AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                LEFT JOIN M_ITEM_CATEGORIES4 CTGRI
                ON
                        CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                    AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                    AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                    AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                    AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                LEFT JOIN M_VENDORS MV
                ON
                       WW.SHIPPER_ID = MV.SHIPPER_ID
                    AND WW.VENDOR_ID = MV.VENDOR_ID
                WHERE
                        WW.SHIPPER_ID = :SHIPPER_ID
                    AND WW.CENTER_ID = :CENTER_ID
                    AND WW.SEQ = :SEQ
                    AND WW.RETURN_ID = :RETURN_ID
                ORDER BY
                        WW.VENDOR_ID
                    ,   MIS.CATEGORY_ID1
                    ,   ITEM_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":SEQ", condition.Seq);
            parameters.Add(":RETURN_ID", condition.HidReturnId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseCorrectionReport>(query.ToString(), parameters);
        }
    }
}