namespace Wms.Areas.Ship.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.EcCancelUploadQuery;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.EcCancelUpload;
    using Wms.Areas.Ship.ViewModels.TransferReference;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class EcCancelUploadController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W-SHP_EcCancelUpload01.SearchConditions";

        private EcCancelUploadQuery _EcCancelUploadQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="EcCancelUploadController"/> class.
        /// </summary>
        public EcCancelUploadController()
        {
            this._EcCancelUploadQuery = new EcCancelUploadQuery();
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
            var vm = new EcCancelUpload01ViewModel
            {
                SearchConditions = searchInfo,
                Results = new EcCancelUpload01Result()
            };

            return this.View("~/Areas/Ship/Views/EcCancelUpload/Index.cshtml", vm);
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
        public ActionResult Search(EcCancelUpload01SearchConditions SearchConditions)
        {
            EcCancelUpload01SearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }
        #endregion Search

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private EcCancelUpload01SearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new EcCancelUpload01SearchConditions() : Request.Cookies.Get<EcCancelUpload01SearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new EcCancelUpload01SearchConditions();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="searchConditions">Search Country Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(EcCancelUpload01SearchConditions searchConditions, bool indexFlag)
        {

            // 作成処理&検索表示
            var vm = new EcCancelUpload01ViewModel
            {
                SearchConditions = searchConditions,
                Results = indexFlag ? new EcCancelUpload01Result() : ((searchConditions.SearchType == Common.SearchTypes.Search ? _EcCancelUploadQuery.InsertShpEcCancelUpload01(searchConditions) : true) ? new EcCancelUpload01Result()
                {
                    EcCancelUpload01s = _EcCancelUploadQuery.EcCancelUpload01GetData(searchConditions)
                }
                : new EcCancelUpload01Result()),

                // Page = searchConditions.Page
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.EcCancelUpload01s.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.EcCancelUpload01s = null;
                }
            }

            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);

            // Return index view
            return this.View("~/Areas/Ship/Views/EcCancelUpload/Index.cshtml", vm);

        }

        #endregion Private

        #region 更新処理
        /// <summary>
        /// 出荷確定
        /// </summary>
        /// <param name="searchConditions">Country Information</param>
        /// <returns>Update</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShipConfirm(EcCancelUpload01SearchConditions searchConditions)
        {
            ModelState.Clear();
            searchConditions.PageSize = this.GetCurrentPageSize();
            if (searchConditions.HidCuClass == EcCancelUploadResource.Upload)
            {
                //更新
                searchConditions.Canup_Kbn = 1;
            }
            else
            {
                //キャンセル
                searchConditions.Canup_Kbn = 0;
            }


            // 実績更新
            var message = string.Empty;
            ProcedureStatus status = ProcedureStatus.Success;
            _EcCancelUploadQuery.ShipConfirm(searchConditions, out status, out message);
            if (status == ProcedureStatus.Success)
            {
                //更新
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
            }
            else
            {
                TempData[AppConst.ERROR] = message;
            }

            // 検索表示
            var vm = new EcCancelUpload01ViewModel
            {
                SearchConditions = searchConditions,
                Results = new EcCancelUpload01Result()
            };
            vm.SearchConditions.Seq = searchConditions.Seq;
            vm.SearchConditions.Page = searchConditions.Page;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            return this.View("~/Areas/Ship/Views/EcCancelUpload/Index.cshtml", vm);

        }

        #endregion

        #region 梱包明細情報
        /// <summary>
        /// 梱包明細情報
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult Detail(EcCancelUpload01SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            // BtoB出荷梱包進捗照会から
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
            var vm = new EcCancelUpload02ViewModel();
            vm.SearchConditions = new EcCancelUpload02SearchConditions()
            {
                CenterId = searchConditions.CenterId,
                Seq = searchConditions.Seq,
                LineNo = searchConditions.LineNo,
                ShipInstructId = searchConditions.HidShipInstructId
            };

            vm.DetailResults = new EcCancelUpload02Result()
            {
                EcCancelUpload02s = _EcCancelUploadQuery.GetDetailData(vm.SearchConditions)
            };
            vm.SearchConditions.ItemSkuSum = vm.DetailResults.EcCancelUpload02s.Select(x => x.ItemSkuId).Distinct().Count();
            vm.SearchConditions.BoxNoSum = vm.DetailResults.EcCancelUpload02s.Where(x => !string.IsNullOrWhiteSpace(x.BoxNo)).Select(x => x.BoxNo).Distinct().Count();
            vm.SearchConditions.ResultQtySum = vm.DetailResults.EcCancelUpload02s.Select(x => x.ResultQty).Sum();
            vm.SearchConditions.AllocQtySum = vm.DetailResults.EcCancelUpload02s.Select(x => x.AllocQty).Sum();
            return this.View("~/Areas/Ship/Views/EcCancelUpload/Detail.cshtml", vm);
        }

        /// <summary>
        /// 梱包明細情報
        /// </summary>
        /// <param name="searchConditions">List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult DetailSearch(EcCancelUpload02SearchConditions searchConditions)
        {
            this.ModelState.Clear();
            var vm = new EcCancelUpload02ViewModel();
            vm.SearchConditions = searchConditions;

            vm.DetailResults = new EcCancelUpload02Result()
            {
                EcCancelUpload02s = _EcCancelUploadQuery.GetDetailData(vm.SearchConditions)
            };
            vm.SearchConditions.ItemSkuSum = vm.DetailResults.EcCancelUpload02s.Select(x => x.ItemSkuId).Distinct().Count();
            vm.SearchConditions.BoxNoSum = vm.DetailResults.EcCancelUpload02s.Where(x => !string.IsNullOrWhiteSpace(x.BoxNo)).Select(x => x.BoxNo).Distinct().Count();
            vm.SearchConditions.ResultQtySum = vm.DetailResults.EcCancelUpload02s.Select(x => x.ResultQty).Sum();
            return this.View("~/Areas/Ship/Views/EcCancelUpload/Detail.cshtml", vm);
        }

        #endregion
    }
}