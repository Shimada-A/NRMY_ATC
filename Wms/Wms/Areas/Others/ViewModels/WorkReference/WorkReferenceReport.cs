namespace Wms.Areas.Others.ViewModels.WorkReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Others.Resources;

    public class WorkReferenceReport
    {
        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.WorkReferenceResource.Center), ResourceType = typeof(Resources.WorkReferenceResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// 作業種類
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.WorkReferenceResource.ProcessingType), ResourceType = typeof(Resources.WorkReferenceResource), Order = 2)]
        public string ProcessingName { get; set; }

        /// <summary>
        /// 作業者ID (WORK_USER_ID)
        /// </summary>
        [Display(Name = nameof(Resources.WorkReferenceResource.WorkUserId), ResourceType = typeof(Resources.WorkReferenceResource), Order = 3)]
        public string WorkUserId { get; set; }

        /// <summary>
        /// 作業者名 (WORK_USER_NAME)
        /// </summary>
        [Display(Name = nameof(Resources.WorkReferenceResource.WorkUserName), ResourceType = typeof(Resources.WorkReferenceResource), Order = 4)]
        public string WorkUserName { get; set; }

        /// <summary>
        /// 作業開始日時 (WORK_START_DATE)
        /// </summary>
        [Display(Name = nameof(Resources.WorkReferenceResource.WorkStartDate), ResourceType = typeof(Resources.WorkReferenceResource), Order = 5)]
        public DateTime? WorkStartDate { get; set; }

        /// <summary>
        /// 作業完了日時 (WORK_END_DATE)
        /// </summary>
        [Display(Name = nameof(Resources.WorkReferenceResource.WorkEndDate), ResourceType = typeof(Resources.WorkReferenceResource), Order = 6)]
        public DateTime? WorkEndDate { get; set; }

        /// <summary>
        /// 中断時間 (WORK_BREAK_TIME)
        /// </summary>
        [Display(Name = nameof(Resources.WorkReferenceResource.WorkBreakTime), ResourceType = typeof(Resources.WorkReferenceResource), Order = 7)]
        public string WorkBreakTime { get; set; }

        /// <summary>
        /// 作業時間 (WORK_TIME)
        /// </summary>
        [Display(Name = nameof(Resources.WorkReferenceResource.WorkTime), ResourceType = typeof(Resources.WorkReferenceResource), Order = 8)]
        public string WorkTime { get; set; }

        /// <summary>
        /// 作業数 (WORK_QTY)
        /// </summary>
        [Display(Name = nameof(Resources.WorkReferenceResource.WorkQty), ResourceType = typeof(Resources.WorkReferenceResource), Order = 9)]
        public int WorkQty { get; set; }

        /// <summary>
        /// ステータス (WORK_STATUS)
        /// </summary>
        [Display(Name = nameof(Resources.WorkReferenceResource.WorkStatus), ResourceType = typeof(Resources.WorkReferenceResource), Order = 10)]
        public string WorkStatusName { get; set; }

        /// <summary>
        /// 備考 (PROCESSING_MESSAGE)
        /// </summary>
        [Display(Name = nameof(Resources.WorkReferenceResource.ProcessingMessage), ResourceType = typeof(Resources.WorkReferenceResource), Order = 11)]
        public string ProcessingMessage { get; set; }
    }
}