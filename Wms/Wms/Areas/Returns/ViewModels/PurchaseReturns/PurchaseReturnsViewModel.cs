namespace Wms.Areas.Returns.ViewModels.PurchaseReturns
{
    using PagedList;
    using System.Collections.Generic;
    public class PurchaseReturnsResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<PurchaseReturnsResultRow> PurchaseReturns { get; set; }
    }
    public class PurchaseReturnsViewModel
    {
        public PurchaseReturnsSearchConditions SearchConditions { get; set; }

        public PurchaseReturnsResult Results { get; set; }

        public int Page { get; set; }

        public IList<PurchaseReturnsResultRow> PurchaseReturns { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseReturnsViewModel"/> class.
        /// </summary>
        public PurchaseReturnsViewModel()
        {
            this.SearchConditions = new PurchaseReturnsSearchConditions();
            this.Results = new PurchaseReturnsResult();
        }
    }
}