namespace Wms.Areas.Stock.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Stock.Resources;
    using Wms.Models;

    /// <summary>
    /// 在庫照会ワーク02
    /// </summary>
    [Table("WW_STK_STOCKS02")]
    public partial class StkStock02 : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StkStock02Resource.Seq), ResourceType = typeof(StkStock02Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StkStock02Resource.LineNo), ResourceType = typeof(StkStock02Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.CenterId), ResourceType = typeof(StkStock02Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 倉庫名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.CenterName), ResourceType = typeof(StkStock02Resource))]
        public string CenterName { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.LocationCd), ResourceType = typeof(StkStock02Resource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.BoxNo), ResourceType = typeof(StkStock02Resource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 事業部
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.DivisionId), ResourceType = typeof(StkStock02Resource))]
        public string DivisionId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.CategoryId1), ResourceType = typeof(StkStock02Resource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.CategoryId1), ResourceType = typeof(StkStock02Resource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// カテゴリ２名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.CategoryName2), ResourceType = typeof(StkStock02Resource))]
        public string CategoryName2 { get; set; }

        /// <summary>
        /// カテゴリ３名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.CategoryName3), ResourceType = typeof(StkStock02Resource))]
        public string CategoryName3 { get; set; }

        /// <summary>
        /// カテゴリ４名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.CategoryName4), ResourceType = typeof(StkStock02Resource))]
        public string CategoryName4 { get; set; }

        /// <summary>
        /// ｼｰｽﾞﾝ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.ItemSeasonName), ResourceType = typeof(StkStock02Resource))]
        public string ItemSeasonName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.ItemSkuId), ResourceType = typeof(StkStock02Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.ItemId), ResourceType = typeof(StkStock02Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.ItemName), ResourceType = typeof(StkStock02Resource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.ItemColorId), ResourceType = typeof(StkStock02Resource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.ItemColorName), ResourceType = typeof(StkStock02Resource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.ItemSizeId), ResourceType = typeof(StkStock02Resource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.ItemSizeName), ResourceType = typeof(StkStock02Resource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 上代単価(税抜)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.NormalSellingPriceExTax), ResourceType = typeof(StkStock02Resource))]
        public int? NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.Jan), ResourceType = typeof(StkStock02Resource))]
        public string Jan { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.GradeId), ResourceType = typeof(StkStock02Resource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.GradeName), ResourceType = typeof(StkStock02Resource))]
        public string GradeName { get; set; }

        /// <summary>
        /// 納品書No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.InvoiceNo), ResourceType = typeof(StkStock02Resource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock02Resource.StockQty), ResourceType = typeof(StkStock02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQty { get; set; }

        #endregion プロパティ
    }
}