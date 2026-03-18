namespace Wms.Areas.Master.Controllers
{
    using Share.Common;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Wms.Areas.Master.ViewModels.SyukkasakiSearchModal;
    using Wms.Controllers;
    using Wms.Resources;

    /// <summary>
    /// 仕入先検索子画面
    /// </summary>
    /// <remarks>検索子画面のため認可検証（MenuAuthorization）はしない</remarks>
    [AllowAnonymous]
    public class SyukkasakiSearchModalController : BaseController
    {
        /// <summary>
        /// 仕入先検索結果取得
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <returns></returns>
        // [HttpPost]
        public ActionResult Index([Bind(Include = "StoreId, StoreName, StoreClass, SortKey, Sort, Page, AreaItem,ParameterStoreId,TempStoreId")] SyukkasakiSearchCondition condition)
        {
            SyukkasakiModalQuery query = new SyukkasakiModalQuery();
            List<SelectListItem> temp = new List<SelectListItem>();
            IEnumerable<SelectListItem> qlist = query.GetAreaList();

            temp.AddRange(qlist);

            if (condition.AreaItem != null)
            {
                int idx = 0;
                foreach (var item in condition.AreaItem)
                {
                    if (!item.IsCheck)
                    {
                        temp[idx].Selected = false;
                    }

                    idx++;
                }
            }
            else
            {
                condition.AreaItem = new List<AreaItem>();
                foreach (var item in temp)
                {
                    AreaItem area = new AreaItem();
                    area.AreaId = item.Value;
                    area.IsCheck = item.Selected;
                    condition.AreaItem.Add(area);
                }
            }

            ViewBag.PopAreaList = temp;

            var vendors = query.Listing(condition, GetCurrentPageSize());
            condition.SyukkasakiViewModel = vendors;

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && condition != null)
            {
                if (condition.SyukkasakiViewModel.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    condition.SyukkasakiViewModel = null;
                }
            }

            ModelState.Clear();
            return PartialView("_SyukkasakiSearchModalCore", condition);
        }

        public JsonResult GetPKey(SyukkasakiSearchCondition condition)
        {
            SyukkasakiModalQuery query = new SyukkasakiModalQuery();
            List<string> pkeys = query.GetPKey(condition);


            return Json(pkeys, JsonRequestBehavior.AllowGet);
        }
    }
}