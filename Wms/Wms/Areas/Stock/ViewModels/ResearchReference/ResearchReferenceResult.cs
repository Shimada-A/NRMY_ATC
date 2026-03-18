using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.Stock.ViewModels.ResearchReference
{
    public class ResearchReferenceResult
    {
        public IPagedList<ResearchReferenceResultRow> ResultRowList { get; set; }

        public ResearchReferenceResultCount ResultCount { get; set; }

        public string SelectedUniqueKey { get; set; }

        public bool IsEmpty()
        {
            return ResultRowList == null || ResultRowList.Count == 0;
        }

        public ResearchReferenceResultRow FindResultRow(string rowNumber)
        {
            if (IsEmpty() || string.IsNullOrEmpty(rowNumber))
            {
                return null;
            }

            return ResultRowList.ToList().Find((val) => val.RowNumber.ToString() == rowNumber);
        }
    }
}