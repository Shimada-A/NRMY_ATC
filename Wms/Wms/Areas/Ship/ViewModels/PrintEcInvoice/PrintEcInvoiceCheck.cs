namespace Wms.Areas.Ship.ViewModels.PrintEcInvoice
{

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PrintEcInvoiceCheck
    {
        /// <summary>
        /// 出荷指示ID
        /// </summary>
        public string ShipInstructId { get; set; }
        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 納品書発行フラグ
        /// </summary>
        public int NouhinPrnFlag { get; set; }
        /// <summary>
        /// 送り状発行フラグ
        /// </summary>
        public int DeliPrnFlag { get; set; }
        /// <summary>
        /// 検品フラグ
        /// </summary>
        public int KenFlag { get; set; }
        /// <summary>
        /// 実績数
        /// </summary>
        public int ResultQty { get; set; }
        /// <summary>
        /// 引当後キャンセルフラグ
        /// </summary>
        public int AftAllocCancelFlag { get; set; }
        /// <summary>
        /// 引当後更新ありフラグ
        /// </summary>
        public int AftAllocUpFlag { get; set; }
        /// <summary>
        /// GAS実績数
        /// </summary>
        public int GasQty { get; set; }
        /// <summary>
        /// 梱包番号(出荷梱包実績)
        /// </summary>
        public string PackBoxNo { get; set; }
        /// <summary>
        /// 引当数(EC出荷)
        /// </summary>
        public int EcAllocQty { get; set; }
        /// <summary>
        /// GAS実績日時
        /// </summary>
        public string GasDate { get; set; }
        /// <summary>
        /// 配送業者ID
        /// </summary>
        public string TransporterId { get; set; }

    }
}