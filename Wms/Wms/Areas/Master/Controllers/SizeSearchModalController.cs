namespace Wms.Areas.Master.Controllers
{
    using System.Web.Mvc;
    using Wms.Areas.Master.ViewModels.SizeSearchModal;
    using Wms.Controllers;

    public class SizeSearchModalController : BaseController
    {
        public ActionResult Index()
        {
            var size = new SizeViewModel().Listing();
            return PartialView("_SizeSearchModalCore", size);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return PartialView("_CountrySearchModal", new SizeViewModel().Listing());
        }
    }
}