namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.EcPrefTransporter;
    using Wms.Controllers;
    using Wms.Resources;

    public class EcPrefTransporterController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASEcPrefTransporter01.SearchConditions";
        private EcPrefTransporter _EcPrefTransporterQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="EcPrefTransporterController"/> class.
        /// </summary>
        public EcPrefTransporterController()
        {
            this._EcPrefTransporterQuery = new EcPrefTransporter();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search 
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// Search
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
        /// <param name="searchCondition">検索条件</param>
        /// <returns>List Record</returns>
        public ActionResult Search(EcPrefTransporterSearchCondition SearchConditions)
        {
            EcPrefTransporterSearchCondition condition;

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

        #endregion Search

        #region Edit

        /// <summary>
        /// 詳細画面
        /// </summary>
        /// <param name="ecPrefTransporters"></param>
        /// <returns></returns>
        public ActionResult Edit(IList<EcPrefTransporterList> ecPrefTransporters)
        {
            var ecPrefTransporter = ecPrefTransporters.Where(x => x.IsCheck == true).Single();

            //Get record from DB
            var target = _EcPrefTransporterQuery.GetTargetById(ecPrefTransporter);

            //更新対象のデータがマスタに無い場合、エラー
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            ViewBag.Transporter = _EcPrefTransporterQuery.GetSelectTransporterListItems();


            return View("~/Areas/Master/Views/EcPrefTransporter/Edit.cshtml", target);
        }

        /// <summary>
        /// 更新処理
        /// </summary
        /// <param name="detail"></param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Detail detail)
        {
            if (ModelState.IsValid)
            {
                if (_EcPrefTransporterQuery.UpdateEcPrefTransporter(detail))
                {
                    TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
                    return RedirectToAction("IndexSearch");
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                    return RedirectToAction("IndexSearch");
                }
            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();

            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }

            return View("~/Areas/Master/Views/EcPrefTransporter/Edit.cshtml", detail);
        }

        #endregion Edit

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <param name="indexFlag"></param>
        /// <returns>SearchCondition</returns>
        private EcPrefTransporterSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new EcPrefTransporterSearchCondition() : Request.Cookies.Get<EcPrefTransporterSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new EcPrefTransporterSearchCondition();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="indexFlag"></param>
        /// <returns></returns>
        private ActionResult GetSearchResultView(EcPrefTransporterSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                EcPrefTransporterResult = indexFlag ? new EcPrefTransporterResult() : new EcPrefTransporterResult()
                {
                    EcPrefTransporters = _EcPrefTransporterQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.EcPrefTransporterResult.EcPrefTransporters.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.EcPrefTransporterResult.EcPrefTransporters = null;
                }
            }

            // 検索用セレクトボックス値を取得
            ViewBag.Center = _EcPrefTransporterQuery.GetSelectCenterListItems();
            ViewBag.Transporter = _EcPrefTransporterQuery.GetSelectTransporterListItems();

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            return this.View("~/Areas/Master/Views/EcPrefTransporter/Index.cshtml", vm);
        }

        #endregion Private
    }
}