using System;
using System.ComponentModel.DataAnnotations;
using Wms.Models;

namespace Wms.Areas.Move.ViewModels.InputTransfer
{
    /// <summary>
    /// ShipFrontage which is posted from view
    /// </summary>
    public class SelectedInputTransfer01ViewModel :BaseModel
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
        public string LineNo { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 入荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArrivePlanDate { get; set; }

        /// <summary>
        /// 移動元センター
        /// </summary>
        public string TransferFromCenterId { get; set; }

        /// <summary>
        /// 移動元店舗
        /// </summary>
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元店舗(名称)
        /// </summary>
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// 予定外
        /// </summary>
        /// <remarks>
        public int? UnplannedFlag { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SlipDate { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        public string BoxNo { get; set; }

        /// <summary>
        /// 伝票番号
        /// </summary>
        /// <remarks>
        public string SlipNo { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        /// <remarks>
        public string BrandId { get; set; }

        /// <summary>
        /// 移動区分
        /// </summary>
        /// <remarks>
        public string TransferClass { get; set; }
    }
}