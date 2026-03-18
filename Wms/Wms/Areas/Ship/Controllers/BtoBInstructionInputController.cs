namespace Wms.Areas.Ship.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.BtoBInstructionInput;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.BtoBInstructionInput;
    using Wms.Areas.Ship.ViewModels.TransferReference;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class BtoBInstructionInputController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-STK_BtoBInstructionInput.SearchConditions";

        private BtoBInstructionInputQuery _BtoBInstructionInputQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBInstructionInputController"/> class.
        /// </summary>
        public BtoBInstructionInputController()
        {
            this._BtoBInstructionInputQuery = new BtoBInstructionInputQuery();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="SearchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(BtoBInstructionInput01SearchConditions SearchConditions)
        {
            BtoBInstructionInput01SearchConditions condition = SearchConditions;

            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        /// <summary>
        /// 検索処理(SKU一覧/指示明細切り替え)
        /// </summary>
        /// <param name="searchConditions">List Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSearch(TransferReferenceSearchConditions searchConditions)
        {

            BtoBInstructionInput01SearchConditions condition = new BtoBInstructionInput01SearchConditions();
            condition.CenterId = searchConditions.CenterId;
            condition.ShipPlanDateFrom = searchConditions.ShipPlanDate;
            condition.ShipPlanDateTo = searchConditions.ShipPlanDate;
            condition.ShipKind = searchConditions.ShipKind;
            if (searchConditions.ShipKind == Common.ShipKinds.Tc)
            {
                condition.InstructClass = "6";
                condition.ShipAllocStatus = "3";
            }
            else
            {
                condition.BatchNo = searchConditions.BatchNo;
                condition.InstructClass = string.IsNullOrWhiteSpace(searchConditions.BatchNo) ? "6" : string.Empty;
            };

            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }
        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private BtoBInstructionInput01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new BtoBInstructionInput01SearchConditions() : Request.Cookies.Get<BtoBInstructionInput01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new BtoBInstructionInput01SearchConditions();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }
        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="searchConditions">Search Country Information</param>
        /// <param name="indexFlag"></param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(BtoBInstructionInput01SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new BtoBInstructionInput01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new BtoBInstructionInput01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _BtoBInstructionInputQuery.InsertShpBtoBInstructionInput(searchConditions) : true) ? new BtoBInstructionInput01Result()
                {
                    BtoBInstructionInput01s = _BtoBInstructionInputQuery.BtoBInstructionInput01GetData(searchConditions)
                }
                : new BtoBInstructionInput01Result()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.BtoBInstructionInput01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.BtoBInstructionInput01s = null;
                }
            }

            vm.SearchConditions.ShipKind = searchConditions.ShipKind;
            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructionInput/Index.cshtml", vm);
        }

        #endregion Private

        #region ロード処理

        /// <summary>
        /// SKU一覧ダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Download(BtoBInstructionInput01SearchConditions searchConditions)
        {
            Reports.Export.BtoBInstructionInputReport report = new Reports.Export.BtoBInstructionInputReport(ReportTypes.Excel, searchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// 指示明細ウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownloadDetail(BtoBInstructionInput01SearchConditions searchConditions)
        {
            Reports.Export.BtoBInstructionInputDetailReport report = new Reports.Export.BtoBInstructionInputDetailReport(ReportTypes.Excel, searchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        #endregion ロード処理

        #region 更新処理
        /// <summary>
        /// レーン仕分実績チェック
        /// </summary>
        /// <param name="centerId"></param>
        /// <param name="Seq"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InputSortCheck(string centerId, long Seq)
        {
            var status = 0;
            status = _BtoBInstructionInputQuery.LaneResultCheck(Seq);
            return Json(new { status });
        }
        /// <summary>
        /// 出荷実績一括入力
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Input(BtoBInstructionInput01SearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _BtoBInstructionInputQuery.Input(searchConditions, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = BtoBInstructionInputResource.SUC_UPDATE;

                // 検索表示
                var vm = new BtoBInstructionInput01ViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new BtoBInstructionInput01Result()
                };
                vm.SearchConditions.Seq = searchConditions.Seq;
                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                // Return index view
                return this.View("~/Areas/Ship/Views/BtoBInstructionInput/Index.cshtml", vm);
            }
            else
            {
                TempData[AppConst.ERROR] = message;
                // 検索
                searchConditions.PageSize = this.GetCurrentPageSize();
                var vm = new BtoBInstructionInput01ViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new BtoBInstructionInput01Result()
                    {
                        BtoBInstructionInput01s = _BtoBInstructionInputQuery.BtoBInstructionInput01GetData(searchConditions)
                    }
                };

                var ProcNumLimit = this.GetCurrentProcNumLimit();
                if (ProcNumLimit != 0 && vm != null)
                {
                    if (vm.Results.BtoBInstructionInput01s.TotalItemCount > ProcNumLimit)
                    {
                        TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                        vm.Results.BtoBInstructionInput01s = null;
                    }
                }
                vm.SearchConditions.Seq = searchConditions.Seq;
                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                // Return index view
                return this.View("~/Areas/Ship/Views/BtoBInstructionInput/Index.cshtml", vm);
            }
        }
        #endregion 更新処理
    }
}