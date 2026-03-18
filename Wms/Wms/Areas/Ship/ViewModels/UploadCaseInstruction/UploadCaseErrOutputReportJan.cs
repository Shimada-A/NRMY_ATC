namespace Wms.Areas.Ship.ViewModels.UploadCaseInstruction
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;

    /// <summary>
    /// ケース出荷指示(JAN抜取)直近取込エラーデータDownLoadRow
    /// </summary>
    public class UploadCaseErrReportJan
    {
        #region プロパティ
        /// <summary>
        /// 取込日時
        /// </summary>
        public string MakeDate { get; set; }

        /// <summary>
        /// 担当者
        /// </summary>
        public string MakeUserId { get; set; }

        /// <summary>
        /// ケース出荷指示名称
        /// </summary>
        public string ShipInstructName { get; set; }

        /// <summary>
        /// エラー行No
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// 店舗ID
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 優先順
        /// </summary>
        public string PrioritOrder { get; set; }

        /// <summary>
        /// 抜き取りJAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public string InstructQty { get; set; }

        /// <summary>
        /// エラー内容
        /// </summary>
        public string ErrMessage { get; set; }
        #endregion プロパティ
    }
}