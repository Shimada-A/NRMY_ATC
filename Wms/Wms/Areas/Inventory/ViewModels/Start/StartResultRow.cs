namespace Wms.Areas.Inventory.ViewModels.Start
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// 棚卸開始処理明細
    /// </summary>
    public class StartResultRow
    {
        #region プロパティ

        /// <summary>
        /// 棚卸No
        /// </summary>
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸開始日時
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸名称
        /// </summary>
        public string InventoryName { get; set; }

        /// <summary>
        /// 棚卸区分名
        /// </summary>
        /// <remarks>
        public string InventoryClassName { get; set; }

        /// <summary>
        /// 外装棚卸名
        /// </summary>
        /// <remarks>
        public string SimpleInventoryFlagName { get; set; }

        #endregion プロパティ
    }
}