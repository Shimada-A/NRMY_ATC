namespace Wms.Areas.Returns.ViewModels.EcReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Returns.Resources;

    /// <summary>
    /// ReportList
    /// </summary>
    public class EcReferenceReport
    {
        #region プロパティ

        /// <summary>
        /// 返品登録日
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ArriveDate), ResourceType = typeof(EcReferenceResource), Order = 1)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// EC注文番号(出荷指示ID)
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ShipInstructId), ResourceType = typeof(EcReferenceResource), Order = 2)]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷日
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.KakuDate), ResourceType = typeof(EcReferenceResource), Order = 3)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 注文明細(出荷指示明細ID)
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ShipInstructId), ResourceType = typeof(EcReferenceResource), Order = 2)]
        public string ShipInstructSeq { get; set; }  

        /// <summary>
        /// 返品伝票ID
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ReturnId), ResourceType = typeof(EcReferenceResource), Order = 4)]
        public string ReturnId { get; set; }

        /// <summary>
        /// 返品伝票明細ID
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ReturnId), ResourceType = typeof(EcReferenceResource), Order = 4)]
        public string ReturnSeq { get; set; }

        /// <summary>
        /// 分類1名CATEGORY_NAME1
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
        /// カラーID
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 出荷実績数
        /// </summary>
        public int ResultQty { get; set; }

        /// <summary>
        /// 返品数
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ReturnQtySum), ResourceType = typeof(EcReferenceResource), Order = 5)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ReturnQty { get; set; }


        #endregion プロパティ
    }
}