namespace Wms.Areas.Move.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Ajax.Utilities;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Move.Query.InputTransfer;
    using Wms.Areas.Move.Resources;
    using Wms.Areas.Move.ViewModels.InputTransfer;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;
    using static Wms.Areas.Move.ViewModels.InputTransfer.InputTransfer01SearchConditions;
    using static Wms.Areas.Move.ViewModels.InputTransfer.PrintCaseLabelConditions;

    public class InputTransferController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-MOV_InputTransfer01.SearchConditions";
        private const string COOKIE_SEARCHCONDITIONS02 = "W-MOV_InputTransfer02.SearchConditions";
        private const string COOKIE_SEARCHCONDITIONS03 = "W-MOV_TransferInput02.SearchConditions";

        private InputTransferQuery _InputTransferQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputTransferController"/> class.
        /// </summary>
        public InputTransferController()
        {
            this._InputTransferQuery = new InputTransferQuery();
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
        public ActionResult Search(InputTransfer01SearchConditions SearchConditions)
        {
            InputTransfer01SearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Selected

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(InputTransfer01SearchConditions searchConditions)
        {
            // 全選択
            _InputTransferQuery.ArrivalAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();

            var vm = new InputTransfer01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new InputTransfer01Result()
                {
                    InputTransfer01s = _InputTransferQuery.InputTransfer01GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.CenterList = _InputTransferQuery.GetSelectListCenters();
            ViewBag.ItemList = _InputTransferQuery.GetSelectListItems();
            ViewBag.TransferStatusList = _InputTransferQuery.GetSelectListTransferStatus();
            ViewBag.DivisionList = _InputTransferQuery.GetSelectListDivisions();
            ViewBag.Category1List = _InputTransferQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _InputTransferQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _InputTransferQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _InputTransferQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Move/Views/InputTransfer/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(InputTransfer01SearchConditions searchConditions)
        {
            // 全解除
            _InputTransferQuery.ArrivalAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new InputTransfer01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new InputTransfer01Result()
                {
                    InputTransfer01s = _InputTransferQuery.InputTransfer01GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.CenterList = _InputTransferQuery.GetSelectListCenters();
            ViewBag.ItemList = _InputTransferQuery.GetSelectListItems();
            ViewBag.TransferStatusList = _InputTransferQuery.GetSelectListTransferStatus();
            ViewBag.DivisionList = _InputTransferQuery.GetSelectListDivisions();
            ViewBag.Category1List = _InputTransferQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _InputTransferQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _InputTransferQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _InputTransferQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Move/Views/InputTransfer/Index.cshtml", vm);
        }

        #endregion Selected

        #region Input

        /// <summary>
        /// Input
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Input()
        {
            var searchInfo = this.GetPreviousSearchInfo02(true);
            return this.GetSearchResultView02(searchInfo, true);
        }

        /// <summary>
        /// Index => Input
        /// </summary>
        /// <param name="InputTransfer01s">List Country Information</param>
        /// <param name="searchConditions">searchConditions</param>
        /// <returns>List Record</returns>
        public ActionResult InputIndexSearch(InputTransfer01SearchConditions SearchConditions, IList<SelectedInputTransfer01ViewModel> InputTransfer01s)
        {
            this.ModelState.Clear();
            // 仕入先別から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            var InputTransfer02 = new InputTransfer02SearchConditions();
            var InputTransfer01 = new SelectedInputTransfer01ViewModel();

            if (SearchConditions.InputTransfer01s.Where(x => x.IsCheck == true).Count() != 1)
            {
                // TODO:エラーメッセージをリソースファイルに
                TempData[AppConst.ERROR] = MessagesResource.SELECT_ONE_RECORD;
                return RedirectToAction("IndexSearch");
            }

            var checkedLineNo = SearchConditions.InputTransfer01s.Where(x => x.IsCheck == true).Select(x => x.LineNo).Single();

            InputTransfer01 = InputTransfer01s.Where(x => x.LineNo == checkedLineNo).Single();

            InputTransfer02.CenterId = SearchConditions.CenterId;
            InputTransfer02.DenpyoNo = InputTransfer01.SlipNo;
            //InputTransfer02.SlipNo = InputTransfer01.BoxNo;
            //InputTransfer02.BrandId = InputTransfer01.BrandId;

            switch (InputTransfer01.TransferClass)
            {
                case "1":
                    InputTransfer02.BaseMoveFlag = true;
                    InputTransfer02.StoreReturnFlag = false;
                    InputTransfer02.BaseMoveNoWmsCenterFlag = false;
                    break;
                case "2":
                    InputTransfer02.BaseMoveFlag = false;
                    InputTransfer02.StoreReturnFlag = true;
                    InputTransfer02.BaseMoveNoWmsCenterFlag = false;
                    break;
                case "3":
                    InputTransfer02.BaseMoveFlag = false;
                    InputTransfer02.StoreReturnFlag = false;
                    InputTransfer02.BaseMoveNoWmsCenterFlag = true;
                    break;
            }

            if (InputTransfer01.UnplannedFlag == 0)
            {
                InputTransfer02.ArriveDateClass = ArriveDateClasses.ArrivePlanDate;
                InputTransfer02.DenpyoDateFrom = InputTransfer01.SlipDate;
                InputTransfer02.DenpyoDateTo = InputTransfer01.SlipDate;
            }
            else
            {
                InputTransfer02.ArriveDateClass = ArriveDateClasses.TransferResultDate;
                InputTransfer02.TransferResultDateFrom = InputTransfer01.ArrivePlanDate;
                InputTransfer02.TransferResultDateTo = InputTransfer01.ArrivePlanDate;
            }
            if (InputTransfer02.StoreReturnFlag || InputTransfer02.BaseMoveNoWmsCenterFlag)
            {
                InputTransfer02.TransferFromStoreId = InputTransfer01.TransferFromStoreId;
                InputTransfer02.TransferFromStoreName = InputTransfer01.TransferFromStoreName;
            }
            if(InputTransfer02.BaseMoveFlag)
            {
                InputTransfer02.TransferFromCenterId = InputTransfer01.TransferFromCenterId;
            }
            InputTransfer02.TransferStatus = SearchConditions.TransferStatus;
            InputTransfer02.FromMenu = true;
            return InputSearch(InputTransfer02);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="SearchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult InputSearch(InputTransfer02SearchConditions SearchConditions)
        {
            var condition = new InputTransfer02SearchConditions();
            InputTransfer02SearchConditions selected = new InputTransfer02SearchConditions();
            selected = (InputTransfer02SearchConditions)this.TempData["Conditions"];
            if (selected != null)
            {
                condition = selected;
            }
            else
            {
                condition = SearchConditions;
            }
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView02(condition, false);  
        }

        /// <summary>
        /// ページ遷移
        /// </summary>
        /// <param name="results">Results Information</param>
        /// <param name="selectedPage"></param>
        /// <returns>Update</returns>
        public ActionResult UpdateWorkSearch(InputTransfer02ResultReceipt Results,int selectedPage)
        {
            ModelState.Clear();
            //ワーク更新
            _InputTransferQuery.UpdateArrInputTransfer02(Results.InputTransfer02s);

            var condition = new InputTransfer02SearchConditions();
            condition = GetPreviousSearchInfo02(false);
            condition.Page = selectedPage;
            condition.SearchType = Common.SearchTypes.SortPage;
            return InputSearch(condition);
        }

        #endregion Input

        #region Accept

        /// <summary>
        /// 受入登録
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <param name="InputTransfer01s"></param>
        /// <param name="allowWorkingCreate"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAcceptSearch(InputTransfer01SearchConditions SearchConditions, IList<SelectedInputTransfer01ViewModel> InputTransfer01s, bool allowWorkingCreate = false)
        {
            if (SearchConditions.InputTransfer01s.Where(x => x.IsCheck == true).Count() == 0)
            {
                // TODO:エラーメッセージをリソースファイルに
                TempData[AppConst.ERROR] = MessagesResource.SELECT_ONE_RECORD;
                return RedirectToAction("IndexSearch");
            }

            foreach (var v in SearchConditions.InputTransfer01s)
            {
                if (v.IsCheck)
                {
                    InputTransfer01s.Where(m => m.LineNo == v.LineNo).FirstOrDefault().IsCheck = true;
                }
            }

            InputTransfer01ViewModel vm = new InputTransfer01ViewModel();
            vm.SearchConditions = SearchConditions;

            //ワーク01更新
            if (_InputTransferQuery.UpdateArrInputTransfer01(InputTransfer01s))
            {
                if (!allowWorkingCreate)
                {
                    if (_InputTransferQuery.CheckWorkingExists01(InputTransfer01s.FirstOrDefault().Seq) > 0)
                    {
                        //確認メッセージ表示
                        ViewBag.hasError = true.ToString();
                        ViewBag.ErrorCode = "WORKING";
                        return AcceptReturnSearchView(vm,false);
                    }
                }
                // 実績登録
                var message = _InputTransferQuery.CreateInputTransfer01(InputTransfer01s.FirstOrDefault().Seq, InputTransfer01s.FirstOrDefault().CenterId);
                if (string.IsNullOrWhiteSpace(message))
                {
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = InputTransferResource.SUC_UPDATE;
                    _InputTransferQuery.UpdateTtrUpdateCount01(InputTransfer01s.FirstOrDefault().Seq);

                    // 入荷ケースラベル発行
                    string controllerName = this.RouteData.Values["controller"].ToString();

                    var report = new Reports.Export.PrintCaseLabelJanCsv(InputTransfer01s.FirstOrDefault().Seq, InputTransfer01s.FirstOrDefault().CenterId);
                    report.Export();

                    if (report.GetData().Any())
                    {
                        // CSV作成
                        new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                        ViewBag.PrintFlg = GetWfrPrintUrl("CaseLabelJan.wfr", report.DownloadFileName);
                    }

                    return AcceptReturnSearchView(vm,true);
                }
                else
                {
                    TempData[AppConst.ERROR] = message;
                    return AcceptReturnSearchView(vm,false);
                }
            }
            else
            {
                TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                return AcceptReturnSearchView(vm,false);
            }
        }

        /// <summary>
        /// 受入登録処理→元画面に戻る
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private ActionResult AcceptReturnSearchView(InputTransfer01ViewModel vm, bool successFlag)
        {
            if (successFlag) {
                _InputTransferQuery.InsertMovInputTransfer01(vm.SearchConditions);
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, vm.SearchConditions);
            } 

            vm.Results.InputTransfer01s = _InputTransferQuery.InputTransfer01GetData(vm.SearchConditions);
            ViewBag.CenterList = _InputTransferQuery.GetSelectListCenters();
            ViewBag.ItemList = _InputTransferQuery.GetSelectListItems();
            ViewBag.TransferStatusList = _InputTransferQuery.GetSelectListTransferStatus();
            ViewBag.DivisionList = _InputTransferQuery.GetSelectListDivisions();
            ViewBag.Category1List = _InputTransferQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _InputTransferQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _InputTransferQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _InputTransferQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            return this.View("~/Areas/Move/Views/InputTransfer/Index.cshtml", vm);
        }
        #endregion Accept

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private InputTransfer01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new InputTransfer01SearchConditions() : Request.Cookies.Get<InputTransfer01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new InputTransfer01SearchConditions();
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
        private ActionResult GetSearchResultView(InputTransfer01SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new InputTransfer01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new InputTransfer01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _InputTransferQuery.InsertMovInputTransfer01(searchConditions) : true) ? new InputTransfer01Result()
                {
                    InputTransfer01s = _InputTransferQuery.InputTransfer01GetData(searchConditions)
                }
                : new InputTransfer01Result()),
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.InputTransfer01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.InputTransfer01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.CenterList = _InputTransferQuery.GetSelectListCenters();
            ViewBag.ItemList = _InputTransferQuery.GetSelectListItems();
            ViewBag.TransferStatusList = _InputTransferQuery.GetSelectListTransferStatus();
            ViewBag.DivisionList = _InputTransferQuery.GetSelectListDivisions();
            ViewBag.Category1List = _InputTransferQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _InputTransferQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _InputTransferQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _InputTransferQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Move/Views/InputTransfer/Index.cshtml", vm);
        }

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private InputTransfer02SearchConditions GetPreviousSearchInfo02(bool indexFlag)
        {
            var condition = indexFlag ? new InputTransfer02SearchConditions() : Request.Cookies.Get<InputTransfer02SearchConditions>(COOKIE_SEARCHCONDITIONS02) ?? new InputTransfer02SearchConditions();
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
        private ActionResult GetSearchResultView02(InputTransfer02SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new InputTransfer02ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new InputTransfer02Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _InputTransferQuery.InsertArrInputTransfer02(searchConditions) : true) ? new InputTransfer02Result()
                {
                    InputTransfer02s = _InputTransferQuery.InputTransfer02GetData(searchConditions)
                }
                : new InputTransfer02Result())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.InputTransfer02s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.InputTransfer02s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS02, searchConditions);
            ViewBag.CenterList = _InputTransferQuery.GetSelectListCenters();
            ViewBag.ItemList = _InputTransferQuery.GetSelectListItems();
            ViewBag.TransferStatusList = _InputTransferQuery.GetSelectListTransferStatus();
            ViewBag.DivisionList = _InputTransferQuery.GetSelectListDivisions();
            ViewBag.Category1List = _InputTransferQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _InputTransferQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _InputTransferQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _InputTransferQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Move/Views/InputTransfer/Input.cshtml", vm);

        }

        /// <summary>
        /// 更新結果を取得する
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private ActionResult GetUpdateResultView(InputTransfer02ViewModel vm)
        {
            vm.Results.InputTransfer02s = _InputTransferQuery.InputTransfer02GetData(vm.SearchConditions);
            ViewBag.CenterList = _InputTransferQuery.GetSelectListCenters();
            ViewBag.ItemList = _InputTransferQuery.GetSelectListItems();
            ViewBag.TransferStatusList = _InputTransferQuery.GetSelectListTransferStatus();
            ViewBag.DivisionList = _InputTransferQuery.GetSelectListDivisions();
            ViewBag.Category1List = _InputTransferQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _InputTransferQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _InputTransferQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _InputTransferQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            return View("~/Areas/Move/Views/InputTransfer/Input.cshtml", vm);
        }
        #endregion Private

        #region 入力

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="results">Results Information</param>
        /// <param name="allowWorkingUpdate"></param>
        /// <returns>Update</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSearch(InputTransfer02ResultReceipt Results,bool allowWorkingUpdate = false)
        {
            InputTransfer02ViewModel vm = new InputTransfer02ViewModel();
            vm.SearchConditions = Request.Cookies.Get<InputTransfer02SearchConditions>(COOKIE_SEARCHCONDITIONS02) ?? new InputTransfer02SearchConditions();
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                // ワーク02更新
                if (_InputTransferQuery.UpdateArrInputTransfer02(Results.InputTransfer02s))
                {
                    if (_InputTransferQuery.CheckInputResultQty(Results.InputTransfer02s.FirstOrDefault().Seq) > 0)
                    {
                        TempData[AppConst.ERROR] = string.Format(MessagesResource.ERR_REQUIRED_INPUT, MovTransInput02Resource.InputResultQty);
                        return GetUpdateResultView(vm);
                    }
                    if (!allowWorkingUpdate)
                    {
                        if (_InputTransferQuery.CheckWorkingExists02(Results.InputTransfer02s.FirstOrDefault().Seq) > 0)
                        {
                            //確認メッセージ表示
                            ViewBag.hasError = true.ToString();
                            ViewBag.ErrorCode = "WORKING";
                            return GetUpdateResultView(vm);
                        }
                    }
                    // 実績更新
                    Dictionary<string, string> outList = _InputTransferQuery.UpdateInputTransfer02(Results.InputTransfer02s.FirstOrDefault().Seq , Results.InputTransfer02s.FirstOrDefault().CenterId);
                    if (string.IsNullOrWhiteSpace(outList["OUT_MESSAGE"]))
                    {
                        // Clear message to back to index screen
                        TempData[AppConst.SUCCESS] = InputTransferResource.SUC_UPDATE;
                        _InputTransferQuery.UpdateTtrUpdateCount02(Results.InputTransfer02s.FirstOrDefault().Seq);
                        // 入荷ケースラベル発行用
                        if (!string.IsNullOrEmpty(outList["OUT_PRINT_NO"]))
                        {
                            string controllerName = this.RouteData.Values["controller"].ToString();

                            var report = new Reports.Export.PrintCaseLabelJanCsv(outList["OUT_PRINT_NO"].Split(',').ToList(), Results.InputTransfer02s.FirstOrDefault().CenterId);
                            report.Export();

                            // CSV作成
                            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                            ViewBag.PrintFlg = GetWfrPrintUrl("CaseLabelJan.wfr", report.DownloadFileName);
                        }

                        return GetSearchResultView(Request.Cookies.Get<InputTransfer01SearchConditions>(COOKIE_SEARCHCONDITIONS), false);
                    }
                    else
                    {
                        TempData[AppConst.ERROR] = outList["OUT_MESSAGE"];
                        return GetUpdateResultView(vm);
                    }
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                    return GetUpdateResultView(vm);
                }

            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return GetUpdateResultView(vm);
        }

        #endregion 入力
    }
}