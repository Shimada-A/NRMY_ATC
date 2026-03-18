namespace Wms.Areas.Master.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Master.ViewModels.Store;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class StoreReport : BaseExportReport<Report>, IReportExportable<Report>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private StoreSearchCondition _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public StoreReport(ReportTypes reportType, StoreSearchCondition search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<Report> GetData()
        {
            Query.Store.Report query = new Query.Store.Report();
            IEnumerable<Report> data = query.Listing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<Report> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                ExcelWriter<Report> report = new ExcelWriter<Report>(resourceKey: "RPT_STORE");
                report.ExportStartCell = "A2";
                return report;
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<Report>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_STORE,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}