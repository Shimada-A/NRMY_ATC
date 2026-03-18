namespace Wms.Areas.Ship.ViewModels.PrintInvoice
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// 海外アソート出荷　判定
    /// </summary>
    public class PrintInvoiceAssortCheck
    {
        /// <summary>
        /// 倉庫ID
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 得意先ID
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷区分
        /// </summary>
        public string ShipClass { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        public string SlipDate { get; set; }

        /// <summary>
        /// 分納枝番
        /// </summary>
        public int? ArriveBranch { get; set; }
    }
}