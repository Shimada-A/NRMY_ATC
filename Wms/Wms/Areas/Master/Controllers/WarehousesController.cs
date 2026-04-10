namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.Warehouses;
    using Wms.Controllers;
    using Wms.Resources;

    public class WarehousesController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASWarehouses01.SearchConditions";

        private Warehouses _WarehousesQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehousesController"/> class.
        /// </summary>
        public WarehousesController()
        {
            this._WarehousesQuery = new Warehouses();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search Warehouses
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// Search Warehouses
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
        /// <param name="SearchConditions"List Warehouses Information
        /// <returns></returns></param>
        /// <returns>List Record</returns>
        public ActionResult Search(WarehousesSearchCondition SearchConditions)
        {
            WarehousesSearchCondition condition;

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

        #endregion Search

        #region Edit

        /// <summary>
        /// Edit Warehouses Information
        /// </summary>
        /// <param name="countries">List Country</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(IList<SelectedWarehouseViewModel> Warehouses)
        {
            var Warehouse = Warehouses.Where(x => x.IsCheck == true).Single();

            // Get record from DB
            var target = _WarehousesQuery.GetTargetById(Warehouse.CenterId, Common.Profile.User.ShipperId);

            // 更新対象のデータがマスタに無い場合、エラー
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            return View("~/Areas/Master/Views/Warehouses/Edit.cshtml", target);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="warehouses">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Warehouses warehouses)
        {
            // 都道府県入力チェック
            if (!_WarehousesQuery.SelectPrefs(warehouses))
            {
                this.ModelState.AddModelError("CenterPrefName", MessageResource.ERR_NOT_PREFS);
            }

            // 郵便番号ハイフンチェック
            if (warehouses.CenterZip.Contains('-'))
            {
                this.ModelState.AddModelError("CenterZip", MessageResource.ERR_ZIP_HYPHEN);
            }

            // 電話番号重複チェック
            if (!_WarehousesQuery.SelectTel(warehouses))
            {
                this.ModelState.AddModelError("CenterTel", MessageResource.ERR_NOT_TEL);
            }

            if (ModelState.IsValid)
            {
                if (_WarehousesQuery.UpdateWarehouses(warehouses))
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

            return View("~/Areas/Master/Views/Warehouses/Edit.cshtml", warehouses);
        }

        #endregion Edit

        /// <summary>
        /// ダウンロード
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Download()
        {
            WarehousesSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.WarehousesReport report = new Reports.Export.WarehousesReport(ReportTypes.Excel, searchCondition);
            report.Export();

            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }


        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>WarehousesSearchCondition</returns>
        private WarehousesSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new WarehousesSearchCondition() : Request.Cookies.Get<WarehousesSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new WarehousesSearchCondition();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Warehouses Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(WarehousesSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                WarehouseResult = indexFlag ? new WarehousesResult() : new WarehousesResult()
                {
                    Warehouses = _WarehousesQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.WarehouseResult.Warehouses.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.WarehouseResult.Warehouses = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            return this.View("~/Areas/Master/Views/Warehouses/Index.cshtml", vm);
        }

        #endregion Private
    }
}