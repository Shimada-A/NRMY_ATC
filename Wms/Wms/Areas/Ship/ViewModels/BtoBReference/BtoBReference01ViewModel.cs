namespace Wms.Areas.Ship.ViewModels.BtoBReference
{
    using PagedList;

    public class BtoBReference01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<BtoBReference01ResultRow> BtoBReference01s { get; set; }
    }

    public class BtoBReference01ViewModel
    {
        public BtoBReference01SearchConditions SearchConditions { get; set; }

        public BtoBReference01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBReference01ViewModel"/> class.
        /// </summary>
        public BtoBReference01ViewModel()
        {
            this.SearchConditions = new BtoBReference01SearchConditions();
            this.Results = new BtoBReference01Result();
        }
    }
}