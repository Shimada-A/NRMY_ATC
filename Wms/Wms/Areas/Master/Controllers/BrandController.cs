namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.Brand;
    using Wms.Controllers;
    using Wms.Extensions.Classes;
    using Wms.Resources;
    using Wms.Areas.Master.Query.Brand;

    public class BrandController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASBrand01.SearchConditions";
        private BrandQuery _BrandQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandController"/> class.
        /// </summary>
        public BrandController()
        {
            this._BrandQuery = new BrandQuery();
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
        public ActionResult Search(BrandSearchCondition SearchConditions)
        {
            BrandSearchCondition condition;

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

        #region Edit

        /// <summary>
        /// Edit Store Information
        /// </summary>
        /// <param name="countries">List Store</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(IList<SelectBrandViewModel> brands)
        {
            var checkBrand = brands.Where(x => x.IsCheck == true).Single();

            // Get record from DB
            var target = _BrandQuery.GetTargetById(checkBrand.BrandId, Common.Profile.User.ShipperId);

            // 更新対象のデータがマスタに無い場合、エラー
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            return View("~/Areas/Master/Views/Brand/Edit.cshtml", target);
        }

        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private BrandSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new BrandSearchCondition() : Request.Cookies.Get<BrandSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new BrandSearchCondition();
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
        private ActionResult GetSearchResultView(BrandSearchCondition condition, bool indexFlag)
        {

            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                BrandResult = indexFlag ? new BrandResult() : new BrandResult()
                {
                    Brands = _BrandQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.BrandResult.Brands.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.BrandResult.Brands = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            return this.View("~/Areas/Master/Views/Brand/Index.cshtml", vm);

        }

        #endregion
    }
}