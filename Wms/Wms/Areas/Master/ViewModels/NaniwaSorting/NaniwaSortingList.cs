namespace Wms.Areas.Master.ViewModels.NaniwaSorting
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    public class NaniwaSortingList : BaseModel
    {
        /// <summary>
        /// 店舗ID
        /// </summary>
        [Display(Name = nameof(NaniwaSortingResource.StoreId), ResourceType = typeof(NaniwaSortingResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// 店舗名
        /// </summary>
        [Display(Name = nameof(NaniwaSortingResource.StoreName), ResourceType = typeof(NaniwaSortingResource))]
        public string StoreName { get; set; }

        /// <summary>
        /// 配送センターコード
        /// </summary>
        [Display(Name = nameof(NaniwaSortingResource.NaniwaDeliCenterCd), ResourceType = typeof(NaniwaSortingResource))]
        public string NaniwaDeliCenterCd { get; set; }

        /// <summary>
        /// 配送センター名
        /// </summary>
        [Display(Name = nameof(NaniwaSortingResource.NaniwaDeliCenterName), ResourceType = typeof(NaniwaSortingResource))]
        public string NaniwaDeliCenterName { get; set; }

        /// <summary>
        /// ROWID
        /// </summary>
        public string RowId { get; set; }
    }
}