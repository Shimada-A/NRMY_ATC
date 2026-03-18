namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// TC出荷指示修正ワーク
    /// </summary>
    [Table("WW_SHP_MOD_TC_INSTRUCTION")]
    public partial class ShpModTcInstruction : BaseModel
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
        [Display(Name = nameof(ShpModTcInstructionResource.Seq), ResourceType = typeof(ShpModTcInstructionResource))]
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
        [Display(Name = nameof(ShpModTcInstructionResource.LineNo), ResourceType = typeof(ShpModTcInstructionResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(ShpModTcInstructionResource.CenterId), ResourceType = typeof(ShpModTcInstructionResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ItemId), ResourceType = typeof(ShpModTcInstructionResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ItemName), ResourceType = typeof(ShpModTcInstructionResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ItemColorId), ResourceType = typeof(ShpModTcInstructionResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ItemColorName), ResourceType = typeof(ShpModTcInstructionResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ItemSizeId), ResourceType = typeof(ShpModTcInstructionResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ItemSizeName), ResourceType = typeof(ShpModTcInstructionResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(ShpModTcInstructionResource.ShipPlanDate), ResourceType = typeof(ShpModTcInstructionResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.EmergencyClassName), ResourceType = typeof(ShpModTcInstructionResource))]
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ShipInstructId), ResourceType = typeof(ShpModTcInstructionResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ShipInstructSeq), ResourceType = typeof(ShpModTcInstructionResource))]
        public long ShipInstructSeq { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ShipToStoreId), ResourceType = typeof(ShpModTcInstructionResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.ShipToStoreName), ResourceType = typeof(ShpModTcInstructionResource))]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 出荷優先順
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.PriorityOrder), ResourceType = typeof(ShpModTcInstructionResource))]
        public int? PriorityOrder { get; set; }

        /// <summary>
        /// 欠品不可店
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.StockOutStore), ResourceType = typeof(ShpModTcInstructionResource))]
        public string StockOutStore { get; set; }

        /// <summary>
        /// レーンNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.LaneNo), ResourceType = typeof(ShpModTcInstructionResource))]
        public int? LaneNo { get; set; }

        /// <summary>
        /// 間口No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpModTcInstructionResource.FrontageNo), ResourceType = typeof(ShpModTcInstructionResource))]
        public int? FrontageNo { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [Display(Name = nameof(ShpModTcInstructionResource.AllocUpDate), ResourceType = typeof(ShpModTcInstructionResource))]
        public DateTime? AllocUpDate { get; set; }

        /// <summary>
        /// 出荷指示数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpModTcInstructionResource.InstructQty), ResourceType = typeof(ShpModTcInstructionResource))]
        public int? InstructQty { get; set; }

        /// <summary>
        /// 最小出荷指示
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpModTcInstructionResource.MinInstructQty), ResourceType = typeof(ShpModTcInstructionResource))]
        public int? MinInstructQty { get; set; }

        /// <summary>
        /// 出荷指示数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpModTcInstructionResource.WmsInstructQty), ResourceType = typeof(ShpModTcInstructionResource))]
        public int? WmsInstructQty { get; set; }

        #endregion プロパティ
    }
}