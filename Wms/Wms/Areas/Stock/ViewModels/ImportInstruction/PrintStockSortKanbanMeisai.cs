namespace Wms.Areas.Stock.ViewModels.ImportInstruction
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Stock.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PrintStockSortKanbanMeisai
    {

        [Display(Name = nameof(ImportInstructionResource.CategoryId1), ResourceType = typeof(ImportInstructionResource), Order = 1)]
        public string BunruiNm { get; set; }

        [Display(Name = nameof(ImportInstructionResource.SortInstructNo), ResourceType = typeof(ImportInstructionResource), Order = 2)]
        public string SortInstructNo { get; set; }

        [Display(Name = nameof(ImportInstructionResource.SortInstructName), ResourceType = typeof(ImportInstructionResource), Order = 2)]
        public string SortInstructName { get; set; }

        [Display(Name = nameof(ImportInstructionResource.SortClass), ResourceType = typeof(ImportInstructionResource), Order = 3)]
        public string SortClass { get; set; }

        [Display(Name = nameof(ImportInstructionResource.ItemCd), ResourceType = typeof(ImportInstructionResource), Order = 4)]
        public string ItemCd { get; set; }

        [Display(Name = nameof(ImportInstructionResource.NumStock), ResourceType = typeof(ImportInstructionResource), Order = 5)]
        public string NumStock { get; set; }

        [Display(Name = nameof(ImportInstructionResource.ItemSize), ResourceType = typeof(ImportInstructionResource), Order = 6)]
        public string ItemSize { get; set; }

        [Display(Name = nameof(ImportInstructionResource.ItemColor), ResourceType = typeof(ImportInstructionResource), Order = 7)]
        public string ItemColor { get; set; }

        [Display(Name = nameof(ImportInstructionResource.CategoryNm), ResourceType = typeof(ImportInstructionResource), Order = 8)]
        public string SortCategoryNm { get; set; }

        [Display(Name = nameof(ImportInstructionResource.Jan1), ResourceType = typeof(ImportInstructionResource), Order = 9)]
        public string Jan1 { get; set; }

        [Display(Name = nameof(ImportInstructionResource.Jan2), ResourceType = typeof(ImportInstructionResource), Order = 10)]
        public string Jan2 { get; set; }

        [Display(Name = nameof(ImportInstructionResource.Note), ResourceType = typeof(ImportInstructionResource), Order = 11)]
        public string Note { get; set; }

        [Display(Name = nameof(ImportInstructionResource.TransferNo), ResourceType = typeof(ImportInstructionResource), Order = 12)]
        public string TransferNo { get; set; }

        [Display(Name = nameof(ImportInstructionResource.CenterIdS), ResourceType = typeof(ImportInstructionResource), Order = 13)]
        public string CenterId { get; set; }

    }
}