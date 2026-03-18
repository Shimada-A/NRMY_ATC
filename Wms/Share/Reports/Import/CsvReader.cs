namespace Share.Reports.Import
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Share.Helpers;

    /// <summary>
    /// CSVリーダー
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CsvReader<T> : IReportReader<T>
    {
        /// <summary>
        /// レポート読み込み
        /// </summary>
        /// <returns>読み込みデータ</returns>
        public IEnumerable<T> Read(FileInfo file)
        {
            var reportData = new List<T>();
            var map = this.GetClassMap();

            using (TextReader textReader = new StreamReader(file.FullName))
            using (var parse = new CsvHelper.CsvParser(textReader, CultureInfo.CurrentCulture))
            {
                var reader = new CsvReader(parse);
                reader.Configuration.RegisterClassMap(map);
                reportData = reader.GetRecords<T>().ToList();
            }

            return reportData;
        }

        /// <summary>
        /// ModelプロパティのDisplayの設定値をヘッダーにセット
        /// </summary>
        /// <returns></returns>
        private DefaultClassMap<T> GetClassMap()
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