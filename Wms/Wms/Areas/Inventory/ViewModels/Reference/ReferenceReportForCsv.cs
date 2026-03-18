namespace Wms.Areas.Inventory.ViewModels.Reference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// 棚卸進捗照会画面（Excel）
    /// </summary>
    public class ReferenceReportForCsv
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CenterId), ResourceType = typeof(Resources.ReferenceResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// 棚卸開始日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryStartDate), ResourceType = typeof(Resources.ReferenceResource), Order = 2)]
        public string InventoryStartDate { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Area), ResourceType = typeof(Resources.ReferenceResource), Order = 3)]
        public string Area { get; set; }

        /// <summary>
        /// 棚列
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryRow), ResourceType = typeof(Resources.ReferenceResource), Order = 4)]
        public string InventoryRow { get; set; }

        /// <summary>
        /// 棚卸No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryNo), ResourceType = typeof(Resources.ReferenceResource), Order = 5)]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryName), ResourceType = typeof(Resources.ReferenceResource), Order = 6)]
        public string InventoryName { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.PrintUser), ResourceType = typeof(Resources.ReferenceResource), Order = 7)]
        public string UserId { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Grade), ResourceType = typeof(Resources.ReferenceResource), Order = 8)]
        public string GradeId { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.LocationCd), ResourceType = typeof(Resources.ReferenceResource), Order = 9)]
        public string LocationCd { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Box), ResourceType = typeof(Resources.ReferenceResource), Order = 10)]
        public string BoxNo { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemId), ResourceType = typeof(Resources.ReferenceResource), Order = 11)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemName), ResourceType = typeof(Resources.ReferenceResource), Order = 12)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColorId), ResourceType = typeof(Resources.ReferenceResource), Order = 13)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColorName), ResourceType = typeof(Resources.ReferenceResource), Order = 14)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSizeId), ResourceType = typeof(Resources.ReferenceResource), Order = 15)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSizeName), ResourceType = typeof(Resources.ReferenceResource), Order = 16)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Jan), ResourceType = typeof(Resources.ReferenceResource), Order = 17)]
        public string Jan { get; set; }

        /// <summary>
        /// 帳簿在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.StockQtyStart), ResourceType = typeof(Resources.ReferenceResource), Order = 20)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int StockQtyStart { get; set; }

        /// <summary>
        /// 実棚数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ResultQty), ResourceType = typeof(Resources.ReferenceResource), Order = 21)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DifferenceQty), ResourceType = typeof(Resources.ReferenceResource), Order = 22)]
        public string DifferenceQty { get; set; }

        /// <summary>
        /// 担当者
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.UserId), ResourceType = typeof(Resources.ReferenceResource), Order = 23)]
        public string UserName { get; set; }

        /// <summary>
        /// カウント回数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CountSeq), ResourceType = typeof(Resources.ReferenceResource), Order = 24)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CountSeq { get; set; }

        #endregion プロパティ
    }
}