namespace Wms.Areas.Master.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Wms.Areas.Master.ViewModels.DeliareaGroupSearchModal;
    using Wms.Controllers;

    public class DeliareaGroupSearchModalController : BaseController
    {
        public ActionResult Index(string ParameterDeliareaGroup)
        {
            var brand = new DeliareaGroupViewModel().Listing(); 
            var deliareaGroups = string.IsNullOrWhiteSpace(ParameterDeliareaGroup) ? null : ParameterDeliareaGroup.Split(',');
            brand = brand.Select(x => { x.IsCheck = (deliareaGroups == null || string.IsNullOrWhiteSpace(x.PrefId)) ? false :(deliareaGroups.Intersect(x.PrefId.Split(',')).ToArray().Length > 0 ? true : false); return x; }).ToList();
            return PartialView("_DeliareaGroupSearchModalCore", brand);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return PartialView("_CountrySearchModal", new DeliareaGroupViewModel().Listing());
        }
    }
}