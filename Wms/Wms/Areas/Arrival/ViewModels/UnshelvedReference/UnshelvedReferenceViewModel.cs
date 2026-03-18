using PagedList;

namespace Wms.Areas.Arrival.ViewModels.UnshelvedReference
{
    public class UnshelvedReferenceResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<UnshelvedReferenceResultRow> UnshelvedReferences { get; set; }
    }

    public class UnshelvedReferenceViewModel
    {
        public UnshelvedReferenceSearchConditions SearchConditions { get; set; }

        public UnshelvedReferenceResult Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnshelvedReferenceViewModel"/> class.
        /// </summary>
        public UnshelvedReferenceViewModel()
        {
            this.SearchConditions = new UnshelvedReferenceSearchConditions();
            this.Results = new UnshelvedReferenceResult();
        }
    }
}