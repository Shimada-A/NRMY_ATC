namespace Wms.Areas.Master.Controllers
{
    using Share.Common;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Wms.Areas.Master.ViewModels.SyukkasakiTransporterSearchModal;
    using Wms.Controllers;
    using Wms.Resources;

    /// <summary>
    /// 配送業者検索子画面
    /// </summary>
    /// <remarks>検索子画面のため認可検証（MenuAuthorization）はしない</remarks>
    [AllowAnonymous]
    public class SyukkasakiTransporterSearchModalController : BaseController
    {
        /// <summary>
        /// 仕入先検索結果取得
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult Index([Bind(Include = "ShipToStoreId, ShipToStoreName, StoreClass, StoreRanks, AreaItem, SortKey, OrderKey, Page, CenterId, CenterName,TransporterId, IsCenterOnly, BrandId, BrandName, StoreOutletsClass")] SyukkasakiTransporterSearchCondition condition)
        {
            List<SelectListItem> temp = new List<SelectListItem>();
            IEnumerable<SelectListItem> qlist = new SyukkasakiTransporterViewModel().GetAreaList(true);

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
            var SyukkasakiTransporter = new SyukkasakiTransporterViewModel().Listing(condition, GetCurrentPageSize());
            ViewBag.TransporterList = new SyukkasakiTransporterViewModel().GetSelectListTransporters();
            condition.SyukkasakiTransporterViewModel = SyukkasakiTransporter;

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && condition != null)
            {
                if (condition.SyukkasakiTransporterViewModel.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    condition.SyukkasakiTransporterViewModel = null;
                }
            }

            ModelState.Clear();
            return PartialView("_SyukkasakiTransporterSearchModalCore", condition);
        }
    }
}