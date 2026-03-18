namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.ShippingHoldStores;
    using Wms.Controllers;
    using Wms.Resources;
    using Wms.Areas.Master.Query.ShippingHoldStores;
    using Wms.Query;
    using System.Web.UI.WebControls;

    public class ShippingHoldStoresController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASShippingHoldStores01.SearchConditions";
        private ShippingHoldStoresQuery _ShippingHoldStoresQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingHoldStoresController"/> class.
        /// </summary>
        public ShippingHoldStoresController()
        {
            this._ShippingHoldStoresQuery = new ShippingHoldStoresQuery();
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
        /// 「確定する」押下時の処理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexConfirm(long seq, string CheckList)
        {
            // 画面でチェックされた一覧
            List<MasShippingHoldStore> updateList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MasShippingHoldStore>>(CheckList);

            // ワークテーブルを更新
            if (!_ShippingHoldStoresQuery.UpdateWork(updateList))
            {
                TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                return RedirectToAction("Index", "ShippingHoldStores");
            }
            // 確定処理実行
            if (_ShippingHoldStoresQuery.Confirm(seq))
            {
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_INSERT;
            }
            else
            {
                TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
            }

            return RedirectToAction("Index", "ShippingHoldStores");
        }

        /// <summary>
        /// キー項目
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public JsonResult GetRowId(long seq)
        {
            //ROWIDを取得する
            List<MasShippingHoldStore> retList = _ShippingHoldStoresQuery.GetKeyWords(seq).ToList();
            return Json(new { resultList = retList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(SearchCondition SearchConditions)
        {
            SearchCondition condition;
            BaseQuery baseQuery = new BaseQuery();

            // When page is clicked, page > 1
            if (SearchConditions.Page >= 1)
            {
                condition = this.GetPreviousSearchInfo(false);
                condition.Page = SearchConditions.Page;
                condition.UpdateWorkFlag = false;
            }
            else
            {
                condition = SearchConditions;
                condition.PageSize = this.GetCurrentPageSize();
                condition.Page = 1;
                condition.UpdateWorkFlag = true;
                // ワークIDを採番する
                condition.Seq = baseQuery.GetWorkId();
                // ワークテーブル登録
                _ShippingHoldStoresQuery.InsertWork(condition);
            }

            return this.GetSearchResultView(condition, false);
        }

        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private SearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new SearchCondition() : Request.Cookies.Get<SearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new SearchCondition();
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
        private ActionResult GetSearchResultView(SearchCondition condition, bool indexFlag)
        {

            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                Details = indexFlag ? new Details() : new Details()
                {
                    ShippingHoldStores = _ShippingHoldStoresQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Details.ShippingHoldStores.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Details.ShippingHoldStores = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            return this.View("~/Areas/Master/Views/ShippingHoldStores/Index.cshtml", vm);

        }
        #endregion

    }
}