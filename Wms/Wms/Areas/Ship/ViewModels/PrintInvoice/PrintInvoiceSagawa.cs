namespace Wms.Areas.Ship.ViewModels.PrintInvoice
{
    using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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
    public class PrintInvoiceSagawa
    {
        [Display(Name = nameof(PrintInvoiceResource.DeliShiwakeCd), ResourceType = typeof(PrintInvoiceResource), Order = 1)]
        public string DeliShiwakeCd { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.OfficeCd), ResourceType = typeof(PrintInvoiceResource), Order = 2)]
        public string OfficeCd { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.LocalCd), ResourceType = typeof(PrintInvoiceResource), Order = 3)]
        public string LocalCd { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.PrintDate), ResourceType = typeof(PrintInvoiceResource), Order = 4)]
        public DateTime PrintDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.Unit), ResourceType = typeof(PrintInvoiceResource), Order = 5)]
        public int UnitCnt { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.BinType), ResourceType = typeof(PrintInvoiceResource), Order = 6)]
        public string BinType { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ArriveRequestDate), ResourceType = typeof(PrintInvoiceResource), Order = 7)]
        public DateTime? ArriveRequestDate { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ArriveRequestTime), ResourceType = typeof(PrintInvoiceResource), Order = 8)]
        public string ArriveRequestTime { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ArriveRequestTimeName), ResourceType = typeof(PrintInvoiceResource), Order = 9)]
        public string ArriveRequestTimeName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestZip), ResourceType = typeof(PrintInvoiceResource), Order = 10)]
        public string ShipToZip { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestTel), ResourceType = typeof(PrintInvoiceResource), Order = 11)]
        public string ShipToTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestPrefName), ResourceType = typeof(PrintInvoiceResource), Order = 12)]
        public string ShipToPrefName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestCityName), ResourceType = typeof(PrintInvoiceResource), Order = 13)]
        public string ShipToCityName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 14)]
        public string ShipToAddress1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress2), ResourceType = typeof(PrintInvoiceResource), Order = 15)]
        public string ShipToAddress2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress3), ResourceType = typeof(PrintInvoiceResource), Order = 16)]
        public string ShipToAddress3 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestAddress4), ResourceType = typeof(PrintInvoiceResource), Order = 17)]
        public string ShipToAddress4 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestBumon1), ResourceType = typeof(PrintInvoiceResource), Order = 18)]
        public string ShipToCompany { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestBumon2), ResourceType = typeof(PrintInvoiceResource), Order = 19)]
        public string ShipToDivision { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DestName), ResourceType = typeof(PrintInvoiceResource), Order = 20)]
        public string ShipToStoreName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorPrefName), ResourceType = typeof(PrintInvoiceResource), Order = 21)]
        public string ConsignorPrefName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorCityName), ResourceType = typeof(PrintInvoiceResource), Order = 22)]
        public string ConsignorCityName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress1), ResourceType = typeof(PrintInvoiceResource), Order = 23)]
        public string ConsignorAddress1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress2), ResourceType = typeof(PrintInvoiceResource), Order = 24)]
        public string ConsignorAddress2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress3), ResourceType = typeof(PrintInvoiceResource), Order = 25)]
        public string ConsignorAddress3 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorAddress4), ResourceType = typeof(PrintInvoiceResource), Order = 26)]
        public string ConsignorAddress4 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorName), ResourceType = typeof(PrintInvoiceResource), Order = 27)]
        public string ConsignorName1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorName2), ResourceType = typeof(PrintInvoiceResource), Order = 28)]
        public string ConsignorName2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ConsignorTel), ResourceType = typeof(PrintInvoiceResource), Order = 29)]
        public string ConsignorTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ClientCd), ResourceType = typeof(PrintInvoiceResource), Order = 30)]
        public string ClientCd { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DeliNo), ResourceType = typeof(PrintInvoiceResource), Order = 31)]
        public string DeliNo { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.SaleOfficeName), ResourceType = typeof(PrintInvoiceResource), Order = 32)]
        public string SalesOfficeName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.SaleOfficeTel), ResourceType = typeof(PrintInvoiceResource), Order = 33)]
        public string SalesOfficeTel { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.SaleOfficeFax), ResourceType = typeof(PrintInvoiceResource), Order = 34)]
        public string SalesOfficeFax { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.PriintCopies), ResourceType = typeof(PrintInvoiceResource), Order = 35)]
        public int PrintCopies { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.DaibikiFlag), ResourceType = typeof(PrintInvoiceResource), Order = 36)]
        public int DaibikiFlag { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.PackingType), ResourceType = typeof(PrintInvoiceResource), Order = 37)]
        public string PackingType { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName1), ResourceType = typeof(PrintInvoiceResource), Order = 38)]
        public string GoodsName1 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName2), ResourceType = typeof(PrintInvoiceResource), Order = 39)]
        public string GoodsName2 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName3), ResourceType = typeof(PrintInvoiceResource), Order = 40)]
        public string GoodsName3 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName4), ResourceType = typeof(PrintInvoiceResource), Order = 41)]
        public string GoodsName4 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.GoodsName5), ResourceType = typeof(PrintInvoiceResource), Order = 42)]
        public string GoodsName5 { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.SystemName), ResourceType = typeof(PrintInvoiceResource), Order = 43)]
        public string SystemName { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ClassCd), ResourceType = typeof(PrintInvoiceResource), Order = 44)]
        public string ClassCd { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.ClaimAmount), ResourceType = typeof(PrintInvoiceResource), Order = 45)]
        public int? ClaimAmount { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.TaxAmount), ResourceType = typeof(PrintInvoiceResource), Order = 46)]
        public int? TaxAmount { get; set; }

        [Display(Name = nameof(PrintInvoiceResource.InsuranceAmount), ResourceType = typeof(PrintInvoiceResource), Order = 47)]
        public int? InsuranceAmount { get; set; }

    }
}