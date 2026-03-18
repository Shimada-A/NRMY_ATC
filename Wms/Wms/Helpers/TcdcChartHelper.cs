namespace Wms.Helpers
{
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// 必須マーク
    /// </summary>
    public static class TcdcChartHelper
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
        public static IHtmlString TcdcChart(
            this HtmlHelper helper,
            string title,
            int tc,
            int dc,
            string mainFunction = "",
            string tcdcFunction = "")
        {
            var html = new StringBuilder();
            if (dc == 0)
            { //0 除算エラーの対応
                html.AppendLine($"<div class='tcdc_chart'>");
                html.AppendLine($"<span class='tcdc_chart_title'>" + title + "</span>");
                html.AppendLine($"<table class='tcdc_chart_body'>");
                html.AppendLine($"<tbody><tr> ");
                html.AppendLine($"<td class='tcdc_chart_td_percent'>" + 0 + "%</td>");
                html.AppendLine($"<td class='tcdc_chart_td_tcdcval'>" + tc.ToString("N0") + "/" + 0 + "</td>");
                html.AppendLine($"</tr><tr><td></td><td> ");
                html.AppendLine($"<div class='tcdc_chart_td_chart'>");
                html.AppendLine($"<div class='progress-bar' role='progressbar' aria-valuenow='87' aria-valuemin='0' aria-valuemax='100' style='width:" + 0 + "%'>");
                html.AppendLine($"<span class='sr-only'>" + 0 + "% Complete</span>");
                html.AppendLine($"</div></div></td></tr></tbody></table></div> ");
            }
            else
            {
                html.AppendLine($"<div onclick=" + mainFunction + " class='tcdc_chart'>");
                html.AppendLine($"<span class='tcdc_chart_title'>" + title + "</span>");
                html.AppendLine($"<table class='tcdc_chart_body'>");
                html.AppendLine($"<tbody><tr> ");
                html.AppendLine($"<td class='tcdc_chart_td_percent'>" + Math.Truncate(Math.Round((decimal)tc * 100 / dc, 2)) + "%</td>");
                html.AppendLine($"<td class='tcdc_chart_td_tcdcval'>" + tc.ToString("N0") + "/" + dc.ToString("N0") + "</td>");
                html.AppendLine($"</tr><tr><td></td><td> ");
                html.AppendLine($"<div class='tcdc_chart_td_chart'>");
                html.AppendLine($"<div class='progress-bar' role='progressbar' aria-valuenow='87' aria-valuemin='0' aria-valuemax='100' style='width:" + Math.Ceiling(Math.Round((decimal)tc * 100 / dc, 2)) + "%'>");
                html.AppendLine($"<span class='sr-only'>" + Math.Truncate(Math.Round((decimal)tc * 100 / dc, 2)) + "% Complete</span>");
                html.AppendLine($"</div></div></td></tr></tbody></table></div> ");
            }


            return MvcHtmlString.Create(html.ToString());
        }
    }
}