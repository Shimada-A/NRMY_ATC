namespace Wms.Areas.Ship.ViewModels.EcAllocationCancel
{
    using PagedList;

    public class EcAllocationCancelResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<EcAllocationCancelResultRow> EcAllocationCancels { get; set; }
    }

    public class EcAllocationCancelViewModel
    {
        public EcAllocationCancelSearchConditions SearchConditions { get; set; }

        public EcAllocationCancelResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInquiryViewModel"/> class.
        /// </summary>
        public EcAllocationCancelViewModel()
        {
            this.SearchConditions = new EcAllocationCancelSearchConditions();
            this.Results = new EcAllocationCancelResult();
        }
    }
}