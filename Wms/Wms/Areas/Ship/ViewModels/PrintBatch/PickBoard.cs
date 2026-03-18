namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class PickBoard
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Center), ResourceType = typeof(PrintBatchResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintUser), ResourceType = typeof(PrintBatchResource), Order = 2)]
        public string PrintUser { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNo1), ResourceType = typeof(PrintBatchResource), Order = 3)]
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchName), ResourceType = typeof(PrintBatchResource), Order = 4)]
        public string BatchName { get; set; }

        /// <summary>
        /// 摘取No
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PicLocNoH), ResourceType = typeof(PrintBatchResource), Order = 5)]
        public string PicLocNo { get; set; }

        /// <summary>
        /// JAN2
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.JanH), ResourceType = typeof(PrintBatchResource), Order = 6)]
        public string Jan { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.HikiQty1), ResourceType = typeof(PrintBatchResource), Order = 7)]
        public int? HikiQty { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemId1), ResourceType = typeof(PrintBatchResource), Order = 8)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemName), ResourceType = typeof(PrintBatchResource), Order = 9)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemColor), ResourceType = typeof(PrintBatchResource), Order = 10)]
        public string ItemColor { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemSize), ResourceType = typeof(PrintBatchResource), Order = 11)]
        public string ItemSize { get; set; }


        #endregion プロパティ
    }
}