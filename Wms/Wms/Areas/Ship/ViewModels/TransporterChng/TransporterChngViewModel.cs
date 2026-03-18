namespace Wms.Areas.Ship.ViewModels.TransporterChng
{
    using PagedList;
    using Wms.ViewModels.Shared;

    public class TransporterChngResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<TransporterChngResultRow> TransporterChngs { get; set; }
    }

    public class TransporterChngViewModel
    {
        public TransporterChngSearchConditions SearchConditions { get; set; }

        public TransporterChngResult Results { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransporterChngViewModel"/> class.
        /// </summary>
        public TransporterChngViewModel()
        {
            this.SearchConditions = new TransporterChngSearchConditions() ;
            this.Results = new TransporterChngResult();
        }
    }
}