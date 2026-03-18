namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class TotalPickingDetailRow
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Center), ResourceType = typeof(PrintBatchResource))]
        public string Center { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNo1), ResourceType = typeof(PrintBatchResource))]
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチNo バーコード(ガイドあり）
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNoBarcode), ResourceType = typeof(PrintBatchResource))]
        public string BatchNoBarcode { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchName), ResourceType = typeof(PrintBatchResource))]
        public string BatchName { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Locsec_1), ResourceType = typeof(PrintBatchResource))]
        public string Locsec_1 { get; set; }

        /// <summary>
        /// ピッキングG
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PickingGroupNo), ResourceType = typeof(PrintBatchResource))]
        public string PickingGroupNo { get; set; }

        /// <summary>
        /// 発行日時
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintDate), ResourceType = typeof(PrintBatchResource))]
        public DateTime PrintDate { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintUser), ResourceType = typeof(PrintBatchResource))]
        public string PrintUser { get; set; }

        /// <summary>
        /// ロケーションバーコード
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.LocationCdBarcode), ResourceType = typeof(PrintBatchResource))]
        public string LocationCdBarcode { get; set; }

        /// <summary>
        /// エリア_ピッキングG
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.AreaEc), ResourceType = typeof(PrintBatchResource))]
        public string AreaEc { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.LocationCd1), ResourceType = typeof(PrintBatchResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// 荷姿区分
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.CaseClass), ResourceType = typeof(PrintBatchResource))]
        public string CaseClass { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemSkuId), ResourceType = typeof(PrintBatchResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemId1), ResourceType = typeof(PrintBatchResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemName), ResourceType = typeof(PrintBatchResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemColorId), ResourceType = typeof(PrintBatchResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemColorName), ResourceType = typeof(PrintBatchResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemSizeId), ResourceType = typeof(PrintBatchResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemSizeName), ResourceType = typeof(PrintBatchResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 指示数
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.HikiQty), ResourceType = typeof(PrintBatchResource))]
        public int? HikiQty { get; set; }

        /// <summary>
        /// 摘取順
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PicLocNo), ResourceType = typeof(PrintBatchResource))]
        public string PicLocNo { get; set; }

        /// <summary>
        /// 荷合わせ
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Assortment), ResourceType = typeof(PrintBatchResource))]
        public string Assortment { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.JanH), ResourceType = typeof(PrintBatchResource))]
        public string Jan { get; set; }

        /// <summary>
        /// JAN入り明細フラグ
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.JanFlag), ResourceType = typeof(PrintBatchResource), Order = 20)]
        public string JanFlag { get; set; }

        /// <summary>
        /// 付属品名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.NoveltyName), ResourceType = typeof(PrintBatchResource), Order = 21)]
        public string NoveltyName { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ListSeq), ResourceType = typeof(PrintBatchResource), Order = 22)]
        public int ListSeq { get; set; }


        #endregion プロパティ
    }
}