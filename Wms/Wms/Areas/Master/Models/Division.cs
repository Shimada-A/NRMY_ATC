namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// 事業部マスタ
    /// </summary>
    [Table("M_DIVISIONS")]
    public partial class Division : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(DivisionResource.DeleteFlag), ResourceType = typeof(DivisionResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 事業部ID (DIVISION_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(DivisionResource.DivisionId), ResourceType = typeof(DivisionResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DivisionId { get; set; }

        /// <summary>
        /// 事業部名 (DIVISION_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(DivisionResource.DivisionName), ResourceType = typeof(DivisionResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DivisionName { get; set; }

        /// <summary>
        /// 事業部記号 (DIVISION_MARK)
        /// </summary>
        [Display(Name = nameof(DivisionResource.DivisionMark), ResourceType = typeof(DivisionResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DivisionMark { get; set; }

        /// <summary>
        /// 表示順 (DISPLAY_ORDER)
        /// </summary>
        [Display(Name = nameof(DivisionResource.DisplayOrder), ResourceType = typeof(DivisionResource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DisplayOrder { get; set; }

        #endregion プロパティ
    }
}