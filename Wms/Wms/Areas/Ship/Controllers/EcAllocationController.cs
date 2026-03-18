namespace Wms.Areas.Ship.Controllers
{
    using Share.Common;
    using Share.Extensions.Classes;
    using System.Collections;
    using System.Text;
    using System.Web.Mvc;
    using System.Web;
    using System.Web.UI;
    using System.Threading;
    using Wms.Areas.Ship.Query.EcAllocation;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.EcAllocation;
    using Wms.Controllers;
    using Wms.Hubs;
    using Wms.Models;
    using Wms.Resources;
    using Wms.Areas.Ship.ViewModels.PrintBatch;
    using System.Linq;
    using Wms.Areas.Ship.Query.PrintBatch;
    using System;

    public class EcAllocationController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_EcAllocation01.SearchConditions";

        private EcAllocationQuery _EcAllocationQuery;

        private PrintBatchQuery _PrintBatchQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="EcAllocationController"/> class.
        /// </summary>
        public EcAllocationController()
        {
            this._EcAllocationQuery = new EcAllocationQuery();
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
        public ActionResult UpdateSuc()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
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
        public ActionResult Search(EcAllocationSearchConditions SearchConditions)
        {
            EcAllocationSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Private
        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private EcAllocationSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new EcAllocationSearchConditions() : Request.Cookies.Get<EcAllocationSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new EcAllocationSearchConditions();
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
        private ActionResult GetSearchResultView(EcAllocationSearchConditions searchConditions, bool indexFlag)
        {
            // 画面選択行更新用
            if (!indexFlag && searchConditions.SearchType == Common.SearchTypes.SortPage)
            {
                _EcAllocationQuery.UpdateShpEcAllocation(searchConditions.EcAllocations);
            }

            // 作成処理&検索表示
            var vm = new EcAllocationViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new EcAllocationResult() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _EcAllocationQuery.InsertShpEcAllocation(searchConditions) : true) ? new EcAllocationResult()
                {
                    EcAllocations = _EcAllocationQuery.GetData(searchConditions)
                }
                : new EcAllocationResult()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.EcAllocations.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.EcAllocations = null;
                }
            }

            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _EcAllocationQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/EcAllocation/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        /// <summary>
        /// ECユニットカンバン帳票を印刷
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        private string ECPrint(EcAllocationSearchConditions SearchConditions, string batchNo)
        {
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            PrintBatchSearchConditions conditons = new PrintBatchSearchConditions();
            conditons.CenterId = SearchConditions.CenterId;
            conditons.AllocGroupNo = batchNo;
            Reports.Export.EcUnitBoard report = new Reports.Export.EcUnitBoard(ReportTypes.Csv, conditons);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = "EcUnitBoard.sty";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);
            return ret;
        }

        /// <summary>
        /// GASカンバン印刷
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        private string GASPrint(EcAllocationSearchConditions SearchConditions, string batchNo)
        {
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            PrintBatchSearchConditions conditons = new PrintBatchSearchConditions();
            conditons.CenterId = SearchConditions.CenterId;
            conditons.AllocGroupNo = batchNo;
            Reports.Export.EcGASBoard report = new Reports.Export.EcGASBoard(ReportTypes.Csv, conditons);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = "EcGASBoard.sty";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);
            return ret;
        }

        #endregion Private

        #region Selected
        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(EcAllocationSearchConditions searchConditions)
        {
            ModelState.Clear();
            // 全選択
            _EcAllocationQuery.ShpEcAllocationAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new EcAllocationViewModel
            {
                SearchConditions = searchConditions,
                Results = new EcAllocationResult()
                {
                    EcAllocations = _EcAllocationQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _EcAllocationQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/EcAllocation/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(EcAllocationSearchConditions searchConditions)
        {
            ModelState.Clear();
            // 全解除
            _EcAllocationQuery.ShpEcAllocationAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new EcAllocationViewModel
            {
                SearchConditions = searchConditions,
                Results = new EcAllocationResult()
                {
                    EcAllocations = _EcAllocationQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _EcAllocationQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/EcAllocation/Index.cshtml", vm);
        }
        #endregion

        #region ロード処理

        /// <summary>
        /// 引当エラーリスト(商品別)
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ItemPrint(EcAllocationSearchConditions SearchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            Reports.Export.EcAllocationItemReportForCsv report = new Reports.Export.EcAllocationItemReportForCsv(ReportTypes.Csv, SearchConditions);
            report.Export();
           
            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = "EcAllocationItem.sty";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);


            return this.File(ret, "application/pdf");
        }

        /// <summary>
        /// 引当エラーリスト(商品別)
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ItemPrintCheck(EcAllocationSearchConditions SearchConditions)
        {
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            Reports.Export.EcAllocationItemReportForCsv report = new Reports.Export.EcAllocationItemReportForCsv(ReportTypes.Csv, SearchConditions);
            report.Export();
            if (!report.GetData().Any())
            {

                return this.Json(1);
            }

            return this.Json(0);
        }

        

        /// <summary>
        /// 引当エラーリスト(注文別)
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderPrint(EcAllocationSearchConditions SearchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            Reports.Export.EcAllocationOrderReportForCsv report = new Reports.Export.EcAllocationOrderReportForCsv(ReportTypes.Csv, SearchConditions);
            report.Export();
           
            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = "EcAllocationOrder.sty";
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);


            return this.File(ret, "application/pdf");
        }

        /// <summary>
        /// 引当エラーリスト(商品別)
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderPrintCheck(EcAllocationSearchConditions SearchConditions)
        {
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            Reports.Export.EcAllocationOrderReportForCsv report = new Reports.Export.EcAllocationOrderReportForCsv(ReportTypes.Csv, SearchConditions);
            report.Export();
            if (!report.GetData().Any())
            {
                return this.Json(1);
            }

            return this.Json(0);
        }

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ItemDownload(EcAllocationSearchConditions SearchConditions)
        {
            Reports.Export.EcAllocationItemReport report = new Reports.Export.EcAllocationItemReport(ReportTypes.Excel, SearchConditions);
            report.Export();
            if (!report.GetData().Any())
            {
                ViewBag.Status = ProcedureStatus.Error;
                ViewBag.ErrorMessage = Share.Common.Resources.MessagesResource.MSG_NOT_FOUND;
                // 検索
                return Index();
            }
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrderDownload(EcAllocationSearchConditions SearchConditions)
        {
            Reports.Export.EcAllocationOrderReport report = new Reports.Export.EcAllocationOrderReport(ReportTypes.Excel, SearchConditions);
            report.Export();
            if (!report.GetData().Any())
            {
                ViewBag.Status = ProcedureStatus.Error;
                ViewBag.ErrorMessage = Share.Common.Resources.MessagesResource.MSG_NOT_FOUND;
                // 検索
                return Index();
            }
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        [HttpPost]
        public ActionResult ReportPrint(EcAllocationSearchConditions SearchConditions)
        {
            return this.File(SearchConditions.Ret, "application/pdf");
        }
        #endregion

        #region Detail
        /// <summary>
        /// EC引当明細画面
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult Detail(long Seq, long LineNo)
        {
            EcAllocationDetailViewModel vm = _EcAllocationQuery.GetDetailData(Seq, LineNo);

            return this.PartialView("~/Areas/Ship/Views/EcAllocation/Detail.cshtml", vm);
        }
        #endregion

        #region 更新処理
        /// <summary>
        /// 引当実行
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EcAllocUpdateSearch(EcAllocationSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _EcAllocationQuery.UpdateShpEcAllocation(searchConditions.EcAllocations);
            // 実績更新
            _EcAllocationQuery.AllocationUpdate(searchConditions);

            // 非同期処理開始
            int i = 0;
            long wkId = searchConditions.Seq;
            searchConditions.IndicateTitle = MessageResource.ALLOC_DOING;
            for (; ; )
            {
                Functions.SendProgress(searchConditions.IndicateTitle, i, 100, searchConditions.ProcessColor);
                Thread.Sleep(100);

                // searchConditions.Seq
                if (i == 100)
                {
                    Thread.Sleep(500);
                    break;
                }

                if (_EcAllocationQuery.GetAllocStatus(wkId).Status == 1)
                {
                    i = 100;
                    searchConditions.IndicateTitle = MessageResource.NORMAL_END;

                }
                else if (_EcAllocationQuery.GetAllocStatus(wkId).Status == 9)
                {
                    i = 100;
                    // 異常の状態を設定する
                    searchConditions.IndicateTitle = MessageResource.ERROR_END;
                    searchConditions.ProcessColor = "red";

                }
                else
                {
                    i = _EcAllocationQuery.GetAllocStatus(wkId).Progress > i ? i + 1 : _EcAllocationQuery.GetAllocStatus(wkId).Progress;
                }
            }

            string msg = _EcAllocationQuery.GetAllocStatus(wkId).Message;
            ProcedureStatus status = (ProcedureStatus)_EcAllocationQuery.GetAllocStatus(wkId).Status2;
            if (status == ProcedureStatus.Success || status == ProcedureStatus.NoAllocData)
            {
                status = ProcedureStatus.Success;
                var message = string.Empty;
                var batchNo = _EcAllocationQuery.GetAllocStatus(wkId).BatchNo;

                if (string.IsNullOrWhiteSpace(batchNo)
                    || (searchConditions.Single && !searchConditions.Order && !searchConditions.Gas && !searchConditions.All)
                    || searchConditions.OrderBatchNo 
                    || searchConditions.AllOrderBatchNo)
                {
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                    TempData[AppConst.SUCCESS] = msg;
                    return RedirectToAction("UpdateSuc");
                }
                else if (searchConditions.BatchNoInUnit)
                {
                    searchConditions.Ret = GASPrint(searchConditions, batchNo);
                    //印字フラグ更新
                    var printConditions = new PrintBatchSearchConditions();
                    printConditions.CenterId = searchConditions.CenterId;
                    printConditions.AllocGroupNo = batchNo;
                    printConditions.No = 5;
                    _PrintBatchQuery.PrintFlagUpdate(printConditions, out status, out message);
                    if (status != ProcedureStatus.Success)
                    {
                        ViewBag.Status = status;
                        ViewBag.ErrorMessage = message;
                        CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                        // 検索
                        return Search(searchConditions);
                    }
                    searchConditions.Print = "Print";
                    TempData[AppConst.SUCCESS] = msg;
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                    // 検索
                    return RedirectToAction("UpdateSuc");
                }
                else
                {
                    searchConditions.Ret = ECPrint(searchConditions, batchNo);
                    //印字フラグ更新
                    var printConditions = new PrintBatchSearchConditions();
                    printConditions.CenterId = searchConditions.CenterId;
                    printConditions.AllocGroupNo = batchNo;
                    printConditions.No = 4;
                    _PrintBatchQuery.PrintFlagUpdate(printConditions, out status, out message);
                    if (status != ProcedureStatus.Success)
                    {
                        ViewBag.Status = status;
                        ViewBag.ErrorMessage = message;
                        CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                        // 検索
                        return Search(searchConditions);
                    }
                    searchConditions.Print = "Print";
                    TempData[AppConst.SUCCESS] = msg;
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                    // 検索
                    return RedirectToAction("UpdateSuc");
                }
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = msg;
                // 検索
                return Search(searchConditions);
            }
        }

        /// <summary>
        /// 引当保持分解除
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EcAllocRelieveSearch(EcAllocationSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _EcAllocationQuery.UpdateShpEcAllocation(searchConditions.EcAllocations);
            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _EcAllocationQuery.AllocationRelieve(searchConditions, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = message;
                return RedirectToAction("UpdateSuc");
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;
                // 検索
                return Search(searchConditions);
            }
        }
        #endregion
    }
}