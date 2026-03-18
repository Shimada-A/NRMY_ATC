namespace Wms.Areas.Move.ViewModels.InputTransfer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Models;

    /// <summary>
    /// 仕入入荷実績入力
    /// </summary>
    public class InputTransfer01ResultRow : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        public bool IsCheck { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.ArrivePlanDate), ResourceType = typeof(Resources.InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 移動元センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.TransferFromCenter), ResourceType = typeof(Resources.InputTransferResource))]
        public string TransferFromCenterId { get; set; }

        /// <summary>
        /// 移動元店舗
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.TransferFromStoreId), ResourceType = typeof(Resources.InputTransferResource))]
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元店舗
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.TransferFromStoreName), ResourceType = typeof(Resources.InputTransferResource))]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.SlipNoPlanQty), ResourceType = typeof(Resources.InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SlipNoPlanQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.SlipNoResultQty), ResourceType = typeof(Resources.InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SlipNoResultQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.ItemSkuQty), ResourceType = typeof(Resources.InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.ArrivePlanQty), ResourceType = typeof(Resources.InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.ResultQty), ResourceType = typeof(Resources.InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.TransferStatus), ResourceType = typeof(Resources.InputTransferResource))]
        public string TransferStatus { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.TransferStatus), ResourceType = typeof(Resources.InputTransferResource))]
        public string TransferStatusName { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.ConfirmDate), ResourceType = typeof(Resources.InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 入荷実績日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.TransferResultDate), ResourceType = typeof(Resources.InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? TransferResultDate { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.SlipDate), ResourceType = typeof(Resources.InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SlipDate { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.BrandName), ResourceType = typeof(Resources.InputTransferResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.BrandName), ResourceType = typeof(Resources.InputTransferResource))]
        public string BrandShortName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.BoxNo), ResourceType = typeof(Resources.InputTransferResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 伝票番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.SlipNo), ResourceType = typeof(Resources.InputTransferResource))]
        public string SlipNo { get; set; }

        /// <summary>
        /// 移動区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.TransferClass), ResourceType = typeof(Resources.InputTransferResource))]
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
        public string LineNo { get; set; }

        /// <summary>
        /// 予定外
        /// </summary>
        /// <remarks>
        public int? UnplannedFlag { get; set; }

        #endregion プロパティ
    }
}