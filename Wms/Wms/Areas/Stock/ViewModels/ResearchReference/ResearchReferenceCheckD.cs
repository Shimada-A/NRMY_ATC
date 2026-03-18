using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.Stock.ViewModels.ResearchReference
{
    /// <summary>
    /// T_STOCK_RESEARCH_CHECK_D用のViewModel
    /// </summary>
    public class ResearchReferenceCheckD
    {
        public string SlipNo { get; set; }

        public int SlipSeq { get; set; }

        public string BatchNo { get; set; }

        public string ItemSkuId { get; set; }

        public string Jan { get; set; }

        public string ItemId { get; set; }

        public string ItemColorId { get; set; }

        public string ItemSizeId { get; set; }

        public string LocationCd { get; set; }

        public string GradeId { get; set; }

        public int Qty { get; set; }

        public string ResearchNote { get; set; }

        public string ResearchUserId { get; set; }

        public string ResearchUserName { get; set; }

        public DateTime ResearchDate { get; set; }

        public int AssortStockOutQty { get; set; }

        public string ResearchClassName { get; set; }


    }
}