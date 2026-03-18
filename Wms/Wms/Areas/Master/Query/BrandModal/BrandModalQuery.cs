namespace Wms.Areas.Master.ViewModels.BrandSearchModal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;

    public partial class BrandViewModel
    {
        public List<BrandViewModel> Listing()
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT *
                  FROM M_BRANDS
                 WHERE SHIPPER_ID = :SHIPPER_ID
                 ORDER BY BRAND_ID
            ");
            parameters.AddDynamicParams(new { SHIPPER_ID = Common.Profile.User.ShipperId });

            return
                Wms.Models.MvcDbContext.Current.Database.Connection.Query<BrandViewModel, Models.Brand, BrandViewModel>(
                    sql.ToString(),
                    (vm, brand) =>
                    {
                        vm.Brand = brand;
                        return vm;
                    },
                    param: parameters,
                    splitOn: "BRAND_ID")
                    .ToList();
        }
    }
}