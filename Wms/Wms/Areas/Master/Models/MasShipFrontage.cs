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
    /// 出荷レーン間口ワーク
    /// </summary>
    [Table("WW_MAS_SHIP_FRONTAGE")]
    public partial class MasShipFrontage : BaseModel
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

        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// NO (NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.No), ResourceType = typeof(ShipFrontageResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long No { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.CenterId), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// レーンNo (LANE_NO)
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.LaneNo), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LaneNo { get; set; }

        /// <summary>
        /// 間口No (FRONTAGE_NO)
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.FrontageNo), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string FrontageNo { get; set; }

        /// <summary>
        /// 店舗ID (STORE_ID)
        /// </summary>
        /// <remarks>
        /// 店舗ID、センターID
        /// </remarks>
        [Display(Name = nameof(ShipFrontageResource.StoreId), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// エラー情報 (ERR_MSG)
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.ErrMsg), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ErrMsg { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.BrandId), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名 (BRAND_NAME)
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.BrandName), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandName { get; set; }

        #endregion
    }
}
