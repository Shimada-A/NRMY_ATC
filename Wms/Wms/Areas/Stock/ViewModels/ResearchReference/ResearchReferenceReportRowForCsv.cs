using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wms.Areas.Stock.Resources;

namespace Wms.Areas.Stock.ViewModels.ResearchReference
{
    public class ResearchReferenceReportRowForCsv : ResearchReferenceReportRow
    {
        public ResearchReferenceReportRowForCsv(ResearchReferenceResultRow rrrr) : base(rrrr)
        {
            Location = string.Format("{0} {1}", rrrr.GetLocationUpper(), rrrr.GetLocationLower());
        }

        [Display(Name = nameof(ResearchReferenceResource.ReportCenterId), ResourceType = typeof(ResearchReferenceResource), Order = 1)]
        public string CenterId { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportCenterName), ResourceType = typeof(ResearchReferenceResource), Order = 2)]
        public string CenterName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportIssuerCode), ResourceType = typeof(ResearchReferenceResource), Order = 3)]
        public string IssuerCode { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportIssuerName), ResourceType = typeof(ResearchReferenceResource), Order = 4)]
        public string IssuerName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThLocation), ResourceType = typeof(ResearchReferenceResource), Order = 128)]
        public string Location { get; set; }

    }
}