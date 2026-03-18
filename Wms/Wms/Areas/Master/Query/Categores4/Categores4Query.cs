namespace Wms.Areas.Master.Query.Categores4
{

    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.Categores4;
    using Wms.Common;
    using Wms.Models;
    using Wms.Controllers;
    using static Wms.Areas.Master.ViewModels.Categores4.Categores4SearchCondition;
    using Share.Extensions.Classes;

    public class Categores4Query
    {
        /// <summary>
        /// 分類1データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys1()
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId1.ToString(),
                    Text = m.CategoryId1.ToString() + "：" + m.CategoryName1
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類2データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys2(string categoryId1 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2.ToString(),
                    Text = m.CategoryId2.ToString() + "：" + m.CategoryName2
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類3データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys3(string categoryId1 = "", string categoryId2 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId3.ToString(),
                    Text = m.CategoryId3.ToString() + "：" + m.CategoryName3
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類4データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys4(string categoryId1 = "", string categoryId2 = "", string categoryId3 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2)
                && (categoryId3 == null ? 1 == 1 : m.CategoryId3 == categoryId3))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId4.ToString(),
                    Text = m.CategoryId4.ToString() + "：" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// Get Country List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Categores4ViewModel> GetData(Categores4SearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                 SELECT 
                        MIC.CATEGORY_ID1
                       ,MIC.CATEGORY_NAME1
                       ,MIC.CATEGORY_CLASS1
                       ,MIC.CATEGORY_ID2
                       ,MIC.CATEGORY_NAME2
                       ,MIC.CATEGORY_CLASS2
                       ,MIC.CATEGORY_ID3
                       ,MIC.CATEGORY_NAME3
                       ,MIC.CATEGORY_CLASS3
                       ,MIC.CATEGORY_ID4
                       ,MIC.CATEGORY_NAME4
                       ,MIC.CATEGORY_CLASS4
                       ,MIC.DELETE_FLAG
                   FROM M_ITEM_CATEGORIES4 MIC
                  WHERE MIC.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // 分類
            if (!string.IsNullOrEmpty(condition.CategoryId1))
            {
                query.Append(" AND MIC.CATEGORY_ID1 = :CATEGORY_ID1 ");
                parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId2))
            {
                query.Append(" AND MIC.CATEGORY_ID2 = :CATEGORY_ID2 ");
                parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId3))
            {
                query.Append(" AND MIC.CATEGORY_ID3 = :CATEGORY_ID3 ");
                parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId4))
            {
                query.Append(" AND MIC.CATEGORY_ID4 = :CATEGORY_ID4 ");
                parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
            }

            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND MIC.DELETE_FLAG <> 1");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Categores4ViewModel>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case Categores4SortKey.CategoryName1:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MIC.CATEGORY_NAME1 DESC,
                                                        MIC.CATEGORY_NAME2 DESC,
                                                        MIC.CATEGORY_NAME3 DESC,
                                                        MIC.CATEGORY_NAME4 DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MIC.CATEGORY_NAME1 ASC,
                                                        MIC.CATEGORY_NAME2 ASC,
                                                        MIC.CATEGORY_NAME3 ASC,
                                                        MIC.CATEGORY_NAME4 ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" 
                                               ORDER BY MIC.CATEGORY_ID1 DESC,
                                                        MIC.CATEGORY_ID2 DESC,
                                                        MIC.CATEGORY_ID3 DESC,
                                                        MIC.CATEGORY_ID4 DESC");
                            break;

                        default:
                            query.AppendLine(@" 
                                               ORDER BY MIC.CATEGORY_ID1 ASC,
                                                        MIC.CATEGORY_ID2 ASC,
                                                        MIC.CATEGORY_ID3 ASC,
                                                        MIC.CATEGORY_ID4 ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Categores4s = MvcDbContext.Current.Database.Connection.Query<Categores4ViewModel>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Categores4ViewModel>(Categores4s, condition.Page, condition.PageSize, totalCount);
        }
    }
}