namespace Wms.Areas.Stock.ViewModels.Adjust
{
    using PagedList;

    public class AdjustResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<AdjustResultRow> Adjusts { get; set; }
    }

    public class AdjustViewModel
    {
        public AdjustSearchConditions SearchConditions { get; set; }

        public AdjustResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjustViewModel"/> class.
        /// </summary>
        public AdjustViewModel()
        {
            this.SearchConditions = new AdjustSearchConditions();
            this.Results = new AdjustResult();
        }
    }
}