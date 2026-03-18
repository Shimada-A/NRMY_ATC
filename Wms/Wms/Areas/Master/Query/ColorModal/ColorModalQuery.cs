namespace Wms.Areas.Master.ViewModels.ColorSearchModal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;

    public partial class ColorViewModel
    {
        public List<ColorViewModel> Listing()
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT *
                  FROM M_COLORS
                 WHERE SHIPPER_ID = :SHIPPER_ID
                 ORDER BY ITEM_COLOR_ID
            ");
            parameters.AddDynamicParams(new { SHIPPER_ID = Common.Profile.User.ShipperId });

            return
                Wms.Models.MvcDbContext.Current.Database.Connection.Query<ColorViewModel, Models.Color, ColorViewModel>(
                    sql.ToString(),
                    (vm, color) =>
                    {
                        vm.Color = color;
                        return vm;
                    },
                    param: parameters,
                    splitOn: "ITEM_COLOR_ID")
                    .ToList();
        }
    }
}