namespace Wms.Areas.Returns.Controllers
{
    using System.Collections.Generic;
    using System;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Returns.Query.PurchaseCorrection;
    using Wms.Areas.Returns.Resources;
    using Wms.Areas.Returns.ViewModels.PurchaseCorrection;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;
    using Wms.Areas.Master.ViewModels.VendorReturnSearchModal;
    using Wms.Areas.Returns.ViewModels.JanSearchModal;

    public class PurchaseCorrectionController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_RET_PurchaseCorrection.SearchConditions";

        private PurchaseCorrectionQuery _PurchaseCorrectionQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBReferenceController"/> class.
        /// </summary>
        public PurchaseCorrectionController()
        {
            this._PurchaseCorrectionQuery = new PurchaseCorrectionQuery();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            PurchaseCorrectionViewModel vm = new PurchaseCorrectionViewModel();
            return this.View("~/Areas/Returns/Views/PurchaseCorrection/Index.cshtml", vm);
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PurchaseCorrectionInputViewModel PurchaseCorrectionInput, int isConfirmed)
        {
            PurchaseCorrectionInputViewModel vm = PurchaseCorrectionInput;
            return this.GetSearchResultView(vm,0, isConfirmed);
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PurchaseCorrectionInputViewModel PurchaseCorrectionInput, int isConfirmed)
        {
            PurchaseCorrectionInputViewModel vm = PurchaseCorrectionInput;
            return this.GetSearchResultView(vm,1, 0);
        }
        #endregion Search

        #region Create
        /// <summary>
        /// 実績登録
        /// </summary>
        /// <returns>List Record</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(PurchaseCorrectionInputViewModel PurchaseCorrectionInput)
        {
            var retmessage = string.Empty;
            var retReturnId = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            new PurchaseCorrectionQuery().UpdRetPurchaseCorrection(PurchaseCorrectionInput, out status, out retmessage, out retReturnId);

            if (status != ProcedureStatus.Success)
            {
                if(status == ProcedureStatus.Inventory)
                {
                    retmessage = string.Format(PurchaseCorrectionResource.ErrInv, retmessage);
                }
                TempData[AppConst.ERROR] = retmessage;
            }
            else
            {
                string controllerName = this.RouteData.Values["controller"].ToString();
                string ret = string.Empty;
                PurchaseCorrectionInput.SearchConditions.ReturnId = retReturnId;
                Reports.Export.PurchaseCorrection.PurchaseCorrectionReportForCsv report = new Reports.Export.PurchaseCorrection.PurchaseCorrectionReportForCsv(ReportTypes.Csv, PurchaseCorrectionInput.SearchConditions);
                report.Export();

                // CSV作成
                new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

                // PDF作成
                string styleName = "PurchaseCorrection.sty";
                //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);
                TempData[AppConst.SUCCESS] = PurchaseCorrectionResource.SUC_TEISEI;
                PurchaseCorrectionInput.SearchConditions.Ret = ret;
                PurchaseCorrectionInput.SearchConditions.Print = "Print";
                ModelState.Clear();
            }
            var vm = new PurchaseCorrectionViewModel
            {
                SearchConditions = PurchaseCorrectionInput.SearchConditions,
                Results = new PurchaseCorrectionResult()
            };
            vm.SearchConditions.Seq = 0;

            return this.View("~/Areas/Returns/Views/PurchaseCorrection/Index.cshtml", vm);
        }

        [HttpPost]
        public ActionResult ReportPrint(PurchaseCorrectionInputViewModel PurchaseCorrectionInput)
        {
            return this.File(PurchaseCorrectionInput.SearchConditions.Ret, "application/pdf");
        }
        #endregion

