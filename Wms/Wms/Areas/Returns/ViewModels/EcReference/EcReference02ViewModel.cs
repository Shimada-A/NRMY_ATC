namespace Wms.Areas.Returns.ViewModels.EcReference
{
    using System.Collections.Generic;
    public class EcReference02Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public List<EcReference02ResultRow> EcReference02s { get; set; }
    }

    public class EcReference02ViewModel
    {
        public EcReference02SearchConditions SearchConditions { get; set; }

        public EcReference02Result Results { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcReference02ViewModel"/> class.
        /// </summary>
        public EcReference02ViewModel()
        {
            this.SearchConditions = new EcReference02SearchConditions();
            this.Results = new EcReference02Result();
        }
    }
}