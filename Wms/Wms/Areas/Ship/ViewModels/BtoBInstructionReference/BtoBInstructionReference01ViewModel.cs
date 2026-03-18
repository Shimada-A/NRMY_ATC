namespace Wms.Areas.Ship.ViewModels.BtoBInstructionReference
{
    using PagedList;

    public class BtoBInstructionReference01Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<BtoBInstructionReference01ResultRow> BtoBInstructionReference01s { get; set; }
    }

    public class BtoBInstructionReference01ViewModel
    {
        public BtoBInstructionReference01SearchConditions SearchConditions { get; set; }

        public BtoBInstructionReference01Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBInstructionReference01ViewModel"/> class.
        /// </summary>
        public BtoBInstructionReference01ViewModel()
        {
            this.SearchConditions = new BtoBInstructionReference01SearchConditions();
            this.Results = new BtoBInstructionReference01Result();
        }
    }
}