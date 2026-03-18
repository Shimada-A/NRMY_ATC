namespace Wms.Areas.Master.ViewModels.Brand
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    public class Detail : BaseModel
    {
        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BrandResource.DeleteFlag), ResourceType = typeof(BrandResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BrandResource.BrandId), ResourceType = typeof(BrandResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名 (BRAND_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BrandResource.BrandName), ResourceType = typeof(BrandResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandName { get; set; }

        /// <summary>
        /// 表示順 (DISPLAY_ORDER)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BrandResource.DisplayOrder), ResourceType = typeof(BrandResource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// ブランド名(略称) (BRAND_SHORT_NAME)
        /// </summary>
        [Display(Name = nameof(BrandResource.BrandShortName), ResourceType = typeof(BrandResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandShortName { get; set; }

        /// <summary>
        /// 事業部ID (DIVISION_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BrandResource.DivisionId), ResourceType = typeof(BrandResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DivisionId { get; set; }

        /// <summary>
        /// 事業部名
        /// </summary>
        public string DivisionName { get; set; }

    }
}