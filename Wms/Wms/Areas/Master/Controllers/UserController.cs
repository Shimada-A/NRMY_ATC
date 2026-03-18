namespace Wms.Areas.Master.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Share.Extensions.Classes;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Master.Reports.Export;
    using Wms.Areas.Master.ViewModels.User;
    using Wms.Controllers;
    using Wms.Resources;

    public class UserController : BaseController
    {
        #region Constants

        /// <summary>
        /// Key to get search info from cookie
        /// </summary>
        private const string COOKIE_SEARCHCONDITIONS = "WMASUser01.SearchConditions";

        private User _UserQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserController()
        {
            this._UserQuery = new User();
        }

        #endregion Constants

        #region Search

        /// <summary>
        /// Search User
        /// </summary>
        /// <returns>List Record</returns>
        public ActionResult Index()
        {
            var searchInfo = this.GetPreviousSearchInfo(true);
            return this.GetSearchResultView(searchInfo, true);
        }

        /// <summary>
        /// Search User
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
        public ActionResult Search(UserSearchCondition SearchConditions)
        {
            UserSearchCondition condition;

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

        public JsonResult GetPKeys(UserSearchCondition SearchConditions)
        {
            List<string> ids = _UserQuery.GetUserId(SearchConditions);
            return Json(ids, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ダウンロード
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Download()
        {
            UserSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.UserReport report = new Reports.Export.UserReport(ReportTypes.Excel, searchCondition);
            report.Export();

            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        /// <summary>
        /// Gas用作業者マスタCSVダウンロード
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GasDownload()
        {
            UserSearchCondition searchCondition = this.GetPreviousSearchInfo(false);

            Reports.Export.UserGasReport report = new Reports.Export.UserGasReport(ReportTypes.Csv, searchCondition);
            report.Export();

            return this.File(report.FileContent, report.ContentType, report.DownloadFileName);
        }

        //A4印刷
        [HttpPost]
        public ActionResult PrintA4()
        {
            UserSearchCondition condition = this.GetPreviousSearchInfo(false);
            const string styleName = "UserLabelA4.sty";
            string controllerName = RouteData.Values["controller"].ToString();
            string ret = string.Empty;
            UserlabelReportForCsv report = new UserlabelReportForCsv(condition);
            report.Export();

            // CSV作成
            new CsvPrintFileCreate().CreateCsvFile(controllerName, report.DownloadFileName, report.FileContent);

            //// PDF作成
            //ret = new CsvPrintFileCreate().OutputPDF(controllerName, styleName, report.DownloadFileName);

            //return this.File(ret, "application/pdf");

            return WfrPrint("UserLabel.wfr", report.DownloadFileName);
        }

        #endregion Search

        #region Edit

        /// <summary>
        /// Insert User Information
        /// </summary>
        /// <param name="SearchConditions"</param>
        /// <returns>Insert View</returns>
        public ActionResult Insert(UserSearchCondition SearchConditions)
        {
            var input = new UserInput()
            {
                User = new User(),
                InUpDiff = "0", //新規作成処理
            };
            input.User.ShipperId = Common.Profile.User.ShipperId;
            input.User.CenterId = Common.Profile.User.CenterId;
            input.User.PermissionLevel = Common.Profile.User.PermissionLevel;
            input.SearchFlag = SearchConditions.SearchFlag;

            ViewBag.Center = _UserQuery.GetSelectListItems();

            return View("~/Areas/Master/Views/User/Edit.cshtml", input);
        }

        /// <summary>
        /// 削除更新処理
        /// </summary>
        /// <param name="users">List User</param>
        /// <returns>Edit View</returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateIncludeValue]
        public ActionResult Delete(string userId)
        {            
            //データ作成区分チェック List
            List<string> deleteCheckList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(userId);

            var Deletecheck = _UserQuery.DataClassCheck(deleteCheckList);

            if (Deletecheck != null)
            {
                TempData[AppConst.ERROR] = string.Format(MessagesResource.ERR_OEVER_DELETE, Deletecheck);
                return RedirectToAction("IndexSearch");
            }

            //// 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (!_UserQuery.DeleteUser(deleteCheckList))
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_ERR_EXCLUSIVE_DELETE;
                return RedirectToAction("IndexSearch");
            }

            //削除フラグ更新処理
            else
            {
                TempData[AppConst.SUCCESS] = MessagesResource.SUC_DELETE;
                return RedirectToAction("IndexSearch");
            }

        }

        /// <summary>
        /// Edit User Information
        /// </summary>
        /// <param name="users">List User</param>
        /// <returns>Edit View</returns>
        public ActionResult Edit(string userid, User user)
        {
            // Get record from DB
            var target = _UserQuery.GetTargetById(userid, Common.Profile.User.ShipperId);

            // 更新対象のデータがマスタに無い場合、エラー（別のユーザーによって削除済みの場合）
            if (target == null)
            {
                TempData[AppConst.ERROR] = MessagesResource.MSG_NOT_FOUND;
                return RedirectToAction("IndexSearch");
            }

            var input = new UserInput()
            {
                User = target,
                InUpDiff = "1", //更新処理
            };
            input.SearchFlag = true;

            ViewBag.Center = _UserQuery.GetSelectListItems();

            return View("~/Areas/Master/Views/User/Edit.cshtml", input);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="user">user Information</param>
        /// <param name="inpassword">inpassword Information</param>
        /// <param name="uppassword">uppassword Information</param>
        /// <param name="inpasswordConfirm">inpasswordConfirm Information</param>
        /// <param name="uppasswordConfirm">uppasswordConfirm Information</param>
        /// <param name="inUpDiff">"0":Insert, "1":Update</param>
        /// <returns>Edit View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateIncludeValue]
        public ActionResult Update([Bind(Include = "UpdateCount, ShipperId, UserId, UserName, CenterId, PermissionLevel")] User user,
                                   [Bind(Include = "UpdateCount, ShipperId, UserId, UserName, CenterId, PermissionLevel")] UserList userList,
                                    string inpassword,
                                    string uppassword,
                                    string inpasswordConfirm,
                                    string uppasswordConfirm,
                                    string inUpDiff)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.SelectMany(m => m.Value.Errors.Select(e => e.ErrorMessage)).ToList();

                // エラー内容を取得・セット
                foreach (var message in errorMessages)
                {
                    ModelState.AddModelError(string.Empty, message);
                }

                var input = new UserInput()
                {
                    User = user,
                    InUpDiff = inUpDiff,
                };
                input.User.UpdateCount = user.UpdateCount;
                input.User.ShipperId = Common.Profile.User.ShipperId;
                ViewBag.Center = _UserQuery.GetSelectListItems();
                return View("~/Areas/Master/Views/User/Edit.cshtml", input);
            }

            //パスワード最小桁数取得
            int minLength = _UserQuery.GetPassMinLength();
            if (inUpDiff == "1")            // 更新の場合
            {
                if (uppassword != string.Empty)
                {
                    if (user.UserId == uppassword)
                    {
                        var input = new UserInput()
                        {
                            User = user,
                            InUpDiff = inUpDiff,
                        };
                        input.User.UpdateCount = user.UpdateCount;
                        input.User.ShipperId = Common.Profile.User.ShipperId;
                        ViewBag.Center = _UserQuery.GetSelectListItems();
                        TempData[AppConst.ERROR] = MessagesResource.ERR_USERID_PASSWORD_SAME;
                        return View("~/Areas/Master/Views/User/Edit.cshtml", input);
                    }

                    if (uppassword.Length < minLength)
                    {
                        var input = new UserInput()
                        {
                            User = user,
                            InUpDiff = inUpDiff,
                        };
                        input.User.UpdateCount = user.UpdateCount;
                        input.User.ShipperId = Common.Profile.User.ShipperId;
                        ViewBag.Center = _UserQuery.GetSelectListItems();
                        TempData[AppConst.ERROR] = string.Format(MessageResource.MinLength, "パスワード", minLength);
                        return View("~/Areas/Master/Views/User/Edit.cshtml", input);
                    }

                    if (IsTheSameOldPassword(uppassword, user))
                    {
                        var input = new UserInput()
                        {
                            User = user,
                            InUpDiff = inUpDiff,
                        };
                        input.User.UpdateCount = user.UpdateCount;
                        input.User.ShipperId = Common.Profile.User.ShipperId;
                        ViewBag.Center = _UserQuery.GetSelectListItems();
                        TempData[AppConst.ERROR] = MessagesResource.ERR_PASSWORD_REPEAT;
                        return View("~/Areas/Master/Views/User/Edit.cshtml", input);
                    }
                }

                if (_UserQuery.UpdateUser(user, uppassword, uppasswordConfirm, userList))
                {
                    // Clear message to back to index screen
                    TempData[AppConst.SUCCESS] = MessagesResource.SUC_UPDATE;
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                }
            }
            else if (inUpDiff == "0")
            {
                // 新規登録の場合
                var u = _UserQuery.GetTargetById(user.UserId, Common.Profile.User.ShipperId);
                if (u != null && u.DeleteFlag ==0)
                {
                    var input = new UserInput()
                    {
                        User = user,
                        InUpDiff = inUpDiff,
                    };
                    input.User.UpdateCount = user.UpdateCount;
                    input.User.ShipperId = Common.Profile.User.ShipperId;
                    ViewBag.Center = _UserQuery.GetSelectListItems();
                    TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                    return View("~/Areas/Master/Views/User/Edit.cshtml", input);
                }

                if (user.UserId == inpassword)
                {
                    var input = new UserInput()
                    {
                        User = user,
                        InUpDiff = inUpDiff,
                    };
                    input.User.UpdateCount = user.UpdateCount;
                    input.User.ShipperId = Common.Profile.User.ShipperId;
                    ViewBag.Center = _UserQuery.GetSelectListItems();
                    TempData[AppConst.ERROR] = MessagesResource.ERR_USERID_PASSWORD_SAME;
                    return View("~/Areas/Master/Views/User/Edit.cshtml", input);
                }

                if (inpassword.Length < minLength)
                {
                    var input = new UserInput()
                    {
                        User = user,
                        InUpDiff = inUpDiff,
                    };
                    input.User.UpdateCount = user.UpdateCount;
                    input.User.ShipperId = Common.Profile.User.ShipperId;
                    ViewBag.Center = _UserQuery.GetSelectListItems();
                    TempData[AppConst.ERROR] = string.Format(MessageResource.MinLength, "パスワード", minLength);
                    return View("~/Areas/Master/Views/User/Edit.cshtml", input);
                }

                if (_UserQuery.InsertUser(user, inpassword, inpasswordConfirm))
                {
                    TempData[AppConst.SUCCESS] = MessagesResource.SUC_INSERT;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData[AppConst.ERROR] = MessagesResource.ERR_ALREADY_SAVED;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("IndexSearch");
        }

        #endregion Edit

        #region Private

        /// <summary>
        /// 前回の検索条件を取得する
        /// </summary>
        /// <returns>UserSearchCondition</returns>
        private UserSearchCondition GetPreviousSearchInfo(bool indexFlag)
        {
            var condition = indexFlag ? new UserSearchCondition() : Request.Cookies.Get<UserSearchCondition>(COOKIE_SEARCHCONDITIONS) ?? new UserSearchCondition();
            condition.PageSize = this.GetCurrentPageSize();
            condition.Page = 1;

            // return search object
            return condition;
        }

        /// <summary>
        /// 検索結果ビューを取得する
        /// </summary>
        /// <param name="condition">Search User Information</param>
        /// <returns>Index View</returns>
        private ActionResult GetSearchResultView(UserSearchCondition condition, bool indexFlag)
        {
            // Save search info
            var vm = new Index
            {
                SearchConditions = condition,
                UserResult = indexFlag ? new UserResult() : new UserResult()
                {
                    Users = _UserQuery.GetData(condition)
                }
            };

            var ProcNumLimit = this.GetCurrentProcNumLimit();
            if (ProcNumLimit != 0 && vm != null)
            {
                if (vm.UserResult.Users.TotalItemCount > ProcNumLimit)
                {
                    TempData[AppConst.ERROR] = string.Format(MessageResource.ERR_PROC_NUM_LIMIT, ProcNumLimit);
                    vm.UserResult.Users = null;
                }
            }

            // 検索用セレクトボックス値を取得
            ViewBag.Center = _UserQuery.GetSelectCenterListItems();
            ViewBag.PermissionLevel = Common.Profile.User.PermissionLevel;
            CookieExtention.SetSearchConditonCookie(COOKIE_SEARCHCONDITIONS, condition);

            // Return index view
            return this.View("~/Areas/Master/Views/User/Index.cshtml", vm);
        }

        /// <summary>
        /// Check the same as the last 3 password changes
        /// </summary>
        /// <param name="password">password value</param>
        /// <param name="User">user authentication</param>
        /// <returns></returns>
        private bool IsTheSameOldPassword(string password, User User)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(User.PasswordHash3))
            {
                result = HashUtil.VerifyHash(password, User.PasswordHash3);
            }

            if (!string.IsNullOrEmpty(User.PasswordHash2) && !result)
            {
                result = HashUtil.VerifyHash(password, User.PasswordHash2);
            }

            if (!string.IsNullOrEmpty(User.PasswordHash1) && !result)
            {
                result = HashUtil.VerifyHash(password, User.PasswordHash1);
            }

            if (!string.IsNullOrEmpty(User.PasswordHash) && !result)
            {
                result = HashUtil.VerifyHash(password, User.PasswordHash);
            }

            return result;
        }

        #endregion Private
    }
}