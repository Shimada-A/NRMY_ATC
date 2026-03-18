using System.ComponentModel.DataAnnotations;
using Wms.Areas.Returns.Resources;

namespace Wms.Areas.Returns.ViewModels.AcceptArrival
{
    public class AcceptArrival03SearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

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
        public string JanCombine { get; set; }

        /// <summary>
        /// Scan数
        /// </summary>
        public string ScanQtyCombine { get; set; }

        /// <summary>
        /// スキャン区分
        /// </summary>
        public ShpScanClasses ShpScanClass { get; set; } = ShpScanClasses.ShpScan;

        /// <summary>
        /// スキャン区分
        /// </summary>
        public enum ShpScanClasses : byte
        {
            [Display(Name = nameof(AcceptArrivalResource.ShpScan), ResourceType = typeof(Resources.AcceptArrivalResource))]
            ShpScan = 1,
            [Display(Name = nameof(AcceptArrivalResource.JanScan), ResourceType = typeof(Resources.AcceptArrivalResource))]
            JanScan = 2,
        }
    }
}