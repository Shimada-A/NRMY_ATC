namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.ItemSku;
    using Wms.Controllers;
    using Wms.Resources;

    public class ItemSkuController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASItemSku01.SearchConditions";
        private ItemSku _ItemSkuQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSkuController"/> class.
        /// </summary>
        public ItemSkuController()
        {
            this._ItemSkuQuery = new ItemSku();
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
        public ActionResult Search(ItemSkuSearchCondition SearchConditions)
        {
            ItemSkuSearchCondition condition;

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
        /// Edit Country Information
        /// </summary>
        /// <param name="countries">List Country</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(IList<SelectedItemSkuViewModel> itemSkus)
        {
            var itemSku = itemSkus.Where(x => x.IsCheck == true).Single();

            // Get record from DB
            var target = _ItemSkuQuery.GetTargetById(itemSku.ItemSkuId, Common.Profile.User.ShipperId);

            // 対象のデータがマスタに無い場合、エラー
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            return View("~/Areas/Master/Views/ItemSku/Edit.cshtml", target);
        }
        /// <summary>
        /// 更新処理 
        /// </summary>
        /// <param name="itemSku"></param>
        /// <returns>EditView</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Detail itemSku)
        {
            if (ModelState.IsValid)
            {
                if (_ItemSkuQuery.UpdateItemSku(itemSku))
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

            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();

            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }

            return View("~/Areas/Master/Views/ItemSku/Edit.cshtml", itemSku);

        }

        #endregion

        #region GetList

        /// <summary>
        /// Get コードより名称を取得
        /// <param name="kbn">マスタ区分</param>
        /// <param name="cd">検索用コード</param>
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult GetCategoryList(int id, string category1, string category2, string category3)
        {
            string _html = "<option value=''>" + Wms.Resources.CommonResource.None + "</option>";

            if (id == 2)
            {
                var listCategory = _ItemSkuQuery.GetSelectListCategorys2(category1);
                foreach (var category in listCategory)
                {
                    _html = _html + "<option value='" + category.Value + "'>" + category.Text + "</option>";
                }
            }
            else if (id == 3)
            {
                var listCategory = _ItemSkuQuery.GetSelectListCategorys3(category1, category2);
                foreach (var category in listCategory)
                {
                    _html = _html + "<option value='" + category.Value + "'>" + category.Text + "</option>";
                }
            }
            else if (id == 4)
            {
                var listCategory = _ItemSkuQuery.GetSelectListCategorys4(category1, category2, category3);
                foreach (var category in listCategory)
                {
                    _html = _html + "<option value='" + category.Value + "'>" + category.Text + "</option>";
                }
            }

            return this.Json(new { html = _html });
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private ItemSkuSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new ItemSkuSearchCondition() : Request.Cookies.Get<ItemSkuSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new ItemSkuSearchCondition();
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
        private ActionResult GetSearchResultView(ItemSkuSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                ItemSkuResult = indexFlag ? new ItemSkuResult() : new ItemSkuResult()
                {
                    ItemSkus = _ItemSkuQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.ItemSkuResult.ItemSkus.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.ItemSkuResult.ItemSkus = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            ViewBag.Category1List = _ItemSkuQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _ItemSkuQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _ItemSkuQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _ItemSkuQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.DivisionList = _ItemSkuQuery.GetSelectListDivisions();
            ViewBag.ItemRankList = _ItemSkuQuery.GetSelectListItemRanks();

            // Return index view
            return this.View("~/Areas/Master/Views/ItemSku/Index.cshtml", vm);
        }

        #endregion
    }
}