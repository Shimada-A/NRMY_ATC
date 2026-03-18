namespace Wms.Areas.Arrival.ViewModels.InputPurchase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class InputPurchase01SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum InputPurchase01Sort : byte
        {
            [Display(Name = nameof(Resources.InputPurchaseResource.ArrivePlanDateVendorIdItemId), ResourceType = typeof(Resources.InputPurchaseResource))]
            ArrivePlanDateVendorIdItemId,

            [Display(Name = nameof(Resources.InputPurchaseResource.ArrivePlanDateVendorIdInvoiceNo), ResourceType = typeof(Resources.InputPurchaseResource))]
            ArrivePlanDateVendorIdInvoiceNo,

            [Display(Name = nameof(Resources.InputPurchaseResource.VendorNameInvoiceNo), ResourceType = typeof(Resources.InputPurchaseResource))]
            VendorNameInvoiceNo,

            [Display(Name = nameof(Resources.InputPurchaseResource.ItemIdInvoiceNo), ResourceType = typeof(Resources.InputPurchaseResource))]
            ItemIdInvoiceNo
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 入荷予定日From
        /// </summary>
        public DateTime? ArrivePlanDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 入荷予定日To
        /// </summary>
        public DateTime? ArrivePlanDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        public string ArrivalStatus { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注ID
        /// </summary>
        public string PoId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; } = 0;

        /// <summary>
        /// Sort key
        /// </summary>
        public InputPurchase01Sort SortKey { get; set; } = InputPurchase01Sort.ArrivePlanDateVendorIdItemId;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQtySum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQtySum { get; set; }

        /// <summary>
        /// 実績数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQtySum { get; set; }

        /// <summary>
        /// 予定伝票数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InvoicePlanQtySum { get; set; }

        /// <summary>
        /// 実績伝票数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InvoiceResultQtySum { get; set; }
    }
}