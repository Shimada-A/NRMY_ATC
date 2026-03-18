namespace Wms.Areas.Master.ViewModels.Location
{
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    public class LocationLabelReportRowForCsv 
    {
        /// <summary>
        /// ロケーションコード(前方)
        /// </summary>
        [Display(Name = nameof(LocationResource.LocationCdFront), ResourceType = typeof(LocationResource), Order = 1)]
        public string LocationCdFront { get; set; }

        /// <summary>
        /// ロケーションコード(後方)
        /// </summary>
        [Display(Name = nameof(LocationResource.LocationCdRear), ResourceType = typeof(LocationResource), Order = 2)]
        public string LocationCdRear { get; set; }

        /// <summary>
        /// ロケーションコード (バーコード用)
        /// </summary>
        [Display(Name = nameof(LocationResource.BarCode), ResourceType = typeof(LocationResource), Order = 3)]
        public string LocationCd { get; set; }
    }
}