namespace Wms.Areas.Returns.ViewModels.PurchaseReturns
{
    using Share.Common.Resources;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Returns.Resources;

    public class PurchaseReturnsResultRow
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
        /// 標準下代
        /// </summary>
        /// <remarks>
        public int? NormalBuyingPrice { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        public int? StockQty { get; set; }

        /// <summary>
        /// 在庫数合計
        /// </summary>
        /// <remarks>
        public int? StockQtySum { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 下代
        /// </summary>
        /// <remarks>
        //[Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        //[Display(Name = nameof(PurchaseReturnsResource.BuyingPrice), ResourceType = typeof(PurchaseReturnsResource))]
        public int? BuyingPrice { get; set; }

        /// <summary>
        /// 返品可能数
        /// </summary>
        public int? ReturnableQty { get; set; }

        /// <summary>
        /// 返品数
        /// </summary>
        /// <remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PurchaseReturnsResource.ReturnQty), ResourceType = typeof(PurchaseReturnsResource))]
        public int? ReturnQty { get; set; }

        /// <summary>
        /// 返品数合計
        /// </summary>
        /// <remarks>
        public int? ReturnQtySum { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 入荷実績数
        /// </summary>
        public int? ArriveQty { get; set; }

        public int? ZeroFlg { get; set; }
    }
}