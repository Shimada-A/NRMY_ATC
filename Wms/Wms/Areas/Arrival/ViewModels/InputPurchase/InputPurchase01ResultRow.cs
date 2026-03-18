namespace Wms.Areas.Arrival.ViewModels.InputPurchase
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 仕入入荷実績入力
    /// </summary>
    public class InputPurchase01ResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.ArrivePlanDate), ResourceType = typeof(Resources.InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.Vendor), ResourceType = typeof(Resources.InputPurchaseResource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.Vendor), ResourceType = typeof(Resources.InputPurchaseResource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.InvoiceNo), ResourceType = typeof(Resources.InputPurchaseResource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.PoId), ResourceType = typeof(Resources.InputPurchaseResource))]
        public string PoId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.CategoryName1), ResourceType = typeof(Resources.InputPurchaseResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.Item), ResourceType = typeof(Resources.InputPurchaseResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.Item), ResourceType = typeof(Resources.InputPurchaseResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.ItemSkuQty), ResourceType = typeof(Resources.InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.PackingPlanQty), ResourceType = typeof(Resources.InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PackingPlanQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.PackingResultQty), ResourceType = typeof(Resources.InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PackingResultQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.ArrivePlanQty), ResourceType = typeof(Resources.InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.ResultQty), ResourceType = typeof(Resources.InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.ArrivalStatus), ResourceType = typeof(Resources.InputPurchaseResource))]
        public string ArrivalStatus { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputPurchaseResource.ArrivalStatusName), ResourceType = typeof(Resources.InputPurchaseResource))]
        public string ArrivalStatusName { get; set; }

        public string CenterId { get; set; }

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