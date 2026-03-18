namespace Wms.Areas.Master.Query.NaniwaSorting
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.ViewModels.NaniwaSorting;
    using Wms.Areas.Master.Models;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.NaniwaSorting.NaniwaSortingSearchCondition;

    public partial class NaniwaSortingQuery
    {
        /// <summary>
        /// 一覧取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<NaniwaSortingList> GetData(NaniwaSortingSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            //一覧表示のSQLを取得
            GetQuery(condition, ref query, ref parameters);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<NaniwaSortingList>(query.ToString(), parameters).Count();

            // ページング
            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // 一覧に表示するデータを取得
            var NaniwaSorting = MvcDbContext.Current.Database.Connection.Query<NaniwaSortingList>(query.ToString(), parameters).ToList();

            return new StaticPagedList<NaniwaSortingList>(NaniwaSorting, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 一覧表示のSQLを取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="query">SQL</param>
        /// <param name="parameters"></param>
        public static void GetQuery(NaniwaSortingSearchCondition condition, ref StringBuilder query, ref DynamicParameters parameters)
        {
            query.Append(@"
                    SELECT
                            NANIWA.SHIPPER_ID AS SHIPPER_ID
                        ,   NANIWA.STORE_ID AS STORE_ID
                        ,   STORES.SHIP_TO_STORE_NAME1 AS STORE_NAME
                        ,   NANIWA.NANIWA_DELI_CENTER_CD AS NANIWA_DELI_CENTER_CD
                        ,   NANIWA_DELI_CENTER.GEN_NAME AS NANIWA_DELI_CENTER_NAME
                        ,   NANIWA.ROWID AS ROW_ID
                    FROM
                            M_NANIWA_SORTING NANIWA
                    LEFT OUTER JOIN
                            V_SHIP_TO_STORES STORES
                    ON
                            NANIWA.SHIPPER_ID = STORES.SHIPPER_ID
                        AND NANIWA.STORE_ID = STORES.SHIP_TO_STORE_ID
                    LEFT OUTER JOIN
                            M_GENERALS NANIWA_DELI_CENTER
                    ON
                            NANIWA_DELI_CENTER.SHIPPER_ID = NANIWA.SHIPPER_ID
                        AND NANIWA_DELI_CENTER.CENTER_ID = '@@@'
                        AND NANIWA_DELI_CENTER.REGISTER_DIVI_CD = '1'
                        AND NANIWA_DELI_CENTER.GEN_DIV_CD = 'NANIWA_DELI_CENTER'
                        AND NANIWA_DELI_CENTER.GEN_CD = NANIWA.NANIWA_DELI_CENTER_CD
                    WHERE
                            NANIWA.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);


            // 店舗ID
            if (!string.IsNullOrEmpty(condition.StoreId))
            {
                query.Append(" AND NANIWA.STORE_ID LIKE :STORE_ID ");
                parameters.Add(":STORE_ID", condition.StoreId + "%");
            }

            // 店舗名
            if (!string.IsNullOrEmpty(condition.StoreName))
            {
                query.Append(" AND STORES.SHIP_TO_STORE_NAME1 LIKE :STORE_NAME ");
                parameters.Add(":STORE_NAME", "%" + condition.StoreName + "%");
            }

            // 配送センターコード
            if (!string.IsNullOrEmpty(condition.NaniwaDeliCenterCd))
            {
                query.Append(" AND NANIWA.NANIWA_DELI_CENTER_CD LIKE :NANIWA_DELI_CENTER_CD ");
                parameters.Add(":NANIWA_DELI_CENTER_CD", condition.NaniwaDeliCenterCd + "%");
            }

            // 配送センター名
            if (!string.IsNullOrEmpty(condition.NaniwaDeliCenterName))
            {
                query.Append(" AND NANIWA_DELI_CENTER.GEN_NAME LIKE :NANIWA_DELI_CENTER_NAME ");
                parameters.Add(":NANIWA_DELI_CENTER_NAME", "%" + condition.NaniwaDeliCenterName + "%");
            }

            // 表示順
            switch (condition.SortKey)
            {
                case NaniwaSortingSortKey.StoreId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"
                                ORDER BY
                                        NANIWA.STORE_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"
                                ORDER BY
                                        NANIWA.STORE_ID ASC");
                            break;
                    }

                    break;

                case NaniwaSortingSortKey.NaniwaDeliCenterCdStoreId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"
                                ORDER BY
                                        NANIWA.NANIWA_DELI_CENTER_CD DESC
                                    ,   NANIWA.STORE_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"
                                ORDER BY
                                        NANIWA.NANIWA_DELI_CENTER_CD ASC
                                    ,   NANIWA.STORE_ID ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY NANIWA.STORE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY NANIWA.STORE_ID ASC");
                            break;
                    }

                    break;
            }
        }

        /// <summary>
        /// 対象のデータを取得
        /// </summary>
        /// <param name="StoreId">店舗ID</param>
        /// <param name="shipperId">荷主ID</param>
        /// <returns></returns>
        public NaniwaSorting GetTargetById(string StoreId, string shipperId)
        {
            return MvcDbContext.Current.NaniwaSorting.Find(StoreId, shipperId);
        }

        /// <summary>
        /// 更新画面に表示するデータを取得
        /// </summary>
        /// <param name="rowid">一覧画面で選択されたデータのROWID</param>
        /// <returns>更新画面に表示するデータ</returns>
        public Detail GetTargetById(string rowid)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@"
                SELECT
                        NANIWA.SHIPPER_ID AS SHIPPER_ID
                    ,   NANIWA.STORE_ID AS STORE_ID
                    ,   STORES.SHIP_TO_STORE_NAME1 AS STORE_NAME
                    ,   NANIWA.NANIWA_DELI_CENTER_CD AS NANIWA_DELI_CENTER_CD
                    ,   NANIWA.UPDATE_COUNT AS UPDATE_COUNT
                    ,   NANIWA.ROWID AS ROW_ID
                FROM
                        M_NANIWA_SORTING NANIWA
                LEFT OUTER JOIN
                        V_SHIP_TO_STORES STORES
                ON
                        NANIWA.SHIPPER_ID = STORES.SHIPPER_ID
                    AND NANIWA.STORE_ID = STORES.SHIP_TO_STORE_ID
                WHERE 
                        NANIWA.ROWID = :ROWID1
            ");
            parameters.Add(":ROWID1", rowid);

            return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// ROWIDを取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<NaniwaSortingList> GetRowId(NaniwaSortingSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();

            //一覧表示のSQLを取得
            GetQuery(condition, ref query, ref parameters);

            // 一覧に表示するデータを取得
            return MvcDbContext.Current.Database.Connection.Query<NaniwaSortingList>(query.ToString(), parameters).ToList();
        }

        /// <summary>
        /// 新規作成
        /// </summary>
        /// <param name="NaniwaSorting">新規登録画面で入力されたデータ</param>
        /// <returns></returns>
        public bool Create(Detail NaniwaSorting)
        {
            var dbContext = MvcDbContext.Current;
            var NaniwaSortingAdd = new NaniwaSorting();
            NaniwaSortingAdd.SetBaseInfoInsert();
            NaniwaSortingAdd.StoreId = NaniwaSorting.StoreId;
            NaniwaSortingAdd.NaniwaDeliCenterCd = NaniwaSorting.NaniwaDeliCenterCd;

            dbContext.NaniwaSorting.Add(NaniwaSortingAdd);
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
        /// 更新
        /// </summary>
        /// <param name="NaniwaSorting">更新画面で入力されたデータ</param>
        /// <returns>Update status</returns>
        public bool Update(Detail NaniwaSorting)
        {
            var dbContext = MvcDbContext.Current;

            var updateNaniwaSortings = MvcDbContext.Current.NaniwaSorting
                .Where(m => m.ShipperId == NaniwaSorting.ShipperId
                    && m.StoreId == NaniwaSorting.StoreId
                    && m.UpdateCount == NaniwaSorting.UpdateCount)
                .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
            if (updateNaniwaSortings == null)
            {
                return false;
            };

            updateNaniwaSortings.SetBaseInfoUpdate();
            updateNaniwaSortings.StoreId = NaniwaSorting.StoreId;
            updateNaniwaSortings.NaniwaDeliCenterCd = NaniwaSorting.NaniwaDeliCenterCd;

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
        /// 削除
        /// </summary>
        /// <param name="rowids">一覧で選択されたデータのROWID</param>
        /// <returns>Update status</returns>
        public string Delete(List<string> rowids)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@" DELETE FROM
                                    M_NANIWA_SORTING NANIWA
                            WHERE
                                    NANIWA.ROWID IN :ROWID");

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                parameters.Add(":ROWID", rowids);
                MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);

                try
                {
                    MvcDbContext.Current.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return "FALSE";
                }

                trans.Commit();
            }
            return string.Empty;
        }

        /// <summary>
        /// 新規登録画面 店舗ID マスタ存在チェック
        /// </summary>
        public bool CheckStoreIdMasterExists(string storeId)
        {
            var stores = MvcDbContext.Current.Stores
                .Where(m => m.ShipperId == Profile.User.ShipperId && m.StoreId == storeId).Count();
            var warehouses = MvcDbContext.Current.Warehouses
                .Where(m => m.ShipperId == Profile.User.ShipperId && m.CenterId == storeId).Count();
            return stores + warehouses > 0 ? true : false;
        }

        /// <summary>
        /// 配送センターコードが汎用コードマスタに登録されているかチェック
        /// </summary>
        /// <param name="NaniwaDeliCenterCd">配送センターコード</param>
        /// <returns>true:汎用コードマスタに登録されている</returns>
        public bool ExistsNaniwaDeliCenterCd(string naniwaDeliCenterCd)
        {
            var parameters = new DynamicParameters();
            var query = new StringBuilder();

            query.Append(@" SELECT
                                    *
                            FROM
                                    M_GENERALS
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                                AND CENTER_ID = '@@@'
                                AND GEN_DIV_CD = 'NANIWA_DELI_CENTER'  -- 汎用分類コード 浪速運送配送センター NANIWA_DELI_CENTER
                                AND REGISTER_DIVI_CD = '1' 
                                AND GEN_CD = :GEN_CD
            ");

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                parameters.Add(":GEN_CD", naniwaDeliCenterCd);

                return MvcDbContext.Current.Database.Connection.Query<Detail>(query.ToString(), parameters).Any();
            }
        }
    }
}