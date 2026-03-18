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
    public class PrintInvoiceNekoposu
    {
        [Display(Name = nameof(PrintInvoiceResource.DeliShiwakeCd), ResourceType = typeof(PrintInvoiceResource), Order = 1)]
        public string DeliShiwakeBarcode { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliShiwakeCd), ResourceType = typeof(PrintInvoiceResource), Order = 2)]
        public string DeliShiwakeCd { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestZip), ResourceType = typeof(PrintInvoiceResource), Order = 3)]
        public string DestZip { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 4)]
        public string DestAddress1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress2), ResourceType = typeof(PrintInvoiceResource), Order = 5)]
        public string DestAddress2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress3), ResourceType = typeof(PrintInvoiceResource), Order = 6)]
        public string DestAddress3 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestBumon1), ResourceType = typeof(PrintInvoiceResource), Order = 7)]
        public string DestBumon1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestBumon2), ResourceType = typeof(PrintInvoiceResource), Order = 8)]
        public string DestBumon2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestName), ResourceType = typeof(PrintInvoiceResource), Order = 9)]
        public string DestName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliNo), ResourceType = typeof(PrintInvoiceResource), Order = 10)]
        public string DeliNo { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.PrintDate), ResourceType = typeof(PrintInvoiceResource), Order = 11)]
        public string PrintDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorZip), ResourceType = typeof(PrintInvoiceResource), Order = 12)]
        public string ConsignorZip { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 13)]
        public string ConsignorAddress1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress2), ResourceType = typeof(PrintInvoiceResource), Order = 14)]
        public string ConsignorAddress2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress2), ResourceType = typeof(PrintInvoiceResource), Order = 15)]
        public string ConsignorAddress3 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorName), ResourceType = typeof(PrintInvoiceResource), Order = 16)]
        public string ConsignorName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ItemName), ResourceType = typeof(PrintInvoiceResource), Order = 17)]
        public string ItemName1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ItemName), ResourceType = typeof(PrintInvoiceResource), Order = 18)]
        public string ItemName2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.Handling1), ResourceType = typeof(PrintInvoiceResource), Order = 19)]
        public string Handling1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.Handling2), ResourceType = typeof(PrintInvoiceResource), Order = 20)]
        public string Handling2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.Article), ResourceType = typeof(PrintInvoiceResource), Order = 21)]
        public string Article { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipStoreCd), ResourceType = typeof(PrintInvoiceResource), Order = 22)]
        public string ShipStoreCd { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliUseDate), ResourceType = typeof(PrintInvoiceResource), Order = 23)]
        public string DeliUseDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliVeryCompanyTel), ResourceType = typeof(PrintInvoiceResource), Order = 24)]
        public string DeliVeryCompanyTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliNo), ResourceType = typeof(PrintInvoiceResource), Order = 25)]
        public string DeliNoBarcode { get; set; }

    }
}