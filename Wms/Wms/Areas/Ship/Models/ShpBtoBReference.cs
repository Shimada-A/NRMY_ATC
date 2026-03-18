namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// BtoB出荷梱包進捗照会ワーク
    /// </summary>
    [Table("WW_SHP_BTO_B_REFERENCE01")]
    public partial class ShpBtoBReference : BaseModel
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
        [Display(Name = nameof(ShpBtoBReference01Resource.Seq), ResourceType = typeof(ShpBtoBReference01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShpBtoBReference01Resource.LineNo), ResourceType = typeof(ShpBtoBReference01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ (IS_CHECK)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShpBtoBReference01Resource.IsCheck), ResourceType = typeof(ShpBtoBReference01Resource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// ケースNo (BOX_NO)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.BoxNo), ResourceType = typeof(ShpBtoBReference01Resource))]
        [MaxLength(72, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 出荷先店舗ID (SHIP_TO_STORE_ID)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.ShipToStoreId), ResourceType = typeof(ShpBtoBReference01Resource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先店舗名 (SHIP_TO_STORE_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.ShipToStoreName), ResourceType = typeof(ShpBtoBReference01Resource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者名 (TRANSPORTER_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.TransporterName), ResourceType = typeof(ShpBtoBReference01Resource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 実績数 (RESULT_QTY)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.ResultQty), ResourceType = typeof(ShpBtoBReference01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ResultQty { get; set; }

        /// <summary>
        /// バッチNo (BATCH_NO)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.BatchNo), ResourceType = typeof(ShpBtoBReference01Resource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BatchNo { get; set; }

        /// <summary>
        /// 出荷予定日 (SHIP_PLAN_DATE)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.ShipPlanDate), ResourceType = typeof(ShpBtoBReference01Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// 状態 (SHIP_STATUS_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.ShipStatusName), ResourceType = typeof(ShpBtoBReference01Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipStatusName { get; set; }

        /// <summary>
        /// 出荷確定日 (KAKU_DATE)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.KakuDate), ResourceType = typeof(ShpBtoBReference01Resource))]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// SKU数 (ITEM_SKU_SUM)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.ItemSkuSum), ResourceType = typeof(ShpBtoBReference01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ItemSkuSum { get; set; }

        /// <summary>
        /// 明細行数 (DETAIL_SUM)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.DetailSum), ResourceType = typeof(ShpBtoBReference01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DetailSum { get; set; }

        /// <summary>
        /// 実績数合計 (RESULT_QTY_SUM)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.ResultQtySum), ResourceType = typeof(ShpBtoBReference01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ResultQtySum { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.CenterId), ResourceType = typeof(ShpBtoBReference01Resource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        #endregion
    }
}
