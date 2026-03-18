namespace Wms.Areas.Master.Query.Location
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Dapper;
    using PagedList;
    using Share.Common.Resources;
    using Share.Reports.Import;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.Location;
    using Wms.Common;
    using Wms.Common.Resources;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;
    using static Wms.Areas.Master.ViewModels.Location.LocationSearchCondition;

    public class Report : BaseQuery
    {
        /// <summary>
        /// Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<ViewModels.Location.Report> LocationListing(LocationSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       ML.LOCATION_CD
                      ,ML.LOCSEC_1
                      ,ML.LOCSEC_2
                      ,ML.LOCSEC_3
                      ,ML.LOCSEC_4
                      ,ML.LOCSEC_5
                      ,ML.LOCATION_CLASS
                      ,MLC.LOCATION_NAME
                      ,ML.CASE_CLASS
                      ,GEN_CASE_CLASS.GEN_NAME AS CASE_CLASS_NAME
                      ,ML.GRADE_ID
                      ,ML.ALLOC_PRIORITY
                      ,ML.PICKING_GROUP_NO
                      ,ML.INVENTORY_NO
                      ,ML.INVENTORY_NAME
                      ,ML.INVENTORY_CONFIRM_FLAG
                      ,TO_CHAR(ML.INVENTORY_START_DATE,'YYYY/MM/DD') AS INVENTORY_START_DATE
                      ,TO_CHAR(ML.INVENTORY_CONFIRM_DATE,'YYYY/MM/DD') AS INVENTORY_CONFIRM_DATE
                  FROM M_LOCATIONS ML
                  LEFT JOIN
                       M_LOCATION_CLASSES MLC
                   ON
                       ML.SHIPPER_ID = MLC.SHIPPER_ID
                   AND ML.LOCATION_CLASS = MLC.LOCATION_CLASS
                  LEFT JOIN
                       M_GENERALS GEN_CASE_CLASS
                  ON
                       GEN_CASE_CLASS.SHIPPER_ID = ML.SHIPPER_ID
                   AND GEN_CASE_CLASS.CENTER_ID = '@@@'
                   AND GEN_CASE_CLASS.REGISTER_DIVI_CD = '1'
                   AND GEN_CASE_CLASS.GEN_DIV_CD = 'CASE_CLASS'
                   AND GEN_CASE_CLASS.GEN_CD = TO_CHAR(ML.CASE_CLASS)
                 WHERE ML.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND ML.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            if (!string.IsNullOrEmpty(condition.LocationClass))
            {
                query.Append(" AND ML.LOCATION_CLASS = :LOCATION_CLASS ");
                parameters.Add(":LOCATION_CLASS", condition.LocationClass);
            }

            if (!string.IsNullOrEmpty(condition.Locsec1))
            {
                query.Append(" AND ML.LOCSEC_1 = :LOCSEC_1 ");
                parameters.Add(":LOCSEC_1", condition.Locsec1);
            }

            if (!string.IsNullOrEmpty(condition.Locsec2))
            {
                query.Append(" AND ML.LOCSEC_2 = :LOCSEC_2 ");
                parameters.Add(":LOCSEC_2", condition.Locsec2);
            }

            if (!string.IsNullOrEmpty(condition.Locsec3))
            {
                query.Append(" AND ML.LOCSEC_3 = :LOCSEC_3 ");
                parameters.Add(":LOCSEC_3", condition.Locsec3);
            }

            if (!string.IsNullOrEmpty(condition.Locsec4))
            {
                query.Append(" AND ML.LOCSEC_4 = :LOCSEC_4 ");
                parameters.Add(":LOCSEC_4", condition.Locsec4);
            }

            if (!string.IsNullOrEmpty(condition.Locsec5))
            {
                query.Append(" AND ML.LOCSEC_5 LIKE :LOCSEC_5 ");
                parameters.Add(":LOCSEC_5", "%" + condition.Locsec5 + "%");
            }

            // Sort function
            switch (condition.SortKey)
            {
                case LocationSortKey.LocationCd:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CD DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CD ASC");
                            break;
                    }

                    break;

                case LocationSortKey.LocationClass:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CLASS DESC, ML.LOCATION_CD DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CLASS ASC, ML.LOCATION_CD ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CD DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CD ASC");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ViewModels.Location.Report>(query.ToString(), parameters);
        }

        /// <summary>
        /// Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<ViewModels.Location.ReportTemp> LocationListingTemp(LocationSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       '' AS LOCSEC_1
                      ,'' AS LOCSEC_2
                      ,'' AS LOCSEC_3
                      ,'' AS LOCSEC_4
                      ,'' AS LOCSEC_5
                      ,'' AS LOCATION_CLASS
                      ,'' AS GRADE_ID
                      ,'' AS ALLOC_PRIORITY
                      ,'' AS PICKING_GROUP_NO
                  FROM DUAL
            ");
            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ViewModels.Location.ReportTemp>(query.ToString(), parameters);
        }

        /// <summary>
        /// アップロードされたデータのチェック
        /// </summary>
        /// <param name="workId">workId</param>
        /// <param name="MasLocation">report</param>
        /// <param name="centerId">centerId</param>
        /// <returns></returns>
        public bool UploadCheck(long workId, List<MasLocation> report, string centerId)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var data in report.Select((v, i) => new { v, i }))
                {
                    // エリア
                    data.v.ErrMsg = CheckLocsec(data.v.Locsec_1, LocationResource.Locsec1);
                    // 棚列
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckLocsec(data.v.Locsec_2, LocationResource.Locsec2);
                    // 棚番
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckLocsec(data.v.Locsec_3, LocationResource.Locsec3);
                    // 段
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckLocsec(data.v.Locsec_4, LocationResource.Locsec4);
                    // 間口
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckLocsec(data.v.Locsec_5, LocationResource.Locsec5);

                    // ロケーションコードが、ロケマスタメンテ不可のロケ区分でロケマスタに登録済みの場合、エラー
                    if (data.v.ErrMsg == null && CheckLocClassMenteFlag(data.v))
                    {
                        data.v.ErrMsg = string.Format(MessageResource.ERR_LOCATION_CD_MENET_DISABLE);
                    }

                    // 在庫テーブルチェック
                    if (data.v.ErrMsg == null && CheckStock(data.v))
                    {
                        data.v.ErrMsg = string.Format(MessageResource.ERR_STOCK_EXIST_UPLOAD);
                    }

                    // ロケーション区分
                    data.v.ErrMsg = data.v.ErrMsg ?? IsNullOrWhiteSpace(data.v.LocationClass, LocationResource.LocationClass);
                    if (data.v.ErrMsg == null && !MvcDbContext.Current.LocationClasses.Where(x => x.ShipperId == Common.Profile.User.ShipperId &&
                                                                                            x.LocationClass == data.v.LocationClass).Any())
                    {
                        data.v.ErrMsg = string.Format(MessagesResource.MasterNotExistsError, LocationResource.LocationClass);
                    }
                    else if (data.v.ErrMsg == null && MvcDbContext.Current.LocationClasses.Where(x => x.ShipperId == Common.Profile.User.ShipperId &&
                                                                                            x.LocationClass == data.v.LocationClass &&
                                                                                            x.MenteDisableFlag == 1).Any())
                    {
                        data.v.ErrMsg = string.Format(MessageResource.ERR_LOC_CLASS_MENTE_DISABLE);
                    }

                    // 格付ID
                    data.v.ErrMsg = data.v.ErrMsg ?? IsNullOrWhiteSpace(data.v.GradeId, LocationResource.GradeId);
                    // ロケーション区分と格付のチェック
                    if (data.v.ErrMsg == null && !MvcDbContext.Current.LocationClassGrade.Where(x => x.ShipperId == Common.Profile.User.ShipperId &&
                                                                            x.CenterId == Common.Profile.User.CenterId &&
                                                                            x.LocationClass == data.v.LocationClass &&
                                                                            x.GradeId == data.v.GradeId).Any())
                    {
                        data.v.ErrMsg = MessageResource.ERR_GRADE_NOT_EXIST;
                    }

                    // 引当優先順位
                    if (data.v.AllocPriority != "")
                    {
                        data.v.ErrMsg = data.v.ErrMsg ?? CheckNumber(data.v.AllocPriority, LocationResource.AllocPriority);
                    }

                    // ピッキンググループNo
                    data.v.ErrMsg = data.v.ErrMsg ?? IsNullOrWhiteSpace(data.v.PickingGroupNo, LocationResource.PickingGroupNo);
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckNumber(data.v.PickingGroupNo, LocationResource.PickingGroupNo);

                    // ファイル内重複チェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckPrimaryKey(report, data.v);

                    if (data.v.ErrMsg != null)
                    {
                        data.v.SetBaseInfoUpdate();
                        if (!string.IsNullOrWhiteSpace(centerId))
                        {
                            data.v.CenterId = centerId;
                        }
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
        public long InsertWwMasLocations(IEnumerable<ViewModels.Location.Report> report)
        {
            var dbContext = MvcDbContext.Current;
            var workId = GetWorkId();
            var no = 0;
            string masCaseClass = string.Empty;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                foreach (var u in report.Select((v, i) => new { v, i }))
                {
                    no = no + 1;
                    if (!string.IsNullOrWhiteSpace(u.v.LocationClass))
                    {
                        // マスタに存在
                        //if (MvcDbContext.Current.LocationClasses.Where(x => x.LocationClass == u.v.LocationClass).Any())
                        //{
                        //    LocationClasses masCaseClass1 = MvcDbContext.Current.LocationClasses.Find(u.v.LocationClass, Profile.User.ShipperId);
                        //    masCaseClass = masCaseClass1.CaseClass.ToString();
                        //}
                        LocationClasses masCaseClass1 = MvcDbContext.Current.LocationClasses.Where(x => x.LocationClass == u.v.LocationClass).FirstOrDefault();
                        if (masCaseClass1 != null)
                        {
                            masCaseClass = masCaseClass1.CaseClass.GetHashCode()
                                                                  .ToString();
                        }
                    }
                    var location = new Models.MasLocation
                    {
                        Seq = workId,
                        No = no,
                        CenterId = Profile.User.CenterId,
                        LocationCd = u.v.Locsec_1 + "-" + u.v.Locsec_2 + "-" + u.v.Locsec_3 + "-" + u.v.Locsec_4 + "-" + u.v.Locsec_5,
                        Locsec_1 = u.v.Locsec_1,
                        Locsec_2 = u.v.Locsec_2,
                        Locsec_3 = u.v.Locsec_3,
                        Locsec_4 = u.v.Locsec_4,
                        Locsec_5 = u.v.Locsec_5,
                        LocationClass = u.v.LocationClass,
                        CaseClass = masCaseClass,
                        GradeId = u.v.GradeId,
                        AllocPriority = string.IsNullOrEmpty(u.v.AllocPriority) ? "999999999" : u.v.AllocPriority,
                        PickingGroupNo = u.v.PickingGroupNo
                    };
                    location.SetBaseInfoInsert();
                    dbContext.MasLocations.Add(location);
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
        /// <param name="workId">workId</param>
        public void MergeLocations(long workId)
        {
            var result = _dbContext.MasLocations.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId && x.ErrMsg == null).ToList();
            var sql = @"
                MERGE INTO M_LOCATIONS T
                USING(
                    SELECT
                        :SHIPPER_ID         SHIPPER_ID,
                        :CENTER_ID          CENTER_ID,
                        :LOCATION_CD        LOCATION_CD,
                        :LOCSEC_1           LOCSEC_1,
                        :LOCSEC_2           LOCSEC_2,
                        :LOCSEC_3           LOCSEC_3,
                        :LOCSEC_4           LOCSEC_4,
                        :LOCSEC_5           LOCSEC_5,
                        :LOCATION_CLASS     LOCATION_CLASS,
                        :CASE_CLASS         CASE_CLASS,
                        :GRADE_ID           GRADE_ID,
                        :ALLOC_PRIORITY     ALLOC_PRIORITY,
                        :PICKING_GROUP_NO   PICKING_GROUP_NO
                    FROM
                       DUAL
                ) F
                ON(
                    F.SHIPPER_ID           = T.SHIPPER_ID
                    AND F.CENTER_ID        = T.CENTER_ID
                    AND F.LOCATION_CD      = T.LOCATION_CD
                )
                WHEN MATCHED THEN
                    UPDATE SET
                        T.UPDATE_DATE = SYSDATE,
                        T.UPDATE_USER_ID = :USER_ID,
                        T.UPDATE_PROGRAM_NAME = :PROGRAM_NAME,
                        T.UPDATE_COUNT = T.UPDATE_COUNT + 1,
                        T.LOCSEC_1     = F.LOCSEC_1,
                        T.LOCSEC_2     = F.LOCSEC_2,
                        T.LOCSEC_3     = F.LOCSEC_3,
                        T.LOCSEC_4     = F.LOCSEC_4,
                        T.LOCSEC_5     = F.LOCSEC_5,
                        T.LOCATION_CLASS     = F.LOCATION_CLASS,
                        T.CASE_CLASS         = F.CASE_CLASS,
                        T.GRADE_ID           = F.GRADE_ID,
                        T.ALLOC_PRIORITY     = F.ALLOC_PRIORITY,
                        T.PICKING_GROUP_NO   = F.PICKING_GROUP_NO
                WHEN NOT MATCHED THEN
                    INSERT (
                        T.MAKE_DATE,
                        T.MAKE_USER_ID,
                        T.MAKE_PROGRAM_NAME,
                        T.UPDATE_DATE,
                        T.UPDATE_USER_ID,
                        T.UPDATE_PROGRAM_NAME,
                        T.UPDATE_COUNT,
                        T.SHIPPER_ID,
                        T.CENTER_ID,
                        T.LOCATION_CD,
                        T.LOCSEC_1,
                        T.LOCSEC_2,
                        T.LOCSEC_3,
                        T.LOCSEC_4,
                        T.LOCSEC_5,
                        T.LOCATION_CLASS,
                        T.CASE_CLASS,
                        T.GRADE_ID,
                        T.ALLOC_PRIORITY,
                        T.PICKING_GROUP_NO
                    )VALUES (
                        SYSDATE,
                        :USER_ID,
                        :PROGRAM_NAME,
                        SYSDATE,
                        :USER_ID,
                        :PROGRAM_NAME,
                        0,
                        F.SHIPPER_ID,
                        F.CENTER_ID,
                        F.LOCATION_CD,
                        F.LOCSEC_1,
                        F.LOCSEC_2,
                        F.LOCSEC_3,
                        F.LOCSEC_4,
                        F.LOCSEC_5,
                        F.LOCATION_CLASS,
                        F.CASE_CLASS,
                        F.GRADE_ID,
                        F.ALLOC_PRIORITY,
                        F.PICKING_GROUP_NO
                    )
            ";
            var parameters = new DynamicParameters();

            foreach (var v in result)
            {
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", v.CenterId);
                parameters.Add(":LOCATION_CD", v.Locsec_1 + '-' + v.Locsec_2 + '-' + v.Locsec_3 + '-' + v.Locsec_4 + '-' + v.Locsec_5);
                parameters.Add(":LOCSEC_1", v.Locsec_1);
                parameters.Add(":LOCSEC_2", v.Locsec_2);
                parameters.Add(":LOCSEC_3", v.Locsec_3);
                parameters.Add(":LOCSEC_4", v.Locsec_4);
                parameters.Add(":LOCSEC_5", v.Locsec_5);
                parameters.Add(":LOCATION_CLASS", v.LocationClass);
                parameters.Add(":CASE_CLASS", v.CaseClass);
                parameters.Add(":GRADE_ID", v.GradeId);
                parameters.Add(":ALLOC_PRIORITY", v.AllocPriority);
                parameters.Add(":PICKING_GROUP_NO", v.PickingGroupNo);
                parameters.Add(":USER_ID", Common.Profile.User.UserId);
                parameters.Add(":PROGRAM_NAME", nameof(MenuResource.W_MAS_Location01));
                _dbContext.Database.Connection.Execute(sql, parameters);
            }
        }

        /// <summary>
        /// Get Location List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<IndexResultRow> GetReportErrList(UploadCondition conditions)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        WW.NO
                    ,   WW.CENTER_ID
                    ,   WW.LOCATION_CD
                    ,   WW.LOCSEC_1
                    ,   WW.LOCSEC_2
                    ,   WW.LOCSEC_3
                    ,   WW.LOCSEC_4
                    ,   WW.LOCSEC_5
                    ,   WW.LOCATION_CLASS
                    ,   LOCATION_CLASSES.LOCATION_NAME
                    ,   CASE_NAME.GEN_NAME AS CASE_NAME
                    ,   GRADE.GRADE_NAME
                    ,   WW.ALLOC_PRIORITY
                    ,   WW.PICKING_GROUP_NO
                    ,   WW.ERR_MSG
                FROM
                        WW_MAS_LOCATIONS WW
                LEFT JOIN
                        M_LOCATION_CLASSES LOCATION_CLASSES
                ON
                        WW.LOCATION_CLASS = LOCATION_CLASSES.LOCATION_CLASS
                    AND WW.SHIPPER_ID = LOCATION_CLASSES.SHIPPER_ID
                LEFT JOIN
                        M_GENERALS CASE_NAME
                ON
                        CASE_NAME.SHIPPER_ID = LOCATION_CLASSES.SHIPPER_ID
                    AND CASE_NAME.CENTER_ID = '@@@'
                    AND CASE_NAME.GEN_DIV_CD = 'CASE_CLASS'
                    AND CASE_NAME.REGISTER_DIVI_CD <> '0'
                    AND CASE_NAME.GEN_CD = TO_CHAR(LOCATION_CLASSES.CASE_CLASS)
                LEFT JOIN
                        M_LOCATION_CLASS_GRADE LOC_GRADE
                ON
                        LOC_GRADE.LOCATION_CLASS = WW.LOCATION_CLASS
                    AND LOC_GRADE.GRADE_ID = WW.GRADE_ID
                    AND LOC_GRADE.CENTER_ID = WW.CENTER_ID
                    AND LOC_GRADE.SHIPPER_ID = WW.SHIPPER_ID
                LEFT JOIN
                        M_GRADES GRADE
                ON
                        GRADE.SHIPPER_ID = LOC_GRADE.SHIPPER_ID
                    AND GRADE.GRADE_ID = LOC_GRADE.GRADE_ID
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
        /// ロケーションコード(エリア、棚列、棚番、段、間口)チェック
        /// </summary>
        /// <param name="locsec"></param>
        /// <param name="resource"></param>
        /// <returns></returns>

        private string CheckLocsec(string locsec, string resource)
        {
            string errMsg = null;
            //未入力チェック
            if (string.IsNullOrWhiteSpace(locsec))
            {
                errMsg = string.Format(MessagesResource.Required, resource);
            }
            //桁数チェック
            else if (locsec.Trim().Length != 3)
            {
                errMsg = string.Format(MessagesResource.LengthError, resource, "3");
            }
            //半角英数字チェック
            else if (Regex.Matches(locsec, "[a-zA-Z0-9]").Count != locsec.Trim().Length)
            {
                errMsg = string.Format(MessagesResource.Alphanumeric, resource);
            }
            return errMsg;
        }

        /// <summary>
        /// 未入力チェック
        /// </summary>
        /// <param name="locsec"></param>
        /// <param name="resource"></param>
        /// <returns></returns>

        private string IsNullOrWhiteSpace(string data, string resource)
        {
            string errMsg = null;
            if (string.IsNullOrWhiteSpace(data))
            {
                errMsg = string.Format(MessagesResource.Required, resource);
            }
            return errMsg;
        }

        /// <summary>
        /// 引当優先順位・ピッキンググループNoチェック
        /// </summary>
        /// <param name="locsec"></param>
        /// <param name="resource"></param>
        /// <returns></returns>

        private string CheckNumber(string data, string resource)
        {
            string errMsg = null;
            // 整数で入力してください
            if (!new ExcelReader<ViewModels.Location.Report>().CheckNumber(data, true))
            {
                errMsg = string.Format(MessagesResource.NotIntError, resource);
            }
            // 0以上999999999以下の値で入力してください
            else if ((int.Parse(data) < 0 || int.Parse(data) > 999999999))
            {
                errMsg = string.Format(MessagesResource.Range, resource, "0", "999999999");
            }
            return errMsg;
        }

        /// <summary>
        /// 在庫データチェック
        /// </summary>
        /// <param name="masLocation">ワーク</param>
        /// <returns>在庫あり かつ ロケーション区分と格付変更あり：true</returns>
        private bool CheckStock(MasLocation masLocation)
        {
            var shipperId = Common.Profile.User.ShipperId;
            var centerId = Common.Profile.User.CenterId;
            var locationCd = masLocation.Locsec_1 + '-' + masLocation.Locsec_2 + '-' + masLocation.Locsec_3 + '-' + masLocation.Locsec_4 + '-' + masLocation.Locsec_5;

            // 在庫テーブルにデータがあるかチェック
            var stockExists = new Location().CheckStock(shipperId, centerId, locationCd);

            // 在庫ありの場合、ロケーション区分と格付が変更されているかチェック
            if (stockExists)
            {
                return !MvcDbContext.Current.Locations.Where(x => x.ShipperId == shipperId
                                                                & x.CenterId == centerId
                                                                & x.LocationCd == locationCd
                                                                & x.LocationClass == masLocation.LocationClass
                                                                & x.GradeId == masLocation.GradeId).Any();
            }

            return stockExists;
        }

        /// <summary>
        /// ロケーションコードが、ロケマスタメンテ不可のロケ区分でロケマスタに登録済みの場合、エラー
        /// </summary>
        /// <param name="masLocation">ワーク</param>
        /// <returns></returns>
        private bool CheckLocClassMenteFlag(MasLocation masLocation)
        {
            var shipperId = Common.Profile.User.ShipperId;
            var centerId = Common.Profile.User.CenterId;
            var locationCd = masLocation.Locsec_1 + '-' + masLocation.Locsec_2 + '-' + masLocation.Locsec_3 + '-' + masLocation.Locsec_4 + '-' + masLocation.Locsec_5;

            // ロケーションマスタにデータがあるかチェック
            var targetLocation = new Location().GetTargetById(locationCd, centerId, shipperId);

            if (targetLocation != null)
            {
                // 登録済みのロケ区分がメンテナンス不可の場合 true
                return MvcDbContext.Current.LocationClasses.Where(x => x.ShipperId == shipperId
                                                                & x.LocationClass == targetLocation.LocationClass
                                                                & x.MenteDisableFlag == 1).Any();
            }

            return false;
        }

        /// <summary>
        /// ロケーションラベル用データ
        /// </summary>
        /// <param name="search"></param>
        public IEnumerable<LocationLabelReportRowForCsv> GetResultRowList(LocationSearchCondition condition)
        {
            List<LocationLabelReportRowForCsv> ret = new List<LocationLabelReportRowForCsv>();
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT
                            ML.LOCATION_CD AS LOCATION_CD
                    FROM
                            M_LOCATIONS ML
                    WHERE
                            ML.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND ML.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            if (!string.IsNullOrEmpty(condition.LocationClass))
            {
                query.Append(" AND ML.LOCATION_CLASS = :LOCATION_CLASS ");
                parameters.Add(":LOCATION_CLASS", condition.LocationClass);
            }

            if (!string.IsNullOrEmpty(condition.Locsec1))
            {
                query.Append(" AND ML.LOCSEC_1 = :LOCSEC_1 ");
                parameters.Add(":LOCSEC_1", condition.Locsec1);
            }

            if (!string.IsNullOrEmpty(condition.Locsec2))
            {
                query.Append(" AND ML.LOCSEC_2 = :LOCSEC_2 ");
                parameters.Add(":LOCSEC_2", condition.Locsec2);
            }

            if (!string.IsNullOrEmpty(condition.Locsec3))
            {
                query.Append(" AND ML.LOCSEC_3 = :LOCSEC_3 ");
                parameters.Add(":LOCSEC_3", condition.Locsec3);
            }

            if (!string.IsNullOrEmpty(condition.Locsec4))
            {
                query.Append(" AND ML.LOCSEC_4 = :LOCSEC_4 ");
                parameters.Add(":LOCSEC_4", condition.Locsec4);
            }

            if (!string.IsNullOrEmpty(condition.Locsec5))
            {
                query.Append(" AND ML.LOCSEC_5 LIKE :LOCSEC_5 ");
                parameters.Add(":LOCSEC_5", "%" + condition.Locsec5 + "%");
            }

            // Sort function
            switch (condition.SortKey)
            {
                case LocationSortKey.LocationCd:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CD DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CD ASC");
                            break;
                    }

                    break;

                case LocationSortKey.LocationClass:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CLASS DESC, ML.LOCATION_CD DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CLASS ASC, ML.LOCATION_CD ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CD DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ML.LOCATION_CD ASC");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            var data = MvcDbContext.Current.Database.Connection.Query<LocationLabelReportRowForCsv>(query.ToString(), parameters).ToList();

            foreach (var d in data)
            {
                var locationCdFront = string.Empty;
                var locationCdRear = string.Empty;
                //ハイフンがない場合はロケコード前方に格納する
                if (d.LocationCd.IndexOf("-") == -1)
                {
                    locationCdFront = d.LocationCd;
                    locationCdRear = "";
                }
                else
                {
                    var arrLocationCd = d.LocationCd.ToString().Split('-');
                    string[] arrLocationCdRear = new string[3];
                    //ロケコードの後ろの3部分を配列に格納
                    Array.Copy(arrLocationCd, arrLocationCd.Length - 3, arrLocationCdRear, 0, 3);
                    //ハイフン区切りに戻す
                    locationCdRear = arrLocationCdRear[0] + "-" + arrLocationCdRear[1] + "-" + arrLocationCdRear[2];
                    //１文字目から後半が始まる前まで抽出
                    locationCdFront = d.LocationCd.ToString().Substring(0, d.LocationCd.ToString().Length - locationCdRear.Length);
                }

                ret.Add(new LocationLabelReportRowForCsv()
                {
                    LocationCdFront = locationCdFront,
                    LocationCdRear = locationCdRear,
                    LocationCd = d.LocationCd
                });
            }

            return ret;
        }

        /// <summary>
        /// ファイル内重複チェック
        /// </summary>
        /// <param name="report"></param>
        /// <param name="data"></param>
        /// <returns></returns>

        private string CheckPrimaryKey(List<MasLocation> report, MasLocation data)
        {
            if (report.Where(x => x.CenterId == data.CenterId
                               && x.LocationCd == data.LocationCd).Count() > 1)
            {
                return string.Format(MessageResource.ErrorUploadUniqueConstraintViolated, LocationResource.LocationCd);
            }

            return null;
        }
    }
}