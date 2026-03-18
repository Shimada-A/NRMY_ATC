namespace Wms.Areas.Styleguide
{
    using System.Web.Mvc;

    public class StyleguideAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Styleguide";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
#if DEBUG
            context.MapRoute(
                "Styleguide_default",
                "Styleguide/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
#endif
        }
    }
}