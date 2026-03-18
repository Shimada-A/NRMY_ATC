namespace Wms.ViewModels.Notice
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Models;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class NoticeResultRow
    {

        public bool IsCheck { get; set; } = false;

        /// <summary>
        /// 行No
        /// </summary>
        public int RowNo { get; set; } = 1;

        /// <summary>
        /// 種別
        /// </summary>
        [Display(Name = nameof(Resources.NoticeResource.MessageClass), ResourceType = typeof(Resources.NoticeResource))]
        public string MessageClassNm { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        public string ShipperId { get; set; }

        /// <summary>
        /// 連携実行状態区分
        /// </summary>
        public string IfRunStateNm { get; set; }

        /// <summary>
        /// お知らせヘッダ
        /// </summary>
        public NoticeHeader NoticeHeader { get; set; }

        /// <summary>
        /// お知らせメッセージマスタ
        /// </summary>
        public NoticeIfMessage NoticeIfMessage { get; set; }

        /// <summary>
        /// 連携実行
        /// </summary>
        public IfRun IfRun { get; set; }

        /// <summary>
        /// 連携実行単位
        /// </summary>
        public IfRunUnitName IfRunUnitName { get; set; }

        /// <summary>
        /// 連携データ
        /// </summary>
        public IfData IfData { get; set; }

        /// <summary>
        /// 連携システム
        /// </summary>
        public IfSystem IfSystem { get; set; }

    }
}