namespace Wms.Areas.Stock.Controllers
{
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Stock.Query.LocMove;
    using Wms.Areas.Stock.ViewModels.LocMove;
    using Wms.Common;
    using Wms.Controllers;
    using Wms.Resources;

    public class LocMoveController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-STK_LocMove.SearchConditions";

        private LocMoveQuery _LocMoveQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocMoveController"/> class.
        /// </summary>
        public LocMoveController()
        {
            this._LocMoveQuery = new LocMoveQuery();
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
        public ActionResult Search(LocMoveSearchConditions SearchConditions)
        {
            LocMoveSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private LocMoveSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new LocMoveSearchConditions() : Request.Cookies.Get<LocMoveSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new LocMoveSearchConditions();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(LocMoveSearchConditions searchConditions)
        {
            // 全選択
            _LocMoveQuery.StkStockAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new LocMoveViewModel
            {
                SearchConditions = searchConditions,
                Results = new LocMoveResult()
                {
                    LocMoves = _LocMoveQuery.LocMoveGetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.LocationClassList = _LocMoveQuery.GetSelectListLocationClasses();
            ViewBag.GradeList = _LocMoveQuery.GetSelectListGrades();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Stock/Views/LocMove/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(LocMoveSearchConditions searchConditions)
        {
            // 全解除
            _LocMoveQuery.StkStockAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new LocMoveViewModel
            {
                SearchConditions = searchConditions,
                Results = new LocMoveResult()
                {
                    LocMoves = _LocMoveQuery.LocMoveGetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.LocationClassList = _LocMoveQuery.GetSelectListLocationClasses();
            ViewBag.GradeList = _LocMoveQuery.GetSelectListGrades();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            // Return index view
            return this.View("~/Areas/Stock/Views/LocMove/Index.cshtml", vm);
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(LocMoveSearchConditions searchConditions, bool indexFlag)
        {
            // 画面選択行更新用
            if (!indexFlag && searchConditions.SearchType == SearchTypes.SortPage)
            {
                _LocMoveQuery.UpdateStkLocMove(searchConditions.LocMoves);
            }

            // 作成処理&検索表示
            var vm = new LocMoveViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new LocMoveResult() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _LocMoveQuery.InsertStkLocMove(searchConditions) : true) ? new LocMoveResult()
                {
                    LocMoves = _LocMoveQuery.LocMoveGetData(searchConditions)
                }
                : new LocMoveResult()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.LocMoves.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.LocMoves = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.LocationClassList = _LocMoveQuery.GetSelectListLocationClasses();
            ViewBag.GradeList = _LocMoveQuery.GetSelectListGrades();

            // Return index view
            return this.View("~/Areas/Stock/Views/LocMove/Index.cshtml", vm);
        }

        #endregion Private

        #region 更新

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSearch(LocMoveSearchConditions searchConditions)
        {
            searchConditions.PageSize = this.GetCurrentPageSize();

            // ワーク02更新
            if (_LocMoveQuery.UpdateStkLocMove(searchConditions.LocMoves))
            {
                // 実績更新
                var message = string.Empty;
                ProcedureStatus status = ProcedureStatus.Success;
                _LocMoveQuery.UpdateLocMove(searchConditions, out status, out message);

                if (status == ProcedureStatus.Success)
                {
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
                    return this.Redirect("Index");
                }
                else
                {
                    TempData[AppConst.ERROR] = message;
                    searchConditions.SearchType = SearchTypes.SortPage;
                    return this.GetSearchResultView(searchConditions, false);
                }
            }
            else
            {
                TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                searchConditions.SearchType = SearchTypes.SortPage;
                return this.GetSearchResultView(searchConditions, false);
            }
        }

        #endregion 更新
    }
}