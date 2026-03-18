namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class BatchBoard
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
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNo1), ResourceType = typeof(PrintBatchResource), Order = 3)]
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチNo バーコード(ガイドあり）
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNoBarcode), ResourceType = typeof(PrintBatchResource), Order = 4)]
        public string BatchNoBarcode { get; set; }

        /// <summary>
        /// 出荷種別
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ShipKindName), ResourceType = typeof(PrintBatchResource), Order = 5)]
        public string ShipKindName { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchName), ResourceType = typeof(PrintBatchResource), Order = 6)]
        public string BatchName { get; set; }

        /// <summary>
        /// 発行日時
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintDate), ResourceType = typeof(PrintBatchResource), Order = 9)]
        public DateTime PrintDate { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintUser), ResourceType = typeof(PrintBatchResource), Order = 10)]
        public string PrintUser { get; set; }


        #endregion プロパティ
    }
}