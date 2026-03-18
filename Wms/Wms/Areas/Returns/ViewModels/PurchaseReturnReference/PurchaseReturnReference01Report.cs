namespace Wms.Areas.Returns.ViewModels.PurchaseReturnReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Returns.Resources;

    public class PurchaseReturnReference01Report
    {
        // 連番
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.No), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 1)]
        public long LineNo { get; set; }

        // 登録日
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.ArriveDate), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 2)]
        public string ArriveDate { get; set; }

        //伝票ID
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.DenpyoId), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 3)]
        public string ReturnId { get; set; }

        //訂正区分
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.RetuenClass), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 4)]
        public string ReturnClassName { get; set; }

        //仕入先ID
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.VendorId), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 5)]
        public string VendorId { get; set; }

        //仕入先名
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.Vendor), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 6)]
        public string VendorName { get; set; }

        // 納品書番号 (INVOICE_NO)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.InvoiceNo), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 7)]
        public string InvoiceNo { get; set; }

        // 登録担当者ID (INPUT_USER_ID)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.InputUserIdId), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 8)]
        public string InputUserId { get; set; }

        // 登録担当者名 (INPUT_USER_NAME)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.InputUserId), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 9)]
        public string InputUserName { get; set; }

        //分類１
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.CategoryName1), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 10)]
        public string CategoryName { get; set; }

        // 品番 (ITEM_ID)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.ItemId), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 11)]
        public string ItemId { get; set; }

        //品名
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.ItemName), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 12)]
        public string ItemName { get; set; }

        // カラーID (ITEM_COLOR_ID)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.ItemColorId), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 13)]
        public string ItemColorId { get; set; }

        //カラー名
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.ItemColor), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 14)]
        public string ItemColorName { get; set; }

        // サイズID (ITEM_SIZE_ID)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.ItemSizeId), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 15)]
        public string ItemSizeId { get; set; }

        //サイズ名
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.ItemSize), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 16)]
        public string ItemSizeName { get; set; }

        // JAN (JAN)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.Jan), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 17)]
        public string Jan { get; set; }

        // SKU (ITEM_SKU_ID)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.Sku), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 18)]
        public string ItemSkuId { get; set; }

        // 返品実績数 (RETURN_QTY)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.SaiReturnQty), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 19)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ReturnQty { get; set; }

        // 標準下代 (NORMAL_BUYING_PRICE)
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.NormalBuyingPrice), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 20)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? NormalBuyingPrice { get; set; }

        // W下代 
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.WBuyingPrice), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 21)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? PurchaseBuyingPrice { get; set; }

        // 上代
        [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.SellingPrice), ResourceType = typeof(PurchaseReturnReferenceResource), Order = 22)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? NormalSellingPriceExTax { get; set; }

    }
}