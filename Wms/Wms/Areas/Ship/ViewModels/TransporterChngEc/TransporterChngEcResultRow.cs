namespace Wms.Areas.Ship.ViewModels.TransporterChngEc
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class TransporterChngEcResultRow
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
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        /// <remarks>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 注文日
        /// </summary>
        /// <remarks>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送指定日
        /// </summary>
        /// <remarks>
         [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        /// <remarks>
        public string TransporterName { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        /// <remarks>
        public string DestZip { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        public int AllocFlag { get; set; } = 0;

        /// <summary>
        /// バッチNo
        /// </summary>
        /// <remarks>
        public string BatchNo { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        public string BoxNo { get; set; }

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