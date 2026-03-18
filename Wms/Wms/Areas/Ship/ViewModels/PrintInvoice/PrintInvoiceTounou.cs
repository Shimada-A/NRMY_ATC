namespace Wms.Areas.Ship.ViewModels.PrintInvoice
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PrintInvoiceTounou
    {

        [Display(Name = nameof(PrintInvoiceResource.DestName), ResourceType = typeof(PrintInvoiceResource), Order = 1)]
        public string StoreName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 2)]
        public string StoreAddress { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestTel), ResourceType = typeof(PrintInvoiceResource), Order = 3)]
        public string StoreTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipToStoreId), ResourceType = typeof(PrintInvoiceResource), Order = 4)]
        public string ShipToStoreId { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 5)]
        public string ConsignorAddress { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorName), ResourceType = typeof(PrintInvoiceResource), Order = 6)]
        public string ConsignorName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorTel), ResourceType = typeof(PrintInvoiceResource), Order = 7)]
        public string ConsignorTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipDate), ResourceType = typeof(PrintInvoiceResource), Order = 8)]
        public string ShipDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.NouhinMonth), ResourceType = typeof(PrintInvoiceResource), Order = 9)]
        public string NouhinMonth { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.NouhinDate), ResourceType = typeof(PrintInvoiceResource), Order = 10)]
        public string NouhinDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.UnitBarcode), ResourceType = typeof(PrintInvoiceResource), Order = 11)]
        public string UnitBarcode { get; set; }

    }
}