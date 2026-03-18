namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class PrintBatchResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 行選択フラグ活性
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// 帳票ID
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 表示番号
        /// </summary>
        public int DispNo { get; set; }

        /// <summary>
        /// 帳票名
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        /// <remarks>
        public string StatusName { get; set; }

        #endregion プロパティ
    }
}