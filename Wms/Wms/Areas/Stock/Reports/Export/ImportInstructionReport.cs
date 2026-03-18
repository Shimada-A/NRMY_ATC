namespace Wms.Areas.Stock.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Stock.ViewModels.ImportInstruction;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class ImportInstructionReport : BaseExportReport<ViewModels.ImportInstruction.ImportInstructionReport>, IReportExportable<ViewModels.ImportInstruction.ImportInstructionReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private ImportInstructionSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public ImportInstructionReport(ReportTypes reportType, ImportInstructionSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.ImportInstruction.ImportInstructionReport> GetData()
        {
            Query.ImportInstruction.Report query = new Query.ImportInstruction.Report();
            IEnumerable<ViewModels.ImportInstruction.ImportInstructionReport> data = query.ImportInstructionListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.ImportInstruction.ImportInstructionReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.ImportInstruction.ImportInstructionReport>(resourceKey: "RPT_STOCK_IMPORTINSTRUCTION");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.ImportInstruction.ImportInstructionReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_STOCK_IMPORTINSTRUCTION,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}