namespace Wms.Areas.Master.ViewModels.NaniwaSorting
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;

    public class Report
    {
        /// <summary>
        /// 店舗ID
        /// </summary>
        [Display(Name = "StoreId", ResourceType = typeof(Resources.NaniwaSortingResource), Order = 1)]
        public string StoreId { get; set; }

        /// <summary>
        /// 店舗名
        /// </summary>
        [Display(Name = "StoreName", ResourceType = typeof(Resources.NaniwaSortingResource), Order = 2)]
        public string StoreName { get; set; }

        /// <summary>
        /// 配送センターコード
        /// </summary>
        /// <remarks>
        /// 汎用コードマスタ「浪速運送配送センター」
        /// </remarks>
        [Display(Name = "NaniwaDeliCenterCd", ResourceType = typeof(Resources.NaniwaSortingResource), Order = 3)]
        public string NaniwaDeliCenterCd { get; set; }

        /// <summary>
        /// 配送センター名
        /// </summary>
        /// <remarks>
        /// 汎用コードマスタ「浪速運送配送センター」
        /// </remarks>
        [Display(Name = "NaniwaDeliCenterName", ResourceType = typeof(Resources.NaniwaSortingResource), Order = 4)]
        public string NaniwaDeliCenterName { get; set; }
    }
}