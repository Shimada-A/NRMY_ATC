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
    /// 在庫仕分実績
    /// </summary>
    [Table("T_SORT_STOCK_RESULTS")]
    public partial class SortStockResult : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905：貝塚、0924：関東　など
        /// </remarks>
        public string CenterId { get; set; }

        /// <summary>
        /// 仕分指示No (SORT_INSTRUCT_NO)
        /// </summary>
        public string SortInstructNo { get; set; }

        /// <summary>
        /// 振替No (TRANSFER_NO)
        /// </summary>
        public string TransferNo { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        /// <remarks>
        /// 商品仕分用指示データのJANが一致するSKUを登録する。
        /// </remarks>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// 梱包番号 (BOX_NO)
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 仕分実績数 (RESULT_QTY)
        /// </summary>
        /// <remarks>
        /// 仕分した数量
        /// </remarks>
        public int ResultQty { get; set; }

        #endregion
    }
}
