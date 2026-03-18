namespace Wms.Areas.Master.Query.Brand
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.Brand;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.Brand.BrandSearchCondition;

    public class BrandQuery
    {
        /// <summary>
        /// Get Country List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Models.Brand> GetData(BrandSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                 SELECT MB.BRAND_ID
                       ,MB.BRAND_NAME
                       ,MB.BRAND_SHORT_NAME
                       ,MB.DELETE_FLAG
                  FROM M_BRANDS MB
                 WHERE MB.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            if (!string.IsNullOrEmpty(condition.BrandId))
            {
                query.Append(" AND MB.BRAND_ID LIKE :BRAND_ID ");
                parameters.Add(":BRAND_ID", condition.BrandId + "%");
            }

            if (!string.IsNullOrEmpty(condition.BrandName))
            {
                query.Append(@" AND MB.BRAND_NAME LIKE :BRAND_NAME");
                parameters.Add(":BRAND_NAME", "%" + condition.BrandName + "%");
            }

            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND MB.DELETE_FLAG <> 1");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Models.Brand>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case BrandSortKey.BrandName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MB.BRAND_NAME DESC,MB.BRAND_ID DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MB.BRAND_NAME ASC,MB.BRAND_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MB.BRAND_ID DESC,MB.BRAND_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MB.BRAND_ID ASC,MB.BRAND_NAME ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Brands = MvcDbContext.Current.Database.Connection.Query<Models.Brand>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Models.Brand>(Brands, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Get Color By Id
        /// </summary>
        /// <param name="BrandId">storeId</param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public Detail GetTargetById(string BrandId, string shipperId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                 SELECT
                       MB.UPDATE_COUNT
                      ,MB.SHIPPER_ID
                      ,MB.BRAND_ID
                      ,MB.BRAND_NAME
                      ,MB.BRAND_SHORT_NAME
                      ,MB.DIVISION_ID
                      ,DIVISION.DIVISION_NAME AS DIVISION_NAME
                      ,MB.DELETE_FLAG
                  FROM M_BRANDS MB
                  LEFT JOIN M_DIVISIONS DIVISION
                    ON MB.SHIPPER_ID = DIVISION.SHIPPER_ID
                   AND MB.DIVISION_ID = DIVISION.DIVISION_ID
                 WHERE MB.SHIPPER_ID = :SHIPPER_ID
                   AND MB.BRAND_ID = :BRAND_ID
            ");
            parameters.Add(":SHIPPER_ID", shipperId);
            parameters.Add(":BRAND_ID", BrandId);
            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }

    }
}