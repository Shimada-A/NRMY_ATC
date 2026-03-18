namespace Wms.Areas.Arrival.ViewModels.InputPurchase
{
    using PagedList;

    public class InputPurchase01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<InputPurchase01ResultRow> InputPurchase01s { get; set; }
    }

    public class InputPurchase01ViewModel
    {
        public InputPurchase01SearchConditions SearchConditions { get; set; }

        public InputPurchase01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputPurchase01ViewModel"/> class.
        /// </summary>
        public InputPurchase01ViewModel()
        {
            this.SearchConditions = new InputPurchase01SearchConditions();
            this.Results = new InputPurchase01Result();
        }
    }
}