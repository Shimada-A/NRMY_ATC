using PagedList;

namespace Wms.Areas.Stock.ViewModels.InOutReference
{
    public class InOutReferenceViewModel
    {
        public InOutReferenceSearchConditions SearchConditions { get; set; }

        public IPagedList<InOutReferenceResultRow> Results { get; set; }
    }
}