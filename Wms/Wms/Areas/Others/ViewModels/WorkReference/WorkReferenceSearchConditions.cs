namespace Wms.Areas.Others.ViewModels.WorkReference
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Others.Resources;
    using Wms.Common;
    using Wms.ViewModels.Shared;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class WorkReferenceSearchConditions : IndicateViewModel
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum WorkReferenceSortKey : byte
        {
            [Display(Name = nameof(WorkReferenceResource.WorkStartDate), ResourceType = typeof(WorkReferenceResource))]
            WorkStartDate,

            [Display(Name = nameof(WorkReferenceResource.ProcessingTypeWorkStartDate), ResourceType = typeof(WorkReferenceResource))]
            ProcessingTypeWorkStartDate
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
        /// Detail Sort key
        /// </summary>
        public WorkReferenceSortKey SortKey { get; set; } = WorkReferenceSortKey.WorkStartDate;

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
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 作業開始日付From
        /// </summary>
        public DateTime? WorkStartDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 作業開始時刻From
        /// </summary>
        public string WorkStartTimeFrom { get; set; }

        /// <summary>
        /// 作業開始日付To
        /// </summary>
        public DateTime? WorkStartDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 作業開始時刻To
        /// </summary>
        public string WorkStartTimeTo { get; set; }


        /// <summary>
        /// 作業種類
        /// </summary>
        public string ProcessingType { get; set; }

        /// <summary>
        /// 作業者ID
        /// </summary>
        public string WorkUserId { get; set; }

        /// <summary>
        /// 作業者名
        /// </summary>
        public string WorkUserName { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string WorkStatus { get; set; }
    }
}