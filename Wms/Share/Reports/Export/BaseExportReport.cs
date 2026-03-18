namespace Share.Reports.Export
{
    using System.Collections.Generic;
    using System.Web;
    using Share.Common;

    /// <summary>
    /// レポート基底クラス
    /// </summary>
    /// <typeparam name="T">レポート対象Entity</typeparam>
    public class BaseExportReport<T> : IReportExportable<T>
    {
        #region プライベート変数,プロパティ

        /// <summary>
        /// レポートファイル種類
        /// </summary>
        protected ReportTypes ReportType { get; private set; }

        /// <summary>
        /// コンテンツタイプ
        /// </summary>
        public string ContentType
        {
            get
            {
                // MimeTypeがIISに登録されていなければWeb.configを使って追加する。
                // https://msdn.microsoft.com/ja-jp/library/ee431622.aspx?f=255&MSPPError=-2147217396
                return MimeMapping.GetMimeMapping(this.DownloadFileName);
            }
        }

        /// <summary>
        /// ダウンロードファイルストリーム
        /// </summary>
        public byte[] FileContent { get; private set; }

        /// <summary>
        /// Gets or sets ダウンロードファイル名
        /// </summary>
        private string _downloadFileName { get; set; }

        /// <summary>
        /// ダウンロードファイル名
        /// </summary>
        public string DownloadFileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._downloadFileName))
                {
                    this._downloadFileName = this.GetDownloadFileName() + this.GetDownloadExtension();
                }

                return this._downloadFileName;
            }
        }

        /// <summary>
        /// 出力が成功したらtrue
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return this.FileContent != null;
            }
        }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get; private set; }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="reportType">レポートファイル種類</param>
        public BaseExportReport(ReportTypes reportType)
        {
            this.ReportType = reportType;
        }

        /// <summary>
        /// ファイル出力
        /// </summary>
        public void Export()
        {
            var data = this.GetData();

            // TODO 出力データに対して、最大出力件数などの検証をする
            // if (data.Count > 100000)
            // {
            //    ErrorMessage = Mvc.Common.Resources.Messages.ERR_XXXXXX;
            //    return;
            // }
            var writer = this.GetWriter();
            this.FileContent = writer.GetReportStream(data);
        }

        /// <summary>
        /// ダウンロードするファイル拡張子を取得
        /// </summary>
        /// <returns></returns>
        private string GetDownloadExtension()
        {
            if (this.ReportType == ReportTypes.Csv)
            {
                return ".csv";
            }
            else
            {
                return ".xlsx";
            }
        }

        #region 派生クラスで実装するため未実装

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <param name="reportType">Excel or Csv</param>
        /// <returns></returns>
        public virtual IReportWriter<T> GetWriter()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public virtual IEnumerable<T> GetData()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名</returns>
        public virtual string GetDownloadFileName()
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}