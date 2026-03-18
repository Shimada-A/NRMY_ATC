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
    public class PrintStockSortKanban
    {

        [Display(Name = nameof(ImportInstructionResource.SortInstructNo), ResourceType = typeof(ImportInstructionResource), Order = 1)]
        public string SortInstructNo { get; set; }

        [Display(Name = nameof(ImportInstructionResource.SortInstructName), ResourceType = typeof(ImportInstructionResource), Order = 2)]
        public string SortInstructName { get; set; }

        [Display(Name = nameof(ImportInstructionResource.SortInstructNoBarcode), ResourceType = typeof(ImportInstructionResource), Order = 3)]
        public string SortInstructNoBarcode { get; set; }
    }
}