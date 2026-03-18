namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.BrandStore;
    using Wms.Controllers;
    using Wms.Extensions.Classes;
    using Wms.Resources;
    using Wms.Areas.Master.Query.BrandStore;

    public class BrandStoreController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASBrandStore01.SearchConditions";
        private BrandStoreQuery _BrandStoreQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandStoreController"/> class.
        /// </summary>
        public BrandStoreController()
        {
            this._BrandStoreQuery = new BrandStoreQuery();
        }

        #endregion

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
        public ActionResult Search(BrandStoreSearchCondition SearchConditions)
        {
            BrandStoreSearchCondition condition;

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
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private BrandStoreSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new BrandStoreSearchCondition() : Request.Cookies.Get<BrandStoreSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new BrandStoreSearchCondition();
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
        private ActionResult GetSearchResultView(BrandStoreSearchCondition condition, bool indexFlag)
        {

            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                BrandStoreResult = indexFlag ? new BrandStoreResult() : new BrandStoreResult()
                {
                    BrandStores = _BrandStoreQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.BrandStoreResult.BrandStores.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.BrandStoreResult.BrandStores = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            return this.View("~/Areas/Master/Views/BrandStore/Index.cshtml", vm);

        }

        #endregion
    }
}