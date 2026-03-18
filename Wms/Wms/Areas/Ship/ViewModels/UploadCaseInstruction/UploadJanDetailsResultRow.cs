namespace Wms.Areas.Ship.ViewModels.UploadCaseInstruction
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// JAN抜き取り明細
    /// </summary>
    public class UploadJanDetailsResultRow
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
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

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
        /// 予定数
        /// </summary>
        public int? InstructQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        public int? AllocQty { get; set; }

        #endregion プロパティ
    }
}