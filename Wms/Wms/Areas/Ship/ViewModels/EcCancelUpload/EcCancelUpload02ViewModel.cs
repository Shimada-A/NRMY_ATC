namespace Wms.Areas.Ship.ViewModels.EcCancelUpload
{
    using PagedList;
    using System.Collections.Generic;

    public class EcCancelUpload02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IEnumerable<EcCancelUpload02ResultRow> EcCancelUpload02s { get; set; }
    }

    public class EcCancelUpload02ViewModel
    {
        public EcCancelUpload02SearchConditions SearchConditions { get; set; }

        public EcCancelUpload02Result DetailResults { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcCancelUpload02ViewModel"/> class.
        /// </summary>
        public EcCancelUpload02ViewModel()
        {
            this.SearchConditions = new EcCancelUpload02SearchConditions();
            this.DetailResults = new EcCancelUpload02Result();
        }
    }
}