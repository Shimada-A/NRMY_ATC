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
    using Wms.Areas.Returns.ViewModels.PurchaseCorrection;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Returns.ViewModels.PurchaseCorrection.PurchaseCorrectionSearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PurchaseCorrectionReport> ReturnListingForCsv(PurchaseCorrectionSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                    SELECT
                            :USER_ID || '　' || MU.USER_NAME PRINT_USER
                        ,   TO_CHAR(SYSDATE, 'YYYY/MM/DD') AS ARRIVE_DATE
                        ,   :RETURN_ID AS RETURN_ID
                        ,   MCS.CENTER_ZIP
                        ,   MCS.CENTER_PREF_NAME || MCS.CENTER_CITY_NAME || MCS.CENTER_ADDRESS1 || CENTER_ADDRESS2 AS CENTER_ADDRESS
                        ,   MCS.CENTER_TEL
                        ,   MCS.CENTER_NAME1 AS CENTER_NAME
                        ,   CTGRI.CATEGORY_ID1
                        ,   CTGRI.CATEGORY_NAME1
                        ,   TAR.VENDOR_ID AS VENDOR_ID
                        ,   MV.VENDOR_NAME1 AS VENDOR_NAME
                        ,   MIS.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   MIS.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   MIS.ITEM_SIZE_ID
                        ,   MS.ITEM_SIZE_NAME
                        ,   SUBSTR(MIS.JAN,1,7) JAN1
                        ,   SUBSTR(MIS.JAN,8,6) JAN2
                        ,   WW.BOX_NO
                        ,   WW.LOCATION_CD
                        ,   WW.INVOICE_NO
                        ,   PLANS.NORMAL_BUYING_PRICE             --下代
                        ,   PLANS.PURCHASE_BUYING_PRICE           --W下代
                        ,   PLANS.NORMAL_SELLING_PRICE_EX_TAX     --上代
                        ,   WW.RETURN_QTY                      --差異数
                        ,   TO_CHAR(SYSDATE, 'YYYY/MM/DD HH24:MI:SS') AS PRINT_DATE
                      FROM WW_RET_PURCHASE_CORRECTION WW
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
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            parameters.Add(":RETURN_ID", condition.ReturnId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseCorrectionReport>(query.ToString(), parameters);
        }
    }
}