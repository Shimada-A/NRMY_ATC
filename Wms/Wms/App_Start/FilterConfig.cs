namespace Wms
{
    using System.Web.Mvc;
    using Share.Extensions.Attributes;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleExceptionAttribute());
            filters.Add(new ApplicationAuthorizeAttribute());
        }
    }

    public class ApplicationAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Note: To reach here, a Web.config path-specific rule 'allow users="?"' is needed (otherwise it redirects to login)
            var httpContext = filterContext.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;

            if (request.IsAjaxRequest())
            {
                response.SuppressFormsAuthenticationRedirect = true;
                response.TrySkipIisCustomErrors = true;
            }

            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}
