namespace Wms.Areas.Ship.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.EcConfirmProgressQuery;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.EcConfirmProgress;
    using Wms.Areas.Ship.ViewModels.TransferReference;
    using Wms.Areas.Ships.Reports.Export;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class EcConfirmProgressController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-SHP_EcConfirmProgress01.SearchConditions";

        private EcConfirmProgressQuery _EcConfirmProgressQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="EcConfirmProgressController"/> class.
        /// </summary>
        public EcConfirmProgressController()
        {
            this._EcConfirmProgressQuery = new EcConfirmProgressQuery();
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
        /// <param name="SearchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(EcConfirmProgress01SearchConditions SearchConditions)
        {
            EcConfirmProgress01SearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EcConfirmProgressSearch(TransferReferenceSearchConditions searchConditions)
        {
            EcConfirmProgress01SearchConditions condition = new EcConfirmProgress01SearchConditions()
            {
                CenterId = searchConditions.CenterId,
                BatchNo = searchConditions.BatchNo,
                ShipPlanDateFrom = searchConditions.ShipPlanDate,
                ShipPlanDateTo = searchConditions.ShipPlanDate,
                Move = 1
            };
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Selected

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(EcConfirmProgress01SearchConditions searchConditions)
        {
            // 全選択
            _EcConfirmProgressQuery.ShpEcConfirmProgressAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new EcConfirmProgress01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new EcConfirmProgress01Result()
                {
                    EcConfirmProgress01s = _EcConfirmProgressQuery.EcConfirmProgress01GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _EcConfirmProgressQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/EcConfirmProgress/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(EcConfirmProgress01SearchConditions searchConditions)
        {
            // 全解除
            _EcConfirmProgressQuery.ShpEcConfirmProgressAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new EcConfirmProgress01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new EcConfirmProgress01Result()
                {
                    EcConfirmProgress01s = _EcConfirmProgressQuery.EcConfirmProgress01GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _EcConfirmProgressQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/EcConfirmProgress/Index.cshtml", vm);
        }

        #endregion Selected

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private EcConfirmProgress01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new EcConfirmProgress01SearchConditions() : Request.Cookies.Get<EcConfirmProgress01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new EcConfirmProgress01SearchConditions();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="searchConditions">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(EcConfirmProgress01SearchConditions searchConditions, bool indexFlag)
        {
            // 画面選択行更新用
            if (!indexFlag && searchConditions.SearchType == Common.SearchTypes.SortPage)
            {
                _EcConfirmProgressQuery.UpdateShpEcConfirmProgress(searchConditions.EcConfirmProgress01s);
            }

            // 作成処理&検索表示
            var vm = new EcConfirmProgress01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new EcConfirmProgress01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _EcConfirmProgressQuery.InsertShpEcConfirmProgress01(searchConditions) : true) ? new EcConfirmProgress01Result()
                {
                    EcConfirmProgress01s = _EcConfirmProgressQuery.EcConfirmProgress01GetData(searchConditions)
                }
                : new EcConfirmProgress01Result()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.EcConfirmProgress01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.EcConfirmProgress01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _EcConfirmProgressQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/EcConfirmProgress/Index.cshtml", vm);

        }

        #endregion Private

        #region 更新処理
        /// <summary>
        /// 出荷確定
        /// </summary>
        /// <param name="searchConditions">Country Information</param>
        /// <returns>Update</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShipConfirm(EcConfirmProgress01SearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _EcConfirmProgressQuery.UpdateShpEcConfirmProgress(searchConditions.EcConfirmProgress01s);
            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _EcConfirmProgressQuery.ShipConfirm(searchConditions.Seq, searchConditions.CenterId, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = string.Format(EcConfirmProgressResource.SUC_UPDATE);

                // 検索表示
                var vm = new EcConfirmProgress01ViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new EcConfirmProgress01Result()
                };
                vm.SearchConditions.Seq = searchConditions.Seq;
                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.TransporterList = _EcConfirmProgressQuery.GetSelectListTransporters();
                return this.View("~/Areas/Ship/Views/EcConfirmProgress/Index.cshtml", vm);
            }
            else
            {
                TempData[AppConst.ERROR] = message;
                // 検索
                searchConditions.PageSize = this.GetCurrentPageSize();
                var vm = new EcConfirmProgress01ViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new EcConfirmProgress01Result()
                    {
                        EcConfirmProgress01s = _EcConfirmProgressQuery.EcConfirmProgress01GetData(searchConditions)
                    }
                };

                var ProcNumLimit = this.GetCurrentProcNumLimit();
                if (ProcNumLimit != 0 && vm != null)
                {
                    if (vm.Results.EcConfirmProgress01s.TotalItemCount > ProcNumLimit)
                    {
                        TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                        vm.Results.EcConfirmProgress01s = null;
                    }
                }

                vm.SearchConditions.Seq = searchConditions.Seq;
                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.TransporterList = _EcConfirmProgressQuery.GetSelectListTransporters();
                return this.View("~/Areas/Ship/Views/EcConfirmProgress/Index.cshtml", vm);
            }
        }

        #endregion

        #region 梱包明細情報
        /// <summary>
        /// 梱包明細情報
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Detail(EcConfirmProgress01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // BtoB出荷梱包進捗照会から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            var vm = new EcConfirmProgress02ViewModel();
            vm.SearchConditions = new EcConfirmProgress02SearchConditions()
            {
                CenterId = searchConditions.CenterId,
                Seq = searchConditions.Seq,
                LineNo = searchConditions.LineNo
            };
            // B 過去含むもしくは日次済指定の場合
            if (searchConditions.ShipStatusOld == true || searchConditions.ShipStatus == "6")
            {
                vm.SearchConditions.ShipStatus = "B";
            }
            // A
            else
            {
                vm.SearchConditions.ShipStatus = "A";
            }
            // 画面選択行更新用
            _EcConfirmProgressQuery.UpdateShpEcConfirmProgress(searchConditions.EcConfirmProgress01s);

            vm.DetailResults = new EcConfirmProgress02Result()
            {
                EcConfirmProgress02s = _EcConfirmProgressQuery.GetDetailData(vm.SearchConditions)
            };
            vm.SearchConditions.ItemSkuSum = vm.DetailResults.EcConfirmProgress02s.Select(x => x.ItemSkuId).Distinct().Count();
            vm.SearchConditions.BoxNoSum = vm.DetailResults.EcConfirmProgress02s.Where(x => !string.IsNullOrWhiteSpace(x.BoxNo)).Select(x => x.BoxNo).Distinct().Count();
            vm.SearchConditions.ResultQtySum = vm.DetailResults.EcConfirmProgress02s.Select(x => x.ResultQty).Sum();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            return this.View("~/Areas/Ship/Views/EcConfirmProgress/Detail.cshtml", vm);
        }

        /// <summary>
        /// 梱包明細情報
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailSearch(EcConfirmProgress02SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            var vm = new EcConfirmProgress02ViewModel();
            vm.SearchConditions = searchConditions;

            vm.DetailResults = new EcConfirmProgress02Result()
            {
                EcConfirmProgress02s = _EcConfirmProgressQuery.GetDetailData(vm.SearchConditions)
            };
            vm.SearchConditions.ItemSkuSum = vm.DetailResults.EcConfirmProgress02s.Select(x => x.ItemSkuId).Distinct().Count();
            vm.SearchConditions.BoxNoSum = vm.DetailResults.EcConfirmProgress02s.Where(x => !string.IsNullOrWhiteSpace(x.BoxNo)).Select(x => x.BoxNo).Distinct().Count();
            vm.SearchConditions.ResultQtySum = vm.DetailResults.EcConfirmProgress02s.Select(x => x.ResultQty).Sum();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            return this.View("~/Areas/Ship/Views/EcConfirmProgress/Detail.cshtml", vm);
        }

        #endregion

        #region ロード処理

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Download(EcConfirmProgress01SearchConditions searchConditions)
        {
            Reports.Export.EcConfirmProgressReport report = new Reports.Export.EcConfirmProgressReport(ReportTypes.Excel, searchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// Index明細レポートダウンロード
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownloadDetail(EcConfirmProgress01SearchConditions searchConditions)
        {
            Reports.Export.EcConfirmProgressReportDetail report = new Reports.Export.EcConfirmProgressReportDetail(ReportTypes.Excel, searchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        //印刷
        [HttpPost]
        public ActionResult Print(EcConfirmProgress02SearchConditions searchConditions)
        {
            string styleName;

            styleName = "EcConfirmDetail.sty";

            string controllerName = RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            EcConfirmProgressReportForCsv report = new EcConfirmProgressReportForCsv(searchConditions);
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