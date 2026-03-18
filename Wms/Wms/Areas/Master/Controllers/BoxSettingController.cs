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
    using Wms.Areas.Master.ViewModels.BoxSetting;
    using Wms.Controllers;
    using Wms.Extensions.Classes;
    using Wms.Models;
    using Wms.Resources;

    public class BoxSettingController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASBoxSetting01.SearchConditions";
        private const string COOKIE_REPORT_WORK_ID = "WMASBoxSetting03.WorkId";
        private const string COOKIE_REPORT_ERR_PAGE = "WMASBoxSetting03.Page";
        private BoxSetting _BoxSettingQuery;
        private Query.BoxSetting.Report _BoxSettingReportQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxSettingController"/> class.
        /// </summary>
        public BoxSettingController()
        {
            this._BoxSettingQuery = new BoxSetting();
            this._BoxSettingReportQuery = new Query.BoxSetting.Report();
        }
        #endregion

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
        public ActionResult Search(BoxSettingSearchCondition SearchConditions)
        {
            BoxSettingSearchCondition condition;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        public JsonResult GetPKeys(BoxSettingSearchCondition SearchConditions)
        {
            List<string> ids = _BoxSettingQuery.GetSettingsId(SearchConditions);
            return Json(ids, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Add

        /// <summary>
        /// 新規作成画面
        /// </summary>
        /// <returns>Create View</returns>
        public ActionResult Create(BoxSettingSearchCondition SearchConditions)
        {
            var vm = new BoxSettingList();

            vm.SearchFlag= SearchConditions.SearchFlag;
            ViewBag.Category1List = _BoxSettingQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BoxSettingQuery.GetSelectListCategorys2(vm.CategoryId1);
            ViewBag.Category3List = _BoxSettingQuery.GetSelectListCategorys3(vm.CategoryId1, vm.CategoryId2);
            ViewBag.Category4List = _BoxSettingQuery.GetSelectListCategorys4(vm.CategoryId1, vm.CategoryId2, vm.CategoryId3);
            return View("~/Areas/Master/Views/BoxSetting/Edit.cshtml", vm);
        }

        /// <summary>
        /// 新規作成処理
        /// </summary>
        /// <param name="boxSetting">BoxSetting Information</param>
        /// <returns>Create View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BoxSettingList boxSetting)
        {
            ViewBag.Category1List = _BoxSettingQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BoxSettingQuery.GetSelectListCategorys2(boxSetting.CategoryId1);
            ViewBag.Category3List = _BoxSettingQuery.GetSelectListCategorys3(boxSetting.CategoryId1, boxSetting.CategoryId2);
            ViewBag.Category4List = _BoxSettingQuery.GetSelectListCategorys4(boxSetting.CategoryId1, boxSetting.CategoryId2, boxSetting.CategoryId3);

            // 品番がマスタに存在しません
            if (!string.IsNullOrWhiteSpace(boxSetting.ItemId) && !MvcDbContext.Current.ItemSkus.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.ItemId == boxSetting.ItemId).Any())
            {
                this.ModelState.AddModelError("ItemId", string.Format(MessagesResource.MasterNotExistsError, BoxSettingResource.ItemId));
            }

            // 品番と分類が一致しません
            if (!string.IsNullOrWhiteSpace(boxSetting.ItemId))
            {
                // 分類1
                if (!string.IsNullOrWhiteSpace(boxSetting.CategoryId1)
                    && !MvcDbContext.Current.ItemSkus.Where(x => x.ShipperId == Common.Profile.User.ShipperId
                                                           && x.ItemId == boxSetting.ItemId
                                                           && x.CategoryId1 == boxSetting.CategoryId1).Any())
                {
                    this.ModelState.AddModelError("ItemId", string.Format(MessagesResource.NotSameError, BoxSettingResource.ItemId, BoxSettingResource.CategoryId1));
                }

                // 分類2
                if (!string.IsNullOrWhiteSpace(boxSetting.CategoryId1)
                    && !string.IsNullOrWhiteSpace(boxSetting.CategoryId2)
                    && !MvcDbContext.Current.ItemSkus.Where(x => x.ShipperId == Common.Profile.User.ShipperId
                                                             && x.ItemId == boxSetting.ItemId
                                                             && x.CategoryId1 == boxSetting.CategoryId1
                                                             && x.CategoryId2 == boxSetting.CategoryId2).Any())
                {
                    this.ModelState.AddModelError("ItemId", string.Format(MessagesResource.NotSameError, BoxSettingResource.ItemId, BoxSettingResource.CategoryId2));
                }

                // 分類3
                if (!string.IsNullOrWhiteSpace(boxSetting.CategoryId1)
                    && !string.IsNullOrWhiteSpace(boxSetting.CategoryId2)
                    && !string.IsNullOrWhiteSpace(boxSetting.CategoryId3)
                    && !MvcDbContext.Current.ItemSkus.Where(x => x.ShipperId == Common.Profile.User.ShipperId
                                                             && x.ItemId == boxSetting.ItemId
                                                             && x.CategoryId1 == boxSetting.CategoryId1
                                                             && x.CategoryId2 == boxSetting.CategoryId2
                                                             && x.CategoryId3 == boxSetting.CategoryId3).Any())
                {
                    this.ModelState.AddModelError("ItemId", string.Format(MessagesResource.NotSameError, BoxSettingResource.ItemId, BoxSettingResource.CategoryId3));
                }

                // 分類4
                if (!string.IsNullOrWhiteSpace(boxSetting.CategoryId1)
                    && !string.IsNullOrWhiteSpace(boxSetting.CategoryId2)
                    && !string.IsNullOrWhiteSpace(boxSetting.CategoryId3)
                    && !string.IsNullOrWhiteSpace(boxSetting.CategoryId4)
                    && !MvcDbContext.Current.ItemSkus.Where(x => x.ShipperId == Common.Profile.User.ShipperId
                                                             && x.ItemId == boxSetting.ItemId
                                                             && x.CategoryId1 == boxSetting.CategoryId1
                                                             && x.CategoryId2 == boxSetting.CategoryId2
                                                             && x.CategoryId3 == boxSetting.CategoryId3
                                                             && x.CategoryId4 == boxSetting.CategoryId4).Any())
                {
                    this.ModelState.AddModelError("ItemId", string.Format(MessagesResource.NotSameError, BoxSettingResource.ItemId, BoxSettingResource.CategoryId4));
                }
            }

            // 閾値区分は必須項目です
            if (boxSetting.ThresholdClass == Common.ThresholdClasses.None)
            {
                this.ModelState.AddModelError("ThresholdClass", string.Format(MessagesResource.Required, BoxSettingResource.ThresholdClass));
            }

            // 率(%)は必須項目です
            if (boxSetting.ThresholdClass == Common.ThresholdClasses.ThresholdRate && boxSetting.ThresholdRate == null)
            {
                this.ModelState.AddModelError("ThresholdRate", string.Format(MessagesResource.Required, BoxSettingResource.ThresholdRate));
            }

            // 数量は必須項目です
            if (boxSetting.ThresholdClass == Common.ThresholdClasses.ThresholdSku && boxSetting.ThresholdSku == null)
            {
                this.ModelState.AddModelError("ThresholdSku", string.Format(MessagesResource.Required, BoxSettingResource.ThresholdSku));
            }

            if (ModelState.RemoveBase().IsValid)
            {
                if (!string.IsNullOrEmpty(boxSetting.ItemId))
                {
                    var item = MvcDbContext.Current.ItemSkus.Where(x => x.ItemId == boxSetting.ItemId && x.ShipperId == Common.Profile.User.ShipperId).FirstOrDefault();
                    if (item != null)
                    {
                        // 荷主ID＋所在ID＋分類1～4＋商品ID　の単位　で重複不可
                        // 品番が入力されている場合は、分類は登録しない
                        var boxSettingExisted = MvcDbContext.Current.BoxSettings.Where(x => x.ShipperId == Common.Profile.User.ShipperId && (x.CategoryId1 == null) && (x.CategoryId2 == null) && (x.CategoryId3 == null) && (x.CategoryId4 == null) && (x.ItemId == boxSetting.ItemId)).FirstOrDefault();
                        if (boxSettingExisted != null)
                        {
                            TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                            return View("~/Areas/Master/Views/BoxSetting/Edit.cshtml", boxSetting);
                        }

                        if (_BoxSettingQuery.Create(boxSetting))
                        {
                            TempData[AppConst.SUCCESS] = MessagesResource.SUC_INSERT;
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    // 荷主ID＋所在ID＋分類1～4＋商品ID　の単位　で重複不可
                    var boxSettingExisted = MvcDbContext.Current.BoxSettings.Where(x => x.ShipperId == Common.Profile.User.ShipperId && (x.CategoryId1 == boxSetting.CategoryId1) && (x.CategoryId2 == boxSetting.CategoryId2) && (x.CategoryId3 == boxSetting.CategoryId3) && (x.CategoryId4 == boxSetting.CategoryId4) && (x.ItemId == boxSetting.ItemId)).FirstOrDefault();
                    if (boxSettingExisted != null)
                    {
                        TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                        return View("~/Areas/Master/Views/BoxSetting/Edit.cshtml", boxSetting);
                    }

                    if (_BoxSettingQuery.Create(boxSetting))
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

            return View("~/Areas/Master/Views/BoxSetting/Edit.cshtml", boxSetting);
        }
        #endregion

        #region Delete

        /// <summary>
        /// Delete rows
        /// </summary>
        /// <param name="countries">List record to delete</param>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string settingsid)
        {
            List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(settingsid);
            var isSuccess = _BoxSettingQuery.Delete(list);

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
        #endregion

        #region Edit

        /// <summary>
        /// Edit BoxSetting Information
        /// </summary>
        /// <param name="countries">List BoxSetting</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(string settingsid)
        {
            // Get record from DB
            var target = _BoxSettingQuery.GetTargetById(int.Parse(settingsid), Common.Profile.User.ShipperId);

            // 更新対象のデータがマスタに無い場合、エラー
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            target.SearchFlag = true;
            ViewBag.Category1List = _BoxSettingQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BoxSettingQuery.GetSelectListCategorys2(target.CategoryId1);
            ViewBag.Category3List = _BoxSettingQuery.GetSelectListCategorys3(target.CategoryId1, target.CategoryId2);
            ViewBag.Category4List = _BoxSettingQuery.GetSelectListCategorys4(target.CategoryId1, target.CategoryId2, target.CategoryId3);

            return View("~/Areas/Master/Views/BoxSetting/Edit.cshtml", target);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(BoxSettingList boxSetting)
        {
            // 閾値区分は必須項目です
            if (boxSetting.ThresholdClass == Common.ThresholdClasses.None)
            {
                this.ModelState.AddModelError("ThresholdClass", string.Format(MessagesResource.Required, BoxSettingResource.ThresholdClass));
            }

            // 率(%)は必須項目です
            if (boxSetting.ThresholdClass == Common.ThresholdClasses.ThresholdRate && boxSetting.ThresholdRate == null)
            {
                this.ModelState.AddModelError("ThresholdRate", string.Format(MessagesResource.Required, BoxSettingResource.ThresholdRate));
            }

            // 数量は必須項目です
            if (boxSetting.ThresholdClass == Common.ThresholdClasses.ThresholdSku && boxSetting.ThresholdSku == null)
            {
                this.ModelState.AddModelError("ThresholdSku", string.Format(MessagesResource.Required, BoxSettingResource.ThresholdSku));
            }

            if (ModelState.IsValid)
            {
                if (_BoxSettingQuery.UpdateBoxSetting(boxSetting))
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

            return View("~/Areas/Master/Views/BoxSetting/Edit.cshtml", boxSetting);
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private BoxSettingSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new BoxSettingSearchCondition() : Request.Cookies.Get<BoxSettingSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new BoxSettingSearchCondition();
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
        private ActionResult GetSearchResultView(BoxSettingSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                BoxSettingResult = indexFlag ? new BoxSettingResult() : new BoxSettingResult()
                {
                    BoxSettings = _BoxSettingQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.BoxSettingResult.BoxSettings.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.BoxSettingResult.BoxSettings = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
            ViewBag.Category1List = _BoxSettingQuery.GetSelectListCategorys1();
            ViewBag.Category2List = _BoxSettingQuery.GetSelectListCategorys2(vm.SearchConditions.CategoryId1);
            ViewBag.Category3List = _BoxSettingQuery.GetSelectListCategorys3(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2);
            ViewBag.Category4List = _BoxSettingQuery.GetSelectListCategorys4(vm.SearchConditions.CategoryId1, vm.SearchConditions.CategoryId2, vm.SearchConditions.CategoryId3);

            // Return index view
            return this.View("~/Areas/Master/Views/BoxSetting/Index.cshtml", vm);
        }

        #endregion

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
            BoxSettingSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.BoxSettingReport report = new Reports.Export.BoxSettingReport(ReportTypes.Excel, searchCondition);
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
            var report = new Reports.Import.BoxSettingReport(ReportTypes.Excel, uploadFile, guid);

            // Excelのデータをワークテーブルに登録（このタイミングでワークIDを採番する）
            report.Import();
            var workId = report._seq;

            // エラーチェック
            var wBoxSettings = MvcDbContext.Current.MasBoxSettings.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId).ToList();
            var check = _BoxSettingReportQuery.UploadCheck(workId, wBoxSettings);

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
                if (MvcDbContext.Current.MasBoxSettings.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId && x.ErrMsg != null).Count() == 0)
                {
                    _BoxSettingReportQuery.MergeBoxSettings(workId);
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
                    BoxSettings = _BoxSettingReportQuery.GetReportErrList(vm.UploadConditions)
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
                    BoxSettings = _BoxSettingReportQuery.GetReportErrList(vm.UploadConditions)
                };
                CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_ERR_PAGE, vm.UploadConditions.Page);
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_WORK_ID, workId);
            return this.View("~/Areas/Master/Views/BoxSetting/Upload.cshtml", vm);
        }


        #endregion ロード処理
    }
}