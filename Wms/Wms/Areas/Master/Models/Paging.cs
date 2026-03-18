namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// プログラム
    /// </summary>
    [Table("M_PAGINGS")]
    public class Paging : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// コントローラー名
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PagingResource.CONTROLLER_NAME), ResourceType = typeof(PagingResource))]
        public string ControllerName { get; set; }

        /// <summary>
        /// アクション名
        /// </summary>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PagingResource.ACTION_NAME), ResourceType = typeof(PagingResource))]
        public string ActionName { get; set; }

        /// <summary>
        /// ページサイズ
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PagingResource.PAGE_SIZE), ResourceType = typeof(PagingResource))]
        public int PageSize { get; set; }

        /// <summary>
        /// 処理可能件数
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PagingResource.PROC_NUM_LIMIT), ResourceType = typeof(PagingResource))]
        public int ProcNumLimit { get; set; }

        /// <summary>
        /// 属性で操作できないDBの設定をする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="subClass"></param>
        public override void OnModelCreating<T>(DbModelBuilder modelBuilder, T subClass)
        {
            base.OnModelCreating<T>(modelBuilder, subClass);
        }

        #endregion プロパティ
    }
}