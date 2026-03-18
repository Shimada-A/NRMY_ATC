namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ケース出荷作業進捗
    /// </summary>
    public class TransferReferenceCaseRow
    {
        /// <summary>
        /// バッチNo(上段)
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチNo(下段)
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        /// 出荷先数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipToStoreQty { get; set; }

        /// <summary>
        /// 指示ケース数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructCaseQty { get; set; }

        /// <summary>
        /// 抜き取りJAN数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? JanQty { get; set; }

        /// <summary>
        /// 抜き取り指示数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 終了ケース/JANピック数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PicQty { get; set; }

        /// <summary>
        /// 終了ケース/JANピック進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PicPercent { get; set; }

        /// <summary>
        /// 欠品登録数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockOutRegQty { get; set; }

        /// <summary>
        /// 欠品確定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockOutFixQty { get; set; }

        /// <summary>
        /// 納品書発行終了数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? NouhinPrnQty { get; set; }

        /// <summary>
        /// 納品書発行仕進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? NouhinPrnPercent { get; set; }

        /// <summary>
        /// ピック種別
        /// </summary>
        public string PickKind { get; set; }

    }
}