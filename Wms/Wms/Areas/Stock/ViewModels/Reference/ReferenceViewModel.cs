namespace Wms.Areas.Stock.ViewModels.Reference
{
    using PagedList;

    public class ReferenceResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<ReferenceResultRow> References { get; set; }
    }

    public class ReferenceViewModel
    {
        public ReferenceSearchConditions SearchConditions { get; set; }

        public ReferenceResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceViewModel"/> class.
        /// </summary>
        public ReferenceViewModel()
        {
            this.SearchConditions = new ReferenceSearchConditions();
            this.Results = new ReferenceResult();
        }
    }
}