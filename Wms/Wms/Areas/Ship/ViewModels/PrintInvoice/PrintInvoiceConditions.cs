namespace Wms.Areas.Ship.ViewModels.PrintInvoice
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PrintInvoiceConditions
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

        public PermissionLevelClasses PermissonLavel { get; set; } = Profile.User.PermissionLevel;

        public string UserId { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Profile.User.CenterId;
        /// <summary>
        /// 発行区分
        /// </summary>
        public PrnClasses PrnClass { get; set; } = PrnClasses.New;

        /// <summary>
        /// ケースNo
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.BoxNo), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(Resources.PrintEcInvoiceResource.ItemSizeId), ResourceType = typeof(Resources.PrintEcInvoiceResource))]
        public string BoxSize { get; set; }

        /// <summary>
        /// 送り状番号
        /// </summary>
        public string DeliNo { get; set; }

        /// <summary>
        /// 配送業者ID
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者マスタ送り状発行フラグ(0:発行しない 1:発行する)
        /// </summary>
        public int InvoicePrintFlag { get; set; }

        /// <summary>
        /// 納品書発行フラグ
        /// </summary>
        public string NouhinPrnFlag { get; set; }

        /// <summary>
        /// 過去分含む
        /// </summary>
        public bool ChkOldData { get; set; } = false;

        /// <summary>
        /// 他のITEM納品書番号のスキャンを許可する
        /// </summary>
        public bool ChkOtherListScan { get; set; } = false;

        public string ErrMassage { get; set; }

        /// <summary>
        /// ファイルパス
        /// </summary>
        public string Ret { get; set; }

        /// <summary>
        /// ユーザー倉庫ID
        /// </summary>
        public string UserCenterId { get; set; }

        /// <summary>
        /// 確認メッセージ確認済み
        /// </summary>
        public bool Confirmed { get; set; }

        /// <summary>
        /// 作業中リスト
        /// </summary>
        public List<ScanAssortView> ScanAssortViews { get; set; } = new List<ScanAssortView>();

        /// <summary>
        /// 入荷伝票日付
        /// </summary>
        public DateTime SlipDate { get; set; }

        /// <summary>
        /// 作業納品書番号
        /// </summary>
        public string InvoiceNo {get; set;}

        /// <summary>
        /// 出荷区分
        /// </summary>
        public int ShipClass { get; set; }

        /// <summary>
        ///  出荷保留区分 0:即出荷 1：出荷保留
        /// </summary>
        public int ShippingHoldClass { get; set; }
    }
}