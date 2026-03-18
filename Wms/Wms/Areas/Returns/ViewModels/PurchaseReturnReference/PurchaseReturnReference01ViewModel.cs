namespace Wms.Areas.Returns.ViewModels.PurchaseReturnReference
{
    using PagedList;

    public class PurchaseReturnReference01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<PurchaseReturnReference01ResultRow> PurchaseReturnReference01s { get; set; }
    }

    public class PurchaseReturnReference01ViewModel
    {
        public PurchaseReturnReference01SearchConditions SearchConditions { get; set; }

        public PurchaseReturnReference01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseReturnReference01ViewModel"/> class.
        /// </summary>
        public PurchaseReturnReference01ViewModel()
        {
            this.SearchConditions = new PurchaseReturnReference01SearchConditions();
            this.Results = new PurchaseReturnReference01Result();
        }
    }
}