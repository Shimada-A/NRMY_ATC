namespace Wms.Areas.Move.ViewModels.TransferReference
{
    using PagedList;

    public class TransferReference01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<TransferReference01ResultRow> TransferReference01s { get; set; }
    }

    public class TransferReference01ViewModel
    {
        public TransferReference01SearchConditions SearchConditions { get; set; }

        public TransferReference01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReference01ViewModel"/> class.
        /// </summary>
        public TransferReference01ViewModel()
        {
            this.SearchConditions = new TransferReference01SearchConditions();
            this.Results = new TransferReference01Result();
        }
    }
}