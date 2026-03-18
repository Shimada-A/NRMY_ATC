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
    /// 移動入荷実績入力ワーク01
    /// </summary>
    [Table("WW_ARR_TRANS_INPUT01")]
    public partial class MovTransInput01 : BaseModel
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
        [Display(Name = nameof(MovTransInput01Resource.Seq), ResourceType = typeof(MovTransInput01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MovTransInput01Resource.LineNo), ResourceType = typeof(MovTransInput01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.CenterId), ResourceType = typeof(MovTransInput01Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 移動区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.TransferClass), ResourceType = typeof(MovTransInput01Resource))]
        public int? TransferClass { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        [Display(Name = nameof(MovTransInput01Resource.ArrivePlanDate), ResourceType = typeof(MovTransInput01Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 移動元ID
        /// </summary>
        [Display(Name = nameof(MovTransInput01Resource.TransferFromStoreId), ResourceType = typeof(MovTransInput01Resource))]
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元名
        /// </summary>
        [Display(Name = nameof(MovTransInput01Resource.TransferFromStoreName), ResourceType = typeof(MovTransInput01Resource))]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.SlipNo), ResourceType = typeof(MovTransInput01Resource))]
        public string SlipNo { get; set; }

        /// <summary>
        /// 予定ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.PlanSlipNo), ResourceType = typeof(MovTransInput01Resource))]
        public string PlanSlipNo { get; set; }

        /// <summary>
        /// 実績ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.ResultSlipNo), ResourceType = typeof(MovTransInput01Resource))]
        public string ResultSlipNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.ItemSkuId), ResourceType = typeof(MovTransInput01Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [Display(Name = nameof(MovTransInput01Resource.ArrivePlanQty), ResourceType = typeof(MovTransInput01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [Display(Name = nameof(MovTransInput01Resource.ResultQty), ResourceType = typeof(MovTransInput01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        [Display(Name = nameof(MovTransInput01Resource.Status), ResourceType = typeof(MovTransInput01Resource))]
        public int? Status { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.ConfirmDate), ResourceType = typeof(MovTransInput01Resource))]
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
        [Display(Name = nameof(MovTransInput01Resource.IfState), ResourceType = typeof(MovTransInput01Resource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public byte? IfState { get; set; }

        /// <summary>
        /// 予定外
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.UnplannedFlag), ResourceType = typeof(MovTransInput01Resource))]
        public int? UnplannedFlag { get; set; }

        /// <summary>
        /// 入荷実績日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.TransferResultDate), ResourceType = typeof(MovTransInput01Resource))]
        public DateTime? TransferResultDate { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.SlipDate), ResourceType = typeof(MovTransInput01Resource))]
        public DateTime? SlipDate { get; set; }

        /// <summary>
        /// 選択フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.SelectedFlag), ResourceType = typeof(MovTransInput01Resource))]
        public bool SelectedFlag { get; set; }

        /// <summary>
        /// 梱包番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.BoxNo), ResourceType = typeof(MovTransInput01Resource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.BrandId), ResourceType = typeof(MovTransInput01Resource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド略称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransInput01Resource.BrandShortName), ResourceType = typeof(MovTransInput01Resource))]
        public string BrandShortName { get; set; }

        #endregion プロパティ
    }
}