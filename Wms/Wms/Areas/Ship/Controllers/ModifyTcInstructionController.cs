namespace Wms.Areas.Ship.Controllers
{
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Ship.Query.ModifyTcInstruction;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.ModifyTcInstruction;
    using Wms.Common;
    using Wms.Controllers;
    using Wms.Resources;

    public class ModifyTcInstructionController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "W_SHP_ModifyTcInstruction.SearchConditions";

        private ModifyTcInstructionQuery _ModifyTcInstructionQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyTcInstructionController"/> class.
        /// </summary>
        public ModifyTcInstructionController()
        {
            this._ModifyTcInstructionQuery = new ModifyTcInstructionQuery();
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
        public ActionResult Search(ModifyTcInstructionSearchConditions SearchConditions)
        {
            ModifyTcInstructionSearchConditions condition = SearchConditions;
            condition.PageSize = this.GetCurrentPageSize();
            return this.GetSearchResultView(condition, false);
        }

        /// <summary>
        /// 一括設定表示
        /// </summary>
        /// <param name="searchCondition"List Country Information</param>
        /// <returns>List Record</returns>
        public ActionResult AllSetSearch(ModifyTcInstructionSearchConditions SearchConditions)
        {
            ModelState.Clear();
            var vm = new ModifyTcInstructionViewModel();
            vm.SearchConditions = SearchConditions;
            vm.SearchConditions.PageSize = this.GetCurrentPageSize();

            // 一括設定
            _ModifyTcInstructionQuery.UpdateAllShpModTcInstruction(SearchConditions); 

            vm.Results = new ModifyTcInstructionResult()
            {
                ModifyTcInstructions = _ModifyTcInstructionQuery.GetData(SearchConditions)
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.Results.ModifyTcInstructions.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.Results.ModifyTcInstructions = null;
                }
            }
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, SearchConditions);
            return this.View("~/Areas/Ship/Views/ModifyTcInstruction/Index.cshtml", vm);
        }

        #endregion Search

        #region 更新処理

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="searchConditions">searchConditions</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSearch(ModifyTcInstructionSearchConditions searchConditions)
        {
            ModelState.Clear();
            var vm = new ModifyTcInstructionViewModel();
            vm.SearchConditions = searchConditions;
            vm.SearchConditions.PageSize = this.GetCurrentPageSize();
            _ModifyTcInstructionQuery.UpdateShpModTcInstruction(searchConditions.ModifyTcInstructions);
            //最小出荷指示数以下で設定されているデータがあった場合
            var message = searchConditions.ErrFlag ? "" : _ModifyTcInstructionQuery.CheckMinInstructQty(searchConditions);
            if (string.IsNullOrWhiteSpace(message))
            {
                ProcedureStatus status = ProcedureStatus.Success;
                // 実績更新
                _ModifyTcInstructionQuery.ModifyTcInstruction(searchConditions, out status, out message);
                if (status == ProcedureStatus.Success)
                {
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = message;
                    // 検索部を表示
                    return RedirectToAction("UpdateAfter");
                }
                else
                {
                    ViewBag.Status = status;
                    ViewBag.Message = message;
                    // 検索
                    vm.SearchConditions.ErrFlag = false;
                    vm.Results = new ModifyTcInstructionResult()
                    {
                        ModifyTcInstructions = _ModifyTcInstructionQuery.GetData(searchConditions)
                    };

                    var ProcNumLimit = this.GetCurrentProcNumLimit();
                    if (ProcNumLimit != 0 && vm != null)
                    {
                        if (vm.Results.ModifyTcInstructions.TotalItemCount > ProcNumLimit)
                        {
                            TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                            vm.Results.ModifyTcInstructions = null;
                        }
                    }
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                    return this.View("~/Areas/Ship/Views/ModifyTcInstruction/Index.cshtml", vm);
                }
            }
            else
            {
                ViewBag.Message = message;
                vm.Results = new ModifyTcInstructionResult()
                {
                    ModifyTcInstructions = _ModifyTcInstructionQuery.GetData(searchConditions)
                };

                var ProcNumLimit = this.GetCurrentProcNumLimit();
                if (ProcNumLimit != 0 && vm != null)
                {
                    if (vm.Results.ModifyTcInstructions.TotalItemCount > ProcNumLimit)
                    {
                        TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                        vm.Results.ModifyTcInstructions = null;
                    }
                }
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, searchConditions);
                return this.View("~/Areas/Ship/Views/ModifyTcInstruction/Index.cshtml", vm);
            }
        }

        /// <summary>
        /// Search Country
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult UpdateAfter()
        {
            var searchInfo = this.GetPreviousSearchInfo(false);
            return this.GetSearchResultView(searchInfo, true);
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>CountrySearchCondition</returns>
        private ModifyTcInstructionSearchConditions GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new ModifyTcInstructionSearchConditions() : Request.Cookies.Get<ModifyTcInstructionSearchConditions>(COOKIE_SEARCHCONDITIONS) ?? new ModifyTcInstructionSearchConditions();
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
        private ActionResult GetSearchResultView(ModifyTcInstructionSearchConditions condition, bool indexFlag)
        {
            ModelState.Clear();
            if (!indexFlag && condition.SearchType == SearchTypes.SortPage)
            {
                _ModifyTcInstructionQuery.UpdateShpModTcInstruction(condition.ModifyTcInstructions);
            }
            // Save search info
            var vm = new ModifyTcInstructionViewModel();

            if (indexFlag)
            {
                vm.SearchConditions = condition;
                vm.Results = new ModifyTcInstructionResult();
                CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
                return this.View("~/Areas/Ship/Views/ModifyTcInstruction/Index.cshtml", vm);
            }
            else
            {
                var message = condition.SearchType == SearchTypes.Search ? _ModifyTcInstructionQuery.MakeData(condition) : string.Empty;
                if (string.IsNullOrWhiteSpace(message))
                {
                    vm.SearchConditions = condition;
                    vm.Results = new ModifyTcInstructionResult()
                    {
                        ModifyTcInstructions = _ModifyTcInstructionQuery.GetData(condition)
                    };

                    var ProcNumLimit = this.GetCurrentProcNumLimit();
                    if (ProcNumLimit != 0 && vm != null)
                    {
                        if (vm.Results.ModifyTcInstructions.TotalItemCount > ProcNumLimit)
                        {
                            TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                            vm.Results.ModifyTcInstructions = null;
                        }
                    }
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
                    return this.View("~/Areas/Ship/Views/ModifyTcInstruction/Index.cshtml", vm);
                }
                else
                {
                    if (message == MessagesResource.MSG_NOT_FOUND)
                    {
                        TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                    }
                    else if (message == ModifyTcInstructionResource.ERR_MULTIPLE_INVOICE)
                    {
                        TempData[AppConst.ERROR] = ModifyTcInstructionResource.ERR_MULTIPLE_INVOICE;
                    }
                    else if (message == ModifyTcInstructionResource.ERR_MULTIPLE_JAN)
                    {
                        TempData[AppConst.ERROR] = ModifyTcInstructionResource.ERR_MULTIPLE_JAN;
                    }
                    vm.SearchConditions = condition;
                    vm.Results = new ModifyTcInstructionResult();
                    ViewBag.Message = message;
                    CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);
                    return this.View("~/Areas/Ship/Views/ModifyTcInstruction/Index.cshtml", vm);
                }
            }
        }

        //JANモーダル
        public ActionResult JanModal(ModifyTcInstructionSearchConditions searchCondition)
        {
            var totalItemCount = 0;
            var janReturns =  _ModifyTcInstructionQuery.ListingJan(searchCondition, ref totalItemCount);
            ModifyTcInstructionSearchConditions vm = new ModifyTcInstructionSearchConditions();
            vm.janViewModel = janReturns;
            vm.totalCnt = totalItemCount;

            return this.PartialView("~/Areas/Ship/Views/ModifyTcInstruction/_JanSearchModal.cshtml", vm);
        }

        #endregion Private
    }
}