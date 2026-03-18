namespace Wms.Query
{
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Common;
    using Wms.Models;

    public class BaseQuery
    {
        protected MvcDbContext _dbContext = MvcDbContext.Current;

        /// <summary>
        /// ワークID取得処理
        /// </summary>
        /// <returns></returns>
        public long GetWorkId()
        {
            return this._dbContext.Database.SqlQuery<long>("SELECT SF_GET_WORK_ID() FROM DUAL").SingleOrDefault();
        }

        /// <summary>
        /// 引当処理中チェック
        /// </summary>
        /// <returns></returns>
        public long IsAllocProcessing(string centerId = null, int allocType = 0)
        {
            StringBuilder query = new StringBuilder();
            DynamicParameters parameters = new DynamicParameters();

           // 店舗マスタ
           query.Append(@"
               SELECT
                    SF_IS_ALLOC_PROCESSING (
                            :SHIPPER_ID
                        ,   :CENTER_ID
                        ,   :ALLOC_TYPE
                    )
               FROM
                    DUAL
           ");

           parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
           parameters.Add(":CENTER_ID", centerId ?? Profile.User.CenterId);
           parameters.Add(":ALLOC_TYPE", allocType);

           return MvcDbContext.Current.Database.Connection.Query<long>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// 日次処理中 チェック
        /// </summary>
        /// <returns></returns>
        public long IsDailyProcessing()
        {
            //return this._dbContext.Database.SqlQuery<long>("SELECT SF_IS_DAILY_PROCESSING() FROM DUAL").SingleOrDefault();
            return 0;
        }

        /// <summary>
        /// 棚卸中チェック
        /// </summary>
        /// <returns></returns>
        public long IsTanaLocProcessing()
        {
            //return this._dbContext.Database.SqlQuery<long>("SELECT SF_IS_TANA_LOC_PROCESSING() FROM DUAL").SingleOrDefault();
            return 0;
        }

        /// <summary>
        /// Get コードより名称を取得
        /// <param name="kbn">マスタ区分</param>
        /// <param name="cd">検索用コード</param>
        /// </summary>
        /// <returns>検索名称</returns>
        public string GetName(string kbn, string cd, string cd1)
        {
            string name = string.Empty;
            StringBuilder query = new StringBuilder();
            DynamicParameters parameters = new DynamicParameters();

            switch (kbn)
            {
                case "STORE":
                    // 店舗マスタ
                    query.Append(@"
                        SELECT
                              STORE_NAME1
                         FROM M_STORES
                         WHERE SHIPPER_ID   = :SHIPPER_ID
                           AND STORE_ID = :STORE_ID
                    ");

                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":STORE_ID", cd);

                    break;
                case "BRAND":
                    // ブランドマスタ
                    query.Append(@"
                        SELECT
                              BRAND_SHORT_NAME
                         FROM M_BRANDS
                         WHERE SHIPPER_ID   = :SHIPPER_ID
                           AND BRAND_ID = :BRAND_ID
                    ");

                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":BRAND_ID", cd);

                    break;
                case "CASECLASS":
                    // ロケーション区分マスタ
                    query.Append(@"
                        SELECT
                              DISTINCT  CASE_CLASS
                         FROM M_LOCATION_CLASSES
                         WHERE SHIPPER_ID   = :SHIPPER_ID
                           AND LOCATION_CLASS = :LOCATION_CLASS
                    ");

                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":LOCATION_CLASS", cd);

                    break;
                case "VENDOR":
                    // 仕入先マスタ
                    query.Append(@"
                        SELECT
                              VENDOR_SHORT_NAME
                         FROM M_VENDORS
                         WHERE SHIPPER_ID   = :SHIPPER_ID
                           AND VENDOR_ID = :VENDOR_ID
                    ");

                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":VENDOR_ID", cd);

                    break;

                default:
                    break;
            }

            if (query != null)
            {
                name = MvcDbContext.Current.Database.Connection.ExecuteScalar<string>(query.ToString(), parameters);
            }

            return name;
        }
    }
}