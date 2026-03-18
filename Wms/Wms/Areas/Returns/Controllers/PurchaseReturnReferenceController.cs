namespace Wms.Areas.Returns.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Arrival.ViewModels.InputPurchase;
    using Wms.Areas.Returns.Query.PurchaseReturnReference;
    using Wms.Areas.Returns.Reports.Export;
    using Wms.Areas.Returns.ViewModels.PurchaseReturnReference;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class PurchaseReturnReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-RET_PurchaseReturnReference01.SearchConditions";

        private PurchaseReturnReferenceQuery _PurchaseReturnReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseReturnReferenceController"/> class.
        /// </summary>
        public PurchaseReturnReferenceController()
        {
            this._PurchaseReturnReferenceQuery = new PurchaseReturnReferenceQuery();
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
        public ActionResult Search(PurchaseReturnReference01SearchConditions SearchConditions)
        {
            PurchaseReturnReference01SearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private PurchaseReturnReference01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new PurchaseReturnReference01SearchConditions() : Request.Cookies.Get<PurchaseReturnReference01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new PurchaseReturnReference01SearchConditions();
            condition.PageSize = this.GetCurrentPageSize();

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(PurchaseReturnReference01SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new PurchaseReturnReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new PurchaseReturnReference01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _PurchaseReturnReferenceQuery.InsertArrPurchaseReturnReference01(searchConditions) : true) ? new PurchaseReturnReference01Result()
                {
                    PurchaseReturnReference01s = _PurchaseReturnReferenceQuery.PurchaseReturnReference01GetData(searchConditions)
                }
                : new PurchaseReturnReference01Result())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.PurchaseReturnReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.PurchaseReturnReference01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _PurchaseReturnReferenceQuery.GetSelectListDivisions();
            ViewBag.Category1List = _PurchaseReturnReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _PurchaseReturnReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _PurchaseReturnReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _PurchaseReturnReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Returns/Views/PurchaseReturnReference/Index.cshtml", vm);
        }

        /// <summary>
        /// 検索結果ビューを取得する(明細)
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView02(PurchaseReturnReference01SearchConditions searchConditions)
        {
            // 作成処理&検索表示
            var vm = new PurchaseReturnReference02ViewModel
            {
                SearchConditions = searchConditions,
                Results = new PurchaseReturnReference02Result()
                {
                    PurchaseReturnReference02s = _PurchaseReturnReferenceQuery.PurchaseReturnReference02GetData(searchConditions)
                }
            };

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Returns/Views/PurchaseReturnReference/Detail.cshtml", vm);
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
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView02(searchInfo);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult DetailsSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            return this.GetSearchResultView02(searchInfo);
        }

        /// <summary>
        /// 明細に移動
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailIndexSearch(PurchaseReturnReference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // 仕入先別から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            return DetailSearch(searchConditions);
        }

        /// <summary>
        /// 明細の検索処理
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailSearch(PurchaseReturnReference01SearchConditions SearchConditions)
        {
            return this.GetSearchResultView02(SearchConditions);
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
        public ActionResult PurchaseReturnReference01Download()
        {
            PurchaseReturnReference01SearchConditions searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.PurchaseReturnReference01Report report = new Reports.Export.PurchaseReturnReference01Report(ReportTypes.Excel, searchCondition);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        //仕入先返品伝票印刷
        [HttpPost]
        public ActionResult PrintReturn()
        {
            PurchaseReturnReference01SearchConditions condition = this.GetPreviousSearchInfo(false);
            string styleName;

            styleName = "PurchaseReturn.sty";

            string controllerName = RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            PurchaseReturnReferenceReportForCsvReturn report = new PurchaseReturnReferenceReportForCsvReturn(condition);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            return this.File(ret, "application/pdf");
        }

        //仕入訂正伝票印刷
        [HttpPost]
        public ActionResult PrintCorrection()
        {
            PurchaseReturnReference01SearchConditions condition = this.GetPreviousSearchInfo(false);
            string styleName;

            styleName = "PurchaseCorrection.sty";

            string controllerName = RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            PurchaseReturnReferenceReportForCsvCorrection report = new PurchaseReturnReferenceReportForCsvCorrection(condition);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            return this.File(ret, "application/pdf");
        }

        #endregion ロード処理
    }
}