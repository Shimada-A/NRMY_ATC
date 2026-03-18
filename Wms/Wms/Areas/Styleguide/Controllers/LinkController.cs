namespace Wms.Areas.Styleguide.Controllers
{
    using System.Web.Mvc;

    public class LinkController : Controller
    {
        // GET: Styleguide/Link
        public ActionResult Index()
        {
            return this.View();
        }

        // [HttpGet]
        // public ActionResult Edit()
        // {
        //    return View();
        // }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                // return HttpNotFound();
                this.ViewBag.id = "idは設定されていません。";
            }
            else
            {
                this.ViewBag.id = id;
            }

            return this.View();
        }
    }
}