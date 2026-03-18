namespace Wms.Areas.Ship.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.UploadCaseInstruction;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class UploadCaseInstructionReportCase : BaseExportReport<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase>, IReportExportable<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private UploadCaseInstructionSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public UploadCaseInstructionReportCase(ReportTypes reportType, UploadCaseInstructionSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase> GetData()
        {
            Query.UploadCaseInstruction.Report query = new Query.UploadCaseInstruction.Report();
            IEnumerable<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase> data = query.UploadCaseInstructionListingCase();

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                   return new ExcelWriter<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase>(resourceKey: "RPT_STOCK_UPLOAD_CASE_INSTRUCTION_CASE");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
                return string.Format(ReportResource.RPT_STOCK_UPLOAD_CASE_INSTRUCTION_CASE,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                this._search.HidCenterId,
                Profile.User.UserId);
        }
    }
}