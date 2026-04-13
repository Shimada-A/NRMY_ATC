namespace Wms.Areas.Arrival.ViewModels.UnshelvedReference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class UnshelvedReferenceSearchConditions
    {
        /// <summary>
        /// Data to sort 明細
        /// </summary>
        public enum ArrivalSortKeyEnum : byte
        {
            [Display(Name = nameof(Resources.UnshelvedReferenceResource.DateVendorIdSku), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
            DateVendorIdSku,
            [Display(Name = nameof(Resources.UnshelvedReferenceResource.BrandIdVendorIdSku), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
            BrandIdVendorIdSku,
            [Display(Name = nameof(Resources.UnshelvedReferenceResource.SkuBrandIdVendorId), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
            SkuBrandIdVendorId,
            //[Display(Name = nameof(Resources.UnshelvedReferenceResource.SkuVendorNameDate), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
            //SkuVendorNameDate
        }

        /// <summary>
        /// Data to sort ケース別
        /// </summary>
        public enum PackageArrivalSortKeyEnum : byte
        {
            [Display(Name = nameof(Resources.UnshelvedReferenceResource.DateVendorIdSkuCaseNo), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
            DateVendorIdSkuCaseNo,
            [Display(Name = nameof(Resources.UnshelvedReferenceResource.VendorIdDateSkuCaseNo), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
            VendorIdDateSkuCaseNo,
            [Display(Name = nameof(Resources.UnshelvedReferenceResource.SkuBrandIdVendorIdCaseNo), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
            SkuBrandIdVendorIdCaseNo
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
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 入荷日From
        /// </summary>
        public DateTime? ArrivalDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 入荷日To
        /// </summary>
        public DateTime? ArrivalDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionCd { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// アイテム
        /// </summary>
        public string ItemCode { get; set;}

        /// <summary>
        /// アイテム名
        /// </summary>
        public string ItemCodeName { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

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
        /// Sort key 明細
        /// </summary>
        public ArrivalSortKeyEnum ArrivalSortKey { get; set; } = ArrivalSortKeyEnum.DateVendorIdSku;

        /// <summary>
        /// Sort key ケース別
        /// </summary>
        public PackageArrivalSortKeyEnum PackageArrivalSortKey { get; set; } = PackageArrivalSortKeyEnum.DateVendorIdSkuCaseNo;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Result Type
        /// </summary>
        public ArrivalTypes ResultType { get; set; } = ArrivalTypes.Arrival;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int ItemSkuSum { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int CaseQtySum { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int StockQtySum { get; set; }

        /// <summary>
        /// バラ数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int TotalQtySum { get; set; }
    }
}