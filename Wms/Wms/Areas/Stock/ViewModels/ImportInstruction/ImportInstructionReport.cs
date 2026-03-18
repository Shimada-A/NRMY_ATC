namespace Wms.Areas.Stock.ViewModels.ImportInstruction
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Stock.Resources;
    using Wms.Common;

    /// <summary>
    /// 在庫仕分指示テンプレート処理DownLoadRow
    /// </summary>
    public class ImportInstructionOutputReport
    {
        #region プロパティ

        /// <summary>
        /// 仕分方法区分 (SORT_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:SKU別、2:カテゴリー別
        /// </remarks>
        [Display(Name = "SortClassDisp", ResourceType = typeof(ImportInstructionResource))]
        public byte SortClass { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Display(Name = "Jan", ResourceType = typeof(ImportInstructionResource))]
        public string Jan { get; set; }

        /// <summary>
        /// カテゴリー名 (SORT_CATEGORY_NAME)
        /// </summary>
        /// <remarks>
        /// 仕分方法区分「2：カテゴリー別」時にカテゴリー名称を設定する
        /// </remarks>
        [Display(Name = "SortCategoryName", ResourceType = typeof(ImportInstructionResource))]
        public string SortCategoryName { get; set; }

        /// <summary>
        /// 振替No (TRANSFER_NO)
        /// </summary>
        [Display(Name = "TransferNo", ResourceType = typeof(ImportInstructionResource))]
        public string TransferNo { get; set; }

        /// <summary>
        /// 仕分予定数数 (STOCK_QTY)
        /// </summary>
        [Display(Name = "StockQty", ResourceType = typeof(ImportInstructionResource))]
        public int? StockQty { get; set; }

        /// <summary>
        /// 備考 (NOTE)
        /// </summary>
        [Display(Name = "Note", ResourceType = typeof(ImportInstructionResource))]
        public string Note { get; set; }

        #endregion プロパティ
    }
}