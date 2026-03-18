namespace Share.Helpers
{
    using System.Web;
    using System.Web.Mvc;
    using Share.Common.Resources;

    /// <summary>
    /// 必須マーク
    /// </summary>
    public static class RequiredMarkHelper
    {
        /// <summary>
        /// 必須を表す HTML span 要素を返します。
        /// </summary>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns>必須を表す HTML span 要素。</returns>
        public static IHtmlString RequiredMark(this HtmlHelper helper)
        {
            return RequiredMark(helper, null);
        }

        /// <summary>
        /// 必須を表す HTML span 要素を返します。
        /// </summary>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns>必須を表す HTML span 要素。</returns>
        public static IHtmlString RequiredMark(this HtmlHelper helper, object htmlAttributes)
        {
            var span = new TagBuilder("span");
            span.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            span.AddCssClass("notnull");
            span.SetInnerText(CommonsResource.Required);
            return MvcHtmlString.Create(span.ToString(TagRenderMode.Normal));
        }
    }
}