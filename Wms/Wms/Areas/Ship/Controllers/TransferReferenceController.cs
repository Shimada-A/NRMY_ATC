namespace Wms.Areas.Ship.Controllers
{
    using System;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.TransferReference;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.EcConfirmProgress;
    using Wms.Areas.Ship.ViewModels.TransferReference;
    using Wms.Controllers;
    using Wms.Resources;
    using Wms.Common;

    public class TransferReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_TransferReference.SearchConditions";
        private const string COOKIE_PICK_RESULTS     = "W_SHP_TransferReference.PickResultConditions";
        private const string COOKIE_PICKSORT_DETAIL  = "W_SHP_TransferReference.PickSortDetailConditions";

        private TransferReferenceQuery _TransferReferenceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipFrontageController"/> class.
        /// </summary>
        public TransferReferenceController()
        {
            this._TransferReferenceQuery = new TransferReferenceQuery();
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
        public ActionResult Search(TransferReferenceSearchConditions SearchConditions)
        {
            TransferReferenceSearchConditions condition;
            if (SearchConditions.SearchType == SearchTypes.SortPage)
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
        /// 検索処理
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EcSearch(EcConfirmProgress01SearchConditions searchConditions)
        //{
        //    TransferReferenceSearchConditions condition = new TransferReferenceSearchConditions()
        //    {
        //        CenterId = searchConditions.CenterId,
        //        ShipKind = searchConditions.ShipKinds.Ec

        //    };
        //    return this.GetSearchResultView(condition, false);
        //}

        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private TransferReferenceSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new TransferReferenceSearchConditions() : Request.Cookies.Get<TransferReferenceSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new TransferReferenceSearchConditions();

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(TransferReferenceSearchConditions condition, bool indexFlag)
        {
            // ダッシュボードから取得
            //string tcDcKbn = (string)this.TempData["TcDcKbn1"];
            Common.ShipKinds tcDcKbn = condition.ShipKind;
            //if (!string.IsNullOrEmpty(tcDcKbn))
            if (tcDcKbn != null)
            {
                switch (tcDcKbn)
                {
                    case Common.ShipKinds.Dc:
                        condition.ShipKind = Common.ShipKinds.Dc;
                        break;
                    default:
                        condition.ShipKind = Common.ShipKinds.Case;
                        break;
                }
            }
            // Save search info
            var vm = new TransferReferenceViewModel
            {
                SearchConditions = condition,
                DcResults = indexFlag ? new TransferReferenceDcResult()
                                      : (condition.ShipKind == Common.ShipKinds.Dc ? new TransferReferenceDcResult()
                                      {
                                          TransferReferenceDcs = _TransferReferenceQuery.GetDcData(condition)
                                      } : new TransferReferenceDcResult()),
                CaseResults = indexFlag ? new TransferReferenceCaseResult()
                                      : (condition.ShipKind == Common.ShipKinds.Case ? new TransferReferenceCaseResult()
                                      {
                                          TransferReferenceCases = _TransferReferenceQuery.GetCaseData(condition)
                                      } : new TransferReferenceCaseResult())
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if(condition.ShipKind == Common.ShipKinds.Dc)
                {
                    if (vm.DcResults.TransferReferenceDcs.Count > ProcNumLimit)
                    {
                        TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                        vm.DcResults.TransferReferenceDcs = null;
                    }
                }
                else if (condition.ShipKind == Common.ShipKinds.Case)
                {
                    if (vm.CaseResults.TransferReferenceCases.Count > ProcNumLimit)
                    {
                        TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                        vm.CaseResults.TransferReferenceCases = null;
                    }
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Ship/Views/TransferReference/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion Private

        #region Detail

        /// <summary>
        /// ピック実績へ
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult PickResult(Common.ShipKinds shipKind, string centerId, DateTime? shipPlanDate, string batchNo)
        {
            // ページサイズ取得
            int PageSize = this.GetCurrentPageSize();
            PickResultViewModel vm = new PickResultViewModel();
            vm.Head = new PickResultHead() { 
                ShipKind = shipKind,
                BatchNo = batchNo,
                ShipDateSearch = shipPlanDate,
                CenterId = centerId,
                PageSize = PageSize
            };

            CookieExtention.SetSearchConditonCookie(COOKIE_PICK_RESULTS, vm.Head);

            return this.PartialView("~/Areas/Ship/Views/TransferReference/PickResult.cshtml", vm);
        }

        /// <summary>
        /// ピック実績　明細取得
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPickResultDetails(PickResultHead SearchConditions, int Page = 1)
        {
            PickResultHead condition;
            // ページ遷移の場合はcookieからヘッダーの情報を取得する
            if (SearchConditions.BatchNo is null)
            {
                    condition = Request.Cookies.Get<PickResultHead>(COOKIE_PICK_RESULTS) ?? new PickResultHead();
            }else{
                condition = SearchConditions;
            }
            PickResultViewModel vm = _TransferReferenceQuery.GetPickResultData(condition.ShipKind, condition.CenterId, condition.ShipDateSearch, condition.BatchNo, Page, condition.PageSize);
            return this.PartialView("~/Areas/Ship/Views/TransferReference/_PickResultDetails.cshtml", vm);
        }

        /// <summary>
        /// ピック仕分詳細へ
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult PickSortDetail(Common.ShipKinds shipKind, string centerId, DateTime? shipPlanDate, string batchNo)
        {
　          // ページサイズを取得する
            int PageSize = this.GetCurrentPageSize();
            PickSortDetailViewModel vm = new PickSortDetailViewModel();
            vm.Head = new PickSortDetailHead()
            {
                ShipKind = shipKind,
                BatchNo = batchNo,
                ShipPlanDateSearch = shipPlanDate,
                CenterId = centerId,
                PageSize = PageSize
            };

            // 検索条件保存
            CookieExtention.SetSearchConditonCookie(COOKIE_PICKSORT_DETAIL, vm.Head);

            return this.PartialView("~/Areas/Ship/Views/TransferReference/PickSortDetail.cshtml", vm);
        }

        /// <summary>
        /// ピック仕分詳細へ 明細取得
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult GetPickSortDetail(PickSortDetailHead SearchConditions, int Page = 1)
        {
            PickSortDetailHead condition;
            if (SearchConditions.BatchNo is null)
            {
                condition = Request.Cookies.Get<PickSortDetailHead>(COOKIE_PICKSORT_DETAIL) ?? new PickSortDetailHead();
            }
            else
            {
                condition = SearchConditions;
            }
            PickSortDetailViewModel vm = _TransferReferenceQuery.GetPickSortDetailData(condition.ShipKind, condition.CenterId, condition.ShipPlanDateSearch, condition.BatchNo, Page, condition.PageSize);
            return this.PartialView("~/Areas/Ship/Views/TransferReference/_PickSortDetails.cshtml", vm);
        }

        /// <summary>
        /// ピック仕分詳細(店別仕分状況)へ
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult PickSortDetailStore(string centerId,  string batchNo,string itemSkuId)
        {
            PickSortDetailStoreViewModel vm = _TransferReferenceQuery.GetPickSortDetailStoreData(centerId, itemSkuId, batchNo);

            return this.PartialView("~/Areas/Ship/Views/TransferReference/PickSortDetailStore.cshtml", vm);
        }

        /// <summary>
        /// レーン仕分詳細へ
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult LaneSortDetail(Common.ShipKinds shipKind, string centerId, DateTime? shipPlanDate, string batchNo)
        {
            LaneSortDetailViewModel vm = _TransferReferenceQuery.GetLaneSortDetailData(shipKind, centerId, shipPlanDate, batchNo);

            return this.PartialView("~/Areas/Ship/Views/TransferReference/LaneSortDetail.cshtml", vm);
        }

        /// <summary>
        /// 店別仕分、摘取詳細へ
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult StoreSortOrderPicDetail(Common.ShipKinds ShipKind,string centerId, DateTime? shipPlanDate, string batchNo)
        {
            StoreSortOrderPicDetailViewModel vm = _TransferReferenceQuery.GetStoreSortOrderPicDetailData(ShipKind, centerId, shipPlanDate, batchNo);

            return this.PartialView("~/Areas/Ship/Views/TransferReference/StoreSortOrderPicDetail.cshtml", vm);
        }

        /// <summary>
        /// ECユニット仕分進捗へ
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult EcunitReference(string centerId, DateTime? shipPlanDate, string batchNo)
        {
            EcunitReferenceViewModel vm = _TransferReferenceQuery.GetEcunitReferenceData(centerId, batchNo);

            return this.PartialView("~/Areas/Ship/Views/TransferReference/EcunitReference.cshtml", vm);
        }

        /// <summary>
        /// 商品詳細へ
        /// </summary>
        /// <param name="searchInfo"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public ActionResult ItemDetail(Common.ShipKinds shipKind, string centerId, DateTime? shipPlanDate, string batchNo)
        {
            //PickSortDetailViewModel vm = _TransferReferenceQuery.GetItemDetailData(shipKind, centerId, shipPlanDate);

            //return this.PartialView("~/Areas/Ship/Views/TransferReference/PickSortDetail.cshtml", vm);
            return null;
        }

        #endregion

        /// <summary>
        /// PrintCaseLabelConditions Information
        /// </summary>
        /// <param name="SearchConditions">PrintCaseLabelConditions</param>
        public ActionResult DownloadPickInfo(TransferReferenceSearchConditions SearchConditions)
        {
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            string controllerName = this.RouteData.Values["controller"].ToString();
            string ret = string.Empty;

            // PC発行入荷ラベル発行
            Reports.Export.PickInfoCsv report = new Reports.Export.PickInfoCsv(ReportTypes.Csv, SearchConditions);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }
    }
}