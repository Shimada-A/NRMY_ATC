namespace Wms.Areas.Stock.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Stock.Resources;
    using Wms.Models;

    /// <summary>
    /// 在庫調整ワーク02
    /// </summary>
    [Table("WW_STK_ADJUST02")]
    public partial class StkAdjust02 : BaseModel
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
        [Display(Name = nameof(StkAdjust02Resource.Seq), ResourceType = typeof(StkAdjust02Resource))]
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
        [Display(Name = nameof(StkAdjust02Resource.LineNo), ResourceType = typeof(StkAdjust02Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.CenterId), ResourceType = typeof(StkAdjust02Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.ItemSkuId), ResourceType = typeof(StkAdjust02Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.Jan), ResourceType = typeof(StkAdjust02Resource))]
        public string Jan { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.ItemId), ResourceType = typeof(StkAdjust02Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.ItemName), ResourceType = typeof(StkAdjust02Resource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.ItemColorId), ResourceType = typeof(StkAdjust02Resource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.ItemColorName), ResourceType = typeof(StkAdjust02Resource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.ItemSizeId), ResourceType = typeof(StkAdjust02Resource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.ItemSizeName), ResourceType = typeof(StkAdjust02Resource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.LocationCd), ResourceType = typeof(StkAdjust02Resource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// ロケーション区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.LocationClass), ResourceType = typeof(StkAdjust02Resource))]
        public string LocationClass { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.GradeId), ResourceType = typeof(Resources.StkAdjust02Resource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.GradeName), ResourceType = typeof(StkAdjust02Resource))]
        public string GradeName { get; set; }

        /// <summary>
        /// 在庫数（変更前）
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.StockQty), ResourceType = typeof(StkAdjust02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? BeforeStockQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.AllocQty), ResourceType = typeof(StkAdjust02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 未引当数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.UnAllocQty), ResourceType = typeof(StkAdjust02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? UnAllocQty { get { return this.StockQty - this.AllocQty; } }

        /// <summary>
        /// 荷姿
        /// </summary>
        [Display(Name = nameof(StkAdjust02Resource.CaseClass), ResourceType = typeof(StkAdjust02Resource))]
        public int? CaseClass { get; set; }

        /// <summary>
        /// 荷姿
        /// </summary>
        [Display(Name = nameof(StkAdjust02Resource.CaseClass), ResourceType = typeof(StkAdjust02Resource))]
        public string CaseClassName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        [Display(Name = nameof(StkAdjust02Resource.BoxNo), ResourceType = typeof(StkAdjust02Resource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.StockQty), ResourceType = typeof(StkAdjust02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQty { get; set; }

        /// <summary>
        /// 訂正後数
        /// </summary>
        //[Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StkAdjust02Resource.AdjustQtyTo), ResourceType = typeof(StkAdjust02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AdjustQtyTo { get; set; }

        /// <summary>
        /// 荷姿別在庫TBL.更新日時
        /// </summary>
        [Display(Name = nameof(StkAdjust02Resource.TpsUpdateDate), ResourceType = typeof(StkAdjust02Resource))]
        public DateTimeOffset TpsUpdateDate { get; set; }

        /// <summary>
        /// 荷姿別在庫TBL.更新ユーザID
        /// </summary>
        [Display(Name = nameof(StkAdjust02Resource.TpsUpdateUserId), ResourceType = typeof(StkAdjust02Resource))]
        public string TpsUpdateUserId { get; set; }

        /// <summary>
        /// 荷姿別在庫TBL.更新プログラム名
        /// </summary>
        [Display(Name = nameof(StkAdjust02Resource.TpsUpdateProgramName), ResourceType = typeof(StkAdjust02Resource))]
        public string TpsUpdateProgramName { get; set; }

        /// <summary>
        /// 荷姿別在庫TBL.更新回数
        /// </summary>
        [Display(Name = nameof(StkAdjust02Resource.TpsUpdateCount), ResourceType = typeof(StkAdjust02Resource))]
        public int TpsUpdateCount { get; set; } = 0;

        /// <summary>
        /// 納品書番号
        /// </summary>
        [Display(Name = nameof(StkAdjust02Resource.InvoiceNo), ResourceType = typeof(StkAdjust02Resource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 荷姿別在庫TBL.引当数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StkAdjust02Resource.AllocQty), ResourceType = typeof(StkAdjust02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? TpsAllocQty { get; set; }

        #endregion プロパティ
    }
}