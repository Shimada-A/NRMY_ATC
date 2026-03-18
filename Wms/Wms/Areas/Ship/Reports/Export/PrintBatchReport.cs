namespace Wms.Areas.Ship.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.PrintBatch;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class PrintBatchReport : BaseExportReport<ViewModels.PrintBatch.PrintBatchReport>, IReportExportable<ViewModels.PrintBatch.PrintBatchReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PrintBatchSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PrintBatchReport(ReportTypes reportType, PrintBatchSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.PrintBatch.PrintBatchReport> GetData()
        {
            Query.PrintBatch.Report query = new Query.PrintBatch.Report();
            IEnumerable<ViewModels.PrintBatch.PrintBatchReport> data = query.GetPrintBatchReport(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.PrintBatch.PrintBatchReport> GetWriter()
        {
            if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.PrintBatch.PrintBatchReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            var reportname = string.Empty;
            if (this._search.PrintFlag =="Ec")
            {
                reportname = ReportResource.RPT_EC_BATCH_LIST;
            }else if (this._search.PrintFlag == "Dc")
            {
                reportname = ReportResource.RPT_DC_BATCH_LIST;
            }else
            {
                reportname = ReportResource.RPT_CASE_SHIP_LIST;
            }

            return string.Format(reportname,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}