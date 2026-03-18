namespace Wms.Areas.Styleguide.Controllers
{
    using System.Web.Mvc;
    using Wms.Areas.Styleguide.Models;

    public class TextBoxController : Controller
    {
        // GET: Styleguide/TextBox
        public ActionResult Index()
        {
            return this.View(new TextBox());
        }

        public ActionResult Number()
        {
            return this.View(new TextBoxNumber());
        }
    }
}