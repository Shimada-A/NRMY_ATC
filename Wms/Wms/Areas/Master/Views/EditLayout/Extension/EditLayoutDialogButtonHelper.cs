using Share.Common.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Wms.Areas.Master.Resources;
using Wms.Common;
using Wms.Helpers;

namespace Wms.Areas.Master.Views.Extention
{
    public static class EditLayoutDialogButtonHelper
    {
        public static IHtmlString CreateOrUpdateButtonFor<TModel>(this HtmlHelper<TModel> helper,
            bool isNewLayout,
            string form = "",
            string validate = "",
            string id =null)
        {
            return DialogButtonHelper.ConfirmButtonFor(helper,
                isNewLayout ? DialogType.Create : DialogType.Update,
                form,
                EditLayoutResource.Confirm,
                validate,
                htmlAttributes: new { id = id ?? "confirmButton", @class = "btn btn-outline-dark btnsize" });
        }
    }
}