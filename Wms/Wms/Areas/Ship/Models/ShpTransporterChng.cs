namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// 配送業者変更ワーク
    /// </summary>
    [Table("WW_SHP_TRANSPORTER_CHNG")]
    public partial class ShpTransporterChng : BaseModel
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
        [Display(Name = nameof(ShpTransporterChngResource.Seq), ResourceType = typeof(ShpTransporterChngResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShpTransporterChngResource.LineNo), ResourceType = typeof(ShpTransporterChngResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ (IS_CHECK)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShpTransporterChngResource.IsCheck), ResourceType = typeof(ShpTransporterChngResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.CenterId), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日 (SHIP_PLAN_DATE)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.ShipPlanDate), ResourceType = typeof(ShpTransporterChngResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 指示区分 (INSTRUCT_CLASS_NAME)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.InstructClassName), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InstructClassName { get; set; }

        /// <summary>
        /// 緊急 (EMERGENCY_CLASS_NAME)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.EmergencyClassName), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID (SHIP_INSTRUCT_ID)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.ShipInstructId), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID (SHIP_INSTRUCT_SEQ)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.ShipInstructSeq), ResourceType = typeof(ShpTransporterChngResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long? ShipInstructSeq { get; set; }

        /// <summary>
        /// 出荷先ID (SHIP_TO_STORE_ID)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.ShipToStoreId), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先名 (SHIP_TO_STORE_NAME)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.ShipToStoreName), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.TransporterId), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者名 (TRANSPORTER_NAME)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.TransporterName), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 引当フラグ (ALLOC_FLAG)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.AllocFlag), ResourceType = typeof(ShpTransporterChngResource))]
        public bool? AllocFlag { get; set; }

        /// <summary>
        /// バッチNo (BATCH_NO)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.BatchNo), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BatchNo { get; set; }

        /// <summary>
        /// ケースNo (BOX_NO)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.BoxNo), ResourceType = typeof(ShpTransporterChngResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 更新回数（出荷） (UPDATE_COUNT_SHIP)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.UpdateCountShip), ResourceType = typeof(ShpTransporterChngResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long? UpdateCountShip { get; set; }

        /// <summary>
        /// 更新回数（梱包） (UPDATE_COUNT_PACKAGE)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.UpdateCountPackage), ResourceType = typeof(ShpTransporterChngResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long? UpdateCountPackage { get; set; }

        /// <summary>
        /// 処理区分 (PROCESS_CLASS)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngResource.ProcessClass), ResourceType = typeof(ShpTransporterChngResource))]
        public int? ProcessClass { get; set; }

        #endregion
    }
}