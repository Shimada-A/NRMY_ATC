namespace Wms.Areas.Ship.ViewModels.DcAllocation
{
    using PagedList;
    using System;
    using Wms.ViewModels.Shared;
    using static Wms.Areas.Ship.ViewModels.DcAllocation.DcAllocationSearchConditions;

    public class DcAllocationResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<DcAllocationResultRow> DcAllocations { get; set; }
    }

    public class DcAllocationViewModel
    {
        public DcAllocationSearchConditions SearchConditions { get; set; }

        public DcAllocationResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInquiryViewModel"/> class.
        /// </summary>
        public DcAllocationViewModel()
        {
            this.SearchConditions = new DcAllocationSearchConditions() ;
            this.Results = new DcAllocationResult();
        }
    }
}