namespace Wms.Areas.Arrival.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Arrival.Resources;
    using Wms.Models;

    /// <summary>
    /// 仕入入荷進捗照会ワーク02
    /// </summary>
    [Table("WW_ARR_PUR_REF02")]
    public partial class ArrPurRef02 : BaseModel
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
        [Display(Name = nameof(ArrPurRef02Resource.Seq), ResourceType = typeof(ArrPurRef02Resource))]
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
        [Display(Name = nameof(ArrPurRef02Resource.LineNo), ResourceType = typeof(ArrPurRef02Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(ArrPurRef02Resource.CenterId), ResourceType = typeof(ArrPurRef02Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ArrivePlanDate), ResourceType = typeof(ArrPurRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.Vendor), ResourceType = typeof(ArrPurRef02Resource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.Vendor), ResourceType = typeof(ArrPurRef02Resource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.InvoiceNo), ResourceType = typeof(ArrPurRef02Resource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.InvoiceSeq), ResourceType = typeof(ArrPurRef02Resource))]
        public long InvoiceSeq { get; set; }

        /// <summary>
        /// 発注番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.PoNo), ResourceType = typeof(ArrPurRef02Resource))]
        public string PoId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.CategoryName1), ResourceType = typeof(ArrPurRef02Resource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ItemId), ResourceType = typeof(ArrPurRef02Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ItemName), ResourceType = typeof(ArrPurRef02Resource))]
        public string ItemName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ItemSkuId), ResourceType = typeof(ArrPurRef02Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ItemColor), ResourceType = typeof(ArrPurRef02Resource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ItemColor), ResourceType = typeof(ArrPurRef02Resource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ItemSize), ResourceType = typeof(ArrPurRef02Resource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ItemSize), ResourceType = typeof(ArrPurRef02Resource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.Jan), ResourceType = typeof(ArrPurRef02Resource))]
        public string Jan { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.BoxNo), ResourceType = typeof(ArrPurRef02Resource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ArrivePlanQty), ResourceType = typeof(ArrPurRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ResultQty), ResourceType = typeof(ArrPurRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.DifferenceQty), ResourceType = typeof(ArrPurRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ArrivalStatus), ResourceType = typeof(ArrPurRef02Resource))]
        public string ArrivalStatus { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.ArrivalStatusName), ResourceType = typeof(ArrPurRef02Resource))]
        public string ArrivalStatusName { get; set; }

        /// <summary>
        /// TC指示数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.InvoicePlanQty), ResourceType = typeof(ArrPurRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? WmsInstructQty { get; set; }

        /// <summary>
        /// 格納予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef02Resource.StoragePlanQty), ResourceType = typeof(ArrPurRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StoragePlanQty { get; set; }

        #endregion プロパティ
    }
}