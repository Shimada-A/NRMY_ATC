namespace Wms.Areas.Move.ViewModels.InputTransfer
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
        [Display(Name = nameof(Resources.InputTransferResource.Center), ResourceType = typeof(Resources.InputTransferResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// 印刷日
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.PrintDate), ResourceType = typeof(Resources.InputTransferResource), Order = 2)]
        public string PrintDate { get; set; }

        /// <summary>
        /// 取引先
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.Customer), ResourceType = typeof(Resources.InputTransferResource), Order = 3)]
        public string Customer { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.ItemCd), ResourceType = typeof(Resources.InputTransferResource), Order = 4)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.ItemName), ResourceType = typeof(Resources.InputTransferResource), Order = 5)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.Color), ResourceType = typeof(Resources.InputTransferResource), Order = 6)]
        public string Color { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.Size), ResourceType = typeof(Resources.InputTransferResource), Order = 7)]
        public string Size { get; set; }

        /// <summary>
        /// JANコード
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.Jan), ResourceType = typeof(Resources.InputTransferResource), Order = 8)]
        public string Jan1 { get; set; }

        /// <summary>
        /// JANコード
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.Jan), ResourceType = typeof(Resources.InputTransferResource), Order = 9)]
        public string Jan2 { get; set; }

        /// <summary>
        /// ラベル番号バーコード
        /// </summary>
        [Display(Name = nameof(Resources.InputTransferResource.LabelNumberBarcode), ResourceType = typeof(Resources.InputTransferResource), Order = 10)]
        public string LabelNumberBarcode { get; set; }
    }
}