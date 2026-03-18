namespace Wms.Areas.Move.ViewModels.InputTransfer
{
    using PagedList;

    public class InputTransfer01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<InputTransfer01ResultRow> InputTransfer01s { get; set; }
    }

    public class InputTransfer01ViewModel
    {
        public InputTransfer01SearchConditions SearchConditions { get; set; }

        public InputTransfer01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputTransfer01ViewModel"/> class.
        /// </summary>
        public InputTransfer01ViewModel()
        {
            this.SearchConditions = new InputTransfer01SearchConditions();
            this.Results = new InputTransfer01Result();
        }
    }
}