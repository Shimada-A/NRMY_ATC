namespace Wms.Areas.Ship.ViewModels.EcAllocationCancel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class EcAllocationCancelResultRow
    {
        #region プロパティ

        /// <summary>
        /// 出荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipRequestDate { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
        /// <remarks>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 注文日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        /// <remarks>
        public string TransporterName { get; set; }

        /// <summary>
        /// 配送指定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? DataDate { get; set; }

        /// <summary>
        /// 指示数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? OrderQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        #endregion プロパティ
    }
}