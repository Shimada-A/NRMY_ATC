namespace Wms.Areas.Ship.ViewModels.BtoBReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class BtoBReference02ResultRow
    {
        #region プロパティ
        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 送り状No
        /// </summary>
        public string DeliNo { get; set; }

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
        /// 仕分番号
        /// </summary>
        public string DeliShiwakeCd { get; set; }

        /// <summary>
        /// カートン確定日時
        /// </summary>
        public string CaseKakuDate { get; set; }

        /// <summary>
        /// カートン確定担当者
        /// </summary>
        public string CaseKakuUser { get; set; }

        /// <summary>
        /// 検品日時
        /// </summary>
        public string KenDate { get; set; }

        /// <summary>
        /// 検品担当者
        /// </summary>
        public string KenUserName { get; set; }

        /// <summary>
        /// 納品書発行日
        /// </summary>
        public string NouhinPrnDate { get; set; }

        /// <summary>
        /// 納品書発行担当者
        /// </summary>
        public string NouhinPrnUserName { get; set; }

        /// <summary>
        /// 確定日
        /// </summary>
        public string KakuDate { get; set; }

        /// <summary>
        /// 確定担当者
        /// </summary>
        public string KakuUserName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 明細ID
        /// </summary>
        public string ShipInstructSeq { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        public string CategoryName1 { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

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
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }
        #endregion プロパティ
    }
}