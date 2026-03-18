namespace Wms.Areas.Master.Models
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.Store;
    using Wms.Common;
    using Wms.Models;
    using Wms.Resources;
    using static Wms.Areas.Master.ViewModels.Store.StoreSearchCondition;

    public partial class Store
    {
        /// <summary>
        /// Get Country List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Store> GetData(StoreSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MS.UPDATE_COUNT
                      ,MS.SHIPPER_ID
                      ,MG.GEN_NAME STORE_CLASS
                      ,MS.STORE_ID
                      ,MS.STORE_NAME1
                      ,MS.STORE_NAME2
                      ,TO_CHAR(MS.OPEN_DATE,'YYYY/MM/DD') AS OPEN_DATE
                      ,TO_CHAR(MS.CLOSE_DATE,'YYYY/MM/DD') AS CLOSE_DATE
                      ,MS.DELETE_FLAG
                      ,MS.STOCK_OUT_DISABLE_FLAG
                      ,MS.INSPECTION_MUST_FLAG
                  FROM M_STORES MS
                  LEFT JOIN M_GENERALS MG
                    ON MS.SHIPPER_ID = MG.SHIPPER_ID
                   AND MG.REGISTER_DIVI_CD = '1'
                   AND MG.CENTER_ID = '@@@'
                   AND MG.GEN_DIV_CD = 'STORE_CLASS'
                   AND MS.STORE_CLASS = MG.GEN_CD
                 WHERE MS.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.StoreClass))
            {
                query.Append(" AND MS.STORE_CLASS = :STORE_CLASS ");
                parameters.Add(":STORE_CLASS", condition.StoreClass);
            }

            if (!string.IsNullOrEmpty(condition.StoreId))
            {
                query.Append(" AND MS.STORE_ID LIKE :STORE_ID ");
                parameters.Add(":STORE_ID", condition.StoreId + "%");
            }

            if (!string.IsNullOrEmpty(condition.StoreName))
            {
                query.Append(@" AND (MS.STORE_NAME1 LIKE :STORE_NAME
                                 OR  MS.STORE_NAME2 LIKE :STORE_NAME)");
                parameters.Add(":STORE_NAME", condition.StoreName + "%");
            }

            if (!string.IsNullOrEmpty(condition.StoreAddress))
            {
                query.Append(@" AND (MS.STORE_PREF_NAME
                                 ||  MS.STORE_CITY_NAME
                                 ||  MS.STORE_ADDRESS1
                                 ||  MS.STORE_ADDRESS2
                                 ||  MS.STORE_ADDRESS3 LIKE :STORE_ADDRESS)");
                parameters.Add(":STORE_ADDRESS", "%" + condition.StoreAddress + "%");
            }

            if (!string.IsNullOrEmpty(condition.StoreTel))
            {
                query.Append(" AND REPLACE(MS.STORE_TEL,'-') LIKE :STORE_TEL ");
                parameters.Add(":STORE_TEL", condition.StoreTel + "%");
            }
            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND MS.DELETE_FLAG <> 1");
            }

            // 検品必須フラグ
            if (condition.InspectionMustFlag)
            {
                query.Append(" AND MS.INSPECTION_MUST_FLAG = 1");
            }

            // データ新規登録日
            // 返品登録日(From-To)
            if (condition.MakeDateFrom != null && condition.MakeDateFrom != CommonResource.None)
            {
                query.Append(" AND TO_CHAR(MS.MAKE_DATE,'YYYY/MM/DD') >= :MAKE_DATE_FROM ");
                parameters.Add(":MAKE_DATE_FROM", condition.MakeDateFrom);
            }

            if (condition.MakeDateTo != null && condition.MakeDateTo != CommonResource.None)
            {
                query.Append(" AND TO_CHAR(MS.MAKE_DATE,'YYYY/MM/DD') <= :MAKE_DATE_TO ");
                parameters.Add(":MAKE_DATE_TO", condition.MakeDateTo);
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Store>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case StoreSortKey.StoreName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MS.STORE_NAME1 DESC,MS.STORE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MS.STORE_NAME1 ASC,MS.STORE_ID ASC");
                            break;
                    }

                    break;

                case StoreSortKey.OpenDateStoreId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MS.OPEN_DATE DESC,MS.STORE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MS.OPEN_DATE ASC,MS.STORE_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MS.STORE_ID DESC,MS.STORE_NAME1 DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MS.STORE_ID ASC,MS.STORE_NAME1 ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Stores = MvcDbContext.Current.Database.Connection.Query<Store>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Store>(Stores, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Get Store By Id
        /// </summary>
        /// <param name="storeClass">storeClass</param>
        /// <param name="storeId">storeId</param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public Detail GetTargetById(string storeId, string shipperId)
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
                            AND GEN_DIV_CD IN ('STORE_CLASS', 'EC_CLASS', 'STORE_RANK', 'SALE_BASE_CLASS', 'ROUND_CLASS', 'TEMP_STORE_CLASS', 'CLOSE_CLASS')
                            AND CENTER_ID = '@@@'
                            AND SHIPPER_ID = :SHIPPER_ID
                    )
                SELECT
                          MS.UPDATE_COUNT
                      ,   MS.SHIPPER_ID
                      ,   MG_SC.GEN_NAME STORE_CLASS
                      ,   MS.STORE_ID
                      ,   MS.STORE_NAME1
                      ,   MS.STORE_NAME2
                      ,   MS.STORE_SHORT_NAME
                      ,   MS.STORE_ZIP
                      ,   MS.STORE_PREF_NAME
                      ,   MS.STORE_CITY_NAME
                      ,   MS.STORE_ADDRESS1
                      ,   MS.STORE_ADDRESS2
                      ,   MS.STORE_ADDRESS3
                      ,   MS.STORE_TEL
                      ,   MS.STORE_FAX
                      ,   MS.STORE_MAIL1
                      ,   MG_ECC.GEN_NAME EC_CLASS
                      ,   MG_SR.GEN_NAME STORE_RANK_ID
                      ,   TO_CHAR(MS.OPEN_DATE,'YYYY/MM/DD') AS OPEN_DATE
                      ,   TO_CHAR(MS.CLOSE_DATE,'YYYY/MM/DD') AS CLOSE_DATE
                      ,   MS.AREA_ID
                      ,   MS.PREF_ID
                      ,   MS.STOCK_OUT_DISABLE_FLAG
                      ,   MS.INSPECTION_MUST_FLAG
                      ,   MS.TRANSPORTER_ID
                      ,   MS.DIVISION_ID
                      ,   DIVISION.DIVISION_NAME AS DIVISION_NAME
                      ,   MS.DELETE_FLAG
                      ,   MS.INVOICE_STORE_NAME
                      ,   MS.PATTERN_ID
                      ,   CASE 
                                WHEN MS.SALE_BASE_CLASS IS NOT NULL THEN MS.SALE_BASE_CLASS||'：'||MG_SBC.GEN_NAME 
                                ELSE NULL 
                          END AS SALE_BASE_CLASS
                      ,   CASE 
                                WHEN MS.ROUND_CLASS IS NOT NULL THEN MS.ROUND_CLASS||'：'||MG_RC.GEN_NAME 
                                ELSE NULL 
                          END AS ROUND_CLASS
                      ,   CASE 
                                WHEN MS.CONTROL_CENTER_ID IS NOT NULL THEN MS.CONTROL_CENTER_ID||'：'||MC.CENTER_NAME1 
                                ELSE NULL 
                          END AS CONTROL_CENTER_ID
                      ,   CASE 
                                WHEN MS.TEMP_STORE_CLASS IS NOT NULL THEN MS.TEMP_STORE_CLASS||'：'||MG_TSC.GEN_NAME 
                                ELSE NULL 
                          END AS TEMP_STORE_CLASS
                      ,   CASE 
                                WHEN MS.CLOSE_CLASS IS NOT NULL THEN MS.CLOSE_CLASS||'：'||MG_CC.GEN_NAME 
                                ELSE NULL
                          END AS CLOSE_CLASS
                  FROM
                          M_STORES MS
                  LEFT JOIN 
                          GENERAL_DATA MG_SC
                    ON    
                          MG_SC.GEN_DIV_CD = 'STORE_CLASS'
                   AND    MS.STORE_CLASS = MG_SC.GEN_CD
                  LEFT JOIN 
                          GENERAL_DATA MG_ECC
                    ON    
                          MG_ECC.GEN_DIV_CD = 'EC_CLASS'
                   AND    MS.EC_CLASS = MG_ECC.GEN_CD
                  LEFT JOIN 
                          GENERAL_DATA MG_SR
                    ON    
                          MG_SR.GEN_DIV_CD = 'STORE_RANK'
                   AND    MS.STORE_RANK_ID = MG_SR.GEN_CD
                  LEFT JOIN 
                          M_DIVISIONS DIVISION
                    ON    
                          MS.SHIPPER_ID = DIVISION.SHIPPER_ID
                   AND    MS.DIVISION_ID = DIVISION.DIVISION_ID
                  LEFT JOIN 
                          GENERAL_DATA MG_SBC
                    ON    
                          MG_SBC.GEN_DIV_CD = 'SALE_BASE_CLASS'
                   AND    MS.SALE_BASE_CLASS = MG_SBC.GEN_CD
                  LEFT JOIN 
                          GENERAL_DATA MG_RC
                    ON    
                          MG_RC.GEN_DIV_CD = 'ROUND_CLASS'
                   AND    MS.ROUND_CLASS = MG_RC.GEN_CD
                  LEFT JOIN 
                          M_CENTERS MC
                    ON    
                          MS.SHIPPER_ID = MC.SHIPPER_ID
                   AND    MS.CONTROL_CENTER_ID = MC.CENTER_ID
                  LEFT JOIN 
                          GENERAL_DATA MG_TSC
                    ON    
                          MG_TSC.GEN_DIV_CD = 'TEMP_STORE_CLASS'
                   AND    MS.TEMP_STORE_CLASS = MG_TSC.GEN_CD
                  LEFT JOIN 
                          GENERAL_DATA MG_CC
                    ON    
                          MG_CC.GEN_DIV_CD = 'CLOSE_CLASS'
                   AND    MS.CLOSE_CLASS = MG_CC.GEN_CD
                 WHERE
                          MS.SHIPPER_ID = :SHIPPER_ID
                   AND    MS.STORE_ID = :STORE_ID
            ");
            parameters.Add(":SHIPPER_ID", shipperId);
            parameters.Add(":STORE_ID", storeId);
            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// 都道府県入力チェック
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public bool SelectPrefs(Detail store)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        MP.PREF_ID
                    ,   MP.PREF_NAME
                FROM
                        M_PREFS MP
                WHERE
                        MP.SHIPPER_ID = :SHIPPER_ID
                    AND MP.PREF_NAME = :PREF_NAME
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":PREF_NAME", store.StorePrefName);

            // Fill data to memory
            var pref = MvcDbContext.Current.Database.Connection.Query<Pref>(query.ToString(), parameters).ToList();

            // Excute paging
            if (pref.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Update Store
        /// </summary>
        /// <param name="store"></param>
        /// <returns>Update status</returns>
        public bool UpdateStore(Detail store)
        {
            var dbContext = MvcDbContext.Current;

            var updatedStore =
                  MvcDbContext.Current.Stores
                  .Where(m => m.ShipperId == store.ShipperId && m.StoreId == store.StoreId && m.UpdateCount == store.UpdateCount)
                  .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
            if (updatedStore == null)
            {
                return false;
            }

            var prefId = MvcDbContext.Current.Prefs
                  .Where(m => m.ShipperId == store.ShipperId && m.PrefName == store.StorePrefName).Select(m => m.PrefId)
                  .FirstOrDefault();


            updatedStore.SetBaseInfoUpdate();

            updatedStore.StoreZip = store.StoreZip;
            updatedStore.StorePrefName = store.StorePrefName;
            updatedStore.StoreCityName = store.StoreCityName;
            updatedStore.StoreAddress1 = store.StoreAddress1;
            updatedStore.StoreAddress2 = store.StoreAddress2;
            updatedStore.StoreAddress3 = store.StoreAddress3;
            updatedStore.StoreTel = store.StoreTel;
            updatedStore.StoreFax = store.StoreFax;
            updatedStore.PrefId = prefId;
            updatedStore.StockOutDisableFlag = store.StockOutDisableFlag;
            updatedStore.InspectionMustFlag = store.InspectionMustFlag;
            updatedStore.InvoiceStoreName = store.InvoiceStoreName;
            updatedStore.PatternId = store.PatternId;

            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }

                trans.Commit();
            }

            return true;
        }
        /// <summary>
        /// 仕分けパターン情報取得
        /// </summary>
        /// <returns>ドロップダウンリスト(Value:PATTERN_ID,Text:PATTERN_NAME)</returns>

        public IEnumerable<SelectListItem> GetSelectListPatternId()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT DISTINCT
                        MSP.PATTERN_ID AS VALUE
                    ,   MSP.PATTERN_NAME AS TEXT
                FROM
                        M_STOCKING_PATTERN MSP
                WHERE
                        MSP.SHIPPER_ID = :SHIPPER_ID
                ORDER BY
                        MSP.PATTERN_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            var stockingPattern = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters)/*.ToList()*/;

            // Excute paging
            return stockingPattern;
        }

    }
}