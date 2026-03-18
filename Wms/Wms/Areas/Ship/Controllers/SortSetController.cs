namespace Wms.Areas.Ship.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.SortSet;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.SortSet;
    using Wms.Controllers;
    using Wms.Resources;

    public class SortSetController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_SortSet.SearchConditions";

        private SortSetQuery _SortSetQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipFrontageController"/> class.
        /// </summary>
        public SortSetController()
        {
            this._SortSetQuery = new SortSetQuery();
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
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(SortSetSearchConditions SearchConditions)
        {
            SortSetSearchConditions condition;

            // When page is clicked, page > 1
            if (SearchConditions.Page >= 1)
            {
                condition = this.GetPreviousSearchInfo(false);
                condition.Page = SearchConditions.Page;
            }
            else
            {
                condition = SearchConditions;
                condition.PageSize = this.GetCurrentPageSize();
                condition.Page = 1;
            }

            return this.GetSearchResultView(condition, false);
        }

        /// <summary>
        /// ページ遷移、ソート変更処理
        /// </summary>
        /// <param name="SortSets"></param>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        public ActionResult UpdateList(IList<SortSetResultRow> SortSets, SortSetSearchConditions SearchConditions)
        {
            SortSetSearchConditions condition;
            if (SearchConditions.Page >= 1)
            {
                condition = this.GetPreviousSearchInfo(false);
                condition.SortKey = SearchConditions.SortKey;
                condition.Sort = SearchConditions.Sort;
                condition.Page = SearchConditions.Page;
            }
            else
            {
                condition = SearchConditions;
                condition.PageSize = this.GetCurrentPageSize();
                condition.Page = 1;
            }

            //入力仕分コード保持(ページ遷移)
            _SortSetQuery.UpdateWorkData(SortSets);

            // Save search info
            var vm = new SortSetViewModel
            {
                SearchConditions = condition,
                Results = new SortSetResult()
                {
                    SortSets = _SortSetQuery.GetData(condition)
                },
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.SortSets.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.SortSets = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Ship/Views/SortSet/Index.cshtml", vm);

        }

        #endregion Search

        #region 更新処理

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="searchConditions">searchConditions</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SortSet(IList<SortSetResultRow> SortSets)
        {
            ModelState.Clear();
            var searchInfo = this.GetPreviousSearchInfo(false);
            searchInfo.PageSize = this.GetCurrentPageSize();

            //入力仕分コード保持(ページ遷移)
            _SortSetQuery.UpdateWorkData(SortSets);

            // 実績更新
            var message = string.Empty;
            var seq = SortSets[0].Seq;
            var centerId = SortSets[0].CenterId;
            ProcedureStatus status = ProcedureStatus.Error;
            _SortSetQuery.SortSet(seq, centerId, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = SortSetResource.SUC_UPDATE;
                // 検索部を表示
                return this.GetSearchResultView(searchInfo, true);
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;
                // 検索
                var vm = new SortSetViewModel
                {
                    SearchConditions = searchInfo,
                    Results = new SortSetResult()
                    {
                        SortSets = new StaticPagedList<SortSetResultRow>(SortSets, searchInfo.Page, searchInfo.PageSize, searchInfo.TotalCount)
                    },
                };
                // Return index view
                return this.View("~/Areas/Ship/Views/SortSet/Index.cshtml", vm);
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private SortSetSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new SortSetSearchConditions() : Request.Cookies.Get<SortSetSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new SortSetSearchConditions();
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
        private ActionResult GetSearchResultView(SortSetSearchConditions condition, bool indexFlag)
        {
            // Save search info
            var vm = new SortSetViewModel
            {
                SearchConditions = condition,
                Results = indexFlag ? new SortSetResult() : ((condition.SearchType == Common.SearchTypes.Search ? _SortSetQuery.InsertWorkData(condition) : true) ? new SortSetResult()
                {
                    SortSets = _SortSetQuery.GetData(condition)
                }
                : new SortSetResult())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.SortSets.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.SortSets = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Ship/Views/SortSet/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion Private

        #region GetData

        /// <summary>
        /// 郵便番号変更
        /// </summary>
        public JsonResult GetSortingCd(string zip,string prefName,string cityName, string address1, string centerId, string transporterId)
        {
            var cd = _SortSetQuery.GetSortingCd(zip, prefName, cityName, address1, centerId, transporterId);

            return this.Json(
                new
                {
                    sortingCd = cd.TrimEnd()
                });
        }
        #endregion GetData
    }
}