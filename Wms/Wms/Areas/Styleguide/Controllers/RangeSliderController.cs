namespace Wms.Areas.Styleguide.Controllers
{
    using System.Web.Mvc;

    public class RangeSliderController : Controller
    {
        // GET: Styleguide/RangeSlider
        public ActionResult Index()
        {
            var m = new Models.RangeSlider()
            {
                // SellingPriceStart = 10,
                // SellingPriceEnd = 90
            };

            return this.View(m);
        }
    }
}