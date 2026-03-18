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
    public class UploadCaseErrReportJan : BaseExportReport<ViewModels.UploadCaseInstruction.UploadCaseErrReportJan>, IReportExportable<ViewModels.UploadCaseInstruction.UploadCaseErrReportJan>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private UploadCaseInstructionSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public UploadCaseErrReportJan(ReportTypes reportType, UploadCaseInstructionSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.UploadCaseInstruction.UploadCaseErrReportJan> GetData()
        {
            Query.UploadCaseInstruction.Report query = new Query.UploadCaseInstruction.Report();
            IEnumerable<ViewModels.UploadCaseInstruction.UploadCaseErrReportJan> data = query.UploadCaseErrListingJan(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.UploadCaseInstruction.UploadCaseErrReportJan> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                    return new ExcelWriter<ViewModels.UploadCaseInstruction.UploadCaseErrReportJan>(resourceKey: "RPT_STOCK_ERR_CASE_INSTRUCTION_JAN");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.UploadCaseInstruction.UploadCaseErrReportJan>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
                return string.Format(ReportResource.RPT_STOCK_ERR_CASE_INSTRUCTION_JAN,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}