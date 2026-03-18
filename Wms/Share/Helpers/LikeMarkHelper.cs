namespace Share.Helpers
{
    using System.Web;
    using System.Web.Mvc;
    using Share.Common.Resources;

    /// <summary>
    /// 検索マーク
    /// </summary>
    public static class LikeMarkHelper
    {
        /// <summary>
        /// (前方一致)(あいまい検索)を表す HTML span 要素を返します。
        /// </summary>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="text">表示する文字。</param>
        /// <returns>(前方一致)(あいまい検索)を表す HTML span 要素。</returns>
        public static IHtmlString LikeMark(this HtmlHelper helper, string text)
        {
            return LikeMark(helper, text, null);
        }

        /// <summary>
        /// (前方一致)(あいまい検索)を表す HTML span 要素を返します。
        /// </summary>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="text">表示する文字。</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns>(前方一致)(あいまい検索)を表す HTML span 要素。</returns>
        public static IHtmlString LikeMark(this HtmlHelper helper, string text, object htmlAttributes)
        {
            var span = new TagBuilder("span");
            span.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            span.AddCssClass("like");
            span.SetInnerText(text);
            return MvcHtmlString.Create(span.ToString(TagRenderMode.Normal));
        }
    }
}