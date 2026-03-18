namespace Wms.Areas.Stock.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Stock.Resources;
    using Wms.Models;

    /// <summary>
    /// 在庫照会ワーク01
    /// </summary>
    [Table("WW_STK_STOCKS01")]
    public partial class StkStock01 : BaseModel
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
        [Display(Name = nameof(StkStock01Resource.Seq), ResourceType = typeof(StkStock01Resource))]
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
        [Display(Name = nameof(StkStock01Resource.LineNo), ResourceType = typeof(StkStock01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.IsCheck), ResourceType = typeof(StkStock01Resource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.CenterId), ResourceType = typeof(StkStock01Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ReferenceResource.CenterName), ResourceType = typeof(ReferenceResource))]
        public string CenterName { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.LocationCd), ResourceType = typeof(StkStock01Resource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.BoxNo), ResourceType = typeof(StkStock01Resource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 事業部
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.DivisionId), ResourceType = typeof(StkStock01Resource))]
        public string DivisionId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId1), ResourceType = typeof(Resources.ReferenceResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.CategoryId1), ResourceType = typeof(StkStock01Resource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// カテゴリ２名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.CategoryName2), ResourceType = typeof(StkStock01Resource))]
        public string CategoryName2 { get; set; }

        /// <summary>
        /// カテゴリ３名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.CategoryName3), ResourceType = typeof(StkStock01Resource))]
        public string CategoryName3 { get; set; }

        /// <summary>
        /// カテゴリ４名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.CategoryName4), ResourceType = typeof(StkStock01Resource))]
        public string CategoryName4 { get; set; }

        /// <summary>
        /// ｼｰｽﾞﾝ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.ItemSeasonName), ResourceType = typeof(StkStock01Resource))]
        public string ItemSeasonName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.ItemSkuId), ResourceType = typeof(StkStock01Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.ItemId), ResourceType = typeof(StkStock01Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.ItemName), ResourceType = typeof(StkStock01Resource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.ItemColorId), ResourceType = typeof(StkStock01Resource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.ItemColorName), ResourceType = typeof(StkStock01Resource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.ItemSizeId), ResourceType = typeof(StkStock01Resource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.ItemSizeName), ResourceType = typeof(StkStock01Resource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 上代単価(税抜)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.NormalSellingPriceExTax), ResourceType = typeof(StkStock01Resource))]
        public int? NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.Jan), ResourceType = typeof(StkStock01Resource))]
        public string Jan { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.GradeId), ResourceType = typeof(StkStock01Resource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.GradeName), ResourceType = typeof(StkStock01Resource))]
        public string GradeName { get; set; }

        /// <summary>
        /// ケース数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.CaseQty), ResourceType = typeof(StkStock01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CaseQty { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.StockQty), ResourceType = typeof(StkStock01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQty { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.AllocQty), ResourceType = typeof(StkStock01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkStock01Resource.UnAllocQty), ResourceType = typeof(StkStock01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? UnAllocQty { get; set; }

        #endregion プロパティ
    }
}