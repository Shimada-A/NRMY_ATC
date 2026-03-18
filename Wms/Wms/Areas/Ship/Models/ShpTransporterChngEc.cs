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
    [Table("WW_SHP_TRANSPORTER_CHNG_EC")]
    public partial class ShpTransporterChngEc : BaseModel
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
        [Display(Name = nameof(ShpTransporterChngEcResource.Seq), ResourceType = typeof(ShpTransporterChngEcResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShpTransporterChngEcResource.LineNo), ResourceType = typeof(ShpTransporterChngEcResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ (IS_CHECK)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShpTransporterChngEcResource.IsCheck), ResourceType = typeof(ShpTransporterChngEcResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.CenterId), ResourceType = typeof(ShpTransporterChngEcResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日 (SHIP_PLAN_DATE)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.ShipPlanDate), ResourceType = typeof(ShpTransporterChngEcResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 出荷指示ID (SHIP_INSTRUCT_ID)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.ShipInstructId), ResourceType = typeof(ShpTransporterChngEcResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 注文日 (ORDER_DATE)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.OrderDate), ResourceType = typeof(ShpTransporterChngEcResource))]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送指定日 (ARRIVE_REQUEST_DATE)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.ArriveRequestDate), ResourceType = typeof(ShpTransporterChngEcResource))]
        public DateTime? ArriveRequestDate { get; set; }


        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.TransporterId), ResourceType = typeof(ShpTransporterChngEcResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者名 (TRANSPORTER_NAME)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.TransporterName), ResourceType = typeof(ShpTransporterChngEcResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 郵便番号 (DEST_ZIP)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.DestZip), ResourceType = typeof(ShpTransporterChngEcResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DestZip { get; set; }


        /// <summary>
        /// 引当フラグ (ALLOC_FLAG)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.AllocFlag), ResourceType = typeof(ShpTransporterChngEcResource))]
        public bool? AllocFlag { get; set; }

        /// <summary>
        /// バッチNo (BATCH_NO)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.BatchNo), ResourceType = typeof(ShpTransporterChngEcResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BatchNo { get; set; }

        /// <summary>
        /// ケースNo (BOX_NO)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.BoxNo), ResourceType = typeof(ShpTransporterChngEcResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 更新回数（出荷） (UPDATE_COUNT_SHIP)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.UpdateCountShip), ResourceType = typeof(ShpTransporterChngEcResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long? UpdateCountShip { get; set; }

        /// <summary>
        /// 更新回数（梱包） (UPDATE_COUNT_PACKAGE)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.UpdateCountPackage), ResourceType = typeof(ShpTransporterChngEcResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long? UpdateCountPackage { get; set; }

        /// <summary>
        /// 処理区分 (PROCESS_CLASS)
        /// </summary>
        [Display(Name = nameof(ShpTransporterChngEcResource.ProcessClass), ResourceType = typeof(ShpTransporterChngEcResource))]
        public int? ProcessClass { get; set; }

        #endregion
    }
}