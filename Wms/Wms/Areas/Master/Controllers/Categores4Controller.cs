namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.Categores4;
    using Wms.Controllers;
    using Wms.Extensions.Classes;
    using Wms.Resources;
    using Wms.Areas.Master.Query.Categores4;

    public class Categores4Controller : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASCategores401.SearchConditions";
        private Categores4Query _Categores4Query;

        /// <summary>
        /// Initializes a new instance of the <see cref="Categores4Controller"/> class.
        /// </summary>
        public Categores4Controller()
        {
            this._Categores4Query = new Categores4Query();
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
        public ActionResult Search(Categores4SearchCondition SearchConditions)
        {
            Categores4SearchCondition condition;

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
        private Categores4SearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new Categores4SearchCondition() : Request.Cookies.Get<Categores4SearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new Categores4SearchCondition();
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
        private ActionResult GetSearchResultView(Categores4SearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                Categores4Result = indexFlag ? new Categores4Result() : new Categores4Result()
                {
                    Categores4s = _Categores4Query.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Categores4Result.Categores4s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Categores4Result.Categores4s = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
            ViewBag.Category1List = _Categores4Query.GetSelectListCategorys1();
            ViewBag.Category2List = _Categores4Query.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _Categores4Query.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _Categores4Query.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            return this.View("~/Areas/Master/Views/Categores4/Index.cshtml", vm);
        }

        #endregion


    }
}