namespace Wms.Areas.Others.ViewModels.WorkingReference
{
    using PagedList;

    public class WorkingReferenceResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<WorkingReferenceResultRow> WorkingReferences { get; set; }

    }

    public class WorkingReferenceViewModel
    {
        public WorkingReferenceSearchConditions SearchConditions { get; set; }

        public WorkingReferenceResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkingReferenceViewModel"/> class.
        /// </summary>
        public WorkingReferenceViewModel()
        {
            this.SearchConditions = new WorkingReferenceSearchConditions();
            this.Results = new WorkingReferenceResult();
        }
    }
}