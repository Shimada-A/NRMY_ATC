namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// BtoB出荷指示進捗照会ワーク
    /// </summary>
    [Table("WW_SHP_BTO_B_INSTRUCTION_REFERENCE")]
    public partial class ShpBtoBInstructionReference : BaseModel
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
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.Seq), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.LineNo), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ (IS_CHECK)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.IsCheck), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.CenterId), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日 (SHIP_PLAN_DATE)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ShipPlanDate), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 指示区分 (INSTRUCT_CLASS_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.InstructClassName), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InstructClassName { get; set; }

        /// <summary>
        /// 緊急 (EMERGENCY_CLASS_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.EmergencyClassName), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID (SHIP_INSTRUCT_ID)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ShipInstructId), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID (SHIP_INSTRUCT_SEQ)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ShipInstructSeq), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long? ShipInstructSeq { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ItemId), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ItemName), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ItemSkuId), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(60, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ItemColorId), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名 (ITEM_COLOR_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ItemColorName), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ItemSizeId), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名 (ITEM_SIZE_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ItemSizeName), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.Jan), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(26, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 出荷先店舗ID (SHIP_TO_STORE_ID)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ShipToStoreId), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先店舗名 (SHIP_TO_STORE_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ShipToStoreName), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者名 (TRANSPORTER_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.TransporterName), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 出荷先数 (SHIP_TO_QTY)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ShipToQty), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ShipToQty { get; set; }

        /// <summary>
        /// 予定数 (INSTRUCT_QTY)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.InstructQty), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? InstructQty { get; set; }

        /// <summary>
        /// データ受信日 (ALLOC_DATE)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.AllocDate), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// 状態 (COMPLETE_FLAG_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.CompleteFlagName), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CompleteFlagName { get; set; }

        /// <summary>
        /// 引当数 (ALLOC_QTY)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.AllocQty), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? AllocQty { get; set; }

        /// <summary>
        /// ピック数 (PIC_QTY)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.PicQty), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? PicQty { get; set; }

        /// <summary>
        /// 欠品確定数 (STOCK_OUT_FIX_QTY)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.StockOutFixQty), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? StockOutFixQty { get; set; }

        /// <summary>
        /// 実績数 (RESULT_QTY)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.ResultQty), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 前回ワークID (SEQ_PRE)
        /// </summary>
        [Display(Name = nameof(ShpBtoBInstructionReferenceResource.SeqPre), ResourceType = typeof(ShpBtoBInstructionReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long SeqPre { get; set; }

        #endregion
    }
}