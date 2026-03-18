namespace Wms.Areas.Ship.ViewModels.UploadCaseInstruction
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// ケース出荷明細
    /// </summary>
    public class UploadCaseDetailsResultRow
    {
        #region プロパティ

        /// No
        /// </summary>
        public int? LineNo { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 優先順
        /// </summary>
        public string PriorityOrder { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        public int? SkuQty { get; set; }

        /// <summary>
        /// 指示数
        /// </summary>
        public int? InstructQty { get; set; }

        #endregion プロパティ
    }
}