using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wms.Areas.Master.Extensions
{
    public static class MasterUrlHelper
    {
        public static string EditLayoutAction(this UrlHelper helper,string actionName)
        {
            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            return helper.Action(actionName,"EditLayout",new { area="Master"});
        }
    }
}