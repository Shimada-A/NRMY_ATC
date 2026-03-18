namespace Wms.Areas.Ship.Controllers
{
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.EcAllocationCancel;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.EcAllocationCancel;
    using Wms.Controllers;
    using Wms.Resources;

    public class EcAllocationCancelController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_EcAllocationCancel.SearchConditions";

        private EcAllocationCancelQuery _EcAllocationCancelQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipFrontageController"/> class.
        /// </summary>
        public EcAllocationCancelController()
        {
            this._EcAllocationCancelQuery = new EcAllocationCancelQuery();
        }

        #endregion Constants

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
        public ActionResult Search(EcAllocationCancelSearchConditions SearchConditions)
        {
            EcAllocationCancelSearchConditions condition;

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

        #region 更新処理

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="searchConditions">searchConditions</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelSearch(EcAllocationCancelSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            searchConditions.Page = 1;

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _EcAllocationCancelQuery.EcAllocationCancel(searchConditions, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = message;
                // 検索部を表示
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;
                // 検索
                var vm = new EcAllocationCancelViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new EcAllocationCancelResult()
                    {
                        EcAllocationCancels = _EcAllocationCancelQuery.GetData(searchConditions)
                    },
                };
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.BatchNoList = _EcAllocationCancelQuery.GetSelectListBatchNos(searchConditions.CenterId);
                // Return index view
                return this.View("~/Areas/Ship/Views/EcAllocationCancel/Index.cshtml", vm);
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private EcAllocationCancelSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new EcAllocationCancelSearchConditions() : Request.Cookies.Get<EcAllocationCancelSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new EcAllocationCancelSearchConditions();
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
        private ActionResult GetSearchResultView(EcAllocationCancelSearchConditions condition, bool indexFlag)
        {
            // Save search info
            var vm = new EcAllocationCancelViewModel
            {
                SearchConditions = condition,
                Results = indexFlag ? new EcAllocationCancelResult() : new EcAllocationCancelResult()
                {
                    EcAllocationCancels = _EcAllocationCancelQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.EcAllocationCancels.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.EcAllocationCancels = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
            ViewBag.BatchNoList = _EcAllocationCancelQuery.GetSelectListBatchNos(condition.CenterId);

            // Return index view
            return this.View("~/Areas/Ship/Views/EcAllocationCancel/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion Private

        #region GetList

        /// <summary>
        /// Get コードより名称を取得
        /// <param name="kbn">マスタ区分</param>
        /// <param name="cd">検索用コード</param>
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult GetBatchNoList(string centerId)
        {
            string _html = "<option value=''>" + Wms.Resources.CommonResource.None + "</option>";

            var listBatchNo = _EcAllocationCancelQuery.GetSelectListBatchNos(centerId);
            foreach (var batchNo in listBatchNo)
            {
                _html = _html + "<option value='" + batchNo.Value + "'>" + batchNo.Text + "</option>";
            }

            return this.Json(new { html = _html });
        }
        #endregion
    }
}