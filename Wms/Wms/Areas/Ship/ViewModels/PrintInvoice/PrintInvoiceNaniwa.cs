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
    public class PrintInvoiceNaniwa
    {
        [Display(Name = nameof(PrintInvoiceResource.DestAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 1)]
        public string StoreAddress1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress2), ResourceType = typeof(PrintInvoiceResource), Order = 2)]
        public string StoreAddress2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestName2), ResourceType = typeof(PrintInvoiceResource), Order = 3)]
        public string ShipToStoreId { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestName), ResourceType = typeof(PrintInvoiceResource), Order = 4)]
        public string DivisionName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestName3), ResourceType = typeof(PrintInvoiceResource), Order = 5)]
        public string StoreName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestTel), ResourceType = typeof(PrintInvoiceResource), Order = 6)]
        public string StoreTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestDirectName), ResourceType = typeof(PrintInvoiceResource), Order = 7)]
        public string NaniwaDeliCenter { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 8)]
        public string ConsignorAddress1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress2), ResourceType = typeof(PrintInvoiceResource), Order = 9)]
        public string ConsignorAddress2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorName), ResourceType = typeof(PrintInvoiceResource), Order = 10)]
        public string ConsignorName1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorName2), ResourceType = typeof(PrintInvoiceResource), Order = 11)]
        public string ConsignorName2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorTel), ResourceType = typeof(PrintInvoiceResource), Order = 12)]
        public string ConsignorTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliNo2), ResourceType = typeof(PrintInvoiceResource), Order = 13)]
        public string DeliNo2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliNo), ResourceType = typeof(PrintInvoiceResource), Order = 14)]
        public string DeliNo { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipDate), ResourceType = typeof(PrintInvoiceResource), Order = 15)]
        public string PrintDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName1), ResourceType = typeof(PrintInvoiceResource), Order = 16)]
        public string GoodsName1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName2), ResourceType = typeof(PrintInvoiceResource), Order = 17)]
        public string GoodsName2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName3), ResourceType = typeof(PrintInvoiceResource), Order = 18)]
        public string GoodsName3 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName4), ResourceType = typeof(PrintInvoiceResource), Order = 19)]
        public string GoodsName4 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.TransactionForm), ResourceType = typeof(PrintInvoiceResource), Order = 20)]
        public string TransactionForm { get; set; }
    }
}