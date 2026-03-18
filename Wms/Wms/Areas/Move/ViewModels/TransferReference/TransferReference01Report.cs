namespace Wms.Areas.Move.ViewModels.TransferReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Move.Resources;

    public class TransferReference01Report
    {

        /// <summary>
        /// 伝票日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.DenpyoDate), ResourceType = typeof(Resources.TransferReferenceResource), Order = 1)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SLIP_DATE { get; set; }

        /// <summary>
        /// 移動元
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreId), ResourceType = typeof(Resources.TransferReferenceResource), Order = 2)]
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreName), ResourceType = typeof(Resources.TransferReferenceResource), Order = 3)]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.SlipNoPlanQty), ResourceType = typeof(Resources.TransferReferenceResource), Order = 4)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SlipNoPlanQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.SlipNoResultQty), ResourceType = typeof(Resources.TransferReferenceResource), Order = 5)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SlipNoResultQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ItemSkuQty), ResourceType = typeof(Resources.TransferReferenceResource), Order = 6)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ArrivePlanQty), ResourceType = typeof(Resources.TransferReferenceResource), Order = 7)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ResultQty), ResourceType = typeof(Resources.TransferReferenceResource), Order = 8)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferStatus), ResourceType = typeof(Resources.TransferReferenceResource), Order = 9)]
        public string TransferStatusName { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ConfirmDate), ResourceType = typeof(Resources.TransferReferenceResource), Order = 10)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }
    }
}