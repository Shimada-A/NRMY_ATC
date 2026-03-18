namespace Wms.Areas.Master.Models
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.ItemSku;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.ItemSku.ItemSkuSearchCondition;

    public partial class ItemSku
    {
        /// <summary>
        /// Get Country List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Detail> GetData(ItemSkuSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MIS.UPDATE_COUNT
                      ,MIS.SHIPPER_ID
                      ,MIS.CATEGORY_ID1
                      ,MIC4.CATEGORY_NAME1
                      ,MIS.ITEM_SKU_ID
                      ,MIS.ITEM_ID
                      ,MIS.ITEM_NAME
                      ,MIS.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,MIS.ITEM_SIZE_ID
                      ,MIS.ITEM_SIZE_NAME
                      ,MIS.JAN
                      ,MIS.SALE_START_DATE
                      ,MIS.SALE_END_DATE
                      ,MIS.WITHDRAWAL_DATE
                      ,MIS.NORMAL_SELLING_PRICE
                      ,MIS.NORMAL_SELLING_PRICE_EX_TAX
                      ,MIS.NORMAL_BUYING_PRICE
                      ,MIS.PURCHASE_BUYING_PRICE
                      ,MIS.DELETE_FLAG
                  FROM M_ITEM_SKU MIS
                  LEFT JOIN (SELECT CATEGORY_ID1
                                   ,MAX(CATEGORY_NAME1) CATEGORY_NAME1
                               FROM M_ITEM_CATEGORIES4
                              WHERE SHIPPER_ID = :SHIPPER_ID
                              GROUP BY CATEGORY_ID1
                             ) MIC4
                    ON MIS.CATEGORY_ID1 = MIC4.CATEGORY_ID1
                  LEFT JOIN M_COLORS MC
                    ON MIS.SHIPPER_ID = MC.SHIPPER_ID
                   AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN M_VENDORS MV
                    ON MIS.SHIPPER_ID = MV.SHIPPER_ID
                   AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                  LEFT JOIN M_BRANDS MB
                    ON MIS.SHIPPER_ID = MB.SHIPPER_ID
                   AND MIS.BRAND_ID = MB.BRAND_ID
                 WHERE MIS.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            // 分類
            if (!string.IsNullOrEmpty(condition.CategoryId1))
            {
                query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId2))
            {
                query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId3))
            {
                query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId4))
            {
                query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
            }

            // 品名
            if (!string.IsNullOrEmpty(condition.ItemName))
            {
                query.Append(@" AND MIS.ITEM_NAME LIKE :ITEM_NAME ");
                parameters.Add(":ITEM_NAME", condition.ItemName + "%");
            }

            // ブランド
            if (!string.IsNullOrEmpty(condition.BrandId))
            {
                query.Append(" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                parameters.Add(":BRAND_ID", condition.BrandId + "%");
            }

            // ブランド名
            if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
            {
                query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                parameters.Add(":BRAND_NAME", condition.BrandName + "%");
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND MIS.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            // 代表仕入先
            if (!string.IsNullOrEmpty(condition.MainVendorId))
            {
                query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :MAIN_VENDOR_ID ");
                parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId + "%");
            }

            // 代表仕入先名
            if (string.IsNullOrEmpty(condition.MainVendorId) && !string.IsNullOrEmpty(condition.MainVendorName))
            {
                query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                parameters.Add(":VENDOR_NAME1", condition.MainVendorName + "%");
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND MIS.JAN LIKE :JAN ");
                parameters.Add(":JAN", condition.Jan + "%");
            }

            // 事業部
            if (!string.IsNullOrEmpty(condition.DivisionId))
            {
                query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                parameters.Add(":DIVISION_ID", condition.DivisionId);
            }

            // 品番SKU
            if (!string.IsNullOrEmpty(condition.ItemSkuId))
            {
                query.Append(" AND MIS.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
            }

            // 商品ランク
            if (!string.IsNullOrEmpty(condition.ItemRankId))
            {
                query.Append(" AND MIS.ITEM_RANK_ID = :ITEM_RANK_ID ");
                parameters.Add(":ITEM_RANK_ID", condition.ItemRankId);
            }

            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND MIS.DELETE_FLAG <> 1");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case ItemSkuSortKey.ItemName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MIS.ITEM_NAME DESC,MIS.ITEM_SKU_ID DESC,MIS.JAN DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MIS.ITEM_NAME ASC,MIS.ITEM_SKU_ID ASC,MIS.JAN ASC");
                            break;
                    }

                    break;

                case ItemSkuSortKey.ItemSkuId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MIS.ITEM_SKU_ID DESC,MIS.JAN DESC,MIS.ITEM_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MIS.ITEM_SKU_ID ASC,MIS.JAN ASC,MIS.ITEM_NAME ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MIS.JAN DESC,MIS.ITEM_SKU_ID DESC,MIS.ITEM_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MIS.JAN ASC,MIS.ITEM_SKU_ID ASC,MIS.ITEM_NAME ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var ItemSkus = MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<Detail>(ItemSkus, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Get ItemSku By Id
        /// </summary>
        /// <param name="locClass">locClass</param>
        /// <param name=""></param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public Detail GetTargetById(string itemSkuId, string shipperId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MIS.SHIPPER_ID
                      ,MIS.UPDATE_COUNT
                      ,MIS.ITEM_SKU_ID
                      ,MIS.ITEM_ID
                      ,MIS.ITEM_NAME
                      ,MIS.ITEM_SHORT_NAME
                      ,MIS.BRAND_ID
                      ,MB.BRAND_NAME
                      ,MIS.CATEGORY_ID1
                      ,MIC4.CATEGORY_NAME1
                      ,MIS.CATEGORY_ID2
                      ,MIC4.CATEGORY_NAME2
                      ,MIS.CATEGORY_ID3
                      ,MIC4.CATEGORY_NAME3
                      ,MIS.CATEGORY_ID4
                      ,MIC4.CATEGORY_NAME4
                      ,MIS.NORMAL_SELLING_PRICE_EX_TAX
                      ,MIS.NORMAL_BUYING_PRICE
                      ,MIS.PURCHASE_BUYING_PRICE
                      ,MIS.PROPER_PRICE_EX_TAX
                      ,MIS.SEASON_YEAR
                      ,MIS.ITEM_SEASON_ID
                      ,MGEN.GEN_NAME AS ITEM_SEASON_NAME
                      ,MIS.SALE_START_DATE
                      ,MIS.SALE_END_DATE
                      ,MIS.ORIGIN_COUNTRY
                      ,MIS.ITEM_CUSTOMER_TYPE_ID
                      ,MG1.GEN_NAME ITEM_CUSTOMER_TYPE_NAME
                      ,MIS.ITEM_RANK_ID
                      ,MG.GEN_NAME ITEM_RANK_NAME
                      ,MIS.MAIN_VENDOR_ID
                      ,MV.VENDOR_NAME1 MAIN_VENDOR_NAME
                      ,MIS.CASE_IRISU
                      ,MIS.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,MIS.ITEM_SIZE_ID
                      ,MS.ITEM_SIZE_NAME
                      ,MIS.JAN
                      ,MIS.PIECE_VOL
                      ,MIS.DIVISION_ID
                      ,MD.DIVISION_NAME
                      ,MIS.NOVELTY_NAME
                      ,MIS.DELETE_FLAG
                      ,MIS.ITEM_CODE
                      ,ITEM_CODE.ITEM_CODE_NAME
                  FROM M_ITEM_SKU MIS
                  LEFT JOIN M_BRANDS MB
                    ON MIS.SHIPPER_ID = MB.SHIPPER_ID
                   AND MIS.BRAND_ID = MB.BRAND_ID
                  LEFT JOIN M_ITEM_CATEGORIES4 MIC4
                    ON MIS.SHIPPER_ID = MIC4.SHIPPER_ID
                   AND MIS.CATEGORY_ID1 = MIC4.CATEGORY_ID1
                   AND MIS.CATEGORY_ID2 = MIC4.CATEGORY_ID2
                   AND MIS.CATEGORY_ID3 = MIC4.CATEGORY_ID3
                   AND MIS.CATEGORY_ID4 = MIC4.CATEGORY_ID4
                  LEFT JOIN M_VENDORS MV
                    ON MIS.SHIPPER_ID = MV.SHIPPER_ID
                   AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                  LEFT JOIN M_COLORS MC
                    ON MIS.SHIPPER_ID = MC.SHIPPER_ID
                   AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN M_SIZES MS
                    ON MIS.SHIPPER_ID = MS.SHIPPER_ID
                   AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                  LEFT JOIN M_GENERALS MG
                    ON MIS.SHIPPER_ID = MG.SHIPPER_ID
                   AND MG.REGISTER_DIVI_CD = 1
                   AND MG.CENTER_ID = '@@@'
                   AND MG.GEN_DIV_CD = 'ITEM_RANKS'
                   AND MIS.ITEM_RANK_ID = MG.GEN_CD
                  LEFT JOIN M_GENERALS MG1
                    ON MIS.SHIPPER_ID = MG1.SHIPPER_ID
                   AND MG1.REGISTER_DIVI_CD = 1
                   AND MG1.CENTER_ID = '@@@'
                   AND MG1.GEN_DIV_CD = 'ITEM_CUSTOMER_TYPE'
                   AND MIS.ITEM_CUSTOMER_TYPE_ID = MG1.GEN_CD
                  LEFT JOIN M_DIVISIONS MD
                    ON MIS.SHIPPER_ID = MD.SHIPPER_ID
                   AND MIS.DIVISION_ID = MD.DIVISION_ID
                 LEFT JOIN M_GENERALS MGEN
                    ON MIS.SHIPPER_ID = MGEN.SHIPPER_ID
                   AND MGEN.REGISTER_DIVI_CD = '1'
                   AND MGEN.CENTER_ID = '@@@'
                   AND MGEN.GEN_DIV_CD = 'SEASON_NAME'
                   AND MIS.ITEM_SEASON_ID = MGEN.GEN_CD
                  LEFT JOIN M_ITEM_CODE ITEM_CODE
                    ON MIS.SHIPPER_ID = ITEM_CODE.SHIPPER_ID
                   AND MIS.ITEM_CODE = ITEM_CODE.ITEM_CODE
                 WHERE MIS.SHIPPER_ID = :SHIPPER_ID
                   AND MIS.ITEM_SKU_ID = :ITEM_SKU_ID

            ");
            parameters.Add(":SHIPPER_ID", shipperId);
            parameters.Add(":ITEM_SKU_ID", itemSkuId);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }


        public bool UpdateItemSku(Detail itemId)
        {
            var dbContext = MvcDbContext.Current;

            var updatedItemId =
                  dbContext.ItemSkus
                  .Where(m => m.ShipperId == itemId.ShipperId && m.ItemId == itemId.ItemId)
                  .ToList();

            //排他チェック用
            var ItemSkuCheck =
                  dbContext.ItemSkus
                  .Where(m => m.ShipperId == itemId.ShipperId && m.ItemSkuId == itemId.ItemSkuId && m.UpdateCount == itemId.UpdateCount)
                  .Any();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (!ItemSkuCheck)
            {
                return false;
            }

            for(int i =0; i < updatedItemId.Count(); i++ )
            {
                updatedItemId[i].SetBaseInfoUpdate();

                updatedItemId[i].NoveltyName = itemId.NoveltyName;
            }

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
        /// 分類1データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys1()
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId1,
                    Text = m.CategoryId1.ToString() + ":" + m.CategoryName1
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
                .Where(m => m.ShipperId == Profile.User.ShipperId  && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2,
                    Text = m.CategoryId2.ToString() + ":" + m.CategoryName2
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
                    Value = m.CategoryId3,
                    Text = m.CategoryId3.ToString() + ":" + m.CategoryName3
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
                    Value = m.CategoryId4,
                    Text = m.CategoryId4.ToString() + ":" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListDivisions()
        {
            return MvcDbContext.Current.Divisions
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.DivisionId,
                    Text = m.DivisionId + ":" + m.DivisionName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 商品ランクデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItemRanks()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT GEN_CD AS VALUE
                      ,GEN_NAME AS TEXT
                  FROM M_GENERALS MG
                 WHERE MG.SHIPPER_ID = :SHIPPER_ID
                   AND MG.REGISTER_DIVI_CD = :REGISTER_DIVI_CD
                   AND MG.GEN_DIV_CD = :GEN_DIV_CD
                 ORDER BY MG.ORDER_NO
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":REGISTER_DIVI_CD", "1");
            parameters.Add(":GEN_DIV_CD", "ITEM_RANKS");

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);

        }
    }
}