namespace Wms.Areas.Ship.Controllers
{
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.PrintEcInvoice;
    using Wms.Areas.Ship.ViewModels.PrintEcInvoice;
    using Wms.Controllers;

    public class PrintEcInvoiceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_PrintEcInvoice.SearchConditions";

        private PrintEcInvoiceQuery _PrintEcInvoiceQuery;
        private Report _report = new Report();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintEcInvoiceController"/> class.
        /// </summary>
        public PrintEcInvoiceController()
        {
            this._PrintEcInvoiceQuery = new PrintEcInvoiceQuery();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        [HttpGet]
        public ActionResult Index()
        {
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, new PrintEcInvoiceConditions());
            return View(GetIndexViewName(), CreateNewViewModel());
        }

        public ActionResult Search(PrintEcInvoiceConditions SearchConditions)
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
        private PrintEcInvoiceConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new PrintEcInvoiceConditions() : Request.Cookies.Get<PrintEcInvoiceConditions>(COOKIE_SEARCHCONDITIONS) ?? new PrintEcInvoiceConditions();
            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        private ActionResult GetSearchResultView(PrintEcInvoiceConditions condition, bool indexFlag)
        {
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View(GetIndexViewName(), condition);
        }

        #endregion Private

        #region GetList

        /// <summary>
        /// Get コードより名称を取得
        /// <param name="centerId">センター</param>
        /// <param name="batchId">バッチ</param>
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult GetBatchName(string centerId, string batchId)
        {
            var batchName = _PrintEcInvoiceQuery.GetBatchName(centerId, batchId);

            return this.Json(new { batchName = batchName });
        }
        #endregion

        /// <summary>
        /// シングル新規発行
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PrintSigle(string centerId, 
            string batchId, string jan, string boxSize,
            string boxSize2 ,string boxSize3 , string boxSize4,
            string boxSize5, string boxSize6, string boxSize7,
            string boxSize8, string boxSize9, string boxSize10)
        {
            string fileNameNouhin = string.Empty;
            string fileNameDeli = string.Empty;
            string printCcdUrlDeli = string.Empty;
            string printCcdUrlNouhin = string.Empty;
            var errShipInstructId = "";

            PrintEcInvoiceConditions SearchConditions = new PrintEcInvoiceConditions { };
            SearchConditions.CenterId = centerId;
            SearchConditions.BatchNo = batchId;
            SearchConditions.Jan = jan;
            SearchConditions.BoxSize1 = boxSize;
            SearchConditions.BoxSize2 = boxSize2;
            SearchConditions.BoxSize3 = boxSize3;
            SearchConditions.BoxSize4 = boxSize4;
            SearchConditions.BoxSize5 = boxSize5;
            SearchConditions.BoxSize6 = boxSize6;
            SearchConditions.BoxSize7 = boxSize7;
            SearchConditions.BoxSize8 = boxSize8;
            SearchConditions.BoxSize9 = boxSize9;
            SearchConditions.BoxSize10 = boxSize10;

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);

            var status = 0;
            var errmessage = "";

            //印刷前処理
            _PrintEcInvoiceQuery.PrintSingleMain(SearchConditions, out status, out errmessage);
            if (status != 0)
            {
                errShipInstructId = SearchConditions.ErrShipInstructId;
                if (status == 50 || status == 51)
                {
                    SearchConditions.ErrMassagePop = errmessage;
                }
                else
                {
                    SearchConditions.ErrMassage1 = errmessage;
                }
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
                return Json(new { status, errmessage, errShipInstructId });
            }

            switch (SearchConditions.TransporterId)
            {
                case "01":  //ヤマト
                    fileNameDeli = "invoice_yamato";
                    break;
                case "02":  //佐川
                    fileNameDeli = "invoice_sagawa";
                    break;
                case "09":  //ヤマトネコポス
                    fileNameDeli = "invoice_nekoposu";
                    break;
            };

