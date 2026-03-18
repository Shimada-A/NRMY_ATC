namespace Wms.Areas.Master.Query.Size
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.Size;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.Size.SizeSearchCondition;

    public class SizeQuery
    {
        public IPagedList<Models.Size> GetData(SizeSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                 SELECT MS.ITEM_SIZE_ID
                       ,MS.ITEM_SIZE_NAME
                       ,MS.DELETE_FLAG
                  FROM M_SIZES MS
                 WHERE MS.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            if (!string.IsNullOrEmpty(condition.ItemSizeId))
            {
                query.Append(" AND MS.ITEM_SIZE_ID LIKE :ITEM_SIZE_ID ");
                parameters.Add(":ITEM_SIZE_ID", condition.ItemSizeId + "%");
            }

            if (!string.IsNullOrEmpty(condition.ItemSizeName))
            {
                query.Append(@" AND MS.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME");
                parameters.Add(":ITEM_SIZE_NAME", condition.ItemSizeName + "%");
            }

            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND MS.DELETE_FLAG <> 1");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Models.Size>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case SizeSortKey.ItemSizeId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MS.ITEM_SIZE_ID DESC,MS.ITEM_SIZE_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MS.ITEM_SIZE_ID ASC,MS.ITEM_SIZE_NAME ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MS.ITEM_SIZE_NAME DESC,MS.ITEM_SIZE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MS.ITEM_SIZE_NAME ASC,MS.ITEM_SIZE_ID ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Sizes = MvcDbContext.Current.Database.Connection.Query<Models.Size>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Models.Size>(Sizes, condition.Page, condition.PageSize, totalCount);
        }
    }
}