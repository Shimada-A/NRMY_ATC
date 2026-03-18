namespace Wms.Areas.Ship.ViewModels.UploadCaseInstruction
{
    using PagedList;

    public class UploadCaseInstructionResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<UploadCaseInstructionResultRow> UploadCaseInstructions { get; set; }

        public IPagedList<UploadCaseDetailsResultRow> CaseDetails { get; set; }

        public IPagedList<UploadJanDetailsResultRow> JanDetails { get; set; }
    }

    public class UploadCaseInstructionViewModel
    {
        public UploadCaseInstructionSearchConditions SearchConditions { get; set; }

        public UploadCaseInstructionResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadCaseInstructionViewModel"/> class.
        /// </summary>
        public UploadCaseInstructionViewModel()
        {
            this.SearchConditions = new UploadCaseInstructionSearchConditions();
            this.Results = new UploadCaseInstructionResult();
        }
    }
}