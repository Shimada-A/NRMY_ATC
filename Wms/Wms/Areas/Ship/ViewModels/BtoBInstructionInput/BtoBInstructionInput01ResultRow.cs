namespace Wms.Areas.Ship.ViewModels.BtoBInstructionInput
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class BtoBInstructionInput01ResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 指示区分
        /// </summary>
        public string InstructClassName { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 明細ID
        /// </summary>
        public string ShipInstructSeq { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        public string Jan { get; set; }

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
        /// 出荷先数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipToQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructQty { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? MakeDate { get; set; }

        /// <summary>
        /// 出荷可能数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public string AllocQty { get; set; }

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