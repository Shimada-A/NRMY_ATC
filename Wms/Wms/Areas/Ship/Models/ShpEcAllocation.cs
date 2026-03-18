namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// EC引当ワーク
    /// </summary>
    [Table("WW_SHP_EC_ALLOCATION")]
    public partial class ShpEcAllocation : BaseModel
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
        [Display(Name = nameof(ShpEcAllocationResource.Seq), ResourceType = typeof(ShpEcAllocationResource))]
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
        [Display(Name = nameof(ShpEcAllocationResource.LineNo), ResourceType = typeof(ShpEcAllocationResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        [Display(Name = nameof(ShpEcAllocationResource.IsCheck), ResourceType = typeof(ShpEcAllocationResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(ShpEcAllocationResource.CenterId), ResourceType = typeof(ShpEcAllocationResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(ShpEcAllocationResource.ShipRequestDate), ResourceType = typeof(ShpEcAllocationResource))]
        public DateTime? ShipRequestDate { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
        [Display(Name = nameof(ShpEcAllocationResource.ShipInstructId), ResourceType = typeof(ShpEcAllocationResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 注文日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [Display(Name = nameof(ShpEcAllocationResource.OrderDate), ResourceType = typeof(ShpEcAllocationResource))]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送業者ID
        /// </summary>
        [Display(Name = nameof(ShpEcAllocationResource.TransporterId), ResourceType = typeof(ShpEcAllocationResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpEcAllocationResource.TransporterName), ResourceType = typeof(ShpEcAllocationResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 配送指定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(ShpEcAllocationResource.ArriveRequestDate), ResourceType = typeof(ShpEcAllocationResource))]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [Display(Name = nameof(ShpEcAllocationResource.DataDate), ResourceType = typeof(ShpEcAllocationResource))]
        public DateTime? DataDate { get; set; }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        [Display(Name = nameof(ShpEcAllocationResource.EcShipClass), ResourceType = typeof(ShpEcAllocationResource))]
        public string EcShipClass { get; set; }

        /// <summary>
        /// 指示数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpEcAllocationResource.OrderQty), ResourceType = typeof(ShpEcAllocationResource))]
        public int? OrderQty { get; set; }

        /// <summary>
        /// 引当フラグ
        /// </summary>
        [Display(Name = nameof(ShpEcAllocationResource.AllocFlag), ResourceType = typeof(ShpEcAllocationResource))]
        public int? AllocFlag { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpEcAllocationResource.AllocQty), ResourceType = typeof(ShpEcAllocationResource))]
        public int? AllocQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpEcAllocationResource.AllocQty), ResourceType = typeof(ShpEcAllocationResource))]
        public int? ItemSkuQty { get; set; }

        #endregion プロパティ
    }
}