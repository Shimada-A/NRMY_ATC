namespace Wms.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Resources;

    /// <summary>
    /// お知らせ連携エラーヘッダ
    /// </summary>
    [Table("T_NOTICE_HEADERS")]
    public partial class NoticeHeader : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// お知らせエラーヘッダID (NOTICE_HEADER_ID)
        /// </summary>
        /// <remarks>
        /// 採番マスタ・採番管理テーブルを使用して採番
        /// シークエンス番号取得関数()で取得する
        /// メールURLに使用される
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeHeaderResource.NoticeHeaderId), ResourceType = typeof(NoticeHeaderResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string NoticeHeaderId { get; set; }

        /// <summary>
        /// 発生日時 (OCCURRENCE_DATE)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeHeaderResource.OccurrenceDate), ResourceType = typeof(NoticeHeaderResource))]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTimeOffset OccurrenceDate { get; set; }

        /// <summary>
        /// お知らせエラーID (NOTICE_IF_MESSAGE_ID)
        /// </summary>
        /// <remarks>
        /// お知らせ連携エラーメッセージマスタのID
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeHeaderResource.NoticeIfMessageId), ResourceType = typeof(NoticeHeaderResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string NoticeIfMessageId { get; set; }

        /// <summary>
        /// 発生連携実行ID (IF_RUN_ID)
        /// </summary>
        [Display(Name = nameof(NoticeHeaderResource.IfRunId), ResourceType = typeof(NoticeHeaderResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long? IfRunId { get; set; }

        /// <summary>
        /// 所在区分 (LOC_CLASS)
        /// </summary>
        /// <remarks>
        /// メール送信・表示対象となる所在区分
        /// コード一覧「所在区分」参照
        /// お知らせ連携エラーメッセージマスタから設定する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeHeaderResource.LocClass), ResourceType = typeof(NoticeHeaderResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte LocClass { get; set; }

        /// <summary>
        /// 所在ID (LOC_ID)
        /// </summary>
        /// <remarks>
        /// メール送信先として使用する所在ID
        /// レコード設定者が、所在区分によって任意に設定
        /// </remarks>
        [Display(Name = nameof(NoticeHeaderResource.LocId), ResourceType = typeof(NoticeHeaderResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocId { get; set; }

        /// <summary>
        /// 件名 (SUBJECT)
        /// </summary>
        /// <remarks>
        /// お知らせ連携エラーメッセージマスタの件名をパラメータで置換したもの
        /// データ受信時：連携データIDに対応する連携データ名を連携データマスタから取得し、設定する。画面の場合はシステムマスタから文字列取得
        /// </remarks>
        [Display(Name = nameof(NoticeHeaderResource.Subject), ResourceType = typeof(NoticeHeaderResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Subject { get; set; }

        /// <summary>
        /// 内容置換パラメータ (MESSAGE_PARAMETER)
        /// </summary>
        /// <remarks>
        /// {0}～で置換するパラメータを区切り記号(@）で連結
        /// データ受信時：連携データIDに対応する連携データ名を連携データマスタから取得し、設定する。画面の場合はシステムマスタから文字列取得
        /// </remarks>
        [Display(Name = nameof(NoticeHeaderResource.MessageParameter), ResourceType = typeof(NoticeHeaderResource))]
        [MaxLength(1000, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string MessageParameter { get; set; }

        /// <summary>
        /// メール送信フラグ (SEND_FLAG)
        /// </summary>
        /// <remarks>
        /// 1:送信済
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeHeaderResource.SendFlag), ResourceType = typeof(NoticeHeaderResource))]
        public bool SendFlag { get; set; }

        /// <summary>
        /// メール送信日時 (SEND_DATE)
        /// </summary>
        [Display(Name = nameof(NoticeHeaderResource.SendDate), ResourceType = typeof(NoticeHeaderResource))]
        public DateTimeOffset? SendDate { get; set; }

        /// <summary>
        /// 連携データID (IF_DATA_ID)
        /// </summary>
        [Display(Name = nameof(NoticeHeaderResource.IfDataId), ResourceType = typeof(NoticeHeaderResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string IfDataId { get; set; }

        /// <summary>
        /// 連携システムID (IF_SYSTEM_ID)
        /// </summary>
        [Display(Name = nameof(NoticeHeaderResource.IfSystemId), ResourceType = typeof(NoticeHeaderResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string IfSystemId { get; set; }

        #endregion
    }
}
