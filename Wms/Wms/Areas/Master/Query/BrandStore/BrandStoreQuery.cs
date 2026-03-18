namespace Wms.Areas.Master.Query.BrandStore
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.BrandStore;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.BrandStore.BrandStoreSearchCondition;

    public class BrandStoreQuery
    {
        /// <summary>
        /// Get Country List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Detail> GetData(BrandStoreSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                 SELECT
                        MBS.BRAND_ID
                   ,    MB.BRAND_NAME
                   ,    MBS.STORE_ID
                   ,    STORE.STORE_NAME1 AS STORE_NAME
                 FROM
                        M_BRAND_STORES MBS
                 LEFT JOIN M_BRANDS MB
                   ON   MBS.SHIPPER_ID = MB.SHIPPER_ID
                  AND   MBS.BRAND_ID = MB.BRAND_ID
                 LEFT JOIN M_STORES STORE
                   ON   MBS.SHIPPER_ID = STORE.SHIPPER_ID
                  AND   MBS.STORE_ID = STORE.STORE_ID
                 WHERE
                        MBS.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            if(!string.IsNullOrEmpty(condition.StoreId))
            {
                query.Append(" AND MBS.STORE_ID LIKE :STORE_ID");
                parameters.Add(":STORE_ID", condition.StoreId + "%");
            }

            if(!string.IsNullOrEmpty(condition.StoreName))
            {
                query.Append(" AND STORE.STORE_NAME1 LIKE:STORE_NAME");
                parameters.Add(":STORE_NAME", condition.StoreName + "%");
            }

            if (!string.IsNullOrEmpty(condition.BrandId))
            {
                query.Append(" AND MBS.BRAND_ID = :BRAND_ID ");
                parameters.Add(":BRAND_ID", condition.BrandId);
            }

            if (!string.IsNullOrEmpty(condition.BrandName))
            {
                query.Append(@" AND MB.BRAND_NAME LIKE :BRAND_NAME");
                parameters.Add(":BRAND_NAME", condition.BrandName + "%");
            }

            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND MBS.DELETE_FLAG <> 1");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case BrandStoreSortKey.SortBrandIdStoreId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MBS.BRAND_ID DESC,MBS.STORE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MBS.BRAND_ID ASC,MBS.STORE_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MBS.STORE_ID DESC,MBS.BRAND_ID DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MBS.STORE_ID ASC,MBS.BRAND_ID ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Brands = MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Detail>(Brands, condition.Page, condition.PageSize, totalCount);
        }
    }
}