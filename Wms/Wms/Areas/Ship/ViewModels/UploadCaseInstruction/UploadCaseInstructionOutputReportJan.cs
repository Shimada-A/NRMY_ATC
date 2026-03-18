namespace Wms.Areas.Ship.ViewModels.UploadCaseInstruction
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;

    /// <summary>
    /// アソートケース出荷指示(JAN抜取)テンプレート処理DownLoadRow
    /// </summary>
    public class UploadCaseInstructionReportJan
    {
        #region プロパティ

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [Display(Name = "ShipPlanDate", ResourceType = typeof(UploadCaseInstructionResource))]
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// 店舗ID
        /// </summary>
        [Display(Name = "ShipToStoreId", ResourceType = typeof(UploadCaseInstructionResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 優先順
        /// </summary>
        [Display(Name = "PrioritOrder", ResourceType = typeof(UploadCaseInstructionResource))]
        public string PrioritOrder { get; set; }

        /// <summary>
        /// 抜き取りJAN
        /// </summary>
        [Display(Name = "Jan", ResourceType = typeof(UploadCaseInstructionResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "InstructQty", ResourceType = typeof(UploadCaseInstructionResource))]
        public string InstructQty { get; set; }

        #endregion プロパティ
    }
}