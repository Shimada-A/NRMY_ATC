namespace Wms.Areas.Stock.ViewModels.Reference
{
    using System.ComponentModel.DataAnnotations;

    public class StockReport
    {
        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Center), ResourceType = typeof(Resources.ReferenceResource), Order = 1)]
        public string Center { get; set; }
        /// <summary>
        /// エリア
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Locsec_1), ResourceType = typeof(Resources.ReferenceResource), Order = 2)]
        public string Locsec_1 { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(Resources.ReferenceResource.PrintUser), ResourceType = typeof(Resources.ReferenceResource), Order = 3)]
        public string PrintUser { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.LocationCd), ResourceType = typeof(Resources.ReferenceResource), Order = 4)]
        public string LocationCd { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId), ResourceType = typeof(Resources.ReferenceResource), Order = 5)]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryName1), ResourceType = typeof(Resources.ReferenceResource), Order = 6)]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemId), ResourceType = typeof(Resources.ReferenceResource), Order = 7)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemName), ResourceType = typeof(Resources.ReferenceResource), Order = 8)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColorId), ResourceType = typeof(Resources.ReferenceResource), Order = 9)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColorName), ResourceType = typeof(Resources.ReferenceResource), Order = 10)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSizeId), ResourceType = typeof(Resources.ReferenceResource), Order = 11)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSizeName), ResourceType = typeof(Resources.ReferenceResource), Order = 12)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.GradeName), ResourceType = typeof(Resources.ReferenceResource), Order = 15)]
        public string GradeName { get; set; }

        /// <summary>
        /// ケース数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CaseQty), ResourceType = typeof(Resources.ReferenceResource), Order = 16)]
        public int? CaseQty { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.StockQty), ResourceType = typeof(Resources.ReferenceResource), Order = 17)]
        public int? StockQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.AllocQty), ResourceType = typeof(Resources.ReferenceResource), Order = 18)]
        public int? AllocQty { get; set; }

        /// <summary>
        /// JANコード
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Jan), ResourceType = typeof(Resources.ReferenceResource), Order = 13)]
        public string Jan { get; set; }

        /// <summary>
        /// ケース明細フラグ
        /// </summary>
        [Display(Name = nameof(Resources.ReferenceResource.PackageStockReportFlg), ResourceType = typeof(Resources.ReferenceResource), Order = 19)]
        public int PackageStockReportFlg { get; set; }

        /// <summary>
        /// JAN入り明細フラグ
        /// </summary>
        [Display(Name = nameof(Resources.ReferenceResource.DetailJanFlag), ResourceType = typeof(Resources.ReferenceResource), Order = 19)]
        public int DetailJanFlg { get; set; }
    }
}