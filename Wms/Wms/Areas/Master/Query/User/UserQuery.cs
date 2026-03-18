namespace Wms.Areas.Master.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Common;
    using Wms.Areas.Master.ViewModels.User;
    using Wms.Common;
    using Wms.Models;
    using Wms.ViewModels;
    using static Wms.Areas.Master.ViewModels.User.UserSearchCondition;

    public partial class User
    {
        /// <summary>
        /// Get User List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<UserList> GetData(UserSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MU.UPDATE_COUNT
                      ,MU.SHIPPER_ID
                      ,MU.USER_ID
                      ,MU.USER_NAME
                      ,MU.PERMISSION_LEVEL
                      ,MG.GEN_NAME PERMISSION_LEVEL_NAME
                      ,MU.CENTER_ID
                      ,MW.CENTER_NAME1 || MW.CENTER_NAME2 CENTER_NAME
                      ,MU.ROWID RID
                  FROM M_USERS MU
                  LEFT JOIN
                       M_CENTERS MW
                    ON MU.SHIPPER_ID = MW.SHIPPER_ID
                   AND MU.CENTER_ID  = MW.CENTER_ID
                  LEFT JOIN
                       M_GENERALS MG
                    ON MG.SHIPPER_ID = MU.SHIPPER_ID
                   AND MG.GEN_CD     = MU.PERMISSION_LEVEL
                   AND MG.GEN_DIV_CD = 'PERMISSION_LEVEL'
                   AND MG.REGISTER_DIVI_CD = '1'
                 WHERE MU.SHIPPER_ID = :SHIPPER_ID
                   AND MU.DELETE_FLAG = 0
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND MU.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            if (!string.IsNullOrEmpty(condition.UserId))
            {
                query.Append(" AND MU.USER_ID LIKE :USER_ID ");
                parameters.Add(":USER_ID", condition.UserId + "%");
            }

            if (!string.IsNullOrEmpty(condition.UserName))
            {
                query.Append(" AND MU.USER_NAME LIKE :USER_NAME ");
                parameters.Add(":USER_NAME", condition.UserName + "%");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<UserList>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case UserSortKey.UserName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MU.USER_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MU.USER_NAME ASC");
                            break;
                    }

                    break;

                case UserSortKey.CenterIdUserId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MU.CENTER_ID DESC , MU.USER_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MU.CENTER_ID ASC , MU.USER_ID ASC");
                            break;
                    }

                    break;

                case UserSortKey.PermissionLevelUserId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MU.PERMISSION_LEVEL DESC , MU.USER_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MU.PERMISSION_LEVEL ASC , MU.USER_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MU.USER_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MU.USER_ID ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var user = MvcDbContext.Current.Database.Connection.Query<UserList>(query.ToString(), parameters).ToList();

            // Excute paging
            return new StaticPagedList<UserList>(user, condition.Page, condition.PageSize, totalCount);
        }
        public List<string> GetUserId(UserSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@" 
                SELECT 
                        MU.USER_ID 
                FROM 
                        M_USERS MU  
                WHERE 
                        MU.SHIPPER_ID = :SHIPPER_ID
                    AND MU.DELETE_FLAG = 0
            ");

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND MU.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            if (!string.IsNullOrEmpty(condition.UserId))
            {
                query.Append(" AND MU.USER_ID LIKE :USER_ID ");
                parameters.Add(":USER_ID", condition.UserId + "%");
            }

            if (!string.IsNullOrEmpty(condition.UserName))
            {
                query.Append(" AND MU.USER_NAME LIKE :USER_NAME ");
                parameters.Add(":USER_NAME", condition.UserName + "%");
            }

            return MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).ToList();

        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        /// 詳細内容の取得
        public User GetTargetById(string userId, string shipperId)
        {
            return MvcDbContext.Current.User.Find(userId, shipperId);
        }


        /// <summary>
        ///  新規登録
        /// </summary>
        /// <param name="user">User information</param>
        /// <param name="password">password</param>
        /// <param name="passwordConfirm">passwordConfirm</param>
        /// <returns></returns>
        public bool InsertUser(User user, string password, string passwordConfirm)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                var deleteTarget = MvcDbContext.Current.User
                    .Where(m => m.ShipperId == Profile.User.ShipperId && m.UserId == user.UserId && m.DeleteFlag == 1)
                    .SingleOrDefault();

                if (deleteTarget != null)
                {
                    MvcDbContext.Current.User.Remove(deleteTarget);
                }

                user.SetBaseInfoInsert();
                //var pw = HashUtil.GenerateSaltedHash(password, HashUtil.GenerateSalt());
                user.PasswordHash = HashUtil.ComputeHashMd5(password);
                user.PasswordHash1 = HashUtil.ComputeHashMd5(password);
                user.PasswordUpdateUserId = user.UpdateUserId;
                user.PasswordUpdateDate = DateTime.Now.Date;
                user.PasswordMistypeCount = 0;
                user.UserLapse = 0;

                MvcDbContext.Current.User.Add(user);

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
        /// データ区分チェック
        /// </summary>
        /// <returns></returns>
        public string DataClassCheck(List<string> list)
        {

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                    *
                FROM
                        M_USERS MU
                WHERE
                        MU.SHIPPER_ID      = :SHIPPER_ID
                    AND MU.USER_ID         IN :USER_ID
                    AND MU.DATE_MAKE_CLASS = 1
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":USER_ID", list);

            // Fill data to memory
            var result = MvcDbContext.Current.Database.Connection.Query<UserList>(query.ToString(), parameters).FirstOrDefault();
            return result == null ? null : result.UserId;
        }

        /// <summary>
        /// Update User 更新内容の取得
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="passwordConfirm"></param>
        /// <returns>Update status</returns>
        public bool UpdateUser(User user, string password, string passwordConfirm ,UserList userList )
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {

                //排他チェック処理
                var updatedUser =
                  MvcDbContext.Current.User
                  .Where(m => m.ShipperId == user.ShipperId && m.UserId == user.UserId && m.UpdateCount == user.UpdateCount)
                  .SingleOrDefault();

                // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                if (updatedUser == null)
                {
                    return false;
                }

                updatedUser.SetBaseInfoUpdate();

                updatedUser.CenterId = user.CenterId;
                updatedUser.UserName = user.UserName;
                updatedUser.PermissionLevel = user.PermissionLevel;

                if (!string.IsNullOrWhiteSpace(password))
                {
                    updatedUser.PasswordUpdateDate = DateTime.Now.Date;
                    updatedUser.PasswordUpdateUserId = updatedUser.UpdateUserId;

                    //var pw = HashUtil.GenerateSaltedHash(password, HashUtil.GenerateSalt());
                    updatedUser.PasswordHash1 = HashUtil.ComputeHashMd5(password);
                    updatedUser.PasswordHash = HashUtil. ComputeHashMd5(password);
                    if (updatedUser.UserLapse == 9)
                    {
                        updatedUser.PasswordMistypeCount = 0;
                    }
                }

                if (updatedUser.UserLapse == 9)
                {
                    updatedUser.UserLapse = 0;
                }

                try
                {
                    MvcDbContext.Current.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    trans.Rollback();
                    throw;
                }
                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Update status</returns>
        public bool DeleteUser(List<string> deleteCheckList)
        {
            var dbContext = MvcDbContext.Current;

            var deleteList = dbContext.User.Where(w => deleteCheckList.Contains(w.UserId)).ToList();

            foreach (var targetDelete in deleteList)
            {
                //排他チェック処理
                var deleteUser = MvcDbContext.Current.User
                  .Where(m => m.ShipperId == targetDelete.ShipperId && m.UserId == targetDelete.UserId && m.UpdateCount == targetDelete.UpdateCount)
                  .SingleOrDefault();

                // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                if (deleteUser == null)
                {
                    return false;
                }

                deleteUser.SetBaseInfoUpdate();
                deleteUser.DeleteFlag = 1;
            }

            using (var trans = dbContext.Database.BeginTransaction())
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
        /// セレクトボックスデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectCenterListItems()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT DISTINCT
                       MU.CENTER_ID AS VALUE
                      ,MW.CENTER_NAME1 AS TEXT
                  FROM M_USERS MU
                  LEFT JOIN
                       M_CENTERS MW
                    ON MU.SHIPPER_ID = MW.SHIPPER_ID
                   AND MU.CENTER_ID = MW.CENTER_ID
                 WHERE MU.SHIPPER_ID = :SHIPPER_ID
                ORDER BY
                       MU.CENTER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            var user = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters).ToList();

            // Excute paging
            return user;
        }

        /// <summary>
        /// セレクトボックスデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return MvcDbContext.Current.Warehouses
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CenterId.ToString(),
                    Text = m.CenterName1
                })
                .OrderBy(m => m.Text);
        }

        public string GetUsable_Class(Uri uri)
        {
            string CONTROLLER_NAME = "Home";
            string AREA_NAME = " ";

            if (uri.AbsolutePath.IndexOf("/Home/Index") >= 0 || uri.AbsolutePath.IndexOf("/Home") >= 0) return "3";
            if (uri.AbsolutePath.IndexOf("/Notice/Search") >= 0 || uri.AbsolutePath.IndexOf("/Notice") >= 0) {
                CONTROLLER_NAME = "Notice";
                return "3";
            }

            //URLが短いため、アラート
            if (uri.Segments.Count() >= 4)
            {
                AREA_NAME = uri.Segments[2].Replace("/", string.Empty).ToUpper();
                CONTROLLER_NAME = uri.Segments[3].Replace("/", string.Empty).ToUpper();
            }
            else if (uri.Segments.Count() == 3)
            {
                AREA_NAME = uri.Segments[1].Replace("/", string.Empty).ToUpper();
                CONTROLLER_NAME = uri.Segments[2].Replace("/", string.Empty).ToUpper();
            }

            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
            SELECT USABLE_CLASS
            FROM M_USER_PROGRAMS M_UP
            INNER JOIN M_PROGRAMS M_P
            ON
	            M_UP.SHIPPER_ID = M_P.SHIPPER_ID
            AND	M_UP.PROGRAM_ID = M_P.PROGRAM_ID
            AND	M_UP.SHIPPER_ID = :SHIPPER_ID
            AND UPPER(M_P.CONTROLLER_NAME) = :CONTROLLER_NAME
            AND 	UPPER(M_P.AREA_NAME) = :AREA_NAME
            AND 	M_UP.PERMISSION_LEVEL = :PERMISSION_LEVEL
            AND M_P.PROGRAM_CLASS = 1
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":AREA_NAME", AREA_NAME); 
            parameters.Add(":CONTROLLER_NAME", CONTROLLER_NAME); // ページ名を取得する
            parameters.Add(":PERMISSION_LEVEL", (int)Profile.User.PermissionLevel);

            return MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).FirstOrDefault() ?? "9";
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="shipperId">shipperId</param>
        /// <param name="CenterId">CenterId</param>
        /// <returns>User</returns>
        public LoginViewModel GetCurrentUser(string shipperId, string userId, string centerId)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(@"
                SELECT
                    MU.SHIPPER_ID
                    ,MU.USER_ID
                    ,MU.USER_NAME
                    ,MU.PERMISSION_LEVEL
                    ,:CENTER_ID AS CENTER_ID
                    ,MW.CENTER_SHORT_NAME
                    ,MU.PASSWORD_HASH
                    ,MU.PASSWORD_UPDATE_DATE
                    ,MU.PASSWORD_UPDATE_USER_ID
                    ,MU.PASSWORD_MISTYPE_COUNT
                    ,MU.PASSWORD_HASH1
                    ,MU.PASSWORD_HASH2
                    ,MU.PASSWORD_HASH3
                    ,MU.USER_LAPSE
                    ,(SELECT MG.GEN_NAME --MAX_MISTYPE_COUNT取得
                      FROM M_GENERALS MG
                      WHERE MU.SHIPPER_ID = MG.SHIPPER_ID
                      AND MG.REGISTER_DIVI_CD = '1'
                      AND MG.GEN_DIV_CD = 'PASSWORD_SETTINGS'
                      AND MG.GEN_CD = 'MAX_MISTYPE_COUNT'
                    ) GEN_NAME
                    ,(SELECT MG_1.GEN_NAME --MIN_LENGTH取得
                      FROM M_GENERALS MG_1
                      WHERE MU.SHIPPER_ID = MG_1.SHIPPER_ID
                      AND MG_1.REGISTER_DIVI_CD = '1'
                      AND MG_1.GEN_DIV_CD = 'PASSWORD_SETTINGS'
                      AND MG_1.GEN_CD = 'MIN_LENGTH'
                    ) MIN_LENGTH
                    ,MU.DATE_MAKE_CLASS
                    FROM M_USERS MU
                    INNER JOIN M_CENTERS MW --倉庫マスタ存在
                    ON   MU.SHIPPER_ID = MW.SHIPPER_ID
                      AND :CENTER_ID = MW.CENTER_ID
                    WHERE
                      MU.SHIPPER_ID = :SHIPPER_ID
                    AND   MU.USER_ID = :USER_ID
                    AND
                    (
                      MU.PERMISSION_LEVEL IN(--汎用マスタに権限があればOK
                          SELECT MG.GEN_CD
                          FROM M_GENERALS MG
                          WHERE MG.REGISTER_DIVI_CD = '1'
                          AND MG.SHIPPER_ID = :SHIPPER_ID
                          AND MG.GEN_DIV_CD = 'CENTER_CHANGE_LEVEL'
                          )
                    OR
                      MU.CENTER_ID = :CENTER_ID --または利用者マスタのセンターコードと一致
                    )
                    AND   MU.DELETE_FLAG = 0
                    AND   MW.DELETE_FLAG = 0

            ");

            DynamicParameters parameters = new DynamicParameters();

            parameters.AddDynamicParams(new
            {
                SHIPPER_ID = shipperId,
                CENTER_ID = centerId,
                USER_ID = userId
            });

            LoginViewModel userModel = MvcDbContext.Current.Database.Connection.Query<LoginViewModel>(sql.ToString(), parameters).FirstOrDefault();

            return userModel;
        }

        /// <summary>
        /// パスワード最小桁数指定取得(汎用コードマスタ)
        /// </summary>
        /// <returns></returns>
        public int GetPassMinLength()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                SELECT
                        GEN_NAME --MIN_LENGTH取得
                FROM
                        M_GENERALS
                WHERE 
                        SHIPPER_ID = :SHIPPER_ID
                    AND REGISTER_DIVI_CD = '1'
                    AND GEN_DIV_CD = 'PASSWORD_SETTINGS'
                    AND GEN_CD = 'MIN_LENGTH'
            ");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            var min_length = MvcDbContext.Current.Database.Connection.Query<string>(sql.ToString(), parameters).FirstOrDefault();

            return int.Parse(min_length);
        }

        /// <summary>
        /// Update PasswordMistypeCount
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Update status</returns>
        public bool UpdatePasswordMistypeCount(LoginViewModel user)
        {
            var dbContext = MvcDbContext.Current;

            var updatedUser =
                  MvcDbContext.Current.User
                  .Where(m => m.ShipperId == user.ShipperId && m.UserId == user.UserId)
                  .SingleOrDefault();

            if (updatedUser == null)
            {
                return false;
            }

            updatedUser.UpdateCount = updatedUser.UpdateCount + 1;
            updatedUser.PasswordMistypeCount += 1;
            updatedUser.UserLapse = byte.Parse(user.UserLapse.ToString());
            updatedUser.UpdateDate = DateTimeOffset.Now;
            updatedUser.UpdateUserId = user.UserId;
            updatedUser.UpdateProgramName = "UnKnown";

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
        /// Update PasswordMistypeCount
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Update status</returns>
        public bool ResetMisstypeCount(LoginViewModel user)
        {
            var dbContext = MvcDbContext.Current;

            var updatedUser =
                  MvcDbContext.Current.User
                  .Where(m => m.ShipperId == user.ShipperId && m.UserId == user.UserId)
                  .SingleOrDefault();

            if (updatedUser == null)
            {
                return false;
            }

            updatedUser.UpdateCount = updatedUser.UpdateCount + 1;
            updatedUser.PasswordMistypeCount = 0;
            updatedUser.UpdateDate = DateTimeOffset.Now;
            updatedUser.UpdateUserId = user.UserId;
            updatedUser.UpdateProgramName = "UnKnown";

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
        /// Change password
        /// </summary>
        /// <param name="changePasswordViewModel">changePasswordViewModel</param>
        /// <returns>True if success and vice-versa</returns>
        public bool ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var dbContext = MvcDbContext.Current;

            var updatedUser =
                   MvcDbContext.Current.User
                   .Where(m => m.ShipperId == changePasswordViewModel.ShipperId && m.UserId == changePasswordViewModel.UserId)
                   .SingleOrDefault();

            if (updatedUser == null)
            {
                return false;
            }

            updatedUser.UpdateCount = updatedUser.UpdateCount + 1;
            updatedUser.PasswordMistypeCount = 0;
            updatedUser.UpdateDate = DateTimeOffset.Now;
            updatedUser.UpdateUserId = changePasswordViewModel.UserId;
            //updatedUser.PasswordHash = Convert.ToBase64String(HashUtil.GenerateSaltedHash(changePasswordViewModel.NewPassword, HashUtil.GenerateSalt()));
            updatedUser.PasswordHash = HashUtil.ComputeHashMd5(changePasswordViewModel.NewPassword);
            updatedUser.UserLapse = 1;
            updatedUser.UpdateProgramName = "UnKnown";
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
    }
}