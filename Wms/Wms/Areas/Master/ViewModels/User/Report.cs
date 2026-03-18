namespace Wms.Areas.Master.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;

    public class Report
    {
        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Display(Name = nameof(UserResource.CenterIdDownload), ResourceType = typeof(UserResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// 倉庫名
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Display(Name = nameof(UserResource.CenterNameDownload), ResourceType = typeof(UserResource), Order = 2)]
        public string CenterName { get; set; }

        /// <summary>
        /// ユーザID (USER_ID)
        /// </summary>
        [Display(Name = nameof(UserResource.UserId), ResourceType = typeof(UserResource), Order = 3)]
        public string UserId { get; set; }

        /// <summary>
        /// ユーザ名 (USER_NAME)
        /// </summary>
        [Display(Name = nameof(UserResource.UserName), ResourceType = typeof(UserResource), Order = 4)]
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
        [Display(Name = nameof(UserResource.PermissionLevel), ResourceType = typeof(UserResource), Order = 5)]
        public string PermissionLevel { get; set; }

        /// <summary>
        /// 権限レベル 名称
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Display(Name = nameof(UserResource.PermissionLevelNameDownload), ResourceType = typeof(UserResource), Order = 6)]
        public string PermissionLevelName { get; set; }


    }
}