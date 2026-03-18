using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.Returns.ViewModels.AcceptArrival
{
    public class SearchResults
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<AcceptArrival02ResultRow> AcceptArrival02Result { get; set; }
    }
    public class AcceptArrival02ViewModel
    {

        public SearchResults Results { get; set; }

        public AcceptArrival02SearchConditions SearchConditions { get; set; }
    }
}