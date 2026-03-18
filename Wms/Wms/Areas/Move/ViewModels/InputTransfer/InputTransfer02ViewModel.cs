namespace Wms.Areas.Move.ViewModels.InputTransfer
{
    using PagedList;
    using System.Collections.Generic;

    public class InputTransfer02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<InputTransfer02ResultRow> InputTransfer02s { get; set; }
    }

    public class InputTransfer02ResultReceipt
    {
        /// <summary>
        /// List record Receipt
        /// </summary>
        public IList<InputTransfer02ResultRow> InputTransfer02s { get; set; }
    }

    public class InputTransfer02ViewModel
    {
        public InputTransfer02SearchConditions SearchConditions { get; set; }

        public InputTransfer02Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputTransfer02ViewModel"/> class.
        /// </summary>
        public InputTransfer02ViewModel()
        {
            this.SearchConditions = new InputTransfer02SearchConditions();
            this.Results = new InputTransfer02Result();
        }
    }
}