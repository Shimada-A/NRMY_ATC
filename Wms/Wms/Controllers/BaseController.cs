namespace Wms.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NLog;
    using Wms.Extensions.Attributes;
    using Wms.Models;

    /// <summary>
    /// コントローラーの基底クラス
    /// </summary>
    [MenuAuthorization]

    // [SystemLogFilter]
    // [RequireHttps]
    public class BaseController : Controller
    {
        public BaseController()
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

        /// <summary>
        /// 帳票を発行する(WonderfulReport)
        /// </summary>
        /// <param name="wfrName">帳票ファイル名</param>
        /// <param name="dataName">データファイル名</param>
        /// <param name="printerName">印刷プリンタ名</param>
        /// <returns></returns>
        protected ActionResult WfrPrint(string wfrName, string dataName, string printerName = "")
        {
            return Redirect(GetWfrPrintUrl(wfrName, dataName, printerName));
        }

        /// <summary>
        /// 帳票発行用URLを返します(WonderfulReport)
        /// </summary>
        /// <param name="wfrName"></param>
        /// <param name="dataName"></param>
        /// <param name="printerName"></param>
        /// <returns></returns>
        protected string GetWfrPrintUrl(string wfrName, string dataName, string printerName = "")
        {
            var server = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
            var printer = (!string.IsNullOrEmpty(printerName)) ? $" /p \"{printerName}\" /po ProgressDlg off" : "";
            return $"Wfr.Preview.3:{server}{Url.Content($"~/Reports/wfr/{wfrName}")} /d Data1 0 {server}{Url.Content($"~/{ConfigurationManager.AppSettings["TempTaskPrintDir"]}/{RouteData.Values["controller"]}/{dataName}")}{printer}";
        }

        /// <summary>
        /// 帳票定義情報(WonderfulReport)
        /// </summary>
        protected class WfrReport
        {
            protected internal string WfrFile { get; set; }

            protected internal string DataFile { get; set; }

            protected internal WfrReport(string wfrName, string dataName)
            {
                WfrFile = wfrName;
                DataFile = dataName;
            }
        }

        /// <summary>
        /// 仕分印刷用URLを返します(WonderfulReport)
        /// </summary>
        /// <param name="wfrReports"></param>
        /// <returns></returns>
        protected string GetWfrSortPrintUrl(IEnumerable<WfrReport> wfrReports)
        {
            if (!wfrReports.Any())
            {
                return string.Empty;
            }

            var server = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);

            var wfrUrl = string.Empty;
            foreach (var wfrReport in wfrReports)
            {
                if (!string.IsNullOrEmpty(wfrUrl)) wfrUrl += " /add ";
                wfrUrl += $"{server}{Url.Content($"~/Reports/wfr/{wfrReport.WfrFile}")} /d Data1 0 {server}{Url.Content($"~/{ConfigurationManager.AppSettings["TempTaskPrintDir"]}/{RouteData.Values["controller"]}/{wfrReport.DataFile}")}";
            }

            return $"Wfr.Preview.3:{wfrUrl} /p5 /po SortListFile {server}{Url.Content($"~/Reports/wfr/SortList.txt")} /po ProgressDlg off";
        }

    }
}