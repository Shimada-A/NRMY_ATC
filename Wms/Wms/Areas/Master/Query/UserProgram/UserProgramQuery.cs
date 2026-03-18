namespace Wms.Areas.Master.Models
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.UserProgram;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.UserProgram.UserProgramSearchCondition;

    public partial class UserProgram
    {
        /// <summary>
        /// Get UserProgram List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<UserProgramList> GetData(UserProgramSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            if (condition.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelNonPermission)
            {
                query.Append(@"
                    SELECT
                         MP.SHIPPER_ID
                        ,MP.PROGRAM_ID
                        ,MP.PROGRAM_NAME
                        ,MP.PROGRAM_CLASS
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                            WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                            AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                            AND PERMISSION_LEVEL = '2') USABLE_CLASS2
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                            WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                            AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                            AND PERMISSION_LEVEL = '2') UPDATE_COUNT2
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '3') USABLE_CLASS3
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '3') UPDATE_COUNT3
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '4') USABLE_CLASS4
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '4') UPDATE_COUNT4
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '5') USABLE_CLASS5
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '5') UPDATE_COUNT5
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '6') USABLE_CLASS6
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '6') UPDATE_COUNT6
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '7') USABLE_CLASS7
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '7') UPDATE_COUNT7
                    FROM M_PROGRAMS MP
                   WHERE MP.SHIPPER_ID = :SHIPPER_ID
                ");
            }
            else
            {
                query.Append(@"
                    SELECT
                         MP.SHIPPER_ID
                        ,MP.PROGRAM_ID
                        ,MP.PROGRAM_NAME
                        ,MP.PROGRAM_CLASS
                ");
                if (condition.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelUserManager)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS2
                    ,MUP.UPDATE_COUNT UPDATE_COUNT2");
                }
                else if (condition.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelWarehouseManager)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS3
                    ,MUP.UPDATE_COUNT UPDATE_COUNT3");
                }
                else if (condition.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelClericalWorker)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS4
                    ,MUP.UPDATE_COUNT UPDATE_COUNT4");
                }
                else if (condition.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelWarehouseWorker)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS5
                    ,MUP.UPDATE_COUNT UPDATE_COUNT5");
                }
                else if (condition.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelPartTimeJobber)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS6
                    ,MUP.UPDATE_COUNT UPDATE_COUNT6");
                }
                else if (condition.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelHeadOfficeWorker)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS7
                    ,MUP.UPDATE_COUNT UPDATE_COUNT7");
                }

                query.Append(@"
                    FROM M_PROGRAMS MP
                    LEFT JOIN
                         M_USER_PROGRAMS MUP
                      ON MUP.SHIPPER_ID = MP.SHIPPER_ID
                     AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                     AND MUP.PERMISSION_LEVEL = :PERMISSION_LEVEL
                   WHERE MP.SHIPPER_ID = :SHIPPER_ID
                ");
            }

            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.ProgramName))
            {
                query.Append(" AND MP.PROGRAM_NAME LIKE :PROGRAM_NAME ");
                parameters.Add(":PROGRAM_NAME", condition.ProgramName + "%");
            }

            if (condition.ProgramClass != ProgramClasses.ProgramNone)
            {
                query.Append(" AND MP.PROGRAM_CLASS = :PROGRAM_CLASS ");
                parameters.Add(":PROGRAM_CLASS", condition.ProgramClass);
            }

            if (condition.PermissionLevel != ProgramPermissionLevelClasses.PermissionLevelNonPermission)
            {
                parameters.Add(":PERMISSION_LEVEL", condition.PermissionLevel);
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<UserProgramList>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case UserProgramSortKey.ProgramName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MP.PROGRAM_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MP.PROGRAM_NAME ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MP.PROGRAM_CLASS DESC, MP.PROGRAM_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MP.PROGRAM_CLASS ASC, MP.PROGRAM_NAME ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var userProgram = MvcDbContext.Current.Database.Connection.Query<UserProgramList>(query.ToString(), parameters).ToList();

            for (var i = 0; i < userProgram.Count; i++)
            {
                userProgram[i].PermissionLevel = condition.PermissionLevel;
                if (userProgram[i].UsableClass2 == 0 || userProgram[i].UsableClass2 == null)
                {
                    userProgram[i].UsableClass2 = 0;
                    userProgram[i].UsableFlg2 = 1;
                }

                if (userProgram[i].UsableClass3 == 0 || userProgram[i].UsableClass3 == null)
                {
                    userProgram[i].UsableClass3 = 0;
                    userProgram[i].UsableFlg3 = 1;
                }

                if (userProgram[i].UsableClass4 == 0 || userProgram[i].UsableClass4 == null)
                {
                    userProgram[i].UsableClass4 = 0;
                    userProgram[i].UsableFlg4 = 1;
                }

                if (userProgram[i].UsableClass5 == 0 || userProgram[i].UsableClass5 == null)
                {
                    userProgram[i].UsableClass5 = 0;
                    userProgram[i].UsableFlg5 = 1;
                }

                if (userProgram[i].UsableClass6 == 0 || userProgram[i].UsableClass6 == null)
                {
                    userProgram[i].UsableClass6 = 0;
                    userProgram[i].UsableFlg6 = 1;
                }

                if (userProgram[i].UsableClass7 == 0 || userProgram[i].UsableClass7 == null)
                {
                    userProgram[i].UsableClass7 = 0;
                    userProgram[i].UsableFlg7 = 1;
                }
            }

            // Excute paging
            return new StaticPagedList<UserProgramList>(userProgram, condition.Page, condition.PageSize, totalCount);
        }

        public List<string> GetProgramId(UserProgramSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            StringBuilder query = new StringBuilder();
            query.Append(@" SELECT MP.PROGRAM_ID FROM  M_PROGRAMS MP WHERE MP.SHIPPER_ID = :SHIPPER_ID");

            // Add search condition
            if (!string.IsNullOrEmpty(condition.ProgramName))
            {
                query.Append(" AND MP.PROGRAM_NAME LIKE :PROGRAM_NAME ");
                parameters.Add(":PROGRAM_NAME", condition.ProgramName + "%");
            }

            if (condition.ProgramClass != ProgramClasses.ProgramNone)
            {
                query.Append(" AND MP.PROGRAM_CLASS = :PROGRAM_CLASS ");
                parameters.Add(":PROGRAM_CLASS", condition.ProgramClass);
            }

            if (condition.PermissionLevel != ProgramPermissionLevelClasses.PermissionLevelNonPermission)
            {
                parameters.Add(":PERMISSION_LEVEL", condition.PermissionLevel);
            }


            return MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).ToList();
        }

        /// <summary>
        /// Get UserProgram By Id
        /// </summary>
        /// <param name="programId">programId</param>
        /// <param name="permissionLevel">permissionLevel</param>
        /// <param name="shipperId">ShipperId</param>
        /// <returns>Country</returns>
        public UserProgram GetTargetById(string programId, int permissionLevel, string shipperId)
        {
            return MvcDbContext.Current.UserPrograms.Find(programId, permissionLevel, shipperId);
        }

        public List<SelectedUserProgramViewModel> GetRows(List<string> rowids, ProgramPermissionLevelClasses PermissionLevel)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            string ids = string.Empty;
            if (rowids != null)
            {
                foreach (var item in rowids)
                {
                    if (string.IsNullOrEmpty(ids))
                    {
                        ids = "'" + item + "'";
                    }
                    else
                    {
                        ids += ", '" + item + "'";
                    }

                }
            }

            StringBuilder query = new StringBuilder();

            if (PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelNonPermission)
            {
                query.Append(@"
                    SELECT
                         MP.SHIPPER_ID
                        ,MP.PROGRAM_ID
                        ,MP.PROGRAM_NAME
                        ,MP.PROGRAM_CLASS
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                            WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                            AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                            AND PERMISSION_LEVEL = '2') USABLE_CLASS2
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                            WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                            AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                            AND PERMISSION_LEVEL = '2') UPDATE_COUNT2
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '3') USABLE_CLASS3
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '3') UPDATE_COUNT3
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '4') USABLE_CLASS4
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '4') UPDATE_COUNT4
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '5') USABLE_CLASS5
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '5') UPDATE_COUNT5
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '6') USABLE_CLASS6
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '6') UPDATE_COUNT6
                        ,(SELECT USABLE_CLASS
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '7') USABLE_CLASS7
                        ,(SELECT UPDATE_COUNT
                            FROM M_USER_PROGRAMS MUP
                             WHERE MUP.SHIPPER_ID = MP.SHIPPER_ID
                             AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                             AND PERMISSION_LEVEL = '7') UPDATE_COUNT7
                    FROM M_PROGRAMS MP
                   WHERE MP.SHIPPER_ID = :SHIPPER_ID
                ");
            }
            else
            {
                query.Append(@"
                    SELECT
                         MP.SHIPPER_ID
                        ,MP.PROGRAM_ID
                        ,MP.PROGRAM_NAME
                        ,MP.PROGRAM_CLASS
                ");
                if (PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelUserManager)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS2
                    ,MUP.UPDATE_COUNT UPDATE_COUNT2");
                }
                else if (PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelWarehouseManager)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS3
                    ,MUP.UPDATE_COUNT UPDATE_COUNT3");
                }
                else if (PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelClericalWorker)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS4
                    ,MUP.UPDATE_COUNT UPDATE_COUNT4");
                }
                else if (PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelWarehouseWorker)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS5
                    ,MUP.UPDATE_COUNT UPDATE_COUNT5");
                }
                else if (PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelPartTimeJobber)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS6
                    ,MUP.UPDATE_COUNT UPDATE_COUNT6");
                }
                else if (PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelHeadOfficeWorker)
                {
                    query.Append(@"
                    ,MUP.USABLE_CLASS USABLE_CLASS7
                    ,MUP.UPDATE_COUNT UPDATE_COUNT7");
                }

                query.Append(@"
                    FROM M_PROGRAMS MP
                    LEFT JOIN
                         M_USER_PROGRAMS MUP
                      ON MUP.SHIPPER_ID = MP.SHIPPER_ID
                     AND MUP.PROGRAM_ID = MP.PROGRAM_ID
                     AND MUP.PERMISSION_LEVEL = :PERMISSION_LEVEL
                   WHERE MP.SHIPPER_ID = :SHIPPER_ID
                ");
            }
            query.Append($"  AND MP.PROGRAM_ID IN({ ids }) ");

            if (PermissionLevel != ProgramPermissionLevelClasses.PermissionLevelNonPermission)
            {
                parameters.Add(":PERMISSION_LEVEL", PermissionLevel);
            }

            var userProgram = MvcDbContext.Current.Database.Connection.Query<SelectedUserProgramViewModel>(query.ToString(), parameters).ToList<SelectedUserProgramViewModel>();

            for (var i = 0; i < userProgram.Count; i++)
            {
                userProgram[i].PermissionLevel = PermissionLevel;
                if (userProgram[i].UsableClass2 == 0)
                {
                    userProgram[i].UsableClass2 = 0;
                    userProgram[i].UsableFlg2 = 1;
                }

                if (userProgram[i].UsableClass3 == 0)
                {
                    userProgram[i].UsableClass3 = 0;
                    userProgram[i].UsableFlg3 = 1;
                }

                if (userProgram[i].UsableClass4 == 0)
                {
                    userProgram[i].UsableClass4 = 0;
                    userProgram[i].UsableFlg4 = 1;
                }

                if (userProgram[i].UsableClass5 == 0)
                {
                    userProgram[i].UsableClass5 = 0;
                    userProgram[i].UsableFlg5 = 1;
                }

                if (userProgram[i].UsableClass6 == 0)
                {
                    userProgram[i].UsableClass6 = 0;
                    userProgram[i].UsableFlg6 = 1;
                }

                if (userProgram[i].UsableClass7 == 0)
                {
                    userProgram[i].UsableClass7 = 0;
                    userProgram[i].UsableFlg7 = 1;
                }
            }

            return userProgram;
        }

        /// <summary>
        /// Update UserProgram
        /// </summary>
        /// <param name="userProgram"></param>
        /// <returns>Update status</returns>
        public bool UpdateUserProgram(List<SelectedUserProgramViewModel> userProgram)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in userProgram)
                {
                    if (u.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelUserManager)
                    {
                        var dataUserPrograms = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 2)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms.UsableClass != u.UsableClass2)
                            {
                                if (dataUserPrograms.UpdateCount == u.UpdateCount2)
                                {
                                    // Set data update
                                    dataUserPrograms.SetBaseInfoUpdate();
                                    dataUserPrograms.UsableClass = u.UsableClass2;
                                }
                                else
                                {
                                    return false;
                                }
                            }
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
                    else if (u.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelWarehouseManager)
                    {
                        var dataUserPrograms = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 3)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms.UsableClass != u.UsableClass3)
                            {
                                if (dataUserPrograms.UpdateCount == u.UpdateCount3)
                                {
                                    // Set data update
                                    dataUserPrograms.SetBaseInfoUpdate();
                                    dataUserPrograms.UsableClass = u.UsableClass3;
                                }
                                else
                                {
                                    return false;
                                }
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
                    else if (u.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelClericalWorker)
                    {
                        var dataUserPrograms = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 4)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms.UsableClass != u.UsableClass4)
                            {
                                if (dataUserPrograms.UpdateCount == u.UpdateCount4)
                                {
                                    // Set data update
                                    dataUserPrograms.SetBaseInfoUpdate();
                                    dataUserPrograms.UsableClass = u.UsableClass4;
                                }
                                else
                                {
                                    return false;
                                }
                            }
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
                    else if (u.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelWarehouseWorker)
                    {
                        var dataUserPrograms = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 5)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms.UsableClass != u.UsableClass5)
                            {
                                if (dataUserPrograms.UpdateCount == u.UpdateCount5)
                                {
                                    // Set data update
                                    dataUserPrograms.SetBaseInfoUpdate();
                                    dataUserPrograms.UsableClass = u.UsableClass5;
                                }
                                else
                                {
                                    return false;
                                }
                            }
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
                    else if (u.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelPartTimeJobber)
                    {
                        var dataUserPrograms = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 6)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms.UsableClass != u.UsableClass6)
                            {
                                if (dataUserPrograms.UpdateCount == u.UpdateCount6)
                                {
                                    // Set data update
                                    dataUserPrograms.SetBaseInfoUpdate();
                                    dataUserPrograms.UsableClass = u.UsableClass6;
                                }
                                else
                                {
                                    return false;
                                }
                            }
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
                    else if (u.PermissionLevel == ProgramPermissionLevelClasses.PermissionLevelHeadOfficeWorker)
                    {
                        var dataUserPrograms = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 7)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms.UsableClass != u.UsableClass7)
                            {
                                if (dataUserPrograms.UpdateCount == u.UpdateCount7)
                                {
                                    // Set data update
                                    dataUserPrograms.SetBaseInfoUpdate();
                                    dataUserPrograms.UsableClass = u.UsableClass7;
                                }
                                else
                                {
                                    return false;
                                }
                            }
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
                    else
                    {
                        var dataUserPrograms2 = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 2)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms2 == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms2.UsableClass != u.UsableClass2)
                            {
                                if (dataUserPrograms2.UpdateCount == u.UpdateCount2)
                                {
                                    // Set data update
                                    dataUserPrograms2.SetBaseInfoUpdate();
                                    dataUserPrograms2.UsableClass = u.UsableClass2;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        var dataUserPrograms3 = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 3)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms3 == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms3.UsableClass != u.UsableClass3)
                            {
                                if (dataUserPrograms3.UpdateCount == u.UpdateCount3)
                                {
                                    // Set data update
                                    dataUserPrograms3.SetBaseInfoUpdate();
                                    dataUserPrograms3.UsableClass = u.UsableClass3;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        var dataUserPrograms4 = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 4)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms4 == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms4.UsableClass != u.UsableClass4)
                            {
                                if (dataUserPrograms4.UpdateCount == u.UpdateCount4)
                                {
                                    // Set data update
                                    dataUserPrograms4.SetBaseInfoUpdate();
                                    dataUserPrograms4.UsableClass = u.UsableClass4;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        var dataUserPrograms5 = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 5)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms5 == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms5.UsableClass != u.UsableClass5)
                            {
                                if (dataUserPrograms5.UpdateCount == u.UpdateCount5)
                                {
                                    // Set data update
                                    dataUserPrograms5.SetBaseInfoUpdate();
                                    dataUserPrograms5.UsableClass = u.UsableClass5;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        var dataUserPrograms6 = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 6)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms6 == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms6.UsableClass != u.UsableClass6)
                            {
                                if (dataUserPrograms6.UpdateCount == u.UpdateCount6)
                                {
                                    // Set data update
                                    dataUserPrograms6.SetBaseInfoUpdate();
                                    dataUserPrograms6.UsableClass = u.UsableClass6;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        var dataUserPrograms7 = MvcDbContext.Current.UserPrograms
                       .Where(m => m.ShipperId == Profile.User.ShipperId && m.ProgramId == u.ProgramId && m.PermissionLevel == 7)
                       .SingleOrDefault();

                        // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
                        if (dataUserPrograms7 == null)
                        {
                            return false;
                        }
                        else
                        {
                            if (dataUserPrograms7.UsableClass != u.UsableClass7)
                            {
                                if (dataUserPrograms7.UpdateCount == u.UpdateCount7)
                                {
                                    // Set data update
                                    dataUserPrograms7.SetBaseInfoUpdate();
                                    dataUserPrograms7.UsableClass = u.UsableClass7;
                                }
                                else
                                {
                                    return false;
                                }
                            }
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
    }
}