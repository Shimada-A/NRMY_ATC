namespace Wms.Areas.Inventory.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Inventory.Query.Reference;
    using Wms.Areas.Inventory.Resources;
    using Wms.Areas.Inventory.ViewModels.Reference;
    using Wms.Common;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class ReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS1 = "W-INV_Reference01.SearchConditions";
        private const string COOKIE_SEARCHCONDITIONS2 = "W-INV_Reference02.SearchConditions";

        private ReferenceQuery _ReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceController"/> class.
        /// </summary>
        public ReferenceController()
        {
            this._ReferenceQuery = new ReferenceQuery();
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
        public ActionResult IndexWithPreviousCondition()
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
        /// <param name="SearchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(Reference01SearchConditions SearchConditions)
        {
            ModelState.Clear();

            Reference01SearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        #endregion Search

        #region ロケ別

        /// <summary>
        /// ロケ別へ
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocDetailSearch(Reference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();

            // 画面選択行更新用
            _ReferenceQuery.UpdateInvReference01s(searchConditions.Reference01s);
            // 棚卸進捗照会から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS1, searchConditions);

            var Inventory = MvcDbContext.Current.InvReference01s
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == searchConditions.Seq && m.LineNo == searchConditions.LineNo).FirstOrDefault();
            var vm = new Reference02ViewModel();
            vm.SearchConditions = new Reference02SearchConditions()
            {
                CenterId = Inventory.CenterId,
                InventoryNo = Inventory.InventoryNo,
                InventoryStatusOld = searchConditions.InventoryStatusOld,
                PageSize = this.GetCurrentPageSize(),
            };

            _ReferenceQuery.InsertInvReference02(vm.SearchConditions);

            _ReferenceQuery.InsertInvReferenceLoc04(vm.SearchConditions);

            vm.Head = _ReferenceQuery.GetReference02Head(vm.SearchConditions);

            vm.Total = _ReferenceQuery.GetReference02Total(vm.SearchConditions);

            vm.Results = new Reference02Result()
            {
                Reference = _ReferenceQuery.GetLocListData(vm.SearchConditions)
            };
            ViewBag.GradeId = _ReferenceQuery.GetSelectListGrades();
            return this.View("~/Areas/Inventory/Views/Reference/LocDetail.cshtml", vm);
        }

        /// <summary>
        /// ロケ別明細情報検索処理
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocDetailsSearch(Reference02SearchConditions searchConditions)
        {
            this.ModelState.Clear();

            //searchConditions.InventoryNo = head.InventoryNo;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS2, searchConditions);

            searchConditions.PageSize = this.GetCurrentPageSize();

            var vm = new Reference02ViewModel();
            
            if (searchConditions.SearchType == Common.SearchTypes.Search)
            {
                _ReferenceQuery.InsertInvReference02(searchConditions);

                _ReferenceQuery.InsertInvReferenceLoc04(searchConditions);
            }

            vm.Head = _ReferenceQuery.GetReference02Head(searchConditions);

            vm.Total = _ReferenceQuery.GetReference02Total(searchConditions);

            vm.Results = new Reference02Result()
            {
                Reference = _ReferenceQuery.GetLocListData(searchConditions)
            };
            vm.SearchConditions = searchConditions;

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.Reference.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.Reference = null;
                }
            }

            vm.SearchConditions.Seq2 = searchConditions.Seq2;
            vm.SearchConditions.Seq4 = searchConditions.Seq4;
            vm.SearchConditions.Page = searchConditions.Page;
            ViewBag.GradeId = _ReferenceQuery.GetSelectListGrades();
            return this.View("~/Areas/Inventory/Views/Reference/LocDetail.cshtml", vm);
        }

        /// <summary>
        /// ロケ別クリア
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult LocDetailClear()
        {
            var condition = Request.Cookies.Get<Reference01SearchConditions>(COOKIE_SEARCHCONDITIONS1);

            var Inventory = MvcDbContext.Current.InvReference01s
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == condition.Seq && m.LineNo == condition.LineNo).FirstOrDefault();

            var vm = new Reference02ViewModel();
            vm.SearchConditions = new Reference02SearchConditions()
            {
                CenterId = Inventory.CenterId,
                InventoryNo = Inventory.InventoryNo,
                InventoryStatusOld = condition.InventoryStatusOld,
                PageSize = this.GetCurrentPageSize(),
            };

            _ReferenceQuery.InsertInvReference02(vm.SearchConditions);

            _ReferenceQuery.InsertInvReferenceLoc04(vm.SearchConditions);

            vm.Head = _ReferenceQuery.GetReference02Head(vm.SearchConditions);

            vm.Total = _ReferenceQuery.GetReference02Total(vm.SearchConditions);

            vm.Results = new Reference02Result()
            {
                Reference = _ReferenceQuery.GetLocListData(vm.SearchConditions)
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.Reference.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.Reference = null;
                }
            }

            ViewBag.GradeId = _ReferenceQuery.GetSelectListGrades();
            return this.View("~/Areas/Inventory/Views/Reference/LocDetail.cshtml", vm);
        }

        #endregion

        #region SKU明細

        /// <summary>
        /// SKU明細へ
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SkuDetailSearch(Reference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // 画面選択行更新用
            _ReferenceQuery.UpdateInvReference01s(searchConditions.Reference01s);
            // 棚卸進捗照会から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS1, searchConditions);

            var Inventory = MvcDbContext.Current.InvReference01s
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == searchConditions.Seq && m.LineNo == searchConditions.LineNo).FirstOrDefault();
            var vm = new Reference02ViewModel();
            vm.SearchConditions = new Reference02SearchConditions()
            {
                CenterId = Inventory.CenterId,
                InventoryNo = Inventory.InventoryNo,
                InventoryStatusOld = searchConditions.InventoryStatusOld,
                PageSize = this.GetCurrentPageSize(),
            };

            _ReferenceQuery.InsertInvReference03(vm.SearchConditions);

            _ReferenceQuery.InsertInvReferenceSku04(vm.SearchConditions);

            vm.Head = _ReferenceQuery.GetReference03Head(vm.SearchConditions);

            vm.Total = _ReferenceQuery.GetReference03Total(vm.SearchConditions);

            vm.Results = new Reference02Result()
            {
                Reference = _ReferenceQuery.GetSkuListData(vm.SearchConditions)
            };
            ViewBag.GradeId = _ReferenceQuery.GetSelectListGrades();
            return this.View("~/Areas/Inventory/Views/Reference/SkuDetail.cshtml", vm);
        }

        /// <summary>
        /// SKU明細情報検索処理
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SkuDetailsSearch(Reference02SearchConditions searchConditions)
        {
            this.ModelState.Clear();

            //searchConditions.InventoryNo = head.InventoryNo;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS2, searchConditions);

            searchConditions.PageSize = this.GetCurrentPageSize();

            var vm = new Reference02ViewModel();

            if (searchConditions.SearchType == Common.SearchTypes.Search)
            {
                _ReferenceQuery.InsertInvReference03(searchConditions);

                _ReferenceQuery.InsertInvReferenceSku04(searchConditions);
            }

            vm.Head = _ReferenceQuery.GetReference03Head(searchConditions);

            vm.Total = _ReferenceQuery.GetReference03Total(searchConditions);

            vm.Results = new Reference02Result()
            {
                Reference = _ReferenceQuery.GetSkuListData(searchConditions)
            };
            vm.SearchConditions = searchConditions;

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.Reference.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.Reference = null;
                }
            }

            vm.SearchConditions.Seq3 = searchConditions.Seq3;
            vm.SearchConditions.Seq4 = searchConditions.Seq4;
            vm.SearchConditions.Page = searchConditions.Page;
            ViewBag.GradeId = _ReferenceQuery.GetSelectListGrades();
            return this.View("~/Areas/Inventory/Views/Reference/SkuDetail.cshtml", vm);
        }

        /// <summary>
        /// SKU明細クリア
        /// </summary>
        public ActionResult SkuDetailClear()
        {
            var condition = Request.Cookies.Get<Reference01SearchConditions>(COOKIE_SEARCHCONDITIONS1);

            var Inventory = MvcDbContext.Current.InvReference01s
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == condition.Seq && m.LineNo == condition.LineNo).FirstOrDefault();

            var vm = new Reference02ViewModel();
            vm.SearchConditions = new Reference02SearchConditions()
            {
                CenterId = Inventory.CenterId,
                InventoryNo = Inventory.InventoryNo,
                InventoryStatusOld = condition.InventoryStatusOld,
                PageSize = this.GetCurrentPageSize(),
            };

            _ReferenceQuery.InsertInvReference03(vm.SearchConditions);

            _ReferenceQuery.InsertInvReferenceSku04(vm.SearchConditions);

            vm.Head = _ReferenceQuery.GetReference03Head(vm.SearchConditions);

            vm.Total = _ReferenceQuery.GetReference03Total(vm.SearchConditions);

            vm.Results = new Reference02Result()
            {
                Reference = _ReferenceQuery.GetSkuListData(vm.SearchConditions)
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.Reference.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.Reference = null;
                }
            }

            ViewBag.GradeId = _ReferenceQuery.GetSelectListGrades();
            return this.View("~/Areas/Inventory/Views/Reference/SkuDetail.cshtml", vm);
        }

        #endregion

        #region Selected

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllSelectedSearch(Reference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // 全選択
            _ReferenceQuery.InvReferenceAllChange(searchConditions, true);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new Reference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new Reference01Result()
                {
                    Reference = _ReferenceQuery.GetData(searchConditions)
                },
            };

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS1, searchConditions);
            ViewBag.InventoryNo = _ReferenceQuery.GetSelectListInventoryNo(searchConditions);

            // Return index view
            return this.View("~/Areas/Inventory/Views/Reference/Index.cshtml", vm);
        }

        /// <summary>
        /// AllSelected
        /// </summary>
        /// <returns>Index View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnSelectedSearch(Reference01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // 全解除
            _ReferenceQuery.InvReferenceAllChange(searchConditions, false);

            searchConditions.PageSize = this.GetCurrentPageSize();

            // 検索表示
            var vm = new Reference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new Reference01Result()
                {
                    Reference = _ReferenceQuery.GetData(searchConditions)
                },
            };

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS1, searchConditions);
            ViewBag.InventoryNo = _ReferenceQuery.GetSelectListInventoryNo(searchConditions);

            // Return index view
            return this.View("~/Areas/Inventory/Views/Reference/Index.cshtml", vm);
        }

        #endregion Selected

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private Reference01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new Reference01SearchConditions() : Request.Cookies.Get<Reference01SearchConditions>(COOKIE_SEARCHCONDITIONS1) ?? new Reference01SearchConditions();
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
        private ActionResult GetSearchResultView(Reference01SearchConditions searchConditions, bool indexFlag)
        {
            // 画面選択行更新用
            if (!indexFlag && searchConditions.SearchType == Common.SearchTypes.SortPage)
            {
                _ReferenceQuery.UpdateInvReference01s(searchConditions.Reference01s);
            }

            // 作成処理&検索表示
            var vm = new Reference01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new Reference01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _ReferenceQuery.InsertInvReference01(searchConditions) : true) ? new Reference01Result()
                {
                    Reference = _ReferenceQuery.GetData(searchConditions)
                }
                : new Reference01Result()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (!indexFlag && ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.Reference.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.Reference = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS1, searchConditions);
            ViewBag.InventoryNo = _ReferenceQuery.GetSelectListInventoryNo(searchConditions);

            // Return index view
            return this.View("~/Areas/Inventory/Views/Reference/Index.cshtml", vm);

        }

        #endregion Private

        #region 仮確定

        /// <summary>
        /// 仮確定
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <returns>Update</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvConfirm(Reference01SearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS1, searchConditions);
            // 画面選択行更新用
            _ReferenceQuery.UpdateInvReference01s(searchConditions.Reference01s);

            var reference = MvcDbContext.Current.InvReference01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == searchConditions.Seq && m.IsCheck)
                                  .SingleOrDefault();

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _ReferenceQuery.InventoryConfirm(reference.InventoryNo, searchConditions.CenterId,out status, out message);

            if (status == ProcedureStatus.NoAllocData)
            {
                ViewBag.Status = status;
                ViewBag.ErrorMessage = message;

                var vm = new Reference01ViewModel
                {
                    SearchConditions = searchConditions,
                    Results = new Reference01Result()
                    {
                        Reference = _ReferenceQuery.GetData(searchConditions)
                    },
                };

                ViewBag.InventoryNo = _ReferenceQuery.GetSelectListInventoryNo(searchConditions);

                // Return index view
                return this.View("~/Areas/Inventory/Views/Reference/Index.cshtml", vm);
            }
            else if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = string.Format(ReferenceResource.SUC_CONFORM);

                searchConditions.SearchType = SearchTypes.Search;
                // Return index view
                return RedirectToAction("IndexWithPreviousCondition");
            }
            else
            {
                TempData[AppConst.ERROR] = message;

                // Return index view
                return RedirectToAction("IndexSearch");
            }
        }

        /// <summary>
        /// もう一度仮確定
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <returns>Update</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvAgainConfirm(Reference01SearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();

            var reference = MvcDbContext.Current.InvReference01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == searchConditions.Seq && m.IsCheck)
                                  .SingleOrDefault();

            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _ReferenceQuery.InventoryAgainConfirm(reference.InventoryNo, searchConditions.CenterId, out status, out message);

            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = string.Format(ReferenceResource.SUC_CONFORM);

                // Return index view
                return IndexWithPreviousCondition();
            }
            else
            {
                TempData[AppConst.ERROR] = message;

                // Return index view
                return RedirectToAction("IndexSearch");
            }
        }

        #endregion 仮確定

        #region 棚卸取消

        /// <summary>
        /// 棚卸取消
        /// </summary>
        /// <param name="searchConditions"></param>
        /// <returns>Update</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvCancel(Reference01SearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS1, searchConditions);
            // 画面選択行更新用
            _ReferenceQuery.UpdateInvReference01s(searchConditions.Reference01s);

            var reference = MvcDbContext.Current.InvReference01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == searchConditions.Seq && m.IsCheck)
                                  .SingleOrDefault();

            // 棚卸取消
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _ReferenceQuery.InventoryCancel(reference.InventoryNo, searchConditions.CenterId, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = string.Format(ReferenceResource.SUC_CANCEL);

                // Return index view
                return IndexWithPreviousCondition();
            }
            else
            {
                TempData[AppConst.ERROR] = message;

                // Return index view
                return RedirectToAction("IndexSearch");
            }
        }

        #endregion 棚卸取消

        #region ロード処理

        /// <summary>
        /// ダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Download(Reference01SearchConditions searchConditions)
        {
            // 画面選択行更新用
            _ReferenceQuery.UpdateInvReference01s(searchConditions.Reference01s);

            Reports.Export.Reference01Report report = new Reports.Export.Reference01Report(ReportTypes.Excel, searchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// ロケ別ダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownloadLocDetail(Reference02SearchConditions searchConditions)
        {

            Reports.Export.Reference02Report report = new Reports.Export.Reference02Report(ReportTypes.Excel, searchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// SKU明細ダウンロード
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownloadSkuDetail(Reference02SearchConditions searchConditions)
        {

            Reports.Export.Reference03Report report = new Reports.Export.Reference03Report(ReportTypes.Excel, searchConditions);
            report.Export();
            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// 差異リスト
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Print(Reference01SearchConditions searchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            // 画面選択行更新用
            _ReferenceQuery.UpdateInvReference01s(searchConditions.Reference01s);

            // 画面選択行更新用
            _ReferenceQuery.UpdateInvReference(searchConditions.Seq, "Detail", searchConditions.CenterId );

            Reports.Export.Reference01ReportForCsv report = new Reports.Export.Reference01ReportForCsv(ReportTypes.Csv, searchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = "InvDifferenceList.wfr";
            return WfrPrint(styleName, report.DownloadFileName); ;
        }

        /// <summary>
        /// ロケ別差異リスト
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintLocDetail(Reference02SearchConditions searchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            // 画面選択行更新用
            _ReferenceQuery.UpdateInvReference(searchConditions.Seq2,"LocDetail", searchConditions.CenterId);

            Reports.Export.Reference02ReportForCsv report = new Reports.Export.Reference02ReportForCsv(ReportTypes.Csv, searchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = "InvDifferenceList.wfr";
            return WfrPrint(styleName,report.DownloadFileName);
        }

        /// <summary>
        /// SKU明細差異リスト
        /// </summary>
        /// <param name="SearchConditions"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrintSkuDetail(Reference02SearchConditions searchConditions)
        {

            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            // 画面選択行更新用
            _ReferenceQuery.UpdateInvReference(searchConditions.Seq3,"SkuDetail", searchConditions.CenterId);

            Reports.Export.Reference03ReportForCsv report = new Reports.Export.Reference03ReportForCsv(ReportTypes.Csv, searchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            // PDF作成
            string styleName = "InvDifferenceList.wfr";
            return WfrPrint(styleName, report.DownloadFileName); ;
        }

        #endregion ロード処理

        #region GetList

        /// <summary>
        /// Get コードより名称を取得
        /// <param name="kbn">マスタ区分</param>
        /// <param name="cd">検索用コード</param>
        /// </summary>
        /// <returns>検索名称</returns>
        public JsonResult GetInventoryNoList(string InventoryClass, string InventoryName, string InventoryDateFrom, string InventoryDateTo, string InventoryStatus, string Location, string Sku, string CenterId)
        {
            string _html = "<option value=''>" + Wms.Resources.CommonResource.None + "</option>";

            var listInventoryNo = _ReferenceQuery.GetSelectInventoryNoList(InventoryClass, InventoryName, InventoryDateFrom, InventoryDateTo, InventoryStatus, Location, Sku, CenterId);
            foreach (var inventoryNo in listInventoryNo)
            {
                _html = _html + "<option value='" + inventoryNo.Value + "'>" + inventoryNo.Text + "</option>";
            }

            return this.Json(new { html = _html });
        }

        #endregion

    }
}