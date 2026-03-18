namespace Wms.Areas.Master.Controllers
{
    using Share.Common;
    using System.Web.Mvc;
    using Wms.Areas.Master.ViewModels.VendorSearchModal;
    using Wms.Controllers;
    using Wms.Resources;

    /// <summary>
    /// 仕入先検索子画面
    /// </summary>
    /// <remarks>検索子画面のため認可検証（MenuAuthorization）はしない</remarks>
    [AllowAnonymous]
    public class VendorSearchModalController : BaseController
    {
        /// <summary>
        /// 仕入先検索結果取得
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult Index([Bind(Include = "VendorId,VendorName,SortKey,OrderKey,Page")] VendorSearchCondition condition)
        {
            var vendors = new VendorViewModel().Listing(condition, GetCurrentPageSize());
            condition.VendorViewModel = vendors;

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && condition != null)
            {
                if (condition.VendorViewModel.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    condition.VendorViewModel = null;
                }
            }

            ModelState.Clear();
            return PartialView("_VendorSearchModalCore", condition);
        }
    }
}