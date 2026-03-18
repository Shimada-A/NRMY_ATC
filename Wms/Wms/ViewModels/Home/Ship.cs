namespace Wms.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Resources;
    using System;

    public class Ship
    {
        /// <summary>
        /// 引当済出荷日
        /// </summary>
        [Display(Name = nameof(HomeResource.ShipPlanDate), ResourceType = typeof(HomeResource))]
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(HomeResource.CenterId), ResourceType = typeof(HomeResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 引当済出荷日当日 EC数量(オーダー数)
        /// </summary>
        [Display(Name = nameof(HomeResource.ShipEcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart ShipEcQty { get; set; }

        /// <summary>
        /// 引当済出荷日当日 TC数量
        /// </summary>
        [Display(Name = nameof(HomeResource.ShipDcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart ShipTcQty { get; set; }

        /// <summary>
        /// 引当済出荷日当日 DC数量
        /// </summary>
        [Display(Name = nameof(HomeResource.ShipDcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart ShipDcQty { get; set; }

        /// <summary>
        /// ピック作業 EC数量
        /// </summary>
        [Display(Name = nameof(HomeResource.PickEcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart PickEcQty { get; set; }

        /// <summary>
        /// ピック作業 DC数量
        /// </summary>
        [Display(Name = nameof(HomeResource.PickDcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart PickDcQty { get; set; }

        /// <summary>
        /// 店別仕分/摘取 TC数量
        /// </summary>
        [Display(Name = nameof(HomeResource.StoreTcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart StoreTcQty { get; set; }

        /// <summary>
        /// 店別仕分/摘取 DC数量
        /// </summary>
        [Display(Name = nameof(HomeResource.StoreDcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart StoreDcQty { get; set; }

        /// <summary>
        /// 納品書発行 EC数量
        /// </summary>
        [Display(Name = nameof(HomeResource.InvoiceEcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart InvoiceEcQty { get; set; }

        /// <summary>
        /// 納品書発行 TC数量
        /// </summary>
        [Display(Name = nameof(HomeResource.InvoiceTcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart InvoiceTcQty { get; set; }

        /// <summary>
        /// 納品書発行 DC数量
        /// </summary>
        [Display(Name = nameof(HomeResource.InvoiceDcQty), ResourceType = typeof(HomeResource))]
        public TcdcChart InvoiceDcQty { get; set; }

        /// <summary>
        /// 当日日付
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd（ddd）}")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}