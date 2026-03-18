namespace Share.Reports.Export
{
    using System.Collections.Generic;

    /// <summary>
    /// 出力データインターフェース
    /// </summary>
    /// <typeparam name="T">出力データのクラス</typeparam>
    public interface IReportExportable<T>
    {
        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <param name="reportType">Excel or Csv</param>
        /// <returns></returns>
        IReportWriter<T> GetWriter();

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        IEnumerable<T> GetData();

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns></returns>
        string GetDownloadFileName();
    }
}
