namespace Wms.Areas.Ship.ViewModels.SortSet
{
    using PagedList;

    public class SortSetResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<SortSetResultRow> SortSets { get; set; }
    }

    public class SortSetViewModel
    {
        public SortSetSearchConditions SearchConditions { get; set; }

        public SortSetResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortSetViewModel"/> class.
        /// </summary>
        public SortSetViewModel()
        {
            this.SearchConditions = new SortSetSearchConditions();
            this.Results = new SortSetResult();
        }
    }
}