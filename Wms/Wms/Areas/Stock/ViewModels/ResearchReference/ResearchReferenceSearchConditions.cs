using Share.Common.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Wms.Areas.Stock.Resources;
using Wms.Common;

namespace Wms.Areas.Stock.ViewModels.ResearchReference
{
    public class ResearchReferenceSearchConditions
    {
        public enum ResearchReferenceStatus
        {
            [Display(Name = nameof(ResearchReferenceResource.StatusNot), ResourceType = typeof(ResearchReferenceResource))]
            Not = 0,

            [Display(Name = nameof(ResearchReferenceResource.StatusDuring), ResourceType = typeof(ResearchReferenceResource))]
            During = 1,

            [Display(Name = nameof(ResearchReferenceResource.StatusComplete), ResourceType = typeof(ResearchReferenceResource))]
            Complete = 9
        }

        public enum ResearchReferenceRegistClass
        {
            [Display(Name = nameof(ResearchReferenceResource.RegistClassShippingPicking), ResourceType = typeof(ResearchReferenceResource))]
            ShippingPicking = 1,

            [Display(Name = nameof(ResearchReferenceResource.RegistClassShippingStore), ResourceType = typeof(ResearchReferenceResource))]
            ShippingStore = 2,

            [Display(Name = nameof(ResearchReferenceResource.RegistClassShippingSingle), ResourceType = typeof(ResearchReferenceResource))]
            ShippingSingle = 3,

            [Display(Name = nameof(ResearchReferenceResource.RegistClassGasShortage), ResourceType = typeof(ResearchReferenceResource))]
            GasShortage = 4,

            [Display(Name = nameof(ResearchReferenceResource.RegistClassShippingInspection), ResourceType = typeof(ResearchReferenceResource))]
            ShippingInspection = 5,

            [Display(Name = nameof(ResearchReferenceResource.RegistClassPurchaseArrivalResult), ResourceType = typeof(ResearchReferenceResource))]
            PurchaseArrivalResult = 6,

            [Display(Name = nameof(ResearchReferenceResource.RegistClassMoveArrivalResult), ResourceType = typeof(ResearchReferenceResource))]
            MoveArrivalResult = 7,

            [Display(Name = nameof(ResearchReferenceResource.RegistClassStorePicking), ResourceType = typeof(ResearchReferenceResource))]
            StorePicking = 8
        }

        public enum ResearchReferenceSortOrder
        {
            [Display(Name = nameof(ResearchReferenceResource.ThSlipNo), ResourceType = typeof(ResearchReferenceResource))]
            SlipNo,

            [Display(Name = nameof(ResearchReferenceResource.SortOrderDateLocationSkuGrade), ResourceType = typeof(ResearchReferenceResource))]
            DateLocationSkuGrade,

            [Display(Name = nameof(ResearchReferenceResource.SortOrderDateSkuLocationGrade), ResourceType = typeof(ResearchReferenceResource))]
            DateSkuLocationGrade,

            [Display(Name = nameof(ResearchReferenceResource.SortOrderSkuLocationGrade), ResourceType = typeof(ResearchReferenceResource))]
            SkuLocationGrade
        }

        public enum ResearchReferenceAscDescSort
        {
            [Display(Name = nameof(FormsResource.ASC), ResourceType = typeof(FormsResource))]
            Asc,

            [Display(Name = nameof(FormsResource.DESC), ResourceType = typeof(FormsResource))]
            Desc
        }

        public ResearchReferenceSearchConditions()
        {
            CenterId = Profile.User.CenterId;
            Status = null;
            OccurredDateFrom = DateTime.Today;
            OccurredDateTo = DateTime.Today;
            RegistClass = null;
            ClearPageInformation();
            ShippingStoreId = string.Empty;
        }

        public string SelectedUniqueKey { get; set; }

        public string CenterId { get; set; }

        public ResearchReferenceStatus? Status { get; set; }

        public DateTime? OccurredDateFrom { get; set; }

        public string OccurredTimeFrom { get; set; }

        public DateTime? OccurredDateTo { get; set; }

        public string OccurredTimeTo { get; set; }

        public ResearchReferenceRegistClass? RegistClass { get; set; }

        public string LocationCd { get; set; }

        public string ShippingStoreId { get; set; }

        public string ShippingStoreName { get; set; }

        public string Jan { get; set; }

        public string Sku { get; set; }

        public string GradeId { get; set; }

        public string RegistUserId { get; set; }

        public string BatchNo { get; set; }

        public string InvoiceNo { get; set; }

        public string BoxNo { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ResearchReferenceAscDescSort AscDescSort { get; set; }

        public ResearchReferenceSortOrder SortOrder { get; set; }

        public int GetOffset()
        {
            return PageNumber <= 0 ? 0 : (PageNumber - 1) * PageSize;
        }

        public bool ClearPageInformation()
        {
            return SetPageInformation(1, ResearchReferenceSortOrder.DateLocationSkuGrade, ResearchReferenceAscDescSort.Asc);
        }

        public bool SetPageInformation(int pageNumber, ResearchReferenceSortOrder sortOrder, ResearchReferenceAscDescSort ascDesc)
        {
            PageNumber = pageNumber;
            SortOrder = sortOrder;
            AscDescSort = ascDesc;

            return true;
        }

        public DateTime? GetOccurredDateTimeFrom()
        {
            return ToDateTime(OccurredDateFrom, OccurredTimeFrom, "00:00:00");
        }

        public DateTime? GetGetOccurredDateTimeTo()
        {
            return ToDateTime(OccurredDateTo, OccurredTimeTo, "23:59:59");
        }

        public int? GetStatusInt()
        {
            return (int?)Status;
        }

        public int? GetRegistClassInt()
        {
            return (int?)RegistClass;
        }

        private static DateTime? ToDateTime(DateTime? date, string time, string emptyTime)
        {
            if (date == null)
            {
                return null;
            }

            return Convert.ToDateTime(string.Format("{0} {1}", date?.ToString("yyyy/MM/dd"), string.IsNullOrEmpty(time) ? emptyTime : time));
        }

        public bool StatusNot { get; set; } = true;

        public bool StatusDuring { get; set; } = true;

        public bool StatusComplete { get; set; }
    }
}