using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Models;

namespace Wms.Areas.Stock.ViewModels.ResearchReference
{
    public class ResearchReferenceInputViewModel
    {
        public ResearchReferenceResultRow ResultRow { get; set; }

        public OutStatusMessage StatusMessage { get; set; }

        public bool IsEmptyStatusMessage()
        {
            return StatusMessage == null;
        }

        // 在庫調査結果明細
        public List<ResearchReferenceCheckD> ResultCheckD { get; set; }
    }
}