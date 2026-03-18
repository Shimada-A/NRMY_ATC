namespace Wms.Areas.Master.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;
    using Wms.Resources;

    /// <summary>
    /// 作業備考マスタ
    /// </summary>
    [Table("M_OPERATION_NOTES")]
    public partial class OperationNotes : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 業務ID (OPERATION_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(OperationNoteResource.OperationId), ResourceType = typeof(OperationNoteResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string OperationId { get; set; }

        /// <summary>
        /// 連番 (SEQ)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(OperationNoteResource.Seq), ResourceType = typeof(OperationNoteResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Seq { get; set; }

        /// <summary>
        /// 備考 (OPERATION_NOTE)
        /// </summary>
        [Display(Name = nameof(OperationNoteResource.OperationNote), ResourceType = typeof(OperationNoteResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string OperationNote { get; set; }

        #endregion
    }
}
