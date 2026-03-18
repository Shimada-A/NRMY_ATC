namespace Wms.Areas.Master.ViewModels.Division
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    public class DivisionSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum DivisionSortKey : byte
        {
            [Display(Name = nameof(Resources.DivisionResource.DivisionId), ResourceType = typeof(Resources.DivisionResource))]
            DivisionId,

            [Display(Name = nameof(Resources.DivisionResource.DivisionName), ResourceType = typeof(Resources.DivisionResource))]
            DivisionName
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
        /// Sort key
        /// </summary>
        public DivisionSortKey SortKey { get; set; }

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
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 事業部ID (DIVISION_ID)
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// 事業部名 (DIVISION_NAME)
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// 事業部記号 (DIVISION_MARK)
        /// </summary>
        public string DivisionMark { get; set; }
    }

    public class DivisionResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Models.Division> Divisions { get; set; }
    }


    public class Index
    {
        public DivisionSearchCondition SearchConditions { get; set; }

        public DivisionResult DivisionResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new DivisionSearchCondition();
            this.DivisionResult = new DivisionResult();
        }
    }
}