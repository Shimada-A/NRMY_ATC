namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// アイテムマスタ
    /// </summary>
    [Table("M_ITEM_CODE")]
    public partial class Item : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemResource.DeleteFlag), ResourceType = typeof(ItemResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// アイテムコード (ITEM_CODE)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemResource.ItemCode), ResourceType = typeof(ItemResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemCode { get; set; }

        /// <summary>
        /// アイテム名 (ITEM_CODE_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemResource.ItemCodeName), ResourceType = typeof(ItemResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemCodeName { get; set; }

        /// <summary>
        /// 表示順 (DISPLAY_ORDER)
        /// </summary>
        [Display(Name = nameof(ItemResource.DisplayOrder), ResourceType = typeof(ItemResource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DisplayOrder { get; set; }

        #endregion プロパティ
    }
}