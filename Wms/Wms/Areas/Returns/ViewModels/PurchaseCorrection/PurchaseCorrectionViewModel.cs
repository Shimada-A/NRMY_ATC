namespace Wms.Areas.Returns.ViewModels.PurchaseCorrection
{
    using PagedList;
    using System.Collections.Generic;
    public class PurchaseCorrectionResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<PurchaseCorrectionResultRow> PurchaseCorrection { get; set; }
    }
    public class PurchaseCorrectionViewModel
    {
        public PurchaseCorrectionSearchConditions SearchConditions { get; set; }

        public PurchaseCorrectionResult Results { get; set; }

        public IList<PurchaseCorrectionResultRow> PurchaseCorrection { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseCorrectionViewModel"/> class.
        /// </summary>
        public PurchaseCorrectionViewModel()
        {
            this.SearchConditions = new PurchaseCorrectionSearchConditions();
            this.Results = new PurchaseCorrectionResult();
        }
    }
}