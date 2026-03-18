using Share.Common.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Wms.Areas.Inventory.Resources;
using Wms.Models;
using Wms.Resources;

namespace Wms.Areas.Inventory.Models
{
    /// <summary>
    /// 棚卸予定
    /// </summary>
    [Table("T_INVENTORY_PLANS")]
    public partial class InventoryPlan : BaseModel
    {
        #region プロパティ

        ///// <summary>
        ///// 所在区分 (LOC_CLASS)
        ///// </summary>
        ///// <remarks>
        ///// 1：自社倉庫　のみ
        ///// </remarks>
        //[Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        //[Display(Name = nameof(ConfirmResource.CenterClass), ResourceType = typeof(ConfirmResource))]
        //[Range(-99, 99, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        //public byte CenterClass { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　905：貝塚、924：関東　など
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.CenterId), ResourceType = typeof(ConfirmResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 棚卸No (INVENTORY_NO)
        /// </summary>
        /// <remarks>
        /// 1回の棚卸作業の識別キー　システムで採番する
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.InventoryNo), ResourceType = typeof(ConfirmResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸行No (INVENTORY_SEQ)
        /// </summary>
        /// <remarks>
        /// 棚卸番号内の識別、実績と結合するためのシーケンス　ソートでは使用しない
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.InventorySeq), ResourceType = typeof(ConfirmResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public int InventorySeq { get; set; }

        /// <summary>
        /// 棚卸開始日時 (INVENTORY_START_DATE)
        /// </summary>
        [Display(Name = nameof(ConfirmResource.InventoryStartDate), ResourceType = typeof(ConfirmResource))]
        public DateTime? InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸区分 (INVENTORY_CLASS)
        /// </summary>
        /// <remarks>
        /// 1：全件棚卸、2：循環棚卸
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.InventoryClass), ResourceType = typeof(ConfirmResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public byte InventoryClass { get; set; }

        /// <summary>
        /// 棚卸名称 (INVENTORY_NAME)
        /// </summary>
        /// <remarks>
        /// 取込ファイル名を登録する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.InventoryName), ResourceType = typeof(ConfirmResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string InventoryName { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.ItemSkuId), ResourceType = typeof(ConfirmResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.ItemName), ResourceType = typeof(ConfirmResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.Jan), ResourceType = typeof(ConfirmResource))]
        [MaxLength(13, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.ItemId), ResourceType = typeof(ConfirmResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.ItemColorId), ResourceType = typeof(ConfirmResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.ItemSizeId), ResourceType = typeof(ConfirmResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// ロケーションコード (LOCATION_CD)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.LocationCd), ResourceType = typeof(ConfirmResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:ケース、2:バラ、9:指定なし　ロケMよりセット
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.CaseClass), ResourceType = typeof(ConfirmResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public byte CaseClass { get; set; }

        /// <summary>
        /// 格付ID (GRADE_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.GradeId), ResourceType = typeof(ConfirmResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 梱包番号 (BOX_NO)
        /// </summary>
        /// <remarks>
        /// バラの場合は空白
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.BoxNo), ResourceType = typeof(ConfirmResource))]
        [MaxLength(36, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string BoxNo { get; set; } = " ";

        /// <summary>
        /// 納品書番号 (INVOICE_NO)
        /// </summary>
        /// <remarks>
        /// 入荷仮ロケの場合のみ。それ以外は空白
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.InvoiceNo), ResourceType = typeof(ConfirmResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string InvoiceNo { get; set; } = " ";

        /// <summary>
        /// 棚卸開始時在庫数 (STOCK_QTY_START)
        /// </summary>
        /// <remarks>
        /// 論理在庫数
        /// </remarks>
        [Display(Name = nameof(ConfirmResource.StockQtyStart), ResourceType = typeof(ConfirmResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public int? StockQtyStart { get; set; }

        /// <summary>
        /// 棚卸確定フラグ (INVENTORY_CONFIRM_FLAG)
        /// </summary>
        /// <remarks>
        /// 1：未確定、2：仮確定、3：本確定
        /// </remarks>
        [Display(Name = nameof(ConfirmResource.InventoryConfirmFlag), ResourceType = typeof(ConfirmResource))]
        public byte InventoryConfirmFlag { get; set; }

        /// <summary>
        /// 棚卸確定日時 (INVENTORY_CONFIRM_DATE)
        /// </summary>
        [Display(Name = nameof(ConfirmResource.InventoryConfirmDate), ResourceType = typeof(ConfirmResource))]
        public DateTime? InventoryConfirmDate { get; set; }

        /// <summary>
        /// 棚卸確定回数 (INVENTORY_CONFIRM_SEQ)
        /// </summary>
        /// <remarks>
        /// 仮確定・本確定した際に何回目の確定だったのかセット
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.InventoryConfirmSeq), ResourceType = typeof(ConfirmResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessageResource.Range), ErrorMessageResourceType = typeof(MessageResource))]
        public int InventoryConfirmSeq { get; set; }

        /// <summary>
        /// 在庫有無フラグ (STOCK_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：在庫無、1：在庫有
        /// 棚卸開始時に在庫がある場合は「1：在庫有」
        /// </remarks>
        [Display(Name = nameof(ConfirmResource.StockFlag), ResourceType = typeof(ConfirmResource))]
        public byte StockFlag { get; set; }

        /// <summary>
        /// 外装棚卸許可フラグ (SIMPLE_INVENTORY_FLAG)
        /// </summary>
        /// <remarks>
        /// 外装での棚卸を許可するか判断
        /// 0：許可しない、1：許可する
        /// </remarks>
        [Display(Name = nameof(ConfirmResource.SimpleInventoryFlag), ResourceType = typeof(ConfirmResource))]
        public byte SimpleInventoryFlag { get; set; }

        /// <summary>
        /// 棚卸差異リスト発行フラグ (DIFFERENCE_LIST_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：未発行、1：発行済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ConfirmResource.DifferenceListFlag), ResourceType = typeof(ConfirmResource))]
        public byte DifferenceListFlag { get; set; }

        /// <summary>
        /// 棚卸差異リスト発行日時 (DIFFERENCE_LIST_DATE)
        /// </summary>
        [Display(Name = nameof(ConfirmResource.DifferenceListDate), ResourceType = typeof(ConfirmResource))]
        public DateTime? DifferenceListDate { get; set; }

        /// <summary>
        /// 棚卸差異リスト発行ユーザーID (DIFFERENCE_LIST_USER_ID)
        /// </summary>
        [Display(Name = nameof(ConfirmResource.DifferenceListUserId), ResourceType = typeof(ConfirmResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessageResource.MaxLength), ErrorMessageResourceType = typeof(MessageResource))]
        public string DifferenceListUserId { get; set; }

        #endregion
    }
}
