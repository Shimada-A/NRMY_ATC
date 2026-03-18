namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// BtoB出荷実績入力ワーク
    /// </summary>
    [Table("WW_SHP_BTO_B_RESULT_INPUT")]
    public class ShpBtoBInstructionInput : BaseModel
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
        /// 出荷先店舗区分 (STORE_CLASS)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BtoBInstructionInputResource.StoreClass), ResourceType = typeof(BtoBInstructionInputResource))]
        public int StoreClass { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.CenterId), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日 (SHIP_PLAN_DATE)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ShipPlanDate), ResourceType = typeof(BtoBInstructionInputResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 指示区分 (INSTRUCT_CLASS_NAME)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.InstructClassName), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InstructClassName { get; set; }

        /// <summary>
        /// 緊急 (EMERGENCY_CLASS_NAME)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.EmergencyClassName), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID (SHIP_INSTRUCT_ID)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ShipInstructId), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID (SHIP_INSTRUCT_SEQ)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ShipInstructSeq), ResourceType = typeof(BtoBInstructionInputResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long? ShipInstructSeq { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemId), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemName), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemSkuId), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(60, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemColorId), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名 (ITEM_COLOR_NAME)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemColorName), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemSizeId), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名 (ITEM_SIZE_NAME)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemSizeName), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.Jan), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(26, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 出荷先店舗ID (SHIP_TO_STORE_ID)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ShipToStoreId), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先店舗名 (SHIP_TO_STORE_NAME)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ShipToStoreName), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者名 (TRANSPORTER_NAME)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.TransporterName), ResourceType = typeof(BtoBInstructionInputResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 出荷先数 (SHIP_TO_QTY)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ShipToQty), ResourceType = typeof(BtoBInstructionInputResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ShipToQty { get; set; }

        /// <summary>
        /// 予定数 (INSTRUCT_QTY)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.InstructQty), ResourceType = typeof(BtoBInstructionInputResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? InstructQty { get; set; }

        /// <summary>
        /// データ受信日 (ALLOC_DATE)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.MakeDate), ResourceType = typeof(BtoBInstructionInputResource))]
        public DateTime? MakeDate { get; set; }

        /// <summary>
        /// 引当数 (ALLOC_QTY)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.AllocQty), ResourceType = typeof(BtoBInstructionInputResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 前回ワークID (SEQ_PRE)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.SeqPre), ResourceType = typeof(BtoBInstructionInputResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long SeqPre { get; set; }

        #endregion
    }
}