namespace Wms.Areas.Arrival.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Arrival.Resources;
    using Wms.Models;

    /// <summary>
    /// 入荷実績確定ワーク01
    /// </summary>
    [Table("WW_ARR_CON_ACT01")]
    public partial class ArrConAct01 : BaseModel
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
        [Display(Name = nameof(ArrConAct01Resource.Seq), ResourceType = typeof(ArrConAct01Resource))]
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
        [Display(Name = nameof(ArrConAct01Resource.LineNo), ResourceType = typeof(ArrConAct01Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.IsCheck), ResourceType = typeof(ArrConAct01Resource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.CenterId), ResourceType = typeof(ArrConAct01Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ArrivePlanDate), ResourceType = typeof(ArrConAct01Resource))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 入荷日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ArriveDate), ResourceType = typeof(ArrConAct01Resource))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.Vendor), ResourceType = typeof(ArrConAct01Resource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.Vendor), ResourceType = typeof(ArrConAct01Resource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.InvoiceNo), ResourceType = typeof(ArrConAct01Resource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.PoId), ResourceType = typeof(ArrConAct01Resource))]
        public string PoId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.CategoryId1), ResourceType = typeof(ArrConAct01Resource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.CategoryId1), ResourceType = typeof(ArrConAct01Resource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ItemId), ResourceType = typeof(ArrConAct01Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ItemIdName), ResourceType = typeof(ArrConAct01Resource))]
        public string ItemName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ItemSkuId), ResourceType = typeof(ArrConAct01Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.SkuQty), ResourceType = typeof(ArrConAct01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SkuQty { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.PlanBoxNoQty), ResourceType = typeof(ArrConAct01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PlanBoxNoQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ResultBoxNoQty), ResourceType = typeof(ArrConAct01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultBoxNoQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ArrivePlanQty), ResourceType = typeof(ArrConAct01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ResultQty), ResourceType = typeof(ArrConAct01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ArrivalStatus), ResourceType = typeof(ArrConAct01Resource))]
        public byte ArrivalStatus { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrConAct01Resource.ArrivalStatus), ResourceType = typeof(ArrConAct01Resource))]
        public string ArrivalStatusName { get; set; }

        /// <summary>
        /// 入荷実績更新回数 (RESULT_UPDATE_COUNT)
        /// </summary>
        /// <remarks>
        /// 仕入入荷実績の更新回数(排他制御用)
        /// </remarks>
        [Display(Name = nameof(ArrConAct01Resource.ResultUpdateCount), ResourceType = typeof(ArrConAct01Resource))]
        public int? ResultUpdateCount { get; set; }

        #endregion プロパティ
    }
}