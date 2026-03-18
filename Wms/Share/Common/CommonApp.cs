namespace Share.Common
{
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// アプリケーション設定クラス
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// ルートパス
        /// </summary>
        public static string RootPath { get; set; }

        /// <summary>
        /// レポートテンプレートフォルダパス
        /// </summary>
        public static string ReportTemplateDir { get; set; }

        ///// <summary>
        ///// 一時保存フォルダパス
        ///// </summary>
        // public static string TempDir { get; set; }

        /// <summary>
        /// アップロードファイルの一時保存フォルダパス
        /// </summary>
        public static string TempUploadDir { get; private set; }

        /// <summary>
        /// Gets 帳票ファイルの一時保存フォルダパス
        /// </summary>
        public static string TempTaskPrintDir { get; private set; }

        public static string TempPrintCcdDir { get; private set; }

        public static string PrintCcdUrl { get; private set; }

        /// <summary>
        /// Gets create!Form作業ディレクトリ
        /// </summary>
        public static string CreateFormWorkDir { get; private set; }

        static AppConfig()
        {
        }

        /// <summary>
        /// どこからでもWebアプリのパスを取得できるように開始時に設定する
        /// </summary>
        /// <param name="rootPath">Webアプリルート物理パス</param>
        public static void SetupPath(string rootPath)
        {
            // ルートパス
            RootPath = rootPath;

            // レポートテンプレートフォルダパス
            ReportTemplateDir = Path.Combine(rootPath, ConfigurationManager.AppSettings["ReportTemplateDir"]);

            // アップロードファイルの一時保存フォルダパス
            TempUploadDir = Path.Combine(rootPath, ConfigurationManager.AppSettings["TempUploadDir"]);
            var tempUploadDir = new DirectoryInfo(TempUploadDir);
            if (!tempUploadDir.Exists)
            {
                tempUploadDir.Create();
            }

            // 帳票ファイルの一時保存フォルダパス
#if DEBUG
            TempTaskPrintDir = Path.Combine(rootPath, ConfigurationManager.AppSettings["TempTaskPrintDir"]);
#else
            TempTaskPrintDir = ConfigurationManager.AppSettings["PhysicalTempTaskPrintDir"];
#endif
            DirectoryInfo tempTaskPrintDir = new DirectoryInfo(TempUploadDir);
            if (!tempTaskPrintDir.Exists)
            {
                tempTaskPrintDir.Create();
            }
            // CCDファイルの一時保存フォルダパス
            TempPrintCcdDir = Path.Combine(rootPath, ConfigurationManager.AppSettings["TempPrintCcdDir"]);
            DirectoryInfo tempPrintCcdDir = new DirectoryInfo(TempPrintCcdDir);
            if (!tempPrintCcdDir.Exists)
            {
                tempPrintCcdDir.Create();
            }
            // CCDファイル保存先参照URL
            PrintCcdUrl = ConfigurationManager.AppSettings["PrintCcdUrl"];

            // Create!Form作業ディレクトリ
            CreateFormWorkDir = ConfigurationManager.AppSettings["CreateFormWorkDir"];
        }

        /// <summary>
        /// DB接続文字列
        /// </summary>
        public static string DB_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;

        /// <summary>
        /// S3のバケット名（画像）
        /// </summary>
        public static string BucketImageName
            => ConfigurationManager.AppSettings["BucketImageBaseName"] + ConfigurationManager.AppSettings["BucketSuffixName"];

        /// <summary>
        /// S3のバケット名（お知らせ）
        /// </summary>
        public static string BucketMessageName
            => ConfigurationManager.AppSettings["BucketMessageBaseName"] + ConfigurationManager.AppSettings["BucketSuffixName"];
    }

    /// <summary>
    /// アプリケーション定数
    /// </summary>
    public class AppConst
    {
        /// <summary>
        /// SUCCESS string
        /// </summary>
        public const string SUCCESS = "SUCCESS";

        /// <summary>
        /// ERROR string
        /// </summary>
        public const string ERROR = "ERROR";

        /// <summary>
        /// Regular expression only allow input half width
        /// </summary>
        public const string REGEX_ONLY_ALLOW_HALF_WIDTH = "^[a-zA-Z0-9@!#\\$\\^%&*()+=\\-\\[\\]\\\\\';,_~\\.\\/\\{\\}\\|\":<>?ｧ-ﾝﾞﾟ]+$";

        /// <summary>
        /// HttpStatus
        /// </summary>
        public const string HTTP_STATUS = "HTTP_STATUS";

        /// <summary>
        ///  Overwrite record which is existed in database
        /// </summary>
        public const string QUE_UPDATE_OVERWRITE = "QUE_UPDATE_OVERWRITE";
    }

    /// <summary>
    /// レポートファイル種類
    /// </summary>
    public enum ReportTypes
    {
        Csv,
        Excel
    }

    /// <summary>
    /// プロシージャー処理結果
    /// </summary>
    public enum ProcedureStatus : int
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 異常終了
        /// </summary>
        Error = -1,

        /// <summary>
        /// 日次中
        /// </summary>
        Daily = -2,

        /// <summary>
        /// 引当中
        /// </summary>
        Alloc = -3,

        /// <summary>
        /// 棚卸中
        /// </summary>
        Inventory = -4,

        /// <summary>
        /// 他の作業者により更新済み
        /// </summary>
        AlreadySaved = -5,

        /// <summary>
        /// ビジー(リトライ)
        /// </summary>
        BusyRetry = 1,

        /// <summary>
        /// 引当対象データがありません
        /// </summary>
        NoAllocData = 2,

        /// <summary>
        /// 欠品確認
        /// </summary>
        ConfirmLoss = 20,
    }
}