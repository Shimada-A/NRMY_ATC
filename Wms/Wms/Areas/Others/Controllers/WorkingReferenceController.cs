namespace Wms.Areas.Others.Controllers
{
    using OfficeOpenXml;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Wms.Areas.Others.Query.WorkingReference;
    using Wms.Areas.Others.Resources;
    using Wms.Areas.Others.ViewModels.WorkingReference;
    using Wms.Controllers;
    using Wms.Hubs;
    using Wms.Models;
    using Wms.Resources;

    public class WorkingReferenceController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_STK_WorkingReference.SearchConditions";

        private WorkingReferenceQuery _WorkingReferenceQuery;
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingReferenceController"/> class.
        /// </summary>
        public WorkingReferenceController()
        {
            this._WorkingReferenceQuery = new WorkingReferenceQuery();
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
            searchInfo.CenterId = Common.Profile.User.CenterId;
            var vm = new WorkingReferenceViewModel
            {
                SearchConditions = searchInfo,
                Results = new WorkingReferenceResult()
            };

            return this.View("~/Areas/Others/Views/WorkingReference/Index.cshtml", vm);
        }
        public ActionResult IndexSearch()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            var msg = string.Empty;
            if (TempData[AppConst.ERROR] != null)
            {
                msg = TempData[AppConst.ERROR].ToString();
            }
            return this.GetSearchResultView(searchInfo, msg, true);
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Search(WorkingReferenceSearchConditions SearchConditions)
        {
            bool flg = false;
            WorkingReferenceSearchConditions condition;
            if (SearchConditions.CenterId == null)
            {
                //ページング
                condition = this.GetPreviousSearchInfo(false);
                condition.Page = SearchConditions.Page;
            }
            else
            {
                flg = true;
                condition = SearchConditions;
                condition.PageSize = this.GetCurrentPageSize();
            }

            return this.GetSearchResultView(condition, "", flg);
        }

        #endregion Search

        /// <summary>
        /// 排他解除
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Delete(WorkingReferenceSearchConditions SearchConditions)
        {
            var retmessage = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _WorkingReferenceQuery.DeleteWorkingData(SearchConditions, out status, out retmessage);

            if (status == ProcedureStatus.Success)
            {
                //仕入入荷計上
                if (SearchConditions.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Shiire)
                {
                    TempData[AppConst.SUCCESS] = string.Format(WorkingReferenceResource.DeleteSucShiire + SearchConditions.InvoiceNo + ")");
                }
                //移動入荷検品
                else if (SearchConditions.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Ido)
                {
                    TempData[AppConst.SUCCESS] = string.Format(WorkingReferenceResource.DeleteSucIdo + SearchConditions.SlipNo + ")");
                }
                else{
                    TempData[AppConst.SUCCESS] = string.Format(WorkingReferenceResource.DeleteSucStock + SearchConditions.BoxNo + ")");
                }
            }
            else
            {
                TempData[AppConst.ERROR] = retmessage;
            }

            var vm = new WorkingReferenceViewModel
            {
                SearchConditions = SearchConditions,
                Results = new WorkingReferenceResult()
            };

            return this.View("~/Areas/Others/Views/WorkingReference/Index.cshtml", vm);
        }
        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private WorkingReferenceSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new WorkingReferenceSearchConditions() : Request.Cookies.Get<WorkingReferenceSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new WorkingReferenceSearchConditions();
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
        private ActionResult GetSearchResultView(WorkingReferenceSearchConditions condition, string pMsg, bool insertFlg)
        {
            //ワークデータ登録/取得
            if (insertFlg && condition.SearchType == Common.SearchTypes.Search)
            {
                //スマホ仕入入荷計上ワークデータ作成
                if (condition.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Shiire)
                {
                    _WorkingReferenceQuery.InsertCaseWk(condition);
                }
                //スマホ移動入荷検品ワークデータ作成
                else if (condition.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Ido)
                {
                    _WorkingReferenceQuery.InsertIdoWk(condition);
                }
                //スマホトータルピック中ワークデータ作成
                else if (condition.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Pic)
                {
                    _WorkingReferenceQuery.InsertPicWk(condition);
                }
                //スマホ店別仕分
                else if (condition.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Store)
                {
                    _WorkingReferenceQuery.InsertStoreWk(condition);
                }
                else
                {
                    _WorkingReferenceQuery.InsertStockWk(condition);
                }

            }
            if (condition.Seq == 0 && condition.CaseViewModels != null)
            {
                condition.Seq = condition.CaseViewModels.FirstOrDefault().Seq;
            }

            var vm = new WorkingReferenceViewModel();
            //スマホ仕入入荷計上ワークデータ取得
            if (condition.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Shiire)
            {
                vm = new WorkingReferenceViewModel
                {
                    SearchConditions = condition,
                    Results = new WorkingReferenceResult()
                    {
                        WorkingReferences = _WorkingReferenceQuery.GetShiireWkData(condition)
                    }
                };
            }
            //スマホ移動入荷検品ワークデータ取得
            else if (condition.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Ido)
            {
                 vm = new WorkingReferenceViewModel
                {
                    SearchConditions = condition,
                    Results = new WorkingReferenceResult()
                    {

                        WorkingReferences = _WorkingReferenceQuery.GetIdoWkData(condition)
                    }
                };
            }
            //スマホトータルピック中ワークデータ取得
            else if (condition.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Pic)
            {
                 vm = new WorkingReferenceViewModel
                {
                    SearchConditions = condition,
                    Results = new WorkingReferenceResult()
                    {
                        WorkingReferences = _WorkingReferenceQuery.GetPicWkData(condition)
                    }
                };

            }
            else if (condition.SearchKind == WorkingReferenceSearchConditions.SearchKinds.Store)
            {
                vm = new WorkingReferenceViewModel
                {
                    SearchConditions = condition,
                    Results = new WorkingReferenceResult()
                    {
                        WorkingReferences = _WorkingReferenceQuery.GetStoreWkData(condition)
                    }
                };
            }
            else
            {
                vm = new WorkingReferenceViewModel
                {
                    SearchConditions = condition,
                    Results = new WorkingReferenceResult()
                    {
                        WorkingReferences = _WorkingReferenceQuery.GetStockWkData(condition)
                    }
                };
            }

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.WorkingReferences.TotalItemCount > ProcNumLimit)
                {
                    if (pMsg.Length == 0)
                    {
                        pMsg = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    }
                    vm.Results.WorkingReferences = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
            TempData[AppConst.ERROR] = pMsg;
            // Return index view
            return this.View("~/Areas/Others/Views/WorkingReference/Index.cshtml", vm);
        }

        #endregion Private

    }
}