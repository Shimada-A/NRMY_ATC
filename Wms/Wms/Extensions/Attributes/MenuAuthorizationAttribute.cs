namespace Wms.Extensions.Attributes
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Check authorization
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MenuAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            // var db = MvcDbContext.Current;
            // var controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            // var canUse = (from userPro in db.UserPrograms
            //              join program in db.Programs
            //              on new { userPro.ProgramId, userPro.ShipperId }
            //              equals new { program.ProgramId, program.ShipperId }
            //              where userPro.UserId == Profile.User.UserId &&
            //                    userPro.ShipperId == Profile.User.ShipperId &&
            //                    userPro.UsableFlag == true &&
            //                    program.ControllerName.ToUpper() == controller.ToUpper()
            //              select userPro.UsableFlag).Any();
            // if (!canUse)
            // {
            //    return false;
            // }
            return base.AuthorizeCore(httpContext);
        }

        /// <summary>
        /// Handle Unauthorized Request
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult("Default", new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
        }
    }
}