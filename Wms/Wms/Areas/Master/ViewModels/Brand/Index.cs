namespace Wms.Areas.Master.ViewModels.Brand
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    public class BrandSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum BrandSortKey : byte
        {
            [Display(Name = nameof(Resources.BrandResource.BrandId), ResourceType = typeof(Resources.BrandResource))]
            BrandId,

            [Display(Name = nameof(Resources.BrandResource.BrandName), ResourceType = typeof(Resources.BrandResource))]
            BrandName
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
        /// Sort key
        /// </summary>
        public BrandSortKey SortKey { get; set; }

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
    public class BrandResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Models.Brand> Brands { get; set; }
    }

    public class Index
    {
        public BrandSearchCondition SearchConditions { get; set; }

        public BrandResult BrandResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new BrandSearchCondition();
            this.BrandResult = new BrandResult();
        }
    }
}