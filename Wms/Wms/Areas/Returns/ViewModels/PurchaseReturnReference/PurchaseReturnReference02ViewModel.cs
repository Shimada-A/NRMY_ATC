namespace Wms.Areas.Returns.ViewModels.PurchaseReturnReference
{
    using PagedList;
    using System.Collections.Generic;

    public class PurchaseReturnReference02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<PurchaseReturnReference02ResultRow> PurchaseReturnReference02s { get; set; }
    }

    public class PurchaseReturnReference02ViewModel
    {
        public PurchaseReturnReference01SearchConditions SearchConditions { get; set; }

        public PurchaseReturnReference02Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseReturnReference02ViewModel"/> class.
        /// </summary>
        public PurchaseReturnReference02ViewModel()
        {
            this.SearchConditions = new PurchaseReturnReference01SearchConditions();
            this.Results = new PurchaseReturnReference02Result();
        }
    }
}