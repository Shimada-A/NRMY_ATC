namespace Share.Helpers
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Stepper
    /// </summary>
    public static class StepperHelper
    {
        /// <summary>
        /// Stepperを出力する
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="source">ステップとタイトルのリスト</param>
        /// <returns></returns>
        public static IHtmlString Stepper(this HtmlHelper helper, IEnumerable<StepperModel> source)
        {
            var lists = new StringBuilder();
            foreach (var s in source)
            {
                var span = new TagBuilder("span");
                span.AddCssClass("num");
                span.SetInnerText(s.Index.ToString());

                var li = new TagBuilder("li");
                if (s.IsActive == true)
                {
                    li.AddCssClass("actived");
                }

                li.InnerHtml = span.ToString(TagRenderMode.Normal) + s.Description;

                lists.AppendLine(li.ToString(TagRenderMode.Normal));
            }

            var ul = new TagBuilder("ul");
            ul.AddCssClass("stepper stepper1");
            ul.InnerHtml = lists.ToString();

            return MvcHtmlString.Create(ul.ToString(TagRenderMode.Normal));
        }
    }

    /// <summary>
    /// Stepperのモデル
    /// </summary>
    public class StepperModel
    {
        /// <summary>
        /// インデックス
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 説明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// アクティブかどうか
        /// </summary>
        public bool IsActive { get; set; } = false;
    }
}