namespace Wms.Areas.Others.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Others.ViewModels.WorkReference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class WorkReferenceReport : BaseExportReport<ViewModels.WorkReference.WorkReferenceReport>, IReportExportable<ViewModels.WorkReference.WorkReferenceReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private WorkReferenceSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public WorkReferenceReport(ReportTypes reportType, WorkReferenceSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.WorkReference.WorkReferenceReport> GetData()
        {
            Query.WorkReference.Report query = new Query.WorkReference.Report();
            IEnumerable<ViewModels.WorkReference.WorkReferenceReport> data = query.WorkReferenceListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.WorkReference.WorkReferenceReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.WorkReference.WorkReferenceReport>(resourceKey: "RPT_WORK_REFERENCE");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.WorkReference.WorkReferenceReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_WORK_REFERENCE,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}