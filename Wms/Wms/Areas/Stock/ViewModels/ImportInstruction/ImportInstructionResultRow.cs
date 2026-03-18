namespace Wms.Areas.Stock.ViewModels.ImportInstruction
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// 在庫仕分指示明細
    /// </summary>
    public class ImportInstructionResultRow
    {
        #region プロパティ
        
        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// No
        /// </summary>
        public int? No { get; set; }

        /// <summary>
        /// 仕分指示No
        /// </summary>
        public string SortInstructNo { get; set; }

        /// <summary>
        /// 仕分指示名称
        /// </summary>
        public string SortInstructName { get; set; }

        /// <summary>
        /// 仕分指示取込日時
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? SortImportDate { get; set; }

        /// <summary>
        /// カテゴリー仕分振分No数
        /// </summary>
        public int? CategorySortQty { get; set; }

        /// <summary>
        /// SKU仕分振分No数
        /// </summary>
        public int? SkuSortQty { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        public string CenterId { get; set; }

        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }

        /// <summary>
        /// ワークID
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 実績有無
        /// </summary>
        public int? ResultFlg { get; set; }

        #endregion プロパティ
    }
}