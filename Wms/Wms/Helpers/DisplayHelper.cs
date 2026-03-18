using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Wms.Helpers
{
    public static class DisplayHelper
    {
        public static IHtmlString DisplayAndHiddenFor<TModel,TProperty>(this HtmlHelper<TModel> helper,
                    Expression<Func<TModel, TProperty>> exp,
                    object htmlAttributes = null)
        {
            return MvcHtmlString.Create(
                $"{DisplayExtensions.DisplayFor(helper,exp,htmlAttributes)}" +
                $"{InputExtensions.HiddenFor(helper,exp)}");
        }
    }
}