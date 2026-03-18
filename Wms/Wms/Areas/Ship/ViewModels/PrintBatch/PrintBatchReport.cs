namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class PrintBatchReport
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Center), ResourceType = typeof(PrintBatchResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// 引当日
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.AllocDate), ResourceType = typeof(PrintBatchResource), Order = 2)]
        public string AllocDate { get; set; }

        /// <summary>
        /// 発行日時
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintDate), ResourceType = typeof(PrintBatchResource), Order = 3)]
        public DateTime PrintDate { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintUser), ResourceType = typeof(PrintBatchResource), Order = 4)]
        public string PrintUser { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNo1), ResourceType = typeof(PrintBatchResource), Order = 5)]
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchName), ResourceType = typeof(PrintBatchResource), Order = 6)]
        public string BatchName { get; set; }

        #endregion プロパティ
    }
}