namespace Wms.Areas.Others.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Others.Query.Inspection;
    using Wms.Areas.Others.ViewModels.Inspection;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;


    public class InspectionController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-OTH_Inspection.SearchConditions";

        private InspectionQuery _InspectionQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="InspectionController"/> class.
        /// </summary>
        public InspectionController()
        {
            this._InspectionQuery = new InspectionQuery();
        }

        #endregion Constants

        #region Search

        // GET: Others/Inspection
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);

        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(InspectionSearchConditions SearchConditions)
        {

            //if (SearchConditions.SearchType == Common.SearchTypes.Search && new BaseQuery().IsAllocProcessing() == 1)
            //{
            //    TempData[AppConst.ERROR] = MessagesResource.IS_ALLOC_PROCESSING;
            //    return this.GetSearchResultView(SearchConditions, true);
            //}
            //else if (SearchConditions.SearchType == Common.SearchTypes.Search && new BaseQuery().IsDailyProcessing() == 1)
            //{
            //    TempData[AppConst.ERROR] = MessagesResource.IS_DAILY_PROCESSING;
            //    return this.GetSearchResultView(SearchConditions, true);
            //}
            //else
            //{
                InspectionSearchConditions condition = SearchConditions;
                condition.PageSize = this.GetCurrentPageSize();
                return this.GetSearchResultView(condition, false);
            //}
        }




        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private InspectionSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new InspectionSearchConditions() : Request.Cookies.Get<InspectionSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new InspectionSearchConditions();
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
        private ActionResult GetSearchResultView(InspectionSearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new InspectionViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new InspectionResult() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _InspectionQuery.InsertInspection(searchConditions) : true) ? new InspectionResult()
                {
                    Inspections = _InspectionQuery.InspectionGetData(searchConditions)
                }
                : new InspectionResult())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.Inspections.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.Inspections = null;
                }
            }

            return View("~/Areas/Others/Views/Inspection/Index.cshtml",vm);

        }

        #endregion
    }
}