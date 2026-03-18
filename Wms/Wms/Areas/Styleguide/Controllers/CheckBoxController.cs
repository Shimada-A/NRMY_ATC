namespace Wms.Areas.Styleguide.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.UI;

    public class CheckBoxController : Controller
    {
        // GET: Styleguide/CheckBox
        public ActionResult Index()
        {
            List<Models.CheckList> sample = new List<Models.CheckList>
            {
                new Models.CheckList { IsCheck = false, Name = "Name1" },
                new Models.CheckList { IsCheck = true, Name = "Name2" },
            };
            return this.View(sample);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult Edit(IList<Models.CheckList> list)
        {
            // listに値が入っている。

            // サンプルのため同じデータを再表示する
            List<Models.CheckList> sample = new List<Models.CheckList>
            {
                new Models.CheckList { IsCheck = true, Name = "Name1" },
                new Models.CheckList { IsCheck = false, Name = "Name2" },
            };
            return this.View("Index", sample);
        }
    }
}