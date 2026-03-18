namespace Wms.Areas.Stock.ViewModels.Reference
{
    using System.ComponentModel.DataAnnotations;

    public class ReferenceReportPackage
    {
        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CenterId), ResourceType = typeof(Resources.ReferenceResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.LocationCd), ResourceType = typeof(Resources.ReferenceResource), Order = 2)]
        public string LocationCd { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Locsec1), ResourceType = typeof(Resources.ReferenceResource), Order = 3)]
        public string Locsec1 { get; set; }

        /// <summary>
        /// 棚列
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Locsec2), ResourceType = typeof(Resources.ReferenceResource), Order = 4)]
        public string Locsec2 { get; set; }

        /// <summary>
        /// 棚番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Locsec3), ResourceType = typeof(Resources.ReferenceResource), Order = 5)]
        public string Locsec3 { get; set; }

        /// <summary>
        /// 段
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Locsec4), ResourceType = typeof(Resources.ReferenceResource), Order = 6)]
        public string Locsec4 { get; set; }

        /// <summary>
        /// 間口
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Locsec5), ResourceType = typeof(Resources.ReferenceResource), Order = 7)]
        public string Locsec5 { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.BoxNo), ResourceType = typeof(Resources.ReferenceResource), Order = 8)]
        public string BoxNo { get; set; }

        /// <summary>
        /// 部門
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryName1), ResourceType = typeof(Resources.ReferenceResource), Order = 9)]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品種
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryName2), ResourceType = typeof(Resources.ReferenceResource), Order = 10)]
        public string CategoryName2 { get; set; }

        /// <summary>
        /// アイテム
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemCode), ResourceType = typeof(Resources.ReferenceResource), Order = 11)]
        public string ItemCodeName { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.SeasonYear), ResourceType = typeof(Resources.ReferenceResource), Order = 12)]
        public string ItemSeasonYear { get; set; }

        /// <summary>
        /// ｼｰｽﾞﾝ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSeasonName), ResourceType = typeof(Resources.ReferenceResource), Order = 13)]
        public string ItemSeasonName { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Brand), ResourceType = typeof(Resources.ReferenceResource), Order = 14)]
        public string BrandName { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemId), ResourceType = typeof(Resources.ReferenceResource), Order = 15)]
        public string ItemId { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemIdName), ResourceType = typeof(Resources.ReferenceResource), Order = 16)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColor), ResourceType = typeof(Resources.ReferenceResource), Order = 17)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColor), ResourceType = typeof(Resources.ReferenceResource), Order = 18)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSize), ResourceType = typeof(Resources.ReferenceResource), Order = 19)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSize), ResourceType = typeof(Resources.ReferenceResource), Order = 20)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.StockQty), ResourceType = typeof(Resources.ReferenceResource), Order = 21)]
        public int? StockQty { get; set; }

        /// <summary>
        /// 標準上代(税抜)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.NormalSellingPriceExTax), ResourceType = typeof(Resources.ReferenceResource), Order = 22)]
        public int? NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Price), ResourceType = typeof(Resources.ReferenceResource), Order = 23)]
        public long? Price { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Jan), ResourceType = typeof(Resources.ReferenceResource), Order = 24)]
        public string Jan { get; set; }

        /// <summary>
        /// 納品書No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InvoiceNo), ResourceType = typeof(Resources.ReferenceResource), Order = 25)]
        public string InvoiceNo { get; set; }

        ///// <summary>
        ///// 引当数
        ///// </summary>
        ///// <remarks>
        //[Display(Name = nameof(Resources.ReferenceResource.AllocQty), ResourceType = typeof(Resources.ReferenceResource), Order = 26)]
        //public int? AllocQty { get; set; }

        ///// <summary>
        ///// 未引当数
        ///// </summary>
        ///// <remarks>
        //[Display(Name = nameof(Resources.ReferenceResource.UnAllocQty), ResourceType = typeof(Resources.ReferenceResource), Order = 27)]
        //public int? UnAllocQty { get; set; }
    }
}