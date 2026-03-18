namespace Wms.Areas.Arrival.Controllers
{
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Arrival.Query.PrintCaseLabel;
    using Wms.Areas.Arrival.Resources;
    using Wms.Areas.Arrival.ViewModels.PrintCaseLabel;
    using Wms.Controllers;
    using static Wms.Areas.Arrival.ViewModels.PrintCaseLabel.PrintCaseLabelConditions;

    public class PrintCaseLabelController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-ARR_PrintCaseLabel.SearchConditions";

        private PrintCaseLabelQuery _PrintCaseLabelQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintCaseLabelController"/> class.
        /// </summary>
        public PrintCaseLabelController()
        {
            this._PrintCaseLabelQuery = new PrintCaseLabelQuery();
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
            return this.View("~/Areas/Arrival/Views/PrintCaseLabel/Index.cshtml", searchInfo);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult IndexSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            return this.View("~/Areas/Arrival/Views/PrintCaseLabel/Index.cshtml", searchInfo);
        }

        #endregion

        #region Print

        /// <summary>
        /// PrintCaseLabelConditions Information
        /// </summary>
        /// <param name="SearchConditions">PrintCaseLabelConditions</param>
        public ActionResult Print(PrintCaseLabelConditions SearchConditions)
        {
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            // PC発行入荷ラベル発行
            Reports.Export.PrintCaseLabelJanCsv report = new Reports.Export.PrintCaseLabelJanCsv(ReportTypes.Csv, SearchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            //// PDF作成
            //string styleName = "CaseLabelJan.sty";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            //return this.File(ret, "application/pdf");

            return WfrPrint("CaseLabelJan.wfr", report.DownloadFileName);
        }

        [HttpPost]
        public JsonResult PrintRe(string boxNo)
        {
            PrintCaseLabelConditions SearchConditions = new PrintCaseLabelConditions { };
            SearchConditions.ReleaseBoxNo = boxNo;
            SearchConditions.ReleaseClass = ReleaseClasses.AgainRelease;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            var status = 0;

            //string str = SearchConditions.ReleaseBoxNo.Substring(1, 4);
            //if (str != Common.Profile.User.CenterId + "1")
            //{
            //    status = -10;
            //    TempData[AppConst.ERROR] = PrintCaseLabelResource.BOX_NO_ERROR;
            //    return Json(new { status });
            //}

            var message = _PrintCaseLabelQuery.BoxNoCheck(SearchConditions);
            if (!string.IsNullOrWhiteSpace(message))
            {
                status = -1;
                TempData[AppConst.ERROR] = message;
                return Json(new { status });
            }

            return Json(new { status });
        }

        [HttpPost]
        public ActionResult PrintReRun(PrintCaseLabelConditions SearchConditions)
        {
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            string ret = string.Empty;
            string controllerName = this.RouteData.Values["controller"].ToString();

            // 入荷ラベル発行
            Reports.Export.PrintCaseLabelCsv report = new Reports.Export.PrintCaseLabelCsv(ReportTypes.Csv, SearchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            //// PDF作成
            //string styleName = "CaseLabel.sty";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            //return File(ret, "application/pdf");
            return WfrPrint("CaseLabelJan.wfr", report.DownloadFileName);
        }

        #endregion Print

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private PrintCaseLabelConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new PrintCaseLabelConditions() : Request.Cookies.Get<PrintCaseLabelConditions>(COOKIE_SEARCHCONDITIONS) ?? new PrintCaseLabelConditions();
            // return search object
            return condition;
        }

        #endregion

    }
}