namespace Wms.Controllers
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Security;
    using Share.Common;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Areas.Master.Models;
    using Wms.Common;
    using Wms.Resources;
    using Wms.ViewModels;

    [AllowAnonymous]
    public class LoginController : Controller
    {
        /// <summary>
        /// GET: Login
        /// </summary>
        /// <returns>Login view</returns>
        public ActionResult Index()
        {
            // Reset message
            this.ViewBag.LoginStatus = string.Empty;
            this.ViewBag.LoginMessage = string.Empty;

            // Check user loged
            // if (User.Identity.IsAuthenticated)
            //    return Redirect("/Home/Index");
            return this.View();
        }

        /// <summary>
        /// Login: Index action
        /// </summary>
        /// <param name="loginViewModel">use loginr info</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateIncludeValue]
        public ActionResult Index([Bind(Include = "ShipperId, CenterId, UserId, PasswordHash")] LoginViewModel loginViewModel)
        {
            // check null
            if (loginViewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // validate model
            if (!this.ModelState.IsValid)
                return this.View();

            // Mapping model
            LoginViewModel user = new User().GetCurrentUser(loginViewModel.ShipperId, loginViewModel.UserId, loginViewModel.CenterId);

            // Validate user login
            var resultValidateUser = this.ValidateUserLogin(user, loginViewModel);

            if (user != null)
            {
                loginViewModel.MinLength = user.MinLength;
            }

            // In the case of login user is valid
            if (resultValidateUser.Item1 == ResultValidateUserLogin.OK)
            {
                // Reset Password Mistype Count
                var resultResetMisstypeCount = new User().ResetMisstypeCount(loginViewModel);
                if (resultResetMisstypeCount)
                {
                    // Set form authentication user
                    var ticket = this.SetFormsAuthentication(user, FormsAuthentication.Timeout.TotalMinutes);
                    return this.Redirect(FormsAuthentication.GetRedirectUrl(ticket.Name, ticket.IsPersistent));
                }
                else
                {
                    this.ViewBag.LoginStatus = 5;
                    this.ViewBag.LoginMessage = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                }
            }
            else if (resultValidateUser.Item2 == MessageResource.ERR_PASSWORD_DEFAULT)
            {
                var resultResetMisstypeCount = new User().ResetMisstypeCount(loginViewModel);
                if (!resultResetMisstypeCount)
                {
                    this.ViewBag.LoginStatus = 5;
                    this.ViewBag.LoginMessage = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                }
            }

            // Case else, return login status and error messsage
            this.ViewBag.LoginStatus = (byte)resultValidateUser.Item1;
            this.ViewBag.LoginMessage = resultValidateUser.Item2;

            return this.View("index", loginViewModel);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>Return to login screen</returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return this.Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <returns>ChangePassword View</returns>
        public ActionResult ChangePassword([Bind(Include = "ShipperId, CenterId, UserId, PasswordHash, MinLength")]LoginViewModel loginViewModel)
        {
            ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel
            {
                ShipperId = loginViewModel.ShipperId,
                CenterId = loginViewModel.CenterId,
                UserId = loginViewModel.UserId,
                PasswordHash = loginViewModel.PasswordHash,
                MinLength = loginViewModel.MinLength
            };

            this.SetPasswordPolicy(changePasswordViewModel);

            return this.View("ChangePassword", changePasswordViewModel);
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateIncludeValue]
        public ActionResult ChangedPassword([Bind(Include = "ShipperId, CenterId, UserId, PasswordHash, MinLength,CurrentPassword,NewPassword,NewConfirmPassword")]ChangePasswordViewModel changePasswordViewModel)
        {
            // check model null
            if (changePasswordViewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // check validation
            if (!this.ModelState.IsValid)
            {
                return this.View(changePasswordViewModel);
            }

            // Get auth user
            LoginViewModel user = new User().GetCurrentUser(changePasswordViewModel.ShipperId, changePasswordViewModel.UserId, changePasswordViewModel.CenterId);

            // Validate password
            var resultValidatePassword = this.ValidatePassword(changePasswordViewModel, user);
            if (resultValidatePassword.Item1.Equals(ResultValidateUserLogin.OK))
            {
                // Call ChangePassword from User
                var resultChangePassword = new User().ChangePassword(changePasswordViewModel);
                if (resultChangePassword)
                {
                    LoginViewModel authUser = new User().GetCurrentUser(changePasswordViewModel.ShipperId, changePasswordViewModel.UserId, changePasswordViewModel.CenterId);

                    // Reset form authentication
                    this.SetFormsAuthentication(authUser, FormsAuthentication.Timeout.TotalMinutes);

                    // Redirect to home page
                    return this.Redirect("~/Home/Index");
                }
                else
                {
                    // TODO
                    this.ViewBag.LoginStatus = 5;
                    this.ViewBag.LoginMessage = MessagesResource.ERR_EXCLUSIVE_UPDATE;
                }
            }

            this.ViewBag.ChangePasswordStatus = (byte)resultValidatePassword.Item1;
            this.ViewBag.ChangePasswordMessage = resultValidatePassword.Item2;
            this.SetPasswordPolicy(changePasswordViewModel);

            return this.View("ChangePassword", changePasswordViewModel);
        }

        #region Private Methods

        /// <summary>
        /// Validate User login
        /// </summary>
        /// <param name="user">User login info get from screen</param>
        /// <returns>status and message </returns>
        public Tuple<ResultValidateUserLogin, string> ValidateUserLogin(LoginViewModel user, LoginViewModel loginViewModel)
        {
            // validation status
            ResultValidateUserLogin status = ResultValidateUserLogin.OK;

            // error message
            var message = string.Empty;

            // Today
            var today = DateTimeOffset.Now;

            // '該当データが存在しない場合 / User is not exist
            if (user == null)
            {
                status = ResultValidateUserLogin.ERR_LOGIN_MENU;
                message = MessageResource.ERR_LOGIN_MENU;
            }
            else
            {
                // Verify vocation
                switch ((UserLapses)user.UserLapse)
                {
                    // ユーザーM.失効区分　＝　0：初期化　の場合 / In the case of Initial password
                    case UserLapses.Initialization:

                        // Check valid password and password misstype count
                        var resultValidateUserInitial = this.CheckPassword(user, loginViewModel);

                        // In the case of login user is not valid
                        if (resultValidateUserInitial.Item1 != ResultValidateUserLogin.OK)
                        {
                            return resultValidateUserInitial;
                        }

                        status = ResultValidateUserLogin.ERR_PASSWORD_DEFAULT;
                        message = MessageResource.ERR_PASSWORD_DEFAULT;

                        break;

                    // 'ユーザーM.失効区分＝9：失効　の場合 / In the case of user be revocation
                    case UserLapses.Revocation:
                        status = ResultValidateUserLogin.ERR_PASSWORD_EXPIRED;
                        message = MessageResource.ERR_PASSWORD_EXPIRED;
                        break;

                    case UserLapses.Enabled:
                        // Handle validate password login
                        var resultValidateUser = this.CheckPassword(user, loginViewModel);

                        // In the case of login user is valid
                        if (resultValidateUser.Item1 != ResultValidateUserLogin.OK)
                        {
                            return resultValidateUser;
                        }

                        break;
                }
            }

            return Tuple.Create(status, message);
        }

        /// <summary>
        /// Check valid user login
        /// </summary>
        /// <param name="user">password of User login from screen</param>
        /// <param name="loginViewModel">User info from database</param>
        /// <param name="isChangePassword">ChangePassword flag</param>
        /// <returns>
        /// Status and messsage
        /// </returns>
        public Tuple<ResultValidateUserLogin, string> CheckPassword(LoginViewModel user, LoginViewModel loginViewModel, bool isChangePassword = false)
        {
            // validation status
            ResultValidateUserLogin status = ResultValidateUserLogin.OK;

            // error message
            var message = string.Empty;

            // Verify password /「ユーザーM.パスワードハッシュ値」＜＞「画面.パスワード」　の場合
            if (!HashUtil.VerifyHash(loginViewModel.PasswordHash, user.PasswordHash))
            {
                // ユーザーM.パスワード入力ミス回数＞　2の場合
                if (user.UserLapse != (byte)UserLapses.Revocation && user.PasswordMistypeCount >= int.Parse(user.GenName) && user.DateMakeClass == "0")
                {
                    user.UserLapse = (byte)UserLapses.Revocation;
                    status = ResultValidateUserLogin.ERR_PASSWORD_MISTYPE1;
                    message = MessageResource.ERR_PASSWORD_MISTYPE;
                }
                else
                {
                    if (isChangePassword)
                    {
                        status = ResultValidateUserLogin.ERR_PASSWORD_DEFAULT;
                        message = MessageResource.ERR_PASSWORD_DEFAULT;
                    }
                    else
                    {
                        status = ResultValidateUserLogin.ERR_LOGIN_MENU;
                        message = MessageResource.ERR_LOGIN_MENU;
                    }
                }

                // パスワード入力ミス回数更新
                var updFlg = new User().UpdatePasswordMistypeCount(user);

                return Tuple.Create(status, message);
            }

            return Tuple.Create(status, message);
        }

        /// <summary>
        /// Check valid user login
        /// </summary>
        /// <param name="user">password of User login from screen</param>
        /// <param name="loginViewModel">User info from database</param>
        /// <param name="isChangePassword">ChangePassword flag</param>
        /// <returns>
        /// Status and messsage
        /// </returns>
        public Tuple<ResultValidateUserLogin, string> CheckChangePassword(ChangePasswordViewModel changePasswordViewModel, LoginViewModel authUser)
        {
            // validation status
            ResultValidateUserLogin status = ResultValidateUserLogin.OK;

            // error message
            var message = string.Empty;

            // Verify password /「ユーザーM.パスワードハッシュ値」＜＞「画面.パスワード」　の場合
            if (!HashUtil.VerifyHash(changePasswordViewModel.CurrentPassword, authUser.PasswordHash))
            {
                // 変更前パスワードチェック
                status = ResultValidateUserLogin.ERR_PASSWORD;
                message = MessageResource.ERR_PASSWORD;
                return Tuple.Create(status, message);
            }
            else
            {
                if (!changePasswordViewModel.NewPassword.Equals(changePasswordViewModel.NewConfirmPassword))
                {
                    // 新パスワードが一致してない
                    status = ResultValidateUserLogin.ERR_LOGIN_MENU;
                    message = MessageResource.ERR_PASSWORD_POLICY;
                    return Tuple.Create(status, message);
                }

                if (this.IsEnglishNumericOtherExists(changePasswordViewModel.NewPassword))
                {
                    // 含められる文字は半角英数字です
                    status = ResultValidateUserLogin.ERR_LOGIN_MENU;
                    message = MessageResource.ERR_PASSWORD_POLICY;
                    return Tuple.Create(status, message);
                }

                if (changePasswordViewModel.UserId.Equals(changePasswordViewModel.NewPassword))
                {
                    // ユーザーIDと同じのパスワードは、使用できません
                    status = ResultValidateUserLogin.ERR_LOGIN_MENU;
                    message = MessageResource.ERR_PASSWORD_POLICY;
                    return Tuple.Create(status, message);
                }

                if (int.Parse(changePasswordViewModel.MinLength) > changePasswordViewModel.NewPassword.Length)
                {
                    // 最低文字数以上
                    status = ResultValidateUserLogin.ERR_LOGIN_MENU;
                    message = MessageResource.ERR_PASSWORD_POLICY;
                    return Tuple.Create(status, message);
                }
            }

            return Tuple.Create(status, message);
        }

        /// <summary>
        /// パスワードポリシー設定
        /// </summary>
        /// <param name="user"></param>
        private void SetPasswordPolicy(ChangePasswordViewModel changePasswordViewModel)
        {
            //// Get shipper
            // var shipper = new Shipper().GetShipperById(Common.Profile.User.ShipperId);
            var shipper = changePasswordViewModel.ShipperId;
            this.ViewBag.PasswordPolicyAlphanumeric = MessageResource.PasswordPolicyAlphanumeric;
            this.ViewBag.PasswordPolicyUserId = MessageResource.PasswordPolicyUserId;

            this.ViewBag.PasswordPolicyMinByte = string.Format(MessageResource.PasswordMinByte, changePasswordViewModel.MinLength);
        }

        /// <summary>
        /// Validate User login
        /// </summary>
        /// <param name="changePasswordViewModel">User login info get from screen</param>
        /// <param name="authUser">Authentication user get from database</param>
        /// <returns>status and message </returns>
        public Tuple<ResultValidateUserLogin, string> ValidatePassword(ChangePasswordViewModel changePasswordViewModel, LoginViewModel authUser)
        {
            // validation status
            ResultValidateUserLogin status = ResultValidateUserLogin.OK;

            // error message
            var message = string.Empty;

            // Today
            var today = DateTimeOffset.Now;

            // '該当データが存在しない場合 / User is not exist
            if (authUser == null)
            {
                status = ResultValidateUserLogin.ERR_LOGIN_MENU;
                message = MessagesResource.MSG_NOT_FOUND;
            }
            else
            {
                // 'ユーザーM.失効区分＝9：失効　の場合 / In the case of user be revocation
                if (authUser.UserLapse == (byte)UserLapses.Revocation)
                {
                    status = ResultValidateUserLogin.ERR_LOGIN_MENU;
                    message = MessageResource.ERR_LOGIN_MENU;
                }

                // Verify vocation
                switch ((UserLapses)authUser.UserLapse)
                {
                    // 'ユーザーM.失効区分＝9：失効　の場合 / In the case of user be revocation
                    case UserLapses.Revocation:
                        status = ResultValidateUserLogin.ERR_LOGIN_MENU;
                        message = MessageResource.ERR_LOGIN_MENU;
                        break;

                    case UserLapses.Initialization:
                    case UserLapses.Enabled:

                        // Check valid password and password misstype count
                        var resultValidateUserInitial = this.CheckChangePassword(changePasswordViewModel, authUser);

                        // In the case of login user is not valid
                        if (resultValidateUserInitial.Item1 != ResultValidateUserLogin.OK)
                        {
                            return resultValidateUserInitial;
                        }

                        break;
                }
            }

            return Tuple.Create(status, message);
        }

        /// <summary>
        /// Check the same as the last 3 password changes
        /// </summary>
        /// <param name="password">password value</param>
        /// <param name="authUser">user authentication</param>
        /// <returns></returns>
        private bool IsTheSameOldPassword(string password, User authUser)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(authUser.PasswordHash3))
            {
                result = HashUtil.VerifyHash(password, authUser.PasswordHash3);
            }

            if (!string.IsNullOrEmpty(authUser.PasswordHash2) && result == false)
            {
                result = HashUtil.VerifyHash(password, authUser.PasswordHash2);
            }

            if (!string.IsNullOrEmpty(authUser.PasswordHash1) && result == false)
            {
                result = HashUtil.VerifyHash(password, authUser.PasswordHash1);
            }

            if (!string.IsNullOrEmpty(authUser.PasswordHash) && result == false)
            {
                result = HashUtil.VerifyHash(password, authUser.PasswordHash);
            }

            return result;
        }

        /// <summary>
        /// The Setting Form Authentication method handle logic set cookie, ticket  for the user
        /// </summary>
        /// <param name="user">User login</param>
        /// <param name="expireTime">expired time</param>
        /// <returns>Form sAuthentication Ticket </returns>
        public FormsAuthenticationTicket SetFormsAuthentication(LoginViewModel user, double expireTime)
        {
            FormsAuthentication.SetAuthCookie(user.UserId + "," + user.ShipperId + "," + user.UserName + "," + user.CenterId + "," + user.PermissionLevel, false);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                                     1, // Ticket version
                                                     user.UserId + "," + user.ShipperId + "," + user.UserName + "," + user.CenterId + "," + user.PermissionLevel, // username to be used by ticket
                                                     DateTime.Now, // ticket issue date-time
                                                     DateTime.Now.AddMinutes(expireTime), // Date and time the cookie will expire
                                                     true, // persistent cookie?
                                                     user.UserId + "," + user.ShipperId + "," + user.UserName + "," + user.CenterId + "," + user.PermissionLevel);

            var cookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                Expires = ticket.Expiration,
                Path = FormsAuthentication.FormsCookiePath,
                Secure = FormsAuthentication.RequireSSL,
                Domain = FormsAuthentication.CookieDomain
            };

            // save cookie to response
            this.HttpContext.Response.Cookies.Add(cookie);

            return ticket;
        }

        // <summary>

        // 数値のみチェック
        // </summary>
        // <param name="str">チェック文字</param>
        // <returns></returns>
        public bool IsNumericExists(string str)
        {
            if (Regex.Matches(str, "[0-9]").Count == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 英字のみチェック
        /// </summary>
        /// <param name="str">チェック文字</param>
        /// <returns></returns>
        public bool IsEnglishExists(string str)
        {
            if (Regex.Matches(str, "[a-zA-Z]").Count == 0)
            {
                return true;
            }

            return false;
        }

        public bool IsEnglishNumericOtherExists(string str)
        {
            if(!(Regex.Match(str, "^[a-zA-Z0-9]+$")).Success)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}