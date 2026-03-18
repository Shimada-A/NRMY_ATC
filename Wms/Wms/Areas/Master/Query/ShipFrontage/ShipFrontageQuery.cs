namespace Wms.Areas.Master.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using Mvc.Common;
    using PagedList;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.ViewModels.ShipFrontage;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.ShipFrontage.ShipFrontageSearchCondition;

    public partial class ShipFrontage
    {
        /// <summary>
        /// Get ShipFrontage List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<ShipFrontageResultRow> GetData(ShipFrontageSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            //一覧表示のSQLを取得
            GetQuery(condition, ref query, ref parameters);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<ShipFrontageResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var ShipFrontages = MvcDbContext.Current.Database.Connection.Query<ShipFrontageResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<ShipFrontageResultRow>(ShipFrontages, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 一覧表示のSQLを取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters"></param>
        public static void GetQuery(ShipFrontageSearchCondition condition, ref StringBuilder query, ref DynamicParameters parameters)
        {
            query.Append(@"
                SELECT
                        SHIP_FRONTAGE.UPDATE_COUNT
                    ,   SHIP_FRONTAGE.SHIPPER_ID
                    ,   SHIP_FRONTAGE.CENTER_ID
                    ,   CENTER.CENTER_NAME1 CENTER_NAME
                    ,   SHIP_FRONTAGE.LANE_NO
                    ,   SHIP_FRONTAGE.FRONTAGE_NO
                    ,   SHIP_FRONTAGE.STORE_ID
                    ,   STORES.SHIP_TO_STORE_NAME1 STORE_NAME
                    ,   SHIP_FRONTAGE.ROWID RID
                    ,   SHIP_FRONTAGE.BRAND_ID
                    ,   MB.BRAND_SHORT_NAME BRAND_NAME
                FROM
                        M_SHIP_FRONTAGE SHIP_FRONTAGE
                LEFT JOIN
                        M_CENTERS CENTER
                ON
                        SHIP_FRONTAGE.CENTER_ID = CENTER.CENTER_ID
                    AND SHIP_FRONTAGE.SHIPPER_ID = CENTER.SHIPPER_ID
                LEFT JOIN
                        V_SHIP_TO_STORES STORES
                ON
                        SHIP_FRONTAGE.STORE_ID = STORES.SHIP_TO_STORE_ID
                    AND SHIP_FRONTAGE.SHIPPER_ID = STORES.SHIPPER_ID
                    AND STORES.DELETE_FLAG = 0
                LEFT JOIN 
                        M_BRANDS MB
                ON
                        SHIP_FRONTAGE.BRAND_ID = MB.BRAND_ID
                    AND SHIP_FRONTAGE.SHIPPER_ID = MB.SHIPPER_ID
                WHERE
                        SHIP_FRONTAGE.SHIPPER_ID = :SHIPPER_ID
                    AND SHIP_FRONTAGE.CENTER_ID = :CENTER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":Center_id", condition.CenterId);

            // Add search condition
            //if (!string.IsNullOrEmpty(condition.CenterId))
            //{
            //    query.Append(" AND SHIP_FRONTAGE.CENTER_ID = :CENTER_ID ");
            //    parameters.Add(":CENTER_ID", condition.CenterId);
            //}

            if (!string.IsNullOrEmpty(condition.BrandId))
            {
                query.Append("AND SHIP_FRONTAGE.BRAND_ID = :BRAND_ID ");
                parameters.Add(":BRAND_ID", condition.BrandId);
            }

            if (!string.IsNullOrEmpty(condition.BrandName))
            {
                query.Append("AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                parameters.Add(":BRAND_NAME", condition.BrandName + "%");
            }

            if (!string.IsNullOrEmpty(condition.LaneNo.ToString()))
            {
                query.Append(" AND SHIP_FRONTAGE.LANE_NO LIKE :LANE_NO ");
                parameters.Add(":LANE_NO", condition.LaneNo );
            }

            if (!string.IsNullOrEmpty(condition.FrontageNo.ToString()))
            {
                query.Append(" AND SHIP_FRONTAGE.FRONTAGE_NO LIKE :FRONTAGE_NO ");
                parameters.Add(":FRONTAGE_NO", condition.FrontageNo);
            }

            if (!string.IsNullOrEmpty(condition.ShipToStoreId))
            {
                query.Append(" AND SHIP_FRONTAGE.STORE_ID = :STORE_ID ");
                parameters.Add(":STORE_ID", condition.ShipToStoreId);
            }

            if (!string.IsNullOrEmpty(condition.ShipToStoreName))
            {
                query.Append(" AND STORES.SHIP_TO_STORE_NAME1 LIKE :STORE_NAME ");
                parameters.Add(":STORE_NAME", condition.ShipToStoreName + "%");
            }

            // Sort function
            switch (condition.SortKey)
            {
                case ShipFrontageSortKey.StoreId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_FRONTAGE.STORE_ID DESC,SHIP_FRONTAGE.BRAND_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_FRONTAGE.STORE_ID ASC,SHIP_FRONTAGE.BRAND_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_FRONTAGE.BRAND_ID DESC, SHIP_FRONTAGE.LANE_NO DESC,SHIP_FRONTAGE.FRONTAGE_NO DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_FRONTAGE.BRAND_ID ASC, SHIP_FRONTAGE.LANE_NO ASC,SHIP_FRONTAGE.FRONTAGE_NO ASC");
                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// Get ROWID
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public List<ShipFrontageResultRow> GetRowId(ShipFrontageSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            //一覧表示のSQLを取得
            GetQuery(condition, ref query, ref parameters);

            return MvcDbContext.Current.Database.Connection.Query<ShipFrontageResultRow>(query.ToString(), parameters).ToList();
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="shipFrontage"></param>
        /// <returns>ShipFrontage</returns>
        public ShipFrontage GetTargetById(ShipFrontageResultRow shipFrontage)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        *
                FROM
                        M_SHIP_FRONTAGE SHIP_FRONTAGE
                WHERE
                        SHIP_FRONTAGE.SHIPPER_ID = :SHIPPER_ID
                    AND SHIP_FRONTAGE.CENTER_ID = :CENTER_ID
                    AND SHIP_FRONTAGE.LANE_NO = :LANE_NO
                    AND SHIP_FRONTAGE.FRONTAGE_NO = :FRONTAGE_NO
                    AND SHIP_FRONTAGE.BRAND_ID = :BRAND_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", shipFrontage.CenterId);
            parameters.Add(":LANE_NO", shipFrontage.LaneNo);
            parameters.Add(":FRONTAGE_NO", shipFrontage.FrontageNo);
            parameters.Add(":BRAND_ID", shipFrontage.BrandId);
            return MvcDbContext.Current.Database.Connection.Query<ShipFrontage>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// ブランドID、荷主ID、倉庫ID、店舗ID単位で登録済みではないかチェック
        /// </summary>
        /// <param name="shipFrontage"></param>
        /// <returns></returns>
        public bool CheckUniqueIndex(ShipFrontageResultRow shipFrontage)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        *
                FROM
                        M_SHIP_FRONTAGE SHIP_FRONTAGE
                WHERE
                        SHIP_FRONTAGE.SHIPPER_ID = :SHIPPER_ID
                    AND SHIP_FRONTAGE.CENTER_ID = :CENTER_ID
                    AND SHIP_FRONTAGE.BRAND_ID = :BRAND_ID
                    AND SHIP_FRONTAGE.LANE_NO || '-' || SHIP_FRONTAGE.FRONTAGE_NO <> :LANE_NO || '-' || :FRONTAGE_NO
                    AND SHIP_FRONTAGE.STORE_ID = :STORE_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", shipFrontage.CenterId);
            parameters.Add(":LANE_NO", shipFrontage.LaneNo);
            parameters.Add(":FRONTAGE_NO", shipFrontage.FrontageNo);
            parameters.Add(":BRAND_ID", shipFrontage.BrandId);
            parameters.Add(":STORE_ID", shipFrontage.StoreId);
            return MvcDbContext.Current.Database.Connection.Query<ShipFrontageResultRow>(query.ToString(), parameters).Any();
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="rowid">rowid</param>
        /// <returns>ShipFrontage</returns>
        public ShipFrontageResultRow GetTargetById(string rowid)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@"
                SELECT
                        SHIP_FRONTAGE.UPDATE_COUNT
                    ,   SHIP_FRONTAGE.SHIPPER_ID
                    ,   SHIP_FRONTAGE.CENTER_ID
                    ,   CENTER.CENTER_NAME1 CENTER_NAME
                    ,   SHIP_FRONTAGE.LANE_NO
                    ,   SHIP_FRONTAGE.FRONTAGE_NO
                    ,   SHIP_FRONTAGE.STORE_ID
                    ,   STORES.SHIP_TO_STORE_NAME1 STORE_NAME
                    ,   SHIP_FRONTAGE.ROWID RID
                    ,   SHIP_FRONTAGE.BRAND_ID
                    ,   MB.BRAND_NAME BRAND_NAME
                FROM
                        M_SHIP_FRONTAGE SHIP_FRONTAGE
                LEFT JOIN
                        M_CENTERS CENTER
                ON
                        SHIP_FRONTAGE.CENTER_ID = CENTER.CENTER_ID
                    AND SHIP_FRONTAGE.SHIPPER_ID = CENTER.SHIPPER_ID
                LEFT JOIN
                        V_SHIP_TO_STORES STORES
                ON
                        SHIP_FRONTAGE.STORE_ID = STORES.SHIP_TO_STORE_ID
                    AND SHIP_FRONTAGE.SHIPPER_ID = STORES.SHIPPER_ID
                LEFT JOIN 
                        M_BRANDS MB
                ON
                        SHIP_FRONTAGE.BRAND_ID = MB.BRAND_ID
                    AND SHIP_FRONTAGE.SHIPPER_ID = MB.SHIPPER_ID
                WHERE
                        SHIP_FRONTAGE.ROWID = :ROWID1
            ");
            parameters.Add(":ROWID1", rowid);

            return MvcDbContext.Current.Database.Connection.Query<ShipFrontageResultRow>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// Delete ShipFrontage
        /// </summary>
        /// <param name="lstShipFrontage">List record is deleted</param>
        /// <returns>List of rows error </returns>
        public bool Delete(List<string> rowids)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@" DELETE FROM M_SHIP_FRONTAGE T  WHERE T.ROWID = :ROWID1");

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var rid in rowids)
                {
                    parameters.Add(":ROWID1", rid);
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
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
        /// Create new ShipFrontage
        /// </summary>
        /// <param name="shipFrontage"></param>
        /// <returns></returns>
        public bool Create(ShipFrontageResultRow resultRow)
        {
            var dbContext = MvcDbContext.Current;
            var shipFrontage = new ShipFrontage();
            shipFrontage.SetBaseInfoInsert();
            shipFrontage.BrandId= resultRow.BrandId;
            shipFrontage.CenterId = resultRow.CenterId;
            shipFrontage.LaneNo = resultRow.LaneNo;
            shipFrontage.FrontageNo = resultRow.FrontageNo;
            shipFrontage.StoreId = resultRow.StoreId;
            dbContext.ShipFrontages.Add(shipFrontage);
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.SaveChanges();
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    AppError.PutLog(ex);
                    return false;
                }
                catch (Exception ex)
                {
                    AppError.PutLog(ex);
                    return false;
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// Update ShipFrontage
        /// </summary>
        /// <param name="shipFrontage"></param>
        /// <returns>Update status</returns>
        public bool UpdateShipFrontage(ShipFrontageResultRow shipFrontage)
        {
            var dbContext = MvcDbContext.Current;

            var updatedShipFrontage =
                  MvcDbContext.Current.ShipFrontages
                  .Where(m => m.ShipperId == shipFrontage.ShipperId && m.CenterId == shipFrontage.CenterId && m.BrandId == shipFrontage.BrandId && m.LaneNo == shipFrontage.LaneNo && m.FrontageNo == shipFrontage.FrontageNo && m.UpdateCount == shipFrontage.UpdateCount)
                  .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合） 
            if (updatedShipFrontage == null)
            {
                return false;
            }

            updatedShipFrontage.SetBaseInfoUpdate();
            updatedShipFrontage.StoreId = shipFrontage.StoreId;

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
        /// マスタ存在チェック
        /// </summary>
        public bool CheckMasterExists(string storeId)
        {
            var stores = MvcDbContext.Current.Stores
                .Where(m => m.ShipperId == Profile.User.ShipperId && m.StoreId == storeId).Count();
            var warehouses = MvcDbContext.Current.Warehouses
                .Where(m => m.ShipperId == Profile.User.ShipperId && m.CenterId == storeId).Count();
            return stores + warehouses > 0 ? true : false;
        }

        /// <summary>
        /// ブランドマスタ存在チェック
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public bool CheckBrandExists(string brandId)
        {
            var brands = MvcDbContext.Current.Brands
                .Where(m => m.ShipperId == Profile.User.ShipperId && m.BrandId == brandId).Count();
            return brands > 0 ? true : false;
        }
    }
}