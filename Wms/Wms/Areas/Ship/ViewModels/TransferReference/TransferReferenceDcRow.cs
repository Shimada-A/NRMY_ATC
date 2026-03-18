namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC出荷作業進捗
    /// </summary>
    public class TransferReferenceDcRow
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
        /// 出荷先数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipToStoreQty { get; set; }

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
        /// レーン仕分終了SKU
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? LaneSortItemSkuQty { get; set; }

        /// <summary>
        /// レーン仕分終了数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? LaneSortSortQty { get; set; }

        /// <summary>
        /// レーン仕分進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? LaneSortPercent { get; set; }

        /// <summary>
        /// レーン仕分終了状態
        /// </summary>
        public string LaneSortEndStatus { get; set; }

        /// <summary>
        /// 店別仕分終了SKU
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StoreSortItemSkuQty { get; set; }

        /// <summary>
        /// 店別仕分終了数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StoreSortSortQty { get; set; }

        /// <summary>
        /// 店別仕分進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StoreSortPercent { get; set; }

        /// <summary>
        /// 店別仕分終了状態
        /// </summary>
        public string StoreSortEndStatus { get; set; }

        /// <summary>
        /// 摘取終了出荷先
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? OrderPicShipToStoreQty { get; set; }

        /// <summary>
        /// 摘取終了数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? OrderPicQty { get; set; }

        /// <summary>
        /// 摘取進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? OrderPicPercent { get; set; }

        /// <summary>
        /// 欠品登録
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockOutRegQty { get; set; }

        /// <summary>
        /// 確定登録
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

        /// <summary>
        /// 出荷種別
        /// </summary>
        public string ShipKind { get; set; }
    }
}