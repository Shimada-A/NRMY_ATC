namespace Wms.Areas.Master.ViewModels.UserProgram
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;
    using Wms.Common;

    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class UserProgramList
    {
        /// <summary>
        /// 荷主ID (SHIPPER_ID)
        /// </summary>
        public string ShipperId { get; set; }

        /// <summary>
        /// プログラムID (PROGRAM_ID)
        /// </summary>
        [Display(Name = nameof(UserProgramResource.ProgramId), ResourceType = typeof(UserProgramResource))]
        public string ProgramId { get; set; }

        /// <summary>
        /// プログラム名 (PROGRAM_NAME)
        /// </summary>
        [Display(Name = nameof(UserProgramResource.ProgramName), ResourceType = typeof(UserProgramResource))]
        public string ProgramName { get; set; }

        /// <summary>
        /// プログラム区分 (PROGRAM_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:PC 2:ハンディ
        /// (以下は使用しないが暫定値 3:帳票 4:I/F 5:ハンディAPI)
        /// </remarks>
        [Display(Name = nameof(UserProgramResource.ProgramClass), ResourceType = typeof(UserProgramResource))]
        public string ProgramClass { get; set; }

        /// <summary>
        /// 権限レベル (PERMISSION_LEVEL)
        /// </summary>
        /// <remarks>
        /// 2:ユーザー管理者 3:倉庫管理者 4:事務担当者 5:現場担当者 6:パート 7:本部用
        /// </remarks>
        [Display(Name = nameof(UserProgramResource.PermissionLevel), ResourceType = typeof(UserProgramResource))]
        public ProgramPermissionLevelClasses PermissionLevel { get; set; }

        /// <summary>
        /// 使用可否区分 (USABLE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:使用不可
        /// 1:閲覧のみ
        /// 2:一部項目のみ更新可・閲覧
        /// 3:登録・更新・削除・閲覧
        /// </remarks>
        [Display(Name = nameof(UserProgramResource.UsableClass), ResourceType = typeof(UserProgramResource))]
        public int? UsableClass2 { get; set; }

        /// <summary>
        /// 使用可否区分 (USABLE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:使用不可
        /// 1:閲覧のみ
        /// 2:一部項目のみ更新可・閲覧
        /// 3:登録・更新・削除・閲覧
        /// </remarks>
        [Display(Name = nameof(UserProgramResource.UsableClass), ResourceType = typeof(UserProgramResource))]
        public int? UsableClass3 { get; set; }

        /// <summary>
        /// 使用可否区分 (USABLE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:使用不可
        /// 1:閲覧のみ
        /// 2:一部項目のみ更新可・閲覧
        /// 3:登録・更新・削除・閲覧
        /// </remarks>
        [Display(Name = nameof(UserProgramResource.UsableClass), ResourceType = typeof(UserProgramResource))]
        public int? UsableClass4 { get; set; }

        /// <summary>
        /// 使用可否区分 (USABLE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:使用不可
        /// 1:閲覧のみ
        /// 2:一部項目のみ更新可・閲覧
        /// 3:登録・更新・削除・閲覧
        /// </remarks>
        [Display(Name = nameof(UserProgramResource.UsableClass), ResourceType = typeof(UserProgramResource))]
        public int? UsableClass5 { get; set; }

        /// <summary>
        /// 使用可否区分 (USABLE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:使用不可
        /// 1:閲覧のみ
        /// 2:一部項目のみ更新可・閲覧
        /// 3:登録・更新・削除・閲覧
        /// </remarks>
        [Display(Name = nameof(UserProgramResource.UsableClass), ResourceType = typeof(UserProgramResource))]
        public int? UsableClass6 { get; set; }

        /// <summary>
        /// 使用可否区分 (USABLE_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:使用不可
        /// 1:閲覧のみ
        /// 2:一部項目のみ更新可・閲覧
        /// 3:登録・更新・削除・閲覧
        /// </remarks>
        [Display(Name = nameof(UserProgramResource.UsableClass), ResourceType = typeof(UserProgramResource))]
        public int? UsableClass7 { get; set; }

        /// <summary>
        /// 使用可否FLAG
        /// </summary>
        /// <remarks>
        /// 0:その他
        /// 1:使用不可
        /// </remarks>
        public int UsableFlg2 { get; set; } = 0;

        /// <summary>
        /// 使用可否FLAG
        /// </summary>
        /// <remarks>
        /// 0:その他
        /// 1:使用不可
        /// </remarks>
        public int UsableFlg3 { get; set; } = 0;

        /// <summary>
        /// 使用可否FLAG
        /// </summary>
        /// <remarks>
        /// 0:その他
        /// 1:使用不可
        /// </remarks>
        public int UsableFlg4 { get; set; } = 0;

        /// <summary>
        /// 使用可否FLAG
        /// </summary>
        /// <remarks>
        /// 0:その他
        /// 1:使用不可
        /// </remarks>
        public int UsableFlg5 { get; set; } = 0;

        /// <summary>
        /// 使用可否FLAG
        /// </summary>
        /// <remarks>
        /// 0:その他
        /// 1:使用不可
        /// </remarks>
        public int UsableFlg6 { get; set; } = 0;

        /// <summary>
        /// 使用可否FLAG
        /// </summary>
        /// <remarks>
        /// 0:その他
        /// 1:使用不可
        /// </remarks>
        public int UsableFlg7 { get; set; } = 0;

        /// <summary>
        /// 更新回数 (UPDATE_COUNT)
        /// </summary>
        public int UpdateCount2 { get; set; }

        /// <summary>
        /// 更新回数 (UPDATE_COUNT)
        /// </summary>
        public int UpdateCount3 { get; set; }

        /// <summary>
        /// 更新回数 (UPDATE_COUNT)
        /// </summary>
        public int UpdateCount4 { get; set; }

        /// <summary>
        /// 更新回数 (UPDATE_COUNT)
        /// </summary>
        public int UpdateCount5 { get; set; }

        /// <summary>
        /// 更新回数 (UPDATE_COUNT)
        /// </summary>
        public int UpdateCount6 { get; set; }

        /// <summary>
        /// 更新回数 (UPDATE_COUNT)
        /// </summary>
        public int UpdateCount7 { get; set; }
    }
}