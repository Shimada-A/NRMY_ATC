namespace Wms.Areas.Ship.ViewModels.EcConfirmProgress
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// DC List
    /// </summary>
    public class EcConfirmProgress02ResultRow
    {
        #region プロパティ

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
        /// 注文番号
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 注文日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送先都道府県
        /// </summary>
        public string DestPrefName { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterName { get; set; }

        /// <summary>
        /// 仕分番号
        /// </summary>
        public string DeliShiwakeCd { get; set; }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        public string EcShipClass { get; set; }

        /// <summary>
        /// 配送指定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// 配送時間帯
        /// </summary>
        public string ArriveRequestTime { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// GASバッチNo
        /// </summary>
        public string GASBatchNo { get; set; }

        /// <summary>
        /// GAS間口No
        /// </summary>
        public int GasMaguchiNo { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 送り状No
        /// </summary>
        public string DeliNo { get; set; }

        /// <summary>
        /// 出荷指示明細ID
        /// </summary>
        public int ShipInstructSeq { get; set; }

        /// <summary>
        /// 分類名1
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
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 納品書発行担当者
        /// </summary>
        public string NouhinPrnUserId { get; set; }

        /// <summary>
        /// 関連注文番号
        /// </summary>
        public string RelatedOrderNo { get; set; }

        /// <summary>
        /// 検品日時
        /// </summary>
        public string KenDate { get; set; }

        /// <summary>
        /// 検品担当者
        /// </summary>
        public string KenUserName { get; set; }

        #endregion プロパティ
    }
}