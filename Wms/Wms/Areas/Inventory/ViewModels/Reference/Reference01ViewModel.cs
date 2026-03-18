namespace Wms.Areas.Inventory.ViewModels.Reference
{
    using PagedList;

    public class Reference01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Reference01ResultRow> Reference { get; set; }
    }

    public class Reference01ViewModel
    {
        public Reference01SearchConditions SearchConditions { get; set; }

        public Reference01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Reference01ViewModel"/> class.
        /// </summary>
        public Reference01ViewModel()
        {
            this.SearchConditions = new Reference01SearchConditions();
            this.Results = new Reference01Result();
        }
    }
}