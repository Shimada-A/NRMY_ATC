namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class StorePicking
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.Center), ResourceType = typeof(PrintBatchResource),Order =1)]
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
        /// 出荷先(ID)
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ShipToStoreIdH), ResourceType = typeof(PrintBatchResource), Order = 5)]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先(名称)
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ShipToStoreNameH), ResourceType = typeof(PrintBatchResource), Order = 6)]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.LocationCd1), ResourceType = typeof(PrintBatchResource), Order = 7)]
        public string LocationCd { get; set; }

        /// <summary>
        /// 荷姿区分
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.CaseClass), ResourceType = typeof(PrintBatchResource), Order = 8)]
        public string CaseClass { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemName), ResourceType = typeof(PrintBatchResource), Order = 9)]
        public string ItemName { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemId1), ResourceType = typeof(PrintBatchResource), Order = 10)]
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemColorId), ResourceType = typeof(PrintBatchResource), Order = 11)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemColorName), ResourceType = typeof(PrintBatchResource), Order = 12)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemSizeId), ResourceType = typeof(PrintBatchResource), Order = 13)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.ItemSizeName), ResourceType = typeof(PrintBatchResource), Order = 14)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.JanH), ResourceType = typeof(PrintBatchResource), Order = 15)]
        public string Jan { get; set; }

        /// <summary>
        /// 指示数
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.HikiQty), ResourceType = typeof(PrintBatchResource), Order = 16)]
        public int? HikiQty { get; set; }

        /// <summary>
        /// 付属品名
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.NoveltyName), ResourceType = typeof(PrintBatchResource), Order = 17)]
        public string NoveltyName { get; set; }

        /// <summary>
        /// JAN入り明細フラグ
        /// </summary>
        [Display(Name = nameof(PrintBatchResource.JanFlag), ResourceType = typeof(PrintBatchResource), Order = 18)]
        public string JanFlag { get; set; }

        #endregion プロパティ
    }
}