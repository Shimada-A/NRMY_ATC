namespace Wms.Areas.Stock.Controllers
{
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Stock.Query.Reference;
    using Wms.Areas.Stock.ViewModels.Reference;
    using Wms.Controllers;
    using Wms.Resources;

    public class ReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-STK_Reference.SearchConditions";

        private ReferenceQuery _ReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceController"/> class.
        /// </summary>
        public ReferenceController()
        {
            this._ReferenceQuery = new ReferenceQuery();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(ReferenceSearchConditions SearchConditions)
        {
            ReferenceSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region PackageStock

        /// <summary>
        /// PackageStock
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PackageStockSearch(ReferenceSearchConditions searchConditions)
        {
            // 在庫照会明細（画面選択行更新用）
            _ReferenceQuery.UpdateStkStock(searchConditions.References);
            searchConditions.PageSize = this.GetCurrentPageSize();

            // 作成処理&検索表示
            var vm = new ReferenceViewModel
            {
                SearchConditions = searchConditions,
                Results = _ReferenceQuery.InsertStkStock02(searchConditions) ? new ReferenceResult()
                {
                    References = _ReferenceQuery.GetData(searchConditions)
                }
                : new ReferenceResult(),
                Page = 1
            };
            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _ReferenceQuery.GetSelectListDivisions();
            ViewBag.LocationClassList = _ReferenceQuery.GetSelectListLocationClasses();
            ViewBag.Category1List = _ReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _ReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _ReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _ReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _ReferenceQuery.GetSelectListItems();
            ViewBag.GradeList = _ReferenceQuery.GetSelectListGrades();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Stock/Views/Reference/Index.cshtml", vm);
        }

        #endregion PackageStock

        #region Selected

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(ReferenceSearchConditions searchConditions)
        {
            // 全選択
            _ReferenceQuery.StkStockAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new ReferenceViewModel
            {
                SearchConditions = searchConditions,
                Results = new ReferenceResult()
                {
                    References = _ReferenceQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _ReferenceQuery.GetSelectListDivisions();
            ViewBag.LocationClassList = _ReferenceQuery.GetSelectListLocationClasses();
            ViewBag.Category1List = _ReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _ReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _ReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _ReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _ReferenceQuery.GetSelectListItems();
            ViewBag.GradeList = _ReferenceQuery.GetSelectListGrades();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Stock/Views/Reference/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(ReferenceSearchConditions searchConditions)
        {
            // 全解除
            _ReferenceQuery.StkStockAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new ReferenceViewModel
            {
                SearchConditions = searchConditions,
                Results = new ReferenceResult()
                {
                    References = _ReferenceQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _ReferenceQuery.GetSelectListDivisions();
            ViewBag.LocationClassList = _ReferenceQuery.GetSelectListLocationClasses();
            ViewBag.Category1List = _ReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _ReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _ReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _ReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _ReferenceQuery.GetSelectListItems();
            ViewBag.GradeList = _ReferenceQuery.GetSelectListGrades();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Stock/Views/Reference/Index.cshtml", vm);
        }

        #endregion Selected

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private ReferenceSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new ReferenceSearchConditions() : Request.Cookies.Get<ReferenceSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new ReferenceSearchConditions();
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
        private ActionResult GetSearchResultView(ReferenceSearchConditions searchConditions, bool indexFlag)
        {
            // 在庫照会明細（画面選択行更新用）
            if (!indexFlag && searchConditions.ResultType == Common.ResultTypes.Stock && searchConditions.SearchType == Common.SearchTypes.SortPage)
            {
                _ReferenceQuery.UpdateStkStock(searchConditions.References);
            }

            // 作成処理&検索表示
            var vm = new ReferenceViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new ReferenceResult() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _ReferenceQuery.InsertStkStock(searchConditions) : true) ? new ReferenceResult()
                {
                    References = _ReferenceQuery.GetData(searchConditions)
                }
                : new ReferenceResult()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.References.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.References = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _ReferenceQuery.GetSelectListDivisions();
            ViewBag.LocationClassList = _ReferenceQuery.GetSelectListLocationClasses();
            ViewBag.Category1List = _ReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _ReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _ReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _ReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _ReferenceQuery.GetSelectListItems();
            ViewBag.GradeList = _ReferenceQuery.GetSelectListGrades();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Stock/Views/Reference/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion Private

        #region ロード処理

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Download()
        {
            ReferenceSearchConditions searchCondition = this.GetPreviousSearchInfo(false);

            if (searchCondition.ResultType == Common.ResultTypes.Stock)
            {
                Reports.Export.StockReport report = new Reports.Export.StockReport(ReportTypes.Excel, searchCondition);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
            else
            {
                Reports.Export.PackageStockReport report = new Reports.Export.PackageStockReport(ReportTypes.Excel, searchCondition);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
        }

        /// <summary>
        /// 帳票を印刷する
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Print(ReferenceSearchConditions SearchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            //JAN入り明細フラグをfalse
            SearchConditions.DetailJanFlag = false;

            // PDF作成
            string styleName = "Stocks.wfr";

            if (SearchConditions.ResultType == Common.ResultTypes.Stock)
            {
                Reports.Export.StockReportForCsv report = new Reports.Export.StockReportForCsv(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                //PDF作成
                return WfrPrint(styleName, report.DownloadFileName);

            }
            else
            {
                Reports.Export.PackageStockReportForCsv report = new Reports.Export.PackageStockReportForCsv(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                //PDF作成
                return WfrPrint(styleName, report.DownloadFileName);

            }


            ///// PDF作成
            //string styleName = "StockDetail.sty";
            //string DownloadFileName = "StockDetail_1page.csv";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, DownloadFileName);
            ////ret = new CsvPrintFileCreate().OutputPrint(controllerName, styleName, DownloadFileName);

            //return this.File(ret, "application/pdf");
        }

        /// <summary>
        /// 帳票を印刷する
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintJan(ReferenceSearchConditions SearchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            //JAN入り明細フラグをtrue
            SearchConditions.DetailJanFlag = true;

            // PDF作成
            string styleName = "Stocks.wfr";

            if (SearchConditions.ResultType == Common.ResultTypes.Stock)
            {
                Reports.Export.StockReportForCsv report = new Reports.Export.StockReportForCsv(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                //PDF作成
                return WfrPrint(styleName, report.DownloadFileName);

            }
            else
            {
                Reports.Export.PackageStockReportForCsv report = new Reports.Export.PackageStockReportForCsv(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                //PDF作成
                return WfrPrint(styleName, report.DownloadFileName);

            }


            ///// PDF作成
            //string styleName = "StockDetail.sty";
            //string DownloadFileName = "StockDetail_1page.csv";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, DownloadFileName);
            ////ret = new CsvPrintFileCreate().OutputPrint(controllerName, styleName, DownloadFileName);

            return this.File(ret, "application/pdf");
        }

        #endregion ロード処理

    }
}