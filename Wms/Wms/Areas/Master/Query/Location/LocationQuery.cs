namespace Wms.Areas.Master.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.ViewModels.Location;
    using Wms.Areas.Stock.Models;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.Location.LocationSearchCondition;

    public partial class Location
    {
        /// <summary>
        /// Get User List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<LocationList> GetData(LocationSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT
                            ML.UPDATE_COUNT
                        ,   ML.LOCATION_CD
                        ,   ML.LOCSEC_1
                        ,   ML.LOCSEC_2
                        ,   ML.LOCSEC_3
                        ,   ML.LOCSEC_4
                        ,   ML.LOCSEC_5
                        ,   ML.LOCATION_CLASS
                        ,   MLC.LOCATION_NAME
                        ,   ML.CASE_CLASS
                        ,   GEN_CASE_CLASS.GEN_NAME AS CASE_CLASS_NAME
                        ,   ML.GRADE_ID
                        ,   GRADE.GRADE_NAME
                        ,   ML.ALLOC_PRIORITY
                        ,   ML.PICKING_GROUP_NO
                        ,   ML.CENTER_ID
                        ,   ML.SHIPPER_ID
                        ,   ML.ROWID RID
                        ,   CASE 
                                WHEN GEN_FIXED_LOCATION_CD.GEN_CD = 'BDF' THEN 1
                                ELSE NVL(MLC.MENTE_DISABLE_FLAG,1)
                            END AS MENTE_DISABLE_FLAG
                    FROM
                            M_LOCATIONS ML
                    LEFT JOIN
                            M_LOCATION_CLASSES MLC
                    ON
                            ML.SHIPPER_ID = MLC.SHIPPER_ID
                        AND ML.LOCATION_CLASS = MLC.LOCATION_CLASS
                    LEFT JOIN
                            M_GENERALS GEN_FIXED_LOCATION_CD
                    ON
                            GEN_FIXED_LOCATION_CD.SHIPPER_ID = ML.SHIPPER_ID
                        AND GEN_FIXED_LOCATION_CD.CENTER_ID = '@@@'
                        AND GEN_FIXED_LOCATION_CD.REGISTER_DIVI_CD = '1'
                        AND GEN_FIXED_LOCATION_CD.GEN_DIV_CD = 'FIXED_LOCATION_CD'
                        AND GEN_FIXED_LOCATION_CD.GEN_NAME = ML.LOCATION_CD
                    LEFT JOIN
                            M_GENERALS GEN_CASE_CLASS
                    ON
                            GEN_CASE_CLASS.SHIPPER_ID = ML.SHIPPER_ID
                        AND GEN_CASE_CLASS.CENTER_ID = '@@@'
                        AND GEN_CASE_CLASS.REGISTER_DIVI_CD = '1'
                        AND GEN_CASE_CLASS.GEN_DIV_CD = 'CASE_CLASS'
                        AND GEN_CASE_CLASS.GEN_CD = TO_CHAR(ML.CASE_CLASS)
                    LEFT JOIN
                            M_GRADES GRADE
                    ON
                            GRADE.SHIPPER_ID = ML.SHIPPER_ID
                        AND GRADE.GRADE_ID = ML.GRADE_ID
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

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<LocationList>(query.ToString(), parameters).Count();

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

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var location = MvcDbContext.Current.Database.Connection.Query<LocationList>(query.ToString(), parameters).ToList();

            // Excute paging
            return new StaticPagedList<LocationList>(location, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<string> GetRowId(LocationSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        ML.ROWID
                FROM
                        M_LOCATIONS ML
                INNER JOIN
                        M_LOCATION_CLASSES MLC
                ON
                        ML.SHIPPER_ID = MLC.SHIPPER_ID
                    AND ML.LOCATION_CLASS = MLC.LOCATION_CLASS
                    AND MLC.MENTE_DISABLE_FLAG = 0
                LEFT JOIN
                        M_GENERALS GEN_FIXED_LOCATION_CD
                ON
                        GEN_FIXED_LOCATION_CD.SHIPPER_ID = ML.SHIPPER_ID
                    AND GEN_FIXED_LOCATION_CD.CENTER_ID = '@@@'
                    AND GEN_FIXED_LOCATION_CD.REGISTER_DIVI_CD = '1'
                    AND GEN_FIXED_LOCATION_CD.GEN_DIV_CD = 'FIXED_LOCATION_CD'
                    AND GEN_FIXED_LOCATION_CD.GEN_NAME = ML.LOCATION_CD
                LEFT JOIN
                        M_GENERALS GEN_CASE_CLASS
                ON
                        GEN_CASE_CLASS.SHIPPER_ID = ML.SHIPPER_ID
                    AND GEN_CASE_CLASS.CENTER_ID = '@@@'
                    AND GEN_CASE_CLASS.REGISTER_DIVI_CD = '1'
                    AND GEN_CASE_CLASS.GEN_DIV_CD = 'CASE_CLASS'
                    AND GEN_CASE_CLASS.GEN_CD = TO_CHAR(ML.CASE_CLASS)
                LEFT JOIN
                        M_GENERALS GEN_LOCATION_CLASS
                ON
                        GEN_LOCATION_CLASS.SHIPPER_ID = ML.SHIPPER_ID
                    AND GEN_LOCATION_CLASS.CENTER_ID = '@@@'
                    AND GEN_LOCATION_CLASS.REGISTER_DIVI_CD = '1'
                    AND GEN_LOCATION_CLASS.GEN_DIV_CD = 'LOCATION_CLASS'
                    AND GEN_LOCATION_CLASS.GEN_CD = TO_CHAR(ML.LOCATION_CLASS)
                LEFT JOIN
                        M_GRADES GRADE
                ON
                        GRADE.SHIPPER_ID = ML.SHIPPER_ID
                    AND GRADE.GRADE_ID = ML.GRADE_ID
                WHERE
                        ML.SHIPPER_ID = :SHIPPER_ID
                    AND GEN_FIXED_LOCATION_CD.SHIPPER_ID IS NULL
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

            return MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).ToList();

        }

        /// <summary>
        /// Get Location By Id
        /// </summary>
        /// <param name="locationCd">locationCd</param>
        /// <param name="centerId">centerId</param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public Location GetTargetById(string locationCd, string centerId, string shipperId)
        {
            return MvcDbContext.Current.Locations.Find(locationCd, centerId, shipperId);
        }

        public Detail GetTargetById(string rowid)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@"
                WITH
                    STOCK AS (
                        SELECT
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   LOCATION_CD
                            ,   MAX(STOCK_QTY) STOCK_QTY
                        FROM
                                T_STOCKS
                        GROUP BY
                                SHIPPER_ID
                            ,   CENTER_ID
                            ,   LOCATION_CD
                )
                SELECT
                        T.*
                    ,   STOCK.STOCK_QTY
                FROM 
                        M_LOCATIONS T
                LEFT JOIN
                        STOCK
                ON
                        T.SHIPPER_ID = STOCK.SHIPPER_ID
                    AND T.CENTER_ID = STOCK.CENTER_ID
                    AND T.LOCATION_CD = STOCK.LOCATION_CD
                WHERE 
                        T.ROWID = :ROWID1
            ");
            parameters.Add(":ROWID1", rowid);

            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// Create new Location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool Create(Detail location)
        {
            var dbContext = MvcDbContext.Current;
            var locationAdd = new Location();
            locationAdd.SetBaseInfoInsert();
            locationAdd.CenterId = location.CenterId;
            locationAdd.LocationCd = location.LocationCd;
            locationAdd.Locsec_1 = location.Locsec_1;
            locationAdd.Locsec_2 = location.Locsec_2;
            locationAdd.Locsec_3 = location.Locsec_3;
            locationAdd.Locsec_4 = location.Locsec_4;
            locationAdd.Locsec_5 = location.Locsec_5;
            locationAdd.LocationClass = location.LocationClass;
            locationAdd.CaseClass = location.CaseClass;
            locationAdd.GradeId = location.GradeId;
            locationAdd.AllocPriority = location.AllocPriority ?? 999999999;
            locationAdd.PickingGroupNo = location.PickingGroupNo;

            dbContext.Locations.Add(locationAdd);
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.SaveChanges();
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
        /// Update User
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Update status</returns>
        public bool UpdateLocation(Detail location)
        {
            var dbContext = MvcDbContext.Current;

            var updateLocations = MvcDbContext.Current.Locations
                  .Where(m => m.ShipperId == location.ShipperId
                        && m.CenterId == location.CenterId
                        && m.LocationCd == location.LocationCd
                        && m.UpdateCount == location.UpdateCount)
                  .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
            if (updateLocations == null)
            {
                return false;
            };

            updateLocations.SetBaseInfoUpdate();

            updateLocations.LocationClass = location.LocationClass;
            updateLocations.CaseClass = location.CaseClass;
            updateLocations.GradeId = location.GradeId;
            updateLocations.AllocPriority = location.AllocPriority ?? 999999999;
            updateLocations.PickingGroupNo = location.PickingGroupNo;

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
        /// Delete Location
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Update status</returns>
        public string DeleteLocation(List<string> rowids)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@" DELETE FROM
                                            M_LOCATIONS T
                                  WHERE
                                            T.ROWID IN :ROWID1");

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    foreach (var rid in rowids)
                    {
                        // 在庫テーブルチェック
                        var target = GetTargetById(rid);
                        if (target == null)
                        {
                            trans.Rollback();
                            return "FALSE";
                        };

                        if (CheckStock(target.ShipperId, target.CenterId, target.LocationCd))
                        {
                            trans.Rollback();
                            return target.LocationCd;
                        }
                    }
                    parameters.Add(":ROWID1", rowids);
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "FALSE";
                }

                trans.Commit();
            }

            return string.Empty;
        }

        /// <summary>
        /// 在庫テーブルチェック
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="centerId"></param>
        /// <param name="locationCd"></param>
        /// <returns>在庫テーブルにデータが存在する:true</returns>
        public bool CheckStock(string shipperId, string centerId, string locationCd)
        {
            return MvcDbContext.Current.Stocks.Where(x => x.ShipperId == shipperId &&
                                                          x.CenterId == centerId &&
                                                          x.LocationCd == locationCd).Any();
        }

        /// <summary>
        /// セレクトボックスデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectClassListItems(bool searchConditionFlag = false)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        MLS.LOCATION_CLASS AS VALUE
                    ,   MLS.LOCATION_NAME AS TEXT
                FROM
                        M_LOCATION_CLASSES MLS
                WHERE
                        MLS.SHIPPER_ID = :SHIPPER_ID
            ");

            if (!searchConditionFlag)
            {
                query.Append(@"
                    AND MLS.MENTE_DISABLE_FLAG = 0
                ");
            }

            query.Append(@"
                ORDER BY
                        MLS.LOCATION_CLASS
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            var locationClass = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters).ToList();

            // Excute paging
            return locationClass;
        }

        /// <summary>
        /// セレクトボックスデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectGradeListItems()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        MG.GRADE_ID AS VALUE
                    ,   MG.GRADE_NAME AS TEXT
                FROM
                        M_GRADES MG
                WHERE
                        MG.SHIPPER_ID = :SHIPPER_ID
                ORDER BY
                        MG.DISPLAY_ORDER
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Fill data to memory
            var locationClass = MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters).ToList();

            // Excute paging
            return locationClass;
        }

        /// <summary>
        /// セレクトボックスデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return MvcDbContext.Current.Locations
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CenterId,
                    Text = m.LocationCd
                })
                .OrderBy(m => m.Text);
        }
    }
}