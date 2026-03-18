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
    [Table("WW_MAS_SHIPPING_HOLD_STORES")]
    public partial class MasShippingHoldStore : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MasShippingHoldStoreResource.Seq), ResourceType = typeof(MasShippingHoldStoreResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Seq { get; set; }

        /// <summary>
        /// NO連番 (NO)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MasShippingHoldStoreResource.No), ResourceType = typeof(MasShippingHoldStoreResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long No { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MasShippingHoldStoreResource.BrandId), ResourceType = typeof(MasShippingHoldStoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名 (BRAND_NAME)
        /// </summary>
        [Display(Name = nameof(MasShippingHoldStoreResource.BrandName), ResourceType = typeof(MasShippingHoldStoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandName { get; set; }

        /// <summary>
        /// 店舗ID (STORE_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MasShippingHoldStoreResource.StoreId), ResourceType = typeof(MasShippingHoldStoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// 店舗名 (STORE_NAME)
        /// </summary>
        [Display(Name = nameof(MasShippingHoldStoreResource.StoreName), ResourceType = typeof(MasShippingHoldStoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreName { get; set; }

        /// <summary>
        /// 出荷保留区分 (SHIPPING_HOLD_CLASS)
        /// </summary>
        /// <remarks>
        /// 0：即出荷、1：出荷保留
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MasShippingHoldStoreResource.ShippingHoldClass), ResourceType = typeof(MasShippingHoldStoreResource))]
        public bool ShippingHoldClass { get; set; }

        #endregion
    }
}
