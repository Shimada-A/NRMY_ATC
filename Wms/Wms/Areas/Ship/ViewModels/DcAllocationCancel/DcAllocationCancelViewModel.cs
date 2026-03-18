namespace Wms.Areas.Ship.ViewModels.DcAllocationCancel
{
    using PagedList;

    public class DcAllocationCancelResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<DcAllocationCancelResultRow> DcAllocationCancels { get; set; }
    }

    public class DcAllocationCancelViewModel
    {
        public DcAllocationCancelSearchConditions SearchConditions { get; set; }

        public DcAllocationCancelResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInquiryViewModel"/> class.
        /// </summary>
        public DcAllocationCancelViewModel()
        {
            this.SearchConditions = new DcAllocationCancelSearchConditions();
            this.Results = new DcAllocationCancelResult();
        }
    }
}