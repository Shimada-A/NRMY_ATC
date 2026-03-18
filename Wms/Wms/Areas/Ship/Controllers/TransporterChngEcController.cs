namespace Wms.Areas.Ship.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Threading;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Models;
    using Wms.Areas.Ship.Query.TransporterChngEc;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.TransporterChngEc;
    using Wms.Controllers;
    using Wms.Hubs;
    using Wms.Models;
    using Wms.Resources;
    using Wms.Common;

    public class TransporterChngEcController : BaseAsyncController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_TransporterChngEc01.SearchConditions";

        private TransporterChngEcQuery _TransporterChngEcQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransporterChngEcController"/> class.
        /// </summary>
        public TransporterChngEcController()
        {
            this._TransporterChngEcQuery = new TransporterChngEcQuery();
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
        public ActionResult UpdateSuc()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
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
        public ActionResult Search(TransporterChngEcSearchConditions SearchConditions)
        {
            TransporterChngEcSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region Selected
        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(TransporterChngEcSearchConditions searchConditions)
        {
            // 全選択
            _TransporterChngEcQuery.ShpTransporterChngAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new TransporterChngEcViewModel
            {
                SearchConditions = searchConditions,
                Results = new TransporterChngEcResult()
                {
                    TransporterChngEcs = _TransporterChngEcQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _TransporterChngEcQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/TransporterChngEc/Index.cshtml", vm);
        }
        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(TransporterChngEcSearchConditions searchConditions)
        {
            // 全解除
            _TransporterChngEcQuery.ShpTransporterChngAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new TransporterChngEcViewModel
            {
                SearchConditions = searchConditions,
                Results = new TransporterChngEcResult()
                {
                    TransporterChngEcs = _TransporterChngEcQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _TransporterChngEcQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/TransporterChngEc/Index.cshtml", vm);
        }
        #endregion

        #region 更新処理
        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransporterChangeSearch(TransporterChngEcSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _TransporterChngEcQuery.UpdateShpTransporterChng(searchConditions.TransporterChngEcs);
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _TransporterChngEcQuery.TransporterChange(searchConditions, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = TransporterChngEcResource.UpdateSuc;
                // 検索部を表示
                return RedirectToAction("UpdateSuc");
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;
                // 検索
                var vm = new TransporterChngEcViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new TransporterChngEcResult()
                    {
                        TransporterChngEcs = _TransporterChngEcQuery.GetData(searchConditions)
                    },
                };
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.TransporterList = _TransporterChngEcQuery.GetSelectListTransporters();
                // Return index view
                return this.View("~/Areas/Ship/Views/TransporterChngEc/Index.cshtml", vm);
            }
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransporterChange2(TransporterChngEcSearchConditions searchConditions)
        {
            ModelState.Clear();
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _TransporterChngEcQuery.TransporterChange2(searchConditions, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = TransporterChngResource.UpdateSuc;
                // 検索部を表示
                return RedirectToAction("UpdateSuc");
            }
            else
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;
                // 検索
                var vm = new TransporterChngEcViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new TransporterChngEcResult(),
                };
                ViewBag.TransporterList = _TransporterChngEcQuery.GetSelectListTransporters();
                // Return index view
                return this.View("~/Areas/Ship/Views/TransporterChngEc/Index.cshtml", vm);
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private TransporterChngEcSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new TransporterChngEcSearchConditions() : Request.Cookies.Get<TransporterChngEcSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new TransporterChngEcSearchConditions();
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
        private ActionResult GetSearchResultView(TransporterChngEcSearchConditions searchConditions, bool indexFlag)
        {
            // 画面選択行更新用
            if (!indexFlag && searchConditions.SearchType == SearchTypes.SortPage)
            {
                _TransporterChngEcQuery.UpdateShpTransporterChng(searchConditions.TransporterChngEcs);
            }

            // 作成処理&検索表示
            var vm = new TransporterChngEcViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new TransporterChngEcResult() : ((searchConditions.SearchType == SearchTypes.Search ? _TransporterChngEcQuery.InsertShpTransporterChngEc(searchConditions) : true) ? new TransporterChngEcResult()
                {
                    TransporterChngEcs = _TransporterChngEcQuery.GetData(searchConditions)
                }
                : new TransporterChngEcResult()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null && !indexFlag)
            {
                if (vm.Results.TransporterChngEcs.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.TransporterChngEcs = null;
                }
            }

            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _TransporterChngEcQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/TransporterChngEc/Index.cshtml", vm);
        }

        #endregion Private

        #region 「対象ケースNo」スキャン後

        /// <summary>
        /// サイズチェック
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult CheckSize(string size)
        {
            var _cnt = _TransporterChngEcQuery.CheckSize(size);

            return this.Json(new { cnt = _cnt });
        }
        #endregion
    }
}