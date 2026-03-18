namespace Wms.Areas.Stock.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Stock.Resources;
    using Wms.Models;

    /// <summary>
    /// 荷姿別在庫
    /// </summary>
    [Table("T_PACKAGE_STOCKS")]
    public partial class PackageStock : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID
        /// </summary>
        [Key]
        [Column(Order = 15)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.CenterId), ResourceType = typeof(PackageStockResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.ItemSkuId), ResourceType = typeof(PackageStockResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.Jan), ResourceType = typeof(PackageStockResource))]
        [MaxLength(13, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 商品ID(品番)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.ItemId), ResourceType = typeof(PackageStockResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.ItemColorId), ResourceType = typeof(PackageStockResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.ItemSizeId), ResourceType = typeof(PackageStockResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// ロケーションコード
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.LocationCd), ResourceType = typeof(PackageStockResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// 格付ID
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.GradeId), ResourceType = typeof(PackageStockResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 梱包番号
        /// </summary>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.BoxNo), ResourceType = typeof(PackageStockResource))]
        [MaxLength(13, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 作成区分
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.MakeClass), ResourceType = typeof(PackageStockResource))]
        public int MakeClass { get; set; }

        /// <summary>
        /// 初期在庫数
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.IniStockQty), ResourceType = typeof(PackageStockResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int IniStockQty { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.StockQty), ResourceType = typeof(PackageStockResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int StockQty { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        [Key]
        [Column(Order = 14)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.InvoiceNo), ResourceType = typeof(PackageStockResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// TC入出荷仕分状況
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(PackageStockResource.SortStatus), ResourceType = typeof(PackageStockResource))]
        public int SortStatus { get; set; }

        /// <summary>
        /// 納品書SKU完了日時
        /// </summary>
        [Display(Name = nameof(PackageStockResource.InvoiceEndDate), ResourceType = typeof(PackageStockResource))]
        public DateTime? InvoiceEndDate { get; set; }

        /// <summary>
        /// 納品書SKU完了担当者
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(PackageStockResource.InvoiceUserId), ResourceType = typeof(PackageStockResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InvoiceUserId { get; set; }

        #endregion プロパティ
    }
}