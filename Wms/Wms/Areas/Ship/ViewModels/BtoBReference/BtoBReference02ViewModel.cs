namespace Wms.Areas.Ship.ViewModels.BtoBReference
{
    using PagedList;
    using System.Collections.Generic;

    public class BtoBReference02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IEnumerable<BtoBReference02ResultRow> BtoBReference02s { get; set; }
    }

    public class BtoBReference02ViewModel
    {
        public BtoBReference02SearchConditions SearchConditions { get; set; }

        public BtoBReference02Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBReference02ViewModel"/> class.
        /// </summary>
        public BtoBReference02ViewModel()
        {
            this.SearchConditions = new BtoBReference02SearchConditions();
            this.Results = new BtoBReference02Result();
        }
    }
}