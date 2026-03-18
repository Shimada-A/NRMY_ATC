namespace Wms.Areas.Ship.ViewModels.EcAllocation
{
    using PagedList;

    public class EcAllocationResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<EcAllocationResultRow> EcAllocations { get; set; }
    }

    public class EcAllocationViewModel
    {
        public EcAllocationSearchConditions SearchConditions { get; set; }

        public EcAllocationResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInquiryViewModel"/> class.
        /// </summary>
        public EcAllocationViewModel()
        {
            this.SearchConditions = new EcAllocationSearchConditions();
            this.Results = new EcAllocationResult();
        }
    }
}