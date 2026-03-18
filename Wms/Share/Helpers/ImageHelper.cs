namespace Share.Helpers
{
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;

    public static class ImageHelper
    {
        /// <summary>
        /// 商品画像のHTMLを返します。
        /// </summary>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="url">S3のパス(バケット名を除く)</param>
        /// <param name="htmlAttributes">この要素に設定する HTML 属性を格納するオブジェクト。</param>
        /// <returns></returns>
        public static IHtmlString ItemImage(this HtmlHelper helper, string url, object htmlAttributes)
        {
            var img = new TagBuilder("img");

            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var noImage = urlHelper.Content("~/Content/i3DESIGN/img/common/No_Image.png");
            img.MergeAttribute("onerror", $"this.src='{noImage}'");

            var domain = ConfigurationManager.AppSettings["ItemImageDns"].ToString();

            string src;
            if (string.IsNullOrWhiteSpace(url))
            {
                src = noImage;
            }
            else
            {
                src = VirtualPathUtility.AppendTrailingSlash(domain) + url;
            }

            img.MergeAttribute("src", src);

            img.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
        }
    }
}