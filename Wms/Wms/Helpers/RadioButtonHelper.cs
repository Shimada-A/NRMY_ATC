using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Wms.Helpers
{
    public static class RadioButtonHelper
    {
        public static IHtmlString BooleanRadioButtonFor<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> exp,
            string onClickEvent = null,
            object htmlAttributes = null)
        {
            var name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(
                ExpressionHelper.GetExpressionText(exp));
            // [を探して、その前の文字まででグループされているものとして作る
            var p = name.LastIndexOf('[');
            string group;
            if(p == -1)
            {
                // 存在していない場合は全体
                group = name;
            }
            else
            {
                group = name.Substring(0, p);
            }
            group += "_";

            var label = new TagBuilder("Label");

            var radio = new TagBuilder("input");
            radio.MergeAttribute("type", "radio");
            radio.MergeAttribute("name", $"_{group}");
            radio.AddCssClass("radio_input");
            // onclick内でチェックon/off関連の処理をおこなう
            radio.MergeAttribute("onclick", 
                $"$('input[type=checkbox][name^=\"{group}\"]').prop('checked', false);" +
                $"$('input[type=hidden][name^=\"{group}\"]').prop(\"value\", false);" +
                $"$('input[type=hidden][name=\"{name}\"]').prop(\"value\", true);{onClickEvent}");

            var checkBox = InputExtensions.CheckBoxFor(helper, exp);
            var span = new TagBuilder("span");
            span.AddCssClass("radio_parts");

            label.InnerHtml = string.Format(
                "{0}{1}{2}",
                radio.ToString(),
                span.ToString(),
                checkBox.ToHtmlString()
                );
            return MvcHtmlString.Create(label.ToString(TagRenderMode.Normal));

        }

    }
}