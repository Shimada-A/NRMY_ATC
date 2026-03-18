namespace Wms.Areas.Inventory.ViewModels.Input
{
    using PagedList;
    using System.Collections.Generic;

    public class InputResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<InputResultRow> Inputs { get; set; }
        public IList<InputResultRow> InputList { get; set; }
    }

    public class InputViewModel
    {
        public InputSearchConditions SearchConditions { get; set; }

        public InputResult Results { get; set; }

        public string ChangeModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputViewModel"/> class.
        /// </summary>
        public InputViewModel()
        {
            this.SearchConditions = new InputSearchConditions();
            this.Results = new InputResult();
        }
    }
}