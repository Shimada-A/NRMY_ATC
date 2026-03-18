namespace Wms.Areas.Returns.ViewModels.AcceptArrival
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Returns.Resources;
    using Wms.Common;
    using Wms.Resources;
    public class AcceptArrival01SearchConditions
    {

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// EC出荷形態
        /// </summary>
        public ShpScanClasses ShpScanClass { get; set; } = ShpScanClasses.ShpScan;

        /// <summary>
        /// EC出荷形態
        /// </summary>
        public enum ShpScanClasses : byte
        {
            [Display(Name = nameof(AcceptArrivalResource.ShpScan), ResourceType = typeof(Resources.AcceptArrivalResource))]
            ShpScan = 1,

            [Display(Name = nameof(AcceptArrivalResource.JanScan), ResourceType = typeof(Resources.AcceptArrivalResource))]
            JanScan = 2,

        }

        /// <summary>
        /// EC注文番号
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        public string DestZip { get; set; }

        /// <summary>
        /// 届先氏名
        /// </summary>
        public string DestName { get; set; }

        /// <summary>
        /// 届先住所
        /// </summary>
        public string DestAddress { get; set; }

        /// <summary>
        /// 届先電話番号
        /// </summary>
        public string DestTel { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }
    }
}