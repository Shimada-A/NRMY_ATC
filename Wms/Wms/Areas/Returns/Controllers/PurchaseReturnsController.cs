namespace Wms.Areas.Returns.Controllers
{
    using Share.Common;
    using Share.Extensions.Classes;
    using System.Web.Mvc;
    using Wms.Areas.Master.ViewModels.VendorReturnSearchModal;
    using Wms.Areas.Returns.Query.PurchaseReturns;
    using Wms.Areas.Returns.Resources;
    using Wms.Areas.Returns.ViewModels.PurchaseReturns;
    using Wms.Controllers;
    using Wms.Resources;

    public class PurchaseReturnsController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_RET_PurchaseReturns.SearchConditions";

        private PurchaseReturnsQuery _PurchaseReturnsQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBReferenceController"/> class.
        /// </summary>
        public PurchaseReturnsController()
        {
            this._PurchaseReturnsQuery = new PurchaseReturnsQuery();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            PurchaseReturnsViewModel vm = new PurchaseReturnsViewModel();
            return this.View("~/Areas/Returns/Views/PurchaseReturns/Index.cshtml", vm);
        }

        /// <summary>
        /// 検索条件不備時
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult ReIndex(PurchaseReturnsSearchConditions SearchConditions)
        {
            PurchaseReturnsViewModel vm = new PurchaseReturnsViewModel();
            vm.SearchConditions = SearchConditions;
            return this.View("~/Areas/Returns/Views/PurchaseReturns/Index.cshtml", vm);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(PurchaseReturnsInputViewModel PurchaseReturnInput)
        {
            PurchaseReturnsInputViewModel vm = PurchaseReturnInput;
            vm.SearchConditions.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(vm, true,true);
        }
        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReSearch(PurchaseReturnsInputViewModel PurchaseReturnInput)
        {
            PurchaseReturnsInputViewModel vm = PurchaseReturnInput;
            vm.SearchConditions.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(vm, true,false);
        }
        #endregion Search

        #region Create
        /// <summary>
        /// 実績登録
        /// </summary>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseReturnsInputViewModel PurchaseReturnInput)
        {
            var UpdateResult = new PurchaseReturnsQuery().UpdatePurchaseReturns(PurchaseReturnInput);
            //ワークテーブルデータ登録処理に失敗した場合はエラー
            if (!UpdateResult)
            {
                TempData[AppConst.ERROR] = "データ検索処理に失敗しました。";
            }
            else
            {
                var retmessage = string.Empty;
                var retReturnId = string.Empty;
                ProcedureStatus status = ProcedureStatus.Success;
                new PurchaseReturnsQuery().UpdRetPurchaseReturn(PurchaseReturnInput, out status, out retmessage,out retReturnId);

                if (status == ProcedureStatus.Success)
                {

                    string controllerName = this.RouteData.Values["controller"].ToString();
                    string ret = string.Empty;
                    PurchaseReturnInput.SearchConditions.ReturnId = retReturnId;
                    Reports.Export.PurchaseReturns.PurchaseReturnsReportForCsv report = new Reports.Export.PurchaseReturns.PurchaseReturnsReportForCsv(ReportTypes.Csv, PurchaseReturnInput.SearchConditions);
                    report.Export();

                    // CSV作成
                    new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                    // PDF作成
                    string styleName = "PurchaseReturn.sty";
                    //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = MessageResource.NORMAL_END;

                    PurchaseReturnInput.SearchConditions.Ret = ret;
                    PurchaseReturnInput.SearchConditions.Print = "Print";
                    ModelState.Clear();
                }
                else
                {
                    if (status == ProcedureStatus.Inventory)
                    {
                        retmessage = string.Format(PurchaseCorrectionResource.ErrInv, retmessage);
                    }
                    TempData[AppConst.ERROR] = retmessage;
                }
            }
            var vm = new PurchaseReturnsViewModel
            {
                SearchConditions = PurchaseReturnInput.SearchConditions,
                Results = new PurchaseReturnsResult()
            };

            return this.View("~/Areas/Returns/Views/PurchaseReturns/Index.cshtml", vm);
        }

        [HttpPost]
        public ActionResult ReportPrint(PurchaseReturnsInputViewModel PurchaseReturnInput)
        {
            return this.File(PurchaseReturnInput.SearchConditions.Ret, "application/pdf");
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private PurchaseReturnsSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new PurchaseReturnsSearchConditions() : Request.Cookies.Get<PurchaseReturnsSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new PurchaseReturnsSearchConditions();
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
        private ActionResult GetSearchResultView(PurchaseReturnsInputViewModel pVm, bool indexFlag, bool insertFlg)
        {
            // 初期化
            var vm = new PurchaseReturnsViewModel();
            ModelState.Clear();
            //初回検索時は、検索対象の仕入先数をチェックする
            if (indexFlag)
            {
                //検索条件が入力されているときのみチェック
                if (!string.IsNullOrWhiteSpace(pVm.SearchConditions.VendorId))
                {
                    int vendorCount = new PurchaseReturnsQuery().CheckVendorCount(pVm.SearchConditions);
                    //複数仕入先が検索結果に含まれる場合はエラー
                    if (vendorCount > 1)
                    {
                        vm = new PurchaseReturnsViewModel();
                        vm.SearchConditions = pVm.SearchConditions;
                        vm.SearchConditions.VendorCount = vendorCount;
                        TempData["VENDOR_CHECK"] = "仕入先が決定できません";
                        return RedirectToAction("ReIndex", pVm.SearchConditions);

                    }
                }
            }

            //ワークデータ登録/取得
            if (insertFlg)
            {
                var InsertResult = new PurchaseReturnsQuery().InsertPurchaseReturns(pVm.SearchConditions);
                //ワークテーブルデータ登録処理に失敗した場合はエラー
                if (!InsertResult)
                {
                    TempData[AppConst.ERROR] = "データ検索処理に失敗しました。";
                    vm.Results.PurchaseReturns = null;
                    // Return index view
                    return this.View("~/Areas/Returns/Views/PurchaseReturns/Index.cshtml", vm);
                }
            }
            else
            {
                var UpdateResult = new PurchaseReturnsQuery().UpdatePurchaseReturns(pVm);
                //ワークテーブルデータ登録処理に失敗した場合はエラー
                if (!UpdateResult)
                {
                    TempData[AppConst.ERROR] = "データ検索処理に失敗しました。";
                    vm.Results.PurchaseReturns = null;
                    // Return index view
                    return this.View("~/Areas/Returns/Views/PurchaseReturns/Index.cshtml", vm);
                }
            }
            // 作成処理&検索表示
            vm = new PurchaseReturnsViewModel
            {
                SearchConditions = pVm.SearchConditions,
                Results = new PurchaseReturnsResult()
                {
                    PurchaseReturns = _PurchaseReturnsQuery.GetPurchaseReturns(pVm.SearchConditions)
                },
                Page = 1
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.PurchaseReturns.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.PurchaseReturns = null;
                }
            }

            vm.SearchConditions.Seq = pVm.SearchConditions.Seq;
            vm.SearchConditions.Page = pVm.SearchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, pVm.SearchConditions);

            // Return index view
            return this.View("~/Areas/Returns/Views/PurchaseReturns/Index.cshtml", vm);

        }
        #endregion Private

        public ActionResult VenderReturnModal(string centerId)
        {
            var totalItemCount = 0;
            var vendorReturns = new VendorReturnViewModel().Listing(centerId,ref totalItemCount);
            Master.ViewModels.VendorReturnSearchModal.VendorReturnSearchCondition vm = new Master.ViewModels.VendorReturnSearchModal.VendorReturnSearchCondition();
            vm.vendorReturnViewModel = vendorReturns;
            vm.totalCnt = totalItemCount;

            return this.PartialView("~/Areas/Master/Views/Shared/_VendorReturnSearchModal.cshtml", vm);
        }

        public ActionResult InvoiceModal(long seq, string centerId, string itemSkuId)
        {
            var invoiceReturns = new PurchaseReturnsQuery().ListingInvoice(seq, centerId, itemSkuId);

            return PartialView("~/Areas/Returns/Views/PurchaseReturns/_InvoiceSearchModal.cshtml", invoiceReturns);
        }
    }
}