namespace Wms.Areas.Styleguide.Controllers
{
    using System.Web.Mvc;

    public class StringFormatController : Controller
    {
        // GET: Styleguide/StringFormat
        public ActionResult DateTime()
        {
            // セッションのテスト
            if (this.Session["DateTime"] == null)
                this.Session["DateTime"] = System.DateTime.Now;

            this.TempData["DateTime"] = this.Session["DateTime"];
            return this.View();
        }

        public ActionResult Number()
        {
            return this.View();
        }
    }
}