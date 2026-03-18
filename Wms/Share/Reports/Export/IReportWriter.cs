namespace Share.Reports.Export
{
    using System.Collections.Generic;

    /// <summary>
    /// レポート書き込みインターフェース
    /// </summary>
    /// <typeparam name="T">出力データのクラス</typeparam>
    public interface IReportWriter<T>
    {
        /// <summary>
        /// ダウンロードファイルストリームを取得
        /// </summary>
        /// <param name="data">出力データ</param>
        /// <returns>ダウンロードファイルストリーム</returns>
        byte[] GetReportStream(IEnumerable<T> data);
    }
}
