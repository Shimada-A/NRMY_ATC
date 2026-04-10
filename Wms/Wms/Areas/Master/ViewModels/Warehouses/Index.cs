namespace Wms.Areas.Master.ViewModels.Warehouses
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class WarehousesSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum WarehousesSortKey : byte
        {
            [Display(Name = nameof(Resources.WarehousesResource.CenterId), ResourceType = typeof(Resources.WarehousesResource))]
            CenterId,

            [Display(Name = nameof(Resources.WarehousesResource.CenterName), ResourceType = typeof(Resources.WarehousesResource))]
            CenterName
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
        /// 倉庫ID
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 倉庫名
        /// </summary>
        public string CenterName { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        public string CenterAddress { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        public string CenterZip {get; set; }

        /// <summary>
        /// TEL
        /// </summary>
        [RegularExpression(@"[0-9]+", ErrorMessage = "TELはハイフンなしで入力してください。")]
        public string CenterTel { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public WarehousesSortKey SortKey { get; set; }

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
    }

    public class WarehousesResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Models.Warehouses> Warehouses { get; set; }
    }

    public class Index
    {
        public WarehousesSearchCondition SearchConditions { get; set; }

        public WarehousesResult WarehouseResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehousesViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new WarehousesSearchCondition();
            this.WarehouseResult = new WarehousesResult();
        }
    }
}