namespace Wms.Areas.Move.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Move.Resources;
    using Wms.Models;

    /// <summary>
    /// 移動入荷進捗照会ワーク02
    /// </summary>
    [Table("WW_ARR_TRANS_REF02")]
    public partial class MovTransRef02 : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MovTransRef02Resource.Seq), ResourceType = typeof(MovTransRef02Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MovTransRef02Resource.LineNo), ResourceType = typeof(MovTransRef02Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.CenterId), ResourceType = typeof(MovTransRef02Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 移動区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.TransferClass), ResourceType = typeof(MovTransRef02Resource))]
        public int? TransferClass { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ArrivePlanDate), ResourceType = typeof(MovTransRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 移動元店舗区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.MovTransRef02Resource.TransferFromStoreClass), ResourceType = typeof(Resources.MovTransRef02Resource))]
        public string TransferFromStoreClass { get; set; }

        /// <summary>
        /// 移動元ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.MovTransRef02Resource.TransferFromStoreId), ResourceType = typeof(Resources.MovTransRef02Resource))]
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.MovTransRef02Resource.TransferFromStoreName), ResourceType = typeof(Resources.MovTransRef02Resource))]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.SlipNo), ResourceType = typeof(MovTransRef02Resource))]
        public string SlipNo { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.SlipSeq), ResourceType = typeof(MovTransRef02Resource))]
        public long SlipSeq { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.CategoryId1), ResourceType = typeof(MovTransRef02Resource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.CategoryName1), ResourceType = typeof(MovTransRef02Resource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ItemId), ResourceType = typeof(MovTransRef02Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ItemName), ResourceType = typeof(MovTransRef02Resource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ItemColorId), ResourceType = typeof(MovTransRef02Resource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ItemColorName), ResourceType = typeof(MovTransRef02Resource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ItemSizeId), ResourceType = typeof(MovTransRef02Resource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ItemSizeName), ResourceType = typeof(MovTransRef02Resource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.Jan), ResourceType = typeof(MovTransRef02Resource))]
        public string Jan { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ItemSkuId), ResourceType = typeof(MovTransRef02Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ArrivePlanQty), ResourceType = typeof(MovTransRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.ResultQty), ResourceType = typeof(MovTransRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.TransferStatus), ResourceType = typeof(MovTransRef02Resource))]
        public string TransferStatus { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.TransferStatus), ResourceType = typeof(MovTransRef02Resource))]
        public string TransferStatusName { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.MovTransRef02Resource.ConfirmDate), ResourceType = typeof(Resources.MovTransRef02Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 予定外
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef02Resource.UnplannedFlag), ResourceType = typeof(MovTransRef02Resource))]
        public int? UnplannedFlag { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.SlipDate), ResourceType = typeof(MovTransRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SlipDate { get; set; }

        /// <summary>
        /// 梱包番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(MovTransRef01Resource.BoxNo), ResourceType = typeof(MovTransRef01Resource))]
        public string BoxNo { get; set; }

        #endregion プロパティ
    }
}