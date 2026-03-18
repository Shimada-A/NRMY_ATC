namespace Wms.Areas.Master.Controllers
{
    using System.Web.Mvc;
    using Wms.Areas.Master.ViewModels.BrandSearchModal;
    using Wms.Controllers;

    public class BrandSearchModalController : BaseController
    {
        public ActionResult Index()
        {
            var brand = new BrandViewModel().Listing();
            return PartialView("_BrandSearchModalCore", brand);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return PartialView("_CountrySearchModal", new BrandViewModel().Listing());
        }
    }
}