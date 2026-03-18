namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// 格付
    /// </summary>
    [Table("M_GRADES")]
    public partial class Grade : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 格付ID (GRADE_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GradeResource.GradeId), ResourceType = typeof(GradeResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 格付名称 (GRADE_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GradeResource.GradeName), ResourceType = typeof(GradeResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GradeName { get; set; }

        /// <summary>
        /// 表示順 (DISPLAY_ORDER)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(GradeResource.DisplayOrder), ResourceType = typeof(GradeResource))]
        [Range(0, 999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int DisplayOrder { get; set; }

        #endregion プロパティ
    }
}