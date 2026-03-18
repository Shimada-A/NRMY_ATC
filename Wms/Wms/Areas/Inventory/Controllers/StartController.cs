namespace Wms.Areas.Inventory.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Inventory.Models;
    using Wms.Areas.Inventory.Query.Start;
    using Wms.Areas.Inventory.Resources;
    using Wms.Areas.Inventory.ViewModels.Start;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class StartController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_INV_Start.SearchConditions";

        private StartQuery _StartQuery;
        private Report _ReportQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartController"/> class.
        /// </summary>
        public StartController()
        {
            this._StartQuery = new StartQuery();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(StartSearchConditions SearchConditions)
        {
            StartSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region ロード処理

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Download(StartSearchConditions SearchConditions)
        {
            Reports.Export.StartReport report = new Reports.Export.StartReport(ReportTypes.Excel, SearchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// Upload処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(StartSearchConditions SearchConditions)
        {
            var guid = Guid.NewGuid().ToString();
            var uploadFile = Request.Files[0];

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);

            // ファイルの取込
            var report = new Reports.Import.StartReport(ReportTypes.Excel, uploadFile, guid);

            // Excelのデータをワークテーブルに登録（このタイミングでワークIDを採番する）
            report.Import();
            var workId = report._seq;
            var message = report._message;
            SearchConditions.Seq = workId;
            SearchConditions.FileName = uploadFile.FileName;

            // 登録
            if (string.IsNullOrWhiteSpace(message))
            {
                // 実績更新
                var retmessage = string.Empty;
                ProcedureStatus status = ProcedureStatus.Success;
                var locCnt = MvcDbContext.Current.InvStart_01s.Where(x => x.Seq == workId).Count();
                if (locCnt == 0)
                {
                    TempData[AppConst.ERROR] = StartResource.ErrNoData;
                    // Return index view
                    return this.Json(new { err = "true" });
                }
                else
                {
                    _StartQuery.InventoryStart(SearchConditions, out status, out retmessage);
                    if (status == ProcedureStatus.Success)
                    {
                        // Clear message to back to index screen
                        TempData[AppConst.SUCCESS] = string.Format(StartResource.AllUploadSuc, locCnt);
                        // Return index view
                        return this.Json(new { err = "false" });
                    }
                    else
                    {
                        TempData[AppConst.ERROR] = retmessage;
                        // Return index view
                        return this.Json(new { err = "true" });
                    }
                }
            }
            else
            {
                // Clear message to back to index screen
                TempData[AppConst.ERROR] = message;
                // Return index view
                return this.Json(new { err = "true" });
            }
        }

        #endregion ロード処理

        #region 更新処理

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="searchConditions">searchConditions</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventoryStart(StartSearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _StartQuery.InventoryStart(searchConditions, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = StartResource.AllUpdateSuc;
                // Return index view
                return RedirectToAction("IndexSearch");
            }
            else
            {
                TempData[AppConst.ERROR] = message;
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                return RedirectToAction("IndexSearch");
            }
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private StartSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new StartSearchConditions() : Request.Cookies.Get<StartSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new StartSearchConditions();
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
        private ActionResult GetSearchResultView(StartSearchConditions condition, bool indexFlag)
        {
            // Save search info
            var vm = new StartViewModel
            {
                SearchConditions = condition,
                Results = indexFlag ? new StartResult() : new StartResult()
                {
                    Starts = _StartQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.Starts.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.Starts = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Inventory/Views/Start/Index.cshtml", vm);
        }

        #endregion Private
    }
}