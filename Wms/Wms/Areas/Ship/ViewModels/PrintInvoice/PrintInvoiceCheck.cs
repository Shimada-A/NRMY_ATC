namespace Wms.Areas.Ship.ViewModels.PrintInvoice
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PrintInvoiceCheck
    {
        public int RowNo { get; set; }
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; }
        /// <summary>
        /// 出荷指示ID
        /// </summary>
        public string ShipInstructId { get; set; }
        /// <summary>
        /// 出荷指示明細ID
        /// </summary>
        public int ShipInstructSeq { get; set; }
        /// <summary>
        /// 引当後出荷停止フラグ
        /// </summary>
        public int AftAllocStopFlag { get; set; }
        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }
        /// <summary>
        /// 配送業者ID
        /// </summary>
        public string TransporterId { get; set; }
        /// <summary>
        /// 検品フラグ
        /// </summary>
        public int KenFlag { get; set; }
        /// <summary>
        /// 納品書発行フラグ
        /// </summary>
        public int NouhinPrnFlag { get; set; }
        /// <summary>
        /// 送り状発行フラグ
        /// </summary>
        public int DeliPrnFlag { get; set; }
        /// <summary>
        /// 送り状番号
        /// </summary>
        public string DeliNo { get; set; }
        /// <summary>
        /// 配送業者マスタ送り状発行フラグ(0:発行しない 1:発行する)
        /// </summary>
        public int InvoicePrintFlag { get; set; }
        /// <summary>
        /// 店舗別棚卸フラグ
        /// </summary>
        public string StoreInvFlag { get; set; }

        /// <summary>
        /// 顧客コード(新しい値)
        /// </summary>
        public string NewClientCd { get; set; }

        /// <summary>
        /// 連携状況（配送業者）
        /// </summary>
        public int IfStateTrp { get; set; }

        /// <summary>
        /// 顧客コード
        /// </summary>
        public string ClientCd { get; set; }

        /// <summary>
        /// 出荷先店舗ID
        /// </summary>
        public string ShipToStoreId { get; set; }
    }
}