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
    using Wms.Areas.Master.ViewModels.ShipFrontage;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class ShipFrontageController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASShipFrontage01.SearchConditions";
        private const string COOKIE_REPORT_WORK_ID = "WMASShipFrontage03.WorkId";
        private const string COOKIE_REPORT_ERR_PAGE = "WMASShipFrontage03c.Page";

        private ShipFrontage _ShipFrontageQuery;
        private Query.ShipFrontage.Report _ShipFrontageReportQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipFrontageController"/> class.
        /// </summary>
        public ShipFrontageController()
        {
            this._ShipFrontageQuery = new ShipFrontage();
            this._ShipFrontageReportQuery = new Query.ShipFrontage.Report();
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
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(ShipFrontageSearchCondition SearchConditions)
        {
            ShipFrontageSearchCondition condition;

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

        public JsonResult GetPKeys(ShipFrontageSearchCondition SearchConditions)
        {
            List<string> ids = _ShipFrontageQuery.GetRowId(SearchConditions).Select(n => n.Rid).ToList();
            return Json(ids, JsonRequestBehavior.AllowGet);
        }
        #endregion Search

        #region Add

        /// <summary>
        /// 新規作成画面
        /// </summary>
        /// <returns>Create View</returns>
        public ActionResult Create(ShipFrontageSearchCondition SearchConditions)
        {
            var input = new ShipFrontageResultRow
            {
                CenterId = SearchConditions.CenterId
            };
            input.SearchFlag= SearchConditions.SearchFlag;
            return View("~/Areas/Master/Views/ShipFrontage/Edit.cshtml", input);
        }

        /// <summary>
        /// 新規作成処理
        /// </summary>
        /// <param name="shipFrontage">ShipFrontage Information</param>
        /// <returns>Create View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShipFrontageResultRow shipFrontage)
        {
            ModelState.Remove("ShipperId");

            // 出荷先IDチェック
            CheckStoreId(shipFrontage);
            
            //ブランドIDチェック
            CheckBrandId(shipFrontage);

            if (ModelState.IsValid)
            {
                var shipFrontageExisted = _ShipFrontageQuery.GetTargetById(shipFrontage);
                if (shipFrontageExisted != null)
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                    return View("~/Areas/Master/Views/ShipFrontage/Edit.cshtml", new ShipFrontageResultRow());
                }
                else
                {
                    if (_ShipFrontageQuery.Create(shipFrontage))
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

            return View("~/Areas/Master/Views/ShipFrontage/Edit.cshtml", new ShipFrontageResultRow());
        }

        #endregion Add

        #region Edit

        /// <summary>
        /// Edit ShipFrontage Information
        /// </summary>
        /// <param name="rowid">rowid</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(string rowid)
        {
            // Get record from DB
            var target = _ShipFrontageQuery.GetTargetById(rowid);

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            target.SearchFlag = true;

            return View("~/Areas/Master/Views/ShipFrontage/Edit.cshtml", target);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="shipFrontage"></param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ShipFrontageResultRow shipFrontage)
        {
            // 出荷先IDチェック
            CheckStoreId(shipFrontage);

            //ブランドIDチェック
            CheckBrandId(shipFrontage);

            if (ModelState.IsValid)
            {
                if (_ShipFrontageQuery.UpdateShipFrontage(shipFrontage))
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

            return View("~/Areas/Master/Views/ShipFrontage/Edit.cshtml", shipFrontage);
        }

        #endregion Edit

        #region Delete

        /// <summary>
        /// Delete rows
        /// </summary>
        /// <param name="rowid">rowid</param>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string rowid)
        {
            List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(rowid);

            var isSuccess = _ShipFrontageQuery.Delete(list);

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

        #region ロード処理

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Download()
        {
            ShipFrontageSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.ShipFrontageReport report = new Reports.Export.ShipFrontageReport(ReportTypes.Excel, searchCondition);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }
        /// <summary>
        /// テンプレートレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DownloadTemp()
        {
            ShipFrontageSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.ShipFrontageReportTemp report = new Reports.Export.ShipFrontageReportTemp(ReportTypes.Excel, searchCondition);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// Upload処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(Index vm)
        {
            var guid = Guid.NewGuid().ToString();
            var uploadFile = Request.Files[0];

            // var importFilePath = Path.Combine(Share.Common.AppConfig.TempUploadDir, Path.GetFileName(uploadFile.FileName));
            // uploadFile.SaveAs(importFilePath);
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, vm.SearchConditions);

            // ファイルの取込
            // var importFile = new FileInfo(importFilePath);
            var report = new Reports.Import.ShipFrontageReport(ReportTypes.Excel, uploadFile, guid);

            // Excelのデータをワークテーブルに登録（このタイミングでワークIDを採番する）
            report.Import();
            var workId = report._seq;

            // エラーチェック
            var wShipFrontages = MvcDbContext.Current.MasShipFrontages.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId).ToList();
            var check = _ShipFrontageReportQuery.UploadCheck(workId, wShipFrontages);

            // 登録
            if (!check)
            {
                this.TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPLOAD;
                return this.Json(vm);
            }
            else
            {
                CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_WORK_ID, workId);

                // エラーが無い場合は、ワークテーブルの内容をもとに、マスタにマージ
                if (!MvcDbContext.Current.MasShipFrontages.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId && x.ErrMsg != null).Any())
                {
                    _ShipFrontageReportQuery.MergeShipFrontages(workId);
                    this.TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPLOAD;
                    return this.Json(new { err = "false" });
                }

                // エラーがある場合は、ワークテーブルの内容をアップロード結果画面に表示　→　このときの表示データをページングしてください
                else
                {
                    return this.Json(new { err = "true" });
                }
            }
        }

        /// <summary>
        /// Upload処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public ActionResult UploadErr(Upload vm)
        {
            var workId = vm.UploadConditions.WorkId == 0 ? (Request.Cookies.Get<long>(COOKIE_REPORT_WORK_ID) != 0 ? Request.Cookies.Get<long>(COOKIE_REPORT_WORK_ID) : 0) : vm.UploadConditions.WorkId;
            if (vm.Page == 0)
            {
                vm = new Upload
                {
                    Page = 1,
                    UploadConditions = new UploadCondition
                    {
                        WorkId = workId,
                        Page = 1,
                        PageSize = GetCurrentPageSize()
                    }
                };

                vm.UploadResult = new UploadResult
                {
                    ShipFrontages = _ShipFrontageReportQuery.GetReportErrList(vm.UploadConditions)
                };
                CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_ERR_PAGE, 1);
            }
            else
            {
                vm.UploadConditions.Page = vm.Page;
                vm.UploadConditions.PageSize = GetCurrentPageSize();
                vm.UploadConditions.WorkId = workId;
                vm.UploadResult = new UploadResult
                {
                    ShipFrontages = _ShipFrontageReportQuery.GetReportErrList(vm.UploadConditions)
                };
                CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_ERR_PAGE, vm.UploadConditions.Page);
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_WORK_ID, workId);
            return this.View("~/Areas/Master/Views/ShipFrontage/Upload.cshtml", vm);
        }

        #endregion ロード処理

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private ShipFrontageSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new ShipFrontageSearchCondition() : Request.Cookies.Get<ShipFrontageSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new ShipFrontageSearchCondition();
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
        private ActionResult GetSearchResultView(ShipFrontageSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                ShipFrontageResult = indexFlag ? new ShipFrontageResult() : new ShipFrontageResult()
                {
                    ShipFrontages = _ShipFrontageQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.ShipFrontageResult.ShipFrontages.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.ShipFrontageResult.ShipFrontages = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Master/Views/ShipFrontage/Index.cshtml", vm);
        }

        /// <summary>
        /// 出荷先IDチェック
        /// </summary>
        /// <param name="storeId">出荷先ID</param>
        /// <returns></returns>
        private void CheckStoreId(ShipFrontageResultRow shipFrontage)
        {
            // マスタ存在チェック
            if (!_ShipFrontageQuery.CheckMasterExists(shipFrontage.StoreId))
            {
                this.ModelState.AddModelError("StoreId", string.Format(MessagesResource.MasterNotExistsError, ShipFrontageResource.StoreId));
            }

            // ブランド―荷主―倉庫―店舗単位で登録済みではないかチェック
            if (_ShipFrontageQuery.CheckUniqueIndex(shipFrontage))
            {
                this.ModelState.AddModelError("StoreId", ShipFrontageResource.ErrorStoreIdAlreadySaved);
            }
        }

        private void CheckBrandId(ShipFrontageResultRow shipFrontage)
        {
            //マスタ存在チェック
            if(!_ShipFrontageQuery.CheckBrandExists(shipFrontage.BrandId))
            {
                this.ModelState.AddModelError("BrandId",string.Format(MessagesResource.MasterNotExistsError, ShipFrontageResource.BrandId));
            }
        }
        #endregion Private
    }
}