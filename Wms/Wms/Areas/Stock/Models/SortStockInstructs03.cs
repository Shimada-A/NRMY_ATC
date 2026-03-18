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
    /// 在庫仕分指示ワーク03
    /// </summary>
    [Table("WW_SORT_STOCK_INSTRUCTS03")]
    public partial class SortStockInstructs03 : BaseModel
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
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 行番号 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 仕分指示Noごとの連番
        /// </remarks>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行番号_明細 (LINE_NO_SEQ)
        /// </summary>
        /// <remarks>
        /// 行Noごとの連番
        /// </remarks>
        [Key]
        [Column(Order = 101)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNoSeq { get; set; }


        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        [MaxLength(40, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 仕分指示No (SORT_INSTRUCT_NO)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortInstructNo { get; set; }

        /// <summary>
        /// 仕分指示名称 (SORT_INSTRUCT_NAME)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortInstructName { get; set; }

        /// <summary>
        /// 仕分指示取込日時 (SORT_IMPORT_DATE)
        /// </summary>
        public DateTime? SortImportDate { get; set; }

        /// <summary>
        /// 仕分方法区分 (SORT_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:SKU別、2:カテゴリー別
        /// </remarks>
        [Key]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-99, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public byte SortClass { get; set; }

        /// <summary>
        /// カテゴリー名 (SORT_CATEGORY_NAME)
        /// </summary>
        /// <remarks>
        /// 仕分方法区分「2：カテゴリー別」時にカテゴリー名称を設定する
        /// </remarks>
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortCategoryName { get; set; }

        /// <summary>
        /// 振替No (TRANSFER_NO)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransferNo { get; set; }

        /// <summary>
        /// 分類1 (CATEGORY_NAME1)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [MaxLength(30, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN_1 (JAN_1)
        /// </summary>
        [MaxLength(7, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan1 { get; set; }

        /// <summary>
        /// JAN_2 (JAN_2)
        /// </summary>
        [MaxLength(6, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan2 { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// カラー (ITEM_COLOR)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColor { get; set; }

        /// <summary>
        /// サイズ (ITEM_SIZE)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSize { get; set; }

        /// <summary>
        /// 備考 (NOTE)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string Note { get; set; }

        /// <summary>
        /// 仕分予定数 (STOCK_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int StockQty { get; set; }

        /// <summary>
        /// メッセージ (MESSAGE)
        /// </summary>
        /// <remarks>
        /// エラー時のメッセージ
        /// </remarks>
        [MaxLength(256, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string Message { get; set; }

        #endregion
    }
}
