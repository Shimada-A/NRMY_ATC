namespace Wms.ViewModels.Notice
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class NoticeSearchConditions
    {
        /// <summary>
        /// ソート内容
        /// </summary>
        public enum NoticeSortKey : byte
        {
            [Display(Name = nameof(Resources.NoticeResource.OccurrenceDate), ResourceType = typeof(Resources.NoticeResource))]
            OccurrenceDate
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
        /// 種別(メッセージ区分)
        /// </summary>
        public string MessageClassValue { get; set; }

        /// <summary>
        /// 発生日(From) (OCCURRENCE_DATE)
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? OccurrenceDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 発生日(To) (OCCURRENCE_DATE)
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? OccurrenceDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// ソート内容
        /// </summary>
        public NoticeSortKey SortKey { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

    }
}