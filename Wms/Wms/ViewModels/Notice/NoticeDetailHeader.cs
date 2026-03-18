namespace Wms.ViewModels.Notice
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class NoticeDetailHeader
    {
        public int RowId { get; set; }

        /// <summary>
        /// チェック
        /// </summary>
        public bool IsCheck { get; set; } = false;

        /// <summary>
        /// 行No
        /// </summary>
        public int RowNo { get; set; } = 1;

        /// <summary>
        /// 種別
        /// </summary>
        public string MessageClassNm { get; set; }

        /// <summary>
        /// コード
        /// </summary>
        public string NoticeHeaderId { get; set; }

        /// <summary>
        /// 発生日時
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTimeOffset OccurrenceDate { get; set; }

        /// <summary>
        /// 連携実行単位名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 連携データ名
        /// </summary>
        public string IfDataName { get; set; }

        /// <summary>
        /// 連携システム名
        /// </summary>
        public string IfSystemName { get; set; }

        /// <summary>
        /// 件名
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// メッセージ内容(マスタ)
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 内容置換パラメータ
        /// </summary>
        public string MessageParameter { get; set; }

        /// <summary>
        /// お知らせエラーID
        /// </summary>
        public string NoticeIfMessageId { get; set; }

        /// <summary>
        /// テーブル名(受信系用)
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 連携実行ID
        /// </summary>
        public long IfRunId { get; set; }

        /// <summary>
        /// 連携実行状態区分
        /// </summary>
        public byte IfRunState { get; set; }

        /// <summary>
        /// 連携実行状態区分名
        /// </summary>
        public string IfRunStateNm { get; set; }

        /// <summary>
        /// 開始時間 (START_TIME)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTimeOffset StartTime { get; set; }

        /// <summary>
        /// 終了時間 (END_TIME)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// 詳細表示フラグ 
        /// </summary>
        public bool ViewFlag { get; set; }


    }
}