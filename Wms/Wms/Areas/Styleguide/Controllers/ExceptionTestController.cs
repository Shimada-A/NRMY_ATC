namespace Wms.Areas.Styleguide.Controllers
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// 例外のテスト用コントローラー
    /// </summary>
    public class ExceptionTestController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult AjaxSuccess()
        {
            return this.View("Index");
        }

        public ActionResult AjaxIndex()
        {
            throw new Exception("ajaxindex error");
        }

        public ActionResult NormalIndex()
        {
            throw new Exception("normalindex error");
        }
    }
}