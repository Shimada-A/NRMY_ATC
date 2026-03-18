namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 仕入入荷進捗照会(仕入先別)
    /// </summary>
    public class PurchaseReference01ResultRow
    {
        #region プロパティ

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanDate), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]

        public DateTime? ArrivePlanDate { get; set; }

        public string strArrivePlanDate
        {
            get
            {
                return DateTime.TryParse(ArrivePlanDate.ToString(), out DateTime dt) ? dt.ToString("yyyy/MM/dd") : string.Empty;
            }
        }

        /// <summary>
        /// ブランド
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.BrandName), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.BrandName), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string BrandShortName { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.Vendor), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.Vendor), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 品番数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ItemQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ItemSkuQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.CasePlanQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CasePlanQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.CaseResultQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CaseResultQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ResultQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 残数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.RemainderQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? RemainderQty { get; set; }

        /// <summary>
        /// 差異(+)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.DifferencePlus), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferencePlus { get; set; }

        /// <summary>
        /// 差異(-)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.DifferenceMinus), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceMinus { get; set; }

        /// <summary>
        /// 予定伝票数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.InvoicePlanQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InvoicePlanQty { get; set; }

        /// <summary>
        /// 実績伝票数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.InvoiceResultQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InvoiceResultQty { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; }

        #endregion プロパティ
    }
}