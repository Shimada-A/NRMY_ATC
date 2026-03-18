using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wms.Areas.DataEx.Extensions
{
    public static class DataExUrlHelper
    {
        public static string OutputGeneralDataAction(this UrlHelper helper,string actionName)
        {
            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            return helper.Action(actionName,"OutputGeneralData",new { area="DataEx"});
        }

        public static string ImportGeneralDataAction(this UrlHelper helper, string actionName)
        {
            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            return helper.Action(actionName, "ImportGeneralData", new { area = "DataEx" });
        }
    }
}