namespace Wms.Areas.Master.ViewModels.VendorSearchModal
{
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.VendorSearchModal.VendorSearchCondition;

    public partial class VendorViewModel
    {
        /// <summary>
        /// 検索結果取得
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="pageSize">最大ページサイズ</param>
        /// <returns>検索結果</returns>
        public IPagedList<VendorViewModel> Listing(VendorSearchCondition conditions, int pageSize)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT * 
                  FROM M_VENDORS
                 WHERE SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            if (!string.IsNullOrWhiteSpace(conditions.VendorId))
            {
                sql.AppendLine(" AND VENDOR_ID LIKE :VENDOR_ID ");
                parameters.Add(":VENDOR_ID", "%" + conditions.VendorId + "%");
            }

            if (!string.IsNullOrWhiteSpace(conditions.VendorName))
            {
                sql.AppendLine(" AND VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                parameters.Add(":VENDOR_NAME1", "%" + conditions.VendorName + "%");
            }

            // 全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<VendorViewModel>(sql.ToString(), parameters).Count();

            switch (conditions.SortKey)
            {
                case SortKey1.Vendor:
                    sql.AppendLine(" ORDER BY ");
                    switch (conditions.OrderKey)
                    {
                        case SortKey2.Desc:
                            sql.AppendLine("    VENDOR_PREF_NAME DESC, VENDOR_CITY_NAME DESC, VENDOR_ADDRESS1 DESC, VENDOR_ADDRESS2 DESC, VENDOR_ADDRESS3 DESC, VENDOR_ID DESC ");
                            break;
                        default:
                            sql.AppendLine("    VENDOR_PREF_NAME ASC, VENDOR_CITY_NAME ASC, VENDOR_ADDRESS1 ASC, VENDOR_ADDRESS2 ASC, VENDOR_ADDRESS3 ASC, VENDOR_ID ASC ");
                            break;
                    }

                    break;
                default:
                    sql.AppendLine(" ORDER BY ");
                    switch (conditions.OrderKey)
                    {
                        case SortKey2.Desc:
                            sql.AppendLine("    VENDOR_ID DESC, VENDOR_PREF_NAME DESC, VENDOR_CITY_NAME DESC, VENDOR_ADDRESS1 DESC, VENDOR_ADDRESS2 DESC, VENDOR_ADDRESS3 DESC ");
                            break;
                        default:
                            sql.AppendLine("    VENDOR_ID ASC, VENDOR_PREF_NAME ASC, VENDOR_CITY_NAME ASC, VENDOR_ADDRESS1 ASC, VENDOR_ADDRESS2 ASC, VENDOR_ADDRESS3 ASC ");
                            break;
                    }

                    break;
            }

            sql.AppendLine("OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");

            parameters.AddDynamicParams(new { OFFSET = (conditions.Page - 1) * pageSize });
            parameters.AddDynamicParams(new { PAGE_SIZE = pageSize });
            var vendors =
                Wms.Models.MvcDbContext.Current.Database.Connection.Query<VendorViewModel, Models.Vendor, VendorViewModel>(
                    sql.ToString(),
                    (vm, vendor) =>
                    {
                        vm.Vendor = vendor;
                        return vm;
                    },
                    param: parameters,
                    splitOn: "VENDOR_ID")
                    .ToList();

            return new StaticPagedList<VendorViewModel>(vendors, (int)conditions.Page, pageSize, totalCount);
        }
    }
}