namespace Wms.Areas.Arrival.ViewModels.ConfirmActual
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;

    /// <summary>
    /// 入荷実績確定
    /// </summary>
    public class ConfirmActualResultRow
    {
        #region プロパティ

        /// <summary>
        /// 更新回数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.UpdateCount), ResourceType = typeof(ConfirmActualResource))]
        public int UpdateCount { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ShipperId), ResourceType = typeof(ConfirmActualResource))]
        public string ShipperId { get; set; }

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.IsCheck), ResourceType = typeof(ConfirmActualResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.CenterId), ResourceType = typeof(ConfirmActualResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ArrivePlanDate), ResourceType = typeof(ConfirmActualResource))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 入荷日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ArriveDate), ResourceType = typeof(ConfirmActualResource))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// 仕入先ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.VendorId), ResourceType = typeof(ConfirmActualResource))]
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.VendorName), ResourceType = typeof(ConfirmActualResource))]
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.InvoiceNo), ResourceType = typeof(ConfirmActualResource))]
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.PoId), ResourceType = typeof(ConfirmActualResource))]
        public string PoId { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.CategoryId1), ResourceType = typeof(ConfirmActualResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.CategoryId1), ResourceType = typeof(ConfirmActualResource))]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ItemId), ResourceType = typeof(ConfirmActualResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ItemIdName), ResourceType = typeof(ConfirmActualResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.SkuQty), ResourceType = typeof(ConfirmActualResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SkuQty { get; set; }

        /// <summary>
        /// ケース予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.PlanBoxNoQty), ResourceType = typeof(ConfirmActualResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PlanBoxNoQty { get; set; }

        /// <summary>
        /// ケース実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ResultBoxNoQty), ResourceType = typeof(ConfirmActualResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultBoxNoQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ArrivePlanQty), ResourceType = typeof(ConfirmActualResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ResultQty), ResourceType = typeof(ConfirmActualResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状況
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ArrivalStatus), ResourceType = typeof(ConfirmActualResource))]
        public byte ArrivalStatus { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(ConfirmActualResource.ArrivalStatus), ResourceType = typeof(ConfirmActualResource))]
        public string ArrivalStatusName { get; set; }

        /// <summary>
        /// 入荷実績更新回数 (RESULT_UPDATE_COUNT)
        /// </summary>
        /// <remarks>
        /// 仕入入荷実績の更新回数(排他制御用)
        /// </remarks>
        [Display(Name = nameof(ArrConAct01Resource.ResultUpdateCount), ResourceType = typeof(ArrConAct01Resource))]
        public int? ResultUpdateCount { get; set; }

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

        #endregion
    }
}
