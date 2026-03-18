namespace Share.Helpers
{
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Share.Common.Resources;

    /// <summary>
    /// ダイアログヘルパー
    /// </summary>
    public static class DialogHelper
    {
        /// <summary>
        /// OKダイアログヘルパー
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper自身</param>
        /// <param name="modalId">modalのid</param>
        /// <param name="title">メッセージタイトル</param>
        /// <param name="message">メッセージ内容</param>
        /// <returns>modalのHTML</returns>
        public static IHtmlString DialogOK(this HtmlHelper htmlHelper, string modalId, string title, string message)
        {
            return Dialog(htmlHelper, modalId, title, message, DialogButton.OK);
        }

        /// <summary>
        /// OK Cancelダイアログヘルパー
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper自身</param>
        /// <param name="modalId">modalのid</param>
        /// <param name="title">メッセージタイトル</param>
        /// <param name="message">メッセージ内容</param>
        /// <returns>modalのHTML</returns>
        public static IHtmlString DialogOKCancel(this HtmlHelper htmlHelper, string modalId, string title, string message)
        {
            return Dialog(htmlHelper, modalId, title, message, DialogButton.OKCancel);
        }

        /// <summary>
        /// ダイアログヘルパー
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper自身</param>
        /// <param name="modalId">modalのid</param>
        /// <param name="title">メッセージタイトル</param>
        /// <param name="message">メッセージ内容</param>
        /// <param name="dialogButton">ボタンの表示を切り替える</param>
        /// <returns>modalのHTML</returns>
        public static IHtmlString Dialog(this HtmlHelper htmlHelper, string modalId, string title, string message, DialogButton dialogButton)
        {
            // ヘッダー
            var divHeader = new TagBuilder("div");
            divHeader.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "modal-header" }));
            var sbHeader = new StringBuilder();
            sbHeader.Append("<button type=\"button\" class=\"close\" data-dismiss=\"modal\"><span>×</span></button>");
            sbHeader.Append("<h4 class=\"modal-title\">");
            sbHeader.Append(title);
            sbHeader.Append("</h4>");
            divHeader.InnerHtml = sbHeader.ToString();

            // メッセージ
            var divBody = new TagBuilder("div");
            divBody.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "modal-body" }));
            var sbBody = new StringBuilder();
            sbBody.Append(message);
            divBody.InnerHtml = sbBody.ToString();

            // フッター
            var divFooter = new TagBuilder("div");
            divFooter.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "modal-footer" }));
            var sbFooter = new StringBuilder();
            if (dialogButton == DialogButton.OK)
            {
                sbFooter.Append("<button type=\"button\" class=\"btn btn-default btn-ok\" data-dismiss=\"modal\">");
                sbFooter.Append(FormsResource.BTN_OK);
                sbFooter.Append("</button>");
            }
            else if (dialogButton == DialogButton.OKCancel)
            {
                sbFooter.Append("<button type=\"button\" class=\"btn btn-default btn-cancel\" data-dismiss=\"modal\">");
                sbFooter.Append(FormsResource.BTN_CANCEL);
                sbFooter.Append("</button>");
                sbFooter.Append("<button type=\"button\" class=\"btn btn-primary btn-ok\">");
                sbFooter.Append(FormsResource.BTN_OK);
                sbFooter.Append("</button>");
            }

            divFooter.InnerHtml = sbFooter.ToString();

            var divConent = new TagBuilder("div");
            divConent.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "modal-content" }));
            divConent.InnerHtml = divHeader.ToString() + divBody.ToString() + divFooter.ToString();

            var divDialog = new TagBuilder("div");
            divDialog.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "modal-dialog" }));
            divDialog.InnerHtml = divConent.ToString();

            var divModal = new TagBuilder("div");
            divModal.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new { @class = "modal fade", id = modalId, tabindex = "-1" }));
            divModal.InnerHtml = divDialog.ToString();

            return MvcHtmlString.Create(divModal.ToString());
        }

        /// <summary>
        /// ダイアログボタンの種類
        /// </summary>
        public enum DialogButton
        {
            OK,
            OKCancel,
        }
    }
}