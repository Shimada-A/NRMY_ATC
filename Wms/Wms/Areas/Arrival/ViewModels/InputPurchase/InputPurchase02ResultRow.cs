namespace Wms.Areas.Arrival.ViewModels.InputPurchase
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;
    using Wms.Models;

    /// <summary>
    /// 仕入入荷実績入力
    /// </summary>
    public class InputPurchase02ResultRow : BaseModel
    {
        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Display(Name = nameof(InputPurchaseResource.Seq), ResourceType = typeof(InputPurchaseResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Display(Name = nameof(InputPurchaseResource.LineNo), ResourceType = typeof(InputPurchaseResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(InputPurchaseResource.CenterId), ResourceType = typeof(InputPurchaseResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ArrivePlanDate), ResourceType = typeof(InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.VendorId), ResourceType = typeof(InputPurchaseResource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.VendorName), ResourceType = typeof(InputPurchaseResource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.InvoiceNo), ResourceType = typeof(InputPurchaseResource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.PoId), ResourceType = typeof(InputPurchaseResource))]
        public string PoNo { get; set; }

        /// <summary>
        /// 納品書行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.InvoiceSeq), ResourceType = typeof(InputPurchaseResource))]
        public long? InvoiceSeq { get; set; }

        /// <summary>
        /// ケースNO
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.BoxNo), ResourceType = typeof(InputPurchaseResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 予定ケースNO
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.PlanBoxNo), ResourceType = typeof(InputPurchaseResource))]
        public string PlanBoxNo { get; set; }

        /// <summary>
        /// 実績ケースNO
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ResultBoxNo), ResourceType = typeof(InputPurchaseResource))]
        public string ResultBoxNo { get; set; }

        /// <summary>
        /// 荷姿区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.CaseClass), ResourceType = typeof(InputPurchaseResource))]
        public int? CaseClass { get; set; }

        /// <summary>
        /// TCDC区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.TcdcClass), ResourceType = typeof(InputPurchaseResource))]
        public int? TcdcClass { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.CategoryId1), ResourceType = typeof(InputPurchaseResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.CategoryName1), ResourceType = typeof(InputPurchaseResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ItemId), ResourceType = typeof(InputPurchaseResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ItemName), ResourceType = typeof(InputPurchaseResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ItemColor), ResourceType = typeof(InputPurchaseResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ItemColor), ResourceType = typeof(InputPurchaseResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ItemSize), ResourceType = typeof(InputPurchaseResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ItemSize), ResourceType = typeof(InputPurchaseResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.Jan), ResourceType = typeof(InputPurchaseResource))]
        public string Jan { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ItemSkuId), ResourceType = typeof(InputPurchaseResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 梱包予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.PackingPlanQty), ResourceType = typeof(InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PackingPlanQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ArrivePlanQty), ResourceType = typeof(InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.PlanQty), ResourceType = typeof(InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ResultQty), ResourceType = typeof(InputPurchaseResource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PackingResultQty { get; set; }

        /// <summary>
        /// 更新回数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ResultUpdateCount), ResourceType = typeof(InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultUpdateCount { get; set; }

        /// <summary>
        /// JAN件数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.JanQty), ResourceType = typeof(InputPurchaseResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? JanQty { get; set; }

        /// <summary>
        /// 予定梱包行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.PlanBoxSeq), ResourceType = typeof(InputPurchaseResource))]
        public long? PlanBoxSeq { get; set; }

        /// <summary>
        /// 実績梱包行番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ResultBoxSeq), ResourceType = typeof(InputPurchaseResource))]
        public long? ResultBoxSeq { get; set; }

        /// <summary>
        /// 入力実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputPurchaseResource.ResultQty), ResourceType = typeof(InputPurchaseResource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InputResultQty { get; set; }
    }
}