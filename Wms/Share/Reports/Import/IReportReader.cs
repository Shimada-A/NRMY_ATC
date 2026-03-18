namespace Share.Reports.Import
{
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReportReader<T>
    {
        /// <summary>
        /// 取込ファイルを読み込み、マッピングしたモデルリストを返す
        /// </summary>
        /// <param name="file">取込ファイル</param>
        /// <returns>マッピングしたモデルリスト</returns>
        IEnumerable<T> Read(HttpPostedFileBase file);
    }
}
