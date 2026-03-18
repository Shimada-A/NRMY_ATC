namespace Wms.Areas.Master.Controllers
{
    using Share.Common;
    using System.Web.Mvc;
    using Wms.Areas.Master.ViewModels.TransporterSearchModal;
    using Wms.Controllers;
    using Wms.Resources;

    /// <summary>
    /// 配送業者検索子画面
    /// </summary>
    /// <remarks>検索子画面のため認可検証（MenuAuthorization）はしない</remarks>
    [AllowAnonymous]
    public class TransporterSearchModalController : BaseController
    {
        /// <summary>
        /// 仕入先検索結果取得
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult Index([Bind(Include = "TransporterId,TransporterName,SortKey,OrderKey,Page,ParameterTransporterId")] TransporterSearchCondition condition)
        {
            var Transporters = new TransporterViewModel().Listing(condition, GetCurrentPageSize());
            condition.TransporterViewModel = Transporters;

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && condition != null)
            {
                if (condition.TransporterViewModel.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    condition.TransporterViewModel = null;
                }
            }

            ModelState.Clear();
            return PartialView("_TransporterSearchModalCore", condition);
        }
    }
}