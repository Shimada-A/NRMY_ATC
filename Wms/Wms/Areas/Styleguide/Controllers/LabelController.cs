namespace Wms.Areas.Styleguide.Controllers
{
    using System.Web.Mvc;

    public class LabelController : Controller
    {
        // GET: Styleguide/Label
        public ActionResult Index()
        {
            var m = new Models.Label();
            return this.View(m);
        }
    }
}