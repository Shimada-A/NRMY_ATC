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
    using Wms.Areas.Ship.Query.TransporterChng;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.TransporterChng;
    using Wms.Controllers;
    using Wms.Hubs;
    using Wms.Models;
    using Wms.Resources;
    using Wms.Common;

    public class TransporterChngController : BaseAsyncController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_TransporterChng01.SearchConditions";

        private TransporterChngQuery _TransporterChngQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransporterChngController"/> class.
        /// </summary>
        public TransporterChngController()
        {
            this._TransporterChngQuery = new TransporterChngQuery();
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
        public ActionResult Search(TransporterChngSearchConditions SearchConditions)
        {
            TransporterChngSearchConditions condition = SearchConditions;
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
        public ActionResult AllSelectedSearch(TransporterChngSearchConditions searchConditions)
        {
            // 全選択
            _TransporterChngQuery.ShpTransporterChngAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new TransporterChngViewModel
            {
                SearchConditions = searchConditions,
                Results = new TransporterChngResult()
                {
                    TransporterChngs = _TransporterChngQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _TransporterChngQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/TransporterChng/Index.cshtml", vm);
        }
        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(TransporterChngSearchConditions searchConditions)
        {
            // 全解除
            _TransporterChngQuery.ShpTransporterChngAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();
            // 検索表示
            var vm = new TransporterChngViewModel
            {
                SearchConditions = searchConditions,
                Results = new TransporterChngResult()
                {
                    TransporterChngs = _TransporterChngQuery.GetData(searchConditions)
                },
            };
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _TransporterChngQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/TransporterChng/Index.cshtml", vm);
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
        public ActionResult TransporterChangeSearch(TransporterChngSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            // 画面選択行更新用
            _TransporterChngQuery.UpdateShpTransporterChng(searchConditions.TransporterChngs);
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _TransporterChngQuery.TransporterChange(searchConditions, out status, out message);
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
                var vm = new TransporterChngViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new TransporterChngResult()
                    {
                        TransporterChngs = _TransporterChngQuery.GetData(searchConditions)
                    },
                };
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                ViewBag.TransporterList = _TransporterChngQuery.GetSelectListTransporters();
                // Return index view
                return this.View("~/Areas/Ship/Views/TransporterChng/Index.cshtml", vm);
            }
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransporterChange2(TransporterChngSearchConditions searchConditions)
        {
            ModelState.Clear();
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _TransporterChngQuery.TransporterChange2(searchConditions, out status, out message);
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
                var vm = new TransporterChngViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new TransporterChngResult(),
                };
                ViewBag.TransporterList = _TransporterChngQuery.GetSelectListTransporters();
                // Return index view
                return this.View("~/Areas/Ship/Views/TransporterChng/Index.cshtml", vm);
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private TransporterChngSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new TransporterChngSearchConditions() : Request.Cookies.Get<TransporterChngSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new TransporterChngSearchConditions();
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
        private ActionResult GetSearchResultView(TransporterChngSearchConditions searchConditions, bool indexFlag)
        {
            // 画面選択行更新用
            if (!indexFlag && searchConditions.SearchType == SearchTypes.SortPage)
            {
                _TransporterChngQuery.UpdateShpTransporterChng(searchConditions.TransporterChngs);
            }

            // 作成処理&検索表示
            var vm = new TransporterChngViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new TransporterChngResult() : ((searchConditions.SearchType == SearchTypes.Search ? _TransporterChngQuery.InsertShpTransporterChng(searchConditions) : true) ? new TransporterChngResult()
                {
                    TransporterChngs = _TransporterChngQuery.GetData(searchConditions)
                }
                : new TransporterChngResult()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null && !indexFlag)
            {
                if (vm.Results.TransporterChngs.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.TransporterChngs = null;
                }
            }

            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            ViewBag.TransporterList = _TransporterChngQuery.GetSelectListTransporters();

            // Return index view
            return this.View("~/Areas/Ship/Views/TransporterChng/Index.cshtml", vm);
        }

        #endregion Private

        #region 「対象ケースNo」スキャン後

        /// <summary>
        /// サイズチェック
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult CheckSize(string size)
        {
            var _cnt = _TransporterChngQuery.CheckSize(size);

            return this.Json(new { cnt = _cnt });
        }
        #endregion
    }
}