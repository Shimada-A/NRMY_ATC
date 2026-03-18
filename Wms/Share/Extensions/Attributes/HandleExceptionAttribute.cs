namespace Share.Extensions.Attributes
{
    using System;
    using System.Net;
    using System.Web.Mvc;

    /// <summary>
    /// コントローラーで例外が発生したときの処理
    /// </summary>
    /// <remarks>https://qiita.com/mocha/items/6928870b2d02d4c1ac37</remarks>
    public class HandleExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                this.HandleAjaxRequestException(filterContext);
            }
            else
            {
                // custom errorが有効でなければ
                // base.OnException()でExceptionHandledがtrueにならないので
                // Application_Errorも呼ばれる
                base.OnException(filterContext);
            }
        }

        private void HandleAjaxRequestException(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.Result = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    filterContext.Exception.Message,
                    filterContext.Exception.StackTrace
                }
            };
        }
    }
}
