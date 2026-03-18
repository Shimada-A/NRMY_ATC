namespace Wms.Areas.Others.Controllers
{
    using System;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Others.Resources;
    using Wms.Controllers;
    using Wms.Resources;
    using Wms.Common;
    using Wms.Areas.Others.Query.WorkReference;
    using Wms.Areas.Others.ViewModels.WorkReference;

    public class WorkReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_OTH_WorkReference.SearchConditions";

        private WorkReferenceQuery _WorkReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="OthersFrontageController"/> class.
        /// </summary>
        public WorkReferenceController()
        {
            this._WorkReferenceQuery = new WorkReferenceQuery();
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
        public ActionResult Search(WorkReferenceSearchConditions SearchConditions)
        {
            WorkReferenceSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private WorkReferenceSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new WorkReferenceSearchConditions() : Request.Cookies.Get<WorkReferenceSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new WorkReferenceSearchConditions();
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
        private ActionResult GetSearchResultView(WorkReferenceSearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new WorkReferenceViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new WorkReferenceResult() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _WorkReferenceQuery.InsertArrWorkReference(searchConditions) : true) ? new WorkReferenceResult()
                {
                    WorkReferences = _WorkReferenceQuery.WorkReferenceGetData(searchConditions)
                }
                : new WorkReferenceResult())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm.Results.WorkReferences != null)
            {
                if (vm.Results.WorkReferences.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.WorkReferences = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.ProcessingTypeList = _WorkReferenceQuery.GetSelectListProcessingType();
            ViewBag.WorkStatusList = _WorkReferenceQuery.GetSelectListWorkStatus();
            ViewBag.CenterList  = _WorkReferenceQuery.GetCenterListItem();
            // Return index view
            return this.View("~/Areas/Others/Views/WorkReference/Index.cshtml", vm);
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
        public ActionResult WorkReferenceDownload()
        {
            WorkReferenceSearchConditions searchCondition = this.GetPreviousSearchInfo(false);
            Reports.Export.WorkReferenceReport report = new Reports.Export.WorkReferenceReport(ReportTypes.Excel, searchCondition);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        #endregion ロード処理

    }
}