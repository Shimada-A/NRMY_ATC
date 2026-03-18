namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.Vendor;
    using Wms.Controllers;
    using Wms.Resources;

    public class VendorController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASVendor01.SearchConditions";

        private Vendor _VendorQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorController"/> class.
        /// </summary>
        public VendorController()
        {
            this._VendorQuery = new Vendor();
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
        public ActionResult Search(VendorSearchCondition SearchConditions)
        {
            VendorSearchCondition condition;

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
        public ActionResult Edit(IList<SelectedVendorViewModel> vendors)
        {
            var vendor = vendors.Where(x => x.IsCheck == true).Single();

            // Get record from DB
            var target = _VendorQuery.GetTargetById(vendor.VendorId, Common.Profile.User.ShipperId);

            // 更新対象のデータがマスタに無い場合、エラー
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            return View("~/Areas/Master/Views/Vendor/Edit.cshtml", target);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Vendor vendor)
        {
            // 都道府県入力チェック
            if (!_VendorQuery.SelectPrefs(vendor))
            {
                this.ModelState.AddModelError("VendorPrefName", MessageResource.ERR_NOT_PREFS);
            }

            // 郵便番号ハイフンチェック
            if (vendor.VendorZip.Contains('-'))
            {
                this.ModelState.AddModelError("VendorZip", MessageResource.ERR_ZIP_HYPHEN);
            }

            if (ModelState.IsValid)
            {
                if (_VendorQuery.UpdateVendor(vendor))
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

            return View("~/Areas/Master/Views/Vendor/Edit.cshtml", vendor);
        }

        #endregion Edit

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private VendorSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new VendorSearchCondition() : Request.Cookies.Get<VendorSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new VendorSearchCondition();
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
        private ActionResult GetSearchResultView(VendorSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                VendorResult = indexFlag ? new VendorResult() : new VendorResult()
                {
                    Vendors = _VendorQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.VendorResult.Vendors.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.VendorResult.Vendors = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view

            // return this.View("Index", vm);
            return this.View("~/Areas/Master/Views/Vendor/Index.cshtml", vm);
        }

        #endregion Private
    }
}