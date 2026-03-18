namespace Wms
{
    using System;
    using System.Data.Entity;
    using System.Net;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using Mvc.Common;
    using Share.Common;
    using Share.Extensions.Classes;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// アプリケーション
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// アプリケーション開始（エントリーポイント）
        /// </summary>
        /// <remarks>
        /// App_Start内のクラスのConfigをロードする
        /// </remarks>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // どこからでもWebアプリのパスを取得できるように開始時に設定する
            AppConfig.SetupPath(HttpContext.Current.Server.MapPath("~/"));

            // DBとDbContextに差異があっても、デバッグ時にエラーにならいようにする設定です
            Database.SetInitializer<MvcDbContext>(null);

            ModelBinders.Binders.DefaultBinder = new TrimModelBinder();

            // Config global multipe language error message
            // http://redwarrior.hateblo.jp/entry/2017/01/30/090000
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "ModelValidateMessage";
            DefaultModelBinder.ResourceClassKey = "ModelValidateMessage";

            // ControllerFactoryをカスタムしたものと差し替え
            ControllerBuilder.Current.SetControllerFactory(
                           typeof(Wms.Controllers.MvcControllerFactory));

            // Redisのタイムアウト対策
            this.SetRangeThreads(50, 100);
        }

        /// <summary>
        /// スレッドプール数の範囲を設定します。
        /// </summary>
        /// <param name="min">スレッドプール数の下限</param>
        /// <param name="max">スレッドプール数の上限</param>
        private void SetRangeThreads(int min, int max)
        {
            // Redisのタイムアウト対策
            // https://stackexchange.github.io/StackExchange.Redis/Timeouts
            // https://gist.github.com/JonCole/e65411214030f0d823cb
            // https://techinfoofmicrosofttech.osscons.jp/index.php?ASP.NET%20config
            // https://docs.microsoft.com/en-us/previous-versions/msp-n-p/ff647787(v=pandp.10)
            ThreadPool.SetMinThreads(min, min);
            ThreadPool.SetMaxThreads(max, max);
        }

        /// <summary>
        /// 認証リクエスト
        /// </summary>
        protected void Application_AuthenticateRequest()
        {
            var cookie = this.Context.Request.Cookies.Get(FormsAuthentication.FormsCookieName);
            if (cookie == null)
                return;

            FormsAuthenticationTicket ticket;
            try
            {
                ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket == null)
                    return;
            }
            catch (ArgumentException)
            {
                return;
            }

            if (ticket == null)
                return;

            // ユーザー情報を取得
            var userId = ticket.Name.Split(',')[0];
            var shipperId = ticket.Name.Split(',')[1];
            var userName = ticket.Name.Split(',')[2];
            var centerId = ticket.Name.Split(',')[3];
            var permissionLevel = ticket.Name.Split(',')[4];

            // var user = MvcDbContext.Current.User.Find(userId, shipperId);
            Areas.Master.Models.User user = new Areas.Master.Models.User()
            {
                UserId = userId,
                UserName = userName,
                ShipperId = shipperId,
                CenterId = centerId,
                PermissionLevel = (PermissionLevelClasses)Enum.Parse(typeof(PermissionLevelClasses), permissionLevel),
            };

            // TODO:パスワードは取得しない
            // var user =(
            //    from u in MvcDbContext.Current.User
            //    where u.UserId == userId && u.ShipperId == shipperId
            //    select new User
            //    {
            //        ShipperId = u.ShipperId,
            //        UserId = u.UserId,
            //        UserName = u.UserName,
            //        UserEnName = u.UserEnName,
            //        UserMail = u.UserMail
            //    }).ToList().FirstOrDefault();

            // In the case of user is null or user is revocation
            // if (user == null || user.UserLapse == (byte)UserLapses.Revocation)
            // {
            //    throw new AuthenticateException();
            // }
            HttpContext.Current.Items["UserProfile"] = user;
        }

        /// <summary>
        /// アプリケーション終了
        /// </summary>
        protected void Application_EndRequest()
        {
            // DbContextが生成されていたら破棄する
            if (MvcDbContext.HasCurrent)
            {
                MvcDbContext.Current.Dispose();
                System.Diagnostics.Debug.WriteLine("Dispose DbContext.");
            }
        }

        /// <summary>
        /// アプリケーションエラー時の処理
        /// </summary>
        protected void Application_Error()
        {
            if (this.Server == null) return;

            var ex = this.Server.GetLastError();
            if (ex == null) return;

             if (ex is HttpException &&
                ((HttpException)ex).GetHttpCode() == (int)HttpStatusCode.NotFound)
            {
                // NotFoundを相手にするとログが大変になるので無視
                return;
            }

            // var logger = LogManager.GetCurrentClassLogger();
            // logger.Error(ex);
            AppError.PutLog(ex);

            // SystemLog.WriteInfo(
            //    note: "アプリケーションエラー",
            //    logResult: LogResultClasses.Failure,
            //    errMessage: ex.Message,
            //    errStackTrace: ex.StackTrace);
        }
    }
}