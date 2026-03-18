namespace Wms.Areas.Master.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;

    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class UserList
    {
        /// <summary>
        /// 更新回数 (UPDATE_COUNT)
        /// </summary>
        public int UpdateCount { get; set; }

        /// <summary>
        /// 荷主ID (SHIPPER_ID)
        /// </summary>
        public string ShipperId { get; set; }

        /// <summary>
        /// ユーザID (USER_ID)
        /// </summary>
        [Display(Name = nameof(UserResource.UserId), ResourceType = typeof(UserResource))]
        public string UserId { get; set; }

        /// <summary>
        /// ユーザ名 (USER_NAME)
        /// </summary>
        [Display(Name = nameof(UserResource.UserName), ResourceType = typeof(UserResource))]
        public string UserName { get; set; }

        /// <summary>
        /// 更新プログラム名
        /// </summary>
        public string UpdateProgramName { get; set; }

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
        [Display(Name = nameof(UserResource.PermissionLevel), ResourceType = typeof(UserResource))]
        public string PermissionLevel { get; set; }

        /// <summary>
        /// 権限レベル (PERMISSION_LEVEL_NAME)
        /// </summary>
        [Display(Name = nameof(UserResource.PermissionLevel), ResourceType = typeof(UserResource))]
        public string PermissionLevelName { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Display(Name = nameof(UserResource.CenterId), ResourceType = typeof(UserResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 所在名 (LOC_NAME)
        /// </summary>
        [Display(Name = nameof(UserResource.CenterId), ResourceType = typeof(UserResource))]
        public string CenterName { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Display(Name = nameof(UserResource.DeleteFlag), ResourceType = typeof(UserResource))]
        public int DeleteFlag { get; set; }

        /// <summary>
        /// データ作成区分(DATE_MAKE_CLASS)
        /// </summary>
        /// <remarks>
        /// [データ作成区分]　0：WMS　1：基幹I/F
        /// </remarks>
        [Display(Name = nameof(UserResource.DateMakeClass), ResourceType = typeof(UserResource))]
        public int DateMakeClass { get; set; }

        /// <summary>
        /// チェック状態
        /// </summary>
        public bool IsCheck { get; set; }

        public string Rid { get; set; }
    }
}