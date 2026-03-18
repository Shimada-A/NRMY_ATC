namespace Wms.Areas.Inventory.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Inventory.Resources;
    using Wms.Models;

    /// <summary>
    /// 棚卸実績入力ワーク01
    /// </summary>
    [Table("WW_INV_INPUT01")]
    public partial class InvInput01 : BaseModel
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
        [Display(Name = nameof(InvInput01Resource.Seq), ResourceType = typeof(InvInput01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905：貝塚、0924：関東　など
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.CenterId), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 棚卸No (INVENTORY_NO)
        /// </summary>
        /// <remarks>
        /// 1回の棚卸作業の識別キー　システムで採番する
        /// </remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.InventoryNo), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸行No (INVENTORY_SEQ)
        /// </summary>
        /// <remarks>
        /// 棚卸番号内の識別、実績と結合するためのシーケンス　ソートでは使用しない
        /// </remarks>
        [Key]
        [Column(Order = 14)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.InventorySeq), ResourceType = typeof(InvInput01Resource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int InventorySeq { get; set; }

        /// <summary>
        /// 棚卸開始日時 (INVENTORY_START_DATE)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.InventoryStartDate), ResourceType = typeof(InvInput01Resource))]
        public DateTime? InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸区分 (INVENTORY_CLASS)
        /// </summary>
        /// <remarks>
        /// 1：全件棚卸、2：循環棚卸
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.InventoryClass), ResourceType = typeof(InvInput01Resource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int InventoryClass { get; set; }

        /// <summary>
        /// 棚卸名称 (INVENTORY_NAME)
        /// </summary>
        /// <remarks>
        /// 取込ファイル名を登録する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.InventoryName), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InventoryName { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.ItemSkuId), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.ItemName), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.Jan), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(13, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.ItemId), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.ItemColorId), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.ItemSizeId), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// ロケーションコード (LOCATION_CD)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.LocationCd), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:ケース、2:バラ、9:指定なし　ロケMよりセット
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.CaseClass), ResourceType = typeof(InvInput01Resource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int CaseClass { get; set; }

        /// <summary>
        /// 格付ID (GRADE_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(InvInput01Resource.GradeId), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 梱包番号 (BOX_NO)
        /// </summary>
        /// <remarks>
        /// バラの場合は空白
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.BoxNo), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(36, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxNo { get; set; } = " ";

        /// <summary>
        /// 納品書番号 (INVOICE_NO)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.InvoiceNo), ResourceType = typeof(InvInput01Resource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InvoiceNo { get; set; } = " ";

        /// <summary>
        /// 帳簿在庫数 (STOCK_QTY_START)
        /// </summary>
        /// <remarks>
        /// 棚卸開始時在庫数
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.StockQtyStart), ResourceType = typeof(InvInput01Resource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? StockQtyStart { get; set; }

        /// <summary>
        /// 棚卸確定フラグ (INVENTORY_CONFIRM_FLAG)
        /// </summary>
        /// <remarks>
        /// 1：未確定、2：仮確定、3：本確定
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.InventoryConfirmFlag), ResourceType = typeof(InvInput01Resource))]
        public int? InventoryConfirmFlag { get; set; }

        /// <summary>
        /// 棚卸確定日時 (INVENTORY_CONFIRM_DATE)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.InventoryConfirmDate), ResourceType = typeof(InvInput01Resource))]
        public DateTime? InventoryConfirmDate { get; set; }

        /// <summary>
        /// 棚卸確定回数 (INVENTORY_CONFIRM_SEQ)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.InventoryConfirmSeq), ResourceType = typeof(InvInput01Resource))]
        public int InventoryConfirmSeq { get; set; } = 0;

        /// <summary>
        /// 在庫有無フラグ (STOCK_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：在庫無、1：在庫有
        /// 棚卸開始時に在庫がある場合は「1：在庫有」
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.StockFlag), ResourceType = typeof(InvInput01Resource))]
        public bool? StockFlag { get; set; }

        /// <summary>
        /// 外装棚卸許可フラグ (SIMPLE_INVENTORY_FLAG)
        /// </summary>
        /// <remarks>
        /// 外装での棚卸を許可するか判断
        /// 0：許可しない、1：許可する
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.SimpleInventoryFlag), ResourceType = typeof(InvInput01Resource))]
        public int? SimpleInventoryFlag { get; set; }

        /// <summary>
        /// 棚卸差異リスト発行フラグ (DIFFERENCE_LIST_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：未発行、1：発行済み
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.DifferenceListFlag), ResourceType = typeof(InvInput01Resource))]
        public int DifferenceListFlag { get; set; } = 0;

        /// <summary>
        /// 棚卸差異リスト発行日時 (DIFFERENCE_LIST_DATE)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.DifferenceListDate), ResourceType = typeof(InvInput01Resource))]
        public DateTime? DifferenceListDate { get; set; }

        /// <summary>
        /// 棚卸差異リスト発行ユーザーID (DIFFERENCE_LIST_USER_ID)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.DifferenceListUserId), ResourceType = typeof(InvInput01Resource))]
        public string DifferenceListUserId { get; set; }

        /// <summary>
        /// 修正前実績数 (RESULT_QTY_BEFORE)
        /// </summary>
        /// <remarks>
        /// 修正前の棚卸実績数 
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.ResultQtyBefore), ResourceType = typeof(InvInput01Resource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ResultQtyBefore { get; set; }

        /// <summary>
        /// 実績数 (RESULT_QTY)
        /// </summary>
        /// <remarks>
        /// 棚卸実績数 
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.ResultQty), ResourceType = typeof(InvInput01Resource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 新規追加フラグ (ADD_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:既存、1:新規追加
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.AddFlag), ResourceType = typeof(InvInput01Resource))]
        public int? AddFlag { get; set; }

        #endregion プロパティ
    }
}