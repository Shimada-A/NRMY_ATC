namespace Wms.Areas.Ship.Query.PrintCaseLabel
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.PrintCaseLabel;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Csvに出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PrintCaseLabelIssueCsv> PrintBtoBIssueListing(PrintCaseLabelConditions condition)
        {
            List<PrintCaseLabelIssueCsv> pcl = new List<PrintCaseLabelIssueCsv>();
            string strShipToStoreId = condition.ShipToStoreId;
            string strTransporterId = condition.TransporterId;
            string[] ShipToStoreIdArray = strShipToStoreId.Split(',');
            string[] TransporterIdArray = strTransporterId.Split(',');

            for (var i = 0; i < ShipToStoreIdArray.Length; i++)
            {
                for (var j = 0; j < condition.NumberofSheets; j++)
                {
                    DynamicParameters parameters = new DynamicParameters();

                    StringBuilder query = new StringBuilder($@"
                        SELECT
                                CENTER.CENTER_ID || '　' || CENTER.CENTER_NAME1 AS CENTER
                            ,   CASE 
                                    WHEN STOR.SHIP_TO_STORE_CLASS = '8' THEN '{PrintCaseLabelResource.CenterMove}'
                                    ELSE '{PrintCaseLabelResource.StoreShip}'
                                END SHIP_TO_STORE_CLASS
                            ,   STOR.SHIP_TO_STORE_ID AS SHIP_TO_STORE_ID
                            ,   STOR.SHIP_TO_STORE_SHORT_NAME AS SHIP_TO_STORE_NAME
                            ,   SF_GET_SHIP_CASE_NO(
                                        :SHIPPER_ID
                                    ,   :CENTER_ID
                                    ,   STOR.SHIP_TO_STORE_ID
                                ) AS LABEL_CASE_NO
                            ,   TRAN.TRANSPORTER_SHORT_NAME AS TRANSPORTER_NAME
                            ,   {((condition.StoreClass == PrintCaseLabelConditions.StoreClasses.Store) ? "SF_GET_MAGUCHI_NO(STOR.SHIPPER_ID, CENTER.CENTER_ID, STOR.SHIP_TO_STORE_ID, :BRAND_ID)" : "STOR.SHIP_TO_STORE_ID")} AS FRONTAGE_NO
                            ,   BRAND.BRAND_NAME AS BRAND_NAME
                        FROM
                                V_SHIP_TO_STORES STOR
                        INNER JOIN
                                M_CENTERS CENTER
                        ON
                                CENTER.CENTER_ID = :CENTER_ID
                            AND CENTER.SHIPPER_ID = STOR.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_TRANSPORTERS TRAN
                        ON
                                TRAN.TRANSPORTER_ID = :TRANSPORTER_ID
                            AND TRAN.SHIPPER_ID = STOR.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_BRANDS BRAND
                        ON
                                BRAND.BRAND_ID = :BRAND_ID
                            AND BRAND.SHIPPER_ID = STOR.SHIPPER_ID
                        WHERE
                                STOR.SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                            AND STOR.SHIPPER_ID = :SHIPPER_ID
                            AND STOR.DELETE_FLAG = 0
                    ");

                    var data = MvcDbContext.Current.Database.Connection.Query<PrintCaseLabelResult>(query.ToString(),
                        new
                        {
                            SHIPPER_ID = Profile.User.ShipperId,
                            CENTER_ID = condition.CenterId,
                            SHIP_TO_STORE_ID = ShipToStoreIdArray[i],
                            TRANSPORTER_ID = TransporterIdArray[i],
                            BRAND_ID = condition.BrandId,
                            BRAND_NAME = condition.BrandName
                        }).FirstOrDefault();

                    if (data != null)
                    {
                        pcl.Add(new PrintCaseLabelIssueCsv()
                        {
                            Center = data.Center,
                            ShipToStoreClass = data.ShipToStoreClass,
                            ShipToStoreId = data.ShipToStoreId,
                            ShipToStoreName = data.ShipToStoreName,
                            LabelCaseNo = data.LabelCaseNo,
                            TransporterName = data.TransporterName,
                            DispIssueFlag = 0,
                            FrontageNo = data.FrontageNo,
                            BrandName = data.BrandName
                        });
                    }
                }
            }

            return pcl;
        }
        /// <summary>
        /// 店舗全件ケースラベル発行データ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintCaseLabelIssueCsv> PrintBtoBIssueAllListing(PrintCaseLabelConditions condition)
        {
            string sql, storesSql;

            string storeOutletsSql = string.Empty;
            if (condition.StoreOutletsClass == PrintCaseLabelConditions.StoreOutletsClasses.NotOutlets)
            {
                storeOutletsSql = "AND NOT EXISTS (SELECT 1 FROM M_GENERALS SUB WHERE SUB.SHIPPER_ID = M_SHIP_FRONTAGE.SHIPPER_ID AND SUB.CENTER_ID = '@@@' AND SUB.REGISTER_DIVI_CD ＝ '1' AND SUB.GEN_DIV_CD = 'EVENT_STORE_CODE' AND SUB.GEN_CD = M_SHIP_FRONTAGE.STORE_ID )";
            }
            else if (condition.StoreOutletsClass == PrintCaseLabelConditions.StoreOutletsClasses.OnlyOutlets)
            {
                storeOutletsSql = "AND EXISTS (SELECT 1 FROM M_GENERALS SUB WHERE SUB.SHIPPER_ID = M_SHIP_FRONTAGE.SHIPPER_ID AND SUB.CENTER_ID = '@@@' AND SUB.REGISTER_DIVI_CD ＝ '1' AND SUB.GEN_DIV_CD = 'EVENT_STORE_CODE' AND SUB.GEN_CD = M_SHIP_FRONTAGE.STORE_ID )";
            }

            if (condition.StoreClass == PrintCaseLabelConditions.StoreClasses.Store)
            {
                storesSql = $@"
                    SELECT
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   STORE_ID AS SHIP_TO_STORE_ID
                        ,   MAX(FRONTAGE_NO) AS FRONTAGE_NO
                    FROM
                            M_SHIP_FRONTAGE
                    WHERE
                            CENTER_ID = :CENTER_ID
                        AND SHIPPER_ID = :SHIPPER_ID
                        {((!string.IsNullOrEmpty(condition.BrandId))? "AND BRAND_ID = :BRAND_ID" : null)}
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
                        ,   SHIP_TO_STORE_ID AS FRONTAGE_NO
                    FROM
                            V_SHIP_TO_STORES
                    WHERE
                            SHIP_TO_STORE_CLASS = '8'      --倉庫
                        AND SHIPPER_ID = :SHIPPER_ID
                ";
            }

            sql = $@"
                WITH
                    SELECTED_LOC_TRANSPORTERS AS (
                        SELECT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   SHIP_TO_STORE_ID
                            ,   MAX(START_DATE) AS NEW_START_DATE
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
                    ,TARGET_LOC_TRANSPORTERS AS (
                        SELECT
                                MTRAN.SHIP_TO_STORE_ID
                            ,   MTRAN.CENTER_ID
                            ,   MTRAN.SHIPPER_ID
                            ,   MTRAN.TRANSPORTER_ID
                        FROM
                                M_LOC_TRANSPORTERS MTRAN
                        INNER JOIN
                                SELECTED_LOC_TRANSPORTERS SLD
                        ON
                                MTRAN.SHIP_TO_STORE_ID = SLD.SHIP_TO_STORE_ID
                            AND MTRAN.START_DATE = SLD.NEW_START_DATE
                            AND MTRAN.CENTER_ID = SLD.CENTER_ID
                            AND MTRAN.SHIPPER_ID = SLD.SHIPPER_ID
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
                            {((!string.IsNullOrEmpty(condition.BatchNo)) ? "AND BATCH_NO = :BATCH_NO" : null)}
                        GROUP BY
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   SHIP_TO_STORE_ID
                    )
                SELECT 
                        STOR.SHIP_TO_STORE_ID
                    ,   LTRAN.TRANSPORTER_ID
                FROM
                        TARGET_V_SHIP_TO_STORES STOR
                INNER JOIN
                        TARGET_LOC_TRANSPORTERS LTRAN
                ON
                        LTRAN.SHIP_TO_STORE_ID = STOR.SHIP_TO_STORE_ID
                    AND LTRAN.CENTER_ID = STOR.CENTER_ID
                    AND LTRAN.SHIPPER_ID = STOR.SHIPPER_ID
            ";

            if (condition.ShipInstructFlag)
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

            sql += $@"
                ORDER BY
                        STOR.FRONTAGE_NO
                    ,   STOR.SHIP_TO_STORE_ID
            ";

            var target_store = MvcDbContext.Current.Database.Connection.Query<PrintCaseAllStore>(sql, 
                new
                {
                    SHIPPER_ID = Profile.User.ShipperId,
                    CENTER_ID = condition.CenterId,
                    BRAND_ID = condition.BrandId,
                    BATCH_NO = condition.BatchNo
                }).ToList();

            List<PrintCaseLabelIssueCsv> pcl = new List<PrintCaseLabelIssueCsv>();
            foreach (var u in target_store)
            {
                for (var i = 0; i < condition.NumberofSheets; i++)
                {
                    StringBuilder query = new StringBuilder($@"
                        SELECT
                                CENTER.CENTER_ID || '　' || CENTER.CENTER_NAME1 AS CENTER
                            ,   CASE 
                                    WHEN STOR.SHIP_TO_STORE_CLASS = '8' THEN '{PrintCaseLabelResource.CenterMove}'
                                    ELSE '{PrintCaseLabelResource.StoreShip}'
                                END SHIP_TO_STORE_CLASS
                            ,   STOR.SHIP_TO_STORE_ID AS SHIP_TO_STORE_ID
                            ,   STOR.SHIP_TO_STORE_SHORT_NAME AS SHIP_TO_STORE_NAME
                            ,   SF_GET_SHIP_CASE_NO(
                                        :SHIPPER_ID
                                    ,   :CENTER_ID
                                    ,   STOR.SHIP_TO_STORE_ID
                                ) AS LABEL_CASE_NO
                            ,   TRAN.TRANSPORTER_SHORT_NAME AS TRANSPORTER_NAME
                            ,   {((condition.StoreClass == PrintCaseLabelConditions.StoreClasses.Store) ? "SF_GET_MAGUCHI_NO(STOR.SHIPPER_ID, CENTER.CENTER_ID, STOR.SHIP_TO_STORE_ID, :BRAND_ID)" : "STOR.SHIP_TO_STORE_ID")} AS FRONTAGE_NO
                            ,   BRAND.BRAND_NAME AS BRAND_NAME
                        FROM
                                V_SHIP_TO_STORES STOR
                        INNER JOIN
                                M_CENTERS CENTER
                        ON
                                CENTER.CENTER_ID = :CENTER_ID
                            AND CENTER.SHIPPER_ID = STOR.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_TRANSPORTERS TRAN
                        ON
                                TRAN.TRANSPORTER_ID = :TRANSPORTER_ID
                            AND TRAN.SHIPPER_ID = STOR.SHIPPER_ID
                        LEFT OUTER JOIN
                                M_BRANDS BRAND
                        ON
                                BRAND.BRAND_ID = :BRAND_ID
                            AND BRAND.SHIPPER_ID = STOR.SHIPPER_ID
                        WHERE
                                STOR.SHIP_TO_STORE_ID = :SHIP_TO_STORE_ID
                            AND STOR.SHIPPER_ID = :SHIPPER_ID
                            AND STOR.DELETE_FLAG = 0
                    ");

                    var data = MvcDbContext.Current.Database.Connection.Query<PrintCaseLabelResult>(query.ToString(),
                        new
                        {
                            SHIPPER_ID = Profile.User.ShipperId,
                            CENTER_ID = condition.CenterId,
                            SHIP_TO_STORE_ID = u.ShipToStoreId,
                            TRANSPORTER_ID = u.TransporterId,
                            BRAND_ID = condition.BrandId,
                            BRAND_NAME = condition.BrandName
                        }).FirstOrDefault();

                    if (data != null)
                    {
                        pcl.Add(new PrintCaseLabelIssueCsv()
                        {
                            Center = data.Center,
                            ShipToStoreClass = data.ShipToStoreClass,
                            ShipToStoreId = data.ShipToStoreId,
                            ShipToStoreName = data.ShipToStoreName,
                            LabelCaseNo = data.LabelCaseNo,
                            TransporterName = data.TransporterName,
                            DispIssueFlag = 0,
                            FrontageNo = data.FrontageNo,
                            BrandName = data.BrandName
                        });
                    }
                }
            }

            return pcl;
        }

        /// <summary>
        /// 配分Csvに出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PrintCaseLabelIssueCsv> PrintEcIssueListing(PrintCaseLabelConditions condition)
        {
            List<PrintCaseLabelIssueCsv> pcl = new List<PrintCaseLabelIssueCsv>();

            for (var j = 0; j < condition.NumberofSheets; j++)
            {
                DynamicParameters parameters = new DynamicParameters();

                StringBuilder query = new StringBuilder(@"
                    SELECT CENTER_ID || '　' || CENTER_NAME1 CENTER
                            ,SF_GET_SEQ(
                                 SHIPPER_ID
                                ,CENTER_ID
                                ,'14'
                            ) LABEL_CASE_NO
                        FROM M_CENTERS
                        WHERE SHIPPER_ID = :SHIPPER_ID
                        AND CENTER_ID = :CENTER_ID
                ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", condition.CenterId);

                var data = MvcDbContext.Current.Database.Connection.Query<PrintCaseLabelResult>(query.ToString(), parameters).ToList();

                pcl.Add(new PrintCaseLabelIssueCsv()
                {
                    Center = data[0].Center,
                    LabelCaseNo = data[0].LabelCaseNo,
                    DispIssueFlag = 0
                });
            }

            return pcl;
        }

        /// <summary>
        /// 配分Csvに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PrintCaseLabelIssueCsv> PrintCaseLabelReissueBtoBListing(PrintCaseLabelConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder();
            query.AppendLine($@"
                    WITH
                        SELECTED_PAKING_DATA AS (
                            SELECT
                                    TSPI.SHIPPER_ID
                                ,   TSPI.CENTER_ID
                                ,   TSPI.BOX_NO
                                ,   TSPI.SHIP_TO_STORE_CLASS
                                ,   TSPI.SHIP_TO_STORE_ID
                                ,   TSPI.TRANSPORTER_ID
                                ,   MAX(MIS.BRAND_ID) AS BRAND_ID
                            FROM
                                    T_SHIP_PACKING_INFO TSPI
                            LEFT OUTER JOIN 
                                    M_ITEM_SKU MIS
                            ON      
                                    MIS.SHIPPER_ID = TSPI.SHIPPER_ID
                                AND MIS.ITEM_SKU_ID = TSPI.ITEM_SKU_ID
                                AND MIS.DELETE_FLAG = 0
                            WHERE
                                    TSPI.BOX_NO = :BOX_NO
                                AND TSPI.CENTER_ID = :CENTER_ID
                                AND TSPI.SHIPPER_ID = :SHIPPER_ID
                            GROUP BY
                                    TSPI.SHIPPER_ID
                                ,   TSPI.CENTER_ID
                                ,   TSPI.BOX_NO
                                ,   TSPI.SHIP_TO_STORE_CLASS
                                ,   TSPI.SHIP_TO_STORE_ID
                                ,   TSPI.TRANSPORTER_ID
                    )
                    SELECT
                            CENTER.CENTER_ID || '　' || CENTER.CENTER_NAME1 AS CENTER
                        ,   CASE 
                                WHEN STOR.SHIP_TO_STORE_CLASS = '8' THEN '{PrintCaseLabelResource.CenterMove}'
                                ELSE '{PrintCaseLabelResource.StoreShip}'
                            END SHIP_TO_STORE_CLASS
                        ,   PACK.SHIP_TO_STORE_ID AS SHIP_TO_STORE_ID
                        ,   STOR.SHIP_TO_STORE_SHORT_NAME AS SHIP_TO_STORE_NAME
                        ,   PACK.BOX_NO AS LABEL_CASE_NO
                        ,   TRAN.TRANSPORTER_SHORT_NAME AS TRANSPORTER_NAME
                        ,   1 AS DISP_ISSUE_FLAG
                        ,   CASE
                                WHEN STOR.SHIP_TO_STORE_CLASS = '8' THEN NULL
                                WHEN CENTER.BRAND_WORK_CLASS = '1' THEN MB.BRAND_NAME
                                ELSE NULL
                            END AS BRAND_NAME
                        ,   CASE
                                WHEN STOR.SHIP_TO_STORE_CLASS = '8' THEN PACK.SHIP_TO_STORE_ID
                                WHEN CENTER.BRAND_WORK_CLASS = '1' THEN TO_NCHAR(SF_GET_MAGUCHI_NO(PACK.SHIPPER_ID, PACK.CENTER_ID, PACK.SHIP_TO_STORE_ID, PACK.BRAND_ID))
                                ELSE TO_NCHAR(SF_GET_MAGUCHI_NO(PACK.SHIPPER_ID, PACK.CENTER_ID, PACK.SHIP_TO_STORE_ID, NULL))
                            END AS FRONTAGE_NO
                    FROM
                            SELECTED_PAKING_DATA PACK
                    LEFT OUTER JOIN
                            M_CENTERS CENTER
                    ON
                            CENTER.CENTER_ID = PACK.CENTER_ID
                        AND CENTER.SHIPPER_ID = PACK.SHIPPER_ID
                    LEFT OUTER JOIN 
                            M_BRANDS MB
                    ON      
                            MB.SHIPPER_ID = PACK.SHIPPER_ID
                        AND MB.BRAND_ID = PACK.BRAND_ID
                        AND MB.DELETE_FLAG = 0
                    LEFT OUTER JOIN
                            V_SHIP_TO_STORES STOR
                    ON
                            STOR.SHIPPER_ID = PACK.SHIPPER_ID
                        AND STOR.SHIP_TO_STORE_ID = PACK.SHIP_TO_STORE_ID
                    LEFT OUTER JOIN
                            M_TRANSPORTERS TRAN
                    ON
                            TRAN.TRANSPORTER_ID = PACK.TRANSPORTER_ID
                        AND TRAN.SHIPPER_ID = PACK.SHIPPER_ID
                ");

            parameters.AddDynamicParams(new { BOX_NO = condition.ReleaseBoxNo });
            parameters.AddDynamicParams(new { CENTER_ID = condition.CenterId });
            parameters.AddDynamicParams(new { SHIPPER_ID = Profile.User.ShipperId });

            return MvcDbContext.Current.Database.Connection.Query<PrintCaseLabelIssueCsv>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Csvに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PrintCaseLabelIssueCsv> PrintCaseLabelReissueEcListing(PrintCaseLabelConditions condition)
        {
            List<PrintCaseLabelIssueCsv> pc = new List<PrintCaseLabelIssueCsv>();
            pc.Add(new PrintCaseLabelIssueCsv() { LabelCaseNo = condition.ReleaseBoxNo, DispIssueFlag = 1 });

            return pc;
        }

    }
}