using Share.Common.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using Wms.Areas.Stock.Resources;
using Wms.Common;

namespace Wms.Areas.Stock.ViewModels.AdjustReference
{
    public class AdjustReferenceSearchConditions
    {
        public enum AdjustReferenceSortOrder
        {
            [Display(Name = nameof(AdjustReferenceResource.SortOrderDateNumberSku), ResourceType = typeof(AdjustReferenceResource))]
            DateNumberSku,

            [Display(Name = nameof(AdjustReferenceResource.SortOrderDateSkuNumber), ResourceType = typeof(AdjustReferenceResource))]
            DateSkuNumber,

            [Display(Name = nameof(AdjustReferenceResource.SortOrderSkuDateNumber), ResourceType = typeof(AdjustReferenceResource))]
            SkuDateNumber
        }

        public enum AdjustReferenceAscDescSort
        {
            [Display(Name = nameof(FormsResource.ASC), ResourceType = typeof(FormsResource))]
            Asc,

            [Display(Name = nameof(FormsResource.DESC), ResourceType = typeof(FormsResource))]
            Desc
        }

        public AdjustReferenceSearchConditions()
        {
            CenterId = Profile.User.CenterId;
            StockAdjustDateFrom = DateTime.Today;
            StockAdjustDateTo = DateTime.Today;
            ClearPageInformation();
        }

        public string CenterId { get; set; }

        public string AdjustReasonCd { get; set; }

        public DateTime? StockAdjustDateFrom { get; set; }

        public DateTime? StockAdjustDateTo { get; set; }

        public string StockAdjustNumberFrom { get; set; }

        public string StockAdjustNumberTo { get; set; }

        public string Jan { get; set; }

        public string ItemCode { get; set; }

        public string ItemId { get; set; }

        public string ColorId { get; set; }

        public string ColorName { get; set; }

        public string SizeId { get; set; }

        public string SizeName { get; set; }

        public string Sku { get; set; }

        public string DivisionId { get; set; }

        public string BrandId { get; set; }

        public string BrandName { get; set; }

        public string VendorId { get; set; }

        public string VendorName { get; set; }

        public string CategoryId1 { get; set; }

        public string CategoryId2 { get; set; }

        public string CategoryId3 { get; set; }

        public string CategoryId4 { get; set; }

        public string LocationCdFrom { get; set; }

        public string LocationCdTo { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public AdjustReferenceSortOrder SortOrder { get; set; }

        public AdjustReferenceAscDescSort AscDescSort { get; set; }

        public int GetOffset()
        {
            return PageNumber <= 0 ? 0 : (PageNumber - 1) * PageSize;
        }

        public bool ClearPageInformation()
        {
            return SetPageInformation(1, AdjustReferenceSortOrder.DateNumberSku, AdjustReferenceAscDescSort.Asc);
        }

        public bool SetPageInformation(int pageNumber, AdjustReferenceSortOrder sortOrder, AdjustReferenceAscDescSort ascDesc)
        {
            PageNumber = pageNumber;
            SortOrder = sortOrder;
            AscDescSort = ascDesc;

            return true;
        }
    }
}