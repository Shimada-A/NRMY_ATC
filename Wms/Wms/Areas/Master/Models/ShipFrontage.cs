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
    /// 出荷レーン間口
    /// </summary>
    [Table("M_SHIP_FRONTAGE")]
    public partial class ShipFrontage : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード
        /// </remarks>
        [Key]
        [Column(Order = 14)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.CenterId), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// レーンNo (LANE_NO)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.LaneNo), ResourceType = typeof(ShipFrontageResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LaneNo { get; set; }

        /// <summary>
        /// 間口No (FRONTAGE_NO)
        /// </summary>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.FrontageNo), ResourceType = typeof(ShipFrontageResource))]
        [Range(-999, 999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int FrontageNo { get; set; }

        /// <summary>
        /// 店舗ID (STORE_ID)
        /// </summary>
        /// <remarks>
        /// 店舗ID
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.StoreId), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.BrandId), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandId { get; set; }

        #endregion
    }
}
