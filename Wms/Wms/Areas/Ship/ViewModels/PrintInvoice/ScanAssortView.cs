namespace Wms.Areas.Ship.ViewModels.PrintInvoice
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// 出荷アソートスキャン明細
    /// </summary>
    public class ScanAssortView
    {
        /// <summary>
        /// 行番号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 出荷区分
        /// </summary>
        public int ShipClass { get; set; }

        /// <summary>
        /// 明細項目
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        public string SlipDate { get; set; }

        /// <summary>
        /// 予定梱包数
        /// </summary>
        public int PlanQty { get; set; }

        /// <summary>
        /// 実績梱包数
        /// </summary>
        public int ResultQty { get; set; }

        /// <summary>
        /// 倉庫名
        /// </summary>
        public string CenterName { get; set; }
    }
}