namespace Share.Helpers
{
    using System;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// カラー
    /// </summary>
    public static class ColorIconHelper
    {
        /// <summary>
        /// カラーアイコンのHTMLを返します。
        /// </summary>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="color">アイコンの色(HTMLカラーコード:#FFFFFFなど)</param>
        /// <returns>カラーアイコンのHTML</returns>
        public static IHtmlString ColorIcon(this HtmlHelper helper, string color)
        {
            var span = new TagBuilder("span");
            span.AddCssClass("color_icon");
            span.Attributes.Add("style", $"background-color: {color};");

            return MvcHtmlString.Create(span.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// カラーアイコンのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="expression">表示するプロパティを格納しているオブジェクトを識別する式。</param>
        /// <returns>カラーアイコンのHTML</returns>
        public static IHtmlString ColorIconFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var backColoer = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).Model?.ToString();
            return ColorIcon(helper, backColoer);
        }
    }
}