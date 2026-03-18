namespace Wms.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Common;

    public class LoginViewModel
    {
        /// <summary>
        /// 荷主ID
        /// </summary>
        /// <remarks>主キーの最後に荷主IDを指定したいのでOrder=99とする</remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.LoginResource.LBL_LOGIN_SHIPPER), ResourceType = typeof(Resources.LoginResource))]
        [NoTrimAttribute]
        public string ShipperId { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        /// <remarks>主キーの最後に荷主IDを指定したいのでOrder=99とする</remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.LoginResource.LBL_LOGIN_CENTER_CODE), ResourceType = typeof(Resources.LoginResource))]
        [NoTrimAttribute]
        public string CenterId { get; set; }

        /// <summary>
        /// ユーザID (USER_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.LoginResource.LBL_LOGIN_USER), ResourceType = typeof(Resources.LoginResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [NoTrimAttribute]
        public string UserId { get; set; }

        /// <summary>
        /// ユーザ名 (USER_NAME)
        /// </summary>
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
        /// </remarks>
        public string PermissionLevel { get; set; }

        /// <summary>
        /// 所在名(略称) (CENTER_NAME)
        /// </summary>
        public string CenterShortName { get; set; }

        /// <summary>
        /// パスワードハッシュ値 (PASSWORD_HASH)
        /// </summary>
        /// <remarks>
        /// ソルト含む値のため同一パスワード文字列でも異なる値がセットされる
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.LoginResource.LBL_LOGIN_PASSWORD), ResourceType = typeof(Resources.LoginResource))]
        [MaxLength(256, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [NoTrimAttribute]
        public string PasswordHash { get; set; }

        /// <summary>
        /// パスワード更新日時 (PASSWORD_UPDATE_DATE)
        /// </summary>
        public DateTime? PasswordUpdateDate { get; set; }

        /// <summary>
        /// パスワード更新ユーザーID (PASSWORD_UPDATE_USER_ID)
        /// </summary>
        public string PasswordUpdateUserId { get; set; }

        /// <summary>
        /// パスワード入力ミス回数 (PASSWORD_MISTYPE_COUNT)
        /// </summary>
        public int? PasswordMistypeCount { get; set; }

        /// <summary>
        /// １つ前パスワード (PASSWORD_HASH1)
        /// </summary>
        public string PasswordHash1 { get; set; }

        /// <summary>
        /// ２つ前パスワード (PASSWORD_HASH2)
        /// </summary>
        public string PasswordHash2 { get; set; }

        /// <summary>
        /// ３つ前パスワード (PASSWORD_HASH3)
        /// </summary>
        public string PasswordHash3 { get; set; }

        /// <summary>
        /// 失効区分 (USER_LAPSE)
        /// </summary>
        public byte? UserLapse { get; set; }

        /// <summary>
        /// 汎用値 (GEN_NAME)
        /// </summary>
        public string GenName { get; set; }

        /// <summary>
        /// 汎用値 (GEN_NAME)最小入力桁数
        /// </summary>
        public string MinLength { get; set; }

        /// <summary>
        /// データ作成区分
        /// </summary>
        public string DateMakeClass { get; set; }
    }
}