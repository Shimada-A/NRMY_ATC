namespace Wms.Areas.Master.Query.Store
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.Store;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;
    using static Wms.Areas.Master.ViewModels.Store.StoreSearchCondition;

    public class Report : BaseQuery
    {
        /// <summary>
        /// エクセルに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<ViewModels.Store.Report> Listing(StoreSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                WITH
                    GENERAL_DATA AS (
                        SELECT
                                GEN_DIV_CD
                            ,   GEN_CD
                            ,   GEN_NAME
                        FROM
                                M_GENERALS
                        WHERE
                                REGISTER_DIVI_CD = '1'
                            AND GEN_DIV_CD IN ('SALE_BASE_CLASS', 'ROUND_CLASS', 'TEMP_STORE_CLASS', 'CLOSE_CLASS')
                            AND CENTER_ID = '@@@'
                            AND SHIPPER_ID = :SHIPPER_ID
                    )
                SELECT
                        TO_CHAR(STORES.MAKE_DATE,'YYYY/MM/DD') AS MAKE_DATE
                    ,   TO_CHAR(STORES.UPDATE_DATE,'YYYY/MM/DD') AS UPDATE_DATE
                    ,   STORES.STORE_ID
                    ,   STORES.STORE_CLASS
                    ,   STORES.STORE_NAME1
                    ,   STORES.STORE_SHORT_NAME
                    ,   STORES.STORE_ZIP
                    ,   STORES.STORE_PREF_NAME
                    ,   STORES.STORE_CITY_NAME
                    ,   STORES.STORE_ADDRESS1
                    ,   STORES.STORE_ADDRESS2
                    ,   STORES.STORE_ADDRESS3
                    ,   STORES.STORE_TEL
                    ,   STORES.STORE_FAX
                    ,   STORES.STORE_MAIL1
                    ,   STORES.EC_CLASS
                    ,   STORES.STORE_RANK_ID
                    ,   TO_CHAR(STORES.OPEN_DATE,'YYYY/MM/DD') AS OPEN_DATE
                    ,   TO_CHAR(STORES.CLOSE_DATE,'YYYY/MM/DD') AS CLOSE_DATE
                    ,   STORES.AREA_ID
                    ,   STORES.PREF_ID
                    ,   STORES.STOCK_OUT_DISABLE_FLAG
                    ,   STORES.INSPECTION_MUST_FLAG
                    ,   STORES.DIVISION_ID
                    ,   STORES.INVOICE_STORE_NAME
                    ,   STORES.PATTERN_ID
                    ,   CASE 
                             WHEN STORES.SALE_BASE_CLASS IS NOT NULL THEN STORES.SALE_BASE_CLASS||'：'||MG_SBC.GEN_NAME 
                             ELSE NULL 
                        END AS SALE_BASE_CLASS
                    ,   CASE 
                             WHEN STORES.ROUND_CLASS IS NOT NULL THEN STORES.ROUND_CLASS||'：'||MG_RC.GEN_NAME 
                             ELSE NULL 
                        END AS ROUND_CLASS
                    ,   CASE 
                             WHEN STORES.CONTROL_CENTER_ID IS NOT NULL THEN STORES.CONTROL_CENTER_ID||'：'||MC.CENTER_NAME1 
                             ELSE NULL 
                        END AS CONTROL_CENTER_ID
                    ,   CASE 
                             WHEN STORES.TEMP_STORE_CLASS IS NOT NULL THEN STORES.TEMP_STORE_CLASS||'：'||MG_TSC.GEN_NAME 
                             ELSE NULL 
                        END AS TEMP_STORE_CLASS
                    ,   CASE 
                             WHEN STORES.CLOSE_CLASS IS NOT NULL THEN STORES.CLOSE_CLASS||'：'||MG_CC.GEN_NAME 
                             ELSE NULL
                        END AS CLOSE_CLASS
                    ,   DECODE(STORES.DELETE_FLAG,0,NULL,1,1) AS DELETE_FLAG
                FROM
                        M_STORES STORES
                LEFT JOIN 
                        GENERAL_DATA MG_SBC
                  ON    
                        MG_SBC.GEN_DIV_CD = 'SALE_BASE_CLASS'
                 AND    STORES.SALE_BASE_CLASS = MG_SBC.GEN_CD
                LEFT JOIN 
                        GENERAL_DATA MG_RC
                  ON    
                        MG_RC.GEN_DIV_CD = 'ROUND_CLASS'
                 AND    STORES.ROUND_CLASS = MG_RC.GEN_CD
                LEFT JOIN 
                        M_CENTERS MC
                  ON    
                        STORES.SHIPPER_ID = MC.SHIPPER_ID
                 AND    STORES.CONTROL_CENTER_ID = MC.CENTER_ID
                LEFT JOIN 
                        GENERAL_DATA MG_TSC
                  ON    
                        MG_TSC.GEN_DIV_CD = 'TEMP_STORE_CLASS'
                 AND    STORES.TEMP_STORE_CLASS = MG_TSC.GEN_CD
                LEFT JOIN 
                        GENERAL_DATA MG_CC
                  ON    
                        MG_CC.GEN_DIV_CD = 'CLOSE_CLASS'
                 AND    STORES.CLOSE_CLASS = MG_CC.GEN_CD
                WHERE
                        STORES.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.StoreClass))
            {
                query.Append(" AND STORES.STORE_CLASS = :STORE_CLASS ");
                parameters.Add(":STORE_CLASS", condition.StoreClass);
            }

            if (!string.IsNullOrEmpty(condition.StoreId))
            {
                query.Append(" AND STORES.STORE_ID LIKE :STORE_ID ");
                parameters.Add(":STORE_ID", condition.StoreId + "%");
            }

            if (!string.IsNullOrEmpty(condition.StoreName))
            {
                query.Append(@" AND (STORES.STORE_NAME1 LIKE :STORE_NAME
                                 OR  STORES.STORE_NAME2 LIKE :STORE_NAME)");
                parameters.Add(":STORE_NAME", condition.StoreName + "%");
            }

            if (!string.IsNullOrEmpty(condition.StoreAddress))
            {
                query.Append(@" AND (STORES.STORE_PREF_NAME
                                 ||  STORES.STORE_CITY_NAME
                                 ||  STORES.STORE_ADDRESS1
                                 ||  STORES.STORE_ADDRESS2
                                 ||  STORES.STORE_ADDRESS3 LIKE :STORE_ADDRESS)");
                parameters.Add(":STORE_ADDRESS", "%" + condition.StoreAddress + "%");
            }

            if (!string.IsNullOrEmpty(condition.StoreTel))
            {
                query.Append(" AND REPLACE(STORES.STORE_TEL,'-') LIKE :STORE_TEL ");
                parameters.Add(":STORE_TEL", condition.StoreTel + "%");
            }

            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND STORES.DELETE_FLAG <> 1");
            }

            // 検品必須フラグ
            if (condition.InspectionMustFlag)
            {
                query.Append(" AND STORES.INSPECTION_MUST_FLAG = 1");
            }

            // データ新規登録日
            // 返品登録日(From-To)
            if (condition.MakeDateFrom != null && condition.MakeDateFrom != CommonResource.None)
            {
                query.Append(" AND TO_CHAR(STORES.MAKE_DATE,'YYYY/MM/DD') >= :MAKE_DATE_FROM ");
                parameters.Add(":MAKE_DATE_FROM", condition.MakeDateFrom);
            }

            if (condition.MakeDateTo != null && condition.MakeDateTo != CommonResource.None)
            {
                query.Append(" AND TO_CHAR(STORES.MAKE_DATE,'YYYY/MM/DD') <= :MAKE_DATE_TO ");
                parameters.Add(":MAKE_DATE_TO", condition.MakeDateTo);
            }

            // Sort function
            switch (condition.SortKey)
            {
                case StoreSortKey.StoreName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY STORES.STORE_NAME1 DESC,STORES.STORE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY STORES.STORE_NAME1 ASC,STORES.STORE_ID ASC");
                            break;
                    }

                    break;

                case StoreSortKey.OpenDateStoreId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY STORES.OPEN_DATE DESC,STORES.STORE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY STORES.OPEN_DATE ASC,STORES.STORE_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY STORES.STORE_ID DESC,STORES.STORE_NAME1 DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY STORES.STORE_ID ASC,STORES.STORE_NAME1 ASC");
                            break;
                    }

                    break;
            }

            return MvcDbContext.Current.Database.Connection.Query<ViewModels.Store.Report>(query.ToString(), parameters).ToList();
        }
    }
}