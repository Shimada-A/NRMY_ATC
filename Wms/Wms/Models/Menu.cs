namespace Wms.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Common.Resources;

    /// <summary>
    /// メニュー
    /// </summary>
    [Table("M_MENUS")]
    public partial class Menu : BaseModel
    {
        /// <summary>
        /// プログラムID (PROGRAM_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ProgramResource.ChildId), ResourceType = typeof(ProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ProgramId { get; set; }

        /// <summary>
        /// 親ID (PARENT_ID)
        /// </summary>
        [Display(Name = nameof(ProgramResource.ParentId), ResourceType = typeof(ProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ParentId { get; set; }

        /// <summary>
        /// メニュー名称 (MENU_NAME)
        /// </summary>
        /// <remarks>
        /// メニュー名称
        /// </remarks>
        [Display(Name = nameof(ProgramResource.MenuName), ResourceType = typeof(ProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string MenuName { get; set; }

        /// <summary>
        /// 表示順 (SORT_NO)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ProgramResource.SortNo), ResourceType = typeof(ProgramResource))]
        [Range(0, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int SortNo { get; set; }

        /// <summary>
        /// アイコン (ICON)
        /// </summary>
        /// <remarks>
        /// cssのクラス名
        /// </remarks>
        [Display(Name = nameof(ProgramResource.Icon), ResourceType = typeof(ProgramResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Icon { get; set; }
    }
}