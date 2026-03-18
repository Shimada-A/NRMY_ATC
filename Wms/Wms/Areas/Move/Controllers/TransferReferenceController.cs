namespace Wms.Areas.Move.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Arrival.ViewModels.InputPurchase;
    using Wms.Areas.Move.Query.TransferReference;
    using Wms.Areas.Move.Reports.Export;
    using Wms.Areas.Move.ViewModels.InputTransfer;
    using Wms.Areas.Move.ViewModels.TransferReference;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;
    using static Wms.Areas.Move.ViewModels.InputTransfer.InputTransfer01SearchConditions;

    public class TransferReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-MOV_TransferReference01.SearchConditions";
        private const string COOKIE_SEARCHCONDITIONS02 = "W-MOV_TransferReference02.SearchConditions";

        private TransferReferenceQuery _TransferReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReferenceController"/> class.
        /// </summary>
        public TransferReferenceController()
        {
            this._TransferReferenceQuery = new TransferReferenceQuery();
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
        public ActionResult Search(TransferReference01SearchConditions SearchConditions)
        {
            TransferReference01SearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private TransferReference01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new TransferReference01SearchConditions() : Request.Cookies.Get<TransferReference01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new TransferReference01SearchConditions();
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
        private ActionResult GetSearchResultView(TransferReference01SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new TransferReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new TransferReference01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _TransferReferenceQuery.InsertArrTransferReference01(searchConditions) : true) ? new TransferReference01Result()
                {
                    TransferReference01s = _TransferReferenceQuery.TransferReference01GetData(searchConditions)
                }
                : new TransferReference01Result())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.TransferReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.TransferReference01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.CenterList = _TransferReferenceQuery.GetSelectListCenters();
            ViewBag.ItemList = _TransferReferenceQuery.GetSelectListItems();
            ViewBag.DivisionList = _TransferReferenceQuery.GetSelectListDivisions();
            ViewBag.Category1List = _TransferReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _TransferReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _TransferReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _TransferReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Move/Views/TransferReference/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private TransferReference02SearchConditions GetPreviousSearchInfo02(bool indexFlag)
        {
            var condition = indexFlag ? new TransferReference02SearchConditions() : Request.Cookies.Get<TransferReference02SearchConditions>(COOKIE_SEARCHCONDITIONS02) ?? new TransferReference02SearchConditions();
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
        private ActionResult GetSearchResultView02(TransferReference02SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new TransferReference02ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new TransferReference02Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _TransferReferenceQuery.InsertArrTransferReference02(searchConditions) : true) ? new TransferReference02Result()
                {
                    TransferReference02s = _TransferReferenceQuery.TransferReference02GetData(searchConditions)
                }
                : new TransferReference02Result())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.TransferReference02s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.TransferReference02s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS02, searchConditions);
            ViewBag.DivisionList = _TransferReferenceQuery.GetSelectListDivisions();
            ViewBag.ItemList = _TransferReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _TransferReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _TransferReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _TransferReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _TransferReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Move/Views/TransferReference/Detail.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion Private

        #region 明細別

        /// <summary>
        /// 明細別メニューから
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Detail()
        {
            var searchInfo = this.GetPreviousSearchInfo02(true);
            searchInfo.FromMenu = true;
            return this.GetSearchResultView02(searchInfo, true);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult DetailsSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo02(false);
            return this.GetSearchResultView02(searchInfo, false);
        }

        /// <summary>
        /// 明細別仕入先別から
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailIndexSearch(TransferReference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // 仕入先別から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            var vm = new TransferReference02ViewModel();

            // 明細へ
            if (searchConditions.LineNo != 0)
            {
                var TransRef01 = MvcDbContext.Current.MovTransRef01s.Where(x => x.Seq == searchConditions.Seq && x.LineNo == searchConditions.LineNo && x.ShipperId == Common.Profile.User.ShipperId).FirstOrDefault();
                if(searchConditions.UnplannedFlag == 0)
                {
                    vm.SearchConditions.ArriveDateClass = TransferReference01SearchConditions.ArriveDateClasses.ArrivePlanDate;
                    vm.SearchConditions.DenpyoDateFrom = TransRef01.SlipDate;
                    vm.SearchConditions.DenpyoDateTo = TransRef01.SlipDate;
                }
                else
                {
                    vm.SearchConditions.ArriveDateClass = TransferReference01SearchConditions.ArriveDateClasses.TransferResultDate;
                    vm.SearchConditions.TransferResultDateFrom = TransRef01.ArrivePlanDate;
                    vm.SearchConditions.TransferResultDateTo = TransRef01.ArrivePlanDate;
                }
                if(TransRef01.TransferClass == (int)TransferClasses.BaseMove)
                {
                    vm.SearchConditions.TransferFromCenterId = TransRef01.TransferFromStoreId;
                    vm.SearchConditions.BaseMoveFlag = true;
                    vm.SearchConditions.StoreReturnFlag = false;
                    vm.SearchConditions.BaseMoveNoWmsCenterFlag = false;
                }
                if (TransRef01.TransferClass == (int)TransferClasses.StoreReturn)
                {
                    vm.SearchConditions.TransferFromStoreId = TransRef01.TransferFromStoreId;
                    vm.SearchConditions.TransferFromStoreName = TransRef01.TransferFromStoreName;
                    vm.SearchConditions.BaseMoveFlag = false;
                    vm.SearchConditions.StoreReturnFlag = true;
                    vm.SearchConditions.BaseMoveNoWmsCenterFlag = false;
                }
                if (TransRef01.TransferClass == (int)TransferClasses.BaseMoveNoWmsCenter)
                {
                    vm.SearchConditions.TransferFromStoreId = TransRef01.TransferFromStoreId;
                    vm.SearchConditions.TransferFromStoreName = TransRef01.TransferFromStoreName;
                    vm.SearchConditions.BaseMoveFlag = false;
                    vm.SearchConditions.StoreReturnFlag = false;
                    vm.SearchConditions.BaseMoveNoWmsCenterFlag = true;
                }
                vm.SearchConditions.CenterId = searchConditions.CenterId;
                vm.SearchConditions.ContainsArchive = searchConditions.ContainsArchive;
            }
            // 明細別照会へ
            else
            {
                vm.SearchConditions.CenterId = searchConditions.CenterId;
                vm.SearchConditions.BaseMoveFlag = searchConditions.BaseMoveFlag;
                vm.SearchConditions.BaseMoveNoWmsCenterFlag = searchConditions.BaseMoveNoWmsCenterFlag;
                vm.SearchConditions.StoreReturnFlag = searchConditions.StoreReturnFlag;
                vm.SearchConditions.ArriveDateClass = searchConditions.ArriveDateClass;
                vm.SearchConditions.DenpyoDateFrom = searchConditions.DenpyoDateFrom;
                vm.SearchConditions.DenpyoDateTo = searchConditions.DenpyoDateTo;
                vm.SearchConditions.TransferResultDateFrom = searchConditions.TransferResultDateFrom;
                vm.SearchConditions.TransferResultDateTo = searchConditions.TransferResultDateTo;
                vm.SearchConditions.TransferFromCenterId = searchConditions.TransferFromCenterId;
                vm.SearchConditions.TransferFromStoreId = searchConditions.TransferFromStoreId;
                vm.SearchConditions.TransferFromStoreName = searchConditions.TransferFromStoreName;
                vm.SearchConditions.TransferStatus = searchConditions.TransferStatus;
                vm.SearchConditions.DivisionId = searchConditions.DivisionId;
                vm.SearchConditions.CategoryId1 = searchConditions.CategoryId1;
                vm.SearchConditions.CategoryId2 = searchConditions.CategoryId2;
                vm.SearchConditions.CategoryId3 = searchConditions.CategoryId3;
                vm.SearchConditions.CategoryId4 = searchConditions.CategoryId4;
                vm.SearchConditions.BrandId = searchConditions.BrandId;
                vm.SearchConditions.BrandName = searchConditions.BrandName;
                vm.SearchConditions.ItemId = searchConditions.ItemId;
                vm.SearchConditions.ItemColorId = searchConditions.ItemColorId;
                vm.SearchConditions.ItemColorName = searchConditions.ItemColorName;
                vm.SearchConditions.ItemSizeId = searchConditions.ItemSizeId;
                vm.SearchConditions.ItemSizeName = searchConditions.ItemSizeName;
                vm.SearchConditions.Jan = searchConditions.Jan;
                vm.SearchConditions.SlipNo = searchConditions.SlipNo;
                vm.SearchConditions.ContainsArchive = searchConditions.ContainsArchive;
                vm.SearchConditions.ItemCode = searchConditions.ItemCode;
                vm.SearchConditions.DenpyoNo = searchConditions.DenpyoNo;

            }
            vm.SearchConditions.FromMenu = false;
            return DetailSearch(vm.SearchConditions);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailSearch(TransferReference02SearchConditions SearchConditions)
        {
            var condition = new TransferReference02SearchConditions();
            TransferReference02SearchConditions selected = new TransferReference02SearchConditions();
            selected = (TransferReference02SearchConditions)this.TempData["Conditions"];
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

        #endregion 明細別

        #region ロード処理

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TransferReference01Download()
        {
            TransferReference01SearchConditions searchCondition = this.GetPreviousSearchInfo(false);
            Reports.Export.TransferReference01Report report = new Reports.Export.TransferReference01Report(ReportTypes.Excel, searchCondition);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TransferReference02Download()
        {
            TransferReference02SearchConditions searchCondition = this.GetPreviousSearchInfo02(false);

            Reports.Export.TransferReference02Report report = new Reports.Export.TransferReference02Report(ReportTypes.Excel, searchCondition);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        //印刷
        [HttpPost]
        public ActionResult Print()
        {
            TransferReference02SearchConditions condition = this.GetPreviousSearchInfo02(false);
            string styleName = "";

            //if (condition.TransferClass.ToString() == "BaseMove")
            //{
            //    //拠点間移動
            //    styleName = "TransferReferencebaseMove.sty";
            //}
            //else
            //{
            //    //店舗返品
            //    styleName = "TransferReferenceStore.sty";
            //}


            string controllerName = RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            TransferreferenceReportForCsv report = new TransferreferenceReportForCsv(condition);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            return this.File(ret, "application/pdf");
        }

        #endregion ロード処理

        #region 実績入力画面へ遷移

        /// <summary>
        /// 移動入荷実績入力画面へ遷移
        /// </summary>
        /// <param name="result">画面モデル</param>
        /// <returns>List Record</returns>
        [HttpPost]
        public ActionResult ResultInput(IList<SelectedTransferReference02ViewModel> TransferReference02s)
        {
            var target = TransferReference02s.Where(x => x.IsCheck).FirstOrDefault();

            InputTransfer02SearchConditions vm = new InputTransfer02SearchConditions();
            var MovTransRef02 = MvcDbContext.Current.MovTransRef02s.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == target.Seq && x.LineNo == target.LineNo).FirstOrDefault();
            vm.CenterId = MovTransRef02.CenterId;
            switch (MovTransRef02.TransferClass)
            {
                case 1:
                    vm.TransferFromCenterId = MovTransRef02.TransferFromStoreId;
                    vm.BaseMoveFlag = true;
                    vm.StoreReturnFlag = false;
                    vm.BaseMoveNoWmsCenterFlag = false;
                    break;
                case 2:
                    vm.TransferFromStoreId = MovTransRef02.TransferFromStoreId;
                    vm.TransferFromStoreName = MovTransRef02.TransferFromStoreName;
                    vm.BaseMoveFlag = false;
                    vm.StoreReturnFlag = true;
                    vm.BaseMoveNoWmsCenterFlag = false;
                    break;
                case 3:
                    vm.TransferFromStoreId = MovTransRef02.TransferFromStoreId;
                    vm.TransferFromStoreName = MovTransRef02.TransferFromStoreName;
                    vm.BaseMoveFlag = false;
                    vm.StoreReturnFlag = false;
                    vm.BaseMoveNoWmsCenterFlag = true;
                    break;
            }
            if(MovTransRef02.UnplannedFlag == 0)
            {
                vm.ArriveDateClass = ArriveDateClasses.ArrivePlanDate;
                vm.DenpyoDateFrom = MovTransRef02.SlipDate;
                vm.DenpyoDateTo = MovTransRef02.SlipDate;
            }
            else
            {
                vm.ArriveDateClass = ArriveDateClasses.TransferResultDate;
                vm.TransferResultDateFrom = MovTransRef02.ArrivePlanDate;
                vm.TransferResultDateTo = MovTransRef02.ArrivePlanDate;
            }
            vm.DenpyoNo = MovTransRef02.SlipNo;

            string path = this.Url.Action("InputSearch", "InputTransfer", new { area = "Move" });

            this.TempData["Conditions"] = vm;
            return this.Redirect(path);
        }

        #endregion 実績入力画面へ遷移
    }
}