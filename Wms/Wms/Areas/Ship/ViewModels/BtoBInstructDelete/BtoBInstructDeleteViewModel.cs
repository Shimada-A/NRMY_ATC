namespace Wms.Areas.Ship.ViewModels.BtoBInstructDelete
{
    using PagedList;
    using Wms.ViewModels.Shared;

    public class BtoBInstructDeleteResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<BtoBInstructDeleteResultRow> BtoBInstructDeletes { get; set; }
    }

    public class BtoBInstructDeleteViewModel
    {
        public BtoBInstructDeleteSearchConditions SearchConditions { get; set; }

        public BtoBInstructDeleteResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInquiryViewModel"/> class.
        /// </summary>
        public BtoBInstructDeleteViewModel()
        {
            this.SearchConditions = new BtoBInstructDeleteSearchConditions() ;
            this.Results = new BtoBInstructDeleteResult();
        }
    }
}