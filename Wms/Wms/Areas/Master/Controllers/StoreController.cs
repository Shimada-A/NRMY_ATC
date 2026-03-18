namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Arrival.Query.ConfirmActual;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.Store;
    using Wms.Controllers;
    using Wms.Extensions.Classes;
    using Wms.Resources;

    public class StoreController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASStore01.SearchConditions";
        private Store _StoreQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreController"/> class.
        /// </summary>
        public StoreController()
        {
            this._StoreQuery = new Store();
        }

        #endregion

        #region Search

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult IndexSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            return this.GetSearchResultView(searchInfo, false);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(StoreSearchCondition SearchConditions)
        {
            StoreSearchCondition condition;

            // When page is clicked, page > 1
            if (SearchConditions.Page >= 1)
            {
                condition = this.GetPreviousSearchInfo(false);
                condition.Page = SearchConditions.Page;
            }
            else
            {
                condition = SearchConditions;
                condition.PageSize = this.GetCurrentPageSize();
                condition.Page = 1;
            }

            return this.GetSearchResultView(condition, false);
        }

        /// <summary>
        /// ダウンロード
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Download()
        {
            StoreSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.StoreReport report = new Reports.Export.StoreReport(ReportTypes.Excel, searchCondition);
            report.Export();

            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        #endregion

        #region Edit

        /// <summary>
        /// Edit Store Information
        /// </summary>
        /// <param name="countries">List Store</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(IList<SelectedStoreViewModel> stores)
        {
            var store = stores.Where(x => x.IsCheck == true).Single();

            // Get record from DB
            var target = _StoreQuery.GetTargetById(store.StoreId, Common.Profile.User.ShipperId);

            // 更新対象のデータがマスタに無い場合、エラー
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            ViewBag.PatternIdList = _StoreQuery.GetSelectListPatternId();

            return View("~/Areas/Master/Views/Store/Edit.cshtml", target);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Detail store)
        {

            // 都道府県入力チェック
            if (!_StoreQuery.SelectPrefs(store))
            {
                this.ModelState.AddModelError("StorePrefName", MessageResource.ERR_NOT_PREFS);
            }

            // 郵便番号ハイフンチェック
            if (store.StoreZip.Contains('-'))
            {
                this.ModelState.AddModelError("StoreZip", MessageResource.ERR_ZIP_HYPHEN);
            }

            if (ModelState.RemoveBase().IsValid)
            {
                if (_StoreQuery.UpdateStore(store))
                {
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
                    return RedirectToAction("IndexSearch");
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                    return RedirectToAction("IndexSearch");
                }
            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            ViewBag.PatternIdList = _StoreQuery.GetSelectListPatternId();

            return View("~/Areas/Master/Views/Store/Edit.cshtml", store);
        }

        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private StoreSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new StoreSearchCondition() : Request.Cookies.Get<StoreSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new StoreSearchCondition();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(StoreSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                StoreResult = indexFlag ? new StoreResult() : new StoreResult()
                {
                    Stores = _StoreQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.StoreResult.Stores.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.StoreResult.Stores = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            // return this.View("Index", vm);
            return this.View("~/Areas/Master/Views/Store/Index.cshtml", vm);
        }

        #endregion
    }
}