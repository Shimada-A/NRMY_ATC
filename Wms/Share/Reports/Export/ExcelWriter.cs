namespace Share.Reports.Export
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using OfficeOpenXml;
    using Share.Common;

    /// <summary>
    /// Excel書き込みクラス
    /// </summary>
    /// <typeparam name="T">レポート対象Entity</typeparam>
    public class ExcelWriter<T> : IReportWriter<T>
    {
        /// <summary>
        /// テンプレートファイル
        /// </summary>
        /// <remarks>
        /// テンプレートファイルをMvc.Report.Export.ExcelTemplateフォルダから取得する
        /// </remarks>
        protected FileInfo TemplateFile
        {
            get { return this.GetTemplateFileInfo(); }
        }

        /// <summary>
        /// リソースキー
        /// </summary>
        /// <remarks>
        /// テンプレートファイルを取得する際のファイル名の一部
        /// </remarks>
        public string ResourceKey { get; }

        /// <summary>
        /// データ出力開始セル
        /// </summary>
        /// <remarks>
        /// テンプレートが異なる場合は派生クラスでoverrideすること
        /// </remarks>
        public virtual string ExportStartCell { get; set; } = "A2";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resourceKey">Share.Reports.Resources.Reports.resxのキー</param>
        public ExcelWriter(string resourceKey)
        {
            this.ResourceKey = resourceKey;
        }

        /// <summary>
        /// ダウンロードファイルストリームを取得
        /// </summary>
        /// <param name="data">出力データ</param>
        /// <returns>ダウンロードファイルストリーム</returns>
        public virtual byte[] GetReportStream(IEnumerable<T> data)
        {
            var stream = new MemoryStream();
            using (var template = System.IO.File.OpenRead(this.TemplateFile.FullName))
            using (var excel = new ExcelPackage(stream, template))
            {
                var sheet = excel.Workbook.Worksheets[1];
                sheet.Cells[this.ExportStartCell].LoadFromCollection(data, false);
                excel.Save();

                return stream.ToArray();
            }
        }

        /// <summary>
        /// テンプレートファイルのFileInfoを取得する。
        /// </summary>
        /// <returns>テンプレートファイルのFileInfo</returns>
        private FileInfo GetTemplateFileInfo()
        {
            var templateFileName = this.ResourceKey;
            var culture = CultureInfo.CurrentCulture.ToString();
            if (culture.Contains("ja"))
            {
                templateFileName += ".ja";
            }
            else if (culture.Contains("zh"))
            {
                templateFileName += ".zh";
            }

            templateFileName += ".xlsx";
            return new FileInfo(Path.Combine(AppConfig.ReportTemplateDir, templateFileName));
        }
    }
}