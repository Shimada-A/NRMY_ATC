namespace Wms.Areas.Ship.ViewModels.SortSet
{
    using PagedList;
    using Share.Common.Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class SortSetSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum SortSetSortKey : byte
        {
            [Display(Name = nameof(SortSetResource.SortDate), ResourceType = typeof(SortSetResource))]
            SortDate,

            [Display(Name = nameof(SortSetResource.SortId), ResourceType = typeof(SortSetResource))]
            SortId
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(FormsResource.ASC), ResourceType = typeof(FormsResource))]
            Asc,

            [Display(Name = nameof(FormsResource.DESC), ResourceType = typeof(FormsResource))]
            Desc
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// Sort key
        /// </summary>
        public SortSetSortKey SortKey { get; set; } = SortSetSortKey.SortDate;

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
        /// Row on page
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;


    }
}