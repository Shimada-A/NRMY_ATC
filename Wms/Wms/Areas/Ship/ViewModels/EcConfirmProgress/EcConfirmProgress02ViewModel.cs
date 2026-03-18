namespace Wms.Areas.Ship.ViewModels.EcConfirmProgress
{
    using PagedList;
    using System.Collections.Generic;

    public class EcConfirmProgress02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IEnumerable<EcConfirmProgress02ResultRow> EcConfirmProgress02s { get; set; }
    }

    public class EcConfirmProgress02ViewModel
    {
        public EcConfirmProgress02SearchConditions SearchConditions { get; set; }

        public EcConfirmProgress02Result DetailResults { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcConfirmProgress02ViewModel"/> class.
        /// </summary>
        public EcConfirmProgress02ViewModel()
        {
            this.SearchConditions = new EcConfirmProgress02SearchConditions();
            this.DetailResults = new EcConfirmProgress02Result();
        }
    }
}