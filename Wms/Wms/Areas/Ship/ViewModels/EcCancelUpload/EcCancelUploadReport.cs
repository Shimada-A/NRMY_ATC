namespace Wms.Areas.Ship.ViewModels.EcCancelUpload
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class EcCancelUploadReport
    {
        #region プロパティ

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(EcCancelUploadResource.CenterId), ResourceType = typeof(EcCancelUploadResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(EcCancelUploadResource.ShipPlanDate), ResourceType = typeof(EcCancelUploadResource), Order = 2)]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
        [Display(Name = nameof(EcCancelUploadResource.ShipInstructId), ResourceType = typeof(EcCancelUploadResource), Order = 3)]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 注文日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [Display(Name = nameof(EcCancelUploadResource.OrderDate), ResourceType = typeof(EcCancelUploadResource), Order = 4)]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        [Display(Name = nameof(EcCancelUploadResource.TransporterName), ResourceType = typeof(EcCancelUploadResource), Order = 5)]
        public string TransporterName { get; set; }

        /// <summary>
        /// 配送指定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(EcCancelUploadResource.ArriveRequestDate), ResourceType = typeof(EcCancelUploadResource), Order = 6)]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [Display(Name = nameof(EcCancelUploadResource.MakeDate), ResourceType = typeof(EcCancelUploadResource), Order = 7)]
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        [Display(Name = nameof(EcCancelUploadResource.ShipStatus), ResourceType = typeof(EcCancelUploadResource), Order = 8)]
        public string ShipStatusName { get; set; }

        /// <summary>
        /// 出荷確定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(EcCancelUploadResource.KakuDate), ResourceType = typeof(EcCancelUploadResource), Order = 9)]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        [Display(Name = nameof(EcCancelUploadResource.EcShipClass), ResourceType = typeof(EcCancelUploadResource), Order = 10)]
        public string EcShipClassName { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(EcCancelUploadResource.OrderQty), ResourceType = typeof(EcCancelUploadResource), Order = 11)]
        public int? OrderQty { get; set; }

        /// <summary>
        /// GAS欠品
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(EcCancelUploadResource.StockOutQty), ResourceType = typeof(EcCancelUploadResource), Order = 12)]
        public int? StockoutQty { get; set; }

        /// <summary>
        /// ｷｬﾝｾﾙ指示
        /// </summary>
        [Display(Name = nameof(EcCancelUploadResource.CancelFlags), ResourceType = typeof(EcCancelUploadResource), Order = 13)]
        public string CancelFlag { get; set; }

        #endregion プロパティ
    }
}