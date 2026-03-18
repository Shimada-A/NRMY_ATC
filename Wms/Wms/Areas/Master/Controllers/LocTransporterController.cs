namespace Wms.Areas.Master.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Ajax.Utilities;
    using OfficeOpenXml.FormulaParsing.Utilities;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Query.LocTransporter;
    using Wms.Areas.Master.ViewModels.LocTransporter;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class LocTransporterController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "temp-P94-1.SearchConditions";
        private const string COOKIE_REPORT_WORK_ID = "WMASLocTransporter03.WorkId";
        private const string COOKIE_REPORT_ERR_PAGE = "WMASLocTransporter03.Page";

        private LocTransporter _LocTransporterQuery;
        private Query.LocTransporter.Report _ReportQuery;

        private string UrlIndex = "~/Areas/Master/Views/LocTransporter/Index.cshtml";
        private string UrlEdit = "~/Areas/Master/Views/LocTransporter/Edit.cshtml";

        /// <summary>
        /// Initializes a new instance of the <see cref="TransporterController"/> class.
        /// </summary>
        public LocTransporterController()
        {
            this._LocTransporterQuery = new LocTransporter();
            this._ReportQuery = new Query.LocTransporter.Report();
        }

        #endregion Constants

        #region Index, Search

        /// <summary>
        /// Search
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            ViewBag.AreaList = _LocTransporterQuery.GetAreaList(true);
            ViewBag.StoreClassList = _LocTransporterQuery.GetStoreClassList();
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        public ActionResult Search([Bind(Include = "CenterId,ShipToStoreId,ShipToStoreName,IsNewDate,ShipToStoreClass,TransporterId,SortKey,Sort,AreaItem,Page")] LocTransporterSearchCondition SearchConditions)
        {
            ViewBag.AreaList = _LocTransporterQuery.GetAreaList(true);
            ViewBag.StoreClassList = _LocTransporterQuery.GetStoreClassList();
            LocTransporterSearchCondition condition;
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

        #endregion Index, Search

        #region 新規登録

        /// <summary>
        /// 新規登録
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            var vm = new Create();
            vm.CenterId = Common.Profile.User.CenterId;
            vm.CenterName = new Warehouses().GetNameById();
            ViewBag.TransportersList = _LocTransporterQuery.GetTransportersList();
            ViewBag.SagawaClientList = _LocTransporterQuery.GetSagawaClientList(vm.CenterId);
            ViewBag.NaniwaControlList = _LocTransporterQuery.GetNaniwaControlList(vm.CenterId);
            ViewBag.WsConsignorList = _LocTransporterQuery.GetWsConsignorIdList(vm.CenterId);

            return this.View("~/Areas/Master/Views/LocTransporter/Create.cshtml", vm);
        }

        /// <summary>
        /// 追加画面
        /// </summary>
        /// <param name="LocTransporters"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(IList<SearchItem> LocTransporters)
        {
            IList<SearchItem> item = LocTransporters.Where(x => x.IsCheck == true).ToList<SearchItem>();
            var vm = new Create();
            vm.SearchConditions.GetRows = item;
            vm.CenterId = item.Select(m => m.CenterId).FirstOrDefault().ToString();
            vm.CenterName = item.Select(m => m.CenterName).FirstOrDefault().ToString();
            ViewBag.TransportersList = _LocTransporterQuery.GetTransportersList();
            ViewBag.SagawaClientList = _LocTransporterQuery.GetSagawaClientList(vm.CenterId);
            ViewBag.NaniwaControlList = _LocTransporterQuery.GetNaniwaControlList(vm.CenterId);
            ViewBag.WsConsignorList = _LocTransporterQuery.GetWsConsignorIdList(vm.CenterId);

            return this.View("~/Areas/Master/Views/LocTransporter/Create.cshtml", vm);
        }

        /// <summary>
        /// 保存する（新規登録画面、追加画面）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Insert(IList<LocTransporter> LocTransporters)
        {
            if (_LocTransporterQuery.LocTransporterAdd(LocTransporters))
            {
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_INSERT;
            }
            else
            {
                TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
            }

            return RedirectToAction("Index", "LocTransporter");
        }

        /// <summary>
        /// 処理対象のROWIDを取得
        /// </summary>
        /// <param name="ShipToStoreId"></param>
        /// <param name="StartDate"></param>
        /// <param name="CenterId"></param>
        /// <param name="RowId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTargetById(string ShipToStoreId, DateTime StartDate, string CenterId, string RowId)
        {
            bool IsRepeat = false;

            if (RowId == "")
            {
                SearchItem item = new SearchItem()
                {
                    ShipToStoreId = ShipToStoreId,
                    StartDate = StartDate,
                    CenterId = CenterId,
                    RowId = RowId
                };

                IsRepeat = _LocTransporterQuery.GetTargetById(item).Any();
            }

            JsonResult rs = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            rs.Data = new { success = IsRepeat };
            return rs;
        }

        /// <summary>
        /// 配送業者：浪速の場合　浪速仕分コード設定がない場合はエラー
        /// </summary>
        /// <param name="ShipToStoreId">出荷先ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckNaniwaSorting(string ShipToStoreId)
        {
            JsonResult rs = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            rs.Data = new { success = _LocTransporterQuery.CheckNaniwaSorting(ShipToStoreId) };

            return rs;
        }

        /// <summary>
        /// ROWIDを取得
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public JsonResult GetRowId(Index vm)
        {
            List<string> rowids = _LocTransporterQuery.GetRowId(vm.SearchConditions).Select(l => l.RowId).ToList();

            //JsonResult rs = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //rs.Data = rowids;

            return Json(rowids, JsonRequestBehavior.AllowGet);
        }

        #endregion 新規登録

        #region Update

        /// <summary>
        /// 詳細画面へ
        /// </summary>
        /// <param name="rowids"></param>
        /// <returns></returns>
        public ActionResult Edit(IList<SearchItem> LocTransporters, string rowids)
        {
            IList<SearchItem> item = LocTransporters.Where(x => x.IsCheck == true).ToList<SearchItem>();
            var vm = new Create();
            vm.SearchConditions.GetRows = item;
            vm.CenterId = item.Select(m => m.CenterId).FirstOrDefault().ToString();
            vm.CenterName = item.Select(m => m.CenterName).FirstOrDefault().ToString();
            ViewBag.TransportersList = _LocTransporterQuery.GetTransportersList();
            ViewBag.SagawaClientList = _LocTransporterQuery.GetSagawaClientList(vm.CenterId);
            ViewBag.NaniwaControlList = _LocTransporterQuery.GetNaniwaControlList(vm.CenterId);
            ViewBag.WsConsignorList = _LocTransporterQuery.GetWsConsignorIdList(vm.CenterId);

            return this.View(UrlEdit, vm);
        }

        /// <summary>
        /// 削除
        /// </summary>
        /// <param name="rowids"></param>
        /// <returns></returns>
        public ActionResult Delete(string rowids)
        {

            List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(rowids);

            //現在有効なデータ（出荷先単位で、適用開始日が当日以前の最新日）は更新不可
            if (_LocTransporterQuery.GetRows(list, true).Any())
            {
                TempData[AppConst.ERROR] = MessageResource.ERR_LOC_TRANSPORTER_DELETE;
            }
            else
            {
                var rows = _LocTransporterQuery.GetRows(list, false)
                    .Select(m => new LocTransporter
                    {
                        CenterId = m.CenterId,
                        ShipToStoreId = m.ShipToStoreId,
                        ShipToStoreClass = m.ShipToStoreClass,
                        StartDate = m.StartDate,
                        UpdateCount = m.UpdateCount,
                        LeadTimesSun = m.LeadTimesSun,
                        LeadTimesMon = m.LeadTimesMon,
                        LeadTimesTue = m.LeadTimesTue,
                        LeadTimesWed = m.LeadTimesWed,
                        LeadTimesThu = m.LeadTimesThu,
                        LeadTimesFri = m.LeadTimesFri,
                        LeadTimesSat = m.LeadTimesSat,
                        LeadTimesHol = m.LeadTimesHol,
                        TransporterId = m.TransporterId,
                        TransporterIdSun = m.TransporterIdSun,
                        TransporterIdMon = m.TransporterIdMon,
                        TransporterIdTue = m.TransporterIdTue,
                        TransporterIdWed = m.TransporterIdWed,
                        TransporterIdThu = m.TransporterIdThu,
                        TransporterIdFri = m.TransporterIdFri,
                        TransporterIdSat = m.TransporterIdSat,
                        TransporterIdHol = m.TransporterIdHol,
                    }).ToList<LocTransporter>();
                _LocTransporterQuery.LocTransporterDel((IList<LocTransporter>)rows);
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_DELETE;
            }
            var condition = this.GetPreviousSearchInfo(false);
            return this.GetSearchResultView(condition, false);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="country">Country Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(IList<SearchItem> LocTransporters)
        {
            if (ModelState.IsValid)
            {
                    foreach (var item in LocTransporters)
                    {
                        if (!_LocTransporterQuery.LocTransporterUpd(item))
                        {
                            TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                            return RedirectToAction("Index");
                        }
                    }
                }
            else
            {
                var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();
                foreach (var message in errorMessages)
                {
                    ModelState.AddModelError(string.Empty, message);
                }

                ViewBag.AreaList = _LocTransporterQuery.GetAreaList(true);
                ViewBag.StoreClassList = _LocTransporterQuery.GetStoreClassList();
                LocTransporterSearchCondition condition = this.GetPreviousSearchInfo(false);

                return this.GetSearchResultView(condition, false);
            }

            TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
            return RedirectToAction("Index");
        }

        #endregion Update

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
            var report = new Reports.Import.LocTransporterReport(ReportTypes.Excel, uploadFile, guid);

            // Excelのデータをワークテーブルに登録（このタイミングでワークIDを採番する）
            report.Import();
            var workId = report._seq;

            // エラーチェック
            var wLocTransporters = MvcDbContext.Current.MasLocTransporters.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId).ToList();
            var check = this._ReportQuery.UploadCheck(workId, wLocTransporters);

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
                if (!MvcDbContext.Current.MasLocTransporters.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == workId && x.ErrMsg != null).Any())
                {
                    _ReportQuery.MergeLocTransporters(workId);
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
                    LocTransporters = _ReportQuery.GetReportErrList(vm.UploadConditions)
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
                    LocTransporters = _ReportQuery.GetReportErrList(vm.UploadConditions)
                };
                CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_ERR_PAGE, vm.UploadConditions.Page);
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_REPORT_WORK_ID, workId);
            return this.View("~/Areas/Master/Views/LocTransporter/Upload.cshtml", vm);
        }

        /// <summary>
        /// Indexレポートダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Download()
        {
            LocTransporterSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.LocTransporterReport report = new Reports.Export.LocTransporterReport(ReportTypes.Excel, searchCondition);
            report.Export();

            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }


        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private LocTransporterSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            LocTransporterSearchCondition condition = indexFlag ? new LocTransporterSearchCondition() : Request.Cookies.Get<LocTransporterSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new LocTransporterSearchCondition();
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
        private ActionResult GetSearchResultView(LocTransporterSearchCondition condition, bool indexFlag)
        {
            ViewBag.StoreClassList = _LocTransporterQuery.GetStoreClassList();
            ViewBag.TransportersList = _LocTransporterQuery.GetTransportersList();
            ViewBag.AreaList = _LocTransporterQuery.GetAreaList(true);

            var vm = new Index
            {
                SearchConditions = condition,
                LocTransporterResult = indexFlag ? new LocTransporterResult() : new LocTransporterResult()
                {
                    LocTransporters = _LocTransporterQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.LocTransporterResult.LocTransporters.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.LocTransporterResult.LocTransporters = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View(UrlIndex, vm);
        }
    }

}