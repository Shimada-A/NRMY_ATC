namespace Wms.Areas.Ship.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Models;

    /// <summary>
    /// 引当進捗ワーク
    /// </summary>
    [Table("W_ALLOC_STATUS")]
    public partial class AllocStatus : BaseModel
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
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// ステータス (STATUS)
        /// </summary>
        [Range(-9, 9, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int Status { get; set; }

        /// <summary>
        /// 進捗率 (PROGRESS)
        /// </summary>
        [Range(-999, 999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int Progress { get; set; }

        /// <summary>
        /// メッセージ (MESSAGE)
        /// </summary>
        [MaxLength(300, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Message { get; set; }

        /// <summary>
        /// ステータス2 (STATUS2)
        /// </summary>
        [Range(-9999, 9999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? Status2 { get; set; }

        /// <summary>
        /// バッチNo (BATCH_NO)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BatchNo { get; set; }

        #endregion
    }
}
