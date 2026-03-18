using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.Security;
using System.Web;
using Wms.Areas.Stock.Resources;

namespace Wms.Areas.Stock.ViewModels.AdjustReference
{
    public class AdjustReferenceReportRow
    {
        public AdjustReferenceReportRow(AdjustReferenceResultRow arrr)
        {
            if (arrr == null)
            {
                throw new ArgumentNullException(nameof(arrr));
            }

            RowNumber = arrr.RowNumber;
            AdjustDateString = arrr.GetAdjustDateString();
            AdjustNumber = arrr.AdjustNumber;
            CategoryName1 = arrr.CategoryName1;
            ItemId = arrr.ItemId;
            ItemName = arrr.ItemName;
            ColorId = arrr.ColorId;
            ColorName = arrr.ColorName;
            SizeId = arrr.SizeId;
            SizeName = arrr.SizeName;
            Jan = arrr.Jan;
            GradeNameFrom = arrr.GradeNameFrom;
            LocationCdFrom = arrr.LocationCdFrom;
            BoxNoFrom = arrr.BoxNoFrom;
            GradeNameTo = arrr.GradeNameTo;
            LocationCdTo = arrr.LocationCdTo;
            BoxNoTo = arrr.BoxNoTo;
            AdjustQuantityTo = arrr.AdjustQuantityTo;
            AdjustReasonName = arrr.AdjustReasonName;
            Note = arrr.Note;
            UpdateUserId = arrr.UpdateUserId;
            UserName = arrr.UserName;
        }

        [Display(Name = nameof(AdjustReferenceResource.ThNumber), ResourceType = typeof(AdjustReferenceResource), Order = 101)]
        public int RowNumber { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ThStockAdjustDate), ResourceType = typeof(AdjustReferenceResource), Order = 102)]
        public string AdjustDateString { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ThStockAdjustNumber), ResourceType = typeof(AdjustReferenceResource), Order = 103)]
        public string AdjustNumber { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ThCategory1), ResourceType = typeof(AdjustReferenceResource), Order = 105)]
        public string CategoryName1 { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportItemId), ResourceType = typeof(AdjustReferenceResource), Order = 106)]
        public string ItemId { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportItemName), ResourceType = typeof(AdjustReferenceResource), Order = 107)]
        public string ItemName { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportColorId), ResourceType = typeof(AdjustReferenceResource), Order = 108)]
        public string ColorId { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportColorName), ResourceType = typeof(AdjustReferenceResource), Order = 109)]
        public string ColorName { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportSizeId), ResourceType = typeof(AdjustReferenceResource), Order = 110)]
        public string SizeId { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportSizeName), ResourceType = typeof(AdjustReferenceResource), Order = 111)]
        public string SizeName { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ThJan), ResourceType = typeof(AdjustReferenceResource), Order = 112)]
        public string Jan { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportGradeFrom), ResourceType = typeof(AdjustReferenceResource), Order = 113)]
        public string GradeNameFrom { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportLocationFrom), ResourceType = typeof(AdjustReferenceResource), Order = 114)]
        public string LocationCdFrom { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportBoxNoFrom), ResourceType = typeof(AdjustReferenceResource), Order = 115)]
        public string BoxNoFrom { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportGradeTo), ResourceType = typeof(AdjustReferenceResource), Order = 116)]
        public string GradeNameTo { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportLocationTo), ResourceType = typeof(AdjustReferenceResource), Order = 117)]
        public string LocationCdTo { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ReportBoxNoTo), ResourceType = typeof(AdjustReferenceResource), Order = 118)]
        public string BoxNoTo { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ThAdjustQuantity), ResourceType = typeof(AdjustReferenceResource), Order = 119)]
        public int AdjustQuantityTo { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ThReason), ResourceType = typeof(AdjustReferenceResource), Order = 120)]
        public string AdjustReasonName { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ThNote), ResourceType = typeof(AdjustReferenceResource), Order = 121)]
        public string Note { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ThUserIdE), ResourceType = typeof(AdjustReferenceResource), Order = 121)]
        public string UpdateUserId { get; set; }

        [Display(Name = nameof(AdjustReferenceResource.ThUserNameE), ResourceType = typeof(AdjustReferenceResource), Order = 121)]
        public string UserName { get; set; }
    }
}