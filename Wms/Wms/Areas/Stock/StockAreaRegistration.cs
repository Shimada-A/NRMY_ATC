namespace Wms.Areas.Stock
{
    using System.Web.Mvc;

    public class StockAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Stock";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Stock_default",
                "Stock/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}