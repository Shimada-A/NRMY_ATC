namespace Wms.Areas.Ship.ViewModels.BtoBInstructionReference
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class BtoBInstructionReference02SearchConditions
    {

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? AllocDate { get; set; }

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
        public long? ShipInstructSeq { get; set; }

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
        /// 配送業者仕分コード
        /// </summary>
        public string DeliShiwakeCd { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 実績数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQtySum { get; set; }

        /// <summary>
        /// 欠品確定日未入力Flag
        /// </summary>
        public bool StockOutFixDateFlag { get; set; }

        /// <summary>
        /// 連携状況(基幹) 0:未送信 1:1部送信済 2:送信済み 3:送信対象外（アソート出荷のとき）
        /// </summary>
        public int? IfStateErp { get; set; }

        /// <summary>
        /// センターID
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 累積フラグ
        /// </summary>
        public int PastFlag { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        /// <summary>
        /// 仕分状況 0:未作業　1:仕分中 2:中断中 3:仕分完了
        /// </summary>
        public int? SortStatus { get; set; }
    }
}