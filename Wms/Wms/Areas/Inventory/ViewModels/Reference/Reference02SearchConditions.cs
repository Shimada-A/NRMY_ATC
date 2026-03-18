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
    public class Reference02SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum Reference02SortKey : byte
        {
            [Display(Name = nameof(ReferenceResource.LocationCaseGrade), ResourceType = typeof(Resources.ReferenceResource))]
            LocationCaseGrade,
            [Display(Name = nameof(ReferenceResource.ResultQtyLocation), ResourceType = typeof(Resources.ReferenceResource))]
            ResultQtyLocation,
            [Display(Name = nameof(ReferenceResource.DifferenceSumLocation), ResourceType = typeof(Resources.ReferenceResource))]
            DifferenceSumLocation,
        }

        /// <summary>
        /// Data to sort
        /// </summary>
        public enum Reference03SortKey : byte
        {
            [Display(Name = nameof(ReferenceResource.LocationCaseItemColorSize), ResourceType = typeof(Resources.ReferenceResource))]
            LocationCaseItemColorSize,
            [Display(Name = nameof(ReferenceResource.ItemColorSizeLocationCase), ResourceType = typeof(Resources.ReferenceResource))]
            ItemColorSizeLocationCase,
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum DetailAscDescSort
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 棚卸No
        /// </summary>
        public string InventoryNo { get; set; }

        /// <summary>
        /// 差異発生している棚卸のみ表示する
        /// </summary>
        public bool InventoryStatusOld { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 棚列
        /// </summary>
        public string InventoryRow { get; set; }

        /// <summary>
        /// ロケーション(FROM)
        /// </summary>
        public string LocationCdFrom { get; set; }

        /// <summary>
        /// ロケーション(TO)
        /// </summary>
        public string LocationCdTo { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        public string GradeId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// Detail Sort key
        /// </summary>
        public Reference02SortKey SortKey02 { get; set; } = Reference02SortKey.LocationCaseGrade;

        /// <summary>
        /// Detail Sort key
        /// </summary>
        public Reference03SortKey SortKey03 { get; set; } = Reference03SortKey.LocationCaseItemColorSize;

        /// <summary>
        /// Sort
        /// </summary>
        public DetailAscDescSort Sort { get; set; }

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
        /// ワーク02ID (SEQ)
        /// </summary>
        public long Seq2 { get; set; }

        /// <summary>
        /// ワーク02ID (SEQ)
        /// </summary>
        public long Seq3 { get; set; }

        /// <summary>
        /// ワーク04ID (SEQ)
        /// </summary>
        public long Seq4 { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        public IList<SelectedReferenceViewModel> Reference02s { get; set; }

    }
}