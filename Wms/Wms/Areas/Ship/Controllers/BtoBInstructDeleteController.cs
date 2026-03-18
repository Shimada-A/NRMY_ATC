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
    using Wms.Areas.Ship.Query.BtoBInstructDelete;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.BtoBInstructDelete;
    using Wms.Controllers;
    using Wms.Hubs;
    using Wms.Models;
    using Wms.Resources;

    public class BtoBInstructDeleteController : BaseAsyncController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_BtoBInstructDelete01.SearchConditions";

        private BtoBInstructDeleteQuery _BtoBInstructDeleteQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBInstructDeleteController"/> class.
        /// </summary>
        public BtoBInstructDeleteController()
        {
            this._BtoBInstructDeleteQuery = new BtoBInstructDeleteQuery();
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
            searchInfo.SelectedCnt = 0;
            return this.GetSearchResultView(searchInfo, true);
        }

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
        public ActionResult Search(BtoBInstructDeleteSearchConditions SearchConditions)
        {
            BtoBInstructDeleteSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
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
        public ActionResult Detail(BtoBInstructDeleteSearchConditions searchConditions)
        {
            // 画面選択行更新
            _BtoBInstructDeleteQuery.UpdateShpBtoBInstructDelete(searchConditions.BtoBInstructDeletes);
            searchConditions.PageSize = this.GetCurrentPageSize();

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);


            // 検索表示
            var detalSearchConditions = new BtoBInsDelDetailSearchConditions();
            detalSearchConditions.CenterId = searchConditions.CenterId;
            detalSearchConditions.ShipInstructId = searchConditions.TargetShipInstructId;
            
            var vm = _BtoBInstructDeleteQuery.GetShpBtoBInstructDeleteDeatil(detalSearchConditions);

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructDelete/Detail.cshtml", vm);
        }

        [HttpPost]
        public ActionResult DetailSearch(BtoBInsDelDetailSearchConditions DetailSearchConditions)
        {
            //this.ModelState.Clear();
            var vm = new BtoBInsDelDetailViewModel();
            vm.DetailSearchConditions = DetailSearchConditions;

            vm = _BtoBInstructDeleteQuery.GetShpBtoBInstructDeleteDeatil(vm.DetailSearchConditions);
            return this.View("~/Areas/Ship/Views/BtoBInstructDelete/Detail.cshtml", vm);
        }
        #endregion

        #region Selected
        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(BtoBInstructDeleteSearchConditions searchConditions)
        {
            // 全選択
            _BtoBInstructDeleteQuery.ShpBtoBInstructDeleteAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new BtoBInstructDeleteViewModel
            {
                SearchConditions = searchConditions,
                Results = new BtoBInstructDeleteResult()
                {
                    BtoBInstructDeletes = _BtoBInstructDeleteQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _BtoBInstructDeleteQuery.GetSelectListTransporters();
            ViewBag.DivisionList = _BtoBInstructDeleteQuery.GetSelectListDivisions();
            ViewBag.Category1List = _BtoBInstructDeleteQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBInstructDeleteQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBInstructDeleteQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBInstructDeleteQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _BtoBInstructDeleteQuery.GetSelectListItems();

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructDelete/Index.cshtml", vm);
        }
        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(BtoBInstructDeleteSearchConditions searchConditions)
        {
            // 全解除
            _BtoBInstructDeleteQuery.ShpBtoBInstructDeleteAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new BtoBInstructDeleteViewModel
            {
                SearchConditions = searchConditions,
                Results = new BtoBInstructDeleteResult()
                {
                    BtoBInstructDeletes = _BtoBInstructDeleteQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _BtoBInstructDeleteQuery.GetSelectListTransporters();
            ViewBag.DivisionList = _BtoBInstructDeleteQuery.GetSelectListDivisions();
            ViewBag.Category1List = _BtoBInstructDeleteQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBInstructDeleteQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBInstructDeleteQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBInstructDeleteQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _BtoBInstructDeleteQuery.GetSelectListItems();
            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructDelete/Index.cshtml", vm);
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
        public ActionResult UpdateSearch(BtoBInstructDeleteSearchConditions searchConditions)
        {
            ModelState.Clear();
            // 画面選択行更新用
            _BtoBInstructDeleteQuery.UpdateShpBtoBInstructDelete(searchConditions.BtoBInstructDeletes);
            searchConditions.PageSize = this.GetCurrentPageSize();
            searchConditions.Page = 1;

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _BtoBInstructDeleteQuery.BtoBInstructDelete(searchConditions, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = BtoBInstructDeleteResource.SUC_UPDATE;
                // 検索部を表示
                return RedirectToAction("UpdateSuc");
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;
                // 検索
                var vm = new BtoBInstructDeleteViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new BtoBInstructDeleteResult()
                    {
                        BtoBInstructDeletes = _BtoBInstructDeleteQuery.GetData(searchConditions)
                    },
                };
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.TransporterList = _BtoBInstructDeleteQuery.GetSelectListTransporters();
                ViewBag.DivisionList = _BtoBInstructDeleteQuery.GetSelectListDivisions();
                ViewBag.Category1List = _BtoBInstructDeleteQuery.GetSelectListCategorys1();
                ViewBag.Category2List = _BtoBInstructDeleteQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
                ViewBag.Category3List = _BtoBInstructDeleteQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
                ViewBag.Category4List = _BtoBInstructDeleteQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
                ViewBag.ItemList = _BtoBInstructDeleteQuery.GetSelectListItems();
                // Return index view
                return this.View("~/Areas/Ship/Views/BtoBInstructDelete/Index.cshtml", vm);
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private BtoBInstructDeleteSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new BtoBInstructDeleteSearchConditions() : Request.Cookies.Get<BtoBInstructDeleteSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new BtoBInstructDeleteSearchConditions();
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
        private ActionResult GetSearchResultView(BtoBInstructDeleteSearchConditions searchConditions, bool indexFlag)
        {
            // 画面選択行更新用
            if (!indexFlag && searchConditions.SearchType == ViewModels.BtoBInstructDelete.SearchTypes.SortPage)
            {
                _BtoBInstructDeleteQuery.UpdateShpBtoBInstructDelete(searchConditions.BtoBInstructDeletes);
            }

            // 作成処理&検索表示
            var vm = new BtoBInstructDeleteViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new BtoBInstructDeleteResult() : ((searchConditions.SearchType == ViewModels.BtoBInstructDelete.SearchTypes.Search ? _BtoBInstructDeleteQuery.InsertShpBtoBInstructDelete(searchConditions) : true) ? new BtoBInstructDeleteResult()
                {
                    BtoBInstructDeletes = _BtoBInstructDeleteQuery.GetData(searchConditions)
                }
                : new BtoBInstructDeleteResult()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null && !indexFlag)
            {
                if (vm.Results.BtoBInstructDeletes.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.BtoBInstructDeletes = null;
                }
            }

            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _BtoBInstructDeleteQuery.GetSelectListTransporters();
            ViewBag.DivisionList = _BtoBInstructDeleteQuery.GetSelectListDivisions();
            ViewBag.Category1List = _BtoBInstructDeleteQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BtoBInstructDeleteQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BtoBInstructDeleteQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BtoBInstructDeleteQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _BtoBInstructDeleteQuery.GetSelectListItems();

            // Return index view
            return this.View("~/Areas/Ship/Views/BtoBInstructDelete/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion Private
    }
}