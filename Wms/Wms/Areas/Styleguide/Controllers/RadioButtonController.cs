namespace Wms.Areas.Styleguide.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class RadioButtonController : Controller
    {
        // GET: Styleguide/RadioButton
        public ActionResult Index()
        {
            var model = new Models.RadioButton
            {
                SampleRadio1 = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "青", Value = "Green", Selected = false },
                new SelectListItem() { Text = "黄", Value = "Yellow", Selected = true },
                new SelectListItem() { Text = "赤", Value = "Red", Selected = false },
            },

                SampleRadio2 = Models.RadioButton.SampleEnum.Apple
            };
            return this.View(model);
        }
    }
}