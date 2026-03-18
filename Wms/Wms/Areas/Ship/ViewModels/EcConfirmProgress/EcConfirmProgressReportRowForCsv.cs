namespace Wms.Areas.Ships.ViewModels.EcConfirmProgress
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;
    using Wms.Models;

    public class EcConfirmProgressReportRowForCsv
    {
        [Display(Name = nameof(EcConfirmProgressResource.CenterId), ResourceType = typeof(EcConfirmProgressResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(EcConfirmProgressResource.PrintUser), ResourceType = typeof(EcConfirmProgressResource), Order = 2)]
        public string PrintUser { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.ShipInstructId), ResourceType = typeof(EcConfirmProgressResource), Order = 3)]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 明細ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.ShipInstructSeq), ResourceType = typeof(EcConfirmProgressResource), Order = 4)]
        public string ShipInstructSeq { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.BoxNo), ResourceType = typeof(EcConfirmProgressResource), Order = 5)]
        public string BoxNo { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.Item), ResourceType = typeof(EcConfirmProgressResource), Order = 6)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.Item), ResourceType = typeof(EcConfirmProgressResource), Order = 7)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.Color), ResourceType = typeof(EcConfirmProgressResource), Order = 8)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.Color), ResourceType = typeof(EcConfirmProgressResource), Order = 9)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.Size), ResourceType = typeof(EcConfirmProgressResource), Order = 10)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.Size), ResourceType = typeof(EcConfirmProgressResource), Order = 11)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.JAN), ResourceType = typeof(EcConfirmProgressResource), Order = 12)]
        public string Jan1 { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.JAN), ResourceType = typeof(EcConfirmProgressResource), Order = 13)]
        public string Jan2 { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.JAN), ResourceType = typeof(EcConfirmProgressResource), Order = 14)]
        public string Jan { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(EcConfirmProgressResource.ResultQty), ResourceType = typeof(EcConfirmProgressResource), Order = 15)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }
    }
}