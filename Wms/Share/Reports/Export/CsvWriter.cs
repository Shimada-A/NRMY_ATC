namespace Share.Reports.Export
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using CsvHelper.Configuration;
    using Share.Helpers;

    /// <summary>
    /// Csv書き込みクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CsvWriter<T> : IReportWriter<T>
    {
        /// <summary>
        /// ダウンロードファイルストリームを取得
        /// </summary>
        /// <param name="data">出力データ</param>
        /// <returns>ダウンロードファイルストリーム</returns>
        public virtual byte[] GetReportStream(IEnumerable<T> data)
        {
            var map = this.GetClassMap();
            var stream = new MemoryStream();
            using (TextWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(true)))
            using (var writer = new CsvHelper.CsvWriter(streamWriter, CultureInfo.CurrentCulture))
            {
                writer.Configuration.RegisterClassMap(map);
                writer.WriteRecords(data);
                writer.Flush();
            }

            //// BOMを付けてCSVファイルをExcelで表示可能にする
            //byte[] bom = { 0xEF, 0xBB, 0xBF };
            //return bom.Concat(stream.ToArray()).ToArray();

            return stream.ToArray();
        }

        /// <summary>
        /// ModelプロパティのDisplayの設定値をヘッダーにセット
        /// </summary>
        /// <returns></returns>
        protected DefaultClassMap<T> GetClassMap()
        {
            var map = new DefaultClassMap<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var property in properties)
            {
                var newMap = MemberMap.CreateGeneric(typeof(T), property);
                newMap.Name(MetaDataHelper.GetDisplayName(typeof(T), property.Name));
                newMap.Index(MetaDataHelper.GetOrderNo(typeof(T), property.Name));
                map.MemberMaps.Add(newMap);
            }

            return map;
        }
    }
}