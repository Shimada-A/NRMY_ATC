namespace Wms.Areas.Master.Models
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.General;
    using Wms.Common;
    using Wms.Models;

    public partial class General
    {
        /// <summary>
        /// Get General List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<GeneralList> GetData(GeneralSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MG.UPDATE_COUNT
                      ,MG.SHIPPER_ID
                      ,MG.CENTER_ID
                      ,DECODE(MG.CENTER_ID, '@@@', 'センター共通', MW.CENTER_NAME1) CENTER_NAME
                      ,MG.REGISTER_DIVI_CD
                      ,MG.GEN_DIV_CD
                      ,(SELECT GEN_NAME
                          FROM M_GENERALS
                         WHERE SHIPPER_ID = MG.SHIPPER_ID
                           AND CENTER_ID = MG.CENTER_ID
                           AND GEN_DIV_CD = MG.GEN_DIV_CD
                           AND REGISTER_DIVI_CD = '0') GEN_DIV_NAME
                      ,MG.GEN_CD
                      ,MG.GEN_NAME
                      ,MG.ORDER_NO
                      ,MG.ROWID RID
                  FROM M_GENERALS MG
                  LEFT JOIN
                       M_CENTERS MW
                    ON MW.SHIPPER_ID = MG.SHIPPER_ID
                   AND MW.CENTER_ID = MG.CENTER_ID
                 WHERE MG.SHIPPER_ID = :SHIPPER_ID
                   AND MG.REGISTER_DIVI_CD = '1'
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND MG.CENTER_ID = :CENTER_ID");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            // ログインユーザーの権限レベルが、閲覧のみ可能なユーザーの場合、ログインユーザーのセンター以外の汎用コードは一覧に表示しない
            if (string.IsNullOrEmpty(condition.CenterId) && CheckUsableClass())
            {
                query.Append(" AND  ( MG.CENTER_ID = :CENTER_ID ");
                query.Append("     OR MG.CENTER_ID = '@@@' ) ");
                parameters.Add(":CENTER_ID", Profile.User.CenterId);
            }

            if (!string.IsNullOrEmpty(condition.GenDivCd))
            {
                query.Append(" AND MG.GEN_DIV_CD = :GEN_DIV_CD");
                parameters.Add(":GEN_DIV_CD", condition.GenDivCd);
            }

            query.Append(@" AND EXISTS (
                                    SELECT
                                        CENTER_ID,GEN_DIV_CD
                                    FROM
                                        M_GENERALS
                                    WHERE
                                        REGISTER_DIVI_CD = '0'
                                    AND MAINTENANCE_FLAG = 1
                                    AND MG.CENTER_ID = CENTER_ID
                                    AND MG.GEN_DIV_CD = GEN_DIV_CD
                            ) 
                            ORDER BY MG.CENTER_ID
                                    ,GEN_DIV_CD
                                    ,ORDER_NO
            ");

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<GeneralList>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var general = MvcDbContext.Current.Database.Connection.Query<GeneralList>(query.ToString(), parameters).ToList();

            // Excute paging
            return new StaticPagedList<GeneralList>(general, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Get ROWID
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public List<string> GetRowId(GeneralSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@" 
                    SELECT 
                        MG.ROWID 
                    FROM 
                        M_GENERALS MG  
                    WHERE 
                        MG.SHIPPER_ID = :SHIPPER_ID 
                    AND MG.REGISTER_DIVI_CD = '1'
            ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND MG.CENTER_ID = :CENTER_ID");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            // ログインユーザーの権限レベルが、閲覧のみ可能なユーザーの場合、ログインユーザーのセンター以外の汎用コードは一覧に表示しない
            if (string.IsNullOrEmpty(condition.CenterId) && CheckUsableClass())
            {
                query.Append(" AND  ( MG.CENTER_ID = :CENTER_ID ");
                query.Append("     OR MG.CENTER_ID = '@@@' ) ");
                parameters.Add(":CENTER_ID", Profile.User.CenterId);
            }

            if (!string.IsNullOrEmpty(condition.GenDivCd))
            {
                query.Append(" AND MG.GEN_DIV_CD = :GEN_DIV_CD");
                parameters.Add(":GEN_DIV_CD", condition.GenDivCd);
            }

            query.Append(@" AND EXISTS (
                                    SELECT
                                        CENTER_ID,GEN_DIV_CD
                                    FROM
                                        M_GENERALS
                                    WHERE
                                        REGISTER_DIVI_CD = '0'
                                    AND MAINTENANCE_FLAG = 1
                                    AND MG.CENTER_ID = CENTER_ID
                                    AND MG.GEN_DIV_CD = GEN_DIV_CD
                            )
            ");

            return MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).ToList();
        }

        /// <summary>
        /// Get General By Id
        /// </summary>
        /// <param name="registerDiviCd">RegisterDiviCd</param>
        /// <param name="genDivCd">GenDivCd</param>
        /// <param name="genCd">GenCd</param>
        /// <param name="centerId">CenterId</param>
        /// <param name="shipperId">ShipperId</param>
        /// <returns>Country</returns>
        public General GetTargetById(string registerDiviCd, string genDivCd, string genCd, string centerId, string shipperId)
        {
            return MvcDbContext.Current.Generals.Find(registerDiviCd, genDivCd, genCd, centerId, shipperId);
        }

        /// <summary>
        /// Get General By Id
        /// </summary>
        /// <param name="rowid">rowid</param>
        /// <returns>General</returns>
        public General GetTargetById(string rowid)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@" SELECT * FROM M_GENERALS MG  WHERE MG.ROWID = :ROWID1");

            parameters.Add(":ROWID1", rowid);
            //return MvcDbContext.Current.Generals.Find(registerDiviCd, genDivCd, genCd, centerId, shipperId);
            return MvcDbContext.Current.Database.Connection.Query<General>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        ///  Insert General
        /// </summary>
        /// <param name="general">General information</param>
        /// <returns></returns>
        public bool InsertGeneral(General general)
        {
            general.SetBaseInfoInsert();

            MvcDbContext.Current.Generals.Add(general);

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
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
        /// Update General
        /// </summary>
        /// <param name="general"></param>
        /// <returns>Update status</returns>
        public bool UpdateGeneral(General general)
        {
            var dbContext = MvcDbContext.Current;

            var updatedGeneral =
                  MvcDbContext.Current.Generals
                  .Where(m => m.ShipperId == general.ShipperId &&
                              m.CenterId == general.CenterId &&
                              m.RegisterDiviCd == general.RegisterDiviCd &&
                              m.GenDivCd == general.GenDivCd &&
                              m.GenCd == general.GenCd &&
                              m.UpdateCount == general.UpdateCount)
                  .SingleOrDefault();

            if (updatedGeneral == null)
            {
                return false;
            }

            updatedGeneral.SetBaseInfoUpdate();

            // updatedGeneral.CenterId = general.CenterId;
            updatedGeneral.GenName = general.GenName;
            updatedGeneral.OrderNo = general.OrderNo;

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
        /// Delete General
        /// </summary>
        /// <param name="general"></param>
        /// <returns>Update status</returns>
        public bool DeleteGeneral(List<string> rowids)
        {
            var parameters = new DynamicParameters();
            var deleteGeneral = new StringBuilder();

            // 共通ヘッダ
            deleteGeneral.Append(@"
                DELETE FROM
                       M_GENERALS MG
                 WHERE
                       MG.ROWID = :ROWID1
            ");

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var rid in rowids)
                {
                    // KEY
                    parameters.Add(":ROWID1", rid);
                    MvcDbContext.Current.Database.Connection.Execute(deleteGeneral.ToString(), parameters);
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
        /// センターセレクトボックスデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectLocListItems()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT DISTINCT
                       MG.CENTER_ID AS VALUE
                      ,N'センター共通' AS TEXT
                  FROM M_GENERALS MG
                 WHERE MG.SHIPPER_ID = :SHIPPER_ID
                   AND MG.CENTER_ID = '@@@'
                 UNION ALL
                SELECT DISTINCT
                       MW.CENTER_ID AS VALUE
                      ,MW.CENTER_NAME1 AS TEXT
                  FROM M_CENTERS MW
                 WHERE MW.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            var loc = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters).ToList();

            // Excute paging
            return loc;
        }

        /// <summary>
        /// 汎用分類コードセレクトボックスデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectGenListItems()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT DISTINCT
                        MG.GEN_DIV_CD AS VALUE
                    ,   MG.GEN_NAME AS TEXT
                FROM
                        M_GENERALS MG
                WHERE
                        MG.SHIPPER_ID = :SHIPPER_ID
                    AND MG.REGISTER_DIVI_CD = '0'
                    AND MG.MAINTENANCE_FLAG = 1
                ORDER BY
                        MG.GEN_DIV_CD
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            var gen = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters).ToList();

            // Excute paging
            return gen;
        }

        /// <summary>
        /// センター取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCenters(string genDivCd)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.AppendLine(@"
                SELECT MG.CENTER_ID VALUE
                      ,CASE WHEN MG.CENTER_ID = '@@@' THEN '"+ GeneralResource.CommonCenter+ @"' ELSE TO_CHAR(MAX(MC.CENTER_NAME1)) END TEXT
                  FROM M_GENERALS MG
                  LEFT JOIN M_CENTERS MC
                    ON MG.SHIPPER_ID = MC.SHIPPER_ID
                   AND MG.CENTER_ID = MC.CENTER_ID
                  WHERE MG.SHIPPER_ID = :SHIPPER_ID
                    AND MG.GEN_DIV_CD = :GEN_DIV_CD
                  GROUP BY MG.CENTER_ID,MG.SHIPPER_ID
                  ORDER BY MG.CENTER_ID ASC
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":GEN_DIV_CD", genDivCd);

            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }

        /// <summary>
        /// ログインユーザーの権限レベルが、閲覧のみ可能かチェック
        /// </summary>
        /// <returns>閲覧のみ可能な場合：true</returns>
        private bool CheckUsableClass()
        {
            return MvcDbContext.Current.UserPrograms.Where(s => s.ShipperId == Profile.User.ShipperId
                                                             && s.ProgramId == "W_MAS_General01"
                                                             && (PermissionLevelClasses)s.PermissionLevel == Profile.User.PermissionLevel
                                                             && s.UsableClass == UsableClasses.UsableOnlyBrowse).Any();

        }
    }
}