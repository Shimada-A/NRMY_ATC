namespace Wms.Areas.Returns.ViewModels.PurchaseCorrection
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PurchaseCorrectionReport
    {

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.PrintUser), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 1)]
        public string PrintUser { get; set; }

        /// <summary>
        /// 訂正日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.CorrectionDate), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 2)]
        public string ArriveDate { get; set; }

        /// <summary>
        /// 返品伝票ID
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.ReturnId), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 3)]
        public string ReturnId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.VendorId), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 4)]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.Vendor), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 5)]
        public string VendorName { get; set; }

        /// <summary>
        /// 仕入先郵便番号
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.CenterZip), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 6)]
        public string CenterZip { get; set; }

        /// <summary>
        /// 仕入先住所
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.CenterAddress), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 7)]
        public string CenterAddress { get; set; }

        /// <summary>
        /// 仕入先電話番号
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.CenterTel), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 8)]
        public string CenterTel { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.CenterName), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 9)]
        public string CenterName { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.CategoryName1), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 10)]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.CategoryName1), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 11)]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.Item), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 12)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.Item), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 13)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.ItemColor), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 14)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.ItemColor), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 15)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.ItemSize), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 16)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.ItemSize), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 17)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.Jan), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 18)]
        public string Jan1 { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.Jan), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 19)]
        public string Jan2 { get; set; }

        /// <summary>
        /// 下代
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.BuyingPrice), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 20)]
        public int? NormalBuyingPrice { get; set; }

        /// <summary>
        /// W下代
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.WBuyingPrice), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 21)]
        public int? PurchaseBuyingPrice { get; set; }

        /// <summary>
        /// 上代
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.SellingPrice), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 22)]
        public int? NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// 返品数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseCorrectionResource.ReturnQty), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 23)]
        public int? ReturnQty { get; set; }

        [Display(Name = nameof(Resources.PurchaseCorrectionResource.PrintDate), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 24)]
        public string PrintDate { get; set; }

        [Display(Name = nameof(Resources.PurchaseCorrectionResource.LocationCd), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 25)]
        public string LocationCd { get; set; }

        [Display(Name = nameof(Resources.PurchaseCorrectionResource.BoxNo), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 26)]
        public string BoxNo { get; set; }

        [Display(Name = nameof(Resources.PurchaseCorrectionResource.InvoiceNo), ResourceType = typeof(Resources.PurchaseCorrectionResource), Order = 27)]
        public string InvoiceNo { get; set; }

    }
}