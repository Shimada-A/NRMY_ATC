namespace Wms.ViewModels.Notice
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class NoticePopUpRow
    {
        /// <summary>
        /// メッセージ区分
        /// </summary>
        public int MessageClass { get; set; }

        /// <summary>
        /// 発生日時
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTimeOffset OccurrenceDate { get; set; }

        /// <summary>
        /// 件名
        /// </summary>
        public string Subject { get; set; }
    }
}