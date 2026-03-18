namespace Wms.Areas.Master.ViewModels.General
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class GeneralSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum GeneralSortKey : byte
        {
            [Display(Name = nameof(Resources.GeneralResource.CenterId), ResourceType = typeof(Resources.GeneralResource))]
            CenterId,
            [Display(Name = nameof(Resources.GeneralResource.GenDivCd), ResourceType = typeof(Resources.GeneralResource))]
            GenDivCd
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
        /// List センターコード
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 汎用分類コード
        /// </summary>
        public string GenDivCd { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public GeneralSortKey SortKey { get; set; }

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

    public class GeneralResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<GeneralList> Generals { get; set; }
    }

    public class Index
    {
        public GeneralSearchCondition SearchConditions { get; set; }

        public GeneralResult GeneralResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new GeneralSearchCondition();
            this.GeneralResult = new GeneralResult();
        }
    }
}