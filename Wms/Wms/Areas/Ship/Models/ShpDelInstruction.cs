namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// 出荷指示削除ワーク
    /// </summary>
    [Table("WW_SHP_DEL_INSTRUCTION")]
    public partial class ShpDelInstruction : BaseModel
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
        [Display(Name = nameof(ShpDelInstructionResource.Seq), ResourceType = typeof(ShpDelInstructionResource))]
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
        [Display(Name = nameof(ShpDelInstructionResource.LineNo), ResourceType = typeof(ShpDelInstructionResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDelInstructionResource.IsCheck), ResourceType = typeof(ShpDelInstructionResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(ShpDelInstructionResource.CenterId), ResourceType = typeof(ShpDelInstructionResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(ShpDelInstructionResource.ShipPlanDate), ResourceType = typeof(ShpDelInstructionResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDelInstructionResource.EmergencyClassName), ResourceType = typeof(ShpDelInstructionResource))]
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDelInstructionResource.ShipInstructId), ResourceType = typeof(ShpDelInstructionResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpDelInstructionResource.ItemSkuQty), ResourceType = typeof(ShpDelInstructionResource))]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// 出荷先数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpDelInstructionResource.ShipToQty), ResourceType = typeof(ShpDelInstructionResource))]
        public int? ShipToQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ShpDelInstructionResource.InstructQty), ResourceType = typeof(ShpDelInstructionResource))]
        public int? InstructQty { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ShpDelInstructionResource.AllocDate), ResourceType = typeof(ShpDelInstructionResource))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? AllocDate { get; set; }

        #endregion プロパティ
    }
}