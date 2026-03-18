namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.ViewModels.UserProgram;
    using Wms.Controllers;
    using Wms.Models;
    using Wms.Resources;

    public class UserProgramController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASUserProgram01.SearchConditions";
        private UserProgram _UserProgramQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserProgramController()
        {
            this._UserProgramQuery = new UserProgram();
        }
        #endregion

        #region Search

        /// <summary>
        /// Search UserProgram
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// Search UserProgram
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
        public ActionResult Search(UserProgramSearchCondition SearchConditions)
        {
            UserProgramSearchCondition condition;

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

        public JsonResult GetPKeys(UserProgramSearchCondition SearchConditions)
        {
            List<string> ids = _UserProgramQuery.GetProgramId(SearchConditions);
            return Json(ids, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Edit

        /// <summary>
        /// Edit UserProgram Information
        /// </summary>
        /// <param name="UserPrograms">List UserProgram</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(string programid, Common.ProgramPermissionLevelClasses pplc)
        {
            List<string> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(programid);

            // Get record from DB
            var input = new UserProgramInput()
            {
                UserProgramViewModel = _UserProgramQuery.GetRows(list, pplc),
                ShipperId = Common.Profile.User.ShipperId
            };

            // 更新対象のデータがマスタに無い場合、エラー
            if (input.UserProgramViewModel.Count == 0)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            return View("~/Areas/Master/Views/UserProgram/Edit.cshtml", input);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="userProgramViewModel">UserProgram Information</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(List<SelectedUserProgramViewModel> userProgramViewModel)
        {
            if (_UserProgramQuery.UpdateUserProgram(userProgramViewModel))
            {
                // Clear message to back to index screen
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
            }
            else
            {
                TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
            }

            return RedirectToAction("IndexSearch");
        }
        #endregion

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>UserProgramSearchCondition</returns>
        private UserProgramSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new UserProgramSearchCondition() : Request.Cookies.Get<UserProgramSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new UserProgramSearchCondition();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search UserProgram Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(UserProgramSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                UserProgramResult = indexFlag ? new UserProgramResult() : new UserProgramResult()
                {
                    UserPrograms = _UserProgramQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.UserProgramResult.UserPrograms.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.UserProgramResult.UserPrograms = null;
                }
            }

            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Master/Views/UserProgram/Index.cshtml", vm);

            // return this.View("Index", vm);
        }

        #endregion
    }
}