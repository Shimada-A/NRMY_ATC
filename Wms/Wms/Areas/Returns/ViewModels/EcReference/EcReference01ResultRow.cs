namespace Wms.Areas.Returns.ViewModels.EcReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EcReference01ResultRow
    {
        /// <summary>
        /// 返品登録日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷確定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 返品伝票ID
        /// </summary>
        public string ReturnId { get; set; }

        /// <summary>
        /// 返品数
        /// </summary>
        public int? ReturnQtySum { get; set; }

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
    }
}