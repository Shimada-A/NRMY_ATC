namespace Share.Helpers
{
    using System;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Share.Common.Resources;

    public static class RangeSliderHelper
    {
        /// <summary>
        /// Range SliderのHTMLを返します。
        /// </summary>
        /// <typeparam name="TModel">モデルの型。</typeparam>
        /// <typeparam name="TProperty">値の型。</typeparam>
        /// <param name="helper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="start">販売価格(下限)のプロパティを格納しているオブジェクトを識別する式。</param>
        /// <param name="end">販売価格(上限)のプロパティを格納しているオブジェクトを識別する式。</param>
        /// <param name="min">RangeSliderの下限</param>
        /// <param name="max">RangeSliderの上限</param>
        /// <param name="dialogId">RangeSliderDialogのHTMLのid属性に設定する値</param>
        /// <returns>RangeSliderのHTML</returns>
        public static IHtmlString RangeSliderFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> start,
            Expression<Func<TModel, TProperty>> end,
            int min,
            int max,
            string dialogId)
        {
            // テキストボックスの中に表示する矢印
            var arrowMark = new TagBuilder("span");
            arrowMark.AddCssClass("arrow");

            // 下限のテキストボックス
            var startText = helper.TextBoxFor(start, new
            {
                @class = "rslider slider_start",
                data_dialog = dialogId,
                data_min = min,
                placeholder = CommonsResource.None
            }).ToString();
            var startSpan = new TagBuilder("span");
            startSpan.AddCssClass("rs_input");
            startSpan.InnerHtml = startText.ToString() + arrowMark.ToString();

            // 上限のテキストボックス
            var endText = helper.TextBoxFor(end, new
            {
                @class = "rslider slider_end",
                data_dialog = dialogId,
                data_max = max,
                placeholder = CommonsResource.None
            }).ToString();
            var endSpan = new TagBuilder("span");
            endSpan.AddCssClass("rs_input");
            endSpan.InnerHtml = endText.ToString() + arrowMark.ToString();

            // 2つのテキストボックスの間に表示するハイフン
            var dash = new TagBuilder("span");
            dash.SetInnerText(" - ");

            return MvcHtmlString.Create(
                startSpan.ToString() +
                dash.ToString() +
                endSpan.ToString() +
                SliderDialog(dialogId));
        }

        /// <summary>
        /// Range SliderのモーダルダイアログのHTMLを返します。
        /// </summary>
        /// <param name="id">モーダルダイアログを表すHTMLのid属性の値</param>
        /// <returns>Range SliderのモーダルダイアログのHTML</returns>
        private static string SliderDialog(string id)
        {
            // RangeSlider
            var slider = new TagBuilder("input");
            slider.AddCssClass("slider");
            slider.MergeAttribute("type", "text");

            // 間隔調整のdiv
            var margin = new TagBuilder("div");
            margin.AddCssClass("mg50");
            margin.InnerHtml = slider.ToString(TagRenderMode.SelfClosing);

            // RangeSliderを格納するdiv
            var dialog = new TagBuilder("div");
            dialog.MergeAttribute("id", id);
            dialog.AddCssClass("slider_dialog");
            dialog.InnerHtml = margin.ToString();

            return dialog.ToString();
        }
    }
}