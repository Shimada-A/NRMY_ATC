using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.Stock.ViewModels.AdjustReference
{
    public class AdjustReferenceResult
    {
        public IPagedList<AdjustReferenceResultRow> ResultRowList { get; set; }

        public bool IsEmpty()
        {
            return ResultRowList == null || ResultRowList.Count == 0;
        }
    }
}