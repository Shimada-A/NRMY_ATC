namespace Wms.Areas.Master.Models
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.ViewModels.BoxSetting;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.BoxSetting.BoxSettingSearchCondition;

    public partial class BoxSetting
    {
        /// <summary>
        /// Get BoxSetting List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<BoxSettingList> GetData(BoxSettingSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = GetQuery();
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            // 分類
            if (!string.IsNullOrEmpty(condition.CategoryId1))
            {
                query.Append(" AND BOX_SETTING.CATEGORY_ID1 = :CATEGORY_ID1 ");
                parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId2))
            {
                query.Append(" AND BOX_SETTING.CATEGORY_ID2 = :CATEGORY_ID2 ");
                parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId3))
            {
                query.Append(" AND BOX_SETTING.CATEGORY_ID3 = :CATEGORY_ID3 ");
                parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId4))
            {
                query.Append(" AND BOX_SETTING.CATEGORY_ID4 = :CATEGORY_ID4 ");
                parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND BOX_SETTING.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<BoxSettingList>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case BoxSettingSortKey.ItemId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" ORDER BY BOX_SETTING.ITEM_ID DESC,BOX_SETTING.CATEGORY_ID1 DESC,BOX_SETTING.CATEGORY_ID2 DESC,BOX_SETTING.CATEGORY_ID3 DESC,BOX_SETTING.CATEGORY_ID4 DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY BOX_SETTING.ITEM_ID ASC,BOX_SETTING.CATEGORY_ID1 ASC,BOX_SETTING.CATEGORY_ID2 ASC,BOX_SETTING.CATEGORY_ID3 ASC,BOX_SETTING.CATEGORY_ID4 ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" ORDER BY BOX_SETTING.CATEGORY_ID1 DESC,BOX_SETTING.CATEGORY_ID2 DESC,BOX_SETTING.CATEGORY_ID3 DESC,BOX_SETTING.CATEGORY_ID4 DESC,BOX_SETTING.ITEM_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY BOX_SETTING.CATEGORY_ID1 ASC,BOX_SETTING.CATEGORY_ID2 ASC,BOX_SETTING.CATEGORY_ID3 ASC,BOX_SETTING.CATEGORY_ID4 ASC,BOX_SETTING.ITEM_ID ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var results = MvcDbContext.Current.Database.Connection.Query<BoxSettingList>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<BoxSettingList>(results, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<string> GetSettingsId(BoxSettingSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            StringBuilder query = new StringBuilder();
            query.Append(@" SELECT T.BOX_SETTINGS_ID FROM M_BOX_SETTINGS T  WHERE T.SHIPPER_ID = :SHIPPER_ID");

            if (!string.IsNullOrEmpty(condition.CategoryId1))
            {
                query.Append(" AND T.CATEGORY_ID1 = :CATEGORY_ID1 ");
                parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId2))
            {
                query.Append(" AND T.CATEGORY_ID2 = :CATEGORY_ID2 ");
                parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId3))
            {
                query.Append(" AND T.CATEGORY_ID3 = :CATEGORY_ID3 ");
                parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId4))
            {
                query.Append(" AND T.CATEGORY_ID4 = :CATEGORY_ID4 ");
                parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND T.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            return MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).ToList();

        }

        /// <summary>
        /// Get Store By Id
        /// </summary>
        /// <param name="locClass">locClass</param>
        /// <param name=""></param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public BoxSettingList GetTargetById(int boxSettingsId, string shipperId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = GetQuery();
            query.Append(@"
                    AND BOX_SETTING.BOX_SETTINGS_ID = :BOX_SETTINGS_ID
            ");
            parameters.Add(":SHIPPER_ID", shipperId);
            parameters.Add(":BOX_SETTINGS_ID", boxSettingsId);

            return MvcDbContext.Current.Database.Connection.Query<BoxSettingList>(query.ToString(), parameters).FirstOrDefault();

        }

        /// <summary>
        /// Delete BoxSetting
        /// </summary>
        /// <param name="lstBoxSetting">List record is deleted</param>
        /// <returns>List of rows error </returns>
        public bool Delete(List<string> settingsids)
        {

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in settingsids)
                {
                    int bsid = int.Parse(u);
                    var boxSetting =
                   MvcDbContext.Current.BoxSettings.Where(m => m.ShipperId == Profile.User.ShipperId && m.BoxSettingsId == bsid).SingleOrDefault();
                    if (boxSetting == null)
                    {
                        return false;
                    }

                    MvcDbContext.Current.BoxSettings.Remove(boxSetting);
                }
                try
                {
                    MvcDbContext.Current.SaveChanges();
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
        /// Update BoxSetting
        /// </summary>
        /// <param name="boxSetting"></param>
        /// <returns>Update status</returns>
        public bool UpdateBoxSetting(BoxSettingList boxSetting)
        {
            var dbContext = MvcDbContext.Current;

            var updatedBoxSetting =
                  MvcDbContext.Current.BoxSettings
                  .Where(m => m.ShipperId == boxSetting.ShipperId && m.BoxSettingsId == boxSetting.BoxSettingsId && m.UpdateCount == boxSetting.UpdateCount)
                  .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合） 
            if (updatedBoxSetting == null)
            {
                return false;
            }

            updatedBoxSetting.SetBaseInfoUpdate();
            updatedBoxSetting.ThresholdClass = boxSetting.ThresholdClass;
            if (boxSetting.ThresholdClass == ThresholdClasses.ThresholdRate)
            {
                updatedBoxSetting.Threshold = boxSetting.ThresholdRate == null ? 0 : boxSetting.ThresholdRate;
            }
            else if (boxSetting.ThresholdClass == ThresholdClasses.ThresholdSku)
            {
                updatedBoxSetting.Threshold = boxSetting.ThresholdSku == null ? 0 : boxSetting.ThresholdSku;
            }
            else
            {
                updatedBoxSetting.Threshold = 0;
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
                    Value = m.CategoryId1.ToString(),
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
                .Where(m => m.ShipperId == Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2.ToString(),
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
                    Value = m.CategoryId3.ToString(),
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
                    Value = m.CategoryId4.ToString(),
                    Text = m.CategoryId4.ToString() + ":" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// Create new BoxSetting
        /// </summary>
        /// <param name="boxSetting"></param>
        /// <returns></returns>
        public bool Create(BoxSettingList row)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                var boxSettings = MvcDbContext.Current.BoxSettings.Where(x => x.ShipperId == Common.Profile.User.ShipperId);

                var boxSetting = new BoxSetting();
                boxSetting.BoxSettingsId = (boxSettings.Any() ? boxSettings.Select(x => x.BoxSettingsId).Max() : 0) + 1;
                boxSetting.ThresholdClass = row.ThresholdClass;

                if (row.ItemId == null)
                {
                    boxSetting.CategoryId1 = row.CategoryId1;
                    boxSetting.CategoryId2 = row.CategoryId2;
                    boxSetting.CategoryId3 = row.CategoryId3;
                    boxSetting.CategoryId4 = row.CategoryId4;
                }
                else
                {
                    boxSetting.ItemId = row.ItemId;
                }

                if (row.ThresholdClass == ThresholdClasses.ThresholdRate)
                {
                    boxSetting.Threshold = row.ThresholdRate == null ? 0 : row.ThresholdRate;
                }
                else if (boxSetting.ThresholdClass == ThresholdClasses.ThresholdSku)
                {
                    boxSetting.Threshold = row.ThresholdSku == null ? 0 : row.ThresholdSku;
                }
                else
                {
                    boxSetting.Threshold = 0;
                }

                boxSetting.SetBaseInfoInsert();
                MvcDbContext.Current.BoxSettings.Add(boxSetting);

                try
                {
                    MvcDbContext.Current.SaveChanges();
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// Get Query
        /// </summary>
        /// <returns></returns>
        public static StringBuilder GetQuery()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                WITH
                    ITEM AS (
                        SELECT
                                SKU.SHIPPER_ID
                            ,   SKU.ITEM_ID
                            ,   MAX(SKU.ITEM_NAME) AS ITEM_NAME
                        FROM
                                M_ITEM_SKU SKU
                        GROUP BY
                                SKU.SHIPPER_ID
                            ,   SKU.ITEM_ID
                )
                ,   CATE1 AS (
                        SELECT
                                CATE.SHIPPER_ID
                            ,   CATE.CATEGORY_ID1
                            ,   MAX(CATE.CATEGORY_NAME1) AS CATEGORY_NAME1
                        FROM
                                M_ITEM_CATEGORIES4 CATE
                        GROUP BY
                                CATE.SHIPPER_ID
                            ,   CATE.CATEGORY_ID1
                )
                ,   CATE2 AS (
                        SELECT
                                CATE.SHIPPER_ID
                            ,   CATE.CATEGORY_ID1
                            ,   CATE.CATEGORY_ID2
                            ,   MAX(CATE.CATEGORY_NAME2) AS CATEGORY_NAME2
                        FROM
                                M_ITEM_CATEGORIES4 CATE
                        GROUP BY
                                CATE.SHIPPER_ID
                            ,   CATE.CATEGORY_ID1
                            ,   CATE.CATEGORY_ID2
                )
                ,   CATE3 AS (
                        SELECT
                                CATE.SHIPPER_ID
                            ,   CATE.CATEGORY_ID1
                            ,   CATE.CATEGORY_ID2
                            ,   CATE.CATEGORY_ID3
                            ,   MAX(CATE.CATEGORY_NAME3) AS CATEGORY_NAME3
                        FROM
                                M_ITEM_CATEGORIES4 CATE
                        GROUP BY
                                CATE.SHIPPER_ID
                            ,   CATE.CATEGORY_ID1
                            ,   CATE.CATEGORY_ID2
                            ,   CATE.CATEGORY_ID3
                )
                ,   CATE4 AS (
                        SELECT
                                CATE.SHIPPER_ID
                            ,   CATE.CATEGORY_ID1
                            ,   CATE.CATEGORY_ID2
                            ,   CATE.CATEGORY_ID3
                            ,   CATE.CATEGORY_ID4
                            ,   MAX(CATE.CATEGORY_NAME4) AS CATEGORY_NAME4
                        FROM
                                M_ITEM_CATEGORIES4 CATE
                        GROUP BY
                                CATE.SHIPPER_ID
                            ,   CATE.CATEGORY_ID1
                            ,   CATE.CATEGORY_ID2
                            ,   CATE.CATEGORY_ID3
                            ,   CATE.CATEGORY_ID4
                )
                SELECT
                        BOX_SETTING.UPDATE_COUNT
                    ,   BOX_SETTING.SHIPPER_ID
                    ,   BOX_SETTING.BOX_SETTINGS_ID
                    ,   BOX_SETTING.CATEGORY_ID1
                    ,   CATE1.CATEGORY_NAME1
                    ,   BOX_SETTING.CATEGORY_ID2
                    ,   CATE2.CATEGORY_NAME2
                    ,   BOX_SETTING.CATEGORY_ID3
                    ,   CATE3.CATEGORY_NAME3
                    ,   BOX_SETTING.CATEGORY_ID4
                    ,   CATE4.CATEGORY_NAME4
                    ,   BOX_SETTING.ITEM_ID
                    ,   ITEM.ITEM_NAME
                    ,   BOX_SETTING.THRESHOLD_CLASS
                    ,   CASE
                            WHEN BOX_SETTING.THRESHOLD_CLASS = 2 THEN BOX_SETTING.THRESHOLD
                            ELSE NULL
                        END THRESHOLD_SKU
                    ,   CASE
                            WHEN BOX_SETTING.THRESHOLD_CLASS = 1 THEN BOX_SETTING.THRESHOLD
                            ELSE NULL
                        END THRESHOLD_RATE
                FROM
                        M_BOX_SETTINGS BOX_SETTING
                LEFT JOIN 
                        CATE1
                ON
                        CATE1.SHIPPER_ID = BOX_SETTING.SHIPPER_ID
                    AND CATE1.CATEGORY_ID1 = BOX_SETTING.CATEGORY_ID1
                LEFT JOIN 
                        CATE2
                ON
                        CATE2.SHIPPER_ID = BOX_SETTING.SHIPPER_ID
                    AND CATE2.CATEGORY_ID1 = BOX_SETTING.CATEGORY_ID1
                    AND CATE2.CATEGORY_ID2 = BOX_SETTING.CATEGORY_ID2
                LEFT JOIN 
                        CATE3
                ON
                        CATE3.SHIPPER_ID = BOX_SETTING.SHIPPER_ID
                    AND CATE3.CATEGORY_ID1 = BOX_SETTING.CATEGORY_ID1
                    AND CATE3.CATEGORY_ID2 = BOX_SETTING.CATEGORY_ID2
                    AND CATE3.CATEGORY_ID3 = BOX_SETTING.CATEGORY_ID3
                LEFT JOIN 
                        CATE4
                ON
                        CATE4.SHIPPER_ID = BOX_SETTING.SHIPPER_ID
                    AND CATE4.CATEGORY_ID1 = BOX_SETTING.CATEGORY_ID1
                    AND CATE4.CATEGORY_ID2 = BOX_SETTING.CATEGORY_ID2
                    AND CATE4.CATEGORY_ID3 = BOX_SETTING.CATEGORY_ID3
                    AND CATE4.CATEGORY_ID4 = BOX_SETTING.CATEGORY_ID4
                LEFT JOIN 
                        ITEM
                ON
                        ITEM.SHIPPER_ID = BOX_SETTING.SHIPPER_ID
                    AND ITEM.ITEM_ID = BOX_SETTING.ITEM_ID
                WHERE
                        BOX_SETTING.SHIPPER_ID = :SHIPPER_ID
            ");

            return query;
        }
    }
}