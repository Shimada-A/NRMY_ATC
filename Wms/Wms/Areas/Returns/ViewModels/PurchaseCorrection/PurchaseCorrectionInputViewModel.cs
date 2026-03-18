namespace Wms.Areas.Returns.ViewModels.PurchaseCorrection
{
    using PagedList;
    using System.Collections.Generic;
    public class PurchaseCorrectionInputResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<PurchaseCorrectionResultRow> PurchaseCorrection { get; set; }
    }
    public class PurchaseCorrectionInputViewModel
    {
        public PurchaseCorrectionSearchConditions SearchConditions { get; set; }

        public PurchaseCorrectionInputResult Results { get; set; }

        public int Page { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseCorrectionViewModel"/> class.
        /// </summary>
        public PurchaseCorrectionInputViewModel()
        {
            this.SearchConditions = new PurchaseCorrectionSearchConditions();
            this.Results = new PurchaseCorrectionInputResult();
        }
    }
}