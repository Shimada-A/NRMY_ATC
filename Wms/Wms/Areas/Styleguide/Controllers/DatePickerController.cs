namespace Wms.Areas.Styleguide.Controllers
{
    using System.Web.Mvc;

    public class DatePickerController : Controller
    {
        // GET: Styleguide/DatePicker
        public ActionResult Index()
        {
            var v = new Models.DatePicker();
            return this.View(v);
        }
    }
}