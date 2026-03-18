namespace Wms.Areas.Master.Controllers
{
    using System.Web.Mvc;
    using Wms.Areas.Master.ViewModels.ColorSearchModal;
    using Wms.Controllers;

    public class ColorSearchModalController : BaseController
    {
        public ActionResult Index()
        {
            var color = new ColorViewModel().Listing();
            return PartialView("_ColorSearchModalCore", color);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return PartialView("_CountrySearchModal", new ColorViewModel().Listing());
        }
    }
}