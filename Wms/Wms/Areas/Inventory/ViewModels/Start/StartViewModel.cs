namespace Wms.Areas.Inventory.ViewModels.Start
{
    using PagedList;

    public class StartResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<StartResultRow> Starts { get; set; }
    }

    public class StartViewModel
    {
        public StartSearchConditions SearchConditions { get; set; }

        public StartResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartViewModel"/> class.
        /// </summary>
        public StartViewModel()
        {
            this.SearchConditions = new StartSearchConditions();
            this.Results = new StartResult();
        }
    }
}