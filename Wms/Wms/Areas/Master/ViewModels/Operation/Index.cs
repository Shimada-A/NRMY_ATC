namespace Wms.Areas.Master.ViewModels.Operation
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class OperationSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum OperationSortKey : byte
        {
            [Display(Name = nameof(Resources.OperationResource.SortCategoryOperationId), ResourceType = typeof(Resources.OperationResource))]
            CategoryOperationId,

            [Display(Name = nameof(Resources.OperationResource.OperationId), ResourceType = typeof(Resources.OperationResource))]
            OperationId

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
        /// 業務ID (OPERATION_ID)
        /// </summary>
        public string OperationId { get; set; }

        /// <summary>
        /// 業務名 (OPERATION_NAME)
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 業務カテゴリ名 (CATEGORY_NAME)
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// WMS業務フラグ (WMS_OPERATION_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：WMS業務外、1：WMS業務
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(OperationResource.WmsOperationFlag), ResourceType = typeof(OperationResource))]
        public bool WmsOperationFlag { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public OperationSortKey SortKey { get; set; }

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

    public class OperationResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Detail> Operations { get; set; }
    }

    public class Index
    {
        public OperationSearchCondition SearchConditions { get; set; }

        public OperationResult OperationResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockingPatternViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new OperationSearchCondition();
            this.OperationResult = new OperationResult();
        }
    }
}