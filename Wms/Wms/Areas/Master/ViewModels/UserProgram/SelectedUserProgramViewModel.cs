namespace Wms.Areas.Master.ViewModels.UserProgram
{
    using Wms.Common;

    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class SelectedUserProgramViewModel
    {
        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        /// <remarks>主キーの最後に荷主IDを指定したいのでOrder=99とする</remarks>
        public string ShipperId { get; set; }

        /// <summary>
        /// プログラムID
        /// </summary>
        public string ProgramId { get; set; }

        /// <summary>
        /// プログラム区分
        /// </summary>
        public string ProgramClass { get; set; }

        /// <summary>
        /// プログラム名
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// 権限レベル
        /// </summary>
        public ProgramPermissionLevelClasses PermissionLevel { get; set; }

        /// <summary>
        /// 使用可否区分
        /// </summary>
        public UsableClasses UsableClass2 { get; set; }

        /// <summary>
        /// 使用可否区分
        /// </summary>
        public UsableClasses UsableClass3 { get; set; }

        /// <summary>
        /// 使用可否区分
        /// </summary>
        public UsableClasses UsableClass4 { get; set; }

        /// <summary>
        /// 使用可否区分
        /// </summary>
        public UsableClasses UsableClass5 { get; set; }

        /// <summary>
        /// 使用可否区分
        /// </summary>
        public UsableClasses UsableClass6 { get; set; }

        /// <summary>
        /// 使用可否区分
        /// </summary>
        public UsableClasses UsableClass7 { get; set; }

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
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount2 { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount3 { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount4 { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount5 { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount6 { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount7 { get; set; }
    }
}