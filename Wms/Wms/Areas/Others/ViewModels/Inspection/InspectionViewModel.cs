namespace Wms.Areas.Others.ViewModels.Inspection
{
    using PagedList;

    public class InspectionResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<InspectionResultRow> Inspections { get; set; }
    }

    public class InspectionViewModel
    {
        public InspectionSearchConditions SearchConditions { get; set; }

        public InspectionResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InspectionViewModel"/> class.
        /// </summary>
        public InspectionViewModel()
        {
            this.SearchConditions = new InspectionSearchConditions();
            this.Results = new InspectionResult();
        }
    }
}