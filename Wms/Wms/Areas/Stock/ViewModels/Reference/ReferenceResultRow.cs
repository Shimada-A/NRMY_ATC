namespace Wms.Areas.Stock.ViewModels.Reference
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// 在庫明細
    /// </summary>
    public class ReferenceResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.IsCheck), ResourceType = typeof(Resources.ReferenceResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CenterId), ResourceType = typeof(Resources.ReferenceResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 倉庫
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CenterName), ResourceType = typeof(Resources.ReferenceResource))]
        public string CenterName { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.LocationCd), ResourceType = typeof(Resources.ReferenceResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.BoxNo), ResourceType = typeof(Resources.ReferenceResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 事業部
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.DivisionId), ResourceType = typeof(Resources.ReferenceResource))]
        public string DivisionId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId1), ResourceType = typeof(Resources.ReferenceResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryId1), ResourceType = typeof(Resources.ReferenceResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// カテゴリ２名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryName2), ResourceType = typeof(Resources.ReferenceResource))]
        public string CategoryName2 { get; set; }

        /// <summary>
        /// カテゴリ３名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryName3), ResourceType = typeof(Resources.ReferenceResource))]
        public string CategoryName3 { get; set; }

        /// <summary>
        /// カテゴリ４名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CategoryName4), ResourceType = typeof(Resources.ReferenceResource))]
        public string CategoryName4 { get; set; }

        /// <summary>
        /// ｼｰｽﾞﾝ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSeasonName), ResourceType = typeof(Resources.ReferenceResource))]
        public ItemSeasons ItemSeasonId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemIdName), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemSkuId { get; set; }

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
        [Display(Name = nameof(Resources.ReferenceResource.ItemIdName), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColor), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemColor), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSize), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSize), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 上代単価(税抜)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.NormalSellingPriceExTax), ResourceType = typeof(Resources.ReferenceResource))]
        public int? NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Jan), ResourceType = typeof(Resources.ReferenceResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.GradeId), ResourceType = typeof(Resources.ReferenceResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Grade), ResourceType = typeof(Resources.ReferenceResource))]
        public string GradeName { get; set; }

        /// <summary>
        /// 納品書No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.InvoiceNo), ResourceType = typeof(Resources.ReferenceResource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// ケース数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.CaseQty), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CaseQty { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.StockQty), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQty { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.AllocQty), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.UnAllocQty), ResourceType = typeof(Resources.ReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? UnAllocQty { get { return this.StockQty - this.AllocQty; } }

        /// <summary>
        /// シーズン年
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemSeasonName), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemSeasonYear { get; set; }

        /// <summary>
        /// アイテムコード
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemCode), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemCode { get; set; }

        /// <summary>
        /// アイテム名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.ItemCode), ResourceType = typeof(Resources.ReferenceResource))]
        public string ItemCodeName { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Brand), ResourceType = typeof(Resources.ReferenceResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Brand), ResourceType = typeof(Resources.ReferenceResource))]
        public string BrandName { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.ReferenceResource.Price), ResourceType = typeof(Resources.ReferenceResource))]
        public string Price { get; set; }

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