namespace Wms.Areas.Stock.ViewModels.ImportInstruction
{
    using PagedList;

    public class ImportInstructionResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<ImportInstructionResultRow> ImportInstructions { get; set; }
    }

    public class ImportInstructionViewModel
    {
        public ImportInstructionSearchConditions SearchConditions { get; set; }

        public ImportInstructionResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportInstructionViewModel"/> class.
        /// </summary>
        public ImportInstructionViewModel()
        {
            this.SearchConditions = new ImportInstructionSearchConditions();
            this.Results = new ImportInstructionResult();
        }
    }
}