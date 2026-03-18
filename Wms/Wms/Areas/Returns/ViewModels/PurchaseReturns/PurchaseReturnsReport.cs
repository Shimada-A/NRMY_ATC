namespace Wms.Areas.Returns.ViewModels.PurchaseReturns
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PurchaseReturnsReport
    {

        /// <summary>
        /// 仕入先
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.Vendor), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 1)]
        public string VendorName { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.PrintUser), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 2)]
        public string PrintUser { get; set; }

        /// <summary>
        /// 返品日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.ReturnDate), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 3)]
        public string ArriveDate { get; set; }

        /// <summary>
        /// 返品伝票ID
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.ReturnId), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 4)]
        public string ReturnId { get; set; }

        /// <summary>
        /// 仕入先郵便番号
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.CenterZip), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 5)]
        public string CenterZip { get; set; }

        /// <summary>
        /// 仕入先住所
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.CenterAddress), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 6)]
        public string CenterAddress { get; set; }

        /// <summary>
        /// 仕入先電話番号
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.CenterTel), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 7)]
        public string CenterTel { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.CenterName), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 8)]
        public string CenterName { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.CategoryName1), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 9)]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.CategoryName1), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 10)]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.Item), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 11)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.Item), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 12)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.ItemColor), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 13)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.ItemColor), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 14)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.ItemSize), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 15)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.ItemSize), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 16)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.Jan), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 17)]
        public string Jan1 { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.Jan), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 18)]
        public string Jan2 { get; set; }

        /// <summary>
        /// 下代(画面入力値)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.BuyingPrice), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 19)]
        public int? NormalBuyingPrice { get; set; }

        /// <summary>
        /// 返品数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReturnsResource.ReturnQty), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 20)]
        public int? ReturnQty { get; set; }

        [Display(Name = nameof(Resources.PurchaseReturnsResource.PrintDate), ResourceType = typeof(Resources.PurchaseReturnsResource), Order = 21)]
        public string PrintDate { get; set; }

    }
}