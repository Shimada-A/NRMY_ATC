namespace Share.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class RadioButtonHelper
    {
        /// <summary>
        /// ラジオボタンリストのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <param name="listItems">RadioButtonの元になるSelectList</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns>ラジオボタンリストのHTML</returns>
        public static IHtmlString RadioButtonList<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> listItems, object htmlAttributes)
        {
            // name属性の値
            var name = ExpressionHelper.GetExpressionText(expression);
            var fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            var result = new StringBuilder();

            int i = 1;
            foreach (var item in listItems)
            {
                var id = $"{fullName}-{i++}";
                var radio = htmlHelper.RadioButton(fullName, item.Value, item.Selected, new { id, @class = "radio_input" });

                var span = new TagBuilder("sapn");
                span.AddCssClass("radio_parts");
                span.SetInnerText(item.Text);

                var label = new TagBuilder("label");
                label.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
                label.InnerHtml = radio.ToString() + span.ToString(TagRenderMode.Normal);

                result.Append(label.ToString(TagRenderMode.Normal));
            }

            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// SelectListからラジオボタンリストのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns>ラジオボタンリストのHTML</returns>
        public static IHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return RadioButtonListFor(htmlHelper, expression, null);
        }

        /// <summary>
        /// SelectListからラジオボタンリストのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns>ラジオボタンリストのHTML</returns>
        public static IHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var listItems = (IEnumerable<SelectListItem>)htmlHelper.ViewData.Eval(name);
            return RadioButtonList(htmlHelper, expression, listItems, htmlAttributes);
        }

        /// <summary>
        /// EnumからラジオボタンリストのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <returns>ラジオボタンリストのHTML</returns>
        public static IHtmlString EnumRadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
            where TProperty : struct
        {
            return EnumRadioButtonListFor(htmlHelper, expression, null);
        }

        /// <summary>
        /// EnumからラジオボタンリストのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns>ラジオボタンリストのHTML</returns>
        public static IHtmlString EnumRadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
            where TProperty : struct
        {
            var selectList = EnumHelper.GetSelectList(typeof(TProperty));

            var model = (Enum)ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            if (model != null)
            {
                var value = model.GetHashCode().ToString();
                foreach (var s in selectList)
                {
                    if (s.Value == value)
                        s.Selected = true;
                }
            }

            return RadioButtonList(htmlHelper, expression, selectList, htmlAttributes);
        }

        // public static IHtmlString RadioButtonItemColorGroupFor<TModel, ItemColorGroupId>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, ItemColorGroupId>> expression, object htmlAttributes)
        // {
        //    //カラーグループの選択項目リスト
        //    var colorGroupList = ItemColorGroup.GetAll()
        //        .OrderBy(m => m.ItemColorGroupDisplayNo)
        //        .Select(m => new SelectListItem()
        //        {
        //            Value = m.ItemColorGroupId,
        //            Text = m.ItemColorGroupName
        //        });

        // return RadioButtonByDBFor<TModel, ItemColorGroupId>(htmlHelper, expression, colorGroupList.ToList(), htmlAttributes);
        // }
    }
}