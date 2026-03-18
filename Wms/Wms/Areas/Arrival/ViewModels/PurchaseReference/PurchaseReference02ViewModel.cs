namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using PagedList;

    public class PurchaseReference02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<PurchaseReference02ResultRow> PurchaseReference02s { get; set; }
    }

    public class PurchaseReference02ViewModel
    {
        public PurchaseReference02SearchConditions SearchConditions { get; set; }

        public PurchaseReference02Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseReference02ViewModel"/> class.
        /// </summary>
        public PurchaseReference02ViewModel()
        {
            this.SearchConditions = new PurchaseReference02SearchConditions();
            this.Results = new PurchaseReference02Result();
        }
    }
}