namespace Wms.Areas.Master.ViewModels.Location
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;

    public class ReportTemp
    {
        /// <summary>
        /// エリア
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec1), ResourceType = typeof(Resources.LocationResource), Order = 3)]
        public string Locsec_1 { get; set; }

        /// <summary>
        /// 棚列
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec2), ResourceType = typeof(Resources.LocationResource), Order = 4)]
        public string Locsec_2 { get; set; }

        /// <summary>
        /// 棚番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec3), ResourceType = typeof(Resources.LocationResource), Order = 5)]
        public string Locsec_3 { get; set; }

        /// <summary>
        /// 段
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec4), ResourceType = typeof(Resources.LocationResource), Order = 6)]
        public string Locsec_4 { get; set; }

        /// <summary>
        /// 間口
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec5), ResourceType = typeof(Resources.LocationResource), Order = 7)]
        public string Locsec_5 { get; set; }

        /// <summary>
        /// ロケーション区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.LocationClass), ResourceType = typeof(Resources.LocationResource), Order = 8)]
        public string LocationClass { get; set; }

        /// <summary>
        /// 格付ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.GradeId), ResourceType = typeof(Resources.LocationResource), Order = 10)]
        public string GradeId { get; set; }

        /// <summary>
        /// 引当優先順位
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.AllocPriority), ResourceType = typeof(Resources.LocationResource), Order = 11)]
        public string AllocPriority { get; set; }

        /// <summary>
        /// ピッキンググループNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.PickingGroupNo), ResourceType = typeof(Resources.LocationResource), Order = 12)]
        public string PickingGroupNo { get; set; }

    }
}