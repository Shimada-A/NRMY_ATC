namespace Wms.Areas.Move.ViewModels.TransferReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Move.Resources;

    /// <summary>
    /// 仕入入荷進捗照会(明細別)
    /// </summary>
    public class TransferReference02ResultRow
    {
        #region プロパティ
        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.CenterId), ResourceType = typeof(TransferReferenceResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 移動区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.TransferClass), ResourceType = typeof(TransferReferenceResource))]
        public string TransferClass { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ArrivePlanDate), ResourceType = typeof(TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 移動元店舗区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreClass), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string TransferFromStoreClass { get; set; }

        /// <summary>
        /// 移動元ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreId), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreName), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.SlipNo), ResourceType = typeof(TransferReferenceResource))]
        public string SlipNo { get; set; }

        /// <summary>
        /// 行No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.SlipSeq), ResourceType = typeof(TransferReferenceResource))]
        public long SlipSeq { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.CategoryId1), ResourceType = typeof(TransferReferenceResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.CategoryName1), ResourceType = typeof(TransferReferenceResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.Item), ResourceType = typeof(TransferReferenceResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.Item), ResourceType = typeof(TransferReferenceResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemColor), ResourceType = typeof(TransferReferenceResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemColor), ResourceType = typeof(TransferReferenceResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemSize), ResourceType = typeof(TransferReferenceResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemSize), ResourceType = typeof(TransferReferenceResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.Jan), ResourceType = typeof(TransferReferenceResource))]
        public string Jan { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemSkuId), ResourceType = typeof(TransferReferenceResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ArrivePlanQty), ResourceType = typeof(TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ResultQty), ResourceType = typeof(TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.DifferenceQty), ResourceType = typeof(TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceQty { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ConfirmDate), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 予定外
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.UnplannedFlag), ResourceType = typeof(TransferReferenceResource))]
        public int? UnplannedFlag { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.TransferStatus), ResourceType = typeof(TransferReferenceResource))]
        public string TransferStatus { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.TransferStatus), ResourceType = typeof(TransferReferenceResource))]
        public string TransferStatusName { get; set; }

        /// <summary>
        /// 移動入荷実績.更新回数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.TtrUpdateCount), ResourceType = typeof(TransferReferenceResource))]
        public int TtrUpdateCount { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.DenpyoDate), ResourceType = typeof(Resources.TransferReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SlipDate { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.BrandName), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.BrandName), ResourceType = typeof(Resources.TransferReferenceResource))]
        public string BrandShortName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.BoxNo), ResourceType = typeof(Resources.TransferReferenceResource))]
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

        #endregion プロパティ
    }
}