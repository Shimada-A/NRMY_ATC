namespace Wms.Areas.Ship.ViewModels.EcAllocation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// EC引当明細
    /// </summary>
    public class EcAllocationResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipRequestDate { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
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
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? DataDate { get; set; }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        public string EcShipClass { get; set; }

        /// <summary>
        /// 指示数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? OrderQty { get; set; }

        /// <summary>
        /// 引当フラグ
        /// </summary>
        public int? AllocFlag { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        public string CenterId { get; set; }

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