using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wms.Areas.Stock.Resources;

namespace Wms.Areas.Stock.ViewModels.AdjustReference
{
    public class AdjustReferenceReportRowForCsv : AdjustReferenceReportRow
    {
        public AdjustReferenceReportRowForCsv(AdjustReferenceResultRow arrr) : base(arrr)
        {
            CategoryId1 = arrr.CategoryId1;
            UpdateUserId = arrr.UpdateUserId;
        }

        [Display(Name = nameof(AdjustReferenceResource.ReportCenterId), ResourceType = typeof(AdjustReferenceResource), Order = 1)]
        public string CenterId { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportCenterName), ResourceType = typeof(AdjustReferenceResource), Order = 2)]
        public string CenterName { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportIssuerCode), ResourceType = typeof(AdjustReferenceResource), Order = 3)]
        public string IssuerCode { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportIssuerName), ResourceType = typeof(AdjustReferenceResource), Order = 4)]
        public string IssuerName { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportCategoryId1), ResourceType = typeof(AdjustReferenceResource), Order = 104)]
        public string CategoryId1 { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportUserId), ResourceType = typeof(AdjustReferenceResource), Order = 122)]
        public string UpdateUserId { get; set; }

    }
}