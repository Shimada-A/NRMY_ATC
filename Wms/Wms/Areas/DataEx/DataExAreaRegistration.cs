using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.DataEx
{
    using System.Web.Mvc;

    public class DataExAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DataEx";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DataEx_default",
                "DataEx/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}