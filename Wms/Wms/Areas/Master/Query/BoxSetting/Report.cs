namespace Wms.Areas.Master.Query.BoxSetting
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Share.Common.Resources;
    using Share.Reports.Import;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.BoxSetting;
    using Wms.Common;
    using Wms.Common.Resources;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;
    using static Wms.Areas.Master.ViewModels.BoxSetting.BoxSettingSearchCondition;

    public class Report : BaseQuery
    {
        /// <summary>
        /// Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<ViewModels.BoxSetting.Report> Listing(BoxSettingSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MBS.CATEGORY_ID1
                      ,MBS.CATEGORY_ID2
                      ,MBS.CATEGORY_ID3
                      ,MBS.CATEGORY_ID4
                      ,MBS.ITEM_ID
                      ,MBS.THRESHOLD_CLASS
                      ,MBS.THRESHOLD
                  FROM M_BOX_SETTINGS MBS
                 WHERE MBS.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            // 分類
            if (!string.IsNullOrEmpty(condition.CategoryId1))
            {
                query.Append(" AND MBS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId2))
            {
                query.Append(" AND MBS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId3))
            {
                query.Append(" AND MBS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
            }

            if (!string.IsNullOrEmpty(condition.CategoryId4))
            {
                query.Append(" AND MBS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND MBS.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }
            // Sort function
            switch (condition.SortKey)
            {
                case BoxSettingSortKey.ItemId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" ORDER BY MBS.ITEM_ID DESC,MBS.CATEGORY_ID1 DESC,MBS.CATEGORY_ID2 DESC,MBS.CATEGORY_ID3 DESC,MBS.CATEGORY_ID4 DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY MBS.ITEM_ID ASC,MBS.CATEGORY_ID1 ASC,MBS.CATEGORY_ID2 ASC,MBS.CATEGORY_ID3 ASC,MBS.CATEGORY_ID4 ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@" ORDER BY MBS.CATEGORY_ID1 DESC,MBS.CATEGORY_ID2 DESC,MBS.CATEGORY_ID3 DESC,MBS.CATEGORY_ID4 DESC,MBS.ITEM_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY MBS.CATEGORY_ID1 ASC,MBS.CATEGORY_ID2 ASC,MBS.CATEGORY_ID3 ASC,MBS.CATEGORY_ID4 ASC,MBS.ITEM_ID ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ViewModels.BoxSetting.Report>(query.ToString(), parameters);
        }

        /// <summary>
        /// アップロードされたデータのチェック
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public bool UploadCheck(long workId, List<MasBoxSetting> report)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var data in report.Select((v, i) => new { v, i }))
                {
                    //品番または分類 未入力チェック
                    if (string.IsNullOrWhiteSpace(data.v.CategoryId1) && string.IsNullOrWhiteSpace(data.v.ItemId))
                    {
                        data.v.ErrMsg = MessageResource.CategoryItemRequird;
                    }
                    else if(string.IsNullOrWhiteSpace(data.v.CategoryId1) && (!string.IsNullOrWhiteSpace(data.v.CategoryId2) || !string.IsNullOrWhiteSpace(data.v.CategoryId3) || !string.IsNullOrWhiteSpace(data.v.CategoryId4)))
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.ErrNotInput, BoxSettingResource.CategoryId1);
                    }
                    else if (!string.IsNullOrWhiteSpace(data.v.CategoryId1) && string.IsNullOrWhiteSpace(data.v.CategoryId2) && (!string.IsNullOrWhiteSpace(data.v.CategoryId3) || !string.IsNullOrWhiteSpace(data.v.CategoryId4)))
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.ErrNotInput, BoxSettingResource.CategoryId2);
                    }
                    else if (!string.IsNullOrWhiteSpace(data.v.CategoryId1) && !string.IsNullOrWhiteSpace(data.v.CategoryId2) && string.IsNullOrWhiteSpace(data.v.CategoryId3) && !string.IsNullOrWhiteSpace(data.v.CategoryId4))
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.ErrNotInput, BoxSettingResource.CategoryId3);
                    }

                    // 分類マスタ存在チェック
                    if (data.v.ErrMsg == null && !string.IsNullOrWhiteSpace(data.v.CategoryId1))
                    {
                        data.v.ErrMsg = CheckCategory(data.v.CategoryId1, data.v.CategoryId2, data.v.CategoryId3, data.v.CategoryId4, BoxSettingResource.Category);
                    }
                    // 品番マスタの分類チェック
                    if (data.v.ErrMsg == null && !string.IsNullOrWhiteSpace(data.v.ItemId))
                    {
                        data.v.ErrMsg = CheckItem(data.v.ItemId, data.v.CategoryId1, data.v.CategoryId2, data.v.CategoryId3, data.v.CategoryId4);
                    }

                    //閾値区分、閾値チェック
                    if (data.v.ErrMsg == null && string.IsNullOrWhiteSpace(data.v.ThresholdClass))
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.Required, BoxSettingResource.ThresholdClass);
                    }
                    else if (data.v.ErrMsg == null && data.v.ThresholdClass.Trim() != "1" && data.v.ThresholdClass.Trim() != "2")
                    {
                        data.v.ErrMsg = MessageResource.ThresholdClassError;
                    }
                    else
                    {
                        data.v.ErrMsg = data.v.ErrMsg ?? CheckThreshold((ThresholdClasses)int.Parse(data.v.ThresholdClass),data.v.Threshold);
                    }

                    // ファイル内重複チェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckPrimaryKey(report, data.v);

                    if (data.v.ErrMsg != null)
                    {
                        data.v.SetBaseInfoUpdate();
                        try
                        {
                            MvcDbContext.Current.SaveChanges();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return false;
                        }
                    }
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// アップロードされたデータのImport
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public long InsertWwMasBoxSettings(IEnumerable<ViewModels.BoxSetting.Report> report)
        {
            var dbContext = MvcDbContext.Current;
            var workId = GetWorkId();
            var no = 0;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                foreach (var u in report.Select((v, i) => new { v, i }))
                {
                    no = no + 1;
                    var boxSetting = new Models.MasBoxSetting
                    {
                        Seq = workId,
                        No = no,
                        CategoryId1 = u.v.CategoryId1,
                        CategoryId2 = u.v.CategoryId2,
                        CategoryId3 = u.v.CategoryId3,
                        CategoryId4 = u.v.CategoryId4,
                        ItemId = u.v.ItemId,
                        ThresholdClass = u.v.ThresholdClass,
                        Threshold = u.v.Threshold
                    };
                    boxSetting.SetBaseInfoInsert();
                    dbContext.MasBoxSettings.Add(boxSetting);
                    try
                    {
                        dbContext.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                    }
                }

                trans.Commit();
            }

            return workId;
        }

        /// <summary>
        /// チェック結果をワークテーブルへ更新
        /// </summary>
        /// <param name="report"></param>
        public void MergeBoxSettings(long workId)
        {
            var sql = @"
                MERGE INTO
                        M_BOX_SETTINGS T
                USING (
                    SELECT
                            WW.SHIPPER_ID
                        ,   WW.NO
                        ,   DECODE(WW.ITEM_ID,NULL,WW.CATEGORY_ID1,NULL) AS CATEGORY_ID1
                        ,   DECODE(WW.ITEM_ID,NULL,WW.CATEGORY_ID2,NULL) AS CATEGORY_ID2
                        ,   DECODE(WW.ITEM_ID,NULL,WW.CATEGORY_ID3,NULL) AS CATEGORY_ID3
                        ,   DECODE(WW.ITEM_ID,NULL,WW.CATEGORY_ID4,NULL) AS CATEGORY_ID4
                        ,   WW.ITEM_ID
                        ,   WW.THRESHOLD_CLASS
                        ,   WW.THRESHOLD
                    FROM
                            WW_MAS_BOX_SETTINGS WW
                    WHERE
                            WW.SHIPPER_ID = :SHIPPER_ID
                        AND WW.SEQ = :SEQ
                        AND WW.ERR_MSG IS NULL
                ) WW
                ON (
                        WW.SHIPPER_ID = WW.SHIPPER_ID
                    AND NVL(WW.CATEGORY_ID1,' ') = NVL(T.CATEGORY_ID1,' ')
                    AND NVL(WW.CATEGORY_ID2,' ') = NVL(T.CATEGORY_ID2,' ')
                    AND NVL(WW.CATEGORY_ID3,' ') = NVL(T.CATEGORY_ID3,' ')
                    AND NVL(WW.CATEGORY_ID4,' ') = NVL(T.CATEGORY_ID4,' ')
                    AND NVL(WW.ITEM_ID,' ') = NVL(T.ITEM_ID,' ')
                )
                WHEN MATCHED THEN
                    UPDATE SET
                            T.UPDATE_DATE = SYSDATE
                        ,   T.UPDATE_USER_ID = :USER_ID
                        ,   T.UPDATE_PROGRAM_NAME = :PROGRAM_NAME
                        ,   T.UPDATE_COUNT = T.UPDATE_COUNT + 1
                        ,   T.THRESHOLD_CLASS = WW.THRESHOLD_CLASS
                        ,   T.THRESHOLD = WW.THRESHOLD
                WHEN NOT MATCHED THEN
                    INSERT (
                            T.MAKE_DATE
                        ,   T.MAKE_USER_ID
                        ,   T.MAKE_PROGRAM_NAME
                        ,   T.UPDATE_DATE
                        ,   T.UPDATE_USER_ID
                        ,   T.UPDATE_PROGRAM_NAME
                        ,   T.UPDATE_COUNT
                        ,   T.SHIPPER_ID
                        ,   T.BOX_SETTINGS_ID
                        ,   T.CATEGORY_ID1
                        ,   T.CATEGORY_ID2
                        ,   T.CATEGORY_ID3
                        ,   T.CATEGORY_ID4
                        ,   T.ITEM_ID
                        ,   T.THRESHOLD_CLASS
                        ,   T.THRESHOLD
                    ) VALUES (
                            SYSDATE
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   SYSDATE
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   0
                        ,   WW.SHIPPER_ID
                        ,   :BOX_SETTINGS_ID + WW.NO
                        ,   WW.CATEGORY_ID1
                        ,   WW.CATEGORY_ID2
                        ,   WW.CATEGORY_ID3
                        ,   WW.CATEGORY_ID4
                        ,   WW.ITEM_ID
                        ,   WW.THRESHOLD_CLASS
                        ,   WW.THRESHOLD
                    )
            ";
            var parameters = new DynamicParameters();

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", workId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);
            parameters.Add(":PROGRAM_NAME", nameof(MenuResource.W_MAS_BoxSetting01));
            parameters.Add(":BOX_SETTINGS_ID", _dbContext.BoxSettings.Where(x => x.ShipperId == Common.Profile.User.ShipperId).Select(x => x.BoxSettingsId).Max());
            _dbContext.Database.Connection.Execute(sql, parameters);
        }

        /// <summary>
        /// Get BoxSetting List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<IndexResultRow> GetReportErrList(UploadCondition conditions)
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
                        WW.SEQ
                    ,   WW.NO
                    ,   WW.UPDATE_COUNT
                    ,   WW.SHIPPER_ID
                    ,   WW.CATEGORY_ID1
                    ,   CATE1.CATEGORY_NAME1
                    ,   WW.CATEGORY_ID2
                    ,   CATE2.CATEGORY_NAME2
                    ,   WW.CATEGORY_ID3
                    ,   CATE3.CATEGORY_NAME3
                    ,   WW.CATEGORY_ID4
                    ,   CATE4.CATEGORY_NAME4
                    ,   WW.ITEM_ID
                    ,   ITEM.ITEM_NAME
                    ,   WW.THRESHOLD_CLASS
                    ,   CASE
                            WHEN WW.THRESHOLD_CLASS = '2' THEN WW.THRESHOLD
                            ELSE NULL
                        END THRESHOLD_SKU
                    ,   CASE
                            WHEN WW.THRESHOLD_CLASS = '1' THEN WW.THRESHOLD
                            ELSE NULL
                        END THRESHOLD_RATE
                    ,   WW.ERR_MSG
                FROM
                        WW_MAS_BOX_SETTINGS WW
                LEFT JOIN 
                        CATE1
                ON
                        CATE1.SHIPPER_ID = WW.SHIPPER_ID
                    AND CATE1.CATEGORY_ID1 = WW.CATEGORY_ID1
                LEFT JOIN 
                        CATE2
                ON
                        CATE2.SHIPPER_ID = WW.SHIPPER_ID
                    AND CATE2.CATEGORY_ID1 = WW.CATEGORY_ID1
                    AND CATE2.CATEGORY_ID2 = WW.CATEGORY_ID2
                LEFT JOIN 
                        CATE3
                ON
                        CATE3.SHIPPER_ID = WW.SHIPPER_ID
                    AND CATE3.CATEGORY_ID1 = WW.CATEGORY_ID1
                    AND CATE3.CATEGORY_ID2 = WW.CATEGORY_ID2
                    AND CATE3.CATEGORY_ID3 = WW.CATEGORY_ID3
                LEFT JOIN 
                        CATE4
                ON
                        CATE4.SHIPPER_ID = WW.SHIPPER_ID
                    AND CATE4.CATEGORY_ID1 = WW.CATEGORY_ID1
                    AND CATE4.CATEGORY_ID2 = WW.CATEGORY_ID2
                    AND CATE4.CATEGORY_ID3 = WW.CATEGORY_ID3
                    AND CATE4.CATEGORY_ID4 = WW.CATEGORY_ID4
                LEFT JOIN 
                        ITEM
                ON
                        ITEM.SHIPPER_ID = WW.SHIPPER_ID
                    AND ITEM.ITEM_ID = WW.ITEM_ID
                WHERE
                        WW.SHIPPER_ID = :SHIPPER_ID
                    AND WW.SEQ = :SEQ
                ORDER BY
                        WW.NO ASC
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":SEQ", conditions.WorkId);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<IndexResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", conditions.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (conditions.Page - 1) * conditions.PageSize });

            // Fill data to memory
            var results = MvcDbContext.Current.Database.Connection.Query<IndexResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<IndexResultRow>(results, conditions.Page, conditions.PageSize, totalCount);
        }

        /// <summary>
        /// 分類チェック
        /// </summary>
        /// <param name="categoryId1">分類1</param>
        /// <param name="categoryId2">分類2</param>
        /// <param name="categoryId3">分類3</param>
        /// <param name="categoryId4">分類4</param>
        /// <param name="resource"></param>
        /// <returns></returns>

        private string CheckCategory(string categoryId1, string categoryId2, string categoryId3, string categoryId4, string resource)
        {
            string errMsg = null;
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        'X'
                FROM
                        M_ITEM_CATEGORIES4 CATE
                WHERE
                        CATE.SHIPPER_ID = :SHIPPER_ID
                    AND CATE.CATEGORY_ID1 = :CATEGORY_ID1
                    AND CATE.CATEGORY_ID2 = NVL(:CATEGORY_ID2,CATE.CATEGORY_ID2)
                    AND CATE.CATEGORY_ID3 = NVL(:CATEGORY_ID3,CATE.CATEGORY_ID3)
                    AND CATE.CATEGORY_ID4 = NVL(:CATEGORY_ID4,CATE.CATEGORY_ID4)
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CATEGORY_ID1", categoryId1);
            parameters.Add(":CATEGORY_ID2", categoryId2);
            parameters.Add(":CATEGORY_ID3", categoryId3);
            parameters.Add(":CATEGORY_ID4", categoryId4);

            if (!MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).Any())
            {
                errMsg = string.Format(MessagesResource.MasterNotExistsError, resource);
            }
            return errMsg;
        }

        /// <summary>
        /// 品番チェック
        /// </summary>
        /// <param name="itemId">品番</param>
        /// <param name="categoryId1">分類1</param>
        /// <param name="categoryId2">分類2</param>
        /// <param name="categoryId3">分類3</param>
        /// <param name="categoryId4">分類4</param>
        /// <returns></returns>

        private string CheckItem(string itemId, string categoryId1 = null, string categoryId2 = null, string categoryId3 = null, string categoryId4 = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        ITEM.CATEGORY_ID1
                    ,   ITEM.CATEGORY_ID2
                    ,   ITEM.CATEGORY_ID3
                    ,   ITEM.CATEGORY_ID4
                FROM
                        M_ITEM_SKU ITEM
                WHERE
                        ITEM.SHIPPER_ID = :SHIPPER_ID
                    AND ITEM.ITEM_ID = :ITEM_ID
                GROUP BY
                        ITEM.CATEGORY_ID1
                    ,   ITEM.CATEGORY_ID2
                    ,   ITEM.CATEGORY_ID3
                    ,   ITEM.CATEGORY_ID4
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":ITEM_ID", itemId);

            var result = MvcDbContext.Current.Database.Connection.Query<ItemSku>(query.ToString(), parameters).FirstOrDefault();

            if (result == null)
            {
                return string.Format(MessagesResource.MasterNotExistsError, BoxSettingResource.ItemId);
            }

            if (!string.IsNullOrWhiteSpace(categoryId1) && result.CategoryId1 != categoryId1)
            {
                return string.Format(MessagesResource.NotSameError, BoxSettingResource.ItemId, BoxSettingResource.CategoryId1);
            }

            if (!string.IsNullOrWhiteSpace(categoryId2) && result.CategoryId2 != categoryId2)
            {
                return string.Format(MessagesResource.NotSameError, BoxSettingResource.ItemId, BoxSettingResource.CategoryId2);
            }

            if (!string.IsNullOrWhiteSpace(categoryId3) && result.CategoryId3 != categoryId3)
            {
                return string.Format(MessagesResource.NotSameError, BoxSettingResource.ItemId, BoxSettingResource.CategoryId3);
            }

            if (!string.IsNullOrWhiteSpace(categoryId4) && result.CategoryId4 != categoryId4)
            {
                return string.Format(MessagesResource.NotSameError, BoxSettingResource.ItemId, BoxSettingResource.CategoryId4);
            }

            return null;
        }

        /// <summary>
        /// 閾値チェック
        /// </summary>
        /// <param name="thresholdClass">閾値区分</param>
        /// <param name="threshold">閾値</param>
        /// <returns></returns>

        private string CheckThreshold(ThresholdClasses thresholdClass , string threshold)
        {
            string errMsg = null;
            bool intFlag = true;
            double min = 0;
            double max = 999999999;
            string resource = "";

            if (thresholdClass == ThresholdClasses.ThresholdRate)
            {
                intFlag = false;
                max = 100;
                resource = BoxSettingResource.ThresholdRate;
            }
            else if (thresholdClass == ThresholdClasses.ThresholdSku)
            {
                intFlag = true;
                max = 999999999;
                resource = BoxSettingResource.ThresholdSku;
            }

            // 未入力チェック
            if (string.IsNullOrWhiteSpace(threshold))
            {
                errMsg = string.Format(MessagesResource.Required, BoxSettingResource.Threshold);
            }
            // 数値チェック
            else if (!new ExcelReader<ViewModels.BoxSetting.Report>().CheckNumber(threshold , intFlag))
            {
                errMsg = string.Format(MessagesResource.FieldMustBeNumeric, BoxSettingResource.Threshold);
            }
            else if (thresholdClass == ThresholdClasses.ThresholdSku && !new ExcelReader<ViewModels.BoxSetting.Report>().CheckNumber(threshold,true))
            {
                errMsg = string.Format(MessagesResource.NotIntError, resource);
            }
            // 範囲チェック
            else if ((double.Parse(threshold) < min || double.Parse(threshold) > max))
            {
                errMsg = string.Format(MessagesResource.Range, resource, min.ToString(), max.ToString());
            }
            return errMsg;
        }

        /// <summary>
        /// ファイル内重複チェック
        /// </summary>
        /// <param name="report"></param>
        /// <param name="data"></param>
        /// <returns></returns>

        private string CheckPrimaryKey(List<MasBoxSetting> report, MasBoxSetting data)
        {
            if (!string.IsNullOrEmpty(data.CategoryId1) &&
                report.Where(x => x.CategoryId1 == data.CategoryId1
                               && x.CategoryId2 == (data.CategoryId2.Trim() ?? x.CategoryId2)
                               && x.CategoryId3 == (data.CategoryId3.Trim() ?? x.CategoryId3)
                               && x.CategoryId4 == (data.CategoryId4.Trim() ?? x.CategoryId4)).Count() > 1)
            {
                return string.Format(MessageResource.ErrorUploadUniqueConstraintViolated, BoxSettingResource.CategoryId1 + "-" + BoxSettingResource.CategoryId2 + "-" + BoxSettingResource.CategoryId3 + "-" + BoxSettingResource.CategoryId4);
            }

            if (report.Where(x => x.CategoryId1 == (data.CategoryId1.Trim() ?? x.CategoryId1)
                               && x.CategoryId2 == (data.CategoryId2.Trim() ?? x.CategoryId2)
                               && x.CategoryId3 == (data.CategoryId3.Trim() ?? x.CategoryId3)
                               && x.CategoryId4 == (data.CategoryId4.Trim() ?? x.CategoryId4)
                               && x.ItemId == data.ItemId).Count() > 1)
            {
                return string.Format(MessageResource.ErrorUploadUniqueConstraintViolated, BoxSettingResource.ItemId);
            }

            return null;
        }
    }
}