namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 仕入入荷進捗照会(明細別)
    /// </summary>
    public class PurchaseReference02ResultRow
    {
        #region プロパティ

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanDate), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.Vendor), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.Vendor), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.InvoiceNo), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.InvoiceSeq), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public long InvoiceSeq { get; set; }

        /// <summary>
        /// 発注番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.PoId), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string PoId { get; set; }

        /// <summary>
        /// 事業部ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.DivisionId), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string DivisionId { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.BrandId), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.BrandName), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string BrandName { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.BrandName), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string BrandShortName { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.CategoryId1), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.CategoryId2), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.CategoryId3), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.CategoryId4), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.CategoryName1), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.Item), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.Item), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ItemSkuId), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ItemColor), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ItemColor), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ItemSize), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ItemSize), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.Jan), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string Jan { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.BoxNo), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ResultQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.DifferenceQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivalStatus), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string ArrivalStatus { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivalStatusName), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        public string ArrivalStatusName { get; set; }

        /// <summary>
        /// TC指示数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.InvoicePlanQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? WmsInstructQty { get; set; }

        /// <summary>
        /// 格納予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.StoragePlanQty), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StoragePlanQty { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.PurchaseReferenceResource.ConfirmDate), ResourceType = typeof(Resources.PurchaseReferenceResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        public string CenterId { get; set; }

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