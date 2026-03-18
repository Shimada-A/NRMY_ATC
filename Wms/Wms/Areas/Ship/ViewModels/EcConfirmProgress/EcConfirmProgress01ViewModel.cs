namespace Wms.Areas.Ship.ViewModels.EcConfirmProgress
{
    using PagedList;

    public class EcConfirmProgress01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<EcConfirmProgress01ResultRow> EcConfirmProgress01s { get; set; }
    }

    public class EcConfirmProgress01ViewModel
    {
        public EcConfirmProgress01SearchConditions SearchConditions { get; set; }

        public EcConfirmProgress01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcConfirmProgress01ViewModel"/> class.
        /// </summary>
        public EcConfirmProgress01ViewModel()
        {
            this.SearchConditions = new EcConfirmProgress01SearchConditions();
            this.Results = new EcConfirmProgress01Result();
        }
    }
}