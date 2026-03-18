namespace Wms.Areas.Ship.ViewModels.BtoBInstructionReference
{
    using PagedList;
    using System.Collections.Generic;

    public class BtoBInstructionReference02DetailResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IEnumerable<BtoBInstructionReference02DetailResultRow> BtoBInstructionReference02Details { get; set; }
    }

    public class BtoBInstructionReference02PackageDetailResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IEnumerable<BtoBInstructionReference02PackageDetailResultRow> BtoBInstructionReference02PackageDetails { get; set; }
    }

    public class BtoBInstructionReference02ViewModel
    {
        public BtoBInstructionReference02SearchConditions SearchConditions { get; set; }

        public BtoBInstructionReference02DetailResult DetailResults { get; set; }
        public BtoBInstructionReference02PackageDetailResult PackageDetailResults { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BtoBInstructionReference02ViewModel"/> class.
        /// </summary>
        public BtoBInstructionReference02ViewModel()
        {
            this.SearchConditions = new BtoBInstructionReference02SearchConditions();
            this.DetailResults = new BtoBInstructionReference02DetailResult();
            this.PackageDetailResults = new BtoBInstructionReference02PackageDetailResult();
        }
    }
}