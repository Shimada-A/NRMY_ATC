namespace Wms.Areas.Ship.ViewModels.UploadCaseInstruction
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;

    /// <summary>
    /// アソートケース出荷指示テンプレート処理DownLoadRow
    /// </summary>
    public class UploadCaseInstructionOutputReport
    {
        #region プロパティ

        /// <summary>
        /// 仕分方法区分 (SORT_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:SKU別、2:カテゴリー別
        /// </remarks>
        [Display(Name = "SortClassDisp", ResourceType = typeof(UploadCaseInstructionResource))]
        public byte SortClass { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Display(Name = "Jan", ResourceType = typeof(UploadCaseInstructionResource))]
        public string Jan { get; set; }

        /// <summary>
        /// カテゴリー名 (SORT_CATEGORY_NAME)
        /// </summary>
        /// <remarks>
        /// 仕分方法区分「2：カテゴリー別」時にカテゴリー名称を設定する
        /// </remarks>
        [Display(Name = "SortCategoryName", ResourceType = typeof(UploadCaseInstructionResource))]
        public string SortCategoryName { get; set; }

        /// <summary>
        /// 振替No (TRANSFER_NO)
        /// </summary>
        [Display(Name = "TransferNo", ResourceType = typeof(UploadCaseInstructionResource))]
        public string TransferNo { get; set; }

        /// <summary>
        /// 仕分予定数数 (STOCK_QTY)
        /// </summary>
        [Display(Name = "StockQty", ResourceType = typeof(UploadCaseInstructionResource))]
        public int StockQty { get; set; }

        /// <summary>
        /// 備考 (NOTE)
        /// </summary>
        [Display(Name = "Note", ResourceType = typeof(UploadCaseInstructionResource))]
        public string Note { get; set; }

        #endregion プロパティ
    }
}