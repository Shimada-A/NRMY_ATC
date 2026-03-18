namespace Wms.Areas.Ship.ViewModels.PrintEcInvoice
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PrintEcInvoiceConditions
    {
        /// <summary>
        /// 発行区分
        /// </summary>
        public enum PrnClasses : byte
        {
            [Display(Name = nameof(Resources.PrintEcInvoiceResource.New), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
            New,

            [Display(Name = nameof(Resources.PrintEcInvoiceResource.ReNouhinPrn), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
            ReNouhinPrn,

            [Display(Name = nameof(Resources.PrintEcInvoiceResource.ReDeliPrn), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
            ReDeliPrn
        }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        public enum EcShipClasses : byte
        {
            [Display(Name = nameof(Resources.PrintEcInvoiceResource.Singles), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
            Singles,

            [Display(Name = nameof(Resources.PrintEcInvoiceResource.Order), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
            Orders,

            [Display(Name = nameof(Resources.PrintEcInvoiceResource.Gas), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
            Gases
        }

        /// <summary>
        /// ECサイト区分
        /// </summary>
        public enum EcClass : byte
        {

            [Display(Name = nameof(Resources.PrintEcInvoiceResource.Rakuten), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
            Rakuten = 7, 
            
            [Display(Name = nameof(Resources.PrintEcInvoiceResource.EcBeing), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
            EcBeing = 6,

            [Display(Name = nameof(Resources.PrintEcInvoiceResource.WaKsnap), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
            WaKsnap = 13

        }

        /// <summary>
        /// 権限レベル
        /// </summary>
        public PermissionLevelClasses PermissonLavel { get; set; } = Profile.User.PermissionLevel;

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Profile.User.CenterId;
        /// <summary>
        /// 発行区分
        /// </summary>
        public PrnClasses PrnClass { get; set; } = PrnClasses.New;

        /// <summary>
        /// EC出荷形態
        /// </summary>
        public EcShipClasses EcShipClass { get; set; } = EcShipClasses.Singles;

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.BatchNo), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチName
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.BatchNo), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BatchName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.Jan), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 個口数
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.UnitCnt), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string UnitCnt { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize1 { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize2 { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize3 { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize4 { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize5 { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize6 { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize7 { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize8 { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize9 { get; set; }


        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize10 { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.BoxNo), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ShipInstructId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 送り状番号
        /// </summary>
        public string DeliNo { get; set; }

        /// <summary>
        /// 配送業者ID
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 出荷指示明細ID
        /// </summary>
        public int ShipInstructSeq { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 過去分含む
        /// </summary>
        public bool ChkOldData { get; set; } = false;

        public string ErrMassage1 { get; set; }
        public string ErrMassage2 { get; set; }
        public string ErrMassage3 { get; set; }
        public string ErrMassage4 { get; set; }

        public string ErrMassagePop { get; set; }
        public string ErrShipInstructId { get; set; }

        public string Ret { get; set; }
    }
}