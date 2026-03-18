namespace Wms.Areas.Others.ViewModels.WorkReference
{
    using PagedList;

    public class WorkReferenceResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<WorkReferenceResultRow> WorkReferences { get; set; }

    }

    public class WorkReferenceViewModel
    {
        public WorkReferenceSearchConditions SearchConditions { get; set; }

        public WorkReferenceResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkReferenceViewModel"/> class.
        /// </summary>
        public WorkReferenceViewModel()
        {
            this.SearchConditions = new WorkReferenceSearchConditions();
            this.Results = new WorkReferenceResult();
        }
    }
}