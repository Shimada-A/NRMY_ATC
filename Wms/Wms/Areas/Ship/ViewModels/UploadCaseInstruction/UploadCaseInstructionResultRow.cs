namespace Wms.Areas.Ship.ViewModels.UploadCaseInstruction
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// 在庫仕分指示明細
    /// </summary>
    public class UploadCaseInstructionResultRow
    {
        #region プロパティ

        public bool IsCheck { get; set; }


        /// <summary>
        /// No
        /// </summary>
        public int? LineNo { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// ケース出荷指示取込日時
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? ShipInstructDate { get; set; }

        /// <summary>
        /// ケース出荷指示名称
        /// </summary>
        public string ShipInstructName { get; set; }

        /// <summary>
        /// 出荷種別
        /// </summary>
        public string ShipKindName { get; set; }

        /// <summary>
        /// 明細行数
        /// </summary>
        public int? DetailRowQty { get; set; }

        /// <summary>
        /// 店舗数
        /// </summary>
        public int? StoreQty { get; set; }

        /// <summary>
        /// ケース数
        /// </summary>
        public int? CaseQty { get; set; }

        /// <summary>
        /// 抜き取りJAN数
        /// </summary>
        public int? PicJanQty { get; set; }

        /// <summary>
        /// 抜き取り指示数
        /// </summary>
        public int? PicInsQty { get; set; }

        /// <summary>
        /// 引当エラー数
        /// </summary>
        public int? HikiErrQty { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        public string CenterId { get; set; }

        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }

        /// <summary>
        /// ワークID
        /// </summary>
        public int Seq { get; set; }

        #endregion プロパティ
    }
}