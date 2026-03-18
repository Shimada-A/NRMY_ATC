namespace Wms.Areas.Master.Query.User
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using Dapper;
    using PagedList;
    using Share.Reports.Import;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.User;
    using Wms.Common;
    using Wms.Common.Resources;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;
    using static Wms.Areas.Master.ViewModels.User.UserSearchCondition;

    public class Report : BaseQuery
    {
        /// <summary>
        /// エクセルに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<ViewModels.User.Report> Listing(UserSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        MU.CENTER_ID
                    ,   MW.CENTER_NAME1 || MW.CENTER_NAME2 CENTER_NAME
                    ,   MU.USER_ID
                    ,   MU.USER_NAME
                    ,   MU.PERMISSION_LEVEL
                    ,   MG.GEN_NAME PERMISSION_LEVEL_NAME
                FROM
                        M_USERS MU
                LEFT JOIN
                        M_CENTERS MW
                ON
                        MU.SHIPPER_ID = MW.SHIPPER_ID
                    AND MU.CENTER_ID  = MW.CENTER_ID
                LEFT JOIN
                        M_GENERALS MG
                ON
                        MG.SHIPPER_ID = MU.SHIPPER_ID
                    AND MG.GEN_CD     = MU.PERMISSION_LEVEL
                    AND MG.GEN_DIV_CD = 'PERMISSION_LEVEL'
                    AND MG.REGISTER_DIVI_CD = '1'
                WHERE
                        MU.SHIPPER_ID = :SHIPPER_ID
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

            return MvcDbContext.Current.Database.Connection.Query<ViewModels.User.Report>(query.ToString(), parameters).ToList();
        }

        /// <summary>
        /// GAS用作業者マスタCSVに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<ViewModels.User.GasReport> GasListing(UserSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        USERS.USER_ID AS USER_ID
                    ,   USERS.USER_NAME AS USER_NAME
                    ,   1 AS USER_MODE --1:初心者
                FROM
                        M_USERS USERS
                WHERE
                        USERS.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND USERS.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            if (!string.IsNullOrEmpty(condition.UserId))
            {
                query.Append(" AND USERS.USER_ID LIKE :USER_ID ");
                parameters.Add(":USER_ID", condition.UserId + "%");
            }

            if (!string.IsNullOrEmpty(condition.UserName))
            {
                query.Append(" AND USERS.USER_NAME LIKE :USER_NAME ");
                parameters.Add(":USER_NAME", condition.UserName + "%");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<ViewModels.User.GasReport>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case UserSortKey.UserName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY USERS.USER_NAME DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY USERS.USER_NAME ASC");
                            break;
                    }

                    break;

                case UserSortKey.CenterIdUserId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY USERS.CENTER_ID DESC , USERS.USER_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY USERS.CENTER_ID ASC , USERS.USER_ID ASC");
                            break;
                    }

                    break;

                case UserSortKey.PermissionLevelUserId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY USERS.PERMISSION_LEVEL DESC , USERS.USER_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY USERS.PERMISSION_LEVEL ASC , USERS.USER_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY USERS.USER_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY USERS.USER_ID ASC");
                            break;
                    }

                    break;
            }

            IEnumerable<ViewModels.User.GasReport> data = MvcDbContext.Current.Database.Connection.Query<ViewModels.User.GasReport>(query.ToString(), parameters).ToList();
            
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            foreach (var d in data)
            {
                //作業者コード 20バイト
                if (sjisEnc.GetByteCount(d.UserId) > 20)
                {
                    byte[] userId = sjisEnc.GetBytes(d.UserId);
                    d.UserId = new String(d.UserId.TakeWhile((c, i) => sjisEnc.GetByteCount(d.UserId.Substring(0, i + 1)) <= 20).ToArray());
                }
                //作業者名称 40バイト
                if (sjisEnc.GetByteCount(d.UserName) > 40)
                {
                    byte[] userName = sjisEnc.GetBytes(d.UserName);
                    d.UserName = new String(d.UserName.TakeWhile((c, i) => sjisEnc.GetByteCount(d.UserName.Substring(0, i + 1)) <= 40).ToArray());
                }
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<UserLabelReportRowForCsv> GetResultRowList(UserSearchCondition condition)
        {
            List<UserLabelReportRowForCsv> ret = new List<UserLabelReportRowForCsv>();
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        MU.USER_ID
                  FROM M_USERS MU
                 WHERE MU.SHIPPER_ID = :SHIPPER_ID
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

            return MvcDbContext.Current.Database.Connection.Query<UserLabelReportRowForCsv>(query.ToString(), parameters).ToList();
        }
    }
}