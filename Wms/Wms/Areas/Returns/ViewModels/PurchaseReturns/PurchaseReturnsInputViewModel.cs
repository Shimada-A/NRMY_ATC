namespace Wms.Areas.Returns.ViewModels.PurchaseReturns
{
    using PagedList;
    using System.Collections.Generic;
    public class PurchaseReturnsInputResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<PurchaseReturnsResultRow> PurchaseReturns { get; set; }
    }
    public class PurchaseReturnsInputViewModel
    {
        public PurchaseReturnsSearchConditions SearchConditions { get; set; }

        public PurchaseReturnsInputResult Results { get; set; }

        public int Page { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseReturnsViewModel"/> class.
        /// </summary>
        public PurchaseReturnsInputViewModel()
        {
            this.SearchConditions = new PurchaseReturnsSearchConditions();
            this.Results = new PurchaseReturnsInputResult();
        }
    }
}