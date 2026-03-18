namespace Share.Helpers
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class AreaButtonHelper
    {
        /// <summary>
        /// ラジオボタンリストのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="listItems">RadioButtonの元になるSelectList</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns>ラジオボタンリストのHTML</returns>
        public static IHtmlString AreaButtonList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            const int lineItems = 9;
            var tbody = new StringBuilder();
            
            int i = 0;
            var table = new TagBuilder("table");
            table.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            tbody.Append("<tbody><tr>");
            foreach (var item in selectList)
            {
                var chkname = $"{name}{i}";
                var chkbox = htmlHelper.CheckBox($"{name}[{i}].IsCheck", item.Selected, new { @id = chkname, @class = "check-box" });

                // string chkbox = $"<input checked=\"checked\" class=\"check-box\"  name=\"AreaItem[{i}].IsCheck\" type=\"checkbox\" value=\"true\">";
                // input[hidden]
                var input = new TagBuilder("input");
                input.MergeAttribute("type", "hidden");
                input.MergeAttribute("value", "false");

                var span = new TagBuilder("sapn");
                span.AddCssClass("chk_parts");

                var label = new TagBuilder("label");
                label.AddCssClass("checkboxgroup");
                label.InnerHtml = chkbox.ToString() + span.ToString(TagRenderMode.Normal);

                // label.InnerHtml = chkbox.ToString() + input.ToString() + span.ToString(TagRenderMode.Normal);

                // checkbox
                var td = new TagBuilder("td");
                td.AddCssClass("w_checkbox_min");
                td.InnerHtml = label.ToString();
                tbody.Append(td.ToString(TagRenderMode.Normal));

                // input[hidden]
                input = new TagBuilder("input");
                input.MergeAttribute("type", "hidden");
                input.MergeAttribute("value", item.Value);

                // input.MergeAttribute("name", "AreaId[" + i + "]");
                input.MergeAttribute("name", $"{name}[{i}].AreaId");

                // checkbox Text
                td = new TagBuilder("td")
                {
                    InnerHtml = item.Text + input.ToString()
                };

                tbody.Append(td.ToString(TagRenderMode.Normal));

                i++;
                if (i % lineItems == 0)
                {
                    tbody.Append("</tr><tr>");
                }
            }

            tbody.Append("</tr></tbody>");
            table.InnerHtml = tbody.ToString();

            return MvcHtmlString.Create(table.ToString());
        }
    }
}