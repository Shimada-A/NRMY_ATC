namespace Wms.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Resources;

    /// <summary>
    /// お知らせ連携エラーメッセージ
    /// </summary>
    [Table("M_NOTICE_IF_MESSAGES")]
    public partial class NoticeIfMessage : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// お知らせエラーID (NOTICE_IF_MESSAGE_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeIfMessageResource.NoticeIfMessageId), ResourceType = typeof(NoticeIfMessageResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string NoticeIfMessageId { get; set; }

        /// <summary>
        /// メッセージ区分 (MESSAGE_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:エラー
        /// 2:警告
        /// 3:お知らせ
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeIfMessageResource.MessageClass), ResourceType = typeof(NoticeIfMessageResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte MessageClass { get; set; }

        /// <summary>
        /// 詳細表示フラグ (VIEW_FLAG)
        /// </summary>
        /// <remarks>
        /// 1:表示する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeIfMessageResource.ViewFlag), ResourceType = typeof(NoticeIfMessageResource))]
        public bool ViewFlag { get; set; }

        /// <summary>
        /// メール送信フラグ (SEND_FLAG)
        /// </summary>
        /// <remarks>
        /// 1:送信する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeIfMessageResource.SendFlag), ResourceType = typeof(NoticeIfMessageResource))]
        public bool SendFlag { get; set; }

        /// <summary>
        /// 所在区分 (LOC_CLASS)
        /// </summary>
        /// <remarks>
        /// メール送信・表示対象となる所在区分
        /// コード一覧「所在区分」参照
        /// </remarks>
        [Display(Name = nameof(NoticeIfMessageResource.LocClass), ResourceType = typeof(NoticeIfMessageResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte? LocClass { get; set; }

        /// <summary>
        /// 件名 (SUBJECT)
        /// </summary>
        /// <remarks>
        /// {0}～でパラメータ置換可
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeIfMessageResource.Subject), ResourceType = typeof(NoticeIfMessageResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Subject { get; set; }

        /// <summary>
        /// メッセージ内容 (MESSAGE)
        /// </summary>
        /// <remarks>
        /// {0}～でパラメータ置換可
        /// </remarks>
        [Display(Name = nameof(NoticeIfMessageResource.Message), ResourceType = typeof(NoticeIfMessageResource))]
        [MaxLength(1000, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Message { get; set; }

        /// <summary>
        /// 遷移先 (DESTINATION)
        /// </summary>
        /// <remarks>
        /// URLの　/{エリア名}/{コントローラー名}/{アクション名}/　部分
        /// </remarks>
        [Display(Name = nameof(NoticeIfMessageResource.Destination), ResourceType = typeof(NoticeIfMessageResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Destination { get; set; }

        #endregion
    }
}
