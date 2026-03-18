namespace Wms.Areas.Returns
{
    using System.Web.Mvc;

    public class ReturnsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Returns";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Returns_default",
                "Returns/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}