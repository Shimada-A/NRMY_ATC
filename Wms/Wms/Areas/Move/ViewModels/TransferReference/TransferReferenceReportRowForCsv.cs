namespace Wms.Areas.Move.ViewModels.TransferReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Areas.Move.Resources;
    using Wms.Common;
    using Wms.Models;

    public class TransferReferenceReportRowForCsv
    {

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ArrivePlanDate), ResourceType = typeof(TransferReferenceResource), Order = 1)]
        public string ArrivePlanDate { get; set; }

        [Display(Name = nameof(TransferReferenceResource.CenterId), ResourceType = typeof(TransferReferenceResource), Order = 2)]
        public string CenterId { get; set; }

        [Display(Name = nameof(TransferReferenceResource.CenterName), ResourceType = typeof(TransferReferenceResource), Order = 3)]
        public string CenterName1 { get; set; }


        /// <summary>
        /// 移動元
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreId), ResourceType = typeof(Resources.TransferReferenceResource), Order = 4)]
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreName), ResourceType = typeof(Resources.TransferReferenceResource), Order = 5)]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.SlipNo), ResourceType = typeof(TransferReferenceResource), Order = 6)]
        public string SlipNo { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.Item), ResourceType = typeof(TransferReferenceResource), Order = 7)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.Item), ResourceType = typeof(TransferReferenceResource), Order = 8)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemColor), ResourceType = typeof(TransferReferenceResource), Order = 9)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemColor), ResourceType = typeof(TransferReferenceResource), Order = 10)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemSize), ResourceType = typeof(TransferReferenceResource), Order = 11)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemSize), ResourceType = typeof(TransferReferenceResource), Order = 12)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.Jan), ResourceType = typeof(TransferReferenceResource), Order = 13)]
        public string Jan { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ArrivePlanQty), ResourceType = typeof(TransferReferenceResource), Order = 14)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 発行者
        /// </summary>
        [Display(Name = nameof(TransferReferenceResource.PrintUser), ResourceType = typeof(TransferReferenceResource), Order = 15)]
        public string PrintUser { get; set; }

        /// <summary>
        /// FULLケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.SlipNo), ResourceType = typeof(TransferReferenceResource), Order = 16)]
        public string FullSlipNo { get; set; }
    }
}