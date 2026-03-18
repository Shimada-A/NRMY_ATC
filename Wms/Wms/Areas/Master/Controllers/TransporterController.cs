namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.Transporter;
    using Wms.Controllers;
    using Wms.Resources;

    public class TransporterController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASTransporter01.SearchConditions";

        private Transporter _TransporterQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransporterController"/> class.
        /// </summary>
        public TransporterController()
        {
            this._TransporterQuery = new Transporter();
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
        public ActionResult Search(TransporterSearchCondition SearchConditions)
        {
            TransporterSearchCondition condition;

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
        /// Edit Country Information
        /// </summary>
        /// <param name="countries">List Country</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(IList<SelectedTransporterViewModel> transporters)
        {
            var transporter = transporters.Where(x => x.IsCheck == true).Single();

            // Get record from DB
            var target = _TransporterQuery.GetTargetById(transporter.TransporterId, Common.Profile.User.ShipperId);

            // 更新対象のデータがマスタに無い場合、エラー
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            return View("~/Areas/Master/Views/Transporter/Edit.cshtml", target);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Transporter transporter)
        {
            // 郵便番号ハイフンチェック
            if (!string.IsNullOrEmpty(transporter.TransporterZip) && transporter.TransporterZip.Contains('-'))
            {
                this.ModelState.AddModelError("TransporterZip", MessageResource.ERR_ZIP_HYPHEN);
            }

            if (ModelState.IsValid)
            {
                if (_TransporterQuery.UpdateTransporter(transporter))
                {
                    // Clear message to back to index screen
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

            return View("~/Areas/Master/Views/Transporter/Edit.cshtml", transporter);
        }

        #endregion Edit

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private TransporterSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new TransporterSearchCondition() : Request.Cookies.Get<TransporterSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new TransporterSearchCondition();
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
        private ActionResult GetSearchResultView(TransporterSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                TransporterResult = indexFlag ? new TransporterResult() : new TransporterResult()
                {
                    Transporters = _TransporterQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.TransporterResult.Transporters.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.TransporterResult.Transporters = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Master/Views/Transporter/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion Private
    }
}