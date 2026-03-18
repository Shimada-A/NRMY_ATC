namespace Wms.Areas.Master.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using CsvHelper.Configuration;
    using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
    using Share.Common;
    using Share.Helpers;
    using Share.Reports.Export;
    using Wms.Areas.Master.ViewModels.User;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class UserGasReport : BaseExportReport<GasReport>, IReportExportable<GasReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private UserSearchCondition _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public UserGasReport(ReportTypes reportType, UserSearchCondition search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<GasReport> GetData()
        {
            Query.User.Report query = new Query.User.Report();
            IEnumerable<ViewModels.User.GasReport> data = query.GasListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.User.GasReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                ExcelWriter < ViewModels.User.GasReport > export = new ExcelWriter<ViewModels.User.GasReport>(resourceKey: "RPT_USER_GAS");
                return export;
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new UserCsvWriter<ViewModels.User.GasReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_USER_GAS,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }

        /// <summary>
        /// 作業者マスタCSV用 Csv書き込みクラス（shift-jisで出力する）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class UserCsvWriter<T> : IReportWriter<T>
        {
            /// <summary>
            /// ダウンロードファイルストリームを取得
            /// </summary>
            /// <param name="data">出力データ</param>
            /// <returns>ダウンロードファイルストリーム</returns>
            public byte[] GetReportStream(IEnumerable<T> data)
            {
                var map = this.GetClassMap();
                var stream = new MemoryStream();
                using (TextWriter streamWriter = new StreamWriter(stream, Encoding.GetEncoding("shift-jis")))
                using (var writer = new CsvHelper.CsvWriter(streamWriter, CultureInfo.CurrentCulture))
                {
                    writer.Configuration.RegisterClassMap(map);
                    writer.WriteRecords(data);
                    writer.Flush();
                }

                return stream.ToArray();
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
}