namespace Wms.Areas.Stock.ViewModels.LocMove 
{ 
    using PagedList;

    public class LocMoveResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<LocMoveResultRow> LocMoves { get; set; }
    }

    public class LocMoveViewModel
    {
        public LocMoveSearchConditions SearchConditions { get; set; }

        public LocMoveResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocMoveViewModel"/> class.
        /// </summary>
        public LocMoveViewModel()
        {
            this.SearchConditions = new LocMoveSearchConditions();
            this.Results = new LocMoveResult();
        }
    }
}