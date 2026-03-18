using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Wms.Areas.Stock.ViewModels.ResearchReference
{
    public class ResearchReferenceInput
    {
        public string Json { get; set; }

        public string ResearchUserId { get; set; }

        public string ResearchLocationCd { get; set; }

        public string ResearchNote { get; set; }

        public ResearchReferenceResultRow ToObject()
        {
            return new JavaScriptSerializer().Deserialize<ResearchReferenceResultRow>(Json);
        }
    }
}