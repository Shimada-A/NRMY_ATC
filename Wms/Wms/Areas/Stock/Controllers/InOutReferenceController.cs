using Share.Common;
using Share.Extensions.Classes;
using System;
using System.Linq;
using System.Web.Mvc;
using Wms.Areas.Stock.Query.InOutReference;
using Wms.Areas.Stock.Reports.Export;
using Wms.Areas.Stock.ViewModels.InOutReference;
using Wms.Controllers;
using Wms.Resources;

namespace Wms.Areas.Stock.Controllers
{
    public class InOutReferenceController : BaseController
    {
        #region Constants
        // 検索条件保存Cookie名
        private string CookieSearchConditions { get; } = "InOutReference.SearchConditions";

        public InOutReferenceController() { }
        #endregion Constants

        #region Index
        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return ViewIndex(new InOutReferenceViewModel()
            {
                SearchConditions = new InOutReferenceSearchConditions()
                {
                    // 初期値設定
                    CenterId = Wms.Common.Profile.User.CenterId,
                    MoveDateFrom = DateTime.Now.Date,
                    MoveDateTo = DateTime.Now.Date,
                    NotZeroDisp = true
                }
            });
        }
        #endregion Index

        #region Search
        /// <summary>
        /// ページ切替
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Search(int page)
        {
            // Cookieより検索条件を取得
            var conditions = Request.Cookies.Get<InOutReferenceSearchConditions>(CookieSearchConditions) ?? new InOutReferenceSearchConditions();

            conditions.SearchType = Common.SearchTypes.SortPage;
            conditions.PageNumber = page;

            return Search(conditions);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(InOutReferenceSearchConditions searchConditions)
        {
            var vm = new InOutReferenceViewModel
            {
                SearchConditions = searchConditions
            };

            // 入力チェック
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
                foreach (var msg in errorMessages)
                {
                    ModelState.AddModelError(string.Empty, msg);
                }
                return ViewIndex(vm);
            }

            ModelState.Clear();

            if (searchConditions.SearchType == Common.SearchTypes.Search)
            {
                // 検索時
                // 表示順、ページ情報を初期値設定
                vm.SearchConditions.PageNumber = 1;
                vm.SearchConditions.PageSize = GetCurrentPageSize();
                vm.SearchConditions.SortKey = InOutReferenceSearchConditions.StockSortKey.Operation;
                vm.SearchConditions.PackageSortKey = InOutReferenceSearchConditions.PackageStockSortKey.Operation;
                vm.SearchConditions.Sort = InOutReferenceSearchConditions.AscDescSort.Asc;
            }

            // 検索
            if (searchConditions.ResultType == Common.ResultTypes.Stock)
            {
                // 在庫明細
                vm.Results = new InOutReferenceQuery().GetStockData(searchConditions);
            }
            else
            {
                // ケース明細
                vm.Results = new InOutReferenceQuery().GetPackageStockData(searchConditions);
            }

            // 処理可能件数チェック
            var procNumLimit = GetCurrentProcNumLimit();

            if (procNumLimit != 0)
            {
                if (vm.Results.TotalItemCount > procNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, procNumLimit);
                    vm.Results = null;
                }
            }

            // Cookieに検索条件を保存
            CookieExtention.SetSearchConditonCookie(CookieSearchConditions, searchConditions);

            return ViewIndex(vm);
        }

        /// <summary>
        /// Index Viewの表示
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private ActionResult ViewIndex(InOutReferenceViewModel viewModel)
        {
            ViewBag.LocationClassList = new InOutReferenceQuery().GetSelectListLocationClasses();

            ModelState.Clear();

            return View("Index", viewModel);
        }
        #endregion Search

        #region Download
        /// <summary>
        /// ダウンロード処理
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Download(InOutReferenceSearchConditions searchConditions)
        {
            if (searchConditions.ResultType == Common.ResultTypes.Stock)
            {
                // 在庫明細
                var report = new InOutReferenceStockReport(ReportTypes.Excel, searchConditions);
                report.Export();
                return File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
            else
            {
                // ケース明細
                var report = new InOutReferencePackageStockReport(ReportTypes.Excel, searchConditions);
                report.Export();
                return File(report.FileContent, report.ContentType, report.DownloadFileName);
            }
        }
        #endregion Download
    }
}