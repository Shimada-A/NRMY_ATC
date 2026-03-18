namespace Wms.Areas.Inventory.ViewModels.Confirm
{
    using PagedList;

    public class ConfirmResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<ConfirmResultRow> Confirms { get; set; }
    }

    public class ConfirmViewModel
    {
        public ConfirmSearchConditions SearchConditions { get; set; }

        public ConfirmResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmViewModel"/> class.
        /// </summary>
        public ConfirmViewModel()
        {
            this.SearchConditions = new ConfirmSearchConditions();
            this.Results = new ConfirmResult();
        }
    }
}