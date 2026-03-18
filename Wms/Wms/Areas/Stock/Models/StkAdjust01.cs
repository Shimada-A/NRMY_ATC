namespace Wms.Areas.Stock.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Stock.Resources;
    using Wms.Models;

    /// <summary>
    /// 在庫調整ワーク01
    /// </summary>
    [Table("WW_STK_ADJUST01")]
    public partial class StkAdjust01 : BaseModel
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
        [Display(Name = nameof(StkAdjust01Resource.Seq), ResourceType = typeof(StkAdjust01Resource))]
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
        [Display(Name = nameof(StkAdjust01Resource.LineNo), ResourceType = typeof(StkAdjust01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.CenterId), ResourceType = typeof(StkAdjust01Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.CenterName), ResourceType = typeof(StkAdjust01Resource))]
        public string CenterName { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.LocationCd), ResourceType = typeof(StkAdjust01Resource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.CategoryId1), ResourceType = typeof(StkAdjust01Resource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.CategoryId1), ResourceType = typeof(StkAdjust01Resource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.ItemSkuId), ResourceType = typeof(StkAdjust01Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.ItemId), ResourceType = typeof(StkAdjust01Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.ItemName), ResourceType = typeof(StkAdjust01Resource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.ItemColorId), ResourceType = typeof(StkAdjust01Resource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.ItemColorName), ResourceType = typeof(StkAdjust01Resource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.ItemSizeId), ResourceType = typeof(StkAdjust01Resource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.ItemSizeName), ResourceType = typeof(StkAdjust01Resource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.Jan), ResourceType = typeof(StkAdjust01Resource))]
        public string Jan { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.GradeId), ResourceType = typeof(StkAdjust01Resource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.GradeName), ResourceType = typeof(StkAdjust01Resource))]
        public string GradeName { get; set; }

        /// <summary>
        /// ケース数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.CaseQty), ResourceType = typeof(StkAdjust01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CaseQty { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.StockQty), ResourceType = typeof(StkAdjust01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust01Resource.AllocQty), ResourceType = typeof(StkAdjust01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        #endregion プロパティ
    }
}