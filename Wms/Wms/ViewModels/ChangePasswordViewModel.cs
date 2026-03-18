namespace Wms.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Resources;

    public class ChangePasswordViewModel
    {
        /// <summary>
        /// 変更前パスワード
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LoginResource.CurrentPassword), ResourceType = typeof(LoginResource))]
        [MaxLength(256, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [DataType(DataType.Password)]
        [NoTrimAttribute]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新しいパスワード
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LoginResource.NewPassword), ResourceType = typeof(LoginResource))]
        [MaxLength(256, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [RegularExpression(Share.Common.AppConst.REGEX_ONLY_ALLOW_HALF_WIDTH, ErrorMessageResourceName = nameof(MessagesResource.OnlyAllowHalfSize), ErrorMessageResourceType = typeof(MessagesResource))]
        [DataType(DataType.Password)]
        [NoTrimAttribute]
        public string NewPassword { get; set; }

        /// <summary>
        /// 新しいパスワード確認入力
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(256, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LoginResource.NewConfirmPassword), ResourceType = typeof(LoginResource))]
        [Compare(nameof(LoginResource.NewPassword), ErrorMessageResourceName = nameof(MessagesResource.ERR_CONFIRM_PASSWORD), ErrorMessageResourceType = typeof(MessagesResource))]
        [RegularExpression(Share.Common.AppConst.REGEX_ONLY_ALLOW_HALF_WIDTH, ErrorMessageResourceName = nameof(MessagesResource.OnlyAllowHalfSize), ErrorMessageResourceType = typeof(MessagesResource))]
        [DataType(DataType.Password)]
        [NoTrimAttribute]
        public string NewConfirmPassword { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        /// <remarks>主キーの最後に荷主IDを指定したいのでOrder=99とする</remarks>
        public string ShipperId { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        /// <remarks>主キーの最後に荷主IDを指定したいのでOrder=99とする</remarks>
        public string CenterId { get; set; }

        /// <summary>
        /// ユーザID (USER_ID)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// パスワードハッシュ値 (PASSWORD_HASH)
        /// </summary>
        /// <remarks>
        /// ソルト含む値のため同一パスワード文字列でも異なる値がセットされる
        /// </remarks>
        public string PasswordHash { get; set; }

        /// <summary>
        /// 汎用値 (GEN_NAME)最小入力桁数
        /// </summary>
        public string MinLength { get; set; }
    }
}