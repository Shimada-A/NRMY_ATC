namespace Wms.Areas.Ship.ViewModels.PrintCaseLabel
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// CSV
    /// </summary>
    public class PrintCaseLabelResult
    {
        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.Center), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// 出荷先区分
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.ShipToStoreClass), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 2)]
        public string ShipToStoreClass { get; set; }

        /// <summary>
        /// 出荷先店舗ID
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.ShipToStoreId), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 3)]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先店舗略称
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.ShipToStoreName), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 4)]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.LabelCaseName), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 5)]
        public string LabelCaseNo { get; set; }

        /// <summary>
        /// 配送業者名
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.TransporterName), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 6)]
        public string TransporterName { get; set; }

        /// <summary>
        /// 間口No
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.FrontageNo), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 7)]
        public string FrontageNo { get; set; }

        /// <summary>
        /// ブランド名
        /// </summary>
        [Display(Name = nameof(Resources.PrintCaseLabelResource.BrandName), ResourceType = typeof(Resources.PrintCaseLabelResource), Order = 8)]
        public string BrandName { get; set; }

    }
}