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
    public class TotalPicking : BaseExportReport<ViewModels.PrintBatch.TotalPicking>, IReportExportable<ViewModels.PrintBatch.TotalPicking>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PrintBatchSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public TotalPicking(ReportTypes reportType, PrintBatchSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.PrintBatch.TotalPicking> GetData()
        {
            Query.PrintBatch.Report query = new Query.PrintBatch.Report();
            IEnumerable<ViewModels.PrintBatch.TotalPicking> data = query.GetTotalPicking(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.PrintBatch.TotalPicking> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.PrintBatch.TotalPicking>(resourceKey: "RPT_STOCK");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.PrintBatch.TotalPicking>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_TOTAL_PICKING,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}