namespace Wms.Areas.Master.ViewModels.ItemSku
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class ItemSkuSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum ItemSkuSortKey : byte
        {
            [Display(Name = nameof(Resources.ItemSkuResource.ItemSkuId), ResourceType = typeof(Resources.ItemSkuResource))]
            ItemSkuId,

            [Display(Name = nameof(Resources.ItemSkuResource.Jan), ResourceType = typeof(Resources.ItemSkuResource))]
            Jan,

            [Display(Name = nameof(Resources.ItemSkuResource.ItemName), ResourceType = typeof(Resources.ItemSkuResource))]
            ItemName

            
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
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 代表仕入先所在ID (MAIN_VENDOR_ID)
        /// </summary>
        public string MainVendorId { get; set; }

        /// <summary>
        /// 代表仕入先所在 (MAIN_VENDOR_NAME)
        /// </summary>
        public string MainVendorName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 事業部CD (DIVISION_ID)
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 商品ランクID (ITEM_RANK_ID)
        /// </summary>
        public string ItemRankId { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public ItemSkuSortKey SortKey { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// 削除フラグ
        /// </summary>
        public bool DeleteFlag { get; set; }
    }

    public class ItemSkuResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Detail> ItemSkus { get; set; }
    }

    public class Index
    {
        public ItemSkuSearchCondition SearchConditions { get; set; }

        public ItemSkuResult ItemSkuResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemSkuViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new ItemSkuSearchCondition();
            this.ItemSkuResult = new ItemSkuResult();
        }
    }
}