using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wms.Areas.Returns.ViewModels.PurchaseReturns
{
    /// <summary>
    /// 納品書番号検索ViewModel
    /// </summary>
    public class InvoiceViewModel
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
        /// カラー名
        /// </summary>
        /// <remarks>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        public string Jan { get; set; }

        /// <summary>
        /// 明細項目
        /// </summary>
        public IList<InvoiceResultRow> InvoiceResults { get; set; }
    }

    public class InvoiceResultRow
    {
        public bool IsSelected { get; set; }

        public string VendorId { get; set; }

        public string VendorName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ConfirmDate { get; set; }

        public string InvoiceNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArriveDate { get; set; }

        public int NormalBuyingPrice { get; set; }

        public int ArriveQty { get; set; }

        public int ReturnableQty { get; set; }
    }
}