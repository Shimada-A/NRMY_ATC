namespace Wms.Areas.Master.ViewModels.StockingPattern
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class StockingPatternSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum StokingPatternSortKey : byte
        {
            [Display(Name = nameof(Resources.StockingPatternResource.PatternId), ResourceType = typeof(Resources.StockingPatternResource))]
            PatternId,

            [Display(Name = nameof(Resources.StockingPatternResource.PatternName), ResourceType = typeof(Resources.StockingPatternResource))]
            PatternName


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
        /// 仕分けパターンID
        /// </summary>
        public string PatternId { get; set; }

        /// <summary>
        /// 仕分けパターン名
        /// </summary>
        public string PatternName { get; set; }
        /// <summary>
        /// Sort key
        /// </summary>
        public StokingPatternSortKey SortKey { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }

    }

    public class StockingPatternResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Detail> StockingPatterns { get; set; }
    }

    public class Index
    {
        public StockingPatternSearchCondition SearchConditions { get; set; }

        public StockingPatternResult StockingPatternResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockingPatternViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new StockingPatternSearchCondition();
            this.StockingPatternResult = new StockingPatternResult();
        }
    }
}