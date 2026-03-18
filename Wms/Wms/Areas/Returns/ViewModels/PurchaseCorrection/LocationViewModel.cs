using System.ComponentModel.DataAnnotations;

namespace Wms.Areas.Returns.ViewModels.LocationSearchModal
{
    public partial class LocationViewModel
    {
        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        public string Jan { get; set; }
        /// <summary>
        /// ロケーション
        /// </summary>
        public string LocationCd { get; set; }

        /// <summary>
        /// ロケーション区分
        /// </summary>
        public string LocationClass { get; set; }

        /// <summary>
        /// 格付け
        /// </summary>
        public string GradeId { get; set; }

        //在庫数
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQty { get; set; }

        //引当数
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        //出荷可能数（在庫数-引当数）
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? KanouQty { get; set; }

        //[荷姿区分]1:ケース2:バラ9:指定なし
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CaseClass { get; set; }

    }
}