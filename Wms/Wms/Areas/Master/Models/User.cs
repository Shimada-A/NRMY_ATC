namespace Wms.Areas.Master.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// 利用者
    /// </summary>
    [Table("M_USERS")]
    public partial class User : BaseModel
    {
        #region プロパティ
        /// <summary>
        /// ユーザID (USER_ID)
        /// </summary>
        /// <remarks>
        /// IFユーザーマスタ.ユーザーID[文字(10)]
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UserResource.UserId), ResourceType = typeof(UserResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string UserId { get; set; }

        /// <summary>
        /// ユーザ名 (USER_NAME)
        /// </summary>
        /// <remarks>
        /// IFユーザーマスタ.ユーザー名[文字(20)]

        /// ハンディの場合：担当者名は先頭から全角５文字分のみ表示
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UserResource.UserName), ResourceType = typeof(UserResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string UserName { get; set; }

        /// <summary>
        /// 権限レベル (PERMISSION_LEVEL)
        /// </summary>
        /// <remarks>
        /// 1.システム管理者
        /// 2.ユーザー管理者
        /// 3.倉庫管理者
        /// 4.事務担当者
        /// 5.現場担当者
        /// 6.パート
        /// 7.本部用　　・・・参照のみ
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UserResource.PermissionLevel), ResourceType = typeof(UserResource))]
        //[Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public PermissionLevelClasses PermissionLevel { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード

        /// IFユーザーマスタ.倉庫コード[文字(3)]

        /// 受信時nullの場合は「101」を設定

        /// メインセンター　権限レベルによって画面でセンター変更できるかどうかが決まる。画面でセンターを変更できる権限については、汎用コードマスタに持つ
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UserResource.CenterId), ResourceType = typeof(UserResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// パスワード (PASSWORD_HASH)
        /// </summary>
        /// <remarks>
        /// IFユーザーマスタ.パスワード[文字(100)]

        /// 受信時nullの場合は　PASSWORD　のハッシュ値（MD5）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UserResource.PasswordHash), ResourceType = typeof(UserResource))]
        [MaxLength(256, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PasswordHash { get; set; }

        /// <summary>
        /// パスワード更新日時 (PASSWORD_UPDATE_DATE)
        /// </summary>
        /// <remarks>
        /// パスワードを変更した日時
        /// </remarks>
        [Display(Name = nameof(UserResource.PasswordUpdateDate), ResourceType = typeof(UserResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? PasswordUpdateDate { get; set; }

        /// <summary>
        /// パスワード更新ユーザーID (PASSWORD_UPDATE_USER_ID)
        /// </summary>
        /// <remarks>
        /// パスワードを変更したユーザ
        /// </remarks>
        [Display(Name = nameof(UserResource.PasswordUpdateUserId), ResourceType = typeof(UserResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PasswordUpdateUserId { get; set; }

        /// <summary>
        /// パスワード入力ミス回数 (PASSWORD_MISTYPE_COUNT)
        /// </summary>
        /// <remarks>
        /// パスワードを間違えた回数　正規ログイン時にクリア
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UserResource.PasswordMistypeCount), ResourceType = typeof(UserResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int PasswordMistypeCount { get; set; }

        /// <summary>
        /// １つ前パスワード (PASSWORD_HASH1)
        /// </summary>
        /// <remarks>
        /// パスワード更新時は「パスワード」で更新
        /// </remarks>
        [Display(Name = nameof(UserResource.PasswordHash1), ResourceType = typeof(UserResource))]
        [MaxLength(256, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PasswordHash1 { get; set; }

        /// <summary>
        /// ２つ前パスワード (PASSWORD_HASH2)
        /// </summary>
        /// <remarks>
        /// パスワード更新時は「１つ前パスワード」で更新
        /// </remarks>
        [Display(Name = nameof(UserResource.PasswordHash2), ResourceType = typeof(UserResource))]
        [MaxLength(256, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PasswordHash2 { get; set; }

        /// <summary>
        /// ３つ前パスワード (PASSWORD_HASH3)
        /// </summary>
        /// <remarks>
        /// パスワード更新時は「２つ前パスワード」で更新
        /// </remarks>
        [Display(Name = nameof(UserResource.PasswordHash3), ResourceType = typeof(UserResource))]
        [MaxLength(256, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PasswordHash3 { get; set; }

        /// <summary>
        /// 失効区分 (USER_LAPSE)
        /// </summary>
        /// <remarks>
        /// 0：初期化　1：有効　9：失効
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UserResource.UserLapse), ResourceType = typeof(UserResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte UserLapse { get; set; }

        /// <summary>
        /// データ作成区分 (DATE_MAKE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0：WMS　1：基幹I/F
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UserResource.DateMakeClass), ResourceType = typeof(UserResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte DateMakeClass { get; set; }

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UserResource.DeleteFlag), ResourceType = typeof(UserResource))]
        public int DeleteFlag { get; set; }

        #endregion
    }
}