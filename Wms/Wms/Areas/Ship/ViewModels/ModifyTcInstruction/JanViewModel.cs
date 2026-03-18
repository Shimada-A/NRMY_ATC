using System;

namespace Wms.Areas.Ship.ViewModels.JanSearchModal
{
    public partial class JanViewModel
    {
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

        public string InvoiceNo { get; set; }

        public string VendorId { get; set; }

        public string VendorName1 { get; set; }

        public int? StockQty { get; set; }

        public string InvoiceEndDate { get; set; }

        public string InvoiceUserId { get; set; }

        public string InvoiceUserName { get; set; }
    }
}