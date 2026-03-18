namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// EC出荷作業進捗
    /// </summary>
    public class TransferReferenceEcRow
    {

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        public bool IsCheck { get; set; }

        /// <summary>
        /// バッチNo(上段)
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチNo(下段)
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        /// オーダー数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipInstructQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// 指示行数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipInstructSeqQty { get; set; }

        /// <summary>
        /// 指示数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 出荷ピック終了数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PicPicQty { get; set; }

        /// <summary>
        /// 出荷ピック進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PicPicPercent { get; set; }

        /// <summary>
        /// 出荷ピック欠品登録
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PicStockOutRegQty { get; set; }

        /// <summary>
        /// 出荷ピック欠品確定
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PicStockOutFixQty { get; set; }

        /// <summary>
        /// ユニット仕分終了数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? EcunitSortQty { get; set; }

        /// <summary>
        /// ユニット仕分進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? EcunitSortPercent { get; set; }

        /// <summary>
        /// GAS進捗GASバッチ数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? GasBatchNoQty { get; set; }

        /// <summary>
        /// GAS進捗終了数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? GasQty { get; set; }

        /// <summary>
        /// GAS進捗進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? GasPercent { get; set; }

        /// <summary>
        /// GAS進捗欠品登録
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? GasStockOutQty { get; set; }

        /// <summary>
        /// GAS進捗欠品確定
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
    }
}