            int intKind = _report.GetPrintKind(SearchConditions);
            if (intKind == (int)PrintEcInvoiceConditions.EcClass.Rakuten)
            {
                fileNameNouhin = "PrintEcRakutenInvoice";
            }
            else if (intKind == (int)PrintEcInvoiceConditions.EcClass.WaKsnap)
            {
                fileNameNouhin = "PrintEcWaKsnapInvoice";
            }
            else
            {
                fileNameNouhin = "PrintEcInvoice";
            }
            //直接印刷処理
            printRun(SearchConditions, out printCcdUrlDeli, out printCcdUrlNouhin, out status);
            if (status != 0)
            {
                errmessage = Wms.Resources.MessageResource.ERR_PRINT;
                SearchConditions.ErrMassage1 = errmessage;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
                return Json(new { status, errmessage });
            }
            var shipInstructId = SearchConditions.ShipInstructId;
            return Json(new { printCcdUrlDeli, printCcdUrlNouhin, fileNameDeli, fileNameNouhin, status, shipInstructId });
        }

        /// <summary>
        /// オーダー・GAS新規発行
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PrintGasOrder(string centerId, string unitCnt, string boxNo,
            string ecShipClass, string boxSize,
            string boxSize2, string boxSize3, string boxSize4,
            string boxSize5, string boxSize6, string boxSize7,
            string boxSize8, string boxSize9, string boxSize10)
        {
            PrintEcInvoiceConditions SearchConditions = new PrintEcInvoiceConditions { };
            SearchConditions.CenterId = centerId;
            SearchConditions.UnitCnt = unitCnt;
            SearchConditions.BoxNo = boxNo;
            SearchConditions.BoxSize1 = boxSize;
            SearchConditions.BoxSize2 = boxSize2;
            SearchConditions.BoxSize3 = boxSize3;
            SearchConditions.BoxSize4 = boxSize4;
            SearchConditions.BoxSize5 = boxSize5;
            SearchConditions.BoxSize6 = boxSize6;
            SearchConditions.BoxSize7 = boxSize7;
            SearchConditions.BoxSize8 = boxSize8;
            SearchConditions.BoxSize9 = boxSize9;
            SearchConditions.BoxSize10 = boxSize10;

            if (ecShipClass == "1")
            {
                SearchConditions.EcShipClass = PrintEcInvoiceConditions.EcShipClasses.Orders;
            }
            else
            {
                SearchConditions.EcShipClass = PrintEcInvoiceConditions.EcShipClasses.Gases;
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            string fileNameNouhin = string.Empty;

            var status = 0;
            var errmessage = "";
            string fileNameDeli = "";
            string printCcdUrlDeli = string.Empty;
            string printCcdUrlNouhin = string.Empty;
            var errShipInstructId = "";

            //印刷前処理
            _PrintEcInvoiceQuery.PrintGasOrderMain(SearchConditions, out status, out errmessage);
            if (status != 0)
            {
                errShipInstructId = SearchConditions.ErrShipInstructId;
                if (status == 100 || status == 101)
                {
                    SearchConditions.ErrMassagePop = errmessage;
                }
                else
                {
                    SearchConditions.ErrMassage2 = errmessage;
                }
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
                return Json(new { status, errmessage, errShipInstructId });
            }

            switch (SearchConditions.TransporterId)
            {
                case "01":  //ヤマト
                    fileNameDeli = "invoice_yamato";
                    break;
                case "02":  //佐川
                    fileNameDeli = "invoice_sagawa";
                    break;
                case "09":  //ヤマトネコポス
                    fileNameDeli = "invoice_nekoposu";
                    break;
            };

            int intKind = _report.GetPrintKind(SearchConditions);
            if (intKind == (int)PrintEcInvoiceConditions.EcClass.Rakuten)
            {
                fileNameNouhin = "PrintEcRakutenInvoice";
            }
            else if (intKind == (int)PrintEcInvoiceConditions.EcClass.WaKsnap)
            {
                fileNameNouhin = "PrintEcWaKsnapInvoice";
            }
            else
            {
                fileNameNouhin = "PrintEcInvoice";
            }

            //直接印刷処理
            printRun(SearchConditions, out printCcdUrlDeli, out printCcdUrlNouhin, out status);
            if (status != 0)
            {
                errmessage = Wms.Resources.MessageResource.ERR_PRINT;
                SearchConditions.ErrMassage2 = errmessage;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
                return Json(new { status, errmessage });
            }
            var shipInstructId = SearchConditions.ShipInstructId;
            return Json(new { printCcdUrlDeli, printCcdUrlNouhin, fileNameDeli, fileNameNouhin, status, shipInstructId });
        }

        /// <summary>
        /// 再発行
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PrintReissue(string centerId, string unitCnt, 
            string boxNo,string shipInstructId, string prnClass, 
            string deliNo,int chkOldData, string boxSize,
            string boxSize2, string boxSize3, string boxSize4,
            string boxSize5, string boxSize6, string boxSize7,
            string boxSize8, string boxSize9, string boxSize10)
        {
            PrintEcInvoiceConditions SearchConditions = new PrintEcInvoiceConditions { };
            SearchConditions.CenterId = centerId;
            SearchConditions.UnitCnt = unitCnt;
            SearchConditions.BoxNo = boxNo;
            SearchConditions.ShipInstructId = shipInstructId;
            SearchConditions.DeliNo = deliNo;
            SearchConditions.BoxSize1 = boxSize;
            SearchConditions.BoxSize2 = boxSize2;
            SearchConditions.BoxSize3 = boxSize3;
            SearchConditions.BoxSize4 = boxSize4;
            SearchConditions.BoxSize5 = boxSize5;
            SearchConditions.BoxSize6 = boxSize6;
            SearchConditions.BoxSize7 = boxSize7;
            SearchConditions.BoxSize8 = boxSize8;
            SearchConditions.BoxSize9 = boxSize9;
            SearchConditions.BoxSize10 = boxSize10;


            if (chkOldData == 1)  //過去分含む
            {
                SearchConditions.ChkOldData = true;
            }
            else
            {
                SearchConditions.ChkOldData = false;
            }
            switch (prnClass)
            {
                case "0":
                    SearchConditions.PrnClass = PrintEcInvoiceConditions.PrnClasses.New;
                    break;
                case "1":
                    SearchConditions.PrnClass = PrintEcInvoiceConditions.PrnClasses.ReNouhinPrn;
                    break;
                case "2":
                    SearchConditions.PrnClass = PrintEcInvoiceConditions.PrnClasses.ReDeliPrn;
                    break;
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            var status = 0;
            var errmessage = "";
            int result = 0;
            string retPath = string.Empty;
            string controllerName = this.RouteData.Values["controller"].ToString();
            string fileNameDeli = string.Empty;
            var errShipInstructId = "";
            string styleNameNouhin = string.Empty;


            _PrintEcInvoiceQuery.PrintReissueMain(SearchConditions, out status, out errmessage);
            if (status != 0)
            {
                errShipInstructId = SearchConditions.ErrShipInstructId;
                SearchConditions.ErrMassage3 = errmessage;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
                return Json(new { status, errmessage, errShipInstructId });
            }

            //送り状再発行
            if (SearchConditions.PrnClass == PrintEcInvoiceConditions.PrnClasses.ReDeliPrn)
            {
                switch (SearchConditions.TransporterId)
                {
                    case "01":  //ヤマト
                        fileNameDeli = "invoice_yamato";

                        Reports.Export.PrintEcInvoiceYamato reportYamato = new Reports.Export.PrintEcInvoiceYamato(ReportTypes.Csv, SearchConditions);
                        reportYamato.Export();
                        // CSV作成
                        new CsvPrintFileCreate().CreateCsvFile(controllerName, reportYamato.DownloadFileName, reportYamato.FileContent);
                        string styleNameYamato = "invoice_yamato.sty";
                        //retPath = new CsvPrintFileCreate().OutputPrint(controllerName, styleNameYamato, reportYamato.DownloadFileName);

                        break;
                    case "02":  //佐川
                        fileNameDeli = "invoice_sagawa";
                        Reports.Export.PrintEcInvoiceSagawa reportSagawa = new Reports.Export.PrintEcInvoiceSagawa(ReportTypes.Csv, SearchConditions);
                        reportSagawa.Export();
                        // CSV作成
                        new CsvPrintFileCreate().CreateCsvFile(controllerName, reportSagawa.DownloadFileName, reportSagawa.FileContent);
                        string styleNameSagawa = "invoice_sagawa.sty";
                        //retPath = new CsvPrintFileCreate().OutputPrint(controllerName, styleNameSagawa, reportSagawa.DownloadFileName);
                        break;
                    case "09":  //ヤマトネコポス
                        fileNameDeli = "invoice_nekoposu";
                        Reports.Export.PrintEcInvoiceNekoposu reportNekoposu = new Reports.Export.PrintEcInvoiceNekoposu(ReportTypes.Csv, SearchConditions);
                        reportNekoposu.Export();
                        // CSV作成
                        new CsvPrintFileCreate().CreateCsvFile(controllerName, reportNekoposu.DownloadFileName, reportNekoposu.FileContent);
                        string styleNameNekoposu = "invoice_nekoposu.sty";
                        //retPath = new CsvPrintFileCreate().OutputPrint(controllerName, styleNameNekoposu, reportNekoposu.DownloadFileName);
                        break;
                };
                if (int.TryParse(retPath, out result))
                {
                    status = result;
                    errmessage = Wms.Resources.MessageResource.ERR_PRINT;
                    SearchConditions.ErrMassage3 = errmessage;
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
                    return Json(new { status, errmessage });
                }
                retPath = AppConfig.PrintCcdUrl + Request.Url.Host + retPath;
            }

            //納品書再発行
            if (SearchConditions.PrnClass == PrintEcInvoiceConditions.PrnClasses.ReNouhinPrn)
            {

                int intKind = _report.GetPrintKind(SearchConditions);
                if (intKind == (int)PrintEcInvoiceConditions.EcClass.Rakuten)
                {
                    //①楽天納品書
                    Reports.Export.PrintEcRakutenInvoice reportNouhinRakuten = new Reports.Export.PrintEcRakutenInvoice(ReportTypes.Csv, SearchConditions);
                    reportNouhinRakuten.Export();
                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, reportNouhinRakuten.DownloadFileName, reportNouhinRakuten.FileContent);
                    styleNameNouhin = "PrintEcRakutenInvoice.sty";
                    //retPath = new CsvPrintFileCreate().OutputPDF(controllerName, styleNameNouhin, reportNouhinRakuten.DownloadFileName);
                }
                else if (intKind == (int)PrintEcInvoiceConditions.EcClass.WaKsnap)
                {
                    //②WaKsnap納品書
                    Reports.Export.PrintEcWaKsnapInvoice reportNouhinWaKsnap = new Reports.Export.PrintEcWaKsnapInvoice(ReportTypes.Csv, SearchConditions);
                    reportNouhinWaKsnap.Export();
                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, reportNouhinWaKsnap.DownloadFileName, reportNouhinWaKsnap.FileContent);
                    styleNameNouhin = "PrintEcWaKsnapInvoice.sty";
                    //retPath = new CsvPrintFileCreate().OutputPDF(controllerName, styleNameNouhin, reportNouhinWaKsnap.DownloadFileName);
                }
                else
                {
                    //③EC納品書
                    Reports.Export.PrintEcInvoice reportNouhin = new Reports.Export.PrintEcInvoice(ReportTypes.Csv, SearchConditions);
                    reportNouhin.Export();
                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, reportNouhin.DownloadFileName, reportNouhin.FileContent);
                    styleNameNouhin = "PrintEcInvoice.sty";
                    //retPath = new CsvPrintFileCreate().OutputPDF(controllerName, styleNameNouhin, reportNouhin.DownloadFileName);

                }

                if (int.TryParse(retPath, out result))
                {
                    status = result;
                    errmessage = Wms.Resources.MessageResource.ERR_PRINT;
                    SearchConditions.ErrMassage3 = errmessage;
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
                    return Json(new { status, errmessage });
                }
            }
            return Json(new { status, prnClass, retPath, fileNameDeli });
        }

        /// <summary>
        /// 納品書・送り状新規発行
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        public void printRun(PrintEcInvoiceConditions SearchConditions, out string url_deli, out string url_nouhin, out int status)
        {
            status = 0;
            int result = 0;
            string retDeliUrl = string.Empty;
            string retNouUrl = string.Empty;
            string controllerName = this.RouteData.Values["controller"].ToString();
            string styleNameNouhin = string.Empty;

            //URL取得
            string strPathAndQuery = AppConfig.PrintCcdUrl + Request.Url.Host;
             switch (SearchConditions.TransporterId)
                {
                case "01":  //ヤマト
                    Reports.Export.PrintEcInvoiceYamato reportYamato = new Reports.Export.PrintEcInvoiceYamato(ReportTypes.Csv, SearchConditions);
                    reportYamato.Export();
                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, reportYamato.DownloadFileName, reportYamato.FileContent);
                    string styleNameYamato = "invoice_yamato.sty";
                    //retDeliUrl = new CsvPrintFileCreate().OutputPrint(controllerName, styleNameYamato, reportYamato.DownloadFileName);
                    if (int.TryParse(retDeliUrl, out result))
                    {
                        status = result;
                        url_deli = "";
                        url_nouhin = "";
                        return;
                    }
                    break;
                case "02":  //佐川送り状発行
                    Reports.Export.PrintEcInvoiceSagawa reportSagawa = new Reports.Export.PrintEcInvoiceSagawa(ReportTypes.Csv, SearchConditions);
                    reportSagawa.Export();
                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, reportSagawa.DownloadFileName, reportSagawa.FileContent);
                    string styleNameSagawa = "invoice_sagawa.sty";
                    //retDeliUrl = new CsvPrintFileCreate().OutputPrint(controllerName, styleNameSagawa, reportSagawa.DownloadFileName);
                    if (int.TryParse(retDeliUrl, out result))
                    {
                        status = result;
                        url_deli = "";
                        url_nouhin = "";
                        return;
                    }
                   break;
                case "09":  //ヤマトネコポス
                    Reports.Export.PrintEcInvoiceNekoposu reportNekoposu = new Reports.Export.PrintEcInvoiceNekoposu(ReportTypes.Csv, SearchConditions);
                    reportNekoposu.Export();
                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, reportNekoposu.DownloadFileName, reportNekoposu.FileContent);
                    string styleNameNekoposu = "invoice_nekoposu.sty";
                    //retDeliUrl = new CsvPrintFileCreate().OutputPrint(controllerName, styleNameNekoposu, reportNekoposu.DownloadFileName);
                    if (int.TryParse(retDeliUrl, out result))
                    {
                        status = result;
                        url_deli = "";
                        url_nouhin = "";
                        return;
                    }
                    break;
            };
            //
            int intKind = _report.GetPrintKind(SearchConditions);
            if (intKind == (int)PrintEcInvoiceConditions.EcClass.Rakuten)
            {
                //①楽天納品書
                Reports.Export.PrintEcRakutenInvoice reportNouhinRakuten = new Reports.Export.PrintEcRakutenInvoice(ReportTypes.Csv, SearchConditions);
                reportNouhinRakuten.Export();
                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, reportNouhinRakuten.DownloadFileName, reportNouhinRakuten.FileContent);
                styleNameNouhin = "PrintEcRakutenInvoice.sty";
                //retNouUrl = new CsvPrintFileCreate().OutputPrint(controllerName, styleNameNouhin, reportNouhinRakuten.DownloadFileName);
            }
            else if (intKind == (int)PrintEcInvoiceConditions.EcClass.WaKsnap)
            {
                //②WaKsnap納品書
                Reports.Export.PrintEcWaKsnapInvoice reportNouhinWaKsnap = new Reports.Export.PrintEcWaKsnapInvoice(ReportTypes.Csv, SearchConditions);
                reportNouhinWaKsnap.Export();
                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, reportNouhinWaKsnap.DownloadFileName, reportNouhinWaKsnap.FileContent);
                styleNameNouhin = "PrintEcWaKsnapInvoice.sty";
                //retNouUrl = new CsvPrintFileCreate().OutputPrint(controllerName, styleNameNouhin, reportNouhinWaKsnap.DownloadFileName);
            }
            else
            {
                //③納品書発行
                Reports.Export.PrintEcInvoice reportNouhin = new Reports.Export.PrintEcInvoice(ReportTypes.Csv, SearchConditions);
                reportNouhin.Export();
                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, reportNouhin.DownloadFileName, reportNouhin.FileContent);
                styleNameNouhin = "PrintEcInvoice.sty";
                //retNouUrl = new CsvPrintFileCreate().OutputPrint(controllerName, styleNameNouhin, reportNouhin.DownloadFileName);
            }
            if (int.TryParse(retNouUrl, out result))
            {
                status = result;
                url_deli = "";
                url_nouhin = "";
                return;
            }

            url_deli = strPathAndQuery + retDeliUrl;
            url_nouhin = strPathAndQuery + retNouUrl;
        }

        /// <summary>
        /// 再発行時PDF出力
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintReRun(PrintEcInvoiceConditions SearchConditions)
        {
            return File(SearchConditions.Ret, "application/pdf");
        }

        private static PrintEcInvoiceConditions CreateNewViewModel()
        {
            return new PrintEcInvoiceConditions { };
        }

        private static string GetIndexViewName()
        {
            return "~/Areas/Ship/Views/PrintEcInvoice/Index.cshtml";
        }

    }
}