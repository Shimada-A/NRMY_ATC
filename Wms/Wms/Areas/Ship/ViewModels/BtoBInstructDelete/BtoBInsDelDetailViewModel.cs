namespace Wms.Areas.Ship.ViewModels.BtoBInstructDelete
{
    using System.Collections.Generic;
    using PagedList;
    using Wms.ViewModels.Shared;

    public class BtoBInsDelDetailResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IEnumerable<BtoBInsDelDetailResultRow> BtoBInsDelDetails { get; set; }
    }

    public class BtoBInsDelDetailViewModel
    {
        public BtoBInsDelDetailSearchConditions DetailSearchConditions { get; set; }

        public BtoBInsDelDetailResult DetailResults { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInquiryViewModel"/> class.
        /// </summary>
        public BtoBInsDelDetailViewModel()
        {
            this.DetailSearchConditions = new BtoBInsDelDetailSearchConditions() ;
            this.DetailResults = new BtoBInsDelDetailResult();
        }
    }
}