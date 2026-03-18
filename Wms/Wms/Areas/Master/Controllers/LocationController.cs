namespace Wms.Areas.Master.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Reports.Export;
    using Wms.Areas.Master.Resources;
    using Wms.Areas.Master.ViewModels.Location;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;

    public class LocationController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASLocation01.SearchConditions";
        private const string COOKIE_REPORT_WORK_ID = "WMASLocation03.WorkId";
        private const string COOKIE_REPORT_ERR_PAGE = "WMASLocation03c.Page";
        private Location _LocationQuery;
        private Query.Location.Report _LocationReportQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationController"/> class.
        /// </summary>
        public LocationController()
        {
            this._LocationQuery = new Location();
            this._LocationReportQuery = new Query.Location.Report();
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
        /// <param name="SearchConditions"</param>
        /// <returns>List Record</returns>
        public ActionResult Search(LocationSearchCondition SearchConditions)
        {
            LocationSearchCondition condition;

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

        public JsonResult GetPKeys(LocationSearchCondition SearchConditions)
        {
            List<string> ids = _LocationQuery.GetRowId(SearchConditions);
            return Json(ids, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Add

        /// <summary>
        /// 新規作成画面
        /// </summary>
        /// <returns>Create View</returns>
        public ActionResult Create(LocationSearchCondition SearchConditions)
        {
            var input = new Detail
            {
                ShipperId = Common.Profile.User.ShipperId,
                CenterId = SearchConditions.CenterId
            };
            input.SearchFlag = SearchConditions.SearchFlag;

            // 更新用セレクトボックス値を取得
            // ロケーション区分
            ViewBag.Class = _LocationQuery.GetSelectClassListItems();

            // 格付
            ViewBag.Grade = _LocationQuery.GetSelectGradeListItems();

            string LocationClass = _LocationQuery.GetSelectClassListItems().First<SelectListItem>().Value;
            string CaseClass = new Wms.Query.BaseQuery().GetName("CASECLASS", LocationClass, string.Empty);
            input.CaseClass = (Common.CaseClassEnum)Enum.Parse(typeof(Common.CaseClassEnum), CaseClass);

            return View("~/Areas/Master/Views/Location/Edit.cshtml", input);
        }

        /// <summary>
        /// 新規作成処理
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Create View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detail location)
        {
            //引当中チェック
            if (IsAllocProcessing(Common.Profile.User.CenterId))
                return RedirectToAction("Index");

            // ロケーション区分
            ViewBag.Class = _LocationQuery.GetSelectClassListItems();

            // 格付
            ViewBag.Grade = _LocationQuery.GetSelectGradeListItems();

            string LocationClass = string.Empty;
            string CaseClass = string.Empty;

            // ロケーション区分と格付のチェック
            if (!MvcDbContext.Current.LocationClassGrade.Where(x => x.ShipperId == location.ShipperId &&
                                                                    x.CenterId == location.CenterId &&
                                                                    x.LocationClass == location.LocationClass &&
                                                                    x.GradeId == location.GradeId).Any())
            {
                this.ModelState.AddModelError("GradeId", MessageResource.ERR_GRADE_NOT_EXIST);
            }

            if (ModelState.IsValid)
            {
                location.LocationCd = location.Locsec_1 + '-' + location.Locsec_2 + '-' + location.Locsec_3 + '-' + location.Locsec_4 + '-' + location.Locsec_5;
                var locationExisted = _LocationQuery.GetTargetById(location.LocationCd, location.CenterId, Common.Profile.User.ShipperId);
                if (locationExisted != null)
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                    LocationClass = _LocationQuery.GetSelectClassListItems().First<SelectListItem>().Value;
                    CaseClass = new Wms.Query.BaseQuery().GetName("CASECLASS", LocationClass, string.Empty);
                    return View("~/Areas/Master/Views/Location/Edit.cshtml", new Detail()
                    {
                        CaseClass = (Common.CaseClassEnum)Enum.Parse(typeof(Common.CaseClassEnum), CaseClass)
                    });
                }
                else
                {
                    if (_LocationQuery.Create(location))
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

            LocationClass = location.LocationClass;
            CaseClass = new Wms.Query.BaseQuery().GetName("CASECLASS", LocationClass, string.Empty);

            return View("~/Areas/Master/Views/Location/Edit.cshtml", new Detail()
            {
                CaseClass = (Common.CaseClassEnum)Enum.Parse(typeof(Common.CaseClassEnum), CaseClass)
            });
        }
        #endregion

        #region Edit

        /// <summary>
        /// Create Location Information
        /// </summary>
        /// <param name="SearchConditions"</param>
        /// <returns>Create View</returns>
        public ActionResult Edit(string rowid)
        {
            var input = _LocationQuery.GetTargetById(rowid);

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (input == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            input.SearchFlag = true;
            // 更新用セレクトボックス値を取得
            // ロケーション区分
            ViewBag.Class = _LocationQuery.GetSelectClassListItems();

            // 格付
            ViewBag.Grade = _LocationQuery.GetSelectGradeListItems();

            return View("~/Areas/Master/Views/Location/Edit.cshtml", input);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Detail location)
        {
            //在庫テーブルチェック 更新画面に遷移後、在庫テーブルに更新対象のロケーションのデータが追加された場合、エラー
            if (location.StockQty == null && _LocationQuery.CheckStock(location.ShipperId, location.CenterId, location.LocationCd))
            {
                TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_STOCK_EXIST, location.LocationCd);
                return RedirectToAction("IndexSearch");
            }

            //引当中チェック
            if (IsAllocProcessing(Common.Profile.User.CenterId))
                return RedirectToAction("IndexSearch");

            // ロケーション区分と格付のチェック
            if (!MvcDbContext.Current.LocationClassGrade.Where(x => x.ShipperId == location.ShipperId &&
                                                                    x.CenterId == location.CenterId &&
                                                                    x.LocationClass == location.LocationClass &&
                                                                    x.GradeId == location.GradeId).Any())
            {
                this.ModelState.AddModelError("GradeId", MessageResource.ERR_GRADE_NOT_EXIST);
            }

            // ロケーション区分
            ViewBag.Class = _LocationQuery.GetSelectClassListItems();
            // 格付
            ViewBag.Grade = _LocationQuery.GetSelectGradeListItems();

            if (ModelState.IsValid)
            {
                if (_LocationQuery.UpdateLocation(location))
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

            return View("~/Areas/Master/Views/Location/Edit.cshtml", location);
        }
        #endregion

        #region Delete

        /// <summary>
        /// Delete Location Information
        /// </summary>
        /// <param name="SearchConditions"</param>
        /// <returns>Delete View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string rowid)
        {
            //引当中チェック
            if (IsAllocProcessing(Common.Profile.User.CenterId))
                return RedirectToAction("IndexSearch");

            List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(rowid);

            string isSuccess = _LocationQuery.DeleteLocation(list);

            if (string.IsNullOrEmpty(isSuccess))
            {
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_DELETE;
            }
            else
            {
                if ("FALSE".Equals(isSuccess))
                {

                    TempData[AppConst.ERROR] = MessagesResource.MSG_ERR_EXCLUSIVE_DELETE;
                }
                else
                {

                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_STOCK_EXIST, isSuccess);
                }
            }

            return RedirectToAction("IndexSearch");
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
            LocationSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.LocationReport report = new Reports.Export.LocationReport(ReportTypes.Excel, searchCondition);
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
            LocationSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.LocationReportTemp report = new Reports.Export.LocationReportTemp(ReportTypes.Excel, searchCondition);
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

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, vm.SearchConditions);

            // ファイルの取込
            // var importFile = new FileInfo(importFilePath);
            var report = new Reports.Import.LocationReport(ReportTypes.Excel, uploadFile, guid);

            // Excelのデータをワークテーブルに登録（このタイミングでワークIDを採番する）
            report.Import();
            var workId = report._seq;

            // エラーチェック
            var wLocations = MvcDbContext.Current.MasLocations.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId).ToList();
            var check = _LocationReportQuery.UploadCheck(workId, wLocations, vm.SearchConditions.CenterId);

            // 登録
            if (!check)
            {
                this.TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPLOAD;
                return this.Json(vm);
            }
            else
            {
                CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_WORK_ID, workId);

                //引当中チェック
                var isAllocProcessing = IsAllocProcessing(Common.Profile.User.CenterId);

                // エラーが無い場合は、ワークテーブルの内容をもとに、マスタにマージ
                if (MvcDbContext.Current.MasLocations.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId && x.ErrMsg != null).Count() == 0)
                {
                    // 引当中の場合、マージしない
                    if (isAllocProcessing)
                    {
                        return this.Json(vm);
                    }
                    _LocationReportQuery.MergeLocations(workId);
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
                    Locations = _LocationReportQuery.GetReportErrList(vm.UploadConditions)
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
                    Locations = _LocationReportQuery.GetReportErrList(vm.UploadConditions)
                };
                CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_ERR_PAGE, vm.UploadConditions.Page);
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_WORK_ID, workId);
            return this.View("~/Areas/Master/Views/Location/Upload.cshtml", vm);
        }

        #endregion ロード処理

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private LocationSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new LocationSearchCondition() : Request.Cookies.Get<LocationSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new LocationSearchCondition();
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
        private ActionResult GetSearchResultView(LocationSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                LocationResult = indexFlag ? new LocationResult() : new LocationResult()
                {
                    Locations = _LocationQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.LocationResult.Locations.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.LocationResult.Locations = null;
                }
            }

            // 検索用セレクトボックス値を取得
            ViewBag.Class = _LocationQuery.GetSelectClassListItems(true);

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Master/Views/Location/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetEditResultView(LocationSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                LocationResult = indexFlag ? new LocationResult() : new LocationResult()
                {
                    Locations = _LocationQuery.GetData(condition)
                }
            };

            // 更新用セレクトボックス値を取得
            // ロケーション区分
            ViewBag.Class = _LocationQuery.GetSelectClassListItems(true);

            // 格付
            ViewBag.Grade = _LocationQuery.GetSelectGradeListItems();

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Master/Views/Location/Edit.cshtml", vm);

            // return this.View("Index", vm);
        }

        /// <summary>
        /// Get コードより名称を取得
        /// <param name="kbn">マスタ区分</param>
        /// <param name="cd">検索用コード</param>
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult GetCaseClass(string kbn, string cd, string cd1)
        {
            var rtn = string.Empty;
            string tmpNm = string.Empty;

            if (!string.IsNullOrEmpty(cd))
            {
                tmpNm = new Wms.Query.BaseQuery().GetName(kbn, cd, cd1);
                Common.CaseClassEnum _name = (Common.CaseClassEnum)Enum.Parse(typeof(Common.CaseClassEnum), tmpNm);
                rtn = EnumHelper<Common.CaseClassEnum>.GetDisplayValue((Common.CaseClassEnum)_name);
            }

            return this.Json(new { name = rtn, val = tmpNm });
        }

        /// <summary>
        /// 引当中の場合、エラーメッセージを取得
        /// </summary>
        /// <param name="centerId">センターID</param>
        /// <returns></returns>
        private bool IsAllocProcessing(string centerId)
        {
            if (new BaseQuery().IsAllocProcessing(centerId, 0) == 1)
            {
                TempData[AppConst.ERROR] = MessageResource.ERR_ALLOC_DOING;
                return true;
            }
            return false;
        }

        #endregion

        #region プリント処理

        //ラベル印刷
        [HttpPost]
        public ActionResult PrintLabel()
        {
            LocationSearchCondition condition = this.GetPreviousSearchInfo(false);
            const string styleName = "LocationLabel.sty";
            string controllerName = RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            LocationlabelReportForCsv report = new LocationlabelReportForCsv(condition);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            return this.File(ret, "application/pdf");
        }

        //A4印刷
        [HttpPost]
        public ActionResult PrintA4()
        {
            LocationSearchCondition condition = this.GetPreviousSearchInfo(false);
            const string styleName = "LocationLabelA4_QR.sty";
            string controllerName = RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            LocationlabelReportForCsv report = new LocationlabelReportForCsv(condition);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            //return this.File(ret, "application/pdf");
            return WfrPrint("LocationLabel.wfr", report.DownloadFileName);
        }
        #endregion
    }
}