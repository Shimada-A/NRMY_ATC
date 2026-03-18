namespace Wms.Areas.Inventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Inventory.Resources;
    using Wms.Models;

    /// <summary>
    /// 棚卸確定画面ワーク
    /// </summary>
    [Table("WW_INV_CONFIRM_SC01")]
    public partial class InventoryConfirm : BaseModel
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
        [Display(Name = nameof(ConfirmResource.Seq), ResourceType = typeof(ConfirmResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.LineNo), ResourceType = typeof(ConfirmResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ (IS_CHECK)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.IsCheck), ResourceType = typeof(ConfirmResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ConfirmResource.CenterId), ResourceType = typeof(Resources.ConfirmResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 棚卸No (INVENTORY_NO)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ConfirmResource.InventoryNo), ResourceType = typeof(Resources.ConfirmResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸開始日時 (INVENTORY_START_DATE)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(Resources.ConfirmResource.InventoryStartDate), ResourceType = typeof(Resources.ConfirmResource))]
        public DateTime? InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸区分 (INVENTORY_CLASS)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ConfirmResource.InventoryClass), ResourceType = typeof(Resources.ConfirmResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int InventoryClass { get; set; }

        /// <summary>
        /// 棚卸名称 (INVENTORY_NAME)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ConfirmResource.InventoryName), ResourceType = typeof(Resources.ConfirmResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InventoryName { get; set; }

        /// <summary>
        /// 棚卸確定フラグ (INVENTORY_CONFIRM_FLAG)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ConfirmResource.InventoryConfirmFlag), ResourceType = typeof(Resources.ConfirmResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int InventoryConfirmFlag { get; set; }

        /// <summary>
        /// 棚卸確定日時 (INVENTORY_CONFIRM_DATE)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [Display(Name = nameof(Resources.ConfirmResource.InventoryConfirmDate), ResourceType = typeof(Resources.ConfirmResource))]
        public DateTime? InventoryConfirmDate { get; set; }

        #endregion
    }
}