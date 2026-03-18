namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;

    public class PurchaseReferenceCsv
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
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.InvoiceNo), ResourceType = typeof(PurchaseReferenceResource), Order = 6)]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 事業部ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.DivisionId), ResourceType = typeof(PurchaseReferenceResource), Order = 7)]
        public string DivisionId { get; set; }

        /// <summary>
        /// 事業部名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.DivisionName), ResourceType = typeof(PurchaseReferenceResource), Order = 8)]
        public string DivisionName { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemId), ResourceType = typeof(PurchaseReferenceResource), Order = 9)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemName), ResourceType = typeof(PurchaseReferenceResource), Order = 10)]
        public string ItemName { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.BrandId), ResourceType = typeof(PurchaseReferenceResource), Order = 11)]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.BrandName1), ResourceType = typeof(PurchaseReferenceResource), Order = 12)]
        public string BrandName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemColorId), ResourceType = typeof(PurchaseReferenceResource), Order = 13)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemColorName), ResourceType = typeof(PurchaseReferenceResource), Order = 14)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemSizeId), ResourceType = typeof(PurchaseReferenceResource), Order = 15)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名称
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ItemSizeName), ResourceType = typeof(PurchaseReferenceResource), Order = 16)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        //[Display(Name = nameof(PurchaseReferenceResource.Jan), ResourceType = typeof(PurchaseReferenceResource), Order = 17)]
        //public string Jan1 { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        //[Display(Name = nameof(PurchaseReferenceResource.Jan), ResourceType = typeof(PurchaseReferenceResource), Order = 18)]
        //public string Jan2 { get; set; }

        /// <summary>
        /// 入荷予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ArrivePlanQty1), ResourceType = typeof(PurchaseReferenceResource), Order = 19)]
        public int? ArrivePlanQty { get; set; }

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
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.Jan), ResourceType = typeof(PurchaseReferenceResource), Order = 24)]
        public string Jan { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ConfirmDate), ResourceType = typeof(Resources.PurchaseReferenceResource), Order = 25)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 実績送信日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.IfRunDate), ResourceType = typeof(Resources.PurchaseReferenceResource), Order = 26)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? IfRunDate { get; set; }

        /// <summary>
        /// 入荷実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PurchaseReferenceResource.ResultQty), ResourceType = typeof(PurchaseReferenceResource), Order = 27)]
        public int? ArriveResultQty { get; set; }

        /// <summary>
        /// 品番連番
        /// </summary>
        [Display(Name = nameof(PurchaseReferenceResource.ItemSeq), ResourceType = typeof(PurchaseReferenceResource), Order = 28)]
        public int? ItemSeq { get; set; }

        /// <summary>
        /// Jan入りフラグ
        /// </summary>
        [Display(Name = nameof(PurchaseReferenceResource.JanFlag), ResourceType = typeof(PurchaseReferenceResource), Order = 29)]
        public int? JanFlag { get; set; }
    }
}