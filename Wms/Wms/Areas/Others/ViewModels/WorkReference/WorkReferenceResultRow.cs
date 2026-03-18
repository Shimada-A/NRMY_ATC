namespace Wms.Areas.Others.ViewModels.WorkReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Others.Resources;
    using Wms.Common;

    /// <summary>
    /// 作業実績明細
    /// </summary>
    public class WorkReferenceResultRow
    {
        #region プロパティ

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Display(Name = "LineNo", ResourceType = typeof(WorkReferenceResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 作業種類 (PROCESSING_TYPE)
        /// </summary>
        [Display(Name = "ProcessingType", ResourceType = typeof(WorkReferenceResource))]
        public string ProcessingName { get; set; }

        /// <summary>
        /// 作業者ID (WORK_USER_ID)
        /// </summary>
        [Display(Name = "WorkUser", ResourceType = typeof(WorkReferenceResource))]
        public string WorkUserId { get; set; }

        /// <summary>
        /// 作業者名 (WORK_USER_NAME)
        /// </summary>
        [Display(Name = "WorkUser", ResourceType = typeof(WorkReferenceResource))]
        public string WorkUserName { get; set; }

        /// <summary>
        /// 作業開始日時 (WORK_START_DATE)
        /// </summary>
        [Display(Name = "WorkStartDate", ResourceType = typeof(WorkReferenceResource))]
        public DateTime? WorkStartDate { get; set; }

        /// <summary>
        /// 作業完了日時 (WORK_END_DATE)
        /// </summary>
        [Display(Name = "WorkEndDate", ResourceType = typeof(WorkReferenceResource))]
        public DateTime? WorkEndDate { get; set; }

        /// <summary>
        /// 中断時間 (WORK_BREAK_TIME)
        /// </summary>
        [Display(Name = "WorkBreakTime", ResourceType = typeof(WorkReferenceResource))]
        public string WorkBreakTime { get; set; }

        /// <summary>
        /// 作業時間 (WORK_TIME)
        /// </summary>
        [Display(Name = "WorkTime", ResourceType = typeof(WorkReferenceResource))]
        public string WorkTime { get; set; }

        /// <summary>
        /// 作業数 (WORK_QTY)
        /// </summary>
        [Display(Name = "WorkQty", ResourceType = typeof(WorkReferenceResource))]
        public int WorkQty { get; set; }

        /// <summary>
        /// ステータス (WORK_STATUS)
        /// </summary>
        [Display(Name = "WorkStatus", ResourceType = typeof(WorkReferenceResource))]
        public string WorkStatusName { get; set; }

        /// <summary>
        /// 備考 (PROCESSING_MESSAGE)
        /// </summary>
        [Display(Name = "ProcessingMessage", ResourceType = typeof(WorkReferenceResource))]
        public string ProcessingMessage { get; set; }

        #endregion プロパティ
    }
}