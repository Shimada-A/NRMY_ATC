namespace Share.Reports.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Share.Common;

    /// <summary>
    /// レポート取込クラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseImportReport<T> : IReportImportable<T>
    {
        #region プライベート変数,プロパティ

        /// <summary>
        /// レポートデータクラス
        /// </summary>
        protected IReportImportable<T> ReportData { get; set; }

        /// <summary>
        /// レポートファイル種類
        /// </summary>
        protected ReportTypes ReportType { get; set; }

        /// <summary>
        /// 取込ファイル
        /// </summary>
        protected HttpPostedFileBase ImportFile { get; set; }

        /// <summary>
        /// ワークIDとしてのGUID
        /// </summary>
        protected string WorkGuid { get; set; }

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="reportData">レポートデータクラス</param>
        /// <param name="reportType">レポートファイル種類</param>
        /// <param name="file">取込ファイル</param>
        /// <param name="guid">ワークIDとしてのGUID</param>
        public BaseImportReport(ReportTypes reportType, HttpPostedFileBase file, string guid)
        {
            this.ReportType = reportType;
            this.ImportFile = file;
            this.WorkGuid = guid;
        }

        /// <summary>
        /// ファイル取込
        /// </summary>
        /// <returns></returns>
        public bool Import()
        {
            // 拡張子の検証
            if (!this.IsValidExtension())
            {
                return false;
            }

            var reader = this.GetReader();
            var reportData = reader.Read(this.ImportFile);
            this.Import(reportData);

            return true;
        }

        /// <summary>
        /// 取込検証
        /// </summary>
        /// <returns>true:成功 false:失敗</returns>
        private bool IsValid()
        {
            // 拡張子の検証
            if (!this.IsValidExtension())
            {
                return false;
            }

            // 個別の検証
            return this.CustomValid();
        }

        /// <summary>
        /// ファイル拡張子の検証
        /// </summary>
        /// <returns>true:成功 false:失敗</returns>
        public bool IsValidExtension()
        {
            var extension = System.IO.Path.GetExtension(this.ImportFile.FileName).ToLower();
            var validExtentions = new List<string>();

            if (this.ReportType == ReportTypes.Csv)
            {
                validExtentions.Add(".csv");
            }
            else if (this.ReportType == ReportTypes.Excel)
            {
                validExtentions.Add(".xlsx");
                validExtentions.Add(".xls");
            }
            else
            {
                // レポートファイル種類の指定が不正です。
            }

            return validExtentions.Contains(extension);
        }

        #region 派生クラスで実装するため未実装

        /// <summary>
        /// 個別の検証
        /// </summary>
        /// <returns></returns>
        public virtual bool CustomValid()
        {
            return true;
        }

        /// <summary>
        /// レポート読み込みクラス生成
        /// </summary>
        /// <returns>レポートリーダー</returns>
        public virtual IReportReader<T> GetReader()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <param name="reportData">レポート読み取りデータ</param>
        public virtual void Import(IEnumerable<T> reportData)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}