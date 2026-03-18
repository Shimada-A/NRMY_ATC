namespace Wms.Areas.Ship.ViewModels.TransporterChngEc
{
    using PagedList;
    using Wms.ViewModels.Shared;

    public class TransporterChngEcResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<TransporterChngEcResultRow> TransporterChngEcs { get; set; }
    }

    public class TransporterChngEcViewModel
    {
        public TransporterChngEcSearchConditions SearchConditions { get; set; }

        public TransporterChngEcResult Results { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransporterChngEcViewModel"/> class.
        /// </summary>
        public TransporterChngEcViewModel()
        {
            this.SearchConditions = new TransporterChngEcSearchConditions() ;
            this.Results = new TransporterChngEcResult();
        }
    }
}