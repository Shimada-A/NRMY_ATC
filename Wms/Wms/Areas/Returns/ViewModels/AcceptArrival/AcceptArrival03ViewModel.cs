using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.Returns.ViewModels.AcceptArrival
{
    public class SearchInput
    {
        /// <summary>
        /// List record
        /// </summary>
        public List<AcceptArrival03ResultRow> AcceptArrival03Result { get; set; }

    }
    public class AcceptArrival03ViewModel
    {

        public SearchInput InputResults { get; set; }

        public AcceptArrival03SearchConditions SearchConditions { get; set; }
    }
}