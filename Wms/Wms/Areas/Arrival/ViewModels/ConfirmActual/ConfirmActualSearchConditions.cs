namespace Wms.Areas.Arrival.ViewModels.ConfirmActual
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class ConfirmActualSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum ArrivalSortKey : byte
        {
            [Display(Name = nameof(ConfirmActualResource.ArrivePlanDateVendorIdInvoiceNo), ResourceType = typeof(ConfirmActualResource))]
            ArrivePlanDateVendorIdInvoiceNo,
            [Display(Name = nameof(ConfirmActualResource.VendorIdInvoiceNoArrivePlanDate), ResourceType = typeof(ConfirmActualResource))]
            VendorIdInvoiceNoArrivePlanDate,
            [Display(Name = nameof(ConfirmActualResource.ArrivePlanDateItemId), ResourceType = typeof(ConfirmActualResource))]
            ArrivePlanDateItemId,
            [Display(Name = nameof(ConfirmActualResource.ItemIdArrivePlanDate), ResourceType = typeof(ConfirmActualResource))]
            ItemIdArrivePlanDate
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(FormsResource.ASC), ResourceType = typeof(FormsResource))]
            Asc,
            [Display(Name = nameof(FormsResource.DESC), ResourceType = typeof(FormsResource))]
            Desc
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Profile.User.CenterId;

        /// <summary>
        /// 入荷予定日From
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArrivePlanDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 入荷予定日To
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArrivePlanDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 入荷日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public string ArriveDate { get; set; } = ConfirmActualResource.None;

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionCd { get; set; }

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
        /// 発注番号
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
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public ArrivalSortKey SortKey { get; set; } = ArrivalSortKey.ArrivePlanDateVendorIdInvoiceNo;

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
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// 選択中数
        /// </summary>
        public int? SelectedCnt { get; set; } = 0;

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuSum { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQtySum { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQtySum { get; set; }

        /// <summary>
        /// 予定伝票数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanSlipQtySum { get; set; }

        /// <summary>
        /// 実績伝票数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultSlipQtySum { get; set; }

        public IList<SelectedConfirmActualViewModel> ConfirmActuals { get; set; }

    }
}