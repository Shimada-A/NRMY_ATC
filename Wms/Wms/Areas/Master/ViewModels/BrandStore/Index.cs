namespace Wms.Areas.Master.ViewModels.BrandStore
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    public class BrandStoreSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum BrandStoreSortKey : byte
        {
            [Display(Name = nameof(Resources.BrandStoreResource.SortStoreIdBrandId), ResourceType = typeof(Resources.BrandStoreResource))]
            SortStoreIdBrandId,

            [Display(Name = nameof(Resources.BrandStoreResource.SortBrandIdStoreId), ResourceType = typeof(Resources.BrandStoreResource))]
            SortBrandIdStoreId
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
        /// 店舗ID
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 店舗名
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名 (BRAND_NAME)
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public BrandStoreSortKey SortKey { get; set; }

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
    public class BrandStoreResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Detail> BrandStores { get; set; }
    }

    public class Index
    {
        public BrandStoreSearchCondition SearchConditions { get; set; }

        public BrandStoreResult BrandStoreResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new BrandStoreSearchCondition();
            this.BrandStoreResult = new BrandStoreResult();
        }
    }
}