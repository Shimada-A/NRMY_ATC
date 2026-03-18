namespace Wms.Areas.Arrival.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Arrival.Resources;
    using Wms.Models;

    /// <summary>
    /// 仕入入荷進捗照会ワーク01
    /// </summary>
    [Table("WW_ARR_PUR_REF01")]
    public partial class ArrPurRef01 : BaseModel
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
        [Display(Name = nameof(ArrPurRef01Resource.Seq), ResourceType = typeof(ArrPurRef01Resource))]
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
        [Display(Name = nameof(ArrPurRef01Resource.LineNo), ResourceType = typeof(ArrPurRef01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.CenterId), ResourceType = typeof(ArrPurRef01Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.ArrivePlanDate), ResourceType = typeof(ArrPurRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.Vendor), ResourceType = typeof(ArrPurRef01Resource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.Vendor), ResourceType = typeof(ArrPurRef01Resource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 予定納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.PlanInvoiceNo), ResourceType = typeof(ArrPurRef01Resource))]
        public string PlanInvoiceNo { get; set; }

        /// <summary>
        /// 予定納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.ResultInvoiceNo), ResourceType = typeof(ArrPurRef01Resource))]
        public string ResultInvoiceNo { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.CasePlanQty), ResourceType = typeof(ArrPurRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CasePlanQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.CaseResultQty), ResourceType = typeof(ArrPurRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? CaseResultQty { get; set; }

        /// <summary>
        /// 商品ID(品番)
        /// </summary>
        [Display(Name = nameof(ArrPurRef01Resource.ItemId), ResourceType = typeof(ArrPurRef01Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [Display(Name = nameof(ArrPurRef01Resource.ItemSkuId), ResourceType = typeof(ArrPurRef01Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.ArrivePlanQty), ResourceType = typeof(ArrPurRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.ResultQty), ResourceType = typeof(ArrPurRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 残数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.RemainderQty), ResourceType = typeof(ArrPurRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? RemainderQty { get; set; }

        /// <summary>
        /// 差異(+)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.DifferencePlus), ResourceType = typeof(ArrPurRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferencePlus { get; set; }

        /// <summary>
        /// 差異(-)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrPurRef01Resource.DifferenceMinus), ResourceType = typeof(ArrPurRef01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceMinus { get; set; }

        #endregion プロパティ
    }
}