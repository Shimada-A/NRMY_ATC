namespace Wms.Areas.Arrival.ViewModels.PrintCaseLabel
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// CSV
    /// </summary>
    public class PrintCaseLabelSql
    {
        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.Center), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// 印刷日
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.PrintDate), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 2)]
        public string PrintDate { get; set; }

        /// <summary>
        /// 取引先
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.Customer), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 3)]
        public string Customer { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.ItemCd), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 4)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.ItemName), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 5)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.Color), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 6)]
        public string Color { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.Size), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 7)]
        public string Size { get; set; }

        /// <summary>
        /// JANコード
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.Jan), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 8)]
        public string Jan1 { get; set; }

        /// <summary>
        /// JANコード
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.Jan), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 9)]
        public string Jan2 { get; set; }

        /// <summary>
        /// ラベル番号バーコード
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.LabelNumberBarcode), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 10)]
        public string LabelNumberBarcode { get; set; }
    }
}