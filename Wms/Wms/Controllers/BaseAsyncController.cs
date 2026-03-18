namespace Wms.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Mvc.Async;
    using System.Web.Routing;
    using NLog;
    using Wms.Extensions.Attributes;
    using Wms.Models;
    using Share.Models;

    /// <summary>
    /// コントローラーの基底クラス
    /// </summary>
    [MenuAuthorization]

    // [SystemLogFilter]
    // [RequireHttps]
    public class BaseAsyncController : AsyncController
    {
        public BaseAsyncController()
        {
            // 下記にコントローラー共通の処理を記述する
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            this.ViewBag.CurrentController = this.RouteData.Values["controller"];
            this.ViewBag.CurrentAction = this.RouteData.Values["action"];

            // 画面のアクセス履歴なのでajax(WebAPI的なもの)は対象外
            // if (!HttpContext.Request.IsAjaxRequest())
            // {
            //    var controller = RouteData.Values["controller"].ToString();
            //    var task = WriteAccessLogAsync(controller);
            // }
            // (例) Cookie取得
            // var title = "";
            // if (Request.Cookies["menutitle"] != null)
            // {
            //    title = WebUtility.UrlDecode(Request.Cookies["menutitle"].Value);
            // }

            // ViewBag.MenuTitle = title;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var rr = this.RouteData;
        }

        /// <summary>
        /// アクセスログ登録
        /// </summary>
        /// <param name="controllerName">コントローラー名</param>
        /// <returns></returns>
        private async Task WriteAccessLogAsync(string controllerName)
        {
            // 非同期処理内で呼ぶと取得できないので、awaitの外でUserを取得しています。
            var user = Common.Profile.User;

            // ログインしていない場合
            if (user == null)
                return;

            try
            {
                await Task.Run(() =>
                {
                    // var accessLog = new AccessLog();
                    // var ret = accessLog.Insert(user, controllerName);

                    // if (!ret)
                    // {
                    // アクセスログ登録失敗時の処理
                    var logger = LogManager.GetCurrentClassLogger();
                    logger.Error($"プログラムマスタに{controllerName}コントローラーが登録されていません。");

                    // }
                });
            }
            catch (Exception ex)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Error(ex, ex.Message);
            }
        }

        /// <summary>
        /// ページサイズを取得する
        /// </summary>
        /// <returns></returns>
        protected int GetCurrentPageSize()
        {
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            var pagingInfo = MvcDbContext.Current.Pagings.FirstOrDefault(m => m.ShipperId.ToUpper() == Common.Profile.User.ShipperId.ToUpper()
                                                                           && m.ControllerName.ToUpper() == controllerName.ToUpper()
                                                                           && actionName.ToUpper().Contains(m.ActionName.ToUpper()));
            if (pagingInfo != null)
            {
                return pagingInfo.PageSize;
            }
            else
            {
                return 100;
            }
        }

        /// <summary>
        /// ページサイズを取得する
        /// </summary>
        /// <returns></returns>
        protected int GetCurrentProcNumLimit()
        {
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            var pagingInfo = MvcDbContext.Current.Pagings.FirstOrDefault(m => m.ShipperId.ToUpper() == Common.Profile.User.ShipperId.ToUpper()
                                                                           && m.ControllerName.ToUpper() == controllerName.ToUpper()
                                                                           && actionName.ToUpper().Contains(m.ActionName.ToUpper()));
            if (pagingInfo != null)
            {
                return pagingInfo.ProcNumLimit;
            }
            else
            {
                return 0;
            }
        }

        public static CurrentBagForCpuTask GetCurrentBagForCpuTask()
        {

            return new CurrentBagForCpuTask()
            {
                ShipperId = Common.Profile.User.ShipperId,
                UserId = Common.Profile.User.UserId,
                CenterId = Common.Profile.User.CenterId,
                UserName = Common.Profile.User.UserName,
                ProgramName = Common.Profile.User.MakeProgramName,
                CurrentMvcDbContext = MvcDbContext.Current,
        };
        }
    }
}