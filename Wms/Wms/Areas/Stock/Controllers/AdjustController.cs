namespace Wms.Areas.Stock.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Stock.Query.Adjust;
    using Wms.Areas.Stock.Resources;
    using Wms.Areas.Stock.ViewModels.Adjust;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;

    public class AdjustController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-STK_Adjust.SearchConditions";

        private AdjustQuery _AdjustQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjustController"/> class.
        /// </summary>
        public AdjustController()
        {
            this._AdjustQuery = new AdjustQuery();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(AdjustSearchConditions SearchConditions)
        {
            if(SearchConditions.SearchType == Common.SearchTypes.Search && new BaseQuery().IsAllocProcessing() == 1)
            {
                TempData[AppConst.ERROR] = MessagesResource.IS_ALLOC_PROCESSING;
                return this.GetSearchResultView(SearchConditions, true);
            }
            else if (SearchConditions.SearchType == Common.SearchTypes.Search && new BaseQuery().IsDailyProcessing() == 1)
            {
                TempData[AppConst.ERROR] = MessagesResource.IS_DAILY_PROCESSING;
                return this.GetSearchResultView(SearchConditions, true);
            }
            else
            {
                AdjustSearchConditions condition = SearchConditions;
                condition.PageSize = this.GetCurrentPageSize();
                return this.GetSearchResultView(condition, false);
            }
        }

        #endregion Search

        #region 入力

        /// <summary>
        /// 入力
        /// </summary>
        /// <param name="searchCondition">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Input(IList<SelectedAdjustViewModel> Adjusts)
        {
            if (Adjusts.Where(x => x.IsCheck == true).Count() != 1)
            {
                // TODO:エラーメッセージをリソースファイルに
                TempData[AppConst.ERROR] = MessagesResource.SELECT_ONE_RECORD;
                return RedirectToAction("IndexSearch");
            }

            // 棚卸中チェック
            if (new BaseQuery().IsTanaLocProcessing() == 1)
            {
                TempData[AppConst.ERROR] = MessagesResource.IS_TANA_LOC_PROCESSING;
                return RedirectToAction("IndexSearch");
            }

            // 引当処理中チェック
            if (new BaseQuery().IsAllocProcessing() == 1)
            {
                TempData[AppConst.ERROR] = MessagesResource.IS_ALLOC_PROCESSING;
                return RedirectToAction("IndexSearch");
            }

            // 日次処理中チェック
            if (new BaseQuery().IsDailyProcessing() == 1)
            {
                TempData[AppConst.ERROR] = MessagesResource.IS_DAILY_PROCESSING;
                return RedirectToAction("IndexSearch");
            }

            // ワークID採番
            var seq = new BaseQuery().GetWorkId();

            //在庫調整ワーク02作成処理
            var adjust01 = Adjusts.Where(x => x.IsCheck == true).Single();
            if (adjust01 != null)
            {
                _AdjustQuery.InsertStkAdjust02(seq, adjust01.Seq, adjust01.LineNo);
            }

            // 「在庫調整（調整数入力・ケース/バラ）」表示処理
            AdjustInputViewModel vm = new AdjustInputViewModel();
            vm = _AdjustQuery.AdjustInputGetData(seq);
            return this.View("~/Areas/Stock/Views/Adjust/Input.cshtml", vm);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(AdjustInputViewModel vm)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                // 排他チェック
                if (!_AdjustQuery.ExclusiveCheck(vm))
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                    return this.View("~/Areas/Stock/Views/Adjust/Input.cshtml", vm);
                }
                // アソートロケチェック
                if (vm.LocationClass == "02" && vm.AllocQty > 0)
                {
                    TempData[AppConst.ERROR] = MessageResource.ERR_ASORTCASE_UPDATE;
                    return this.View("~/Areas/Stock/Views/Adjust/Input.cshtml", vm);
                }

                // ワーク02更新
                if (_AdjustQuery.UpdateStkAdjust02(vm))
                {
                    // 「在庫・荷姿別在庫」更新処理
                    var message = _AdjustQuery.UpdateStock(vm);
                    if (string.IsNullOrWhiteSpace(message))
                    {
                        // Clear message to back to index screen
                        TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
                        return RedirectToAction("IndexSearch");
                    }
                    else
                    {
                        TempData[AppConst.ERROR] = message;
                        return this.View("~/Areas/Stock/Views/Adjust/Input.cshtml", vm);
                    }
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                    return this.View("~/Areas/Stock/Views/Adjust/Input.cshtml", vm);
                }

            }

            // エラー内容を取得・セット
            var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
            foreach (var message in errorMessages)
            {
                ModelState.AddModelError(string.Empty, message);
            }
            return this.View("~/Areas/Stock/Views/Adjust/Input.cshtml", vm);
        }

        #endregion 入力

        #region 新規登録
        /// <summary>
        /// 新規登録画面へ遷移
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        public ActionResult CreateInput(AdjustSearchConditions SearchConditions)
        {
            var vm = new AdjustCreateViewModel
            {
                CenterId = SearchConditions.CenterId
            };

            return View("Create", vm);
        }

        /// <summary>
        /// 新規登録処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdjustCreateViewModel vm)
        {
            //JANとSKUのどちらかは必須
            if (string.IsNullOrEmpty(vm.ItemSkuId) && string.IsNullOrEmpty(vm.Jan))
            {
                ModelState.AddModelError(nameof(vm.ItemSkuId), AdjustResource.RequiredWhichJan);
                ModelState.AddModelError(nameof(vm.Jan), AdjustResource.RequiredWhichJan);
            }

            var location = _AdjustQuery.GetLocation(vm.CenterId, vm.LocationCd);

            //ロケマスタに存在しない
            if (location == null)
            {
                ModelState.AddModelError(nameof(vm.LocationCd), AdjustResource.NotExistsLocation);
            }
            else
            {
                //ケース保管ロケまたはアソートロケ時はケースNoは必須
                if (location.LocationClass == "01" || location.LocationClass == "02")
                {
                    if (string.IsNullOrEmpty(vm.BoxNo))
                    {
                        ModelState.AddModelError(nameof(vm.BoxNo), AdjustResource.RequiredBoxNo);
                    }
                }

                //調整ロケは指定できない
                if (location.LocationClass == "16")
                {
                    ModelState.AddModelError(nameof(vm.LocationCd), AdjustResource.AdjustLocationError);
                }
            }

            //ケースNoが登録されているロケーションコードと違う
            if (!string.IsNullOrEmpty(vm.BoxNo))
            {
                var locationCd = _AdjustQuery.GetLocationCdBoxNo(vm.CenterId, vm.BoxNo);

                if (locationCd != null && locationCd != vm.LocationCd)
                {
                    ModelState.AddModelError(nameof(vm.BoxNo), AdjustResource.DifferentLocationCd);
                }
            }

            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
                foreach (var msg in errorMessages)
                {
                    ModelState.AddModelError(string.Empty, msg);
                }
                return View("Create", vm);
            }

            ModelState.Clear();

            //JANとSKUの両方入力された場合、JANを無効とする
            if (!string.IsNullOrEmpty(vm.ItemSkuId) && !string.IsNullOrEmpty(vm.Jan))
            {
                vm.Jan = null;
            }

            // 在庫作成処理
            var message = _AdjustQuery.CreateStock(vm);
            if (string.IsNullOrWhiteSpace(message))
            {
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_INSERT;
                return RedirectToAction("IndexSearch");
            }
            else
            {
                TempData[AppConst.ERROR] = message;
                return View("Create", vm);
            }
        }
        #endregion 新規登録

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private AdjustSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new AdjustSearchConditions() : Request.Cookies.Get<AdjustSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new AdjustSearchConditions();
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
        private ActionResult GetSearchResultView(AdjustSearchConditions searchConditions, bool indexFlag)
        {
            // 作成処理&検索表示
            var vm = new AdjustViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new AdjustResult() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _AdjustQuery.InsertStkAjust(searchConditions) : true) ? new AdjustResult()
                {
                    Adjusts = _AdjustQuery.GetData(searchConditions)
                }
                : new AdjustResult()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm.Results.Adjusts != null)
            {
                if (vm.Results.Adjusts.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.Adjusts = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.DivisionList = _AdjustQuery.GetSelectListDivisions();
            ViewBag.LocationClassList = _AdjustQuery.GetSelectListLocationClasses();
            ViewBag.Category1List = _AdjustQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _AdjustQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _AdjustQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _AdjustQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);
            ViewBag.GradeList = _AdjustQuery.GetSelectListGrades();
            ViewBag.ItemList = _AdjustQuery.GetSelectListItems();

            // Return index view
            return this.View("~/Areas/Stock/Views/Adjust/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion Private
    }
}