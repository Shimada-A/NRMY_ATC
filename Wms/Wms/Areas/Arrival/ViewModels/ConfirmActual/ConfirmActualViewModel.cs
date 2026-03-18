using PagedList;

namespace Wms.Areas.Arrival.ViewModels.ConfirmActual
{
    public class ConfirmActualResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<ConfirmActualResultRow> ConfirmActuals { get; set; }
    }

    public class ConfirmActualViewModel
    {
        public ConfirmActualSearchConditions SearchConditions { get; set; }

        public ConfirmActualResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmActualViewModel"/> class.
        /// </summary>
        public ConfirmActualViewModel()
        {
            this.SearchConditions = new ConfirmActualSearchConditions();
            this.Results = new ConfirmActualResult();
        }
    }
}