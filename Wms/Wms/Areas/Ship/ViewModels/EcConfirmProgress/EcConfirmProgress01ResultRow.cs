namespace Wms.Areas.Ship.ViewModels.EcConfirmProgress
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class EcConfirmProgress01ResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID
        /// </summary>
        public long? ShipInstructSeq { get; set; }

        /// <summary>
        /// 注文日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterName { get; set; }

        /// <summary>
        /// 配送指定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        public string ShipStatusName { get; set; }

        /// <summary>
        /// 出荷確定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        public string EcShipClassName { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? OrderQty { get; set; }

        /// <summary>
        /// GAS欠品
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockoutQty { get; set; }

        /// <summary>
        /// ｷｬﾝｾﾙ指示
        /// </summary>
        public string CancelFlag { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 予定数 (ALLOC_QTY)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// ケースNo (BOX_NO)
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 出荷先店舗ID (SHIP_TO_STORE_ID)
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; }

        #endregion プロパティ
    }
}