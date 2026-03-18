using Share.Common;
using Share.Extensions.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Wms.Areas.Arrival.Query.ConfirmActual;
using Wms.Areas.Arrival.Resources;
using Wms.Areas.Arrival.ViewModels.ConfirmActual;
using Wms.Areas.Arrival.ViewModels.PurchaseReference;
using Wms.Controllers;
using Wms.Models;
using Wms.Resources;

namespace Wms.Areas.Arrival.Controllers
{
    public class ConfirmActualController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-ARR_ConfirmActual.SearchConditions";
        private ConfirmActualQuery _ConfirmActualQuery;

        public object results { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmActualController"/> class.
        /// </summary>
        public ConfirmActualController()
        {
            this._ConfirmActualQuery = new ConfirmActualQuery();
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
        /// <param name="SearchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(ConfirmActualSearchConditions SearchConditions)
        {
            ConfirmActualSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion

        #region Selected

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(ConfirmActualSearchConditions searchConditions)
        {
            // 全選択
            _ConfirmActualQuery.ArrivalAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();

            var vm = new ConfirmActualViewModel
            {
                SearchConditions = searchConditions,
                Results = new ConfirmActualResult()
                {
                    ConfirmActuals = _ConfirmActualQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.ArrivalStatus = _ConfirmActualQuery.GetSelectListArrivalStatus();
            ViewBag.DivisionList = _ConfirmActualQuery.GetSelectListDivisions();
            ViewBag.Category1List = _ConfirmActualQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _ConfirmActualQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _ConfirmActualQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _ConfirmActualQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Arrival/Views/ConfirmActual/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(ConfirmActualSearchConditions searchConditions)
        {
            // 全解除
            _ConfirmActualQuery.ArrivalAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new ConfirmActualViewModel
            {
                SearchConditions = searchConditions,
                Results = new ConfirmActualResult()
                {
                    ConfirmActuals = _ConfirmActualQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.ArrivalStatus = _ConfirmActualQuery.GetSelectListArrivalStatus();
            ViewBag.DivisionList = _ConfirmActualQuery.GetSelectListDivisions();
            ViewBag.Category1List = _ConfirmActualQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _ConfirmActualQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _ConfirmActualQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _ConfirmActualQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Arrival/Views/ConfirmActual/Index.cshtml", vm);
        }

        #endregion Selected

        #region Jump

        /// <summary>
        /// Edit ConfirmActual Information
        /// </summary>
        /// <param name="ConfirmActuals">List ConfirmActual</param>
        public ActionResult Jump(ConfirmActualSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _ConfirmActualQuery.UpdateIsCheck(searchConditions.ConfirmActuals);

            var countriesSelected =
                MvcDbContext.Current.ArrConAct01s.Where(x => x.Seq == searchConditions.Seq && x.ShipperId == Common.Profile.User.ShipperId && x.IsCheck).FirstOrDefault();
            //ConfirmActuals
            //     .Where(x => x.IsCheck == true)
            //     .FirstOrDefault();
            PurchaseReference02SearchConditions vm = new PurchaseReference02SearchConditions();
            vm.CenterId = countriesSelected.CenterId;
            vm.ArrivePlanDateFrom = countriesSelected.ArrivePlanDate;
            vm.InvoiceNo = countriesSelected.InvoiceNo;
            vm.FromMenu = false;
            vm.ReturnDisp = "ConfirmActual";
            string path = this.Url.Action("DetailSearch", "PurchaseReference", new { area = "Arrival" });

            this.TempData["Conditions"] = vm;
            return this.Redirect(path);
        }

        #endregion Jump

        #region Update

        /// <summary>
        /// Edit ConfirmActual Information
        /// </summary>
        /// <param name="ConfirmActuals">List ConfirmActual</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateConfirmedArrival(ConfirmActualSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _ConfirmActualQuery.UpdateIsCheck(searchConditions.ConfirmActuals);

            // 実績更新
            var message = _ConfirmActualQuery.UpdateConfirmedArrival(searchConditions);
            if (string.IsNullOrWhiteSpace(message))
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = InputPurchaseResource.UpdateConfirmedSecMsg;

                var vm = new ConfirmActualViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new ConfirmActualResult()
                };

                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.ArrivalStatus = _ConfirmActualQuery.GetSelectListArrivalStatus();
                ViewBag.DivisionList = _ConfirmActualQuery.GetSelectListDivisions();
                ViewBag.Category1List = _ConfirmActualQuery.GetSelectListCategorys1();
                ViewBag.Category2List = _ConfirmActualQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
                ViewBag.Category3List = _ConfirmActualQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
                ViewBag.Category4List = _ConfirmActualQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

                // Return index view
                return this.View("~/Areas/Arrival/Views/ConfirmActual/Index.cshtml", vm);
            }
            else
            {
                TempData[AppConst.ERROR] = message;

                return RedirectToAction("IndexSearch");
            }
        }

        /// <summary>
        /// Edit ConfirmActual Information
        /// </summary>
        /// <param name="ConfirmActuals">List ConfirmActual</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateArrived(ConfirmActualSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _ConfirmActualQuery.UpdateIsCheck(searchConditions.ConfirmActuals);

            // 実績更新
            var message = _ConfirmActualQuery.UpdateArrived(searchConditions);
            if (string.IsNullOrWhiteSpace(message))
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = InputPurchaseResource.UpdateArrivedSecMsg;

                var vm = new ConfirmActualViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new ConfirmActualResult()
                };

                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.ArrivalStatus = _ConfirmActualQuery.GetSelectListArrivalStatus();
                ViewBag.DivisionList = _ConfirmActualQuery.GetSelectListDivisions();
                ViewBag.Category1List = _ConfirmActualQuery.GetSelectListCategorys1();
                ViewBag.Category2List = _ConfirmActualQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
                ViewBag.Category3List = _ConfirmActualQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
                ViewBag.Category4List = _ConfirmActualQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

                // Return index view
                return this.View("~/Areas/Arrival/Views/ConfirmActual/Index.cshtml", vm);

            }
            else
            {
                TempData[AppConst.ERROR] = message;

                return RedirectToAction("IndexSearch");
            }


        }

        #endregion Update

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private ConfirmActualSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new ConfirmActualSearchConditions() : Request.Cookies.Get<ConfirmActualSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new ConfirmActualSearchConditions();
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
        private ActionResult GetSearchResultView(ConfirmActualSearchConditions searchConditions, bool indexFlag)
        {
            // 画面選択行更新用
            if (!indexFlag && searchConditions.SearchType == Common.SearchTypes.SortPage)
            {
                _ConfirmActualQuery.UpdateIsCheck(searchConditions.ConfirmActuals);
            }

            // 作成処理&検索表示
            var vm = new ConfirmActualViewModel
            {
                SearchConditions = searchConditions,
                //Results = indexFlag ? new ConfirmActualResult() : new ConfirmActualResult()
                Results = indexFlag ? new ConfirmActualResult() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _ConfirmActualQuery.InsertArrConfirmActual01(searchConditions) : true) ? new ConfirmActualResult()
                {
                    ConfirmActuals = _ConfirmActualQuery.GetData(searchConditions)
                }
                : new ConfirmActualResult())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.ConfirmActuals.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.ConfirmActuals = null;
                }
            }

            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.ArrivalStatus = _ConfirmActualQuery.GetSelectListArrivalStatus();
            ViewBag.DivisionList = _ConfirmActualQuery.GetSelectListDivisions();
            ViewBag.Category1List = _ConfirmActualQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _ConfirmActualQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _ConfirmActualQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _ConfirmActualQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Arrival/Views/ConfirmActual/Index.cshtml", vm);
        }

        #endregion

    }
}