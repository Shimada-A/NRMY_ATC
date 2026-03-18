namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;

    public class PurchaseReferencePackingCsv
    {

        /// <summary>
        /// センターコード
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.CenterId), ResourceType = typeof(PurchaseReferenceResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// センター名称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.CenterName), ResourceType = typeof(PurchaseReferenceResource), Order = 2)]
        public string CenterName { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ArrivePlanDate), ResourceType = typeof(PurchaseReferenceResource), Order = 3)]
        public string ArrivePlanDate { get; set; }

        /// <summary>
        /// 仕入先ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.VendorId), ResourceType = typeof(PurchaseReferenceResource), Order = 4)]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.VendorName), ResourceType = typeof(PurchaseReferenceResource), Order = 5)]
        public string VendorName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.BoxNo), ResourceType = typeof(PurchaseReferenceResource), Order = 6)]
        public string BoxNo { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.InvoiceNo), ResourceType = typeof(PurchaseReferenceResource), Order = 7)]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.Jan), ResourceType = typeof(PurchaseReferenceResource), Order = 8)]
        public string Jan { get; set; }

        /// <summary>
        /// ITEM_SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemSkuId), ResourceType = typeof(PurchaseReferenceResource), Order = 11)]
        public string ItemSkuId { get; set; }


        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemName), ResourceType = typeof(PurchaseReferenceResource), Order = 12)]
        public string ItemName { get; set; }

        /// <summary>
        /// 梱包数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.PackingPlanQty), ResourceType = typeof(PurchaseReferenceResource), Order = 13)]
        public int? PackingPlanQty { get; set; }

        /// <summary>
        /// 発行者コード
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.PrintUser), ResourceType = typeof(PurchaseReferenceResource), Order = 22)]
        public string PrintUser { get; set; } = Common.Profile.User.UserId;

        /// <summary>
        /// 発行者名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.PrintUserName), ResourceType = typeof(PurchaseReferenceResource), Order = 23)]
        public string PrintUserName { get; set; } = Common.Profile.User.UserName;

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

        /// <summary>
        /// 表示用ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.DispBoxNo), ResourceType = typeof(Resources.PurchaseReferenceResource), Order = 22)]
        public string DispBoxNo { get; set; }
    }
}