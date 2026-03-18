namespace Wms.Areas.Ship.ViewModels.UploadCaseInstruction
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;

    /// <summary>
    /// ケース出荷指示(ケース出荷)テンプレート処理DownLoadRow
    /// </summary>
    public class UploadCaseInstructionReportCase
    {
        #region プロパティ

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [Display(Name = "ShipPlanDate", ResourceType = typeof(UploadCaseInstructionResource))]
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        [Display(Name = "BoxNo", ResourceType = typeof(UploadCaseInstructionResource))]
        public string BoxNo { get; set; }

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

        #endregion プロパティ
    }
}