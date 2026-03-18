using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Wms.Common;

namespace Wms.Areas.Stock.ViewModels.AdjustReference
{
    public class AdjustReferenceViewModel
    {
        public AdjustReferenceViewModel()
        {
            Condition = new AdjustReferenceSearchConditions();
            Result = new AdjustReferenceResult();
            IsShowResultList = false;
            CanChangeCenter = false;
        }

        public AdjustReferenceSearchConditions Condition { get; set; }

        public AdjustReferenceResult Result { get; set; }

        public bool IsShowResultList { get; set; }

        public bool CanChangeCenter { get; set; }

        public void SetResult(AdjustReferenceResult result)
        {
            Result = result;
        }
    }
}