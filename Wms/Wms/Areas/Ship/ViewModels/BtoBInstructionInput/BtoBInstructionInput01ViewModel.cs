namespace Wms.Areas.Ship.ViewModels.BtoBInstructionInput
{
    using PagedList;

    public class BtoBInstructionInput01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<BtoBInstructionInput01ResultRow> BtoBInstructionInput01s { get; set; }
    }

    public class BtoBInstructionInput01ViewModel
    {
        public BtoBInstructionInput01SearchConditions SearchConditions { get; set; }

        public BtoBInstructionInput01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBInstructionInput01ViewModel"/> class.
        /// </summary>
        public BtoBInstructionInput01ViewModel()
        {
            this.SearchConditions = new BtoBInstructionInput01SearchConditions();
            this.Results = new BtoBInstructionInput01Result();
        }
    }
}