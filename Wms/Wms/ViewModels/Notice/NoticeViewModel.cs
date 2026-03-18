namespace Wms.ViewModels.Notice
{
    using System.Collections.Generic;
    using PagedList;
    using Wms.Models;

    public class NoticeResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<NoticeResultRow> NoticeResultRows { get; set; }
    }

    /// <summary>
    /// お知らせ連携エラー明細の一覧
    /// </summary>
    public class NoticeResultDetail
    {
        public List<NoticeDetailRow> NoticeDetailRows { get; set; }
    }

    /// <summary>
    /// 受信エラーの一覧
    /// </summary>
    public class NoticeResultReceive
    {
        public List<NoticeDetailReceiveRow> NoticeReceiveRows { get; set; }
    }

    /// <summary>
    /// ポップアップ表示一覧
    /// </summary>
    public class NoticePopUpResult
    {
        public List<NoticePopUpRow> NoticePopUpRows { get; set; }
    }


    public class NoticeViewModel
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        public NoticeSearchConditions SearchConditions { get; set; }

        /// <summary>
        /// お知らせ連携エラー
        /// </summary>
        public NoticeResult Results { get; set; }

        /// <summary>
        /// ページNo
        /// </summary>
        public int Page { get; set; }

        public NoticeDetailHeader DetailHeader { get; set; }

        /// <summary>
        /// お知らせ連携エラー明細
        /// </summary>
        public NoticeResultDetail Details { get; set; }

        /// <summary>
        /// 受信エラー明細
        /// </summary>
        public NoticeResultReceive ReceiveDetails { get; set; }

        /// <summary>
        /// ポップアップ表示明細
        /// </summary>
        public NoticePopUpResult PopUpResults { get; set; }

        public NoticeViewModel()
        {
            this.SearchConditions = new NoticeSearchConditions();
            this.Results = new NoticeResult();
            this.DetailHeader = new NoticeDetailHeader();
            this.Details = new NoticeResultDetail();
            this.ReceiveDetails = new NoticeResultReceive();
            this.PopUpResults = new NoticePopUpResult();
        }
    }
}