namespace Wms.Areas.Arrival.ViewModels.InputPurchase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class InputPurchase02SearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 入荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注ID
        /// </summary>
        public string PoId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQtySum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQtySum { get; set; }

        /// <summary>
        /// 実績数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQtySum { get; set; }

        /// <summary>
        /// 梱包予定データ有無
        /// </summary>
        public bool PlanExistFlag { get; set; } = true;

        /// <summary>
        /// 仕入入荷進捗画面から遷移した場合、true
        /// </summary>
        public bool FromPurchaseReference { get; set; }
    }
}