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

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PurchaseReturnsReport> ReturnListingForCsv(PurchaseReturnsSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                    SELECT
                            :USER_ID || '　' || MU.USER_NAME PRINT_USER
                        ,   TO_CHAR(TMR.ARRIVE_DATE, 'YYYY/MM/DD') AS ARRIVE_DATE
                        ,   TMR.RETURN_ID
                        ,   MCS.CENTER_ZIP
                        ,   MCS.CENTER_PREF_NAME || MCS.CENTER_CITY_NAME || MCS.CENTER_ADDRESS1 || CENTER_ADDRESS2 AS CENTER_ADDRESS
                        ,   MCS.CENTER_TEL
                        ,   MCS.CENTER_NAME1 AS CENTER_NAME
                        ,   CTGRI.CATEGORY_ID1
                        ,   CTGRI.CATEGORY_NAME1
                        ,   TMR.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   TMR.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   TMR.ITEM_SIZE_ID
                        ,   MS.ITEM_SIZE_NAME
                        ,   SUBSTR(TMR.JAN,1,7) JAN1
                        ,   SUBSTR(TMR.JAN,8,6) JAN2
                        ,   TMR.NORMAL_BUYING_PRICE * TMR.RETURN_QTY AS NORMAL_BUYING_PRICE
                        ,   TMR.RETURN_QTY
                        ,   TMR.VENDOR_ID || MV.VENDOR_NAME1 AS VENDOR_NAME
                        ,   TO_CHAR(SYSDATE, 'YYYY/MM/DD HH24:MI:SS') AS PRINT_DATE
                    FROM
                            T_MKRETURN_RESULTS TMR
                    INNER JOIN
                            M_USERS MU
                    ON 
                            TMR.SHIPPER_ID   = MU.SHIPPER_ID
                        AND :USER_ID = MU.USER_ID
                    INNER JOIN
                            M_CENTERS MCS
                    ON
                            TMR.SHIPPER_ID   = MCS.SHIPPER_ID
                        AND TMR.CENTER_ID   = MCS.CENTER_ID
                    LEFT JOIN 
                            M_ITEM_SKU MIS
                    ON 
                            TMR.SHIPPER_ID = MIS.SHIPPER_ID
                        AND TMR.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                    LEFT JOIN 
                            M_COLORS MC
                    ON 
                            TMR.SHIPPER_ID = MC.SHIPPER_ID
                        AND TMR.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                    LEFT JOIN 
                            M_SIZES MS
                    ON 
                            TMR.SHIPPER_ID = MS.SHIPPER_ID
                        AND TMR.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                    LEFT JOIN 
                            M_ITEM_CATEGORIES4 CTGRI
                    ON
                            CTGRI.CATEGORY_ID1 = MIS.CATEGORY_ID1
                        AND CTGRI.CATEGORY_ID2 = MIS.CATEGORY_ID2
                        AND CTGRI.CATEGORY_ID3 = MIS.CATEGORY_ID3
                        AND CTGRI.CATEGORY_ID4 = MIS.CATEGORY_ID4
                        AND CTGRI.SHIPPER_ID = MIS.SHIPPER_ID
                    LEFT JOIN 
                            M_VENDORS MV
                    ON 
                            TMR.SHIPPER_ID = MV.SHIPPER_ID
                        AND TMR.VENDOR_ID = MV.VENDOR_ID
                    WHERE
                            TMR.SHIPPER_ID = :SHIPPER_ID
                        AND TMR.CENTER_ID = :CENTER_ID
                        AND TMR.RETURN_ID = :RETURN_ID
                    ORDER  BY
                            TMR.RETURN_ID
                        ,   TMR.RETURN_SEQ
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", Profile.User.CenterId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            parameters.Add(":RETURN_ID", condition.ReturnId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PurchaseReturnsReport>(query.ToString(), parameters);
        }
    }
}