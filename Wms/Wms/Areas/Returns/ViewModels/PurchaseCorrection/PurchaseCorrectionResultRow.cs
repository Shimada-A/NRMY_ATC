namespace Wms.Areas.Returns.ViewModels.PurchaseCorrection
{
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Wms.Areas.Returns.Resources;

    public class PurchaseCorrectionResultRow
    {
        /// <summary>
        /// SEQ
        /// </summary>
        public string Seq { get; set; }

        /// <summary>
        /// LINE_NO
        /// </summary>
        public string LineNo { get; set; }

        /// <summary>
        /// 納品書No
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        public string CategoryName1 { get; set; }

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
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        public string LocationCd { get; set; }

        /// <summary>
        /// 標準下代
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? NormalBuyingPrice { get; set; }

        /// <summary>
        /// W下代
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PurchaseBuyingPrice { get; set; }

        /// <summary>
        /// 標準下代
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SaiQty { get; set; }

    }
}