namespace Wms.Areas.Returns.ViewModels.EcReference
{
    using PagedList;
    public class EcReference01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<EcReference01ResultRow> EcReference01s { get; set; }
    }

    public class EcReference01ViewModel
    {
        public EcReference01SearchConditions SearchConditions { get; set; }

        public EcReference01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcReference01ViewModel"/> class.
        /// </summary>
        public EcReference01ViewModel()
        {
            this.SearchConditions = new EcReference01SearchConditions();
            this.Results = new EcReference01Result();
        }
    }
}