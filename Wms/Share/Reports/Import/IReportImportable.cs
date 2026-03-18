namespace Share.Reports.Import
{
    using System.Collections.Generic;

    /// <summary>
    /// 取込データインターフェース
    /// </summary>
    /// <typeparam name="T">取込データのクラス</typeparam>
    public interface IReportImportable<T>
    {
        /// <summary>
        /// レポート読み込みクラス生成
        /// </summary>
        /// <returns>レポートリーダー</returns>
        IReportReader<T> GetReader();

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <param name="reportData">レポート読み取りデータ</param>
        void Import(IEnumerable<T> reportData);
    }
}
