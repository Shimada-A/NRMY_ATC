namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;

    public class PurchaseReference02BoxNoReport
    {

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ArrivePlanDate), ResourceType = typeof(PurchaseReferenceResource), Order = 1)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.BrandId), ResourceType = typeof(PurchaseReferenceResource), Order = 2)]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.BrandName), ResourceType = typeof(PurchaseReferenceResource), Order = 3)]
        public string BrandName { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.Vendor), ResourceType = typeof(PurchaseReferenceResource), Order = 4)]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.Vendor), ResourceType = typeof(PurchaseReferenceResource), Order = 5)]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.InvoiceNo), ResourceType = typeof(PurchaseReferenceResource), Order = 6)]
        public string InvoiceNo { get; set; }

        ///// <summary>
        ///// 行番号
        ///// </summary>
        ///// <remarks>
        //[Display(Name = nameof(PurchaseReferenceResource.InvoiceSeq), ResourceType = typeof(PurchaseReferenceResource), Order = 5)]
        //public long InvoiceSeq { get; set; }

        /// <summary>
        /// 発注番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.PoId), ResourceType = typeof(PurchaseReferenceResource), Order = 7)]
        public string PoId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.CategoryName1), ResourceType = typeof(PurchaseReferenceResource), Order = 8)]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.Item), ResourceType = typeof(PurchaseReferenceResource), Order = 9)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.Item), ResourceType = typeof(PurchaseReferenceResource), Order = 10)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemColor), ResourceType = typeof(PurchaseReferenceResource), Order = 11)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemColor), ResourceType = typeof(PurchaseReferenceResource), Order = 12)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemSize), ResourceType = typeof(PurchaseReferenceResource), Order = 13)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemSize), ResourceType = typeof(PurchaseReferenceResource), Order = 14)]
        public string ItemSizeName { get; set; }

        ///// <summary>
        ///// JAN
        ///// </summary>
        ///// <remarks>
        //[Display(Name = nameof(PurchaseReferenceResource.Jan), ResourceType = typeof(PurchaseReferenceResource), Order = 14)]
        //public string Jan { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.BoxNo), ResourceType = typeof(PurchaseReferenceResource), Order = 15)]
        public string BoxNo { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ArrivePlanQty), ResourceType = typeof(PurchaseReferenceResource), Order = 16)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ResultQty), ResourceType = typeof(PurchaseReferenceResource), Order = 17)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.DifferenceQty), ResourceType = typeof(PurchaseReferenceResource), Order = 18)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ArrivalStatus), ResourceType = typeof(PurchaseReferenceResource), Order = 19)]
        public int? ArrivalStatus { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ArrivalStatusName), ResourceType = typeof(PurchaseReferenceResource), Order = 20)]
        public string ArrivalStatusName { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ConfirmDate), ResourceType = typeof(Resources.PurchaseReferenceResource), Order = 21)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 実績送信日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.IfRunDate), ResourceType = typeof(Resources.PurchaseReferenceResource), Order = 22)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? IfRunDate { get; set; }
    }
}