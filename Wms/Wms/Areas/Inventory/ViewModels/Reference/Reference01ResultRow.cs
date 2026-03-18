namespace Wms.Areas.Inventory.ViewModels.Reference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// 棚卸進捗照会
    /// </summary>
    public class Reference01ResultRow
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
        [Display(Name = nameof(Resources.ReferenceResource.InventoryNo), ResourceType = typeof(Resources.ReferenceResource))]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸開始日時
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(Resources.ReferenceResource.InventoryStartDate), ResourceType = typeof(Resources.ReferenceResource))]
        public DateTime? InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryClass), ResourceType = typeof(Resources.ReferenceResource))]
        public int InventoryClass { get; set; }

        /// <summary>
        /// 棚卸名称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryName), ResourceType = typeof(Resources.ReferenceResource))]
        public string InventoryName { get; set; }

        /// <summary>
        /// 帳簿在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.StockQtyStart), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int StockQtyStart { get; set; }

        /// <summary>
        /// 実棚数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ResultQty), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数(+)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DifferencePlus), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferencePlus { get; set; }

        /// <summary>
        /// 差異数(-)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DifferenceMinus), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceMinus { get; set; }

        /// <summary>
        /// 差異数合計
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DifferenceSum), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceSum { get; set; }

        /// <summary>
        /// 棚卸状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InventoryStatus), ResourceType = typeof(Resources.ReferenceResource))]
        public string InventoryStatus { get; set; }

        /// <summary>
        /// 棚卸実績Flag
        /// </summary>
        /// <remarks>
        public int ResultsDataFlag { get; set; }

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

        #endregion プロパティ
    }
}