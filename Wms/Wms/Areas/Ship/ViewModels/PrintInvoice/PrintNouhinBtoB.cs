namespace Wms.Areas.Ship.ViewModels.PrintInvoice
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
    public class PrintNouhinBtoB
    {
        [Display(Name = nameof(PrintInvoiceResource.NouhinNo), ResourceType = typeof(PrintInvoiceResource), Order = 1)]
        public string NouhinNo { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipToZip), ResourceType = typeof(PrintInvoiceResource), Order = 2)]
        public string ShipToZip { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipToAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 3)]
        public string ShipToAddress1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipToAddress2), ResourceType = typeof(PrintInvoiceResource), Order = 4)]
        public string ShipToAddress2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipToAddress3), ResourceType = typeof(PrintInvoiceResource), Order = 5)]
        public string ShipToAddress3 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipToStoreName), ResourceType = typeof(PrintInvoiceResource), Order = 6)]
        public string ShipToStoreName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipToStoreId), ResourceType = typeof(PrintInvoiceResource), Order = 7)]
        public string ShipToStoreId { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.KakuDate), ResourceType = typeof(PrintInvoiceResource), Order = 8)]
        public DateTime KakuDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliDate), ResourceType = typeof(PrintInvoiceResource), Order = 9)]
        public DateTime DeliDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.SaleBaseClassName), ResourceType = typeof(PrintInvoiceResource), Order = 10)]
        public string SaleBaseClassName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.CenterName), ResourceType = typeof(PrintInvoiceResource), Order = 11)]
        public string CenterName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.KakuUserName), ResourceType = typeof(PrintInvoiceResource), Order = 12)]
        public string KakuUserName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.SalesClassName), ResourceType = typeof(PrintInvoiceResource), Order = 13)]
        public string SalesClassName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.OffRate), ResourceType = typeof(PrintInvoiceResource), Order = 14)]
        public string OffRate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.CompanyName), ResourceType = typeof(PrintInvoiceResource), Order = 15)]
        public string CompanyName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.CompanyDivisionName), ResourceType = typeof(PrintInvoiceResource), Order = 16)]
        public string CompanyDivisionName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.CompanyAddress), ResourceType = typeof(PrintInvoiceResource), Order = 17)]
        public string CompanyAddress { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.CompanyTelFax), ResourceType = typeof(PrintInvoiceResource), Order = 18)]
        public string CompanyTelFax { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.CompanyCheckingAccount), ResourceType = typeof(PrintInvoiceResource), Order = 19)]
        public string CompanyCheckingAccount { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ItemId), ResourceType = typeof(PrintInvoiceResource), Order = 20)]
        public string ItemId { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ItemName), ResourceType = typeof(PrintInvoiceResource), Order = 21)]
        public string ItemName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ItemColorId), ResourceType = typeof(PrintInvoiceResource), Order = 22)]
        public string ItemColorId { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ItemColorName), ResourceType = typeof(PrintInvoiceResource), Order = 23)]
        public string ItemColorName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ItemSizeName), ResourceType = typeof(PrintInvoiceResource), Order = 24)]
        public string ItemSizeName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.SizeResultQty), ResourceType = typeof(PrintInvoiceResource), Order = 25)]
        public string SizeResultQty { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ColorResultQty), ResourceType = typeof(PrintInvoiceResource), Order = 26)]
        public int ColorResultQty { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.NormalSellingPrice), ResourceType = typeof(PrintInvoiceResource), Order = 27)]
        public int NormalSellingPrice { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ColorAmount), ResourceType = typeof(PrintInvoiceResource), Order = 28)]
        public int ColorAmount { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliveryNote), ResourceType = typeof(PrintInvoiceResource), Order = 29)]
        public string DeliveryNote { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.BatchName), ResourceType = typeof(PrintInvoiceResource), Order = 30)]
        public string BatchName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.TotalResultQty), ResourceType = typeof(PrintInvoiceResource), Order = 31)]
        public int TotalResultQty { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.TotalAmount), ResourceType = typeof(PrintInvoiceResource), Order = 32)]
        public int TotalAmount { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.TaxAmount), ResourceType = typeof(PrintInvoiceResource), Order = 33)]
        public int TaxAmount { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.Title), ResourceType = typeof(PrintInvoiceResource), Order = 34)]
        public string Title { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.BoxNo), ResourceType = typeof(PrintInvoiceResource), Order = 35)]
        public string BoxNo { get; set; }
    }
}