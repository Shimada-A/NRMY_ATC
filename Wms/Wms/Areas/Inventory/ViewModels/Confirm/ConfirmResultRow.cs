namespace Wms.Areas.Inventory.ViewModels.Confirm
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// 在庫明細
    /// </summary>
    public class ConfirmResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 棚卸No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ConfirmResource.InventoryNo), ResourceType = typeof(Resources.ConfirmResource))]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸開始日時
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(Resources.ConfirmResource.InventoryStartDate), ResourceType = typeof(Resources.ConfirmResource))]
        public DateTime? InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ConfirmResource.InventoryClass), ResourceType = typeof(Resources.ConfirmResource))]
        public string InventoryClass { get; set; }

        /// <summary>
        /// 棚卸名称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ConfirmResource.InventoryName), ResourceType = typeof(Resources.ConfirmResource))]
        public string InventoryName { get; set; }

        /// <summary>
        /// 棚卸確定フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ConfirmResource.InventoryConfirmFlag), ResourceType = typeof(Resources.ConfirmResource))]
        public string InventoryConfirmFlag { get; set; }

        /// <summary>
        /// 棚卸確定日時
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ConfirmResource.InventoryConfirmDate), ResourceType = typeof(Resources.ConfirmResource))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? InventoryConfirmDate { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; }

        /// <summary>
        /// 棚卸実績Flag
        /// </summary>
        /// <remarks>
        public int ResultsDataFlag { get; set; }

        #endregion プロパティ
    }
}