namespace Wms.Areas.Arrival.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Arrival.ViewModels.PrintCaseLabel;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class PrintCaseLabelJanCsv : BaseExportReport<ViewModels.PrintCaseLabel.PrintCaseLabelCsv>, IReportExportable<ViewModels.PrintCaseLabel.PrintCaseLabelCsv>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PrintCaseLabelConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PrintCaseLabelJanCsv(ReportTypes reportType, PrintCaseLabelConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.PrintCaseLabel.PrintCaseLabelCsv> GetData()
        {
            Query.PrintCaseLabel.Report query = new Query.PrintCaseLabel.Report();
            IEnumerable<ViewModels.PrintCaseLabel.PrintCaseLabelCsv> data = query.PrintCaseLabelJanListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.PrintCaseLabel.PrintCaseLabelCsv> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.PrintCaseLabel.PrintCaseLabelCsv>(resourceKey: "RPT_PRINTCASELABEL");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.PrintCaseLabel.PrintCaseLabelCsv>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_ARRIVAL_CASE_LABEL,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}