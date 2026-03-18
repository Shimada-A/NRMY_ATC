namespace Wms.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Resources;

    /// <summary>
    /// 連携実行
    /// </summary>
    [Table("T_IF_RUNS")]
    public partial class IfRun : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 連携実行ID (IF_RUN_ID)
        /// </summary>
        /// <remarks>
        /// 自動連番
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunResource.IfRunId), ResourceType = typeof(IfRunResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IfRunId { get; set; }

        /// <summary>
        /// 連携実行単位ID (IF_RUN_UNIT_ID)
        /// </summary>
        [Display(Name = nameof(IfRunResource.IfRunUnitId), ResourceType = typeof(IfRunResource))]
        public string IfRunUnitId { get; set; }

        /// <summary>
        /// 連携実行状態区分 (IF_RUN_STATE)
        /// </summary>
        /// <remarks>
        /// 1:未実行 2:実行中 3:正常 4:ビジー（リトライ） 5:警告終了 6:スキップ 7:日次中待機、引当中待機 9:異常終了
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunResource.IfRunState), ResourceType = typeof(IfRunResource))]
        public byte IfRunState { get; set; }

        /// <summary>
        /// 開始時間 (START_TIME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfRunResource.StartTime), ResourceType = typeof(IfRunResource))]
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// 終了時間 (END_TIME)
        /// </summary>
        [Display(Name = nameof(IfRunResource.EndTime), ResourceType = typeof(IfRunResource))]
        public DateTimeOffset? EndTime { get; set; }

        #endregion
    }
}
