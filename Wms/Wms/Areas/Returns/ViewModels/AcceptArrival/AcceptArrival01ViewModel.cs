namespace Wms.Areas.Returns.ViewModels.AcceptArrival
{
    using System.Collections.Generic;

    public class ScanResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public List<AcceptArrival01ResultRow> AcceptArrival01Result { get; set; }
    }
    public class AcceptArrival01ViewModel
    {
        /// <summary>
        /// Gets or sets 処理区分
        /// </summary>
        public string ChangeModel { get; set; }

        /// <summary>
        /// Gets or sets 削除行の番号
        /// </summary>
        public int? DelRowNo { get; set; }

        public AcceptArrival01SearchConditions SearchConditions { get; set; }

        public ScanResult Results { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcReference01ViewModel"/> class.
        /// </summary>
        public AcceptArrival01ViewModel()
        {
            this.SearchConditions = new AcceptArrival01SearchConditions();
            this.Results = new ScanResult();
        }

    }
}