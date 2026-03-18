namespace Wms.Areas.Stock.ViewModels.Adjust
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Stock.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class AdjustSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum AdjustSortKey : byte
        {
            [Display(Name = nameof(AdjustResource.LocationSkuGrade), ResourceType = typeof(AdjustResource))]
            LocationSkuGrade,

            [Display(Name = nameof(AdjustResource.SkuGradeLocation), ResourceType = typeof(AdjustResource))]
            SkuGradeLocation
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// ロケーション区分
        /// </summary>
        public string LocationClass { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        public string Locsec1 { get; set; }

        /// <summary>
        /// 代表仕入先
        /// </summary>
        public string MainVendorId { get; set; }

        /// <summary>
        /// 代表仕入先
        /// </summary>
        public string MainVendorName { get; set; }

        /// <summary>
        /// ロケーションFrom
        /// </summary>
        public string LocationCdFrom { get; set; }

        /// <summary>
        /// ロケーションTo
        /// </summary>
        public string LocationCdTo { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// アイテムコード
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        public string GradeId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public AdjustSortKey SortKey { get; set; } = AdjustSortKey.LocationSkuGrade;

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// ロケーション数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? LocationSum { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuSum { get; set; }

        /// <summary>
        /// 在庫数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQtySum { get; set; }

        /// <summary>
        /// 引当数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQtySum { get; set; }

        /// <summary>
        /// 未引当数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? UnAllocQtySum { get { return this.StockQtySum - this.AllocQtySum; } }
    }
}