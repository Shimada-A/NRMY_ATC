namespace Wms.Areas.Move.ViewModels.TransferReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 移動入荷進捗照会(移動元別)
    /// </summary>
    public class TransferReference01ResultRow
    {
        #region プロパティ

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ArrivePlanDate), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 移動元
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreId), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreName), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.SlipNoPlanQty), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SlipNoPlanQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.SlipNoResultQty), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SlipNoResultQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ItemSkuQty), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ArrivePlanQty), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ResultQty), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferStatus), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string TransferStatusName { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ConfirmDate), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.DenpyoDate), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DenpyoDate { get; set; }

        /// <summary>
        /// 移動区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferClass), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string TransferClass { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; }

        /// <summary>
        /// 予定外
        /// </summary>
        /// <remarks>
        public int? UnplannedFlag { get; set; }


        #endregion プロパティ
    }
}