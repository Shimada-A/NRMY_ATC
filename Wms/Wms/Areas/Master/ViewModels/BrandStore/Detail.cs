namespace Wms.Areas.Master.ViewModels.BrandStore
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
        [Display(Name = nameof(BrandStoreResource.DeleteFlag), ResourceType = typeof(BrandStoreResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 店舗ID (STORE_ID)
        /// </summary>
        /// <remarks>
        /// IF得意先ブランドマスタ.得意先コード[文字(8)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BrandStoreResource.StoreId), ResourceType = typeof(BrandStoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        public string StoreName { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        /// <remarks>
        /// IF得意先ブランドマスタ.ブランドコード[文字(2)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(BrandStoreResource.BrandId), ResourceType = typeof(BrandStoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandId { get; set; }

        public string BrandName { get; set; }

    }
}