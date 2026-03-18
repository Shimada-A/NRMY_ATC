namespace Wms.Areas.Master.Query.Color
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.Color;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.Color.ColorSearchCondition;

    public partial class ColorQuery
    {
        /// <summary>
        /// Get Country List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Models.Color> GetData(ColorSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                 SELECT MC.ITEM_COLOR_ID
                       ,MC.ITEM_COLOR_NAME
                       ,MC.DELETE_FLAG
                  FROM M_COLORS MC
                 WHERE MC.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            if (!string.IsNullOrEmpty(condition.ItemColorId))
            {
                query.Append(" AND MC.ITEM_COLOR_ID LIKE :ITEM_COLOR_ID ");
                parameters.Add(":ITEM_COLOR_ID", condition.ItemColorId + "%");
            }

            if (!string.IsNullOrEmpty(condition.ItemColorName))
            {
                query.Append(@" AND MC.ITEM_COLOR_NAME LIKE :ITEM_COLOR_NAME");
                parameters.Add(":ITEM_COLOR_NAME", "%" + condition.ItemColorName + "%");
            }

            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND MC.DELETE_FLAG <> 1");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Models.Color>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case ColorSortKey.ItemColorName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MC.ITEM_COLOR_NAME DESC,MC.ITEM_COLOR_ID DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MC.ITEM_COLOR_NAME ASC,MC.ITEM_COLOR_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MC.ITEM_COLOR_ID DESC,MC.ITEM_COLOR_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MC.ITEM_COLOR_ID ASC,MC.ITEM_COLOR_NAME ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Colors = MvcDbContext.Current.Database.Connection.Query<Models.Color>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Models.Color>(Colors, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Get Color By Id
        /// </summary>
        /// <param name="itemColorId">storeId</param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public Detail GetTargetById(string itemColorId, string shipperId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MC.UPDATE_COUNT
                      ,MC.SHIPPER_ID
                      ,MC.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,MC.ITEM_COLOR_CODE
                      ,MC.ITEM_COLOR_SHORT_NAME
                      ,MC.DELETE_FLAG
                  FROM M_COLORS MC
                 WHERE MC.SHIPPER_ID = :SHIPPER_ID
                   AND MC.ITEM_COLOR_ID = :ITEM_COLOR_ID
            ");
            parameters.Add(":SHIPPER_ID", shipperId);
            parameters.Add(":ITEM_COLOR_ID", itemColorId);
            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }

    }






}