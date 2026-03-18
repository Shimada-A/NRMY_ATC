namespace Wms.Areas.Master.ViewModels.ShipFrontage
{
    using CsvHelper.Configuration.Attributes;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;

    public class ReportTemp
    {
        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ShipFrontageResource.BrandId), ResourceType = typeof(Resources.ShipFrontageResource), Order = 1)]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名
        /// </summary>
        [Display(Name = nameof(Resources.ShipFrontageResource.BrandId), ResourceType = typeof(Resources.ShipFrontageResource),Order = 2)]
        public string BrandName { get; set; }

        /// <summary>
        /// レーンNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ShipFrontageResource.LaneNo), ResourceType = typeof(Resources.ShipFrontageResource), Order = 3)]
        public string LaneNo { get; set; }

        /// <summary>
        /// 間口No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ShipFrontageResource.FrontageNo), ResourceType = typeof(Resources.ShipFrontageResource), Order = 4)]
        public string FrontageNo { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ShipFrontageResource.StoreId), ResourceType = typeof(Resources.ShipFrontageResource), Order = 5)]
        public string StoreId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        [Display(Name = nameof(Resources.ShipFrontageResource.StoreName), ResourceType = typeof(Resources.ShipFrontageResource),Order = 6)]
        public string StoreName { get; set; }
    }
}