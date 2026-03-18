namespace Wms.Areas.Ship.Controllers
{
    using System;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.Query.PrintCaseLabel;
    using Wms.Areas.Ship.ViewModels.PrintCaseLabel;
    using Wms.Controllers;
    using Wms.Resources;
    using static Wms.Areas.Ship.ViewModels.PrintCaseLabel.PrintCaseLabelConditions;

    public class PrintCaseLabelController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-SHP_PrintCaseLabel.SearchConditions";

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
            return this.View("~/Areas/Ship/Views/PrintCaseLabel/Index.cshtml", searchInfo);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult IndexSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            return this.View("~/Areas/Ship/Views/PrintCaseLabel/Index.cshtml", searchInfo);
        }

        /// <summary>
        /// 確認メッセージ(BtoB全店舗ケースラベル発行時)
        /// </summary>
        /// <param name="printCaseLabelConditions"></param>
        /// <returns></returns>
        public ActionResult Search(PrintCaseLabelConditions printCaseLabelConditions)
        {
            PrintCaseLabelConditions condition = printCaseLabelConditions;
            return GetStoreCount(condition);
        }

        #endregion

        #region Print

        private ActionResult GetStoreCount(PrintCaseLabelConditions printCaseLabelConditions)
        {
            ModelState.Clear();
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, printCaseLabelConditions);
            var store_count = _PrintCaseLabelQuery.StoreAllCount(printCaseLabelConditions);

            ViewBag.Message = String.Format(PrintCaseLabelResource.ConfirmMsg, store_count);
            return this.View("~/Areas/Ship/Views/PrintCaseLabel/Index.cshtml", printCaseLabelConditions);
        }

        /// <summary>
        /// PrintCaseLabelConditions Information
        /// </summary>
        /// <param name="SearchConditions">PrintCaseLabelConditions</param>
        [HttpPost]
        public JsonResult Print(string centerId, string releaseClass, string storeClass, string shipToStoreId, string transporterId, string numberofSheets, string releaseBoxNo, string brandId, string brandName, string shipInstructFlag, string batchNo, string storeOutletsClass)
        {
            PrintCaseLabelConditions SearchConditions = new PrintCaseLabelConditions { };
            SearchConditions.CenterId = centerId;
            SearchConditions.ShipToClass = ShipToClasses.BtoB;
            SearchConditions.ShipToStoreId = shipToStoreId;
            SearchConditions.TransporterId = transporterId;
            SearchConditions.BrandId = brandId;
            SearchConditions.BrandName = brandName;
            SearchConditions.BatchNo = batchNo;

            if (releaseClass == "0")
            {
                SearchConditions.ReleaseClass = ReleaseClasses.Release;
            }
            else
            {
                SearchConditions.ReleaseClass = ReleaseClasses.AgainRelease;
            }
            if (storeClass == "0")
            {
                SearchConditions.StoreClass = StoreClasses.Store;
            }
            else
            {
                SearchConditions.StoreClass = StoreClasses.Centers;
            }
            if (shipInstructFlag == "true")
            {
                SearchConditions.ShipInstructFlag = true;
            }
            else
            {
                SearchConditions.ShipInstructFlag = false;
            }

            if (!string.IsNullOrEmpty(numberofSheets)) 
            { 
                SearchConditions.NumberofSheets = long.Parse(numberofSheets); 
            }

            if (storeOutletsClass == "0")
            {
                SearchConditions.StoreOutletsClass = StoreOutletsClasses.Outlets;
            }
            else if (storeOutletsClass == "1")
            {
                SearchConditions.StoreOutletsClass = StoreOutletsClasses.NotOutlets;
            }
            else
            {
                SearchConditions.StoreOutletsClass = StoreOutletsClasses.OnlyOutlets;
            }

            SearchConditions.ReleaseBoxNo = releaseBoxNo;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            var status = "0";
            int result = 0;
            string errmessage = string.Empty;

            string printCcdUrlCaseLabel = string.Empty;
            string fileNameCaseLabel = string.Empty;
            //URL取得
            string strPathAndQuery = AppConfig.PrintCcdUrl + Request.Url.Host;

            if (SearchConditions.ReleaseClass == ReleaseClasses.Release)
            {
                if (SearchConditions.ShipToClass == ShipToClasses.BtoB)
                {
                    // 発行-BtoB選択時
                    Reports.Export.PrintBtoBInvoiceCsv report = new Reports.Export.PrintBtoBInvoiceCsv(ReportTypes.Csv, SearchConditions);
                    report.Export();

                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                    // CCDファイル作成
                    //string styleName = "CaseLabelBtoB.sty";
                    //fileNameCaseLabel = "CaseLabelBtoB";
                    //ret = new CsvPrintFileCreate().OutputPrint(controllerName, styleName, report.DownloadFileName);

                    //プリンタ名取得
                    var printer = _PrintCaseLabelQuery.GetPrinterName(SearchConditions.CenterId, "CASE_LABEL_BTOB");
                    //印刷URL取得
                    ret = GetWfrPrintUrl("CaseLabelBtoB.wfr", report.DownloadFileName, printer);
                }
                else
                {
                    //// 発行-EC選択時
                    //Reports.Export.PrintEcInvoiceCsv report = new Reports.Export.PrintEcInvoiceCsv(ReportTypes.Csv, SearchConditions);
                    //report.Export();

                    //// CSV作成
                    //new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                    //// CCDファイル作成
                    //string styleName = "CaseLabelEc.sty";
                    //fileNameCaseLabel = "CaseLabelEc";
                    //ret = new CsvPrintFileCreate().OutputPrint(controllerName, styleName, report.DownloadFileName);
                }

            }
            else
            {
                if (SearchConditions.ShipToClass == ShipToClasses.BtoB)
                {
                    //出荷梱包実績存在チェック
                    status = _PrintCaseLabelQuery.BoxNoCheck(SearchConditions);
                    if (status == "1")
                    {
                        errmessage = MessageResource.BOX_NO_NOT_EXIST_ERROR;
                        return Json(new { status, errmessage });
                    }

                    // BtoB出荷ラベル再発行
                    Reports.Export.PrintCaseLabelReissueBtoBCsv report = new Reports.Export.PrintCaseLabelReissueBtoBCsv(ReportTypes.Csv, SearchConditions);
                    report.Export();

                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                    // CCDファイル作成
                    //string styleName = "CaseLabelBtoB.sty";
                    //fileNameCaseLabel = "CaseLabelBtoB";
                    //ret = new CsvPrintFileCreate().OutputPrint(controllerName, styleName, report.DownloadFileName);

                    //プリンタ名取得
                    var printer = _PrintCaseLabelQuery.GetPrinterName(SearchConditions.CenterId, "CASE_LABEL_BTOB");
                    //印刷URL取得
                    ret = GetWfrPrintUrl("CaseLabelBtoB.wfr", report.DownloadFileName, printer);
                }
                else
                {
                    ////ECラベルチェック
                    //string chkecLabel = SearchConditions.CenterId + "E";
                    //string ecLabel = SearchConditions.ReleaseBoxNo.Substring(0, 5);
                    //if (ecLabel != chkecLabel || SearchConditions.ReleaseBoxNo.Length != 12)
                    //{
                    //    status = "-1";
                    //    errmessage = MessageResource.BOX_NO_ERROR;
                    //    return Json(new { status, errmessage });
                    //}
                    ////出荷梱包実績存在チェック
                    //status = _PrintCaseLabelQuery.BoxNoCheck(SearchConditions);
                    //if (status == "1")
                    //{
                    //    errmessage = MessageResource.BOX_NO_NOT_EXIST_ERROR;
                    //    return Json(new { status, errmessage });
                    //}

                    //// EC出荷ラベル再発行
                    //Reports.Export.PrintCaseLabelReissueEcCsv report = new Reports.Export.PrintCaseLabelReissueEcCsv(ReportTypes.Csv, SearchConditions);
                    //report.Export();

                    //// CSV作成
                    //new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                    //// CCDファイル作成
                    //string styleName = "CaseLabelEc.sty";
                    //fileNameCaseLabel = "CaseLabelEc";
                    //ret = new CsvPrintFileCreate().OutputPrint(controllerName, styleName, report.DownloadFileName);
                }

            }
            if (int.TryParse(ret, out result))
            {
                status = result.ToString();
                errmessage = Wms.Resources.MessageResource.ERR_PRINT;
                return Json(new { status, errmessage });
            }
            //printCcdUrlCaseLabel = strPathAndQuery + ret;
            return Json(new { status, ret });
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

        /// <summary>
        /// 検索条件.センターID変更時処理
        /// </summary>
        /// <param name="centerId">検索条件.センターID</param>
        [HttpPost]
        public JsonResult ChgCenter (string centerId)
        {
            var brandWorkClass = _PrintCaseLabelQuery.GetbrandWorkClass(centerId);

            return Json(new { brandWorkClass });
        }
        #endregion
        }
}