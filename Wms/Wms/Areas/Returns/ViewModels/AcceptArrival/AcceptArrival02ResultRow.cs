namespace Wms.Areas.Returns.ViewModels.AcceptArrival
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AcceptArrival02ResultRow
    {

        /// <summary>
        /// 行選択フラグ
        /// </summary>
        /// <remarks>
        public bool IsCheck { get; set; }

        /// <summary>
        /// J注文番号
        /// </summary>
        public String ShipInstructId { get; set; }

        /// <summary>
        /// 出荷確定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string DestPrefName { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        public string DestZip { get; set; }

        /// <summary>
        /// 出荷先名称
        /// </summary>
        public string DestName { get; set; }
    }
}