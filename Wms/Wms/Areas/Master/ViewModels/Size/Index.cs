namespace Wms.Areas.Master.ViewModels.Size
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Common;

    public class SizeSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum SizeSortKey : byte
        {
            [Display(Name = nameof(Resources.SizeResource.ItemSizeId), ResourceType = typeof(Resources.SizeResource))]
            ItemSizeId,

            [Display(Name = nameof(Resources.SizeResource.ItemSizeName), ResourceType = typeof(Resources.SizeResource))]
            ItemSizeName
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
        /// Sort key
        /// </summary>
        public SizeSortKey SortKey { get; set; }

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
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名 (ITEM_SIZE_NAME)
        /// </summary>
        public string ItemSizeName { get; set; }
    }

    public class SizeResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Models.Size> Sizes { get; set; }
    }

    public class Index
    {
        public SizeSearchCondition SearchConditions { get; set; }

        public SizeResult SizeResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new SizeSearchCondition();
            this.SizeResult = new SizeResult();
        }
    }
}