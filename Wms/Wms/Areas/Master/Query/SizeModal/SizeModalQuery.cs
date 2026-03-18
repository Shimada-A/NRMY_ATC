namespace Wms.Areas.Master.ViewModels.SizeSearchModal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;

    public partial class SizeViewModel
    {
        public List<SizeViewModel> Listing()
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT *
                  FROM M_SIZES
                 WHERE SHIPPER_ID = :SHIPPER_ID
                 ORDER BY ITEM_SIZE_ID
            ");
            parameters.AddDynamicParams(new { SHIPPER_ID = Common.Profile.User.ShipperId });

            return
                Wms.Models.MvcDbContext.Current.Database.Connection.Query<SizeViewModel, Models.Size, SizeViewModel>(
                    sql.ToString(),
                    (vm, size) =>
                    {
                        vm.Size = size;
                        return vm;
                    },
                    param: parameters,
                    splitOn: "ITEM_SIZE_ID")
                    .ToList();
        }
    }
}