namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;

    public class PurchaseReference01Report
    {

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ArrivePlanDate), ResourceType = typeof(PurchaseReferenceResource), Order = 1)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.BrandId), ResourceType = typeof(PurchaseReferenceResource), Order = 2)]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.BrandName), ResourceType = typeof(PurchaseReferenceResource), Order = 3)]
        public string BrandName { get; set; }

        /// <summary>
        /// 仕入先ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.Vendor), ResourceType = typeof(PurchaseReferenceResource), Order = 4)]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.Vendor), ResourceType = typeof(PurchaseReferenceResource), Order = 5)]
        public string VendorName { get; set; }

        /// <summary>
        /// 品番数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemQty), ResourceType = typeof(PurchaseReferenceResource), Order = 6)]
        public int? ItemQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemSkuQty), ResourceType = typeof(PurchaseReferenceResource), Order = 7)]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.CasePlanQty), ResourceType = typeof(PurchaseReferenceResource), Order = 8)]
        public int? CasePlanQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.CaseResultQty), ResourceType = typeof(PurchaseReferenceResource), Order = 9)]
        public int? CaseResultQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ArrivePlanQty), ResourceType = typeof(PurchaseReferenceResource), Order = 10)]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ResultQty), ResourceType = typeof(PurchaseReferenceResource), Order = 11)]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異(+)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.DifferencePlus), ResourceType = typeof(PurchaseReferenceResource), Order = 12)]
        public int? DifferencePlus { get; set; }

        /// <summary>
        /// 差異(-)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.DifferenceMinus), ResourceType = typeof(PurchaseReferenceResource), Order = 13)]
        public int? DifferenceMinus { get; set; }
    }
}