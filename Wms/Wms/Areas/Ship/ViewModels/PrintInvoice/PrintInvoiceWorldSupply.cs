using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wms.Areas.Ship.Resources;

namespace Wms.Areas.Ship.ViewModels.PrintInvoice
{
    public class PrintInvoiceWorldSupply
    {
        [Display(Name = nameof(PrintInvoiceResource.TransactionForm), ResourceType = typeof(PrintInvoiceResource), Order = 1)]
        public string DeliveryClass { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ShipDate), ResourceType = typeof(PrintInvoiceResource), Order = 2)]
        public string PrintDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestName), ResourceType = typeof(PrintInvoiceResource), Order = 3)]
        public string StoreName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestZip), ResourceType = typeof(PrintInvoiceResource), Order = 4)]
        public string StoreZip { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestTel), ResourceType = typeof(PrintInvoiceResource), Order = 5)]
        public string StoreTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 6)]
        public string StoreAddress { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.SlipCount), ResourceType = typeof(PrintInvoiceResource), Order = 7)]
        public string SlipCount {  get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorName), ResourceType = typeof(PrintInvoiceResource), Order = 8)]
        public string ConsignorName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorZip), ResourceType = typeof(PrintInvoiceResource), Order = 9)]
        public string ConsignorZip { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 10)]
        public string ConsignorAddress { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorTel), ResourceType = typeof(PrintInvoiceResource), Order = 11)]
        public string ConsignorTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliNo), ResourceType = typeof(PrintInvoiceResource), Order = 12)]
        public string DeliNo { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName1), ResourceType = typeof(PrintInvoiceResource), Order = 13)]
        public string Article1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName2), ResourceType = typeof(PrintInvoiceResource), Order = 14)]
        public string Article2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName3), ResourceType = typeof(PrintInvoiceResource), Order = 15)]
        public string Article3 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName4), ResourceType = typeof(PrintInvoiceResource), Order = 16)]
        public string Article4 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName5), ResourceType = typeof(PrintInvoiceResource), Order = 17)]
        public string Article5 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.TagClass), ResourceType = typeof(PrintInvoiceResource), Order = 18)]
        public string TagClass { get; set; }
    }
}