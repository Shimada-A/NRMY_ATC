namespace Wms.Areas.Stock.Models
{
    using Mvc.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using Wms.Models;
    using Wms.Areas.Stock.Resources;
    using Share.Common.Resources;

    /// <summary>
    /// 在庫仕分指示ワーク02
    /// </summary>
    [Table("WW_SORT_STOCK_INSTRUCTS02")]
    public partial class SortStockInstructs02 : BaseModel
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
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = "Seq", ResourceType = typeof(ImportInstructionResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 行番号 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// アップロードしたときの行番号
        /// </remarks>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = "LineNo", ResourceType = typeof(ImportInstructionResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        [Display(Name = "CenterId", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(40, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 仕分指示No (SORT_INSTRUCT_NO)
        /// </summary>
        [Display(Name = "SortInstructNo", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortInstructNo { get; set; }

        /// <summary>
        /// 仕分指示名称 (SORT_INSTRUCT_NAME)
        /// </summary>
        [Display(Name = "SortInstructName", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortInstructName { get; set; }

        /// <summary>
        /// 仕分指示取込日時 (SORT_IMPORT_DATE)
        /// </summary>
        [Display(Name = "SortImportDate", ResourceType = typeof(ImportInstructionResource))]
        public DateTime? SortImportDate { get; set; }

        /// <summary>
        /// 仕分方法区分 (SORT_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:SKU別、2:カテゴリー別
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = "SortClass", ResourceType = typeof(ImportInstructionResource))]
        [Range(-99, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public byte SortClass { get; set; }

        /// <summary>
        /// カテゴリー名 (SORT_CATEGORY_NAME)
        /// </summary>
        /// <remarks>
        /// 仕分方法区分「2：カテゴリー別」時にカテゴリー名称を設定する
        /// </remarks>
        [Display(Name = "SortCategoryName", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortCategoryName { get; set; }

        /// <summary>
        /// 振替No (TRANSFER_NO)
        /// </summary>
        [Display(Name = "TransferNo", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransferNo { get; set; }

        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        [Display(Name = "CategoryId1", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Display(Name = "ItemSkuId", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(30, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = "Jan", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(13, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [Display(Name = "ItemId", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        [Display(Name = "ItemColorId", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(5, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        [Display(Name = "ItemSizeId", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(5, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// 備考 (NOTE)
        /// </summary>
        [Display(Name = "Note", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string Note { get; set; }

        /// <summary>
        /// 仕分予定数 (STOCK_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = "StockQty", ResourceType = typeof(ImportInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? StockQty { get; set; }

        /// <summary>
        /// メッセージ (MESSAGE)
        /// </summary>
        /// <remarks>
        /// エラー時のメッセージ
        /// </remarks>
        [Display(Name = "Message", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(256, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string Message { get; set; }

        #endregion
    }
}
