namespace Wms.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// ControllerFactory
    /// </summary>
    public class MvcControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// CreateController
        /// 一部Genericのコントローラーに対応
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var t = this.GetControllerType(requestContext, controllerName);
            if (t != null) return Activator.CreateInstance(t) as IController;

            //// 所在検索ダイアログ行きの場合
            // if (controllerName == "LocationSearchModal")
            // {
            //    var locSearch = typeof(Wms.Areas.Master.Controllers.LocationSearchModalController<>);
            //    var param = (requestContext.RouteData.Values.ContainsKey("param")
            //        ? requestContext.RouteData.Values["param"] :
            //          requestContext.HttpContext.Request.Form["param"]);
            //    if (param == null)
            //        param = (requestContext.HttpContext.Request.QueryString["param"] == null ? null :
            //            requestContext.HttpContext.Request.QueryString["param"]);
            //    var controllerType = locSearch.MakeGenericType(Type.GetType($"Mvc.Areas.Master.ViewModels.LocationSearchModal.{param}"));
            //    return Activator.CreateInstance(controllerType) as IController;
            // }
            return base.CreateController(requestContext, controllerName);
        }
    }
}