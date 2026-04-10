namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// センターマスタ
    /// </summary>
    [Table("M_CENTERS")]
    public partial class Centers : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(CentersResource.CenterId), ResourceType = typeof(CentersResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 電話番号 (CENTER_TEL)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(CentersResource.CenterTel), ResourceType = typeof(CentersResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterTel { get; set; }

        #endregion プロパティ
    }
}