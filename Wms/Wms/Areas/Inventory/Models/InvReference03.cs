namespace Wms.Areas.Inventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Inventory.Resources;
    using Wms.Models;

    /// <summary>
    /// 棚卸進捗照会ワーク03
    /// </summary>
    [Table("WW_INV_REFERENCE03")]
    public partial class InvReference03 : BaseModel
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
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ReferenceResource.LineNo), ResourceType = typeof(ReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.CenterId), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 行選択フラグ (IS_CHECK)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ReferenceResource.IsCheck), ResourceType = typeof(ReferenceResource))]
        public bool IsCheck { get; set; }



        /// <summary>
        /// 棚卸No (INVENTORY_NO)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryNo), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸開始日時 (INVENTORY_START_DATE)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryStartDate), ResourceType = typeof(Resources.ReferenceResource))]
        public DateTime? InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸区分 (INVENTORY_CLASS)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryClass), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int InventoryClass { get; set; }

        /// <summary>
        /// 棚卸名称 (INVENTORY_NAME)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryName), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InventoryName { get; set; }

        /// <summary>
        /// 外装棚卸許可フラグ (SIMPLE_INVENTORY_FLAG)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.SimpleInventoryFlag), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int SimpleInventoryFlag { get; set; }

        /// <summary>
        /// 棚卸状況 (INVENTORY_STATUS)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryStatus), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InventoryStatus { get; set; }

        /// <summary>
        /// 棚卸確定フラグ (INVENTORY_CONFIRM_FLAG)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryConfirmFlag), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-9, 9, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int InventoryConfirmFlag { get; set; }

        /// <summary>
        /// 棚卸確定日時 (INVENTORY_CONFIRM_DATE)
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryConfirmDate), ResourceType = typeof(Resources.ReferenceResource))]
        public DateTime? InventoryConfirmDate { get; set; }

        /// <summary>
        /// ロケーションコード (LOCATION_CD)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.LocationCd), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.ItemSkuId), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.ItemId), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.ItemName), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.Jan), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(13, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.ItemColorId), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.ItemSizeId), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_CLASS)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.CaseClass), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int CaseClass { get; set; }

        /// <summary>
        /// 格付ID (GRADE_ID)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.GradeId), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 梱包番号 (BOX_NO)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.BoxNo), ResourceType = typeof(Resources.ReferenceResource))]
        [MaxLength(36, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxNo { get; set; } = " ";

        /// <summary>
        /// 棚卸開始時在庫数 (STOCK_QTY_START)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.StockQtyStart), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int StockQtyStart { get; set; }

        /// <summary>
        /// 実績数 (RESULT_QTY)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ResultQty), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数＋ (DIFFERENCE_PLUS)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.DifferencePlus), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DifferencePlus { get; set; }

        /// <summary>
        /// 差異数－ (DIFFERENCE_MINUS)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.DifferenceMinus), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DifferenceMinus { get; set; }

        /// <summary>
        /// カウント数 (COUNT_SEQ)
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.ReferenceResource.CountSeq), ResourceType = typeof(Resources.ReferenceResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? CountSeq { get; set; }



        #endregion
    }
}