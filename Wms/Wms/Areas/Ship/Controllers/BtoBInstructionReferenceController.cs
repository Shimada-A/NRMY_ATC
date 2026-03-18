namespace Wms.Areas.Ship.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.BtoBInstructionReference;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.BtoBInstructionReference;
    using Wms.Areas.Ship.ViewModels.TransferReference;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class BtoBInstructionReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-STK_BtoBInstructionReference.SearchConditions";

        private BtoBInstructionReferenceQuery _BtoBInstructionReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBInstructionReferenceController"/> class.
        /// </summary>
        public BtoBInstructionReferenceController()
        {
            this._BtoBInstructionReferenceQuery = new BtoBInstructionReferenceQuery();
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
            var searchConditions = this.GetPreviousSearchInfo(false);
            // 検索表示
            var vm = new BtoBInstructionReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new BtoBInstructionReference01Result()
                {
                    BtoBInstructionReference01s = _BtoBInstructionReferenceQuery.BtoBInstructionReference01GetData(searchConditions)
                },
                Page = 1
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.BtoBInstructionReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.BtoBInstructionReference01s = null;
                }
            }
            ViewBag.DivisionList = _BtoBInstructionReferenceQuery.GetSelectListDivisions();
            ViewBag.TransporterList = _BtoBInstructionReferenceQuery.GetSelectListTransporters();
            ViewBag.ItemList = _BtoBInstructionReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _BtoBInstructionReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBInstructionReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBInstructionReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBInstructionReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Index.cshtml", vm);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult DetailBackSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            searchInfo.Seq = searchInfo.OldSeq;
            searchInfo.ResultType = ResultTypes.Sku;
            searchInfo.Seq = MvcDbContext.Current.ShpBtoBInstructionReferences.Where(x => x.Seq == searchInfo.Seq && x.ShipperId == Common.Profile.User.ShipperId).FirstOrDefault().SeqPre;

            // 作成処理&検索表示
            var vm = new BtoBInstructionReference01ViewModel
            {
                SearchConditions = searchInfo,
                Results = new BtoBInstructionReference01Result()
                {
                    BtoBInstructionReference01s = _BtoBInstructionReferenceQuery.BtoBInstructionReference01GetData(searchInfo)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.BtoBInstructionReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.BtoBInstructionReference01s = null;
                }
            }

            vm.SearchConditions.Seq = searchInfo.Seq;
            vm.SearchConditions.Page = searchInfo.Page;
            vm.SearchConditions.DetailFlag = "List";
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchInfo);
            ViewBag.DivisionList = _BtoBInstructionReferenceQuery.GetSelectListDivisions();
            ViewBag.TransporterList = _BtoBInstructionReferenceQuery.GetSelectListTransporters();
            ViewBag.ItemList = _BtoBInstructionReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _BtoBInstructionReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBInstructionReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBInstructionReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBInstructionReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Index.cshtml", vm);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(BtoBInstructionReference01SearchConditions SearchConditions)
        {
            BtoBInstructionReference01SearchConditions condition = SearchConditions;

            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        /// <summary>
        /// 検索処理(作業進捗照会画面より遷移)
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BtoBInstructionSearch(TransferReferenceSearchConditions searchConditions)
        {

            BtoBInstructionReference01SearchConditions condition = new BtoBInstructionReference01SearchConditions();
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

        #region Detail

        /// <summary>
        /// Detail
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailSearch(BtoBInstructionReference01SearchConditions searchConditions)
        {
            // BtoB出荷進捗照会明細（画面選択行更新用）
            _BtoBInstructionReferenceQuery.UpdateShpBtoBInstructionReference(searchConditions.BtoBInstructionReference01s);
            searchConditions.PageSize = this.GetCurrentPageSize();
            searchConditions.OldSeq = searchConditions.Seq;

            // 作成処理&検索表示
            var vm = new BtoBInstructionReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = _BtoBInstructionReferenceQuery.InsertSelectedShpBtoBInstructionReference(searchConditions) ? new BtoBInstructionReference01Result()
                {
                    BtoBInstructionReference01s = _BtoBInstructionReferenceQuery.BtoBInstructionReference01GetData(searchConditions)
                }
                : new BtoBInstructionReference01Result(),
                Page = 1
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.BtoBInstructionReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.BtoBInstructionReference01s = null;
                }
            }
            vm.SearchConditions.ShipKind = searchConditions.ShipKind;
            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            vm.SearchConditions.DetailFlag = "Detail";
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _BtoBInstructionReferenceQuery.GetSelectListDivisions();
            ViewBag.TransporterList = _BtoBInstructionReferenceQuery.GetSelectListTransporters();
            ViewBag.ItemList = _BtoBInstructionReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _BtoBInstructionReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBInstructionReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBInstructionReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBInstructionReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Index.cshtml", vm);
        }

        #endregion Detail

        #region Selected

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(BtoBInstructionReference01SearchConditions searchConditions)
        {
            // 全選択
            _BtoBInstructionReferenceQuery.ShpBtoBInstructionReferenceAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new BtoBInstructionReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new BtoBInstructionReference01Result()
                {
                    BtoBInstructionReference01s = _BtoBInstructionReferenceQuery.BtoBInstructionReference01GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _BtoBInstructionReferenceQuery.GetSelectListDivisions();
            ViewBag.TransporterList = _BtoBInstructionReferenceQuery.GetSelectListTransporters();
            ViewBag.ItemList = _BtoBInstructionReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _BtoBInstructionReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBInstructionReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBInstructionReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBInstructionReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(BtoBInstructionReference01SearchConditions searchConditions)
        {
            // 全解除
            _BtoBInstructionReferenceQuery.ShpBtoBInstructionReferenceAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new BtoBInstructionReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new BtoBInstructionReference01Result()
                {
                    BtoBInstructionReference01s = _BtoBInstructionReferenceQuery.BtoBInstructionReference01GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _BtoBInstructionReferenceQuery.GetSelectListDivisions();
            ViewBag.TransporterList = _BtoBInstructionReferenceQuery.GetSelectListTransporters();
            ViewBag.ItemList = _BtoBInstructionReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _BtoBInstructionReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBInstructionReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBInstructionReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBInstructionReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Index.cshtml", vm);
        }

        #endregion Selected

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private BtoBInstructionReference01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new BtoBInstructionReference01SearchConditions() : Request.Cookies.Get<BtoBInstructionReference01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new BtoBInstructionReference01SearchConditions();
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
        private ActionResult GetSearchResultView(BtoBInstructionReference01SearchConditions searchConditions, bool indexFlag)
        {
            // BtoB出荷進捗照会明細（画面選択行更新用）
            if (!indexFlag && searchConditions.SearchType == Common.SearchTypes.SortPage)
            {
                _BtoBInstructionReferenceQuery.UpdateShpBtoBInstructionReference(searchConditions.BtoBInstructionReference01s);
            }

            // 作成処理&検索表示
            var vm = new BtoBInstructionReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new BtoBInstructionReference01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _BtoBInstructionReferenceQuery.InsertShpBtoBInstructionReference(searchConditions) : true) ? new BtoBInstructionReference01Result()
                {
                    BtoBInstructionReference01s = _BtoBInstructionReferenceQuery.BtoBInstructionReference01GetData(searchConditions)
                }
                : new BtoBInstructionReference01Result()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.BtoBInstructionReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.BtoBInstructionReference01s = null;
                }
            }

            vm.SearchConditions.ShipKind = searchConditions.ShipKind;
            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            vm.SearchConditions.DetailFlag = searchConditions.ResultType == ResultTypes.Sku ? "List" : searchConditions.DetailFlag;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _BtoBInstructionReferenceQuery.GetSelectListDivisions();
            ViewBag.TransporterList = _BtoBInstructionReferenceQuery.GetSelectListTransporters();
            ViewBag.ItemList = _BtoBInstructionReferenceQuery.GetSelectListItems();
            ViewBag.Category1List = _BtoBInstructionReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBInstructionReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBInstructionReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBInstructionReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Index.cshtml", vm);

            // return this.View("Index", vm);
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
        public ActionResult Download(BtoBInstructionReference01SearchConditions searchConditions)
        {
            Reports.Export.BtoBInstructionReferenceReport report = new Reports.Export.BtoBInstructionReferenceReport(ReportTypes.Excel, searchConditions);
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
        public ActionResult DownloadDetail(BtoBInstructionReference01SearchConditions searchConditions)
        {
            Reports.Export.BtoBInstructionReferenceDetailReport report = new Reports.Export.BtoBInstructionReferenceDetailReport(ReportTypes.Excel, searchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        #endregion ロード処理

        #region 更新処理
        /// <summary>
        /// 出荷キャンセル
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShipCancel(BtoBInstructionReference01SearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _BtoBInstructionReferenceQuery.UpdateShpBtoBInstructionReference(searchConditions.BtoBInstructionReference01s);
            // 実績更新
            var message = string.Empty;
            var batchNo = string.Empty;
            _BtoBInstructionReferenceQuery.ShipCancel(searchConditions.Seq, out message, out batchNo);
            if (string.IsNullOrWhiteSpace(message))
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = string.Format(BtoBInstructionReferenceResource.SUC_UPDATE, batchNo);

                // 検索表示
                var vm = new BtoBInstructionReference01ViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new BtoBInstructionReference01Result()
                };
                vm.SearchConditions.Seq = searchConditions.Seq;
                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.DivisionList = _BtoBInstructionReferenceQuery.GetSelectListDivisions();
                ViewBag.TransporterList = _BtoBInstructionReferenceQuery.GetSelectListTransporters();
                ViewBag.ItemList = _BtoBInstructionReferenceQuery.GetSelectListItems();
                ViewBag.Category1List = _BtoBInstructionReferenceQuery.GetSelectListCategorys1();
                ViewBag.Category2List = _BtoBInstructionReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
                ViewBag.Category3List = _BtoBInstructionReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
                ViewBag.Category4List = _BtoBInstructionReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

                // Return index view
                return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Index.cshtml", vm);
            }
            else
            {
                TempData[AppConst.ERROR] = message;
                // 検索
                searchConditions.PageSize = this.GetCurrentPageSize();
                var vm = new BtoBInstructionReference01ViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new BtoBInstructionReference01Result()
                    {
                        BtoBInstructionReference01s = _BtoBInstructionReferenceQuery.BtoBInstructionReference01GetData(searchConditions)
                    }
                };

                var ProcNumLimit = this.GetCurrentProcNumLimit();
                if (ProcNumLimit != 0 && vm != null)
                {
                    if (vm.Results.BtoBInstructionReference01s.TotalItemCount > ProcNumLimit)
                    {
                        TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                        vm.Results.BtoBInstructionReference01s = null;
                    }
                }

                vm.SearchConditions.Seq = searchConditions.Seq;
                vm.SearchConditions.Page = searchConditions.Page;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.DivisionList = _BtoBInstructionReferenceQuery.GetSelectListDivisions();
                ViewBag.TransporterList = _BtoBInstructionReferenceQuery.GetSelectListTransporters();
                ViewBag.ItemList = _BtoBInstructionReferenceQuery.GetSelectListItems();
                ViewBag.Category1List = _BtoBInstructionReferenceQuery.GetSelectListCategorys1();
                ViewBag.Category2List = _BtoBInstructionReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
                ViewBag.Category3List = _BtoBInstructionReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
                ViewBag.Category4List = _BtoBInstructionReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

                // Return index view
                return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Index.cshtml", vm);
            }
        }

        /// <summary>
        /// 欠品確定
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockOutFix(BtoBInstructionReference02SearchConditions searchConditions)
        {
            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _BtoBInstructionReferenceQuery.StockOutFix(searchConditions, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = BtoBInstructionReferenceResource.SUC_CONFIRM;
                var condition = Request.Cookies.Get<BtoBInstructionReference01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new BtoBInstructionReference01SearchConditions();

                var vm = _BtoBInstructionReferenceQuery.GetPackageData(condition);

                return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Detail.cshtml", vm);
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.Message = message;
                var condition = Request.Cookies.Get<BtoBInstructionReference01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new BtoBInstructionReference01SearchConditions();

                var vm = _BtoBInstructionReferenceQuery.GetPackageData(condition);

                return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Detail.cshtml", vm);
            }
        }
        #endregion

        #region 梱包明細情報
        /// <summary>
        /// 梱包明細情報
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult PackageDetail(BtoBInstructionReference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // BtoB出荷進捗照会明細（画面選択行更新用）
            _BtoBInstructionReferenceQuery.UpdateShpBtoBInstructionReference(searchConditions.BtoBInstructionReference01s);
            // BtoB出荷進捗照会から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            var vm = _BtoBInstructionReferenceQuery.GetPackageData(searchConditions);

            return this.View("~/Areas/Ship/Views/BtoBInstructionReference/Detail.cshtml", vm);
        }
        #endregion

    }
}