namespace Wms.Areas.Returns.ViewModels.EcReference
{
    using System.ComponentModel.DataAnnotations;
    using System;
    public class EcReference02ResultRow
    {
        #region プロパティ
        /// <summary>
        /// 返品登録日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷確定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 返品伝票ID
        /// </summary>
        public string ReturnId { get; set; }

        /// <summary>
        /// 関連注文番号
        /// </summary>
        public string RelatedOrderNo { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        public string CategoryName1 { get; set; }

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
        /// 出荷実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 返品実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ReturnQty { get; set; }
        #endregion プロパティ
    }
}