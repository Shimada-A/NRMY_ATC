namespace Wms.Areas.Inventory.ViewModels.Reference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// 棚卸進捗照会画面（ロケ別）
    /// </summary>
    public class Reference02ResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// ロケーションコード
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.LocationCd), ResourceType = typeof(Resources.ReferenceResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// 荷姿区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CaseClass), ResourceType = typeof(Resources.ReferenceResource))]
        public string CaseClass { get; set; }

        /// <summary>
        /// 格付ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.GradeId), ResourceType = typeof(Resources.ReferenceResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.BoxNo), ResourceType = typeof(Resources.ReferenceResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemId), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemName), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Jan), ResourceType = typeof(Resources.ReferenceResource))]
        public string Jan { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColorId), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColorId), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSizeId), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSizeId), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 帳簿在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.StockQtyStart), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int StockQtyStart { get; set; }

        /// <summary>
        /// 実棚数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ResultQty), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数(+)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DifferencePlus), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferencePlus { get; set; }

        /// <summary>
        /// 差異数(-)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DifferenceMinus), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceMinus { get; set; }

        /// <summary>
        /// カウント回数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CountSeq), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CountSeq { get; set; }

        /// <summary>
        /// 差異数合計
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DifferenceSum), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceSum { get; set; }

        /// <summary>
        /// 担当者
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.UserId), ResourceType = typeof(Resources.ReferenceResource))]
        public string UserId { get; set; }

        /// <summary>
        /// 担当者名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.UserId), ResourceType = typeof(Resources.ReferenceResource))]
        public string UserName { get; set; }

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