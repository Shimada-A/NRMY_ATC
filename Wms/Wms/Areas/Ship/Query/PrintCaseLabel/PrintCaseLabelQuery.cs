namespace Wms.Areas.Ship.Query.PrintCaseLabel
{
    using Dapper;
    using Share.Common;
    using StackExchange.Redis;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Wms.Areas.Ship.ViewModels.PrintCaseLabel;
    using Wms.Common;
    using Wms.Models;

    public class PrintCaseLabelQuery
    {
        /// <summary>
        /// 出荷梱包実績存在チェック
        /// </summary>
        public string BoxNoCheck(PrintCaseLabelConditions SearchConditions)
        {
            var ship_packing_data = MvcDbContext.Current.ShipPackingInfoes
                .Where(m => m.BoxNo == SearchConditions.ReleaseBoxNo
                         && m.CenterId == SearchConditions.CenterId
                         && m.ShipperId == Profile.User.ShipperId)
                .Select(m => m.BoxNo)
                .ToList();

            return ship_packing_data.Count > 0 ? "0" : "1";
        }

        /// <summary>
        /// 店舗・配送業者全件取得
        /// </summary>
        /// <param name="p_centerId"></param>
        /// <returns></returns>
        public string StoreAllCount(PrintCaseLabelConditions conditions)
        {
            string sql, storesSql;

            string storeOutletsSql = string.Empty;
            if (conditions.StoreOutletsClass == PrintCaseLabelConditions.StoreOutletsClasses.NotOutlets)
            {
                storeOutletsSql = "AND NOT EXISTS (SELECT 1 FROM M_GENERALS SUB WHERE SUB.SHIPPER_ID = M_SHIP_FRONTAGE.SHIPPER_ID AND SUB.CENTER_ID = '@@@' AND SUB.REGISTER_DIVI_CD ＝ '1' AND SUB.GEN_DIV_CD = 'EVENT_STORE_CODE' AND SUB.GEN_CD = M_SHIP_FRONTAGE.STORE_ID )";
            }
            else if (conditions.StoreOutletsClass == PrintCaseLabelConditions.StoreOutletsClasses.OnlyOutlets)
            {
                storeOutletsSql = "AND EXISTS (SELECT 1 FROM M_GENERALS SUB WHERE SUB.SHIPPER_ID = M_SHIP_FRONTAGE.SHIPPER_ID AND SUB.CENTER_ID = '@@@' AND SUB.REGISTER_DIVI_CD ＝ '1' AND SUB.GEN_DIV_CD = 'EVENT_STORE_CODE' AND SUB.GEN_CD = M_SHIP_FRONTAGE.STORE_ID )";
            }

            if (conditions.StoreClass == PrintCaseLabelConditions.StoreClasses.Store)
            {
                storesSql = $@"
                    SELECT
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   STORE_ID AS SHIP_TO_STORE_ID
                    FROM
                            M_SHIP_FRONTAGE
                    WHERE
                            CENTER_ID = :CENTER_ID
                        AND SHIPPER_ID = :SHIPPER_ID
                        {((!string.IsNullOrEmpty(conditions.BrandId))? "AND BRAND_ID = :BRAND_ID" : null)}
                        {((!string.IsNullOrEmpty(storeOutletsSql)) ? storeOutletsSql : null)} 
                    GROUP BY
                           SHIPPER_ID
                       ,   CENTER_ID
                       ,   STORE_ID
                    ";
            }
            else
            {
                storesSql = $@"
                    SELECT
                            SHIPPER_ID
                        ,   :CENTER_ID AS CENTER_ID
                        ,   SHIP_TO_STORE_ID
                    FROM
                            V_SHIP_TO_STORES
                    WHERE
                            SHIP_TO_STORE_CLASS = '8'      --倉庫
                        AND SHIPPER_ID = :SHIPPER_ID
                ";
            }

            sql = $@"
                WITH
                    TARGET_LOC_TRANSPORTERS AS (
                        SELECT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   SHIP_TO_STORE_ID
                        FROM
                                M_LOC_TRANSPORTERS
                        WHERE
                                START_DATE <= TRUNC(SYSDATE)
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                        GROUP BY
                                SHIP_TO_STORE_ID
                            ,   CENTER_ID
                            ,   SHIPPER_ID
                    )
                    ,TARGET_V_SHIP_TO_STORES AS ({storesSql})
                    ,SHIP_DATA AS (
                        SELECT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   SHIP_TO_STORE_ID
                        FROM
                                T_SHIPS
                        WHERE
                                CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            {((!string.IsNullOrEmpty(conditions.BatchNo)) ? "AND BATCH_NO = :BATCH_NO" : null)}
                        GROUP BY
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   SHIP_TO_STORE_ID
                    )
                SELECT 
                        COUNT(*)
                FROM
                        TARGET_V_SHIP_TO_STORES STOR
                INNER JOIN
                        TARGET_LOC_TRANSPORTERS LTRAN
                ON
                        LTRAN.SHIP_TO_STORE_ID = STOR.SHIP_TO_STORE_ID
                    AND LTRAN.CENTER_ID = STOR.CENTER_ID
                    AND LTRAN.SHIPPER_ID = STOR.SHIPPER_ID
            ";

            if (conditions.ShipInstructFlag)
            {
                sql += $@"
                    INNER JOIN
                            SHIP_DATA SHIP
                    ON
                            SHIP.SHIP_TO_STORE_ID = STOR.SHIP_TO_STORE_ID
                        AND SHIP.CENTER_ID = STOR.CENTER_ID
                        AND SHIP.SHIPPER_ID = STOR.SHIPPER_ID
                ";
            }

            return MvcDbContext.Current.Database.Connection.Query<string>(sql, 
                new
                {
                    SHIPPER_ID = Profile.User.ShipperId,
                    CENTER_ID = conditions.CenterId,
                    BRAND_ID = conditions.BrandId,
                    BATCH_NO = conditions.BatchNo
                }).SingleOrDefault();
        }

        /// <summary>
        /// ブランド別作業区分取得
        /// </summary>
        /// <param name="centerId"></param>
        /// <returns></returns>
        public string GetbrandWorkClass(string centerId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                    SELECT
                            BRAND_WORK_CLASS
                    FROM
                            M_CENTERS
                    WHERE
                            CENTER_ID = :CENTER_ID
                        AND SHIPPER_ID = :SHIPPER_ID
                        AND DELETE_FLAG = 0
            ");
            parameters.AddDynamicParams(new { CENTER_ID = centerId });
            parameters.AddDynamicParams(new { SHIPPER_ID = Profile.User.ShipperId });

            return MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).SingleOrDefault();
        }

        /// <summary>
        /// 直接印刷プリンタ名を取得
        /// </summary>
        /// <param name="centerId">倉庫ID</param>
        /// <param name="reportId">帳票識別ID</param>
        /// <returns></returns>
        public string GetPrinterName(string centerId, string reportId)
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.RegisterDiviCd == "1" &&
                            m.GenDivCd == "PRINTER_NAME" &&
                            m.GenCd == reportId &&
                            m.CenterId == centerId &&
                            m.ShipperId == Profile.User.ShipperId)
                .Select(m => m.GenName)
                .SingleOrDefault();
        }
    }
}