namespace Wms.Areas.Ship.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;

    using Wms.Areas.Ship.Query.PrintInvoice;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.PrintInvoice;
    using Wms.Controllers;
    using Wms.Models;

    public class PrintInvoiceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_PrintInvoice.SearchConditions";

        private PrintInvoiceQuery _PrintInvoiceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintInvoiceController"/> class.
        /// </summary>
        public PrintInvoiceController()
        {
            this._PrintInvoiceQuery = new PrintInvoiceQuery();
        }

        #endregion Constants

        #region Search

        [HttpGet]
        public ActionResult Index()
        {
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, new PrintInvoiceConditions());
            return View(GetIndexViewName(), CreateNewViewModel());
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(PrintInvoiceConditions SearchConditions)
        {
            ModelState.Clear();
            return this.GetSearchResultView(SearchConditions, false);
        }

        #endregion Search

        #region Private

        [HttpPost]
        [ValidateAntiForgeryToken]
        private ActionResult GetSearchResultView(PrintInvoiceConditions condition, bool indexFlag)
        {
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            return this.View(GetIndexViewName(), condition);
        }

        #endregion Private

        #region Print

        /// <summary>
        /// 新規発行
        /// </summary>
        /// <param name="userCenterId"></param>
        /// <param name="prnClass"></param>
        /// <param name="boxSize"></param>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PrintNew(string userCenterId, string prnClass, string boxSize, string boxNo)
        {
            PrintInvoiceConditions SearchConditions = new PrintInvoiceConditions
            {
                UserCenterId = userCenterId,
                BoxSize = boxSize,
                BoxNo = boxNo
            };
            switch (prnClass)
            {
                case "0":
                    SearchConditions.PrnClass = PrintInvoiceConditions.PrnClasses.New;
                    break;
                case "1":
                    SearchConditions.PrnClass = PrintInvoiceConditions.PrnClasses.ReNouhinPrn;
                    break;
                case "2":
                    SearchConditions.PrnClass = PrintInvoiceConditions.PrnClasses.ReDeliPrn;
                    break;
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);

            //印刷前処理(チェック＆梱包実績更新)
            _PrintInvoiceQuery.PrintBeforeMain(SearchConditions, out int status, out string errmessage);
            if (status != 0)
            {
                SearchConditions.ErrMassage = errmessage;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
                return Json(new { status, errmessage });
            }

            string retDeliUrl = string.Empty;
            string retNouUrl = string.Empty;
            string message = string.Empty;
            List<WfrReport> wfrReports = new List<WfrReport>();
            int result;
            //送り状発行
            if (SearchConditions.InvoicePrintFlag == 1)
            {
                retDeliUrl = GetDeliUrl(SearchConditions);
            }

            if (SearchConditions.NouhinPrnFlag == "0")
            {
                retNouUrl = GetNouUrl(SearchConditions, false);
            }

            //納品書発行不要かつ送り状発行不要
            if (SearchConditions.NouhinPrnFlag == "8" && SearchConditions.InvoicePrintFlag == 0)
            {
                message = Resources.PrintInvoiceResource.PrintMassage;
            }

            return Json(new { retDeliUrl, retNouUrl, status, prnClass, message });
            //return Json(new { retDeliUrl = GetWfrSortPrintUrl(wfrReports), retNouUrl = "", status, prnClass, message });
        }

        /// <summary>
        /// 海外アソート出荷か判定をする
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckAssort(PrintInvoiceConditions PrintInvoiceConditions)
        {
            // a) 海外アソートか判定をする
            var AssortList = _PrintInvoiceQuery.GetAssortList(PrintInvoiceConditions.BoxNo);
            if (AssortList.InvoiceNo is null)
            {
                return Json(new { Status = -1, Message = PrintInvoiceResource.ErrorNotExist, DialogFlg = false });
            }
            else if (AssortList.ShipClass != "1")
            {
                return Json(new { Status = -1, Message = PrintInvoiceResource.ErrorNotShipStore, DialogFlg = false });
            }
            else if (AssortList.ArriveBranch is null)
            {
                return Json(new { Status = -1, Message = PrintInvoiceResource.ErrorNoArrivePlan, DialogFlg = false });
            }

            // 既に入荷実績が存在するか判定
            if (_PrintInvoiceQuery.IsExistsArriveResult(PrintInvoiceConditions.BoxNo))
            {
                return Json(new { Status = -1, Message = PrintInvoiceResource.ErrorExistsArriveResult, DialogFlg = false });
            }

            // a`)他の納品書のケーススキャン時はエラーとする
            if (!PrintInvoiceConditions.ChkOtherListScan && PrintInvoiceConditions.ScanAssortViews != null)
            {
                if (PrintInvoiceConditions.ScanAssortViews.Where(x => x.InvoiceNo != AssortList.InvoiceNo).Any())
                {
                    return Json(new { Status = -1, Message = PrintInvoiceResource.ErrorOtherInvoiceNo, DialogFlg = false });
                }
            }

            // b) 出荷保留区分を取得する
            var ShippingHoldClass = _PrintInvoiceQuery.GetShipHoldClass(AssortList);

            // 入荷実績を取得
            var ArriveResult = _PrintInvoiceQuery.CountArriveResult(AssortList);

            // c) ケース内で最初のケースの場合は確認メッセージを表示する
            if (ArriveResult == null)
            {
                // 締め年月日を取得
                var ClosedDate = _PrintInvoiceQuery.GetClosedDate();

                return Json(new { Status = 0, Message = "", DialogFlg = true, AssortList.CenterId, AssortList.InvoiceNo, AssortList.SlipDate, ShippingHoldClass, ClosedDate });
            }
            else
            {
                return Json(new { Status = 0, Message = "", DialogFlg = false, AssortList.CenterId, AssortList.InvoiceNo, ArriveResult.SlipDate, ShippingHoldClass });
            }
        }

        /// <summary>
        /// 入荷伝票日付確認メッセージ 確定ボタン押下時
        /// </summary>
        /// <param name="box_no"></param>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConfirmDialog(PrintInvoiceConditions PrintInvoiceConditions)
        {
            int status;
            string message = string.Empty;
            string retDeliUrl = string.Empty;
            string retNouUrl = string.Empty;

            // 伝票日付が未来日の場合は「1:出荷保留」にする
            if (PrintInvoiceConditions.SlipDate.Date > DateTime.Today) PrintInvoiceConditions.ShippingHoldClass = 1;

            // 入荷・出荷実績を作成する
            _PrintInvoiceQuery.CreateResults(PrintInvoiceConditions, out status, out message);
            // エラーだった場合はメッセージを表示して中断
            if (status != (byte)ProcedureStatus.Success)
            {
                return Json(new { Status = status, Message = message });
            }

            // 即出荷のデータは印刷を行う
            if (PrintInvoiceConditions.ShippingHoldClass == 0)
            {
                // 梱包実績更新
                _PrintInvoiceQuery.PrintBeforeMain(PrintInvoiceConditions, out status, out message);
                // エラーだった場合はメッセージを表示して中断
                if (status != (byte)ProcedureStatus.Success)
                {
                    return Json(new { Status = status, Message = message });
                }

                // 送り状作成・印刷URL取得
                if (PrintInvoiceConditions.InvoicePrintFlag == 1)
                {
                    retDeliUrl = GetDeliUrl(PrintInvoiceConditions);
                }
                // 納品書作成・印刷URL取得
                if (PrintInvoiceConditions.NouhinPrnFlag == "0")
                {
                    retNouUrl = GetNouUrl(PrintInvoiceConditions, false);
                }
            }

            // 明細情報　取得
            var assort = _PrintInvoiceQuery.GetView(PrintInvoiceConditions.InvoiceNo);

            // メッセージをセット
            message = CreateCompletedMessage(assort);

            // 出荷保留区分「1」の場合はメッセージを追加
            if (PrintInvoiceConditions.ShippingHoldClass == 1)
            {
                message += PrintInvoiceResource.AssortMessage3;
            }

            // 作業中リスト更新用
            var ScanAssortViews = PrintInvoiceConditions.ScanAssortViews;

            // 新規で明細に追加するのか判定をする
            bool CreateFlag = true;
            bool WorkInvoiceFlag = false;

            if (ScanAssortViews.Where(x => x.InvoiceNo == assort.InvoiceNo).Any())
            {
                //既にスキャン済の納品書番号
                CreateFlag = false;

                // 1行目の伝票以外を作業時は文字の色を変更する
                if (ScanAssortViews.First().InvoiceNo != assort.InvoiceNo) WorkInvoiceFlag = true;
            }
            // 未作業の伝票がスキャンされた場合
            if (CreateFlag)
            {
                // 1行目の伝票以外を作業時は文字の色を変更する
                if (ScanAssortViews.Any()) WorkInvoiceFlag = true;

                if (assort.PlanQty - assort.ResultQty > 0)
                {
                    assort.SlipDate = PrintInvoiceConditions.SlipDate.Date.ToString("yyyy/MM/dd");
                    ScanAssortViews.Add(assort);
                }
            }

            return Json(new { retDeliUrl, retNouUrl, Status = 0, Message = message, WorkInvoiceFlag, PrintInvoiceConditions });
        }

        /// <summary>
        /// 海外アソート　部分ビュー取得
        /// </summary>
        /// <param name="ScanAssortView"></param>
        /// <returns></returns>
        public ActionResult GetAssortView(PrintInvoiceConditions PrintInvoiceConditions)
        {
            ModelState.Clear();

            var UpdateList = new List<ScanAssortView>();

            // 明細のメッセージをセット
            foreach (var x in PrintInvoiceConditions.ScanAssortViews)
            {
                var target = _PrintInvoiceQuery.GetView(x.InvoiceNo);
                if (target.PlanQty - target.ResultQty > 0)
                {
                    x.Message = CreateCompletedMessage(target);
                    UpdateList.Add(x);
                }
            }

            PrintInvoiceConditions.ScanAssortViews = UpdateList;
            return View("~/Areas/Ship/Views/PrintInvoice/_ScanAssortView.cshtml", PrintInvoiceConditions);
        }

        /// <summary>
        /// 完了メッセージ生成
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private static string CreateCompletedMessage(ScanAssortView assortView)
        {
            if (assortView.PlanQty - assortView.ResultQty > 0)
            {
                return string.Format(PrintInvoiceResource.AssortMessage1, (assortView.PlanQty - assortView.ResultQty), assortView.PlanQty, assortView.InvoiceNo, assortView.CenterName);
            }
            else
            {
                return string.Format(PrintInvoiceResource.AssortMessage2, assortView.InvoiceNo, assortView.CenterName);
            }
        }

        /// <summary>
        /// 出荷残一覧　取得処理
        /// </summary>
        /// <param name="InvoiceNo"></param>
        /// <returns></returns>
        public ActionResult GetZanStoreList(string InvoiceNo)
        {
            ZanShipStore vm = new ZanShipStore()
            {
                ZanShipStores = _PrintInvoiceQuery.GetZanStoreList(InvoiceNo)
            };
            return View("~/Areas/Ship/Views/PrintInvoice/_ShipToStoreListViewDetail.cshtml", vm);
        }

        /// <summary>
        ///  送り状帳票作成・印刷用URL作成
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public string GetDeliUrl(PrintInvoiceConditions conditions)
        {
            string printer;
            string controllerName = this.RouteData.Values["controller"].ToString();
            switch (conditions.TransporterId)
            {
                case "02":  //佐川
                    Reports.Export.PrintInvoiceSagawa reportSagawa = new Reports.Export.PrintInvoiceSagawa(ReportTypes.Csv, conditions);
                    reportSagawa.Export();

                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, reportSagawa.DownloadFileName, reportSagawa.FileContent);

                    //プリンタ名取得
                    printer = _PrintInvoiceQuery.GetPrinterName(conditions.UserCenterId, "SAGAWA_INVOICE");
                    //印刷URL取得
                    return GetWfrPrintUrl("InvoiceSagawa.wfr", reportSagawa.DownloadFileName, printer);

                case "03":  //浪速
                    var report = new Reports.Export.PrintInvoiceNaniwa(ReportTypes.Csv, conditions);
                    report.Export();

                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                    //プリンタ名取得
                    printer = _PrintInvoiceQuery.GetPrinterName(conditions.UserCenterId, "NANIWA_INVOICE");
                    //印刷URL取得
                    return GetWfrPrintUrl("InvoiceNaniwa.wfr", report.DownloadFileName, printer);

                case "08":  //ワールドサプライ
                    var reportWorld = new Reports.Export.PrintInvoiceWorldSupply(ReportTypes.Csv, conditions);
                    reportWorld.Export();

                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, reportWorld.DownloadFileName, reportWorld.FileContent);

                    //プリンタ名取得
                    printer = _PrintInvoiceQuery.GetPrinterName(conditions.UserCenterId, "WORLD_INVOICE");
                    //印刷URL取得
                    return GetWfrPrintUrl("InvoiceWorldSupply.wfr", reportWorld.DownloadFileName, printer);
            };
            return string.Empty;
        }

        /// <summary>
        ///  納品書帳票作成・印刷用URL取得
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public string GetNouUrl(PrintInvoiceConditions conditions, bool preview)
        {
            string controllerName = this.RouteData.Values["controller"].ToString();
            //納品書発行
            Reports.Export.PrintBtoBInvoice reportNouhin = new Reports.Export.PrintBtoBInvoice(ReportTypes.Csv, conditions);
            reportNouhin.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, reportNouhin.DownloadFileName, reportNouhin.FileContent);

            //プリンタ名取得
            string printer = string.Empty;
            if (!preview)
            {
                printer = _PrintInvoiceQuery.GetPrinterName(conditions.UserCenterId, "DELIVERY_SLIP");
            }
            //印刷URL取得
            return GetWfrPrintUrl("PrintBtoBInvoice.wfr", reportNouhin.DownloadFileName, printer);
        }

        /// <summary>
        /// 再発行処理
        /// </summary>
        /// <param name="userCenterId"></param>
        /// <param name="prnClass"></param>
        /// <param name="boxSize"></param>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PrintRe(string userCenterId, string prnClass, string boxSize, string boxNo, string deliNo, int chkOldData, bool confirmed)
        {
            PrintInvoiceConditions SearchConditions = new PrintInvoiceConditions
            {
                UserCenterId = userCenterId,
                BoxSize = boxSize,
                BoxNo = boxNo,
                DeliNo = deliNo,
                Confirmed = confirmed
            };
            if (chkOldData == 1)
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
                    SearchConditions.PrnClass = PrintInvoiceConditions.PrnClasses.New;
                    break;
                case "1":
                    SearchConditions.PrnClass = PrintInvoiceConditions.PrnClasses.ReNouhinPrn;
                    break;
                case "2":
                    SearchConditions.PrnClass = PrintInvoiceConditions.PrnClasses.ReDeliPrn;
                    break;
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);

            //印刷前処理(チェック＆梱包実績更新)
            _PrintInvoiceQuery.PrintBeforeMain(SearchConditions, out int status, out string errmessage);
            if (status != 0)
            {
                SearchConditions.ErrMassage = errmessage;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
                return Json(new { status, errmessage });
            }

            string retPath = string.Empty;
            string controllerName = this.RouteData.Values["controller"].ToString();
            List<WfrReport> wfrReports = new List<WfrReport>();
            int result;
            //送り状再発行
            if (SearchConditions.InvoicePrintFlag == 1 && SearchConditions.PrnClass == PrintInvoiceConditions.PrnClasses.ReDeliPrn)
            {
                retPath = GetDeliUrl(SearchConditions);
            }

            //納品書再発行
            if (SearchConditions.PrnClass == PrintInvoiceConditions.PrnClasses.ReNouhinPrn)
            {
                retPath = GetNouUrl(SearchConditions, true);
            }
            //if (int.TryParse(retPath, out result))
            //{
            //    status = result;
            //    errmessage = Wms.Resources.MessageResource.ERR_PRINT;
            //    SearchConditions.ErrMassage = errmessage;
            //    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            //    return Json(new { status, errmessage });
            //}
            return Json(new { status, prnClass, retPath });
        }

        /// <summary>
        /// 再発行時PDF出力
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintReRun(PrintInvoiceConditions SearchConditions)
        {
            return File(SearchConditions.Ret, "application/pdf");
        }

        #endregion

        private static PrintInvoiceConditions CreateNewViewModel()
        {
            return new PrintInvoiceConditions
            {
                ErrMassage = "",
                UserCenterId = MvcDbContext.Current.User
                                        .Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.UserId == Common.Profile.User.UserId)
                                        .Select(x => x.CenterId)
                                        .Single()
            };
        }

        private static string GetIndexViewName()
        {
            return "~/Areas/Ship/Views/PrintInvoice/Index.cshtml";
        }
    }
}