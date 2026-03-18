using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wms.Areas.Stock.Resources;

namespace Wms.Areas.Stock.ViewModels.ResearchReference
{
    public class ResearchReferenceReportRow
    {
        public ResearchReferenceReportRow(ResearchReferenceResultRow rrrr)
        {
            if (rrrr == null)
            {
                throw new ArgumentNullException(nameof(rrrr));
            }

            RowNumber = rrrr.RowNumber;
            SlipNo = rrrr.SlipNo;
            OccurredDateTimeString = rrrr.GetOccurredDateTimeString();
            RegistClassName = rrrr.RegistClassName;
            RegistUserName = rrrr.RegistUserName;
            BatchNo = rrrr.GetBatchNoForDisplay();
            LocationUpper = rrrr.GetLocationUpper();
            LocationLower = rrrr.GetLocationLower();
            InvoiceNo = rrrr.InvoiceNo;
            BoxNo = rrrr.BoxNo;
            ItemId = rrrr.ItemId;
            ItemName = rrrr.ItemName;
            ColorId = rrrr.ColorId;
            ColorName = rrrr.ColorName;
            SizeId = rrrr.SizeId;
            SizeName = rrrr.SizeName;
            Jan = rrrr.Jan;
            GradeName = rrrr.GradeName;
            DiffQuantity = rrrr.DiffQuantity;
            ListPrintFlagName = rrrr.ListPrintFlagName;
            StatusName = rrrr.StatusName;
            ResearchDateTimeString = rrrr.GetResearchDateTimeString();
            ResearchUserName = rrrr.ResearchUserName;
            ResearchLocationCd = rrrr.ResearchLocationCd;
            ResearchNote = rrrr.ResearchNote;
        }

        [Display(Name = nameof(ResearchReferenceResource.ThNumber), ResourceType = typeof(ResearchReferenceResource), Order = 101)]
        public int RowNumber { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThSlipNo), ResourceType = typeof(ResearchReferenceResource), Order = 102)]
        public string SlipNo { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThRegistDateTime), ResourceType = typeof(ResearchReferenceResource), Order = 103)]
        public string OccurredDateTimeString { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThRegistClass), ResourceType = typeof(ResearchReferenceResource), Order = 104)]
        public string RegistClassName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThRegistUserId), ResourceType = typeof(ResearchReferenceResource), Order = 105)]
        public string RegistUserName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThBatchNo), ResourceType = typeof(ResearchReferenceResource), Order = 106)]
        public string BatchNo { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportLocationUpper), ResourceType = typeof(ResearchReferenceResource), Order = 107)]
        public string LocationUpper { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportLocationLower), ResourceType = typeof(ResearchReferenceResource), Order = 108)]
        public string LocationLower { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThInvoiceNo), ResourceType = typeof(ResearchReferenceResource), Order = 109)]
        public string InvoiceNo { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThBoxNo), ResourceType = typeof(ResearchReferenceResource), Order = 110)]
        public string BoxNo { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportItemId), ResourceType = typeof(ResearchReferenceResource), Order = 111)]
        public string ItemId { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportItemName), ResourceType = typeof(ResearchReferenceResource), Order = 112)]
        public string ItemName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportColorId), ResourceType = typeof(ResearchReferenceResource), Order = 113)]
        public string ColorId { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportColorName), ResourceType = typeof(ResearchReferenceResource), Order = 114)]
        public string ColorName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportSizeId), ResourceType = typeof(ResearchReferenceResource), Order = 115)]
        public string SizeId { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ReportSizeName), ResourceType = typeof(ResearchReferenceResource), Order = 116)]
        public string SizeName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThJan), ResourceType = typeof(ResearchReferenceResource), Order = 117)]
        public string Jan { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThGrade), ResourceType = typeof(ResearchReferenceResource), Order = 118)]
        public string GradeName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThDiffQuantity), ResourceType = typeof(ResearchReferenceResource), Order = 119)]
        public int? DiffQuantity { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThListPringFlag), ResourceType = typeof(ResearchReferenceResource), Order = 120)]
        public string ListPrintFlagName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThStatus), ResourceType = typeof(ResearchReferenceResource), Order = 121)]
        public string StatusName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThResearchDateTime), ResourceType = typeof(ResearchReferenceResource), Order = 122)]
        public string ResearchDateTimeString { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThResearchUserId), ResourceType = typeof(ResearchReferenceResource), Order = 123)]
        public string ResearchUserName { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThReferenceLocation), ResourceType = typeof(ResearchReferenceResource), Order = 124)]
        public string ResearchLocationCd { get; set; }

        [Display(Name = nameof(ResearchReferenceResource.ThResearchNote), ResourceType = typeof(ResearchReferenceResource), Order = 125)]
        public string ResearchNote { get; set; }
    }
}