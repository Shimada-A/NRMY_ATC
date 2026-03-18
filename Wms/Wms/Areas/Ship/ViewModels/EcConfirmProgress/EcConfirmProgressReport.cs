namespace Wms.Areas.Ship.ViewModels.EcConfirmProgress
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class EcConfirmProgressReport
    {
        #region プロパティ

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.CenterId), ResourceType = typeof(EcConfirmProgressResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(EcConfirmProgressResource.ShipPlanDate), ResourceType = typeof(EcConfirmProgressResource), Order = 2)]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.ShipInstructId), ResourceType = typeof(EcConfirmProgressResource), Order = 3)]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 注文日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [Display(Name = nameof(EcConfirmProgressResource.OrderDate), ResourceType = typeof(EcConfirmProgressResource), Order = 4)]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.TransporterName), ResourceType = typeof(EcConfirmProgressResource), Order = 5)]
        public string TransporterName { get; set; }

        /// <summary>
        /// 配送指定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(EcConfirmProgressResource.ArriveRequestDate), ResourceType = typeof(EcConfirmProgressResource), Order = 6)]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [Display(Name = nameof(EcConfirmProgressResource.MakeDate), ResourceType = typeof(EcConfirmProgressResource), Order = 7)]
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.ShipStatus), ResourceType = typeof(EcConfirmProgressResource), Order = 8)]
        public string ShipStatusName { get; set; }

        /// <summary>
        /// 出荷確定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(EcConfirmProgressResource.KakuDate), ResourceType = typeof(EcConfirmProgressResource), Order = 9)]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.EcShipClass), ResourceType = typeof(EcConfirmProgressResource), Order = 10)]
        public string EcShipClassName { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(EcConfirmProgressResource.OrderQty), ResourceType = typeof(EcConfirmProgressResource), Order = 11)]
        public int? OrderQty { get; set; }

        /// <summary>
        /// GAS欠品
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(EcConfirmProgressResource.StockOutQty), ResourceType = typeof(EcConfirmProgressResource), Order = 12)]
        public int? StockoutQty { get; set; }

        /// <summary>
        /// ｷｬﾝｾﾙ指示
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.CancelFlags), ResourceType = typeof(EcConfirmProgressResource), Order = 13)]
        public string CancelFlag { get; set; }

        /// <summary>
        /// 関連注文番号
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.RelatedOrderNo), ResourceType = typeof(EcConfirmProgressResource), Order = 14)]
        public string RelatedOrderNo { get; set; }

        /// <summary>
        /// 送り状No
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.DeliNo), ResourceType = typeof(EcConfirmProgressResource), Order = 15)]
        public string DeliNo { get; set; }
        #endregion プロパティ
    }
}