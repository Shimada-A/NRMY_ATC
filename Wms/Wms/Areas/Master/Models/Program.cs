namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using Share.Common.Resources;
    using Wms.Models;
    using Wms.Resources;

    /// <summary>
    /// プログラム
    /// </summary>
    [Table("M_PROGRAMS")]
    public class Program : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// プログラムID (PROGRAM_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ProgramResource.ProgramId), ResourceType = typeof(Resources.ProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ProgramId { get; set; }

        /// <summary>
        /// プログラム区分 (PROGRAM_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:PC 2:ハンディ
        /// (以下は使用しないが暫定値 3:帳票 4:I/F 5:ハンディAPI)
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ProgramResource.ProgramClass), ResourceType = typeof(Resources.ProgramResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte ProgramClass { get; set; }

        /// <summary>
        /// プログラム名 (PROGRAM_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ProgramResource.ProgramName), ResourceType = typeof(Resources.ProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ProgramName { get; set; }

        /// <summary>
        /// コントローラー名 (CONTROLLER_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ProgramResource.ControllerName), ResourceType = typeof(Resources.ProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ControllerName { get; set; }

        /// <summary>
        /// アクション名 (ACTION_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ProgramResource.ActionName), ResourceType = typeof(Resources.ProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ActionName { get; set; }

        /// <summary>
        /// エリア名 (AREA_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ProgramResource.AreaName), ResourceType = typeof(Resources.ProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string AreaName { get; set; }

        public override void OnModelCreating<T>(DbModelBuilder modelBuilder, T subClass)
        {
            base.OnModelCreating<T>(modelBuilder, subClass);
            modelBuilder.Entity<Program>().Property(m => (decimal)m.ProgramClass).HasPrecision(2, 0);
        }

        #endregion プロパティ
    }
}