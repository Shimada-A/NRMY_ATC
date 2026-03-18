namespace Wms.Areas.Stock.ViewModels.LocMove
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 仕入入荷実績入力
    /// </summary>
    public class LocMoveResultRow
    {
        #region プロパティ

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        public bool IsCheck { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        public string LocationCd { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        public string GradeId { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        public string GradeName { get; set; }

        /// <summary>
        /// ケース数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CaseQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SkuQty { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 未引当数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? NotAllocQty { get; set; }

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