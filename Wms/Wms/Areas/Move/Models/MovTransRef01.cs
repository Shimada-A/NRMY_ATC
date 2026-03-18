namespace Wms.Areas.Move.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Move.Resources;
    using Wms.Models;
    using Wms.Resources;

    /// <summary>
    /// 移動入荷進捗照会ワーク01
    /// </summary>
    [Table("WW_ARR_TRANS_REF01")]
    public partial class MovTransRef01 : BaseModel
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
        [Display(Name = nameof(MovTransRef01Resource.Seq), ResourceType = typeof(MovTransRef01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MovTransRef01Resource.LineNo), ResourceType = typeof(MovTransRef01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.CenterId), ResourceType = typeof(MovTransRef01Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 移動区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.TransferClass), ResourceType = typeof(MovTransRef01Resource))]
        public int? TransferClass { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        [Display(Name = nameof(MovTransRef01Resource.ArrivePlanDate), ResourceType = typeof(MovTransRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 移動元ID
        /// </summary>
        [Display(Name = nameof(MovTransRef01Resource.TransferFromStoreId), ResourceType = typeof(MovTransRef01Resource))]
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元名
        /// </summary>
        [Display(Name = nameof(MovTransRef01Resource.TransferFromStoreName), ResourceType = typeof(MovTransRef01Resource))]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.SlipNo), ResourceType = typeof(MovTransRef01Resource))]
        public string SlipNo { get; set; }

        /// <summary>
        /// 予定ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.PlanSlipNo), ResourceType = typeof(MovTransRef01Resource))]
        public string PlanSlipNo { get; set; }

        /// <summary>
        /// 実績ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.ResultSlipNo), ResourceType = typeof(MovTransRef01Resource))]
        public string ResultSlipNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.ItemSkuId), ResourceType = typeof(MovTransRef01Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [Display(Name = nameof(MovTransRef01Resource.ArrivePlanQty), ResourceType = typeof(MovTransRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [Display(Name = nameof(MovTransRef01Resource.ResultQty), ResourceType = typeof(MovTransRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        [Display(Name = nameof(MovTransRef01Resource.Status), ResourceType = typeof(MovTransRef01Resource))]
        public int? Status { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.ConfirmDate), ResourceType = typeof(MovTransRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 連携状況 (IF_STATE)
        /// </summary>
        /// <remarks>
        /// 0:未設定
        /// 1:未送信 
        /// 2:送信済み
        /// 3:送信対象外
        /// </remarks>
        [Display(Name = nameof(MovTransRef01Resource.IfState), ResourceType = typeof(MovTransRef01Resource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public byte? IfState { get; set; }

        /// <summary>
        /// 予定外
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput02Resource.UnplannedFlag), ResourceType = typeof(MovTransInput02Resource))]
        public int? UnplannedFlag { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.SlipDate), ResourceType = typeof(MovTransRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SlipDate { get; set; }

        #endregion プロパティ
    }
}