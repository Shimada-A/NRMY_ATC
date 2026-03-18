namespace Wms.Areas.Ship.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.BtoBReference;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.BtoBReference;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class BtoBReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-ARR_BtoBReference01.SearchConditions";
        private const string COOKIE_SEARCHCONDITIONS02 = "W-ARR_BtoBReference02.SearchConditions";

        private BtoBReferenceQuery _BtoBReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBReferenceController"/> class.
        /// </summary>
        public BtoBReferenceController()
        {
            this._BtoBReferenceQuery = new BtoBReferenceQuery();
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
        public ActionResult Search(BtoBReference01SearchConditions SearchConditions)
        {
            BtoBReference01SearchConditions condition = SearchConditions;
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
        public ActionResult AllSelectedSearch(BtoBReference01SearchConditions searchConditions)
        {
            // 全選択
            _BtoBReferenceQuery.ShpBtoBReferenceAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new BtoBReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new BtoBReference01Result()
                {
                    BtoBReference01s = _BtoBReferenceQuery.BtoBReference01GetData(searchConditions)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.BtoBReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.BtoBReference01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _BtoBReferenceQuery.GetSelectListDivisions();
            ViewBag.TransporterList = _BtoBReferenceQuery.GetSelectListTransporters();
            ViewBag.ItemList = _BtoBReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _BtoBReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBReference/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(BtoBReference01SearchConditions searchConditions)
        {
            // 全解除
            _BtoBReferenceQuery.ShpBtoBReferenceAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new BtoBReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new BtoBReference01Result()
                {
                    BtoBReference01s = _BtoBReferenceQuery.BtoBReference01GetData(searchConditions)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.BtoBReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.BtoBReference01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _BtoBReferenceQuery.GetSelectListDivisions();
            ViewBag.TransporterList = _BtoBReferenceQuery.GetSelectListTransporters();
            ViewBag.ItemList = _BtoBReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _BtoBReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBReference/Index.cshtml", vm);
        }

        #endregion Selected

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private BtoBReference01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new BtoBReference01SearchConditions() : Request.Cookies.Get<BtoBReference01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new BtoBReference01SearchConditions();
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
        private ActionResult GetSearchResultView(BtoBReference01SearchConditions searchConditions, bool indexFlag)
        {
            // BtoB出荷梱包進捗照会（画面選択行更新用）
            if (!indexFlag && searchConditions.SearchType == Common.SearchTypes.SortPage)
            {
                _BtoBReferenceQuery.UpdateShpBtoBReference(searchConditions.BtoBReference01s);
            }
            // 作成処理&検索表示
            var vm = new BtoBReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new BtoBReference01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _BtoBReferenceQuery.InsertShpBtoBReference01(searchConditions) : true) ? new BtoBReference01Result()
                {
                    BtoBReference01s = _BtoBReferenceQuery.BtoBReference01GetData(searchConditions)
                }
                : new BtoBReference01Result())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.BtoBReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.BtoBReference01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _BtoBReferenceQuery.GetSelectListDivisions();
            ViewBag.TransporterList = _BtoBReferenceQuery.GetSelectListTransporters();
            ViewBag.ItemList = _BtoBReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _BtoBReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBReference/Index.cshtml", vm);

            // return this.View("Index", vm);
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
        public ActionResult ShipConfirm(BtoBReference01SearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _BtoBReferenceQuery.UpdateShpBtoBReference(searchConditions.BtoBReference01s);
            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            var batchNo = string.Empty;
            _BtoBReferenceQuery.ShipConfirm(searchConditions.Seq, searchConditions.CenterId, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = string.Format(BtoBReferenceResource.SUC_UPDATE);

                // 検索表示
                var vm = new BtoBReference01ViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new BtoBReference01Result()
                };
                vm.SearchConditions.Seq = searchConditions.Seq;
                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.DivisionList = _BtoBReferenceQuery.GetSelectListDivisions();
                ViewBag.TransporterList = _BtoBReferenceQuery.GetSelectListTransporters();
                ViewBag.ItemList = _BtoBReferenceQuery.GetSelectListItems();
                ViewBag.Category1List = _BtoBReferenceQuery.GetSelectListCategorys1();
                ViewBag.Category2List = _BtoBReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
                ViewBag.Category3List = _BtoBReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
                ViewBag.Category4List = _BtoBReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
                return this.View("~/Areas/Ship/Views/BtoBReference/Index.cshtml", vm);
            }
            else
            {
                TempData[AppConst.ERROR] = message;
                // 検索
                searchConditions.PageSize = this.GetCurrentPageSize();
                var vm = new BtoBReference01ViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new BtoBReference01Result()
                    {
                        BtoBReference01s = _BtoBReferenceQuery.BtoBReference01GetData(searchConditions)
                    }
                };

                var ProcNumLimit = this.GetCurrentProcNumLimit();
                if (ProcNumLimit != 0 && vm != null)
                {
                    if (vm.Results.BtoBReference01s.TotalItemCount > ProcNumLimit)
                    {
                        TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                        vm.Results.BtoBReference01s = null;
                    }
                }

                vm.SearchConditions.Seq = searchConditions.Seq;
                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.DivisionList = _BtoBReferenceQuery.GetSelectListDivisions();
                ViewBag.TransporterList = _BtoBReferenceQuery.GetSelectListTransporters();
                ViewBag.ItemList = _BtoBReferenceQuery.GetSelectListItems();
                ViewBag.Category1List = _BtoBReferenceQuery.GetSelectListCategorys1();
                ViewBag.Category2List = _BtoBReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
                ViewBag.Category3List = _BtoBReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
                ViewBag.Category4List = _BtoBReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
                return this.View("~/Areas/Ship/Views/BtoBReference/Index.cshtml", vm);
            }
        }
        #endregion

        #region 梱包明細情報
        /// <summary>
        /// 梱包明細情報
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Detail(BtoBReference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // BtoB出荷梱包進捗照会から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            var vm = new BtoBReference02ViewModel();
            vm.SearchConditions = new BtoBReference02SearchConditions()
            {
                CenterId = searchConditions.CenterId,
                Seq = searchConditions.Seq,
                LineNo = searchConditions.LineNo
            };
            // B
            if (string.IsNullOrWhiteSpace(searchConditions.ShipBoxStatus) && searchConditions.ShipBoxStatusOld)
            {
                vm.SearchConditions.Parten = "B";
            }
            // C
            else if (searchConditions.ShipBoxStatus == "5" && !searchConditions.ShipBoxStatusOld)
            {
                vm.SearchConditions.Parten = "C";
            }
            // A
            else
            {
                vm.SearchConditions.Parten = "A";
            }
            // 画面選択行更新用
            _BtoBReferenceQuery.UpdateShpBtoBReference(searchConditions.BtoBReference01s);

            vm.Results = new BtoBReference02Result()
            {
                BtoBReference02s = _BtoBReferenceQuery.GetDetailData(vm.SearchConditions)
            };
            vm.SearchConditions.ItemSkuSum = vm.Results.BtoBReference02s.Select(x => x.ItemSkuId).Distinct().Count();
            vm.SearchConditions.ResultQtySum = vm.Results.BtoBReference02s.Select(x => x.ResultQty).Sum();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            return this.View("~/Areas/Ship/Views/BtoBReference/Detail.cshtml", vm);
        }

        /// <summary>
        /// 梱包明細情報
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailSearch(BtoBReference02SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            var vm = new BtoBReference02ViewModel();
            vm.SearchConditions = searchConditions;

            vm.Results = new BtoBReference02Result()
            {
                BtoBReference02s = _BtoBReferenceQuery.GetDetailData(vm.SearchConditions)
            };
            vm.SearchConditions.ItemSkuSum = vm.Results.BtoBReference02s.Select(x => x.ItemSkuId).Distinct().Count();
            vm.SearchConditions.ResultQtySum = vm.Results.BtoBReference02s.Select(x => x.ResultQty).Sum();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            return this.View("~/Areas/Ship/Views/BtoBReference/Detail.cshtml", vm);
        }
        #endregion

        #region ロード処理

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Download(BtoBReference01SearchConditions SearchConditions)
        {
            try
            {
                Reports.Export.BtoBReferenceReport report = new Reports.Export.BtoBReferenceReport(ReportTypes.Excel, SearchConditions);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
            catch (System.Exception ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "Download");
                throw;
            }

        }
        /// <summary>
        /// 梱包明細(JAN入り)ダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintJan(BtoBReference02SearchConditions searchConditions)
        {
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            Reports.Export.BtoBReferenceReportJan report = new Reports.Export.BtoBReferenceReportJan(ReportTypes.Csv, searchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = "BtoBReference.wfr";

            return WfrPrint(styleName, report.DownloadFileName); ;
        }

        /// <summary>
        /// 出荷梱包進捗照会 ダウンロード（ケース単位）
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadCase(BtoBReference01SearchConditions SearchConditions)
        {
            try
            {
                Reports.Export.BtoBReferenceCaseReport report = new Reports.Export.BtoBReferenceCaseReport(ReportTypes.Excel, SearchConditions);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
            catch (System.Exception ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "DownloadCase");
                throw;
            }

        }
        #endregion
    }
}