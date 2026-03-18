namespace Wms.Areas.Arrival.ViewModels.InputPurchase
{
    using PagedList;
    using System.Collections.Generic;

    public class InputPurchase02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<InputPurchase02ResultRow> InputPurchase02s { get; set; }
    }

    public class InputPurchase02ViewModel
    {
        public InputPurchase02SearchConditions SearchConditions { get; set; }

        public InputPurchase02Result Results { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputPurchase02ViewModel"/> class.
        /// </summary>
        public InputPurchase02ViewModel()
        {
            this.SearchConditions = new InputPurchase02SearchConditions();
            this.Results = new InputPurchase02Result();
        }
    }
}