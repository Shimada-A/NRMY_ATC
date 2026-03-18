namespace Share.Common
{
    using System.Configuration;
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    /// 帳票ファイルの作成クラス
    /// </summary>
    public class CsvPrintFileCreate
    {
        /// <summary>
        /// 帳票用のCSVを作成
        /// </summary>
        /// <param name="controllerName">controller名</param>
        /// <param name="downloadFileName">ファイル名</param>
        /// <param name="fileContent">出力データ</param>
        public void CreateCsvFile(string controllerName, string downloadFileName, byte[] fileContent)
        {
            string importFilePath = Path.Combine(AppConfig.TempTaskPrintDir, controllerName, Path.GetFileName(downloadFileName));
            FileInfo fi = new FileInfo(importFilePath);
            DirectoryInfo di = fi.Directory;
            if (!di.Exists)
            {
                di.Create();
            }

            FileStream fs = new FileStream(importFilePath, FileMode.Create);
            fs.Write(fileContent, 0, fileContent.Length);
            fs.Dispose();
        }

        #region コメントアウト(OutputPDF)
        ///// <summary>
        ///// PDFを作成
        ///// </summary>
        ///// <param name="controllerName">controller名</param>
        ///// <param name="styleName">帳票定義ファイル名</param>
        ///// <param name="downloadFileName">ファイル名</param>
        ///// <returns>出力されたPDFの物理パス</returns>
        //public string OutputPDF(string controllerName, string styleName, string downloadFileName)
        //{
        //    string importFilePath = Path.Combine(AppConfig.TempTaskPrintDir, controllerName, Path.GetFileName(downloadFileName));
        //    string exportFilePath = Path.Combine(AppConfig.TempTaskPrintDir, controllerName, Path.GetFileNameWithoutExtension(downloadFileName));
        //    string workDirPath = AppConfig.CreateFormWorkDir;

        //    // パラメータ
        //    RuntimeParam param = new RuntimeParam
        //    {
        //        WorkDir = workDirPath,
        //        StyleFile = styleName,
        //        OutputFile = exportFilePath + ".pdf",
        //        //PrinterJobName = "Microsoft Print to PDF",
        //        InputData = new InputDataParam[]
        //        {
        //            new InputDataParamStandard
        //            {
        //                DataFile = importFilePath
        //            }
        //        },
        //    };

        //    // 実行
        //    CastRuntime cast = new CastRuntime
        //    {
        //        ProcessWindow = ProcessWindowStyle.CpwShow,
        //        ShownErrWindow = true,
        //    };
        //    int ret = cast.Execute(param);

        //    return ret == 1 ? param.OutputFile : ret.ToString("D");
        //}
        #endregion

        #region コメントアウト(OutputPrint)
        ///// <summary>
        ///// クライアント直接印刷専用パラメータ
        ///// </summary>
        ///// <param name="controllerName">controller名</param>
        ///// <param name="styleName">帳票定義ファイル名</param>
        ///// <param name="downloadFileName">ファイル名</param>
        ///// <returns>出力されたCCDファイルのUrlパス</returns>
        //public string OutputPrint(string controllerName, string styleName, string downloadFileName)
        //{
        //    string importFilePath = Path.Combine(AppConfig.TempTaskPrintDir, controllerName, Path.GetFileName(downloadFileName));
        //    string exportFilePath = Path.Combine(AppConfig.TempPrintCcdDir, controllerName, Path.GetFileNameWithoutExtension(downloadFileName));
        //    //string exportFilePath = Path.Combine(AppConfig.TempPrintCcdDir, controllerName, Path.GetFileName(OutputfileName));
        //    FileInfo fi = new FileInfo(exportFilePath);
        //    DirectoryInfo di = fi.Directory;
        //    if (!di.Exists)
        //    {
        //        di.Create();
        //    }
        //    string workDirPath = AppConfig.CreateFormWorkDir;

        //    //実行パラメーターの設定(クライアント直接印刷専用パラメータ)
        //    PrintStageWebExpansionParam exParam = new PrintStageWebExpansionParam();
        //    //exParam.ProhibitedSavingCcd = false;
        //    ExpansionParam[] expansions = { exParam };

        //    // パラメータ
        //    RuntimeParam param = new RuntimeParam
        //    {
        //        WorkDir = workDirPath,
        //        StyleFile = styleName,
        //        OutputFile = exportFilePath + ".ccd",
        //        Expansions = expansions,
        //        InputData = new InputDataParam[]
        //        {
        //            new InputDataParamStandard
        //            {
        //                DataFile = importFilePath
        //            }
        //        },
        //    };

        //    // 実行
        //    PrintStageWebRuntime printStageWeb = new PrintStageWebRuntime();

        //    int ret = printStageWeb.Execute(param);

        //    var urlHelper = new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext);
        //    var urlOutputFile = urlHelper.Content("~/" + ConfigurationManager.AppSettings["TempPrintCcdDir"] + "/"+ controllerName + "/" + Path.GetFileNameWithoutExtension(downloadFileName) + ".ccd");

        //    return ret == 1 ? urlOutputFile : ret.ToString("D");
        //}
        #endregion
    }
}