        #region Private

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(PurchaseCorrectionInputViewModel pVm,int kbn, int isConfirmed)
        {
            // 初期化
            var vm = new PurchaseCorrectionViewModel();
            ModelState.Clear();
            int status = 0;
            string message = "";

            var result = false;
            if (kbn == 0)
            {
                //ワークデータ登録/取得
                result = new PurchaseCorrectionQuery().InsertPurchaseCorrection(pVm.SearchConditions, isConfirmed, out status, out message);
            }
            else
            {
                //ワークデータ削除
                result = new PurchaseCorrectionQuery().DeletePurchaseCorrection(pVm.SearchConditions);
            }

            //ワークテーブルデータ登録処理に失敗した場合はエラー
            if (!result)
            {
                if (status == -103)
                {
                    ViewBag.Status = status;
                    ViewBag.ErrorMessage = message;
                    vm.Results.PurchaseCorrection = _PurchaseCorrectionQuery.GetPurchaseCorrection(pVm.SearchConditions);
                    if (vm.Results.PurchaseCorrection.Count == 0)
                    {
                        vm.Results.PurchaseCorrection = null;
                    }
                    vm.SearchConditions = pVm.SearchConditions;
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, pVm.SearchConditions);
                }
                else if (status == -101 || status == -102) {
                    TempData[AppConst.ERROR] = message;
                    vm.Results.PurchaseCorrection = _PurchaseCorrectionQuery.GetPurchaseCorrection(pVm.SearchConditions);
                    if (vm.Results.PurchaseCorrection.Count == 0)
                    {
                        vm.Results.PurchaseCorrection = null;
                    }
                    vm.SearchConditions.CenterId = pVm.SearchConditions.CenterId;
                    vm.SearchConditions.Seq = pVm.SearchConditions.Seq;
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, pVm.SearchConditions);
                }
                else
                {
                    TempData[AppConst.ERROR] = PurchaseCorrectionResource.ERR_SEARCH;
                    vm.Results.PurchaseCorrection = null;
                }
                // Return index view
                return this.View("~/Areas/Returns/Views/PurchaseCorrection/Index.cshtml", vm);
            }
            // 作成処理&検索表示
            vm = new PurchaseCorrectionViewModel
            {
                SearchConditions = new PurchaseCorrectionSearchConditions(),
                Results = new PurchaseCorrectionResult()
                {
                    PurchaseCorrection = _PurchaseCorrectionQuery.GetPurchaseCorrection(pVm.SearchConditions)
                }
            };
            vm.SearchConditions.CenterId = pVm.SearchConditions.CenterId;
            vm.SearchConditions.Seq = pVm.SearchConditions.Seq;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, pVm.SearchConditions);

            // Return index view
            return this.View("~/Areas/Returns/Views/PurchaseCorrection/Index.cshtml", vm);

        }
        #endregion Private

        #region 入力値チェック
        /// <summary>
        /// JANチェック
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJanCheck(string jan)
        {
            bool result = new PurchaseCorrectionQuery().ExistJan(jan);

            if (result)
            {
                return this.Json(1);
            }

            return this.Json(0);
        }
        [HttpPost]
        public ActionResult GetInvoiceNoCheck(string centerId, string jan, string invoiceNo)
        {
            bool result = new PurchaseCorrectionQuery().ExistInvoiceNo(centerId, jan, invoiceNo);

            if (result)
            {
                return this.Json(1);
            }

            return this.Json(0);
        }
        [HttpPost]
        public ActionResult GetLocationCdCheck(string centerId, string jan, string locationCd)
        {
            bool result = new PurchaseCorrectionQuery().ExistLocationCd(centerId, jan, locationCd);

            if (result)
            {
                int stockQty = new PurchaseCorrectionQuery().GetStockQty(jan, locationCd);
                result = new PurchaseCorrectionQuery().ChkCaseLocationCd(centerId,  locationCd);
                if (result)
                {
                    //バラ以外
                    return this.Json(new { kbn = 1, stockQty = stockQty });
                }
                else
                {
                    //バラ
                    return this.Json(new { kbn = 2, stockQty = stockQty });
                }
            }

            return this.Json(new { kbn = 0, stockQty = 0 });
        }
        [HttpPost]
        public ActionResult GetBoxNoCheck(string centerId, string jan, string locationCd, string boxNo, string invoiceNo)
        {
            bool result = new PurchaseCorrectionQuery().ExistBoxNo(centerId, jan, locationCd, boxNo, invoiceNo);

            if (result)
            {
                int stockQty = new PurchaseCorrectionQuery().GetCaseStockQty(centerId, jan, locationCd, boxNo, invoiceNo);
                return this.Json(new { kbn = 1, stockQty = stockQty });
            }

            return this.Json(new { kbn = 0, stockQty = 0 });
        }
        [HttpPost]
        public ActionResult GetBaraStockCheck(string centerId, string jan, string locationCd, string invoiceNo)
        {
            bool result = new PurchaseCorrectionQuery().ExistBaraStock(centerId, jan, locationCd, invoiceNo);

            if (result)
            {
                return this.Json(1);
            }

            return this.Json(0);
        }
        #endregion 入力値チェック

        #region Modal
        //JANモーダル
        public ActionResult JanModal(PurchaseCorrectionSearchConditions searchCondition)
        {
            var totalItemCount = 0;
            var janReturns = new PurchaseCorrectionQuery().ListingJan(searchCondition,ref totalItemCount);
            PurchaseCorrectionSearchConditions vm = new PurchaseCorrectionSearchConditions();
            vm.janViewModel = janReturns;
            vm.totalCnt = totalItemCount;

            return this.PartialView("~/Areas/Returns/Views/PurchaseCorrection/_JanSearchModal.cshtml", vm);
        }

        //納品書モーダル
        public ActionResult InvoiceModal(PurchaseCorrectionSearchConditions searchCondition)
        {
            var totalItemCount = 0;
            var invoiceReturns = new PurchaseCorrectionQuery().ListingInvoice(searchCondition, ref totalItemCount);
            PurchaseCorrectionSearchConditions vm = new PurchaseCorrectionSearchConditions();
            vm.invoiceViewModel = invoiceReturns;
            vm.totalCnt = totalItemCount;

            return this.PartialView("~/Areas/Returns/Views/PurchaseCorrection/_InvoiceSearchModal.cshtml", vm);
        }

        //ロケモーダル
        public ActionResult LocationModal(PurchaseCorrectionSearchConditions searchCondition)
        {
            var totalItemCount = 0;
            var locationReturns = new PurchaseCorrectionQuery().ListingLocation(searchCondition, ref totalItemCount);
            PurchaseCorrectionSearchConditions vm = new PurchaseCorrectionSearchConditions();
            vm.locationViewModel = locationReturns;
            vm.totalCnt = totalItemCount;

            return this.PartialView("~/Areas/Returns/Views/PurchaseCorrection/_LocationSearchModal.cshtml", vm);
        }

        //ケースモーダル
        public ActionResult CaseModal(PurchaseCorrectionSearchConditions searchCondition)
        {
            var totalItemCount = 0;
            var caseReturns = new PurchaseCorrectionQuery().ListingCase(searchCondition, ref totalItemCount);
            PurchaseCorrectionSearchConditions vm = new PurchaseCorrectionSearchConditions();
            vm.caseViewModel = caseReturns;
            vm.totalCnt = totalItemCount;

            return this.PartialView("~/Areas/Returns/Views/PurchaseCorrection/_CaseSearchModal.cshtml", vm);
        }
        #endregion Modal
    }
}