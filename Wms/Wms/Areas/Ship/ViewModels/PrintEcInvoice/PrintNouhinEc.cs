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
    public class PrintNouhinEc
    {
        [Display(Name = nameof(PrintEcInvoiceResource.DestName), ResourceType = typeof(PrintEcInvoiceResource), Order = 1)]
        public string DestName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.OrderNo), ResourceType = typeof(PrintEcInvoiceResource), Order = 2)]
        public string OrderNo { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.OrderDate), ResourceType = typeof(PrintEcInvoiceResource), Order = 3)]
        public string OrderDate { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.PaymentMethod), ResourceType = typeof(PrintEcInvoiceResource), Order = 4)]
        public string PaymentMethod { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageStoreName), ResourceType = typeof(PrintEcInvoiceResource), Order = 5)]
        public string EcOkaiageStoreName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ShipDate), ResourceType = typeof(PrintEcInvoiceResource), Order = 6)]
        public string ShipDate { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.SumOrderQty), ResourceType = typeof(PrintEcInvoiceResource), Order = 7)]
        public string SumOrderQty { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.DestZip), ResourceType = typeof(PrintEcInvoiceResource), Order = 8)]
        public string DestZip { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.DestAddress), ResourceType = typeof(PrintEcInvoiceResource), Order = 9)]
        public string DestAddress { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.DestTel), ResourceType = typeof(PrintEcInvoiceResource), Order = 10)]
        public string DestTel { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ArriveRequestDate), ResourceType = typeof(PrintEcInvoiceResource), Order = 11)]
        public string ArriveRequestDate { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageStoreName1), ResourceType = typeof(PrintEcInvoiceResource), Order = 12)]
        public string EcOkaiageStoreName1 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageStoreName2), ResourceType = typeof(PrintEcInvoiceResource), Order = 13)]
        public string EcOkaiageStoreName2 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageShipper1), ResourceType = typeof(PrintEcInvoiceResource), Order = 14)]
        public string EcOkaiageShipper1 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageShipper2), ResourceType = typeof(PrintEcInvoiceResource), Order = 15)]
        public string EcOkaiageShipper2 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageShipper3), ResourceType = typeof(PrintEcInvoiceResource), Order = 16)]
        public string EcOkaiageShipper3 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ItemId), ResourceType = typeof(PrintEcInvoiceResource), Order = 17)]
        public string ItemId { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ColorName), ResourceType = typeof(PrintEcInvoiceResource), Order = 18)]
        public string ColorName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.SizeName), ResourceType = typeof(PrintEcInvoiceResource), Order = 19)]
        public string SizeName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ItemName), ResourceType = typeof(PrintEcInvoiceResource), Order = 20)]
        public string ItemName { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.Jan), ResourceType = typeof(PrintEcInvoiceResource), Order = 21)]
        public string Jan { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.Tanka), ResourceType = typeof(PrintEcInvoiceResource), Order = 22)]
        public string Tanka { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.OrderQty), ResourceType = typeof(PrintEcInvoiceResource), Order = 23)]
        public string OrderQty { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.ItemAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 24)]
        public string ItemAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.TotalItemAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 25)]
        public string TotalItemAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.CarriageAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 26)]
        public string CarriageAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.CommissionAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 27)]
        public string CommissionAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.PointDiscountAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 28)]
        public string PointDiscountAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.TotalClaimAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 29)]
        public string TotalClaimAmount { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.OrderBarcode), ResourceType = typeof(PrintEcInvoiceResource), Order = 31)]
        public string OrderBarcode { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage11), ResourceType = typeof(PrintEcInvoiceResource), Order = 32)]
        public string EcOkaiageMessage11 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage12), ResourceType = typeof(PrintEcInvoiceResource), Order = 33)]
        public string EcOkaiageMessage12 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage13), ResourceType = typeof(PrintEcInvoiceResource), Order = 34)]
        public string EcOkaiageMessage13 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage21), ResourceType = typeof(PrintEcInvoiceResource), Order = 35)]
        public string EcOkaiageMessage21 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage22), ResourceType = typeof(PrintEcInvoiceResource), Order = 36)]
        public string EcOkaiageMessage22 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage23), ResourceType = typeof(PrintEcInvoiceResource), Order = 37)]
        public string EcOkaiageMessage23 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage24), ResourceType = typeof(PrintEcInvoiceResource), Order = 38)]
        public string EcOkaiageMessage24 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage25), ResourceType = typeof(PrintEcInvoiceResource), Order = 39)]
        public string EcOkaiageMessage25 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage26), ResourceType = typeof(PrintEcInvoiceResource), Order = 40)]
        public string EcOkaiageMessage26 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage27), ResourceType = typeof(PrintEcInvoiceResource), Order = 41)]
        public string EcOkaiageMessage27 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage28), ResourceType = typeof(PrintEcInvoiceResource), Order = 42)]
        public string EcOkaiageMessage28 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage29), ResourceType = typeof(PrintEcInvoiceResource), Order = 43)]
        public string EcOkaiageMessage29 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage210), ResourceType = typeof(PrintEcInvoiceResource), Order = 44)]
        public string EcOkaiageMessage210 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage211), ResourceType = typeof(PrintEcInvoiceResource), Order = 45)]
        public string EcOkaiageMessage211 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage212), ResourceType = typeof(PrintEcInvoiceResource), Order = 46)]
        public string EcOkaiageMessage212 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage213), ResourceType = typeof(PrintEcInvoiceResource), Order = 47)]
        public string EcOkaiageMessage213 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage214), ResourceType = typeof(PrintEcInvoiceResource), Order = 48)]
        public string EcOkaiageMessage214 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage215), ResourceType = typeof(PrintEcInvoiceResource), Order = 49)]
        public string EcOkaiageMessage215 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage216), ResourceType = typeof(PrintEcInvoiceResource), Order = 50)]
        public string EcOkaiageMessage216 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage217), ResourceType = typeof(PrintEcInvoiceResource), Order = 51)]
        public string EcOkaiageMessage217 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage218), ResourceType = typeof(PrintEcInvoiceResource), Order = 52)]
        public string EcOkaiageMessage218 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage219), ResourceType = typeof(PrintEcInvoiceResource), Order = 53)]
        public string EcOkaiageMessage219 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage220), ResourceType = typeof(PrintEcInvoiceResource), Order = 54)]
        public string EcOkaiageMessage220 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage221), ResourceType = typeof(PrintEcInvoiceResource), Order = 55)]
        public string EcOkaiageMessage221 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage222), ResourceType = typeof(PrintEcInvoiceResource), Order = 56)]
        public string EcOkaiageMessage222 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage223), ResourceType = typeof(PrintEcInvoiceResource), Order = 57)]
        public string EcOkaiageMessage223 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage224), ResourceType = typeof(PrintEcInvoiceResource), Order = 58)]
        public string EcOkaiageMessage224 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage225), ResourceType = typeof(PrintEcInvoiceResource), Order = 59)]
        public string EcOkaiageMessage225 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage226), ResourceType = typeof(PrintEcInvoiceResource), Order = 60)]
        public string EcOkaiageMessage226 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage227), ResourceType = typeof(PrintEcInvoiceResource), Order = 61)]
        public string EcOkaiageMessage227 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage31), ResourceType = typeof(PrintEcInvoiceResource), Order = 62)]
        public string EcOkaiageMessage31 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.EcOkaiageMessage32), ResourceType = typeof(PrintEcInvoiceResource), Order = 63)]
        public string EcOkaiageMessage32 { get; set; }

        [Display(Name = nameof(PrintEcInvoiceResource.CampaignDiscountAmount), ResourceType = typeof(PrintEcInvoiceResource), Order = 64)]
        public string CampaignDiscountAmount { get; set; }

        
    }
}