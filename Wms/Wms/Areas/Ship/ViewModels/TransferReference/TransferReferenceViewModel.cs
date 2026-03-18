namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using PagedList;
    using System.Collections.Generic;

    //public class TransferReferenceTcResult
    //{
    //    /// <summary>
    //    /// List record
    //    /// </summary>
    //    public IList<TransferReferenceTcRow> TransferReferenceTcs { get; set; }
    //}

    public class TransferReferenceDcResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<TransferReferenceDcRow> TransferReferenceDcs { get; set; }
    }

    //public class TransferReferenceEcResult
    //{
    //    /// <summary>
    //    /// List record
    //    /// </summary>
    //    public IList<TransferReferenceEcRow> TransferReferenceEcs { get; set; }
    //}

    public class TransferReferenceCaseResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<TransferReferenceCaseRow> TransferReferenceCases { get; set; }
    }

    public class TransferReferenceViewModel
    {
        public TransferReferenceSearchConditions SearchConditions { get; set; }



        //public TransferReferenceTcResult TcResults { get; set; }

        public TransferReferenceDcResult DcResults { get; set; }

        //public TransferReferenceEcResult EcResults { get; set; }

        public TransferReferenceCaseResult CaseResults { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReferenceViewModel"/> class.
        /// </summary>
        public TransferReferenceViewModel()
        {
            this.SearchConditions = new TransferReferenceSearchConditions();
            //this.TcResults = new TransferReferenceTcResult();
            this.DcResults = new TransferReferenceDcResult();
            //this.EcResults = new TransferReferenceEcResult();
            this.CaseResults = new TransferReferenceCaseResult();
        }
    }
}