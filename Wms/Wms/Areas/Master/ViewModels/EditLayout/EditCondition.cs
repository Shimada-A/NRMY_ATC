using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Models;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public class EditCondition : EditLayoutBase
    {
        public EditCondition()
        {

        }

        public EditCondition(Layout layout, List<LayoutCondition> conditions)
            :base(layout)
        {
            Details = new List<ObjectDetailDTO>();
            foreach(var item in conditions)
            {
                Details.Add(new ObjectDetailDTO(item));
            }
        }

        public EditCondition(Layout layout, List<ObjectDetailDTO> conditions)
                : base(layout)
        {
            Details = conditions;
        }

        public List<ObjectDetailDTO> Details { get; set; }
    }
}