namespace Wms.Areas.Ship.ViewModels.EcCancelUpload
{
    using PagedList;

    public class EcCancelUpload01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<EcCancelUpload01ResultRow> EcCancelUpload01s { get; set; }
    }

    public class EcCancelUpload01ViewModel
    {
        public EcCancelUpload01SearchConditions SearchConditions { get; set; }

        public EcCancelUpload01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcCancelUpload01ViewModel"/> class.
        /// </summary>
        public EcCancelUpload01ViewModel()
        {
            this.SearchConditions = new EcCancelUpload01SearchConditions();
            this.Results = new EcCancelUpload01Result();
        }
    }
}