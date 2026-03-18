namespace Wms.Areas.Master.ViewModels.NaniwaSorting
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class NaniwaSortingSearchCondition
    {
        /// <summary>
        /// 表示順
        /// </summary>
        public enum NaniwaSortingSortKey : byte
        {
            [Display(Name = nameof(Resources.NaniwaSortingResource.StoreId), ResourceType = typeof(Resources.NaniwaSortingResource))]
            StoreId,
            [Display(Name = nameof(Resources.NaniwaSortingResource.NaniwaDeliCenterCdStoreId), ResourceType = typeof(Resources.NaniwaSortingResource))]
            NaniwaDeliCenterCdStoreId
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
        /// 配送センターコード
        /// </summary>
        public string NaniwaDeliCenterCd { get; set; }

        /// <summary>
        /// 配送センター名称
        /// </summary>
        public string NaniwaDeliCenterName { get; set; }

        /// <summary>
        /// 表示順
        /// </summary>
        public NaniwaSortingSortKey SortKey { get; set; }

        /// <summary>
        /// 昇順降順
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
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }
    }

    public class NaniwaSortingResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<NaniwaSortingList> NaniwaSortings { get; set; }
    }

    public class Index
    {
        public NaniwaSortingSearchCondition SearchConditions { get; set; }

        public NaniwaSortingResult NaniwaSortingResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NaniwaSortingViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new NaniwaSortingSearchCondition();
            this.NaniwaSortingResult = new NaniwaSortingResult();
        }
    }
}