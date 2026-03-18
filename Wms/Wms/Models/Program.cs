namespace Wms.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using Share.Common.Resources;
    using Wms.Common;
    using Wms.Common.Resources;

    /// <summary>
    /// プログラム
    /// </summary>
    [Table("M_PROGRAMS")]
    public class Program : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// プログラムID
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ProgramResource.ProgramId), ResourceType = typeof(ProgramResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ProgramId { get; set; }

        /// <summary>
        /// プログラム区分
        /// </summary>
        /// <remarks>
        /// 1：マスタ、2：トランザクション
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ProgramResource.ProgramClass), ResourceType = typeof(ProgramResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public ProgramClasses ProgramClass { get; set; }

        /// <summary>
        /// プログラム名
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ProgramResource.ProgramName), ResourceType = typeof(ProgramResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ProgramName { get; set; }

        /// <summary>
        /// コントローラー名
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ProgramResource.ControllerName), ResourceType = typeof(ProgramResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ControllerName { get; set; }

        /// <summary>
        /// アクション名
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ProgramResource.ActionName), ResourceType = typeof(ProgramResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ActionName { get; set; }

        /// <summary>
        /// エリア名
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ProgramResource.AreaName), ResourceType = typeof(ProgramResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string AreaName { get; set; }

        public override void OnModelCreating<T>(DbModelBuilder modelBuilder, T subClass)
        {
            base.OnModelCreating<T>(modelBuilder, subClass);
            modelBuilder.Entity<Program>().Property(m => (decimal)m.ProgramClass).HasPrecision(2, 0);
        }

        #endregion

    }
}
