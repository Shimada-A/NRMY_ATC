namespace Wms.Areas.Master.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;
    using Wms.Resources;

    /// <summary>
    /// 海外アソート出荷保留店舗マスタ
    /// </summary>
    [Table("M_SHIPPING_HOLD_STORES")]
    public partial class ShippingHoldStore : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShippingHoldStoreResource.BrandId), ResourceType = typeof(ShippingHoldStoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// 店舗ID (STORE_ID)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShippingHoldStoreResource.StoreId), ResourceType = typeof(ShippingHoldStoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// 出荷保留区分 (SHIPPING_HOLD_CLASS)
        /// </summary>
        /// <remarks>
        /// 0：即出荷、1：出荷保留
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShippingHoldStoreResource.ShippingHoldClass), ResourceType = typeof(ShippingHoldStoreResource))]
        public bool ShippingHoldClass { get; set; }

        #endregion
    }
}
