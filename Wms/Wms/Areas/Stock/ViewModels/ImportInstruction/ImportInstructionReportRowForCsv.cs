using System.ComponentModel.DataAnnotations;
using Wms.Areas.Stock.Resources;

namespace Wms.Areas.Stock.ViewModels.ImportInstruction
{
    public class ImportInstructionReportRowForCsv
    {
        [Display(Name = nameof(ImportInstructionResource.ReportType), ResourceType = typeof(ImportInstructionResource), Order = 1)]
        public byte ReportType { get; set; }

        [Display(Name = nameof(ImportInstructionResource.CenterId), ResourceType = typeof(ImportInstructionResource), Order = 2)]
        public string CenterId { get; set; }

        [Display(Name = nameof(ImportInstructionResource.SortInstructNo), ResourceType = typeof(ImportInstructionResource), Order = 3)]
        public string SortInstructNo { get; set; }

        [Display(Name = nameof(ImportInstructionResource.SortInstructName), ResourceType = typeof(ImportInstructionResource), Order = 4)]
        public string SortInstructName { get; set; }

        [Display(Name = nameof(ImportInstructionResource.TransferNo), ResourceType = typeof(ImportInstructionResource), Order = 5)]
        public string TransferNo { get; set; }

        [Display(Name = nameof(ImportInstructionResource.SortCategoryName), ResourceType = typeof(ImportInstructionResource), Order = 6)]
        public string SortCategoryName { get; set; }

        [Display(Name = nameof(ImportInstructionResource.CategoryName1), ResourceType = typeof(ImportInstructionResource), Order = 7)]
        public string CategoryName1 { get; set; }

        [Display(Name = nameof(ImportInstructionResource.ItemCd), ResourceType = typeof(ImportInstructionResource), Order = 8)]
        public string ItemId { get; set; }

        [Display(Name = nameof(ImportInstructionResource.ItemColor), ResourceType = typeof(ImportInstructionResource), Order = 9)]
        public string ItemColor { get; set; }

        [Display(Name = nameof(ImportInstructionResource.ItemSize), ResourceType = typeof(ImportInstructionResource), Order = 10)]
        public string ItemSize { get; set; }

        [Display(Name = nameof(ImportInstructionResource.Jan), ResourceType = typeof(ImportInstructionResource), Order = 11)]
        public string Jan { get; set; }

        [Display(Name = nameof(ImportInstructionResource.NumStock), ResourceType = typeof(ImportInstructionResource), Order = 12)]
        public int StockQty { get; set; }

        [Display(Name = nameof(ImportInstructionResource.Note), ResourceType = typeof(ImportInstructionResource), Order = 13)]
        public string Note { get; set; }
    }
}