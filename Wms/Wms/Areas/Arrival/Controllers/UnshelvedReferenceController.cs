using Share.Common;
using Share.Extensions.Classes;
using System.Web.Mvc;
using Wms.Areas.Arrival.Query.UnshelvedReference;
using Wms.Areas.Arrival.ViewModels.UnshelvedReference;
using Wms.Controllers;
using Wms.Resources;

namespace Wms.Areas.Arrival.Controllers
{
    public class UnshelvedReferenceController : BaseController
    {
        #region Constants
        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-ARR_UnshelvedReference.SearchConditions";
        private UnshelvedReferenceQuery _UnshelvedReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnshelvedReferenceController"/> class.
        /// </summary>
        public UnshelvedReferenceController()
        {
            this._UnshelvedReferenceQuery = new UnshelvedReferenceQuery();
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
            return this.GetSearchResultView(searchInfo,true);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(UnshelvedReferenceSearchConditions SearchConditions)
        {
            UnshelvedReferenceSearchConditions condition = null;
            condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }
        #endregion

        #region Private
        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private UnshelvedReferenceSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag? new UnshelvedReferenceSearchConditions(): Request.Cookies.Get<UnshelvedReferenceSearchConditions>(COOKIE_SEARCHCONDITIONS) != null ? Request.Cookies.Get<UnshelvedReferenceSearchConditions>(COOKIE_SEARCHCONDITIONS) : new UnshelvedReferenceSearchConditions();
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
        private ActionResult GetSearchResultView(UnshelvedReferenceSearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new UnshelvedReferenceViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new UnshelvedReferenceResult() : new UnshelvedReferenceResult() {
                    UnshelvedReferences = _UnshelvedReferenceQuery.GetData(searchConditions)
                },
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.UnshelvedReferences.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.UnshelvedReferences = null;
                }
            }

            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _UnshelvedReferenceQuery.GetSelectListDivisions();
            ViewBag.Category1List = _UnshelvedReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _UnshelvedReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _UnshelvedReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _UnshelvedReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.ItemList = _UnshelvedReferenceQuery.GetSelectListItems();

            // Return index view
            return this.View("~/Areas/Arrival/Views/UnshelvedReference/Index.cshtml", vm);
        }

        #endregion

        #region ロード処理

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Download()
        {
            UnshelvedReferenceSearchConditions searchCondition = this.GetPreviousSearchInfo(false);

            if (searchCondition.ResultType == Common.ArrivalTypes.Arrival)
            {
                Reports.Export.UnshelvedReferenceReport report = new Reports.Export.UnshelvedReferenceReport(ReportTypes.Excel, searchCondition);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
            else
            {
                Reports.Export.PackageUnshelvedReferenceReport report = new Reports.Export.PackageUnshelvedReferenceReport(ReportTypes.Excel, searchCondition);
                report.Export();
                return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
        }
        #endregion
    }
}