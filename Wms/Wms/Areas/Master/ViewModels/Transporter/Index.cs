namespace Wms.Areas.Master.ViewModels.Transporter
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class TransporterSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum TransporterSortKey : byte
        {
            [Display(Name = nameof(Resources.TransporterResource.TransporterId), ResourceType = typeof(Resources.TransporterResource))]
            TransporterId,

            [Display(Name = nameof(Resources.TransporterResource.TransporterName), ResourceType = typeof(Resources.TransporterResource))]
            TransporterName
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
        /// 配送業者ID
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者名
        /// </summary>
        public string TransporterName { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public TransporterSortKey SortKey { get; set; }

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

    public class TransporterResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Models.Transporter> Transporters { get; set; }
    }

    public class Index
    {
        public TransporterSearchCondition SearchConditions { get; set; }

        public TransporterResult TransporterResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransporterViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new TransporterSearchCondition();
            this.TransporterResult = new TransporterResult();
        }
    }
}