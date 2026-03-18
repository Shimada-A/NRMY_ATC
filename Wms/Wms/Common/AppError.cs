namespace Mvc.Common
{
    using System;
    using NLog;
    using Wms.Areas.Log.Models;
    using Wms.Common;

    public class AppError
    {
        public static void PutLog(Exception ex)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Error(ex);
            SystemLog.WriteInfo(
                note: "アプリケーションエラー",
                logResult: LogResultClasses.Failure,
                errMessage: ex.Message,
                errStackTrace: ex.StackTrace);
        }

        public static void PutLogREF(Exception ex, string prg)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Error(ex);
            SystemLog.WriteInfo(
                note: "アプリケーションエラー" + prg,
                logResult: LogResultClasses.Failure,
                errMessage: ex.Message,
                errStackTrace: ex.StackTrace);
        }
    }
}