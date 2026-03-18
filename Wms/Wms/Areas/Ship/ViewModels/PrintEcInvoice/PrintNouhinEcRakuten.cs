namespace Wms.Areas.Ship.ViewModels.PrintEcInvoice
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary> 
    public class PrintNouhinEcRakuten
    {

        [Display(Name = nameof(PrintEcInvoiceResource.ClientName), ResourceType = typeof(PrintEcInvoiceResource), Order = 1)]
        public string ClientName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.DestName), ResourceType = typeof(PrintEcInvoiceResource), Order = 2)]
        public string DestName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageShipper1), ResourceType = typeof(PrintEcInvoiceResource), Order = 3)]
        public string EcRakutenOkaiageShipper1 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageShipper2), ResourceType = typeof(PrintEcInvoiceResource), Order = 4)]
        public string EcRakutenOkaiageShipper2 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageShipper3), ResourceType = typeof(PrintEcInvoiceResource), Order = 5)]
        public string EcRakutenOkaiageShipper3 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageShipper4), ResourceType = typeof(PrintEcInvoiceResource), Order = 6)]
        public string EcRakutenOkaiageShipper4 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageShipper5), ResourceType = typeof(PrintEcInvoiceResource), Order = 7)]
        public string EcRakutenOkaiageShipper5 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageShipper6), ResourceType = typeof(PrintEcInvoiceResource), Order = 8)]
        public string EcRakutenOkaiageShipper6 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageShipper7), ResourceType = typeof(PrintEcInvoiceResource), Order = 9)]
        public string EcRakutenOkaiageShipper7 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.OrderNo), ResourceType = typeof(PrintEcInvoiceResource), Order = 10)]
        public string OrderNo { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.OrderDate), ResourceType = typeof(PrintEcInvoiceResource), Order = 11)]
        public string OrderDate { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.PaymentMethod), ResourceType = typeof(PrintEcInvoiceResource), Order = 12)]
        public string PaymentMethod { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.PrintDate), ResourceType = typeof(PrintEcInvoiceResource), Order = 13)]
        public string PrintDate { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ItemId), ResourceType = typeof(PrintEcInvoiceResource), Order = 14)]
        public string ItemId { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ColorName), ResourceType = typeof(PrintEcInvoiceResource), Order = 15)]
        public string ColorName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.SizeName), ResourceType = typeof(PrintEcInvoiceResource), Order = 16)]
        public string SizeName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ItemName), ResourceType = typeof(PrintEcInvoiceResource), Order = 17)]
        public string ItemName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.Jan), ResourceType = typeof(PrintEcInvoiceResource), Order = 18)]
        public string Jan { get; set; }


        [Display(Name = nameof(PrintEcInvoiceResource.TaxRate), ResourceType = typeof(PrintEcInvoiceResource), Order = 19)]
        public string TaxRate { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.OrderQty), ResourceType = typeof(PrintEcInvoiceResource), Order = 20)]
        public string OrderQty { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.Tanka), ResourceType = typeof(PrintEcInvoiceResource), Order = 21)]
        public string Tanka { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ItemAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 22)]
        public string ItemAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.TotalItemAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 23)]
        public string TotalItemAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.TotalTax), ResourceType = typeof(PrintEcInvoiceResource), Order = 24)]
        public string TotalTax { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.CarriageAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 25)]
        public string CarriageAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.CashOnDeliveryAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 26)]
        public string CashOnDeliveryAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.PointDiscountAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 27)]
        public string PointDiscountAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.CouponDiscountAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 28)]
        public string CouponDiscountAmount { get; set; }

         [Display(Name = nameof(PrintEcInvoiceResource.TotalClaimAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 29)]
        public string TotalClaimAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.TotalTaxAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 30)]
        public string TotalTaxAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.TotalTaxAmountTitle), ResourceType = typeof(PrintEcInvoiceResource), Order = 31)]
        public string TotalTaxAmountTitle { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageMessage11), ResourceType = typeof(PrintEcInvoiceResource), Order = 32)]
        public string EcRakutenOkaiageMessage11 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageMessage12), ResourceType = typeof(PrintEcInvoiceResource), Order = 33)]
        public string EcRakutenOkaiageMessage12 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcRakutenOkaiageMessage13), ResourceType = typeof(PrintEcInvoiceResource), Order = 34)]
        public string EcRakutenOkaiageMessage13 { get; set; }

    }
}