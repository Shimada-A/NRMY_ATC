using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Wms.Areas.Stock.ViewModels.ResearchReference.ResearchReferenceSearchConditions;

namespace Wms.Areas.Stock.ViewModels.ResearchReference
{
    public class ResearchReferenceViewModel
    {
        public ResearchReferenceViewModel()
        {
            Condition = new ResearchReferenceSearchConditions();
            Result = new ResearchReferenceResult();
            IsShowResultList = false;
            CanChangeCenter = false;
        }

        public ResearchReferenceSearchConditions Condition { get; set; }

        public ResearchReferenceSortOrder GetSortOrder
        {
            get
            {
                return this.Condition.SortOrder;
            }
        }

        public ResearchReferenceResult Result { get; set; }

        public bool IsShowResultList { get; set; }

        public bool CanChangeCenter { get; set; }
    }
}