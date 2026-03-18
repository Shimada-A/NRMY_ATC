namespace Wms.Areas.Ship.ViewModels.BtoBReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class BtoBReference01ResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterName { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        public string ShipStatusName { get; set; }

        /// <summary>
        /// 確定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? KakuDate { get; set; }

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

        /// <summary>
        /// 店舗別棚卸フラグ
        /// </summary>
        public string StoreInvFlag { get; set; }

        #endregion プロパティ
    }
}