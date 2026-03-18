namespace Wms.Areas.Inventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Inventory.Resources;
    using Wms.Models;

    /// <summary>
    /// 棚卸進捗照会ワーク04
    /// </summary>
    [Table("WW_INV_REFERENCE04")]
    public partial class InvReference04 : BaseModel
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
        [Display(Name = nameof(ReferenceResource.Seq), ResourceType = typeof(ReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.CenterId), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 棚卸No (INVENTORY_NO)
        /// </summary>
        /// <remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryNo), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InventoryNo { get; set; }

        /// <summary>
        /// ロケーション数 (LOCATION_COUNT)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.LocationCount), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int LocationCount { get; set; }

        /// <summary>
        /// 帳簿在庫SKU数 (STOCK_SKU)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.StockSku), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int StockSku { get; set; }

        /// <summary>
        /// 実棚SKU数 (RESULT_SKU)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.ResultSku), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ResultSku { get; set; }

        /// <summary>
        /// 帳簿在庫ケース数 (STOCK_CASE)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.StockCase), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int StockCase { get; set; }

        /// <summary>
        /// 実棚ケース数 (RESULT_CASE)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.ResultCase), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ResultCase { get; set; }

        /// <summary>
        /// 帳簿在庫バラ数 (STOCK_QtY)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.StockQty), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int StockQty { get; set; }

        /// <summary>
        /// 実棚バラ数 (RESULT_QtY)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.ResultQty), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ResultQty { get; set; }

        #endregion
    }
}