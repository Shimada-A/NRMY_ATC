namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// 分類4
    /// </summary>
    [Table("M_ITEM_CATEGORIES4")]
    public partial class ItemCategories4 : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.DeleteFlag), ResourceType = typeof(ItemCategories4Resource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.CategoryId1), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類名1 (CATEGORY_NAME1)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.CategoryName1), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 分類付属区分1 (CATEGORY_CLASS1)
        /// </summary>
        [Display(Name = nameof(ItemCategories4Resource.CategoryClass1), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryClass1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.CategoryId2), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類名2 (CATEGORY_NAME2)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.CategoryName2), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryName2 { get; set; }

        /// <summary>
        /// 分類付属区分2 (CATEGORY_CLASS2)
        /// </summary>
        [Display(Name = nameof(ItemCategories4Resource.CategoryClass2), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryClass2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.CategoryId3), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類名3 (CATEGORY_NAME3)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.CategoryName3), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryName3 { get; set; }

        /// <summary>
        /// 分類付属区分3 (CATEGORY_CLASS3)
        /// </summary>
        [Display(Name = nameof(ItemCategories4Resource.CategoryClass3), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryClass3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        [Key]
        [Column(Order = 14)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.CategoryId4), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 分類名4 (CATEGORY_NAME4)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.CategoryName4), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryName4 { get; set; }

        /// <summary>
        /// 分類付属区分4 (CATEGORY_CLASS4)
        /// </summary>
        [Display(Name = nameof(ItemCategories4Resource.CategoryClass4), ResourceType = typeof(ItemCategories4Resource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryClass4 { get; set; }

        /// <summary>
        /// 分類1表示順 (DISPLAY_ORDER1)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.DisplayOrder1), ResourceType = typeof(ItemCategories4Resource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DisplayOrder1 { get; set; }

        /// <summary>
        /// 分類2表示順 (DISPLAY_ORDER2)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.DisplayOrder2), ResourceType = typeof(ItemCategories4Resource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DisplayOrder2 { get; set; }

        /// <summary>
        /// 分類3表示順 (DISPLAY_ORDER3)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.DisplayOrder3), ResourceType = typeof(ItemCategories4Resource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DisplayOrder3 { get; set; }

        /// <summary>
        /// 分類4表示順 (DISPLAY_ORDER4)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemCategories4Resource.DisplayOrder4), ResourceType = typeof(ItemCategories4Resource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DisplayOrder4 { get; set; }

        #endregion プロパティ
    }
}