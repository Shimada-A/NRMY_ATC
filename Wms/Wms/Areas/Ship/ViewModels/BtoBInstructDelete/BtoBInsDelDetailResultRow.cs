namespace Wms.Areas.Ship.ViewModels.BtoBInstructDelete
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class BtoBInsDelDetailResultRow
    {
        #region プロパティ

        /// <summary>
        /// 出荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        /// <remarks>
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        /// <remarks>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 明細ID
        /// </summary>
        /// <remarks>
        public string ShipInstructSeq { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
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
        /// 予定数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructQty { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        /// <remarks>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        /// <remarks>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 出荷先区分
        /// </summary>
        /// <remarks>
        public string ShipToStoreClassName { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        /// <remarks>
        public string TransporterName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        public string ItemSkuId { get; set; }

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