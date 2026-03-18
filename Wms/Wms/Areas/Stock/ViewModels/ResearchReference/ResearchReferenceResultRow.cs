using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using static Wms.Areas.Stock.ViewModels.ResearchReference.ResearchReferenceSearchConditions;

namespace Wms.Areas.Stock.ViewModels.ResearchReference
{
    public class ResearchReferenceResultRow
    {
        public int RowNumber { get; set; }

        public string CenterId { get; set; }

        public string SlipNo { get; set; }

        public DateTime? OccurredDateTime { get; set; }

        public ResearchReferenceRegistClass? RegistClass { get; set; }

        public string RegistUserId { get; set; }

        public string RegistUserName { get; set; }

        public string BatchNo { get; set; }

        public string GasBatchNo { get; set; }

        public string LocationCd { get; set; }

        public string FrontageNo { get; set; }

        public string BoxNo { get; set; }

        public string Sku { get; set; }

        public string Jan { get; set; }

        public string ItemId { get; set; }

        public string ColorId { get; set; }

        public string SizeId { get; set; }

        public string GradeId { get; set; }

        public int? DiffQuantity { get; set; }

        public string InvoiceNo { get; set; }

        public string ShippingStoreId { get; set; }

        public DateTime? ResearchDateTime { get; set; }

        public string ResearchUserId { get; set; }

        public string ResearchUserName { get; set; }

        public string ResearchLocationCd { get; set; }

        public string ResearchNote { get; set; }

        public string ItemName { get; set; }

        public string ColorName { get; set; }

        public string SizeName { get; set; }

        public string GradeName { get; set; }

        public string RegistClassName { get; set; }

        public string ListPrintFlagName { get; set; }

        public string Status { get; set; }

        public string StatusName { get; set; }

        public string ShippingStoreName { get; set; }

        public int? SlipSequence { get; set; }

        public string GetOccurredDateTimeString()
        {
            return OccurredDateTime?.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public string GetResearchDateTimeString()
        {
            if (ResearchDateTime == null)
            {
                return string.Empty;
            }

            return ResearchDateTime?.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public string GetBatchNoForDisplay()
        {
            return RegistClass == ResearchReferenceRegistClass.GasShortage ? GasBatchNo : BatchNo;
        }

        public string GetLocationUpper()
        {
            if (RegistClass == ResearchReferenceRegistClass.GasShortage)
            {
                return FrontageNo;
            }

            if (string.IsNullOrEmpty(ShippingStoreId))
            {
                return LocationCd;
            }

            return ShippingStoreId;
        }

        public string GetLocationLower()
        {
            if (RegistClass == ResearchReferenceRegistClass.GasShortage)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(ShippingStoreId))
            {
                return string.Empty;
            }

            return ShippingStoreName;
        }

        public string ToJson()
        {
            if (this != null)
            {
                return new JavaScriptSerializer().Serialize(this);
            }
            return string.Empty;
        }

        public string CreateUniqueKey()
        {
            return string.Format("{0}-{1}", SlipNo, SlipSequence);
        }

    }
}