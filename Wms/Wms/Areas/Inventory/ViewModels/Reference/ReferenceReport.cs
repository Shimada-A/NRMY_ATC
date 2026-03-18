namespace Wms.Areas.Inventory.ViewModels.Reference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// 棚卸進捗照会画面（Excel）
    /// </summary>
    public class ReferenceReport
    {
        #region プロパティ

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CenterId), ResourceType = typeof(Resources.ReferenceResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// 棚卸開始日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryStartDate), ResourceType = typeof(Resources.ReferenceResource), Order = 2)]
        public string InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryNo), ResourceType = typeof(Resources.ReferenceResource), Order = 3)]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryName), ResourceType = typeof(Resources.ReferenceResource), Order = 4)]
        public string InventoryName { get; set; }

        /// <summary>
        /// 棚卸行No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventorySeq), ResourceType = typeof(Resources.ReferenceResource), Order = 5)]
        public int InventorySeq { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.LocationCd), ResourceType = typeof(Resources.ReferenceResource), Order = 6)]
        public string Area { get; set; }

        /// <summary>
        /// 棚列
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.LocationCd), ResourceType = typeof(Resources.ReferenceResource), Order = 7)]
        public string InventoryRow { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.LocationCd), ResourceType = typeof(Resources.ReferenceResource), Order = 8)]
        public string LocationCd { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.GradeId), ResourceType = typeof(Resources.ReferenceResource), Order = 9)]
        public string Grade { get; set; }

        /// <summary>
        /// 荷姿コード
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CaseClass), ResourceType = typeof(Resources.ReferenceResource), Order = 10)]
        public int? CaseId { get; set; }

        /// <summary>
        /// 荷姿名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CaseClass), ResourceType = typeof(Resources.ReferenceResource), Order = 11)]
        public string CaseName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.BoxNo), ResourceType = typeof(Resources.ReferenceResource), Order = 12)]
        public string BoxNo { get; set; }

        /// <summary>
        /// 事業部コード
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DivisionId), ResourceType = typeof(Resources.ReferenceResource), Order = 13)]
        public string DivisionId { get; set; }

        /// <summary>
        /// 事業部名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DivisionId), ResourceType = typeof(Resources.ReferenceResource), Order = 14)]
        public string DivisionName { get; set; }

        /// <summary>
        /// カテゴリ1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId1), ResourceType = typeof(Resources.ReferenceResource), Order = 15)]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// カテゴリ1名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId1), ResourceType = typeof(Resources.ReferenceResource), Order = 16)]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// カテゴリ2
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId2), ResourceType = typeof(Resources.ReferenceResource), Order = 17)]
        public string CategoryId2 { get; set; }

        /// <summary>
        /// カテゴリ2名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId2), ResourceType = typeof(Resources.ReferenceResource), Order = 18)]
        public string CategoryName2 { get; set; }

        /// <summary>
        /// カテゴリ3
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId3), ResourceType = typeof(Resources.ReferenceResource), Order = 19)]
        public string CategoryId3 { get; set; }

        /// <summary>
        /// カテゴリ3名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId3), ResourceType = typeof(Resources.ReferenceResource), Order = 20)]
        public string CategoryName3 { get; set; }

        /// <summary>
        /// カテゴリ4
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId4), ResourceType = typeof(Resources.ReferenceResource), Order = 21)]
        public string CategoryId4 { get; set; }

        /// <summary>
        /// カテゴリ4名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId4), ResourceType = typeof(Resources.ReferenceResource), Order = 22)]
        public string CategoryName4 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemId), ResourceType = typeof(Resources.ReferenceResource), Order = 23)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemName), ResourceType = typeof(Resources.ReferenceResource), Order = 24)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColorId), ResourceType = typeof(Resources.ReferenceResource), Order = 25)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColorId), ResourceType = typeof(Resources.ReferenceResource), Order = 26)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSizeId), ResourceType = typeof(Resources.ReferenceResource), Order = 27)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSizeId), ResourceType = typeof(Resources.ReferenceResource), Order = 28)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Jan), ResourceType = typeof(Resources.ReferenceResource), Order = 29)]
        public string Jan { get; set; }

        /// <summary>
        /// 帳簿在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.StockQtyStart), ResourceType = typeof(Resources.ReferenceResource), Order = 30)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int StockQtyStart { get; set; }

        /// <summary>
        /// 実棚数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ResultQty), ResourceType = typeof(Resources.ReferenceResource), Order = 31)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DifferenceQty), ResourceType = typeof(Resources.ReferenceResource), Order = 32)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceQty { get; set; }

        /// <summary>
        /// カウント回数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CountSeq), ResourceType = typeof(Resources.ReferenceResource), Order = 33)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CountSeq { get; set; }

        /// <summary>
        /// 担当者コード
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.UserId), ResourceType = typeof(Resources.ReferenceResource), Order = 34)]
        public string UserId { get; set; }

        /// <summary>
        /// 担当者名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.UserId), ResourceType = typeof(Resources.ReferenceResource), Order = 35)]
        public string UserName { get; set; }

        #endregion プロパティ
    }
}