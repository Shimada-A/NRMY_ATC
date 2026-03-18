namespace Share.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc.Ajax;
    using PagedList;
    using PagedList.Mvc;

    public static class PagerHelper
    {
        /// <summary>
        /// ページネーションのHTMLを返します。
        /// </summary>
        /// <typeparam name="T">リストの型</typeparam>
        /// <param name="htmlHelper">このメソッドによって拡張される HTML ヘルパー インスタンス。</param>
        /// <param name="list">ページネーションの対象データ</param>
        /// <param name="pageNumber">表示するページ番号</param>
        /// <param name="pageSize">1ページあたりのデータ数</param>
        /// <param name="totalItemCount">全ページのデータ数</param>
        /// <param name="generatePageUrl">リンクのURLパラメーター引数</param>
        /// <returns>ページネーションのHTML</returns>
        /// <remarks>
        /// ViewModelにIPagedListのプロパティを持たせるとModelBind時にインスタンス化できないため、
        /// プロパティはListに変更してPagedListPagerをラップしたこのヘルパーメソッドでStaticPagedListを作る。
        /// </remarks>
        public static IHtmlString ListPager<T>(
            this System.Web.Mvc.HtmlHelper htmlHelper,
            IList<T> list,
            int pageNumber,
            int pageSize,
            int totalItemCount,
            Func<int, string> generatePageUrl)
        {
            var pagedList = new StaticPagedList<T>(list, pageNumber, pageSize, totalItemCount);

            return htmlHelper.PagedListPager(pagedList, generatePageUrl, Default);
        }

        /// <summary>
        /// PagedListRenderOptionsの初期値を取得します。
        /// </summary>
        public static PagedListRenderOptions Default
        {
            get
            {
                return new PagedListRenderOptions()
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Never,
                    DisplayLinkToLastPage = PagedListDisplayMode.Never,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,

                    // 前後ボタンの文字はCSSで定義済
                    LinkToNextPageFormat = "&nbsp;",
                    LinkToPreviousPageFormat = "&nbsp;",
                    MaximumPageNumbersToDisplay = 5,
                    UlElementClasses = new[] { "paging" }
                };
            }
        }

        public static PagedListRenderOptions GetOptions(string updateTargetId, string onComplete)
        {
            var option = PagedListRenderOptions.EnableUnobtrusiveAjaxReplacing(new AjaxOptions()
            {
                UpdateTargetId = updateTargetId,
                OnComplete = onComplete,
            });

            option.DisplayLinkToFirstPage = PagedListDisplayMode.Never;
            option.DisplayLinkToLastPage = PagedListDisplayMode.Never;
            option.DisplayLinkToNextPage = PagedListDisplayMode.Always;
            option.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;

            // 前後ボタンの文字はCSSで定義済
            option.LinkToNextPageFormat = "&nbsp;";
            option.LinkToPreviousPageFormat = "&nbsp;";
            option.MaximumPageNumbersToDisplay = 5;
            option.UlElementClasses = new[] { "paging" };

            return option;
        }

        // public static MvcHtmlString PagedListPager(this System.Web.Mvc.HtmlHelper html, IPagedList list, Func<int, string> generatePageUrl);

        // public static MvcHtmlString PagedListPager(this System.Web.Mvc.HtmlHelper html, IPagedList list, Func<int, string> generatePageUrl, PagedListRenderOptions options);
    }
}