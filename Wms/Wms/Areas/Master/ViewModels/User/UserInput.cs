namespace Wms.Areas.Master.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Wms.Resources;

    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class UserInput 
    {
        public Models.User User { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.UserResource.PasswordHash), ResourceType = typeof(Resources.UserResource))]
        public string InPassword { get; set; }

        [Compare("InPassword")]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resources.UserResource.PasswordHashConfirm), ResourceType = typeof(Resources.UserResource))]
        public string InPasswordConfirm { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = nameof(Resources.UserResource.PasswordHash), ResourceType = typeof(Resources.UserResource))]
        public string UpPassword { get; set; }

        [Compare("UpPassword")]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Resources.UserResource.PasswordHashConfirm), ResourceType = typeof(Resources.UserResource))]
        public string UpPasswordConfirm { get; set; }

        public string InUpDiff { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Display(Name = nameof(Resources.UserResource.DeleteFlag), ResourceType = typeof(Resources.UserResource))]
        public int DeleteFlag { get; set; }

        /// <summary>
        /// データ作成区分(DATE_MAKE_CLASS)
        /// </summary>
        /// <remarks>
        /// [データ作成区分]　0：WMS　1：基幹I/F
        /// </remarks>
        [Display(Name = nameof(Resources.UserResource.DateMakeClass), ResourceType = typeof(Resources.UserResource))]
        public int DateMakeClass { get; set; }

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }

    }
}