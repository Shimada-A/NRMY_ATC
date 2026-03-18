namespace Wms.Areas.Inventory.ViewModels.Reference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Inventory.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class Reference01SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum ReferenceSortKey : byte
        {
            [Display(Name = nameof(ReferenceResource.InventoryNo), ResourceType = typeof(Resources.ReferenceResource))]
            InventoryNo,
            [Display(Name = nameof(ReferenceResource.InventoryStartDateNo), ResourceType = typeof(Resources.ReferenceResource))]
            InventoryStartDateNo,
            [Display(Name = nameof(ReferenceResource.InventoryNameNo), ResourceType = typeof(Resources.ReferenceResource))]
            InventoryNameNo,
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 棚卸区分
        /// </summary>
        public string InventoryClass { get; set; } = "2";

        /// <summary>
        /// 棚卸名称
        /// </summary>
        public string InventoryName { get; set; }

        /// <summary>
        /// 棚卸開始日From
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? InventoryDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 棚卸開始日To
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? InventoryDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 棚卸状況
        /// </summary>
        public string InventoryStatus { get; set; }

        /// <summary>
        /// 差異発生している棚卸のみ表示する
        /// </summary>
        public bool InventoryStatusOld { get; set; }

        /// <summary>
        /// 棚卸No
        /// </summary>
        public string InventoryNo { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        public string LocationCd { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// Detail Sort key
        /// </summary>
        public ReferenceSortKey SortKey { get; set; } = ReferenceSortKey.InventoryNo;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// 選択中数
        /// </summary>
        public int? SelectedCnt { get; set; } = 0;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        public IList<SelectedReferenceViewModel> Reference01s { get; set; }

        /// <summary>
        /// 選択中実績済数
        /// </summary>
        public int? ResultCnt { get; set; } = 0;

        /// <summary>
        /// 選択中実績済数
        /// </summary>
        public int? InventoryCnt { get; set; } = 0;

    }
}