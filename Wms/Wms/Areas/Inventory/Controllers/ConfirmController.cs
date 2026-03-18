namespace Wms.Areas.Inventory.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Inventory.Query.Confirm;
    using Wms.Areas.Inventory.Resources;
    using Wms.Areas.Inventory.ViewModels.Confirm;
    using Wms.Common;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class ConfirmController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-INV_Confirm.SearchConditions";

        private ConfirmQuery _ConfirmQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmController"/> class.
        /// </summary>
        public ConfirmController()
        {
            this._ConfirmQuery = new ConfirmQuery();
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
        /// <param name="SearchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(ConfirmSearchConditions SearchConditions)
        {
            ConfirmSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private ConfirmSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new ConfirmSearchConditions() : Request.Cookies.Get<ConfirmSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new ConfirmSearchConditions();
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
        private ActionResult GetSearchResultView(ConfirmSearchConditions searchConditions, bool indexFlag)
        {

            // 作成処理&検索表示
            var vm = new ConfirmViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new ConfirmResult() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _ConfirmQuery.InsertInvConfirmSc01(searchConditions) : true) ? new ConfirmResult()
                {
                    Confirms = _ConfirmQuery.GetData(searchConditions)
                }
                : new ConfirmResult()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.Confirms.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.Confirms = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.InventoryConfirmFlag = _ConfirmQuery.GetSelectListInventoryConfirmFlag();
            ViewBag.InventoryNo = _ConfirmQuery.GetSelectListInventoryNo(searchConditions);

            // Return index view
            return this.View("~/Areas/Inventory/Views/Confirm/Index.cshtml", vm);

        }

        #endregion Private

        #region 本確定

        /// <summary>
        /// 本確定
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <returns>Update</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvConfirm(ConfirmSearchConditions searchConditions)
        {
            ModelState.Clear();

            searchConditions.PageSize = this.GetCurrentPageSize();

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            // 画面選択行更新用
            _ConfirmQuery.UpdateInvConfirm(searchConditions.Seq, searchConditions.LineNo);

            var Confirm = MvcDbContext.Current.InventoryConfirms
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == searchConditions.Seq && m.IsCheck)
                                  .SingleOrDefault();

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _ConfirmQuery.InventoryConfirm(searchConditions.CenterId, Confirm.InventoryNo, out status, out message);
            if (status == ProcedureStatus.NoAllocData)
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;

                var vm = new ConfirmViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new ConfirmResult()
                    {
                        Confirms = _ConfirmQuery.GetData(searchConditions)
                    },

                    // Page = searchConditions.Page
                };
                ViewBag.InventoryConfirmFlag = _ConfirmQuery.GetSelectListInventoryConfirmFlag();
                ViewBag.InventoryNo = _ConfirmQuery.GetSelectListInventoryNo(searchConditions);

                return this.View("~/Areas/Inventory/Views/Confirm/Index.cshtml", vm);

            }
            else if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = string.Format(ConfirmResource.SUC_CONFORM);

                // Return index view
                return RedirectToAction("IndexSearch");
            }
            else
            {
                TempData[AppConst.ERROR] = message;

                // Return index view
                return RedirectToAction("IndexSearch");
            }
        }

        /// <summary>
        /// もう一度本確定
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <returns>Update</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvAgainConfirm(ConfirmSearchConditions searchConditions)
        {
            ModelState.Clear();

            searchConditions.PageSize = this.GetCurrentPageSize();

            var Confirm = MvcDbContext.Current.InventoryConfirms
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == searchConditions.Seq && m.IsCheck)
                                  .SingleOrDefault();

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _ConfirmQuery.InventoryAgainConfirm(searchConditions.CenterId, Confirm.InventoryNo, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = string.Format(ConfirmResource.SUC_CONFORM);

                // Return index view
                return IndexSearch();
            }
            else
            {
                TempData[AppConst.ERROR] = message;

                // Return index view
                return RedirectToAction("IndexSearch");
            }
        }

        #endregion 本確定

        #region GetList

        /// <summary>
        /// Get コードより名称を取得
        /// <param name="kbn">マスタ区分</param>
        /// <param name="cd">検索用コード</param>
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult GetInventoryNoList(string InventoryDateFrom, string InventoryDateTo, string InventoryStatus, string CenterId)
        {
            string _html = "<option value=''>" + Wms.Resources.CommonResource.None + "</option>";

            var listInventoryNo = _ConfirmQuery.GetInventoryNoList(InventoryDateFrom, InventoryDateTo, InventoryStatus, CenterId);
            foreach (var inventoryNo in listInventoryNo)
            {
                _html = _html + "<option value='" + inventoryNo.Value + "'>" + inventoryNo.Text + "</option>";
            }

            return this.Json(new { html = _html });
        }

        #endregion

    }
}