namespace Wms.Areas.Log.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Dapper;
    using NLog;
    using Share.Common.Resources;
    using Wms.Areas.Log.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// システムログ
    /// </summary>
    [Table("L_SYSTEM_LOG")]
    public class SystemLog
    {
        #region プロパティ

        /// <summary>
        /// 作成日時 (MAKE_DATE)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SystemLogResource.MakeDate), ResourceType = typeof(SystemLogResource))]
        public DateTimeOffset MakeDate { get; set; }

        /// <summary>
        /// 作成ユーザーID (MAKE_USER_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SystemLogResource.MakeUserId), ResourceType = typeof(SystemLogResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string MakeUserId { get; set; }

        /// <summary>
        /// 作成プログラム名 (MAKE_PROGRAM_NAME)
        /// </summary>
        /// <remarks>
        /// ストアド名。C#からの場合はプログラムID
        /// </remarks>
        [Display(Name = nameof(SystemLogResource.MakeProgramName), ResourceType = typeof(SystemLogResource))]
        [MaxLength(61, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string MakeProgramName { get; set; }

        /// <summary>
        /// 荷主ID (SHIPPER_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SystemLogResource.ShipperId), ResourceType = typeof(SystemLogResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipperId { get; set; }

        /// <summary>
        /// ID (SYSTEM_LOG_ID)
        /// </summary>
        /// <remarks>
        /// 自動連番
        /// </remarks>
        [Key]
        [Column(Order = 100)]
        [Display(Name = nameof(SystemLogResource.SystemLogId), ResourceType = typeof(SystemLogResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long? SystemLogId { get; set; }

        /// <summary>
        /// HTTPメソッド (HTTP_METHOD)
        /// </summary>
        /// <remarks>
        /// BaseControllerで登録
        /// </remarks>
        [Display(Name = nameof(SystemLogResource.HttpMethod), ResourceType = typeof(SystemLogResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string HttpMethod { get; set; }

        /// <summary>
        /// コントローラー名 (CONTROLLER_NAME)
        /// </summary>
        /// <remarks>
        /// BaseControllerで登録
        /// </remarks>
        [Display(Name = nameof(SystemLogResource.ControllerName), ResourceType = typeof(SystemLogResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ControllerName { get; set; }

        /// <summary>
        /// アクション名 (ACTION_NAME)
        /// </summary>
        /// <remarks>
        /// BaseControllerで登録
        /// </remarks>
        [Display(Name = nameof(SystemLogResource.ActionName), ResourceType = typeof(SystemLogResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ActionName { get; set; }

        /// <summary>
        /// プログラム名 (PROGRAM_NAME)
        /// </summary>
        /// <remarks>
        /// メソッド名やストアド名
        /// </remarks>
        [Display(Name = nameof(SystemLogResource.ProgramName), ResourceType = typeof(SystemLogResource))]
        [MaxLength(61, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ProgramName { get; set; }

        /// <summary>
        /// パラメーター情報 (PARAMETERS)
        /// </summary>
        /// <remarks>
        /// jsonで登録（or Key1:Value1,Key2:Value2 形式 ←Oracle12.2以降ならjson_objectを使ってjsonに統一したい）
        /// </remarks>
        [Display(Name = nameof(SystemLogResource.Parameters), ResourceType = typeof(SystemLogResource))]
        public string Parameters { get; set; }

        /// <summary>
        /// 処理件数 (ROW_COUNT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SystemLogResource.RowCount), ResourceType = typeof(SystemLogResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int RowCount { get; set; }

        /// <summary>
        /// 処理状態 (STATUS_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:開始 2:終了 (一連のアクションやプログラム単位の開始と終了)
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SystemLogResource.StatusClass), ResourceType = typeof(SystemLogResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public LogStatusClasses StatusClass { get; set; }

        /// <summary>
        /// 処理結果区分 (RESULT_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:正常 2:異常 3:警告
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SystemLogResource.ResultClass), ResourceType = typeof(SystemLogResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public LogResultClasses ResultClass { get; set; }

        /// <summary>
        /// エラーメッセージ (ERR_MESSAGE)
        /// </summary>
        [Display(Name = nameof(SystemLogResource.ErrMessage), ResourceType = typeof(SystemLogResource))]
        public string ErrMessage { get; set; }

        /// <summary>
        /// エラースタックトレース (ERR_STACK_TRACE)
        /// </summary>
        [Display(Name = nameof(SystemLogResource.ErrStackTrace), ResourceType = typeof(SystemLogResource))]
        public string ErrStackTrace { get; set; }

        /// <summary>
        /// 補足 (NOTE)
        /// </summary>
        [Display(Name = nameof(SystemLogResource.Note), ResourceType = typeof(SystemLogResource))]
        public string Note { get; set; }

        #endregion プロパティ

        #region メソッド

        /// <summary>
        /// システムログ情報登録する
        /// </summary>
        /// <returns>true:成功 false:失敗</returns>
        public async System.Threading.Tasks.Task Save()
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    // Create parameter
                    var parameters = new DynamicParameters();
                    parameters.Add(":IN_MAKE_USER_ID", MakeUserId ?? "unknown", dbType: DbType.String, direction: ParameterDirection.Input);
                    parameters.Add(":IN_SHIPPER_ID", ShipperId ?? "unknown", dbType: DbType.String, direction: ParameterDirection.Input);
                    parameters.Add(":IN_HTTP_METHOD", HttpMethod, dbType: DbType.String, direction: ParameterDirection.Input);
                    parameters.Add(":IN_CONTROLLER_NAME", ControllerName, dbType: DbType.String, direction: ParameterDirection.Input);
                    parameters.Add(":IN_ACTION_NAME", ActionName, dbType: DbType.String, direction: ParameterDirection.Input);
                    parameters.Add(":IN_PROGRAM_NAME", ProgramName, dbType: DbType.String, direction: ParameterDirection.Input);
                    parameters.Add(":IN_PARAMETERS", "", dbType: DbType.String, direction: ParameterDirection.Input);
                    parameters.Add(":IN_ROW_COUNT", RowCount, dbType: DbType.Int32, direction: ParameterDirection.Input);
                    parameters.Add(":IN_STATUS_CLASS", StatusClass, dbType: DbType.Int16, direction: ParameterDirection.Input);
                    parameters.Add(":IN_RESULT_CLASS", ResultClass, dbType: DbType.Int16, direction: ParameterDirection.Input);
                    parameters.Add(":IN_ERR_MESSAGE", ErrMessage, dbType: DbType.String, direction: ParameterDirection.Input);
                    parameters.Add(":IN_ERR_STACK_TRACE", ErrStackTrace, dbType: DbType.String, direction: ParameterDirection.Input);
                    parameters.Add(":IN_NOTE", Note, dbType: DbType.String, direction: ParameterDirection.Input);

                    // Execute insert
                    using (var db = new MvcDbContext())
                    {
                        db.Database.Connection.Execute("PK_COMM_LOG.SP_INSERT_L01", parameters, null, null, CommandType.StoredProcedure);
                    }
                });
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex, ex.Message);
            }
        }

        /// <summary>
        /// システムログ情報を情報として登録する
        /// </summary>
        /// <param name="note">補足</param>
        /// <param name="makeUserId">作成ユーザーID</param>
        /// <param name="shipperId">荷主ID</param>
        /// <param name="httpMethod">HTTPメソッド</param>
        /// <param name="controllerName">コントローラー名</param>
        /// <param name="actionName">アクション名</param>
        /// <param name="programName">プログラム名</param>
        /// <param name="parameters">パラメータ</param>
        /// <param name="rowCount">処理件数</param>
        /// <param name="logStatus">処理状態</param>
        /// <param name="logResult">処理結果</param>
        /// <param name="errMessage">エラーメッセージ</param>
        /// <param name="errStackTrace">エラースタックトレース</param>
        public static void WriteInfo(
            string note = "",
            string makeUserId = "",
            string shipperId = "",
            string httpMethod = "",
            string controllerName = "",
            string actionName = "",
            [CallerMemberName] string programName = "",
            string parameters = "",
            int rowCount = 0,
            LogStatusClasses logStatus = LogStatusClasses.None,
            LogResultClasses logResult = LogResultClasses.None,
            string errMessage = "",
            string errStackTrace = "")
        {
            var log = new SystemLog()
            {
                MakeUserId = makeUserId == "" ? Profile.User?.UserId : makeUserId,
                MakeProgramName = programName,
                ShipperId = shipperId == "" ? Profile.User?.ShipperId : shipperId,
                HttpMethod = httpMethod,
                ControllerName = controllerName,
                ActionName = actionName,
                ProgramName = programName,
                Parameters = parameters,
                RowCount = rowCount,
                StatusClass = logStatus,
                ResultClass = logResult,
                ErrMessage = errMessage,
                ErrStackTrace = errStackTrace,
                Note = note
            };

            Write(log);
            WriteLog(log);
        }

        /// <summary>
        /// システムログ情報を登録する
        /// </summary>
        /// <param name="log">システムログ</param>
        private static void Write(SystemLog log)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + $"  {log.ProgramName}\t{log.Note}\t{log.ErrMessage}");

            var parameters = new DynamicParameters();
            parameters.Add(":IN_MAKE_USER_ID", log.MakeUserId, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_MAKE_PROGRAM_NAME", log.MakeProgramName, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_SHIPPER_ID", log.ShipperId, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_HTTP_METHOD", log.HttpMethod, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_CONTROLLER_NAME", log.ControllerName, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_ACTION_NAME", log.ActionName, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_PROGRAM_NAME", log.ProgramName, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_PARAMETERS", log.Parameters, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_ROW_COUNT", log.RowCount, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add(":IN_STATUS_CLASS", log.StatusClass, dbType: DbType.Int16, direction: ParameterDirection.Input);
            parameters.Add(":IN_RESULT_CLASS", log.ResultClass, dbType: DbType.Int16, direction: ParameterDirection.Input);
            parameters.Add(":IN_ERR_MESSAGE", log.ErrMessage, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_ERR_STACK_TRACE", log.ErrStackTrace, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add(":IN_NOTE", log.Note, dbType: DbType.String, direction: ParameterDirection.Input);

            using (var context = new MvcDbContext())
            {
                var db = context.Database;
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        db.Connection.ExecuteAsync(
                            "PK_COMM_LOG.SP_INSERT_L01",
                            parameters,
                            transaction.UnderlyingTransaction,
                            commandType: CommandType.StoredProcedure);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.GetCurrentClassLogger().Warn(ex);
                    }
                }
            }
        }

        /// <summary>
        /// LOGに書き込む
        /// </summary>
        /// <param name="log">システムログ</param>
        private static void WriteLog(SystemLog log)
        {
            // イベントログ
            string source = ConfigurationManager.AppSettings["eventLogSource"].ToString();
            int eventID = int.Parse(ConfigurationManager.AppSettings["eventLogID"]);

            var type = EventLogEntryType.Error;
            if (log.ResultClass == LogResultClasses.Success)
            {
                type = EventLogEntryType.SuccessAudit;
            }
            else if (log.ResultClass == LogResultClasses.Warning)
            {
                type = EventLogEntryType.Warning;
            }

            EventLog.WriteEntry(source, log.ErrMessage.ToString(), type, eventID);
        }

        #endregion メソッド
    }
}