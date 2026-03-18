namespace Wms.Areas.Ship.ViewModels.PrintBatch
{
    using PagedList;
    using System.Collections.Generic;

    public class PrintBatchResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<PrintBatchResultRow> PrintBatchs { get; set; }
    }

    public class PrintBatchViewModel
    {
        public PrintBatchSearchConditions SearchConditions { get; set; }

        public PrintBatchResult Results { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintBatchViewModel"/> class.
        /// </summary>
        public PrintBatchViewModel()
        {
            this.SearchConditions = new PrintBatchSearchConditions();
            this.Results = new PrintBatchResult();
        }
    }
}