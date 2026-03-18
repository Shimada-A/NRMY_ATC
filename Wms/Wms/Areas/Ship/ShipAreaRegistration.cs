namespace Wms.Areas.Ship
{
    using System.Web.Mvc;

    public class ShipAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ship";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Ship_default",
                "Ship/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}