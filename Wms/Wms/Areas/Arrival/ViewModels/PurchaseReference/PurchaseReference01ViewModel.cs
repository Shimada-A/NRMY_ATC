namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using PagedList;

    public class PurchaseReference01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<PurchaseReference01ResultRow> PurchaseReference01s { get; set; }
    }

    public class PurchaseReference01ViewModel
    {
        public PurchaseReference01SearchConditions SearchConditions { get; set; }

        public PurchaseReference01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseReference01ViewModel"/> class.
        /// </summary>
        public PurchaseReference01ViewModel()
        {
            this.SearchConditions = new PurchaseReference01SearchConditions();
            this.Results = new PurchaseReference01Result();
        }
    }
}