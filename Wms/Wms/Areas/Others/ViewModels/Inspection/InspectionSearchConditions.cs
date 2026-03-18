using System;
namespace Wms.Areas.Others.ViewModels.Inspection
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Others.Resources;
    using Wms.Common;
    using Wms.Resources;

    public class InspectionSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum ArrivalSortKey : byte
        {
            [Display(Name = nameof(InspectionResource.MakeDate), ResourceType = typeof(InspectionResource))]
            MakeDate,
            [Display(Name = nameof(InspectionResource.MakeUserId), ResourceType = typeof(InspectionResource))]
            MakeUserId,
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
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// Sort key
        /// </summary>
        public ArrivalSortKey SortKey { get; set; } = ArrivalSortKey.MakeDate;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;


        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 作業日From
        /// </summary>
        public DateTime? MakeDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 作業時From
        /// </summary>
        public string MakeTimeFrom { get; set; }

        /// <summary>
        /// 作業日To
        /// </summary>
        public DateTime? MakeDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 作業時To
        /// </summary>
        public string MakeTimeTo { get; set; }

        /// <summary>
        /// 作業分類(PROCESSING_ID)
        /// </summary>
        public string ProcessingId { get; set; }

        /// <summary>
        /// 作業者(MAKE_USER_ID)
        /// </summary>
        public string MakeUserId { get; set; }

    }
}