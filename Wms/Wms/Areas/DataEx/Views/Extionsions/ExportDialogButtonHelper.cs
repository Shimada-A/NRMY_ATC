using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.Master.Resources;
using Wms.Common;
using Wms.Helpers;

namespace Wms.Areas.DataEx.Views.Extionsions
{
    public static class ExportDialogButtonHelper
    {
        public static IHtmlString ExportCsvButtonFor<TModel>(this HtmlHelper<TModel> helper,
            string form = "",
            string validate = "",
            string id = null)
        {
            return DialogButtonHelper.ConfirmButtonFor(helper,
                DialogType.Csv,
                form,
                Wms.Resources.CommonResource.Export,
                validate,
                htmlAttributes: new { id = id ?? "confirmButton", @class = "btn btn-outline-dark btnsize" });
        }
    }
}