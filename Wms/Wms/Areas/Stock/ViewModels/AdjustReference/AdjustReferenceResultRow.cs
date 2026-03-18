using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.Stock.ViewModels.AdjustReference
{
    public class AdjustReferenceResultRow
    {
        public int RowNumber { get; set; }

        public string AdjustNumber { get; set; }

        public DateTime AdjustDate { get; set; }

        public string Sku { get; set; }

        public string Jan { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public string ColorId { get; set; }

        public string ColorName { get; set; }

        public string SizeId { get; set; }

        public string SizeName { get; set; }

        public string Note { get; set; }

        public string CategoryId1 { get; set; }

        public string CategoryName1 { get; set; }

        public string GradeNameFrom { get; set; }

        public string LocationCdFrom { get; set; }

        public string BoxNoFrom { get; set; }

        public string GradeNameTo{ get; set; }

        public string LocationCdTo { get; set; }

        public string BoxNoTo { get; set; }

        public int AdjustQuantityTo { get; set; }

        public int AdjustReasonCd { get; set; }

        public string AdjustReasonName { get; set; }

        public string UpdateUserId { get; set; }

        public string UserName { get; set; }

        public string GetAdjustDateString()
        {
            return AdjustDate.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}