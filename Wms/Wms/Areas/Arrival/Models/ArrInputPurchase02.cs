namespace Wms.Areas.Arrival.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Arrival.Resources;
    using Wms.Models;

    /// <summary>
    /// 仕入入荷実績入力ワーク02
    /// </summary>
    [Table("WW_ARR_INPUT_PURCHASE02")]
    public partial class ArrInputPurchase02 : BaseModel
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
        [Display(Name = nameof(ArrInputPurchase02Resource.Seq), ResourceType = typeof(ArrInputPurchase02Resource))]
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
        [Display(Name = nameof(ArrInputPurchase02Resource.LineNo), ResourceType = typeof(ArrInputPurchase02Resource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(ArrInputPurchase02Resource.CenterId), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ArrivePlanDate), ResourceType = typeof(ArrInputPurchase02Resource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.VendorId), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.VendorName), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.InvoiceNo), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.PoId), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string PoId { get; set; }

        /// <summary>
        /// 納品書行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.InvoiceSeq), ResourceType = typeof(ArrInputPurchase02Resource))]
        public long? InvoiceSeq { get; set; }

        /// <summary>
        /// 予定ケースNO
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.PlanBoxNo), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string PlanBoxNo { get; set; }

        /// <summary>
        /// 実績ケースNO
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ResultBoxNo), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string ResultBoxNo { get; set; }

        /// <summary>
        /// 荷姿区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.CaseClass), ResourceType = typeof(ArrInputPurchase02Resource))]
        public int? CaseClass { get; set; }

        /// <summary>
        /// TCDC区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.TcdcClass), ResourceType = typeof(ArrInputPurchase02Resource))]
        public int? TcdcClass { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.CategoryId1), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.CategoryName1), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ItemId), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ItemName), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ItemColorId), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ItemColorName), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ItemSizeId), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ItemSizeName), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.Jan), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string Jan { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ItemSkuId), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 梱包予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.PackingPlanQty), ResourceType = typeof(ArrInputPurchase02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PackingPlanQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ArrivePlanQty), ResourceType = typeof(ArrInputPurchase02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.PackingResultQty), ResourceType = typeof(ArrInputPurchase02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PackingResultQty { get; set; }

        /// <summary>
        /// 更新回数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ResultUpdateCount), ResourceType = typeof(ArrInputPurchase02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultUpdateCount { get; set; }

        /// <summary>
        /// JAN件数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.JanQty), ResourceType = typeof(ArrInputPurchase02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? JanQty { get; set; }

        /// <summary>
        /// 予定梱包行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.PlanBoxSeq), ResourceType = typeof(ArrInputPurchase02Resource))]
        public long? PlanBoxSeq { get; set; }

        /// <summary>
        /// 実績梱包行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.ResultBoxSeq), ResourceType = typeof(ArrInputPurchase02Resource))]
        public long? ResultBoxSeq { get; set; }

        /// <summary>
        /// 入力実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.InputResultQty), ResourceType = typeof(ArrInputPurchase02Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InputResultQty { get; set; }

        /// <summary>
        /// 表示梱包番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ArrInputPurchase02Resource.DispBoxNo), ResourceType = typeof(ArrInputPurchase02Resource))]
        public string DispBoxNo { get; set; }

        #endregion プロパティ
    }
}