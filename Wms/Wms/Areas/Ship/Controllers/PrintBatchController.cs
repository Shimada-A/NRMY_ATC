namespace Wms.Areas.Ship.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.PrintBatch;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.PrintBatch;
    using Wms.Controllers;
    using Wms.Resources;

    public class PrintBatchController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_PrintBatch.SearchConditions";

        private PrintBatchQuery _PrintBatchQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintBatchController"/> class.
        /// </summary>
        public PrintBatchController()
        {
            this._PrintBatchQuery = new PrintBatchQuery();
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
        public ActionResult Search(PrintBatchSearchConditions SearchConditions)
        {
            ModelState.Clear();
            return this.GetSearchResultView(SearchConditions, false);
        }

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private PrintBatchSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new PrintBatchSearchConditions() : Request.Cookies.Get<PrintBatchSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new PrintBatchSearchConditions();

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(PrintBatchSearchConditions condition, bool indexFlag)
        {
            // Save search info
            var vm = new PrintBatchViewModel
            {
                SearchConditions = condition,
                Results = indexFlag ? new PrintBatchResult() : new PrintBatchResult()
                {
                    PrintBatchs = _PrintBatchQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.PrintBatchs.Count > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.PrintBatchs = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
            ViewBag.BatchNoList = _PrintBatchQuery.GetSelectListBatchNos(condition.CenterId);
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Ship/Views/PrintBatch/Index.cshtml", vm);

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

            var listBatchNo = _PrintBatchQuery.GetSelectListBatchNos(centerId);
            foreach (var batchNo in listBatchNo)
            {
                _html = _html + "<option value='" + batchNo.Value + "'>" + batchNo.Text + "</option>";
            }

            return this.Json(new { html = _html });
        }
        #endregion

        #region 帳票

        /// <summary>
        /// バッチ一覧
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintCheckEc(PrintBatchSearchConditions SearchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            SearchConditions.PrintFlag = "Ec";

            Reports.Export.PrintBatchReport report = new Reports.Export.PrintBatchReport(ReportTypes.Csv, SearchConditions);
            report.Export();

            if (!report.GetData().Any())
            {
                return this.Json(1);
            }

            return this.Json(0);


        }
        /// <summary>
        /// バッチ一覧
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintCheckDc(PrintBatchSearchConditions SearchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            SearchConditions.PrintFlag = "Dc";

            Reports.Export.PrintBatchReport report = new Reports.Export.PrintBatchReport(ReportTypes.Csv, SearchConditions);
            report.Export();

            if (!report.GetData().Any())
            {
                return this.Json(1);
            }

            return this.Json(0);


        }

        /// <summary>
        /// バッチ一覧
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintCheckShip(PrintBatchSearchConditions SearchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            SearchConditions.PrintFlag = "Ship";
            Reports.Export.PrintBatchReport report = new Reports.Export.PrintBatchReport(ReportTypes.Csv, SearchConditions);
            report.Export();



            if (!report.GetData().Any())
            {
                return this.Json(1);
            }

            return this.Json(0);


        }
        /// <summary>
        /// バッチ一覧
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Print(PrintBatchSearchConditions SearchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            Reports.Export.PrintBatchReport report = new Reports.Export.PrintBatchReport(ReportTypes.Csv, SearchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = string.Empty;
            if (SearchConditions.PrintFlag == "Dc")
            {
                styleName = "DcBatchList.wfr";
            }
            else
            {
                styleName = "CaseShipList.wfr";
            }

            return WfrPrint(styleName, report.DownloadFileName);
        }

        /// <summary>
        /// ケース出荷_JAN抜取ピッキングリスト
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reports(PrintBatchSearchConditions SearchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            string styleName = string.Empty;
            string DownloadFileName = string.Empty;
            if (SearchConditions.No == 1)
            {
                Reports.Export.BatchBoard report = new Reports.Export.BatchBoard(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                // PDF作成
                styleName = "BatchBoard.wfr";
                DownloadFileName = report.DownloadFileName;
            }
            if (SearchConditions.No == 2)
            {
                Reports.Export.TotalPicking report = new Reports.Export.TotalPicking(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                // PDF作成
                styleName = "TotalPicking.wfr";
                DownloadFileName = report.DownloadFileName;
            }

            //ECバッチ単位ピッキングリスト
            //if (SearchConditions.No == 3)
            //{
            //    Reports.Export.EcBatchPicking report = new Reports.Export.EcBatchPicking(ReportTypes.Csv, SearchConditions);
            //    report.Export();

            //    // CSV作成
            //    new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            //    // PDF作成
            //    string styleName;
            //    if (SearchConditions.chkJan)
            //    {
            //        styleName = "EcBatchPickingJan.sty";
            //    }
            //    else
            //    {
            //        styleName = "EcBatchPicking.sty";
            //    }

            //    ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);
            //}
            if (SearchConditions.No == 4)
            {
                Reports.Export.EcUnitBoard report = new Reports.Export.EcUnitBoard(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                // PDF作成
                styleName = "EcUnitBoard.sty";
                //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);
            }
            if (SearchConditions.No == 5)
            {
                Reports.Export.EcGASBoard report = new Reports.Export.EcGASBoard(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                // PDF作成
                styleName = "EcGASBoard.sty";
                //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);
            }
            if (SearchConditions.No == 6)
            {
                Reports.Export.CaseShipPicking report = new Reports.Export.CaseShipPicking(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                // PDF作成
                styleName = "CaseShipPicking_Store.wfr";
                DownloadFileName = report.DownloadFileName;
            }
            if (SearchConditions.No == 7)
            {
                Reports.Export.CaseJanPicking report = new Reports.Export.CaseJanPicking(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                // PDF作成
                styleName = "CaseJanPicking.wfr";

                DownloadFileName = report.DownloadFileName;
            }
            if (SearchConditions.No == 8)
            {
                Reports.Export.PickBoard report = new Reports.Export.PickBoard(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                // PDF作成
                styleName = "PickBoard.wfr";
                DownloadFileName = report.DownloadFileName;
            }
            //店別ピッキングリスト
            if (SearchConditions.No == 10)
            {
                Reports.Export.StorePicking report = new Reports.Export.StorePicking(ReportTypes.Csv, SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                // PDF作成
                styleName = "StorePicking.wfr";

                DownloadFileName = report.DownloadFileName;
            }

            //印字フラグ更新
            var status = ProcedureStatus.Success;
            var message = string.Empty;
            if (!SearchConditions.OnlyPicDuring && !SearchConditions.OnlyUnpicked)
            {
                _PrintBatchQuery.PrintFlagUpdate(SearchConditions, out status, out message);
            }
            if (status == ProcedureStatus.Success)
            {
                //return this.File(ret, "application/pdf");
                SearchConditions.Ret = ret;
                SearchConditions.Print = "Print";
                SearchConditions.StyleName = styleName;
                SearchConditions.DownloadFileName = DownloadFileName;
                // 検索
                return Search(SearchConditions);
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;
                // 検索
                return Search(SearchConditions);
            }
        }
        [HttpPost]
        public ActionResult ReportPrint(PrintBatchSearchConditions SearchConditions)
        {
            return WfrPrint(SearchConditions.StyleName, SearchConditions.DownloadFileName);
        }


        #endregion 帳票
    }
}