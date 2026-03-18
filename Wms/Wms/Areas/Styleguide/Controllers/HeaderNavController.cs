namespace Wms.Areas.Styleguide.Controllers
{
    using System.Web.Mvc;

    public class HeaderNavController : Controller
    {
        // GET: Styleguide/HeaderNav
        public ActionResult Index()
        {
            return this.View();
        }
    }
}