namespace Wms.Areas.Stock.ViewModels.LocMove
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class LocMoveSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum LocMoveSort : byte
        {
            [Display(Name = nameof(Resources.LocMoveResource.Loc), ResourceType = typeof(Resources.LocMoveResource))]
            Loc,

            [Display(Name = nameof(Resources.LocMoveResource.GradeLoc), ResourceType = typeof(Resources.LocMoveResource))]
            GradeLoc
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
        /// ロケーション区分
        /// </summary>
        public string LocationClass { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        public string Locsec1 { get; set; }

        /// <summary>
        /// ロケーションFROM
        /// </summary>
        public string LocationCdFrom { get; set; }

        /// <summary>
        /// ロケーションTO
        /// </summary>
        public string LocationCdTo { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        public string GradeId { get; set; }

        /// <summary>
        /// 引当数0のみ表示
        /// </summary>
        public bool AllocQtyZero { get; set; } = true;

        /// <summary>
        /// 移動先ロケーション
        /// </summary>
        public string LocationCdMove { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; } = 0;

        /// <summary>
        /// Sort key
        /// </summary>
        public LocMoveSort SortKey { get; set; }

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
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// 選択されたロケ
        /// </summary>
        public IList<SelectedLocMoveViewModel> LocMoves { get; set; }

        /// <summary>
        /// 選択されたロケ数
        /// </summary>
        public int? SelectedCount { get;set; }

        /// <summary>
        /// ロケーション数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")] 
        public int? LocationCount { get;set; }

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQtySum { get; set; }

        /// <summary>
        /// 在庫数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQtySum { get; set; }

        /// <summary>
        /// 引当数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQtySum { get; set; }

        /// <summary>
        /// 未引当数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? NotAllocQtySum { get; set; }

    }
}