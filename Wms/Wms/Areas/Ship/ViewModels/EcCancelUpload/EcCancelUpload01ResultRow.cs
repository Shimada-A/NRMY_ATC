namespace Wms.Areas.Ship.ViewModels.EcCancelUpload
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class EcCancelUpload01ResultRow
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

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? CuDate { get; set; }

        public string CuClass { get; set; }

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
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

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
        /// GAS実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? GasQty { get; set; }

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

        public long ItemSkuIdCnt { get; set; }

        #endregion プロパティ
    }
}