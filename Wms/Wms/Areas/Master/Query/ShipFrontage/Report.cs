namespace Wms.Areas.Master.Query.ShipFrontage
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Share.Common.Resources;
    using Share.Reports.Import;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.ShipFrontage;
    using Wms.Common;
    using Wms.Common.Resources;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;
    using static Wms.Areas.Master.ViewModels.ShipFrontage.ShipFrontageSearchCondition;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<ViewModels.ShipFrontage.Report> ShipFrontageListing(ShipFrontageSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            //一覧表示のSQLを取得
            ShipFrontage.GetQuery(condition, ref query, ref parameters);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ViewModels.ShipFrontage.Report>(query.ToString(), parameters);
        }

        /// <summary>
        /// テンプレートExcelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<ViewModels.ShipFrontage.ReportTemp> ShipFrontageListingTemp(ShipFrontageSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       '' AS BRAND_ID
                      ,'' AS BRAND_NAME
                      ,'' AS LANE_NO
                      ,'' AS FRONTAGE_NO
                      ,'' AS STORE_ID
                      ,'' AS STORE_NAME
                  FROM DUAL
            ");

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ViewModels.ShipFrontage.ReportTemp>(query.ToString());
        }

        /// <summary>
        /// アップロードされたデータのチェック
        /// </summary>
        /// <param name="workId">workId</param>
        /// <param name="report">report</param>
        /// <returns></returns>
        public bool UploadCheck(long workId, List<MasShipFrontage> report)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var data in report.Select((v, i) => new { v, i }))
                {
                    //ブランドID 
                    data.v.ErrMsg = data.v.ErrMsg ?? IsNullOrWhiteSpace(data.v.BrandId, ShipFrontageResource.BrandId);

                    // レーンNo
                    data.v.ErrMsg = data.v.ErrMsg ?? IsNullOrWhiteSpace(data.v.LaneNo, ShipFrontageResource.LaneNo);
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckNumber(data.v.LaneNo, ShipFrontageResource.LaneNo, 0, 99);

                    // 間口No
                    data.v.ErrMsg = data.v.ErrMsg ?? IsNullOrWhiteSpace(data.v.FrontageNo, ShipFrontageResource.FrontageNo);
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckNumber(data.v.FrontageNo, ShipFrontageResource.FrontageNo, 0, 999);

                    // 出荷先ID
                    data.v.ErrMsg = data.v.ErrMsg ?? IsNullOrWhiteSpace(data.v.StoreId, ShipFrontageResource.StoreId);

                    //ブランドIDマスタ存在チェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckBrandId( data.v);

                    // 出荷先IDマスタ存在チェック
                    data.v.ErrMsg = data.v.ErrMsg ?? CheckStoreId(report, data.v);

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
        public long InsertWwMasShipFrontages(IEnumerable<ViewModels.ShipFrontage.Report> report)
        {
            var dbContext = MvcDbContext.Current;
            var workId = GetWorkId();
            var no = 0;
            int? masCaseClass = null;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                foreach (var u in report.Select((v, i) => new { v, i }))
                {
                    no = no + 1;

                    var shipFrontage = new Models.MasShipFrontage
                    {
                        Seq = workId,
                        No = no,
                        CenterId = Profile.User.CenterId,
                        BrandId = u.v.BrandId,
                        BrandName= u.v.BrandName,
                        LaneNo = u.v.LaneNo,
                        FrontageNo = u.v.FrontageNo,
                        StoreId = u.v.StoreId
                    };
                    shipFrontage.SetBaseInfoInsert();
                    dbContext.MasShipFrontages.Add(shipFrontage);
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
        public void MergeShipFrontages(long workId)
        {
            var result = _dbContext.MasShipFrontages.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId && x.ErrMsg == null).ToList();
            var sql = @"
                MERGE INTO M_SHIP_FRONTAGE T
                USING(
                    SELECT
                        :SHIPPER_ID         SHIPPER_ID,
                        :CENTER_ID          CENTER_ID,
                        :LANE_NO            LANE_NO,
                        :FRONTAGE_NO        FRONTAGE_NO,
                        :STORE_ID           STORE_ID,
                        :BRAND_ID           BRAND_ID
                    FROM
                       DUAL
                ) F
                ON(
                    F.SHIPPER_ID           = T.SHIPPER_ID
                    AND F.CENTER_ID        = T.CENTER_ID
                    AND F.LANE_NO          = T.LANE_NO
                    AND F.FRONTAGE_NO      = T.FRONTAGE_NO
                    AND F.BRAND_ID         = T.BRAND_ID
                )
                WHEN MATCHED THEN
                    UPDATE SET
                        T.UPDATE_DATE = SYSDATE,
                        T.UPDATE_USER_ID = :USER_ID,
                        T.UPDATE_PROGRAM_NAME = :PROGRAM_NAME,
                        T.UPDATE_COUNT = T.UPDATE_COUNT + 1,
                        T.STORE_ID     = F.STORE_ID
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
                        T.LANE_NO,
                        T.FRONTAGE_NO,
                        T.STORE_ID,
                        T.BRAND_ID
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
                        F.LANE_NO,
                        F.FRONTAGE_NO,
                        F.STORE_ID,
                        F.BRAND_ID
                    )
            ";
            var parameters = new DynamicParameters();

            foreach (var v in result)
            {
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":CENTER_ID", v.CenterId);
                parameters.Add(":LANE_NO", v.LaneNo);
                parameters.Add(":FRONTAGE_NO", v.FrontageNo);
                parameters.Add(":STORE_ID", v.StoreId);
                parameters.Add(":BRAND_ID", v.BrandId);
                parameters.Add(":USER_ID", Common.Profile.User.UserId);
                parameters.Add(":PROGRAM_NAME", nameof(MenuResource.W_MAS_ShipFrontage01));
                _dbContext.Database.Connection.Execute(sql, parameters);
            }
        }

        /// <summary>
        /// Get ShipFrontage List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<IndexResultRow> GetReportErrList(UploadCondition conditions)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       WMSF.CENTER_ID
                      ,WMSF.BRAND_ID
                      ,WMSF.BRAND_NAME
                      ,WMSF.LANE_NO
                      ,WMSF.FRONTAGE_NO
                      ,WMSF.STORE_ID
                      ,VSTS.SHIP_TO_STORE_NAME1 AS STORE_NAME
                      ,WMSF.ERR_MSG
                      ,WMSF.SHIPPER_ID
                      ,WMSF.SEQ
                      ,WMSF.NO
                      ,WMSF.UPDATE_COUNT
                  FROM WW_MAS_SHIP_FRONTAGE WMSF
                  LEFT JOIN V_SHIP_TO_STORES VSTS
                    ON WMSF.STORE_ID = VSTS.SHIP_TO_STORE_ID
                   AND WMSF.SHIPPER_ID = VSTS.SHIPPER_ID
                 WHERE WMSF.SHIPPER_ID = :SHIPPER_ID
                   AND WMSF.SEQ = :SEQ
                 ORDER BY WMSF.NO ASC
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
        /// Delete ShipFrontage
        /// </summary>
        /// <param name="lstShipFrontage">List record is deleted</param>
        /// <returns>List of rows error </returns>
        public bool UploadDelete(IList<SelectedShipFrontageUploadViewModel> lstShipFrontage)
        {
            var shipFrontagesSelected =
                 lstShipFrontage
                 .Where(x => x.IsCheck == true)
                 .ToList();

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in shipFrontagesSelected)
                {
                    var updatedShipFrontage =
                   MvcDbContext.Current.MasShipFrontages
                   .Where(m => m.ShipperId == u.ShipperId && m.Seq == u.Seq && m.No == u.No && m.UpdateCount == u.UpdateCount)
                   .SingleOrDefault();
                    if (updatedShipFrontage == null)
                    {
                        return false;
                    }

                    MvcDbContext.Current.MasShipFrontages.Remove(updatedShipFrontage);

                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// 未入力チェック
        /// </summary>
        /// <param name="data"></param>
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
        /// 数値チェック
        /// </summary>
        /// <param name="data"></param>
        /// <param name="resource"></param>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        /// <returns></returns>

        private string CheckNumber(string data, string resource, int Min, int Max)
        {
            string errMsg = null;
            // 整数で入力してください
            if (!new ExcelReader<ViewModels.ShipFrontage.Report>().CheckNumber(data, true))
            {
                errMsg = string.Format(MessagesResource.NotIntError, resource);
            }
            // 0以上999999999以下の値で入力してください
            else if ((int.Parse(data) < Min || int.Parse(data) > Max))
            {
                errMsg = string.Format(MessagesResource.Range, resource, Min.ToString(), Max.ToString());
            }
            return errMsg;
        }

        /// <summary>
        /// 出荷先ID マスタチェック
        /// </summary>
        /// <param name="report"></param>
        /// <param name="data"></param>
        /// <returns></returns>

        private string CheckStoreId(List<MasShipFrontage> report, MasShipFrontage data)
        {
            int targetLaneNo = int.Parse(data.LaneNo);
            int targetFrontageNo = int.Parse(data.FrontageNo);

            //出荷先ID 店舗マスタ 登録されていない場合エラー
            if (!MvcDbContext.Current.ShipToStores.Where(x => x.ShipperId == data.ShipperId &&
                                                              x.ShipToStoreId == data.StoreId).Any())
            {
                return string.Format(MessagesResource.MasterNotExistsError, ShipFrontageResource.StoreId);
            }

            //出荷先ID 出荷レーン間口マスタ 登録済みの場合エラー
            if (MvcDbContext.Current.ShipFrontages.Where(x => x.ShipperId == data.ShipperId &&
                                                              x.CenterId == data.CenterId &&
                                                              x.LaneNo + '-' + x.FrontageNo != targetLaneNo + '-' + targetFrontageNo &&
                                                              x.BrandId == data.BrandId &&
                                                              x.StoreId == data.StoreId).Any())
            {
                return ShipFrontageResource.ErrorStoreIdAlreadySaved;
            }

            return null;
        }


        /// <summary>
        /// ブランドID マスタチェック
        /// </summary>
        /// <param name="report"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string CheckBrandId(MasShipFrontage data)
        {

            //ブランドID ブランドマスタ 登録されてない場合エラー
            if (!MvcDbContext.Current.Brands.Where(x => x.ShipperId == data.ShipperId &&
                                                       x.BrandId == data.BrandId).Any())
            {
                return string.Format(MessagesResource.MasterNotExistsError, ShipFrontageResource.BrandId);
            }

            return null;
        }

        /// <summary>
        /// ファイル内重複チェック
        /// </summary>
        /// <param name="report"></param>
        /// <param name="data"></param>
        /// <returns></returns>

        private string CheckPrimaryKey(List<MasShipFrontage> report, MasShipFrontage data)
        {
            // レーン間口
            if(report.Where(x => x.LaneNo == data.LaneNo
                              && x.FrontageNo == data.FrontageNo
                              && x.BrandId == data.BrandId).Count() > 1)
            {
                return string.Format(MessageResource.ErrorUploadUniqueConstraintViolated, ShipFrontageResource.BrandId + "－" + ShipFrontageResource.LaneNo +  "－" + ShipFrontageResource.FrontageNo);
            }

            // 出荷先ID
            if (report.Where(x => x.StoreId == data.StoreId &&
                                  x.LaneNo + "-" + x.FrontageNo != data.LaneNo + "-" + data.FrontageNo &&
                                  x.BrandId == data.BrandId).Any())
            {
                return string.Format(MessageResource.ErrorUploadUniqueConstraintViolated, ShipFrontageResource.BrandId + "－" + ShipFrontageResource.StoreId);
            }

            return null;
        }

    }
}