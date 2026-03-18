namespace Wms.Areas.Move.ViewModels.InputTransfer
{
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.PeerResolvers;
    using Wms.Areas.Move.Resources;
    using Wms.Models;

    /// <summary>
    /// 仕入入荷実績入力
    /// </summary>
    public class InputTransfer02ResultRow : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.SlipNo), ResourceType = typeof(InputTransferResource))]
        public string SlipNo { get; set; }

        /// <summary>
        /// 行No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.SlipSeq), ResourceType = typeof(InputTransferResource))]
        public long SlipSeq { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.CategoryName1), ResourceType = typeof(InputTransferResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.Item), ResourceType = typeof(InputTransferResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.Item), ResourceType = typeof(InputTransferResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.ItemColor), ResourceType = typeof(InputTransferResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.ItemColor), ResourceType = typeof(InputTransferResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.ItemSize), ResourceType = typeof(InputTransferResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.ItemSize), ResourceType = typeof(InputTransferResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.Jan), ResourceType = typeof(InputTransferResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.ArrivePlanQty), ResourceType = typeof(InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.ResultQty), ResourceType = typeof(InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 入力実績数
        /// </summary>
        /// <remarks>
        /// 画面で入力された実績数
        /// </remarks>
        [Display(Name = nameof(MovTransInput02Resource.InputResultQty), ResourceType = typeof(MovTransInput02Resource))]
        [MinValue(0)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InputResultQty { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.ConfirmDate), ResourceType = typeof(InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        public string TransferStatus { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; }


        /// <summary>
        /// 入荷実績日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputTransferResource.TransferResultDate), ResourceType = typeof(InputTransferResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? TransferResultDate { get; set; }

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
        /// 移動区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.TransferClass), ResourceType = typeof(Resources.InputTransferResource))]
        public string TransferClass { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.InputTransferResource.BoxNo), ResourceType = typeof(Resources.InputTransferResource))]
        public string BoxNo { get; set; }

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
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        #endregion プロパティ
    }
}