namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// EC出荷進捗照会ワーク
    /// </summary>
    [Table("WW_SHP_EC_CONFIRM_PROGRESS")]
    public partial class ShpEcConfirmProgress : BaseModel
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
        [Display(Name = nameof(EcConfirmProgressResource.Seq), ResourceType = typeof(EcConfirmProgressResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EcConfirmProgressResource.LineNo), ResourceType = typeof(EcConfirmProgressResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ (IS_CHECK)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EcConfirmProgressResource.IsCheck), ResourceType = typeof(EcConfirmProgressResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.CenterId), ResourceType = typeof(EcConfirmProgressResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日 (SHIP_PLAN_DATE)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.ShipPlanDate), ResourceType = typeof(EcConfirmProgressResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 注文番号 (SHIP_INSTRUCT_ID)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.ShipInstructId), ResourceType = typeof(EcConfirmProgressResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID (SHIP_INSTRUCT_SEQ)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.ShipInstructSeq), ResourceType = typeof(EcConfirmProgressResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long? ShipInstructSeq { get; set; }

        /// <summary>
        /// 注文日 (ORDER_DATE)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.OrderDate), ResourceType = typeof(EcConfirmProgressResource))]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送業者名 (TRANSPORTER_NAME)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.TransporterName), ResourceType = typeof(EcConfirmProgressResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 配送指定日 (ARRIVE_REQUEST_DATE)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.ArriveRequestDate), ResourceType = typeof(EcConfirmProgressResource))]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// データ受信日 (ALLOC_DATE)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.MakeDate), ResourceType = typeof(EcConfirmProgressResource))]
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// 状態 (SHIP_STATUS_NAME)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.ShipStatus), ResourceType = typeof(EcConfirmProgressResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipStatusName { get; set; }

        /// <summary>
        /// 出荷確定日(KAKU_DATE)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.KakuDate), ResourceType = typeof(EcConfirmProgressResource))]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// GAS欠品 (STOCKOUT_QTY)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.StockOutQty), ResourceType = typeof(EcConfirmProgressResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? StockoutQty { get; set; }

        /// <summary>
        /// ｷｬﾝｾﾙ指示 (CANCEL_FLAG)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.CancelFlags), ResourceType = typeof(EcConfirmProgressResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CancelFlag { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.ItemSkuId), ResourceType = typeof(EcConfirmProgressResource))]
        [MaxLength(60, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 予定数 (RESULT_QTY)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.OrderQty), ResourceType = typeof(EcConfirmProgressResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? OrderQty { get; set; }

        /// <summary>
        /// ケースNo (BOX_NO)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.BoxNo), ResourceType = typeof(EcConfirmProgressResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 出荷先店舗ID (SHIP_TO_STORE_ID)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.ShipToStoreId), ResourceType = typeof(EcConfirmProgressResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 前回ワークID (SEQ_PRE)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.SeqPre), ResourceType = typeof(EcConfirmProgressResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long SeqPre { get; set; }

        /// <summary>
        /// EC出荷形態 (EC_SHIP_CLASS_NAME)
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.EcShipClass), ResourceType = typeof(EcConfirmProgressResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string EcShipClassName { get; set; }

        #endregion
    }
}