namespace Wms.Areas.Arrival.ViewModels.UnshelvedReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 仕入入荷未棚付
    /// </summary>
    public class UnshelvedReferenceResultRow
    {
        #region プロパティ

        /// <summary>
        /// 所在ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.CenterId), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 所在
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.CenterName), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string CenterName { get; set; }

        /// <summary>
        /// 入荷日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.ArrivalDate), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド略称
        /// </summary>
        /// <remarks>
        public string BrandShortName { get; set; }

        /// <summary>
        /// 仕入先ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.Vendor), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先NAME
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.Vendor), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.InvoiceNo), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 納品書行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.InvoiceSeq), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public int InvoiceSeq { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.CategoryId1), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.CategoryId1), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.ItemId), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.ItemIdName), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.ItemColor), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.ItemColor), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.ItemSize), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.ItemSize), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.Jan), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string Jan { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.BoxNo), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// ケース数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.CaseQty), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CaseQty { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.UnshelvedReferenceResource.StockQty), ResourceType = typeof(Resources.UnshelvedReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int StockQty { get; set; }

        /// <summary>
        /// バラ在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = "バラ在庫数")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int BaraQty { get; set; }

        #endregion
    }
}
