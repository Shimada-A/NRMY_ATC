namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// カラー
    /// </summary>
    [Table("M_SIZES")]
    public partial class Size : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SizeResource.DeleteFlag), ResourceType = typeof(SizeResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SizeResource.ItemSizeId), ResourceType = typeof(SizeResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名 (ITEM_SIZE_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SizeResource.ItemSizeName), ResourceType = typeof(SizeResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 表示順 (DISPLAY_ORDER)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SizeResource.DisplayOrder), ResourceType = typeof(SizeResource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int DisplayOrder { get; set; }

        #endregion
    }
}