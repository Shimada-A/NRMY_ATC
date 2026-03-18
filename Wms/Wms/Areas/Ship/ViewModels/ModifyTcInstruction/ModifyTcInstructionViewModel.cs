namespace Wms.Areas.Ship.ViewModels.ModifyTcInstruction
{
    using PagedList;

    public class ModifyTcInstructionResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<ModifyTcInstructionResultRow> ModifyTcInstructions { get; set; }
    }

    public class ModifyTcInstructionViewModel
    {
        public ModifyTcInstructionSearchConditions SearchConditions { get; set; }

        public ModifyTcInstructionResult Results { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyTcInstructionViewModel"/> class.
        /// </summary>
        public ModifyTcInstructionViewModel()
        {
            this.SearchConditions = new ModifyTcInstructionSearchConditions();
            this.Results = new ModifyTcInstructionResult();
        }
    }
}