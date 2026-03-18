namespace Wms.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Resources;

    /// <summary>
    /// 連携実行単位
    /// </summary>
    [Table("M_IF_RUN_UNITS")]
    public partial class IfRunUnit : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 連携実行単位ID (IF_RUN_UNIT_ID)
        /// </summary>
        /// <remarks>
        /// 通信処理　API実行時、システムID設定がないので、連携実行単位は連携システムID間で同一にはできない！
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunUnitResource.IfRunUnitId), ResourceType = typeof(IfRunUnitResource))]
        public string IfRunUnitId { get; set; }

        /// <summary>
        /// 連携データID (IF_DATA_ID)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunUnitResource.IfDataId), ResourceType = typeof(IfRunUnitResource))]
        public string IfDataId { get; set; }

        /// <summary>
        /// 連携システムID (IF_SYSTEM_ID)
        /// </summary>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunUnitResource.IfSystemId), ResourceType = typeof(IfRunUnitResource))]
        public string IfSystemId { get; set; }

        /// <summary>
        /// 実行順序 (IF_RUN_SEQ)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunUnitResource.IfRunSeq), ResourceType = typeof(IfRunUnitResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte IfRunSeq { get; set; }

        /// <summary>
        /// 異常終了時中断フラグ (ERROR_STOP_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:中断しない 1:中断する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunUnitResource.ErrorStopFlag), ResourceType = typeof(IfRunUnitResource))]
        public bool ErrorStopFlag { get; set; }

        #endregion
    }
}
