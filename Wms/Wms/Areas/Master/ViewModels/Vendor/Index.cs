namespace Wms.Areas.Master.ViewModels.Vendor
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class VendorSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum VendorSortKey : byte
        {
            [Display(Name = nameof(Resources.VendorResource.VendorId), ResourceType = typeof(Resources.VendorResource))]
            VendorId,

            [Display(Name = nameof(Resources.VendorResource.VendorName), ResourceType = typeof(Resources.VendorResource))]
            VendorName
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
        /// 仕入先コード
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public VendorSortKey SortKey { get; set; }

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

    public class VendorResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Models.Vendor> Vendors { get; set; }
    }

    public class Index
    {
        public VendorSearchCondition SearchConditions { get; set; }

        public VendorResult VendorResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new VendorSearchCondition();
            this.VendorResult = new VendorResult();
        }
    }
}