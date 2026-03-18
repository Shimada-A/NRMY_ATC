namespace Wms.ViewModels.Notice
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class NoticeDetailRow
    {
        /// <summary>
        /// 行No
        /// </summary>
        public int RowNo { get; set; }

        /// <summary>
        /// EC区分名称
        /// </summary>
        public string EcClassNm { get; set; }

        /// <summary>
        /// お知らせ連携エラー明細
        /// </summary>
        public NoticeDetail NoticeDetail { get; set; }
    }
}