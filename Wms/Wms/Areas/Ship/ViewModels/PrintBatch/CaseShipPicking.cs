namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class CaseShipPicking
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Center), ResourceType = typeof(PrintBatchResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNo1), ResourceType = typeof(PrintBatchResource), Order = 2)]
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchName), ResourceType = typeof(PrintBatchResource), Order = 3)]
        public string BatchName { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Locsec_1), ResourceType = typeof(PrintBatchResource), Order = 4)]
        public string Locsec_1 { get; set; }

        /// <summary>
        /// バッチNo バーコード(ガイドあり）
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BatchNoBarcode), ResourceType = typeof(PrintBatchResource), Order = 5)]
        public string BatchNoBarcode { get; set; }

        /// <summary>
        /// 発行日時
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintDate), ResourceType = typeof(PrintBatchResource), Order = 6)]
        public DateTime PrintDate { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.PrintUser), ResourceType = typeof(PrintBatchResource), Order = 7)]
        public string PrintUser { get; set; }

        /// <summary>
        /// ロケーションバーコード
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.LocationCdBarcode), ResourceType = typeof(PrintBatchResource), Order = 8)]
        public string LocationCdBarcode { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.LocationCd1), ResourceType = typeof(PrintBatchResource), Order = 9)]
        public string LocationCd { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.BoxNo), ResourceType = typeof(PrintBatchResource), Order = 10)]
        public string BoxNo { get; set; }

        /// <summary>
        /// 入数
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.StockQty), ResourceType = typeof(PrintBatchResource), Order = 11)]
        public int? StockQty { get; set; }

        /// <summary>
        /// 店番
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.StoreId), ResourceType = typeof(PrintBatchResource), Order = 12)]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 店名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.StoreName), ResourceType = typeof(PrintBatchResource), Order = 13)]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.TransporterName), ResourceType = typeof(PrintBatchResource), Order = 14)]
        public string TransporterName { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ListSeq), ResourceType = typeof(PrintBatchResource), Order = 15)]
        public string ListSeq { get; set; }

        /// <summary>
        /// 間口
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.FrontageNo), ResourceType = typeof(PrintBatchResource), Order = 16)]
        public string FrontageNo { get; set; }

        /// <summary>
        /// JAN入り明細フラグ
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.JanFlag), ResourceType = typeof(PrintBatchResource), Order = 17)]
        public string JanFlag { get; set; }

        #endregion プロパティ
    }
}