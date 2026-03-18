using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Wms.Areas.Arrival.ViewModels.InputPurchase
{
    /// <summary>
    /// ShipFrontage which is posted from view
    /// </summary>
    [Serializable]
    public class SelectedInputPurchase01ViewModel
    {
        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; } = false;

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
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 荷主ID
        /// </summary>
        public string ShipperId { get; set; }

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        /// <remarks>
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注ID
        /// </summary>
        /// <remarks>
        public string PoId { get; set; }
    }
}