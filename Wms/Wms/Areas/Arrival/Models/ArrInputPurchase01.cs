namespace Wms.Areas.Arrival.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Arrival.Resources;
    using Wms.Models;

    /// <summary>
    /// 仕入入荷実績入力ワーク01
    /// </summary>
    [Table("WW_ARR_INPUT_PURCHASE01")]
    public partial class ArrInputPurchase01 : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArrInputPurchase01Resource.Seq), ResourceType = typeof(ArrInputPurchase01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ArrInputPurchase01Resource.LineNo), ResourceType = typeof(ArrInputPurchase01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(ArrInputPurchase01Resource.CenterId), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.ArrivePlanDate), ResourceType = typeof(ArrInputPurchase01Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.VendorId), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.VendorName), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.InvoiceNo), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.PoId), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string PoId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.CategoryName1), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.ItemId), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.ItemName), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string ItemName { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.ItemSkuQty), ResourceType = typeof(ArrInputPurchase01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.PackingPlanQty), ResourceType = typeof(ArrInputPurchase01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PackingPlanQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.PackingResultQty), ResourceType = typeof(ArrInputPurchase01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PackingResultQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.ArrivePlanQty), ResourceType = typeof(ArrInputPurchase01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.ResultQty), ResourceType = typeof(ArrInputPurchase01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.ArrivalStatus), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string ArrivalStatus { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase01Resource.ArrivalStatusName), ResourceType = typeof(ArrInputPurchase01Resource))]
        public string ArrivalStatusName { get; set; }

        #endregion プロパティ
    }
}