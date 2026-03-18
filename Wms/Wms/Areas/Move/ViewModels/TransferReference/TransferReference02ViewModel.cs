namespace Wms.Areas.Move.ViewModels.TransferReference
{
    using PagedList;

    public class TransferReference02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<TransferReference02ResultRow> TransferReference02s { get; set; }
    }

    public class TransferReference02ViewModel
    {
        public TransferReference02SearchConditions SearchConditions { get; set; }

        public TransferReference02Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReference02ViewModel"/> class.
        /// </summary>
        public TransferReference02ViewModel()
        {
            this.SearchConditions = new TransferReference02SearchConditions();
            this.Results = new TransferReference02Result();
        }
    }
}