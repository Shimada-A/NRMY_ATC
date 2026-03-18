namespace Wms.Areas.Stock.ViewModels.Adjust
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Stock.Resources;
    using Wms.Common;

    /// <summary>
    /// 在庫明細
    /// </summary>
    public class AdjustResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.CenterId), ResourceType = typeof(AdjustResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 倉庫
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.CenterName), ResourceType = typeof(AdjustResource))]
        public string CenterName { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.LocationCd), ResourceType = typeof(AdjustResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.CategoryId1), ResourceType = typeof(AdjustResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.CategoryId1), ResourceType = typeof(AdjustResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.ItemId), ResourceType = typeof(AdjustResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.ItemIdName), ResourceType = typeof(AdjustResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.ItemColor), ResourceType = typeof(AdjustResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.ItemColor), ResourceType = typeof(AdjustResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.ItemSize), ResourceType = typeof(AdjustResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.ItemSize), ResourceType = typeof(AdjustResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.Jan), ResourceType = typeof(AdjustResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.GradeId), ResourceType = typeof(AdjustResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.Grade), ResourceType = typeof(AdjustResource))]
        public string GradeName { get; set; }

        /// <summary>
        /// ケース数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.CaseQty), ResourceType = typeof(AdjustResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CaseQty { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.StockQty), ResourceType = typeof(AdjustResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQty { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.AllocQty), ResourceType = typeof(AdjustResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.UnAllocQty), ResourceType = typeof(AdjustResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? UnAllocQty { get { return this.StockQty - this.AllocQty; } }

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
        /// ロケーション区分
        /// </summary>
        public string LocationClass { get; set; }

        #endregion プロパティ
    }
}