namespace Wms.Areas.Arrival.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Arrival.Resources;
    using Wms.Models;
    using Wms.Resources;

    /// <summary>
    /// 仕入入荷実績
    /// </summary>
    [Table("T_ARRIVE_RESULTS")]
    public partial class ArriveResult : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905：貝塚、0924：関東　など
        /// </remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.CenterId), ResourceType = typeof(ArriveResultResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 納品書番号 (INVOICE_NO)
        /// </summary>
        /// <remarks>
        /// 上位から連携される納品書番号。対応するTC出荷がある場合は同じ納品書番号がセットされる
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.InvoiceNo), ResourceType = typeof(ArriveResultResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 納品書行番号 (INVOICE_SEQ)
        /// </summary>
        /// <remarks>
        /// 上位から連携される納品書番号。対応するTC出荷がある場合は同じ納品書番号がセットされる
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.InvoiceSeq), ResourceType = typeof(ArriveResultResource))]
        [Range(-9999, 9999, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public int InvoiceSeq { get; set; }

        /// <summary>
        /// 仕入先ID (VENDOR_ID)
        /// </summary>
        /// <remarks>
        /// 仕入先所在ID
        /// </remarks>
        [Display(Name = nameof(ArriveResultResource.VendorId), ResourceType = typeof(ArriveResultResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string VendorId { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.ItemSkuId), ResourceType = typeof(ArriveResultResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.Jan), ResourceType = typeof(ArriveResultResource))]
        [MaxLength(13, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.ItemId), ResourceType = typeof(ArriveResultResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.ItemColorId), ResourceType = typeof(ArriveResultResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.ItemSizeId), ResourceType = typeof(ArriveResultResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// 入荷日 (ARRIVE_DATE)
        /// </summary>
        [Display(Name = nameof(ArriveResultResource.ArriveDate), ResourceType = typeof(ArriveResultResource))]
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// 実績数 (RESULT_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.ResultQty), ResourceType = typeof(ArriveResultResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public int ResultQty { get; set; }

        /// <summary>
        /// 入荷状況 (ARRIVAL_STATUS)
        /// </summary>
        /// <remarks>
        /// 2:一部入荷済 3:入荷済 4:入荷確定済 5:実績送信済
        /// (レコード無しが 1:未入荷 となる)
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.ArrivalStatus), ResourceType = typeof(ArriveResultResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public byte ArrivalStatus { get; set; }

        /// <summary>
        /// 実績確定日時 (CONFIRM_DATE)
        /// </summary>
        [Display(Name = nameof(ArriveResultResource.ConfirmDate), ResourceType = typeof(ArriveResultResource))]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 実績確定担当者ID (CONFIRM_USER_ID)
        /// </summary>
        [Display(Name = nameof(ArriveResultResource.ConfirmUserId), ResourceType = typeof(ArriveResultResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ConfirmUserId { get; set; }

        /// <summary>
        /// 連携状況 (IF_STATE)
        /// </summary>
        /// <remarks>
        /// 0:未設定
        /// 1:未送信 
        /// 2:送信済み
        /// 3:送信対象外
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.IfState), ResourceType = typeof(ArriveResultResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public byte IfState { get; set; }

        /// <summary>
        /// 連携処理中フラグ (SENDING_FLAG)
        /// </summary>
        /// <remarks>
        /// 1:処理中（送信中）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.SendingFlag), ResourceType = typeof(ArriveResultResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public byte SendingFlag { get; set; }

        /// <summary>
        /// 連携実行ID (IF_RUN_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArriveResultResource.IfRunId), ResourceType = typeof(ArriveResultResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public int IfRunId { get; set; }

        /// <summary>
        /// 連携日時 (IF_RUN_DATE)
        /// </summary>
        [Display(Name = nameof(ArriveResultResource.IfRunDate), ResourceType = typeof(ArriveResultResource))]
        public DateTimeOffset? IfRunDate { get; set; }

        #endregion
    }
}
