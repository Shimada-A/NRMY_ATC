namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class EcBatchPicking
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Center), ResourceType = typeof(PrintBatchResource),Order =1)]
        public string Center { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNo1), ResourceType = typeof(PrintBatchResource), Order = 2)]
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチNo バーコード(ガイドあり）
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNoBarcode), ResourceType = typeof(PrintBatchResource), Order = 3)]
        public string BatchNoBarcode { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchName), ResourceType = typeof(PrintBatchResource), Order = 4)]
        public string BatchName { get; set; }

        /// <summary>
        /// EC1バッチ区分
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.EcOneBatchClass), ResourceType = typeof(PrintBatchResource), Order = 5)]
        public string EcOneBatchClass { get; set; }

        /// <summary>
        /// EC１バッチID
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.EcOneBatchId), ResourceType = typeof(PrintBatchResource), Order = 6)]
        public string EcOneBatchId { get; set; }

        /// <summary>
        /// 発行日時
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintDate), ResourceType = typeof(PrintBatchResource), Order = 7)]
        public DateTime PrintDate { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintUser), ResourceType = typeof(PrintBatchResource), Order = 8)]
        public string PrintUser { get; set; }

        /// <summary>
        /// バッチグループNo
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.AllocGroupNo1), ResourceType = typeof(PrintBatchResource), Order = 9)]
        public string AllocGroupNo { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.LocationCd1), ResourceType = typeof(PrintBatchResource), Order = 10)]
        public string LocationCd { get; set; }

        /// <summary>
        /// 荷姿区分
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.CaseClass), ResourceType = typeof(PrintBatchResource), Order = 11)]
        public string CaseClass { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemId1), ResourceType = typeof(PrintBatchResource), Order = 12)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemName), ResourceType = typeof(PrintBatchResource), Order = 13)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemColorId), ResourceType = typeof(PrintBatchResource), Order = 14)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemColorName), ResourceType = typeof(PrintBatchResource), Order = 15)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemSizeId), ResourceType = typeof(PrintBatchResource), Order = 16)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemSizeName), ResourceType = typeof(PrintBatchResource), Order = 17)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN1
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Jan1), ResourceType = typeof(PrintBatchResource), Order = 18)]
        public string Jan1 { get; set; }

        /// <summary>
        /// JAN2
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Jan2), ResourceType = typeof(PrintBatchResource), Order = 19)]
        public string Jan2 { get; set; }

        /// <summary>
        /// 指示数
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.HikiQty), ResourceType = typeof(PrintBatchResource), Order = 20)]
        public int? HikiQty { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.LocationCd2), ResourceType = typeof(PrintBatchResource), Order = 21)]
        public string LocationCd2 { get; set; }

        /// <summary>
        /// 荷姿区分
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.CaseClass2), ResourceType = typeof(PrintBatchResource), Order = 22)]
        public string CaseClass2 { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Jan1), ResourceType = typeof(PrintBatchResource), Order = 18)]
        public string Jan { get; set; }

        #endregion プロパティ
    }
}