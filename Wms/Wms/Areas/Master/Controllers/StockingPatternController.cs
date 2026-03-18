namespace Wms.Areas.Master.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.StockingPattern;
    using Wms.Areas.Move.Query.InputTransfer;
    using Wms.Areas.Ship.ViewModels.PrintEcInvoice;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class StockingPatternController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASStockingPattern01.SearchConditions";

        private StockingPattern _StockingPatternQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockingPatternController/> class.
        /// </summary>
        public StockingPatternController()
        {
            this._StockingPatternQuery = new StockingPattern();
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
        /// <param name="SearchConditions"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(StockingPatternSearchCondition SearchConditions)
        {
            StockingPatternSearchCondition condition;

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

        public JsonResult GetPKeys(StockingPatternSearchCondition SearchConditions)
        {
            List<string> ids = _StockingPatternQuery.GetRowId(SearchConditions).Select(n => n.PatternId).ToList();
            return Json(ids, JsonRequestBehavior.AllowGet);
        }
        #endregion Search

        #region Add

        /// <summary>
        /// 新規作成画面
        /// </summary>
        /// <returns>Create View</returns>
        public ActionResult Create(StockingPatternSearchCondition SearchConditions)
        {
            var category = _StockingPatternQuery.GetListCategory1(true,null);
            var input = new Detail
            {
                Categories = category 
            };
            input.SearchFlag= SearchConditions.SearchFlag;

            ViewBag.StockingClassRadio = _StockingPatternQuery.GetRadioButtonListStockingClass();
            return View("~/Areas/Master/Views/StockingPattern/Edit.cshtml", input );
        }

        /// <summary>
        /// 新規作成処理
        /// </summary>
        /// <param name="stockingPattern">stockingPattern Information</param>
        /// <returns>Create View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detail stockingPattern, List<CategoryTableItem> categoryTable)
        {
            ModelState.Remove("ShipperId");


            if (ModelState.IsValid)
            {
                var stockingPatternExisted = _StockingPatternQuery.GetDeleteTargetById(stockingPattern ,categoryTable);
                if (stockingPatternExisted != null)
                {
                    var category = _StockingPatternQuery.GetListCategory1(true, null);
                    var input = new Detail
                    {
                        Categories = category
                    };
                    TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                    ViewBag.StockingClassRadio = _StockingPatternQuery.GetRadioButtonListStockingClass();
                    return View("~/Areas/Master/Views/StockingPattern/Edit.cshtml", input);
                }
                else
                {
                    if (_StockingPatternQuery.Create(stockingPattern, categoryTable))
                    {
                        TempData[AppConst.SUCCESS] = MessagesResource.SUC_INSERT;
                        return RedirectToAction("Index");
                    }
                }
            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return View("~/Areas/Master/Views/StockingPattern/Edit.cshtml", new Detail());
        }

        #endregion Add

        #region Edit

        /// <summary>
        /// Edit StockingPattern Information
        /// </summary>
        /// <param name="StockingPatterns">StockingPatterns</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(List<Detail> StockingPatterns)
        {

            string patternId = StockingPatterns.Where(n => n.IsCheck).Select(n => n.PatternId).FirstOrDefault();
            // Get record from DB
            var target = _StockingPatternQuery.GetEditTargetById(patternId);
            var category = _StockingPatternQuery.GetListCategory1(false, patternId);

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            var input = new Detail
            {
                PatternId = target.PatternId,
                PatternName = target.PatternName,
                Categories = category,
                ShipperId = target.ShipperId
            };
            input.SearchFlag = true;

            ViewBag.StockingClassRadio = _StockingPatternQuery.GetRadioButtonListStockingClass();
            return View("~/Areas/Master/Views/StockingPattern/Edit.cshtml", input);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="stockingPattern"></param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Detail stockingPattern , List<CategoryTableItem> categoryTable)
        {
            if (ModelState.IsValid)
            {
                if (_StockingPatternQuery.UpdateStockingPattern(stockingPattern,categoryTable))
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

            return View("~/Areas/Master/Views/StockingPattern/Edit.cshtml", stockingPattern);
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// Delete patternId
        /// </summary>
        /// <param name="patternId">stockingPatterns</param>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string patternId)
        {
            List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(patternId);

            var isSuccess = _StockingPatternQuery.Delete(list);

            if (isSuccess)
            {
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_DELETE;
            }
            else
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_ERR_EXCLUSIVE_DELETE;
            }

            return RedirectToAction("IndexSearch");
        }

        #endregion Delete

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>StockingPatternSearchCondition</returns>
        private StockingPatternSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new StockingPatternSearchCondition() : Request.Cookies.Get<StockingPatternSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new StockingPatternSearchCondition();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search stockingPattern Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(StockingPatternSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                StockingPatternResult = indexFlag ? new StockingPatternResult() : new StockingPatternResult()
                {
                    StockingPatterns = _StockingPatternQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.StockingPatternResult.StockingPatterns.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.StockingPatternResult.StockingPatterns = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Master/Views/StockingPattern/Index.cshtml", vm);
        }

        #endregion Private
    }
}