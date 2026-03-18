namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Common;
    using Wms.Models;
    using Wms.Resources;

    /// <summary>
    /// プログラム使用可否
    /// </summary>
    [Table("M_USER_PROGRAMS")]
    public partial class UserProgram : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// プログラムID (PROGRAM_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.UserProgramResource.ProgramId), ResourceType = typeof(Resources.UserProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ProgramId { get; set; }

        /// <summary>
        /// 権限レベル (PERMISSION_LEVEL)
        /// </summary>
        /// <remarks>
        /// 1:システム管理者
        /// 2:ユーザー管理者
        /// 3:倉庫管理者
        /// 4:事務担当者
        /// 5:現場担当者
        /// 6:パート
        /// 7:本部用
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.UserProgramResource.PermissionLevel), ResourceType = typeof(Resources.UserProgramResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte PermissionLevel { get; set; }

        /// <summary>
        /// 使用可否区分 (USABLE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:使用不可
        /// 1:閲覧のみ
        /// 2:一部項目のみ更新可・閲覧
        /// 3:登録・更新・削除・閲覧
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.UserProgramResource.UsableClass), ResourceType = typeof(Resources.UserProgramResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public UsableClasses UsableClass { get; set; }

        #endregion プロパティ
    }
}