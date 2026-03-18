namespace Wms.Areas.Arrival.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Arrival.Query.PurchaseReference;
    using Wms.Areas.Arrival.ViewModels.InputPurchase;
    using Wms.Areas.Arrival.ViewModels.PurchaseReference;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class PurchaseReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-ARR_PurchaseReference01.SearchConditions";
        private const string COOKIE_SEARCHCONDITIONS02 = "W-ARR_PurchaseReference02.SearchConditions";

        private PurchaseReferenceQuery _PurchaseReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseReferenceController"/> class.
        /// </summary>
        public PurchaseReferenceController()
        {
            this._PurchaseReferenceQuery = new PurchaseReferenceQuery();
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
        public ActionResult Search(PurchaseReference01SearchConditions SearchConditions)
        {
            PurchaseReference01SearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private PurchaseReference01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new PurchaseReference01SearchConditions() : Request.Cookies.Get<PurchaseReference01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new PurchaseReference01SearchConditions();
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
        private ActionResult GetSearchResultView(PurchaseReference01SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new PurchaseReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new PurchaseReference01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _PurchaseReferenceQuery.InsertArrPurchaseReference01(searchConditions) : true) ? new PurchaseReference01Result()
                {
                    PurchaseReference01s = _PurchaseReferenceQuery.PurchaseReference01GetData(searchConditions)
                }
                : new PurchaseReference01Result())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.PurchaseReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.PurchaseReference01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _PurchaseReferenceQuery.GetSelectListDivisions();
            ViewBag.BrandList = _PurchaseReferenceQuery.GetSelectListBrands();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Arrival/Views/PurchaseReference/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private PurchaseReference02SearchConditions GetPreviousSearchInfo02(bool indexFlag)
        {
            var condition = indexFlag ? new PurchaseReference02SearchConditions() : Request.Cookies.Get<PurchaseReference02SearchConditions>(COOKIE_SEARCHCONDITIONS02) ?? new PurchaseReference02SearchConditions();
            condition.PageSize = indexFlag ? this.GetCurrentPageSize() : condition.PageSize;
            condition.Page = indexFlag ? 1 : condition.Page;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView02(PurchaseReference02SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new PurchaseReference02ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new PurchaseReference02Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _PurchaseReferenceQuery.InsertArrPurchaseReference02(searchConditions) : true) ? new PurchaseReference02Result()
                {
                    PurchaseReference02s = _PurchaseReferenceQuery.PurchaseReference02GetData(searchConditions)
                }
                : new PurchaseReference02Result())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.PurchaseReference02s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.PurchaseReference02s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS02, searchConditions);
            ViewBag.DivisionList = _PurchaseReferenceQuery.GetSelectListDivisions();
            ViewBag.ItemList = _PurchaseReferenceQuery.GetSelectListItems();
            ViewBag.BrandList = _PurchaseReferenceQuery.GetSelectListBrands();
            ViewBag.Category1List = _PurchaseReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _PurchaseReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _PurchaseReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _PurchaseReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Arrival/Views/PurchaseReference/Detail.cshtml", vm);

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
        /// 入荷実績入力から
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailResultInput()
        {
            var searchInfo = this.GetPreviousSearchInfo02(false);
            return this.GetSearchResultView02(searchInfo, false);
        }
        /// <summary>
        /// 明細別仕入先別から
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailIndexSearch(PurchaseReference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // 仕入先別から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            var vm = new PurchaseReference02ViewModel();

            // 明細へ
            if (searchConditions.LineNo != 0)
            {
                var PurRef01 = MvcDbContext.Current.ArrPurRef01s.Where(x => x.Seq == searchConditions.Seq && x.LineNo == searchConditions.LineNo && x.ShipperId == Common.Profile.User.ShipperId).FirstOrDefault();
                var vendor = MvcDbContext.Current.Vendors.Where(x => x.VendorId == PurRef01.VendorId && x.ShipperId == Common.Profile.User.ShipperId).FirstOrDefault();
                vm.SearchConditions.ArrivePlanDateFrom = PurRef01.ArrivePlanDate;
                vm.SearchConditions.ArrivePlanDateTo = PurRef01.ArrivePlanDate;
                vm.SearchConditions.VendorId = PurRef01.VendorId;
                vm.SearchConditions.VendorName = (vendor == null) ? "" :vendor.VendorName1;
                vm.SearchConditions.CenterId = searchConditions.CenterId;
                vm.SearchConditions.ContainsArchive = searchConditions.ContainsArchive;
                vm.SearchConditions.DivisionId = searchConditions.DivisionId;
                vm.SearchConditions.BrandId = searchConditions.BrandId;
                vm.SearchConditions.BrandName = searchConditions.BrandName;
                vm.SearchConditions.InvoiceNo = searchConditions.InvoiceNo;
            }
            // 明細別照会へ
            else
            {
                vm.SearchConditions.CenterId = searchConditions.CenterId;
                vm.SearchConditions.ArrivePlanDateFrom = searchConditions.ArrivePlanDateFrom;
                vm.SearchConditions.ArrivePlanDateTo = searchConditions.ArrivePlanDateTo;
                vm.SearchConditions.DivisionId = searchConditions.DivisionId;
                vm.SearchConditions.BrandId = searchConditions.BrandId;
                vm.SearchConditions.BrandName = searchConditions.BrandName;
                vm.SearchConditions.VendorId = searchConditions.VendorId;
                vm.SearchConditions.VendorName = searchConditions.VendorName;
                vm.SearchConditions.InvoiceNo = searchConditions.InvoiceNo;
                vm.SearchConditions.ContainsArchive = searchConditions.ContainsArchive;
            }
            vm.SearchConditions.FromMenu = false;
            return DetailSearch(vm.SearchConditions);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailSearch(PurchaseReference02SearchConditions SearchConditions)
        {
            var condition = new PurchaseReference02SearchConditions();
            PurchaseReference02SearchConditions selected = new PurchaseReference02SearchConditions();
            selected = (PurchaseReference02SearchConditions)this.TempData["Conditions"];
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
        public ActionResult PurchaseReference01Download()
        {
            PurchaseReference01SearchConditions searchCondition = this.GetPreviousSearchInfo(false);
            Reports.Export.PurchaseReference01Report report = new Reports.Export.PurchaseReference01Report(ReportTypes.Excel, searchCondition);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PurchaseReference02Download()
        {
            PurchaseReference02SearchConditions searchCondition = this.GetPreviousSearchInfo02(false);

            if (searchCondition.ResultType == PurchaseReference02SearchConditions.ResultTypes.Detail)
            {
                Reports.Export.PurchaseReference02DetailReport report = new Reports.Export.PurchaseReference02DetailReport(ReportTypes.Excel, searchCondition);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
            else
            {
                Reports.Export.PurchaseReference02BoxNoReport report = new Reports.Export.PurchaseReference02BoxNoReport(ReportTypes.Excel, searchCondition);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
        }

        #endregion ロード処理

        #region 実績入力画面へ遷移

        /// <summary>
        /// 入荷実績入力画面へ遷移
        /// </summary>
        /// <param name="result">画面モデル</param>
        /// <returns>List Record</returns>
        [HttpPost]
        public ActionResult ResultInput(IList<SelectedPurchaseReference02ViewModel> PurchaseReference02s)
        {
            var target = PurchaseReference02s.Where(x => x.IsCheck).FirstOrDefault();

            SelectedInputPurchase01ViewModel vm = new SelectedInputPurchase01ViewModel();
            var ArrPurRef02 = MvcDbContext.Current.ArrPurRef02s.Where(x=>x.ShipperId==Common.Profile.User.ShipperId && x.Seq== target.Seq && x.LineNo == target.LineNo).FirstOrDefault();
            vm.CenterId = ArrPurRef02.CenterId;
            vm.ShipperId = ArrPurRef02.ShipperId;
            vm.ArrivePlanDate = ArrPurRef02.ArrivePlanDate;
            vm.VendorId = ArrPurRef02.VendorId;
            vm.VendorName = ArrPurRef02.VendorName;
            vm.InvoiceNo = ArrPurRef02.InvoiceNo;
            vm.PoId = ArrPurRef02.PoId;

            string path = this.Url.Action("Input", "InputPurchase", new { area = "Arrival" });

            this.TempData["Conditions"] = vm;
            return this.Redirect(path);
        }

        #endregion 実績入力画面へ遷移

        #region 帳票

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(PurchaseReference02SearchConditions SearchConditions)
        {
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            Reports.Export.PurchaseReference01Csv report = new Reports.Export.PurchaseReference01Csv(ReportTypes.Csv, SearchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            //// PDF作成
            //string styleName = "PurchaseReference.sty";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            //return this.File(ret, "application/pdf");

            return WfrPrint("PurchaseReference.wfr", report.DownloadFileName);
        }

        /// <summary>
        /// 入荷仕分リスト(JAN入り)ダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportJan(PurchaseReference02SearchConditions SearchConditions)
        {
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            Reports.Export.PurchaseReference01JanCsv report = new Reports.Export.PurchaseReference01JanCsv(ReportTypes.Csv, SearchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            //// PDF作成
            //string styleName = "PurchaseReferenceJan.sty";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            //return this.File(ret, "application/pdf");

            return WfrPrint("PurchaseReference.wfr", report.DownloadFileName);
        }

        /// <summary>
        /// 仕入梱包リスト(JAN入り)ダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportPacking(PurchaseReference02SearchConditions SearchConditions)
        {
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            Reports.Export.PurchaseReference01PackingCsv report = new Reports.Export.PurchaseReference01PackingCsv(ReportTypes.Csv, SearchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = "PurchaseReferencePacking.wfr";
            return WfrPrint(styleName, report.DownloadFileName);
        }
        #endregion 帳票
    }
}