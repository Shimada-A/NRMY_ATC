namespace Wms.Areas.Master.Models
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Wms.Areas.Master.ViewModels.Vendor;
    using Wms.Common;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.Vendor.VendorSearchCondition;

    public partial class Vendor
    {
        /// <summary>
        /// Get Vendor List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<Vendor> GetData(VendorSearchCondition condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       MV.UPDATE_COUNT
                      ,MV.SHIPPER_ID
                      ,MV.VENDOR_ID
                      ,MV.VENDOR_NAME1
                      ,MV.VENDOR_NAME2
                      ,MV.VENDOR_SHORT_NAME
                      ,MV.VENDOR_KANA_NAME1
                      ,MV.VENDOR_KANA_NAME2
                      ,MV.VENDOR_KANA_SHORT_NAME
                      ,MV.VENDOR_COUNTRY
                      ,MV.VENDOR_ZIP
                      ,MV.VENDOR_PREF_NAME
                      ,MV.VENDOR_PREF_KANA_NAME
                      ,MV.VENDOR_CITY_NAME
                      ,MV.VENDOR_CITY_KANA_NAME
                      ,MV.VENDOR_ADDRESS1
                      ,MV.VENDOR_KANA_ADDRESS1
                      ,MV.VENDOR_ADDRESS2
                      ,MV.VENDOR_KANA_ADDRESS2
                      ,MV.VENDOR_ADDRESS3
                      ,MV.VENDOR_KANA_ADDRESS3
                      ,MV.VENDOR_TEL
                      ,MV.VENDOR_FAX
                      ,MV.VENDOR_MAIL1
                      ,MV.VENDOR_MAIL2
                      ,MV.VENDOR_STAFF_NAME
                      ,MV.CHANGE_ASSET_TIMING
                      ,MV.PAYMENT_CLOSING_DATE
                      ,MV.PAYMENT_CLASS
                      ,MV.PAYMENT_DATE
                      ,MV.PAYMENT_KIND_CLASS
                      ,MV.SUPPLIER_CLASS
                      ,MV.DELETE_FLAG
                  FROM M_VENDORS MV
                 WHERE MV.SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

            // Add search condition
            if (!string.IsNullOrEmpty(condition.VendorId))
            {
                query.Append(" AND MV.VENDOR_ID LIKE :VENDOR_ID ");
                parameters.Add(":VENDOR_ID", condition.VendorId + "%");
            }

            if (!string.IsNullOrEmpty(condition.VendorName))
            {
                query.Append(@" AND (MV.VENDOR_NAME1 LIKE :VENDOR_NAME1) ");
                parameters.Add(":VENDOR_NAME1", condition.VendorName + "%");
            }

            // 削除フラグfalseの時は削除フラグ1は非表示
            if (!condition.DeleteFlag)
            {
                query.Append(" AND MV.DELETE_FLAG <> 1");
            }

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<Vendor>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case VendorSortKey.VendorName:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MV.VENDOR_NAME1 DESC,MV.VENDOR_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MV.VENDOR_NAME1 ASC,MV.VENDOR_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MV.VENDOR_ID DESC,MV.VENDOR_NAME1 DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MV.VENDOR_ID ASC,MV.VENDOR_NAME1 ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            List<Vendor> vendors = MvcDbContext.Current.Database.Connection.Query<Vendor>(query.ToString(), parameters).ToList();

            // Excute paging
            return new StaticPagedList<Vendor>(vendors, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Get Vendor By Id
        /// </summary>
        /// <param name="vendorId">vendorId</param>
        /// <param name="shipperId">shipperId</param>
        /// <returns>Country</returns>
        public Vendor GetTargetById(string vendorId, string shipperId)
        {
            return MvcDbContext.Current.Vendors.Find(vendorId, shipperId);
        }

        /// <summary>
        /// 都道府県入力チェック
        /// </summary>
        /// <param name="warehouses"></param>
        /// <returns></returns>
        public bool SelectPrefs(Vendor vendor)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                        MP.PREF_ID
                    ,   MP.PREF_NAME
                FROM
                        M_PREFS MP
                WHERE
                        MP.SHIPPER_ID = :SHIPPER_ID
                    AND MP.PREF_NAME = :PREF_NAME
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":PREF_NAME", vendor.VendorPrefName);

            // Fill data to memory
            var pref = MvcDbContext.Current.Database.Connection.Query<Pref>(query.ToString(), parameters).ToList();

            // Excute paging
            if (pref.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Update Vendor
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns>Update status</returns>
        public bool UpdateVendor(Vendor vendor)
        {
            var dbContext = MvcDbContext.Current;

            var updatedVendor =
                  MvcDbContext.Current.Vendors
                  .Where(m => m.ShipperId == vendor.ShipperId && m.VendorId == vendor.VendorId && m.UpdateCount == vendor.UpdateCount)
                  .SingleOrDefault();

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (updatedVendor == null)
            {
                return false;
            }

            updatedVendor.SetBaseInfoUpdate();

            updatedVendor.VendorZip = vendor.VendorZip;
            updatedVendor.VendorPrefName = vendor.VendorPrefName;
            updatedVendor.VendorPrefKanaName = vendor.VendorPrefKanaName;
            updatedVendor.VendorCityName = vendor.VendorCityName;
            updatedVendor.VendorCityKanaName = vendor.VendorCityKanaName;
            updatedVendor.VendorAddress1 = vendor.VendorAddress1;
            updatedVendor.VendorKanaAddress1 = vendor.VendorKanaAddress1;
            updatedVendor.VendorAddress2 = vendor.VendorAddress2;
            updatedVendor.VendorKanaAddress2 = vendor.VendorKanaAddress2;
            updatedVendor.VendorAddress3 = vendor.VendorAddress3;
            updatedVendor.VendorKanaAddress3 = vendor.VendorKanaAddress3;
            updatedVendor.VendorTel = vendor.VendorTel;
            updatedVendor.VendorFax = vendor.VendorFax;

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