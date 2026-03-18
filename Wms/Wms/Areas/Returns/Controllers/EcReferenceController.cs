namespace Wms.Areas.Returns.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Returns.Query.EcReference;
    using Wms.Areas.Returns.Resources;
    using Wms.Areas.Returns.ViewModels.EcReference;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;


    public class EcReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_RET_EcReference01.SearchConditions";
        private const string COOKIE_SEARCHCONDITIONS02 = "W_RET_EcReference02.SearchConditions";

        private EcReferenceQuery _EcReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBReferenceController"/> class.
        /// </summary>
        public EcReferenceController()
        {
            this._EcReferenceQuery = new EcReferenceQuery();
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
        public ActionResult Search(EcReference01SearchConditions SearchConditions)
        {
            EcReference01SearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search


        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private EcReference01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new EcReference01SearchConditions() : Request.Cookies.Get<EcReference01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new EcReference01SearchConditions();
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
        private ActionResult GetSearchResultView(EcReference01SearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new EcReference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new EcReference01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _EcReferenceQuery.InsertRetEcReference01(searchConditions) : true) ? new EcReference01Result()
                {
                    EcReference01s = _EcReferenceQuery.EcReference01GetData(searchConditions)
                }
            : new EcReference01Result())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.EcReference01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.EcReference01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _EcReferenceQuery.GetSelectListDivisions();
            ViewBag.Category1List = _EcReferenceQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _EcReferenceQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _EcReferenceQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _EcReferenceQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Returns/Views/EcReference/Index.cshtml", vm);

        }
        #endregion

        #region 返品明細情報

        /// <summary>
        /// 返品明細情報
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Detail(EcReference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            var vm = new EcReference02ViewModel();
            vm.SearchConditions = new EcReference02SearchConditions()
            {
                CenterId = searchConditions.CenterId,
                Seq = searchConditions.Seq,
                LineNo = searchConditions.LineNo
            };

            vm.Results = new EcReference02Result()
            {
                EcReference02s = _EcReferenceQuery.EcReference02GetData(vm.SearchConditions)
            };
            return this.View("~/Areas/Returns/Views/EcReference/Detail.cshtml", vm);
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
        public ActionResult Download(EcReference01SearchConditions SearchConditions)
        {
            Reports.Export.EcReferenceReport report = new Reports.Export.EcReferenceReport(ReportTypes.Excel, SearchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }
        #endregion
    }

}
