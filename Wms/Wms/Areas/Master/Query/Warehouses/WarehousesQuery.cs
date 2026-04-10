namespace Wms.Areas.Master.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.Warehouses;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.Warehouses.WarehousesSearchCondition;

    public partial class Warehouses
    {
        /// <summary>
        /// string型のWmsClassをbool型に変換
        /// </summary>
        public bool WmsClassBool 
        {
            get { return Convert.ToBoolean(byte.Parse(WmsClass)); }            

        }

        /// <summary>
        /// string型のBrandWorkClassをbool型に変換
        /// </summary>
        public bool BrandWorkClassBool
        {
            get { return Convert.ToBoolean(byte.Parse(BrandWorkClass)); }

        }
        /// <summary>
        /// Get Warehouses List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Warehouses> GetData(WarehousesSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MW.UPDATE_COUNT
                      ,MW.SHIPPER_ID
                      ,MW.CENTER_ID
                      ,MW.CENTER_NAME1 || MW.CENTER_NAME2 CENTER_NAME1
                      ,CENTER_SHORT_NAME
                      ,MW.WMS_CLASS
                      ,MW.BRAND_WORK_CLASS
                      ,MW.CENTER_PREF_NAME
                      ,MW.CENTER_CITY_NAME
                  FROM M_CENTERS MW
                 WHERE MW.SHIPPER_ID = :SHIPPER_ID
                   AND MW.DELETE_FLAG = 0
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND MW.CENTER_ID LIKE :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId + "%");
            }

            if (!string.IsNullOrEmpty(condition.CenterName))
            {
                query.Append(" AND MW.CENTER_NAME1 LIKE :CENTER_NAME ");
                parameters.Add(":CENTER_NAME", condition.CenterName + "%");
            }

            if (!string.IsNullOrEmpty(condition.CenterAddress))
            {
                query.Append(@" AND (MW.CENTER_PREF_NAME
                                 ||  MW.CENTER_CITY_NAME
                                 ||  MW.CENTER_ADDRESS1
                                 ||  MW.CENTER_ADDRESS2
                                 ||  MW.CENTER_ADDRESS3 LIKE :CENTER_ADDRESS)");
                parameters.Add(":CENTER_ADDRESS", "%" + condition.CenterAddress + "%");
            }

            if (!string.IsNullOrEmpty(condition.CenterTel))
            {
                query.Append(" AND REPLACE(MW.CENTER_TEL,'-') LIKE :CENTER_TEL ");
                parameters.Add(":CENTER_TEL", condition.CenterTel + "%");
            }

            if (!string.IsNullOrEmpty(condition.CenterZip))
            {
                query.Append(@" AND MW.CENTER_ZIP LIKE :CENTER_ZIP ");
                parameters.Add(":CENTER_ZIP", "%" + condition.CenterZip + "%");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Warehouses>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case WarehousesSortKey.CenterName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MW.CENTER_NAME1 DESC,MW.CENTER_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MW.CENTER_NAME1 ASC,MW.CENTER_ID ASC");
                            break;
                    }

                    break;

                default:

                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MW.CENTER_ID DESC,MW.CENTER_NAME1 DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MW.CENTER_ID ASC,MW.CENTER_NAME1 ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            List<Warehouses> warehouses = MvcDbContext.Current.Database.Connection.Query<Warehouses>(query.ToString(), parameters).ToList();

            // Excute paging
            return new StaticPagedList<Warehouses>(warehouses, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Get Warehouses By Id
        /// </summary>
        /// <param name="CenterId">CenterId</param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public Warehouses GetTargetById(string CenterId, string shipperId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MC.UPDATE_DATE
                      ,MC.UPDATE_USER_ID
                      ,MC.UPDATE_PROGRAM_NAME
                      ,MC.UPDATE_COUNT
                      ,MC.SHIPPER_ID
                      ,MC.CENTER_ID
                      ,MC.CENTER_CLASS
                      ,MC.CENTER_NAME1
                      ,MC.CENTER_NAME2
                      ,MC.CENTER_SHORT_NAME
                      ,MC.CENTER_KANA_NAME1
                      ,MC.CENTER_KANA_NAME2
                      ,MC.CENTER_KANA_SHORT_NAME
                      ,MC.CENTER_COUNTRY
                      ,MC.CENTER_ZIP
                      ,MC.CENTER_PREF_NAME
                      ,MC.CENTER_PREF_KANA_NAME
                      ,MC.CENTER_CITY_NAME
                      ,MC.CENTER_CITY_KANA_NAME
                      ,MC.CENTER_ADDRESS1
                      ,MC.CENTER_KANA_ADDRESS1
                      ,MC.CENTER_ADDRESS2
                      ,MC.CENTER_KANA_ADDRESS2
                      ,MC.CENTER_ADDRESS3
                      ,MC.CENTER_KANA_ADDRESS3
                      ,MC.CENTER_TEL
                      ,MC.CENTER_FAX
                      ,MC.CENTER_MAIL1
                      ,MC.CENTER_MAIL2
                      ,MP.PREF_ID
                      ,MC.WMS_CLASS
                      ,MC.BRAND_WORK_CLASS
                      ,MC.INVOICE_CENTER_NAME
                  FROM M_CENTERS MC
                  LEFT JOIN M_PREFS MP
                     ON MP.SHIPPER_ID = MC.SHIPPER_ID
                     AND MP.PREF_NAME = MC.CENTER_PREF_NAME
                 WHERE MC.SHIPPER_ID = :SHIPPER_ID
                   AND MC.CENTER_ID = :CENTER_ID
            ");
            parameters.Add(":SHIPPER_ID", shipperId);
            parameters.Add(":CENTER_ID", CenterId);

            return MvcDbContext.Current.Database.Connection.Query<Warehouses>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// 都道府県マスタから都道府県名を取得する
        /// </summary>
        /// <param name="warehouses"></param>
        /// <returns></returns>
        public bool SelectPrefs(Warehouses warehouses)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT MP.PREF_ID
                      ,MP.PREF_NAME
                  FROM M_PREFS MP
                 WHERE MP.SHIPPER_ID = :SHIPPER_ID
                   AND MP.PREF_NAME = :PREF_NAME
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":PREF_NAME", warehouses.CenterPrefName);

            // Fill data to memory
            var pref = MvcDbContext.Current.Database.Connection.Query<Pref>(query.ToString(), parameters).ToList();

            // Excute paging
            if (pref.Count == 0) {
                return false;
            }
            else {
                return true;
            }

        }
        
        /// <summary>
        /// センターマスタから電話番号を取得する
        /// </summary>
        /// <param name="warehouses"></param>
        /// <returns></returns>
        public bool SelectTel(Warehouses warehouses)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT MC.CENTER_ID
                      ,MC.CENTER_TEL
                  FROM M_CENTERS MC
                 WHERE MC.SHIPPER_ID = :SHIPPER_ID
                   AND MC.CENTER_TEL = :CENTER_TEL
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":CENTER_TEL", warehouses.CenterTel);

            // Fill data to memory
            var tel = MvcDbContext.Current.Database.Connection.Query<Centers>(query.ToString(), parameters).ToList();

            // Excute paging
            if ((tel.Count == 0) ||
               (tel.Count == 1 && tel[0].CenterTel.ToString() == warehouses.CenterTel)) {
                return true;
            }
            else {
                return false;
            }

        }

        /// <summary>
        /// Update Warehouses
        /// </summary>
        /// <param name="warehouses"></param>
        /// <returns>Update status</returns>
        public bool UpdateWarehouses(Warehouses warehouses)
        {
            var dbContext = MvcDbContext.Current;

            var updatedWarehouses = MvcDbContext.Current.Warehouses
                  .Where(m => m.ShipperId == warehouses.ShipperId && m.CenterId == warehouses.CenterId && m.UpdateCount == warehouses.UpdateCount)
                  .SingleOrDefault();

            var Prefs = MvcDbContext.Current.Prefs
                  .Where(m => m.ShipperId == warehouses.ShipperId && m.PrefName == warehouses.CenterPrefName)
                  .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって更新済みまたは削除済みの場合）
            if (updatedWarehouses == null)
            {
                return false;
            }

            updatedWarehouses.SetBaseInfoUpdate();

            updatedWarehouses.CenterShortName= warehouses.CenterShortName;
            updatedWarehouses.CenterZip = warehouses.CenterZip;
            updatedWarehouses.CenterPrefName = warehouses.CenterPrefName;
            updatedWarehouses.CenterCityName = warehouses.CenterCityName;
            updatedWarehouses.CenterAddress1 = warehouses.CenterAddress1;
            updatedWarehouses.CenterAddress2 = warehouses.CenterAddress2;
            updatedWarehouses.CenterAddress3 = warehouses.CenterAddress3;
            updatedWarehouses.CenterTel = warehouses.CenterTel;
            updatedWarehouses.CenterFax = warehouses.CenterFax;
            updatedWarehouses.PrefId = Prefs.PrefId;
            updatedWarehouses.InvoiceCenterName = warehouses.InvoiceCenterName;

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
        /// Get warehousesName By Id
        /// </summary>
        /// <returns>string</returns>
        public string GetNameById()
        {
            var sql = @"
                SELECT
                  CENTER_SHORT_NAME
                FROM
                    M_CENTERS
                WHERE
                     CENTER_ID  = :CENTER_ID
                 AND SHIPPER_ID = :SHIPPER_ID
            ";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new
            {
                SHIPPER_ID = Common.Profile.User.ShipperId,
                CENTER_ID = Common.Profile.User.CenterId
            });

            return MvcDbContext.Current.Database.Connection.Query<string>(sql.ToString(), parameters).FirstOrDefault();
        }
    }
}