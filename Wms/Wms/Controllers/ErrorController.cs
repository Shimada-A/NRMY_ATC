namespace Wms.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Share.Common;
    using Share.Common.Resources;
    using static System.Configuration.ConfigurationManager;

    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(int statusCode)
        {
            this.TempData["StatusCode"] = statusCode.ToString();
            this.Response.StatusCode = statusCode;
            ViewModels.VmError vmerror = new ViewModels.VmError();
            this.SetupViewData(statusCode);

            if ((HttpStatusCode)statusCode == HttpStatusCode.InternalServerError)
            {
                vmerror.Information = MessagesResource.TextEscalation;
            }

            if (AppSettings["ErrorView"] == "true")
            {
                int.TryParse(AppSettings["ErrorViewBeforeSeconds"], out int intBeforeSeconds);

                foreach (EventLogEntry item in new EventLog("Application", ".").Entries)
                {
                    if (item.TimeGenerated >= DateTime.Now.AddSeconds(-1 * intBeforeSeconds))
                    {
                        vmerror.Category = item.Category;
                        vmerror.EventID = item.InstanceId.ToString();
                        vmerror.Message = item.Message;
                        vmerror.ReplacementStrings = item.ReplacementStrings[0];

                        // error.SiteName = item.Site.Name;
                        vmerror.Source = item.Source;
                        vmerror.TimeWritten = item.TimeWritten.ToString("yyyy/MM/dd hh:mm:ss");
                        vmerror.DisplayDiv = "grid";
                        break;
                    }
                }
            }

            return this.View(vmerror);
        }

        public ActionResult HttpError(HttpException error)
        {
            this.SetupViewData(error.GetHttpCode());
            return this.View("Error");
        }

        private void SetupViewData(int statusCode)
        {
            this.ViewData[AppConst.HTTP_STATUS] = statusCode;
            this.ViewData[AppConst.ERROR] = this.GetMessage((HttpStatusCode)statusCode);
        }

        private string GetMessage(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest:

                    return MessagesResource.BadRequest;

                case HttpStatusCode.NotFound:

                    return MessagesResource.NotFound;

                case HttpStatusCode.Forbidden:

                    return MessagesResource.Forbidden;

                case HttpStatusCode.InternalServerError:

                    return MessagesResource.InternalServerError;
            }

            return string.Empty;
        }
    }
}