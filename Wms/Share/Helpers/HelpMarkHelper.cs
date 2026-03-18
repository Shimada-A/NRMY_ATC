namespace Share.Helpers
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// ヘルプマーク
    /// </summary>
    public static class HelpMarkHelper
    {
        /// <summary>
        /// ヘルプを表す HTML span 要素を返します。
        /// </summary>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="message">ツールチップに表示する補足情報。</param>
        /// <returns>ヘルプを表す HTML span 要素</returns>
        public static IHtmlString HelpMark(this HtmlHelper helper, string message)
        {
            var span = new TagBuilder("span");
            span.AddCssClass("help-mark");
            span.Attributes.Add("title", message);
            span.SetInnerText("?");
            return MvcHtmlString.Create(span.ToString(TagRenderMode.Normal));
        }
    }
}