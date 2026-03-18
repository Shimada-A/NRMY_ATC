namespace Share.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    public static class NumberTextBoxHelper
    {
        public static IHtmlString NumberTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return NumberTextBoxFor(htmlHelper, expression, attr);
        }

        /// <summary>
        /// 数値のテキストボックスのHTMLを返します。（※EditorForから呼び出さないとAttributeが取れない。EditorForとこれを直接呼び出したときの両立が難しい。）
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns>数値のテキストボックスのHTML</returns>
        public static IHtmlString NumberTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            var attr = new RouteValueDictionary(htmlAttributes);
            AddCssClassForSpinner(ref attr);

            var modelMetadata = htmlHelper.ViewContext.ViewData.ModelMetadata;
            var props = modelMetadata.ContainerType?.GetProperties();
            var prop = props?.Where(x => x.Name == modelMetadata.PropertyName).SingleOrDefault();

            if (prop != null)
            {
                var range = Attribute.GetCustomAttribute(prop, typeof(RangeAttribute)) as RangeAttribute;
                AddRangeAttribute(range, ref attr);
            }

            return htmlHelper.TextBoxFor(expression, attr);
        }

        /// <summary>
        /// jQuery UIのSpinner用にCSSクラスを付与します。
        /// </summary>
        /// <param name="attr">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        private static void AddCssClassForSpinner(ref RouteValueDictionary attr)
        {
            var cssClass = attr["class"]?.ToString();
            if (cssClass == null || !cssClass.Contains("spinner"))
            {
                attr.Remove("class");
                attr.Add("class", $"spinner {cssClass}".TrimEnd());
            }
        }

        /// <summary>
        /// jQuery UIのSpinner用にRangeをデータとして付与します。
        /// </summary>
        /// <param name="range">プロパティに設定されたRange属性を格納するオブジェクト。</param>
        /// <param name="attr">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        private static void AddRangeAttribute(RangeAttribute range, ref RouteValueDictionary attr)
        {
            if (range != null)
            {
                attr.Add("data-min", range.Minimum);
                attr.Add("data-max", range.Maximum);
            }
        }
    }
}