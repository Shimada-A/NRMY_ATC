namespace Wms.Areas.Master.ViewModels.ShippingHoldStores
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    public class SearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum SortKeys : byte
        {
            [Display(Name = nameof(Resources.ShippingHoldStoreResource.SortKeyBrand), ResourceType = typeof(Resources.ShippingHoldStoreResource))]
            SortKeyBrand,

            [Display(Name = nameof(Resources.ShippingHoldStoreResource.SortKeyStore), ResourceType = typeof(Resources.ShippingHoldStoreResource))]
            SortKeyStore
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
        /// ブランドID (BRAND_ID)
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名 (BRAND_NAME)
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 出荷先ID (BRAND_ID)
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 出荷先名 (BRAND_NAME)
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public SortKeys SortKey { get; set; }

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
        /// ワークID
        /// </summary>
        public long? Seq { get; set; } = 0;

        /// <summary>
        /// ワーク更新フラグ
        /// </summary>
        public bool UpdateWorkFlag { get; set; } = false; 
    }
    public class Details
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Models.MasShippingHoldStore> ShippingHoldStores { get; set; }
    }

    public class Index
    {
        public SearchCondition SearchConditions { get; set; }

        public Details Details { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new SearchCondition();
            this.Details = new Details();

        }
    }
}