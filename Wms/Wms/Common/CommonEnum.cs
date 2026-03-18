namespace Wms.Common
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Resources;
    using Wms.Areas.Master.Resources;

    /// <summary>
    /// Result of validate login
    /// </summary>
    public enum ResultValidateUserLogin : byte
    {
        OK = 0,

        [Display(Name = nameof(CommonResource.ERR_PASSWORD_DEFAULT), ResourceType = typeof(CommonResource))]
        ERR_PASSWORD_DEFAULT = 1,

        [Display(Name = nameof(CommonResource.ERR_LOGIN_MENU), ResourceType = typeof(CommonResource))]
        ERR_LOGIN_MENU = 2,

        [Display(Name = nameof(CommonResource.ERR_PASSWORD_EXPIRED), ResourceType = typeof(CommonResource))]
        ERR_PASSWORD_EXPIRED = 3,

        [Display(Name = nameof(CommonResource.ERR_PASSWORD_MISTYPE1), ResourceType = typeof(CommonResource))]
        ERR_PASSWORD_MISTYPE1 = 4,

        [Display(Name = nameof(CommonResource.WAR_PASSWORD_EXPIRED), ResourceType = typeof(CommonResource))]
        WAR_PASSWORD_EXPIRED = 5,

        [Display(Name = nameof(CommonResource.ERR_ACCOUNT_EXPIRATION), ResourceType = typeof(CommonResource))]
        ERR_ACCOUNT_EXPIRATION = 6,

        [Display(Name = nameof(CommonResource.ERR_PASSWORD_MISTYPE2), ResourceType = typeof(CommonResource))]
        ERR_PASSWORD_MISTYPE2 = 7,

        [Display(Name = nameof(CommonResource.ERR_PASSWORD), ResourceType = typeof(CommonResource))]
        ERR_PASSWORD = 8,
    }

    /// <summary>
    /// 発注状況 0：未発注 1：発注済み
    /// </summary>
    public enum OrderStates : byte
    {
        [Display(Name = nameof(CommonResource.UnOrdered), ResourceType = typeof(CommonResource))]
        UnOrdered = 0,

        [Display(Name = nameof(CommonResource.Ordered), ResourceType = typeof(CommonResource))]
        Ordered = 1,
    }

    /// <summary>
    /// 初回配分状況 0：未配分 1：配分済み
    /// </summary>
    public enum FirstDbStates : byte
    {
        [Display(Name = nameof(CommonResource.Unallocated), ResourceType = typeof(CommonResource))]
        Unallocated = 0,

        [Display(Name = nameof(CommonResource.Allocated), ResourceType = typeof(CommonResource))]
        Allocated = 1,
    }

    /// <summary>
    /// 閾値区分1:率(%) 2:ケース数
    /// （汎用ある）
    /// </summary>
    public enum ThresholdClasses : byte
    {
        [Display(Name = nameof(CommonResource.None), ResourceType = typeof(CommonResource))]
        None = 0,

        [Display(Name = nameof(CommonResource.ThresholdRate), ResourceType = typeof(CommonResource))]
        ThresholdRate = 1,

        [Display(Name = nameof(CommonResource.ThresholdSku), ResourceType = typeof(CommonResource))]
        ThresholdSku = 2,
    }

    /// <summary>
    /// 権限レベル 1：システム管理者　2：ユーザー管理者　3：倉庫管理者　4：事務担当者　5：現場担当者　6：パート　7:本部用
    /// （汎用ある）
    /// </summary>
    public enum PermissionLevelClasses : byte
    {
        [Display(Name = nameof(CommonResource.PermissionLevelNonPermission), ResourceType = typeof(CommonResource))]
        PermissionLevelNonPermission = 0,

        [Display(Name = nameof(CommonResource.PermissionLevelSystemManager), ResourceType = typeof(CommonResource))]
        PermissionLevelSystemManager = 1,

        [Display(Name = nameof(CommonResource.PermissionLevelUserManager), ResourceType = typeof(CommonResource))]
        PermissionLevelUserManager = 2,

        [Display(Name = nameof(CommonResource.PermissionLevelWarehouseManager), ResourceType = typeof(CommonResource))]
        PermissionLevelWarehouseManager = 3,

        [Display(Name = nameof(CommonResource.PermissionLevelClericalWorker), ResourceType = typeof(CommonResource))]
        PermissionLevelClericalWorker = 4,

        [Display(Name = nameof(CommonResource.PermissionLevelWarehouseWorker), ResourceType = typeof(CommonResource))]
        PermissionLevelWarehouseWorker = 5,

        [Display(Name = nameof(CommonResource.PermissionLevelPartTimeJobber), ResourceType = typeof(CommonResource))]
        PermissionLevelPartTimeJobber = 6,

        [Display(Name = nameof(CommonResource.PermissionLevelHeadOfficeWorker), ResourceType = typeof(CommonResource))]
        PermissionLevelHeadOfficeWorker = 7,
    }

    /// <summary>
    /// 権限レベル 2：ユーザー管理者　3：倉庫管理者　4：事務担当者　5：現場担当者　6：パート　7:本部用
    /// （汎用ある）
    /// </summary>
    public enum ProgramPermissionLevelClasses : byte
    {
        [Display(Name = nameof(CommonResource.None), ResourceType = typeof(CommonResource))]
        PermissionLevelNonPermission = 0,

        [Display(Name = nameof(CommonResource.PermissionLevelUserManager), ResourceType = typeof(CommonResource))]
        PermissionLevelUserManager = 2,

        [Display(Name = nameof(CommonResource.PermissionLevelWarehouseManager), ResourceType = typeof(CommonResource))]
        PermissionLevelWarehouseManager = 3,

        [Display(Name = nameof(CommonResource.PermissionLevelClericalWorker), ResourceType = typeof(CommonResource))]
        PermissionLevelClericalWorker = 4,

        [Display(Name = nameof(CommonResource.PermissionLevelWarehouseWorker), ResourceType = typeof(CommonResource))]
        PermissionLevelWarehouseWorker = 5,

        [Display(Name = nameof(CommonResource.PermissionLevelPartTimeJobber), ResourceType = typeof(CommonResource))]
        PermissionLevelPartTimeJobber = 6,

        [Display(Name = nameof(CommonResource.PermissionLevelHeadOfficeWorker), ResourceType = typeof(CommonResource))]
        PermissionLevelHeadOfficeWorker = 7,
    }

    /// <summary>
    /// 名義変更タイミング 1:荷主固定 2:入荷時名義変更 3:出荷時名義変更
    /// </summary>
    public enum ChangeAssetTimingEnum : byte
    {
        [Display(Name = nameof(CommonResource.CHANGE_ASSET_TIMING_FIXED), ResourceType = typeof(CommonResource))]
        Fixed = 1,

        [Display(Name = nameof(CommonResource.CHANGE_ASSET_TIMING_INCHANGE), ResourceType = typeof(CommonResource))]
        InChange = 2,

        [Display(Name = nameof(CommonResource.CHANGE_ASSET_TIMING_OUTCHANGE), ResourceType = typeof(CommonResource))]
        OutChange = 3,
    }

    /// <summary>
    /// 支払区分 1:当月 2:翌月 3:翌々月
    /// </summary>
    public enum PaymentClassEnum : byte
    {
        [Display(Name = nameof(CommonResource.PAYMENT_CLASS_THIS), ResourceType = typeof(CommonResource))]
        ThisMonth = 1,

        [Display(Name = nameof(CommonResource.PAYMENT_CLASS_NEXT), ResourceType = typeof(CommonResource))]
        NextMonth = 2,

        [Display(Name = nameof(CommonResource.PAYMENT_CLASS_AFTER), ResourceType = typeof(CommonResource))]
        AfterNext = 3,
    }

    /// <summary>
    /// 支払種別 1:振込、2:手形、3:現金
    /// </summary>
    public enum PaymentKindClassEnum : byte
    {
        [Display(Name = nameof(CommonResource.PAYMENT_KIND_CLASS_TRANSFER), ResourceType = typeof(CommonResource))]
        Transfer = 1,

        [Display(Name = nameof(CommonResource.PAYMENT_KIND_CLASS_BILLS), ResourceType = typeof(CommonResource))]
        Bills = 2,

        [Display(Name = nameof(CommonResource.PAYMENT_KIND_CLASS_CASH), ResourceType = typeof(CommonResource))]
        Cash = 3,
    }

    /// <summary>
    /// プログラム区分 1:PC 2:ハンディ (以下は使用しないが暫定値 3:帳票 4:I/F 5:ハンディAPI)
    /// （汎用ある）
    /// </summary>
    public enum ProgramClasses : byte
    {
        [Display(Name = nameof(CommonResource.None), ResourceType = typeof(CommonResource))]
        ProgramNone = 0,

        [Display(Name = nameof(CommonResource.ProgramPc), ResourceType = typeof(CommonResource))]
        ProgramPc = 1,

        [Display(Name = nameof(CommonResource.ProgramHandy), ResourceType = typeof(CommonResource))]
        ProgramHandy = 2,

        [Display(Name = nameof(CommonResource.ProgramReport), ResourceType = typeof(CommonResource))]
        ProgramReport = 3,

        [Display(Name = nameof(CommonResource.ProgramIOrF), ResourceType = typeof(CommonResource))]
        ProgramIOrF = 4,

        [Display(Name = nameof(CommonResource.ProgramHandyApi), ResourceType = typeof(CommonResource))]
        ProgramHandyApi = 5,
    }

    /// <summary>
    /// 使用可否区分 0:使用不可 1:閲覧のみ 2:一部項目のみ更新可・閲覧 3:登録・更新・削除・閲覧
    /// （汎用ある）
    /// </summary>
    public enum UsableClasses : byte
    {
        [Display(Name = nameof(CommonResource.UsableNone), ResourceType = typeof(CommonResource))]
        UsableNone = 0,

        [Display(Name = nameof(CommonResource.UsableOnlyBrowse), ResourceType = typeof(CommonResource))]
        UsableOnlyBrowse = 1,

        [Display(Name = nameof(CommonResource.UsableEditBrowse), ResourceType = typeof(CommonResource))]
        UsableEditBrowse = 2,

        [Display(Name = nameof(CommonResource.UsableAll), ResourceType = typeof(CommonResource))]
        UsableAll = 3,
    }

    /// <summary>
    /// 失効区分 0：初期、1：可、9：失効
    /// </summary>
    public enum UserLapses : byte
    {
        [Display(Name = nameof(CommonResource.Initialization), ResourceType = typeof(CommonResource))]
        Initialization = 0,

        [Display(Name = nameof(CommonResource.Enabled), ResourceType = typeof(CommonResource))]
        Enabled = 1,

        [Display(Name = nameof(CommonResource.Revocation), ResourceType = typeof(CommonResource))]
        Revocation = 9,
    }

    /// <summary>
    /// 引当対象区分 0:引当不可、1:ECのみ可（格付けはSのみ）、2:店のみ可（格付けはA,S）、3:EC,店可（格付けはSのみ）
    /// （汎用ある）
    /// </summary>
    public enum AllocSplyClassEnum : byte
    {
        [Display(Name = nameof(CommonResource.ALLOC_SPLY_CLASS_NOT), ResourceType = typeof(CommonResource))]
        Not = 0,

        [Display(Name = nameof(CommonResource.ALLOC_SPLY_CLASS_EC), ResourceType = typeof(CommonResource))]
        Ec = 1,

        [Display(Name = nameof(CommonResource.ALLOC_SPLY_CLASS_STORE), ResourceType = typeof(CommonResource))]
        Store = 2,

        [Display(Name = nameof(CommonResource.ALLOC_SPLY_CLASS_ALL), ResourceType = typeof(CommonResource))]
        All = 3,
    }

    /// <summary>
    /// 荷姿区分 1:ケース、2:バラ、9:指定なし
    /// （汎用ある）
    /// </summary>
    public enum CaseClassEnum : byte
    {
        [Display(Name = nameof(CommonResource.CASE_CLASS_BOX), ResourceType = typeof(CommonResource))]
        Box = 1,

        [Display(Name = nameof(CommonResource.CASE_CLASS_BULK), ResourceType = typeof(CommonResource))]
        Bulk = 2,

        [Display(Name = nameof(CommonResource.CASE_CLASS_NONE), ResourceType = typeof(CommonResource))]
        None = 9,
    }

    /// <summary>
    /// 確定フラグ 1：未確定、2：仮確定、3：本確定
    /// </summary>
    public enum ConfirmFlagEnum : byte
    {
        [Display(Name = nameof(CommonResource.CONFIRM_FLAG_NOT), ResourceType = typeof(CommonResource))]
        Not = 1,

        [Display(Name = nameof(CommonResource.CONFIRM_FLAG_TEMP), ResourceType = typeof(CommonResource))]
        Temp = 2,

        [Display(Name = nameof(CommonResource.CONFIRM_FLAG_TRUE), ResourceType = typeof(CommonResource))]
        True = 3,
    }

    /// <summary>
    /// 明細種別 0：在庫明細 1：ケース明細
    /// </summary>
    public enum ResultTypes : byte
    {
        [Display(Name = nameof(CommonResource.Stock), ResourceType = typeof(CommonResource))]
        Stock = 0,

        [Display(Name = nameof(CommonResource.PackageStock), ResourceType = typeof(CommonResource))]
        PackageStock = 1,
    }

    /// <summary>
    /// 検索種別 0：検索 1：並べ替え/改ページ
    /// </summary>
    public enum SearchTypes : byte
    {
        [Display(Name = nameof(CommonResource.Search), ResourceType = typeof(CommonResource))]
        Search = 0,

        [Display(Name = nameof(CommonResource.SortPage), ResourceType = typeof(CommonResource))]
        SortPage = 1,
    }

    /// <summary>
    /// シーズン 1:春, 2:春夏, 3:夏, 5:秋, 6:秋冬, 7:冬, 9:通年
    /// </summary>
    public enum ItemSeasons : byte
    {
        [Display(Name = nameof(CommonResource.None), ResourceType = typeof(CommonResource))]
        None = 0,

        [Display(Name = nameof(CommonResource.Spring), ResourceType = typeof(CommonResource))]
        Spring = 1,

        [Display(Name = nameof(CommonResource.SprSum), ResourceType = typeof(CommonResource))]
        SprSum = 2,

        [Display(Name = nameof(CommonResource.Summer), ResourceType = typeof(CommonResource))]
        Summer = 3,

        [Display(Name = nameof(CommonResource.Autumn), ResourceType = typeof(CommonResource))]
        Autumn = 5,

        [Display(Name = nameof(CommonResource.AutWin), ResourceType = typeof(CommonResource))]
        AutWin = 6,

        [Display(Name = nameof(CommonResource.Winter), ResourceType = typeof(CommonResource))]
        Winter = 7,

        [Display(Name = nameof(CommonResource.Year), ResourceType = typeof(CommonResource))]
        Year = 9,
    }

    /// <summary>
    /// 配送指定日 0:指定日なし 1:全て 2:指定日あり
    /// </summary>
    public enum TransporterDateClasses : byte
    {
        [Display(Name = nameof(CommonResource.NoDesignatedDate), ResourceType = typeof(CommonResource))]
        NoDesignatedDate = 0,

        [Display(Name = nameof(CommonResource.All), ResourceType = typeof(CommonResource))]
        All = 1,

        [Display(Name = nameof(CommonResource.WithDesignatedDate), ResourceType = typeof(CommonResource))]
        WithDesignatedDate = 2,
    }

    //ログ処理状態
    public enum LogStatusClasses : byte
    {
        None = 0,
        Start = 1,
        End = 2
    }

    //ログ処理状態
    public enum LogResultClasses : byte
    {
        None = 0,
        Success = 1,
        Failure = 2,
        Warning = 3
    }

    /// <summary>
    /// 明細種別 0：入荷明細 1：ケース別
    /// </summary>
    public enum ArrivalTypes : byte
    {
        [Display(Name = nameof(CommonResource.Arrival), ResourceType = typeof(CommonResource))]
        Arrival = 0,

        [Display(Name = nameof(CommonResource.PackageArrival), ResourceType = typeof(CommonResource))]
        PackageArrival = 1,
    }

    /// <summary>
    /// 出荷区分 TC出荷、DC出荷、EC出荷、ケース出荷
    /// </summary>
    public enum ShipKinds : byte
    {
        [Display(Name = nameof(CommonResource.Tc), ResourceType = typeof(CommonResource))]
        Tc = 0,

        [Display(Name = nameof(CommonResource.Dc), ResourceType = typeof(CommonResource))]
        Dc = 1,

        [Display(Name = nameof(CommonResource.Ec), ResourceType = typeof(CommonResource))]
        Ec = 2,

        [Display(Name = nameof(CommonResource.Case), ResourceType = typeof(CommonResource))]
        Case = 3,
    }

    /// <summary>
    /// 指示区分
    /// </summary>
    public enum InstructClasses
    {
        [Display(Name = nameof(CommonResource.FirstDb), ResourceType = typeof(CommonResource))]
        FirstDb = 6,

        [Display(Name = nameof(CommonResource.FollowDb), ResourceType = typeof(CommonResource))]
        FollowDb = 7
    }

    public enum IoClass
    {
        /// <summary>
        /// 未指定
        /// </summary>
        [Display(Name = nameof(EditLayoutResource.None), ResourceType = typeof(EditLayoutResource))]
        None = 0,

        /// <summary>
        /// 取込
        /// </summary>
        [Display(Name = nameof(EditLayoutResource.Import), ResourceType = typeof(EditLayoutResource))]
        Import = 1,

        /// <summary>
        /// 出力
        /// </summary>
        [Display(Name = nameof(EditLayoutResource.Output), ResourceType = typeof(EditLayoutResource))]
        Output = 2
    }

    public enum EncodeType
    {
        /// <summary>
        /// shift-jis
        /// </summary>
        [Display(Name = nameof(CommonResource.ShiftJis), ResourceType = typeof(CommonResource))]
        ShiftJis = 1,
        /// <summary>
        /// Utf8
        /// </summary>
        [Display(Name = nameof(CommonResource.Utf8), ResourceType = typeof(CommonResource))]
        Utf8 = 2
    }

    public enum EncloseType
    {
        /// <summary>
        /// なし
        /// </summary>
        [Display(Name = nameof(CommonResource.NotAvailable), ResourceType = typeof(CommonResource))]
        NotAvailable = 0,
        /// <summary>
        /// あり
        /// </summary>
        [Display(Name = nameof(CommonResource.Available), ResourceType = typeof(CommonResource))]
        Available = 1
    }

    public enum TitleClass
    {
        /// <summary>
        /// なし
        /// </summary>
        [Display(Name = nameof(CommonResource.NotAvailable), ResourceType = typeof(CommonResource))]
        NotAvailable = 0,
        /// <summary>
        /// あり
        /// </summary>
        [Display(Name = nameof(CommonResource.Available), ResourceType = typeof(CommonResource))]
        Available = 1
    }

    public enum HeadingRow
    {
        /// <summary>
        /// なし
        /// </summary>
        [Display(Name = nameof(CommonResource.NotAvailable), ResourceType = typeof(CommonResource))]
        Not = 0,
        /// <summary>
        /// あり
        /// </summary>
        [Display(Name = nameof(CommonResource.Available), ResourceType = typeof(CommonResource))]
        Available = 1
    }

    public enum PadDirection
    {

        /// <summary>
        /// 左埋め
        /// </summary>
        [Display(Name = nameof(CommonResource.PadLeft), ResourceType = typeof(CommonResource))]
        PadLeft = 1,
        /// <summary>
        /// 右埋め
        /// </summary>
        [Display(Name = nameof(CommonResource.PadRight), ResourceType = typeof(CommonResource))]
        PadRight = 2
    }

    public enum DataType
    {
        /// <summary>
        /// 文字列
        /// </summary>
        [Display(Name = nameof(CommonResource.String), ResourceType = typeof(CommonResource))]
#pragma warning disable CA1720 // 識別子に型名が含まれます
        String = 0,
#pragma warning restore CA1720 // 識別子に型名が含まれます

        /// <summary>
        /// 数値
        /// </summary>
        [Display(Name = nameof(CommonResource.Number), ResourceType = typeof(CommonResource))]
        Number = 1,

        /// <summary>
        /// 日付
        /// 日付
        /// </summary>
        [Display(Name = nameof(CommonResource.Date), ResourceType = typeof(CommonResource))]
        Date = 2
    }

    public enum FileType
    {
        /// <summary>
        /// CSV
        /// </summary>
        [Display(Name = nameof(CommonResource.Csv), ResourceType = typeof(CommonResource))]
        CSV = 0,

        /// <summary>
        /// TSV
        /// </summary>
        [Display(Name = nameof(CommonResource.Tsv), ResourceType = typeof(CommonResource))]
        TSV = 1,

        /// <summary>
        /// 固定値
        /// 日付
        /// </summary>
        [Display(Name = nameof(CommonResource.Fixed), ResourceType = typeof(CommonResource))]
        Fixed = 2
    }

    public enum FileTypeImport
    {
        /// <summary>
        /// CSV
        /// </summary>
        [Display(Name = nameof(CommonResource.Csv), ResourceType = typeof(CommonResource))]
        CSV = 0,

        /// <summary>
        /// TSV
        /// </summary>
        [Display(Name = nameof(CommonResource.Tsv), ResourceType = typeof(CommonResource))]
        TSV = 1
    }


    public enum ConditionClass
    {
        /// <summary>
        /// 制約無し
        /// </summary>
        [Display(Name = nameof(CommonResource.NoCondition), ResourceType = typeof(CommonResource))]
        None = 0,
        /// <summary>
        /// イコール
        /// </summary>
        [Display(Name = nameof(CommonResource.Equal), ResourceType = typeof(CommonResource))]
        Equal = 1,
        /// <summary>
        /// ノットイコール
        /// </summary>
        [Display(Name = nameof(CommonResource.NotEqual), ResourceType = typeof(CommonResource))]
        NotEqual = 2,
        /// <summary>
        /// 以上
        /// </summary>
        [Display(Name = nameof(CommonResource.GreaterEqual), ResourceType = typeof(CommonResource))]
        GreaterEqual = 3,
        /// <summary>
        /// 以下
        /// </summary>
        [Display(Name = nameof(CommonResource.LessEqual), ResourceType = typeof(CommonResource))]
        LessEqual = 4,
        /// <summary>
        /// 範囲
        /// </summary>
        [Display(Name = nameof(CommonResource.Range), ResourceType = typeof(CommonResource))]
        Range = 5,
        /// <summary>
        /// 曖昧
        /// </summary>
        [Display(Name = nameof(CommonResource.Like), ResourceType = typeof(CommonResource))]
        Like = 6,
        /// <summary>
        ///  当日
        /// </summary>
        [Display(Name = nameof(CommonResource.Today), ResourceType = typeof(CommonResource))]
        Today = 7,
        /// <summary>
        ///  当月
        /// </summary>
        [Display(Name = nameof(CommonResource.ThisMonth), ResourceType = typeof(CommonResource))]
        ThisMonth = 8,
    }

    public enum SortDirection
    {
        /// <summary>
        /// 昇順
        /// </summary>
        [Display(Name = nameof(CommonResource.Asc), ResourceType = typeof(CommonResource))]
        Asc = 1,
        /// <summary>
        /// 降順
        /// </summary>
        [Display(Name = nameof(CommonResource.Desc), ResourceType = typeof(CommonResource))]
        Desc = 2
    }

    /// <summary>
    /// ダイアログの種類
    /// </summary>
    public enum DialogType
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Other = 4,
        Csv = 5
    }

    public enum UpdateClass
    {
        /// <summary>
        /// 更新不可
        /// </summary>
        [Display(Name = nameof(CommonResource.DisabledUpdating), ResourceType = typeof(CommonResource))]
        DisabledUpdating = 0,
        /// <summary>
        ///  更新可能
        /// </summary>
        [Display(Name = nameof(CommonResource.EnabledUpdating), ResourceType = typeof(CommonResource))]
        EnabledUpdating = 1
    }

    public enum ImportClass
    {

        /// <summary>
        /// ファイル
        /// </summary>
        [Display(Name = nameof(CommonResource.File), ResourceType = typeof(CommonResource))]
        File = 0,

        /// <summary>
        /// 固定値
        /// </summary>
        [Display(Name = nameof(CommonResource.FixedValue), ResourceType = typeof(CommonResource))]
        FixedValue = 1,
    }


}