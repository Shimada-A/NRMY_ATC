namespace Wms.Areas.Ship.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Threading;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Models;
    using Wms.Areas.Ship.Query.DcAllocation;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.DcAllocation;
    using Wms.Controllers;
    using Wms.Hubs;
    using Wms.Models;
    using Wms.Resources;

    public class DcAllocationController : BaseAsyncController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_DcAllocation01.SearchConditions";

        private DcAllocationQuery _DcAllocationQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="DcAllocationController"/> class.
        /// </summary>
        public DcAllocationController()
        {
            this._DcAllocationQuery = new DcAllocationQuery();
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
            searchInfo.Sort = DcAllocationSearchConditions.AscDescSort.Asc;
            searchInfo.SortKey = DcAllocationSearchConditions.DcAllocationSortKey.ShipPlanDateInstructIdSku;
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult IndexSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            searchInfo.Seq = searchInfo.OldSeq;
            searchInfo.ResultType = ResultTypes.Sku;
            // 作成処理&検索表示
            var vm = new DcAllocationViewModel
            {
                SearchConditions = searchInfo,
                Results = new DcAllocationResult()
                {
                    DcAllocations = _DcAllocationQuery.GetData(searchInfo)
                },

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.DcAllocations.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.DcAllocations = null;
                }
            }

            vm.SearchConditions.Page = searchInfo.Page;
            vm.SearchConditions.DetailFlag = "List";
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchInfo);
            ViewBag.TransporterList = _DcAllocationQuery.GetSelectListTransporters();
            ViewBag.DivisionList = _DcAllocationQuery.GetSelectListDivisions();
            ViewBag.Category1List = _DcAllocationQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _DcAllocationQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _DcAllocationQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _DcAllocationQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _DcAllocationQuery.GetSelectListItems();

            // Return index view
            return this.View("~/Areas/Ship/Views/DcAllocation/Index.cshtml", vm);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(DcAllocationSearchConditions SearchConditions)
        {
            DcAllocationSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            condition.PickKind = condition.InstructKbn == DcAllocationSearchConditions.InstructClass.First ? DcAllocationSearchConditions.PickClass.Total : DcAllocationSearchConditions.PickClass.Store;

            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region PackageStock
        /// <summary>
        /// PackageStock
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailSearch(DcAllocationSearchConditions searchConditions)
        {
            // SKU一覧（画面選択行更新用）
            _DcAllocationQuery.UpdateShpDcAllocation(searchConditions.DcAllocations);
            searchConditions.PageSize = this.GetCurrentPageSize();
            searchConditions.OldSeq = searchConditions.Seq;
            // 作成処理&検索表示
            var vm = new DcAllocationViewModel
            {
                SearchConditions = searchConditions,
                Results = _DcAllocationQuery.InsertShpDcAllocationDeatil(searchConditions) ? new DcAllocationResult()
                {
                    DcAllocations = _DcAllocationQuery.GetData(searchConditions)
                } : new DcAllocationResult(),
                Page = 1
            };
            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            vm.SearchConditions.DetailFlag = "Detail";
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _DcAllocationQuery.GetSelectListTransporters();
            ViewBag.DivisionList = _DcAllocationQuery.GetSelectListDivisions();
            ViewBag.Category1List = _DcAllocationQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _DcAllocationQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _DcAllocationQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _DcAllocationQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _DcAllocationQuery.GetSelectListItems();

            // Return index view
            return this.View("~/Areas/Ship/Views/DcAllocation/Index.cshtml", vm);
        }
        #endregion

        #region Selected
        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(DcAllocationSearchConditions searchConditions)
        {
            // 全選択
            _DcAllocationQuery.ShpDcAllocationAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new DcAllocationViewModel
            {
                SearchConditions = searchConditions,
                Results = new DcAllocationResult()
                {
                    DcAllocations = _DcAllocationQuery.GetData(searchConditions)
                },
            };
            
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _DcAllocationQuery.GetSelectListTransporters();
            ViewBag.DivisionList = _DcAllocationQuery.GetSelectListDivisions();
            ViewBag.Category1List = _DcAllocationQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _DcAllocationQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _DcAllocationQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _DcAllocationQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _DcAllocationQuery.GetSelectListItems();

            // Return index view
            return this.View("~/Areas/Ship/Views/DcAllocation/Index.cshtml", vm);
        }
        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(DcAllocationSearchConditions searchConditions)
        {
            // 全解除
            _DcAllocationQuery.ShpDcAllocationAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new DcAllocationViewModel
            {
                SearchConditions = searchConditions,
                Results = new DcAllocationResult()
                {
                    DcAllocations = _DcAllocationQuery.GetData(searchConditions)
                },
            };
            
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _DcAllocationQuery.GetSelectListTransporters();
            ViewBag.DivisionList = _DcAllocationQuery.GetSelectListDivisions();
            ViewBag.Category1List = _DcAllocationQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _DcAllocationQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _DcAllocationQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _DcAllocationQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _DcAllocationQuery.GetSelectListItems();

            // Return index view
            return this.View("~/Areas/Ship/Views/DcAllocation/Index.cshtml", vm);
        }
        #endregion

        #region 更新処理
        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSearch(DcAllocationSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            var checkMessage = "";

            // 画面選択行更新用
            _DcAllocationQuery.UpdateShpDcAllocation(searchConditions.DcAllocations);

            // 入力値チェック
            if (!_DcAllocationQuery.AllocationInputCheck(searchConditions, out checkMessage))
            {
                //TempData[AppConst.ERROR] = checkMessage;
                ViewBag.Status = -1;
                ViewBag.ErrorMessage = checkMessage;
                searchConditions.SearchType = SearchTypes.SortPage;
                return this.GetSearchResultView(searchConditions, false);
            }

            // 実績更新
            _DcAllocationQuery.AllocationUpdate(searchConditions);

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
                    Thread.Sleep(100);
                    break;
                }

                if (_DcAllocationQuery.GetAllocStatus(wkId).Status == 1)
                {
                    i = 100;
                    searchConditions.IndicateTitle = MessageResource.NORMAL_END;

                }
                else if (_DcAllocationQuery.GetAllocStatus(wkId).Status == 9)
                {
                    i = 100;
                    // 異常の状態を設定する
                    searchConditions.IndicateTitle = MessageResource.ERROR_END;
                    searchConditions.ProcessColor = "red";

                }
                else
                {
                    i = _DcAllocationQuery.GetAllocStatus(wkId).Progress > i ? i + 1 : _DcAllocationQuery.GetAllocStatus(wkId).Progress;
                }
            }

            string message = _DcAllocationQuery.GetAllocStatus(wkId).Message;
            ProcedureStatus status = (ProcedureStatus)_DcAllocationQuery.GetAllocStatus(wkId).Status2;
            if (status == ProcedureStatus.Success || status == ProcedureStatus.NoAllocData)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = message;
                return RedirectToAction("UpdateSuc");
            }
            else if (status == ProcedureStatus.ConfirmLoss)
            {
                ViewBag.Status = status;
                ViewBag.ConfirmMessage = message;
                // 検索
                searchConditions.SearchType = SearchTypes.SortPage;
                return this.GetSearchResultView(searchConditions, false);
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;
                // 検索
                searchConditions.SearchType = SearchTypes.SortPage;
                return this.GetSearchResultView(searchConditions, false);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSearchCompleted()
        {

            DcAllocationSearchConditions searchConditions = (DcAllocationSearchConditions)AsyncManager.Parameters["searchConditions"];
            OutProcedureModel outProcedureModel = (OutProcedureModel)AsyncManager.Parameters["outProcedureModel"];
            searchConditions.IncrementMolecule();
            if (outProcedureModel.status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = outProcedureModel.message;
                return RedirectToAction("UpdateSuc");
            }
            else
            {
                ViewBag.Status = outProcedureModel.status;
                ViewBag.ErrorMessage = outProcedureModel.message;
                // 検索
                return this.GetSearchResultView(searchConditions, false);
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private DcAllocationSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new DcAllocationSearchConditions() : Request.Cookies.Get<DcAllocationSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new DcAllocationSearchConditions();
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
        private ActionResult GetSearchResultView(DcAllocationSearchConditions searchConditions, bool indexFlag)
        {
            // 画面選択行更新用
            if (!indexFlag && searchConditions.SearchType == ViewModels.DcAllocation.SearchTypes.SortPage)
            {
                _DcAllocationQuery.UpdateShpDcAllocation(searchConditions.DcAllocations);
            }

            // 作成処理&検索表示
            var vm = new DcAllocationViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new DcAllocationResult() : ((searchConditions.SearchType == ViewModels.DcAllocation.SearchTypes.Search ? _DcAllocationQuery.InsertShpDcAllocation(searchConditions) : true) ? new DcAllocationResult()
                {
                    DcAllocations = _DcAllocationQuery.GetData(searchConditions)
                }
                : new DcAllocationResult()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null && !indexFlag)
            {
                if (vm.Results.DcAllocations.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.DcAllocations = null;
                }
            }

            vm.SearchConditions.DetailFlag = searchConditions.ResultType == ResultTypes .Sku ? "List" : searchConditions.DetailFlag;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _DcAllocationQuery.GetSelectListTransporters();
            ViewBag.DivisionList = _DcAllocationQuery.GetSelectListDivisions();
            ViewBag.Category1List = _DcAllocationQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _DcAllocationQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _DcAllocationQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _DcAllocationQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _DcAllocationQuery.GetSelectListItems();

            // Return index view
            return this.View("~/Areas/Ship/Views/DcAllocation/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion Private

        #region ロード処理

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ItemDownload(DcAllocationSearchConditions SearchConditions)
        {
            Reports.Export.DcAllocationItemReport report = new Reports.Export.DcAllocationItemReport(ReportTypes.Excel, SearchConditions);
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
        public ActionResult ShipInstructDownload(DcAllocationSearchConditions SearchConditions)
        {
            Reports.Export.DcAllocationShipInstructReport report = new Reports.Export.DcAllocationShipInstructReport(ReportTypes.Excel, SearchConditions);
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
        #endregion
    }
}