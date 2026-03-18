namespace Wms.Areas.Arrival.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Arrival.Query.InputPurchase;
    using Wms.Areas.Arrival.Resources;
    using Wms.Areas.Arrival.ViewModels.InputPurchase;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class InputPurchaseController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-ARR_InputPurchase01.SearchConditions";
        private const string COOKIE_SEARCHCONDITIONS02 = "W-ARR_InputPurchase02.SearchConditions";

        private InputPurchaseQuery _InputPurchaseQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputPurchaseController"/> class.
        /// </summary>
        public InputPurchaseController()
        {
            this._InputPurchaseQuery = new InputPurchaseQuery();
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
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(InputPurchase01SearchConditions SearchConditions)
        {
            InputPurchase01SearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private InputPurchase01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new InputPurchase01SearchConditions() : Request.Cookies.Get<InputPurchase01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new InputPurchase01SearchConditions();
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
        private ActionResult GetSearchResultView(InputPurchase01SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new InputPurchase01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new InputPurchase01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _InputPurchaseQuery.InsertArrInputPurchase01(searchConditions) : true) ? new InputPurchase01Result()
                {
                    InputPurchase01s = _InputPurchaseQuery.InputPurchase01GetData(searchConditions)
                }
                : new InputPurchase01Result()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.InputPurchase01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.InputPurchase01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.ArriveStatusList = _InputPurchaseQuery.GetSelectListArriveStatus();
            ViewBag.DivisionList = _InputPurchaseQuery.GetSelectListDivisions();
            ViewBag.Category1List = _InputPurchaseQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _InputPurchaseQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _InputPurchaseQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _InputPurchaseQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Arrival/Views/InputPurchase/Index.cshtml", vm);
        }

        #endregion Private

        #region 入力

        /// <summary>
        /// 入力
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Input(IList<SelectedInputPurchase01ViewModel> inputPurchase01s)
        {
            var inputPurchase01 = new SelectedInputPurchase01ViewModel();
            // データを取得
            SelectedInputPurchase01ViewModel selected = new SelectedInputPurchase01ViewModel();
            selected = (SelectedInputPurchase01ViewModel)this.TempData["Conditions"];
            if (selected != null)
            {
                inputPurchase01 = selected;
            }
            else
            {
                if (inputPurchase01s.Where(x => x.IsCheck == true).Count() != 1)
                {
                    // TODO:エラーメッセージをリソースファイルに
                    TempData[AppConst.ERROR] = MessagesResource.SELECT_ONE_RECORD;
                    return RedirectToAction("IndexSearch");
                }

                inputPurchase01 = inputPurchase01s.Where(x => x.IsCheck == true).Single();
            }

            // 仕入入荷進捗画面から遷移してきた場合、true
            bool fromPurchaseReference = inputPurchase01.Seq == 0 && inputPurchase01.LineNo == 0 ? true : false;

            //仕入入荷実績入力画面ワーク02作成処理
            InputPurchase02ViewModel vm = new InputPurchase02ViewModel();
            vm.SearchConditions = _InputPurchaseQuery.InsertArrInputPurchase02(inputPurchase01, fromPurchaseReference);

            // Get record from DB
            vm.Results.InputPurchase02s = _InputPurchaseQuery.InputPurchase02GetData(vm.SearchConditions);

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS02, vm.SearchConditions);
            return View("~/Areas/Arrival/Views/InputPurchase/Input.cshtml", vm);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(InputPurchase02Result results, int isConfirmed)
        {
            InputPurchase02ViewModel vm = new InputPurchase02ViewModel();
            vm.SearchConditions = Request.Cookies.Get<InputPurchase02SearchConditions>(COOKIE_SEARCHCONDITIONS02) ?? new InputPurchase02SearchConditions();
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                // ワーク02更新
                if (_InputPurchaseQuery.UpdateArrInputPurchase02(results.InputPurchase02s))
                {
                    // 実績更新
                    var message = string.Empty;
                    ProcedureStatus status = ProcedureStatus.Success;
                    _InputPurchaseQuery.UpdateInputPurchase(results, (int)isConfirmed, out status, out message);

                    if (status == ProcedureStatus.ConfirmLoss)
                    {
                        ViewBag.Status = status;
                        ViewBag.ErrorMessage = message;
                        vm.Results.InputPurchase02s = _InputPurchaseQuery.InputPurchase02GetData(vm.SearchConditions);
                        return View("~/Areas/Arrival/Views/InputPurchase/Input.cshtml", vm);
                    }
                    else if (status == ProcedureStatus.Success)
                    {
                        // Clear message to back to index screen
                        TempData[AppConst.SUCCESS] = InputPurchaseResource.SUC_UPDATE;

                        //仕入入荷実績入力画面ワーク02作成処理（更新後のデータでもう一度作成する）
                        var inputPurchase01 = new SelectedInputPurchase01ViewModel()
                        {
                            ShipperId = Common.Profile.User.ShipperId,
                            CenterId = vm.SearchConditions.CenterId,
                            ArrivePlanDate = vm.SearchConditions.ArrivePlanDate,
                            VendorId = vm.SearchConditions.VendorId,
                            VendorName = vm.SearchConditions.VendorName,
                            InvoiceNo = vm.SearchConditions.InvoiceNo,
                            PoId = vm.SearchConditions.PoId
                        };
                        var fromPurchaseReference = vm.SearchConditions.FromPurchaseReference;
                        vm.SearchConditions = _InputPurchaseQuery.InsertArrInputPurchase02(inputPurchase01, fromPurchaseReference);

                        vm.Results.InputPurchase02s = _InputPurchaseQuery.InputPurchase02GetData(vm.SearchConditions);
                        CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS02, vm.SearchConditions);
                        return View("~/Areas/Arrival/Views/InputPurchase/Input.cshtml", vm);
                    }
                    else
                    {
                        TempData[AppConst.ERROR] = message;
                        vm.Results.InputPurchase02s = results.InputPurchase02s;
                        return View("~/Areas/Arrival/Views/InputPurchase/Input.cshtml", vm);
                    }
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                    vm.Results.InputPurchase02s = results.InputPurchase02s;
                    return View("~/Areas/Arrival/Views/InputPurchase/Input.cshtml", vm);
                }

            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            vm.Results.InputPurchase02s = results.InputPurchase02s;
            return View("~/Areas/Arrival/Views/InputPurchase/Input.cshtml", vm);
        }

        #endregion 入力
    }
}