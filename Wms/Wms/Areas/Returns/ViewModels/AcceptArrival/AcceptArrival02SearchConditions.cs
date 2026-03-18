namespace Wms.Areas.Returns.ViewModels.AcceptArrival
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Returns.Resources;
    public class AcceptArrival02SearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// Data to sort
        /// </summary>
        public enum DataSortKey : byte
        {
            [Display(Name = nameof(Resources.AcceptArrivalResource.ShpIns), ResourceType = typeof(Resources.AcceptArrivalResource))]
            ShpIns,

            [Display(Name = nameof(Resources.AcceptArrivalResource.KakuShpIns), ResourceType = typeof(Resources.AcceptArrivalResource))]
            KakuShpIns,

            [Display(Name = nameof(Resources.AcceptArrivalResource.ZipKaku), ResourceType = typeof(Resources.AcceptArrivalResource))]
            ZipKaku
        }

        /// <summary>
        /// Sort key
        /// </summary>
        public DataSortKey SortKey { get; set; } = DataSortKey.ShpIns;

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

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
        public  string JanCombine { get; set; }

        /// <summary>
        /// Scan数
        /// </summary>
        public string ScanQtyCombine { get; set; }
    }
}