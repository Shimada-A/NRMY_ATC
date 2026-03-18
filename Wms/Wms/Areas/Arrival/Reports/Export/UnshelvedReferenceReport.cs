namespace Wms.Areas.Arrival.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Arrival.ViewModels.UnshelvedReference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class UnshelvedReferenceReport : BaseExportReport<ViewModels.UnshelvedReference.UnshelvedReferenceReport>, IReportExportable<ViewModels.UnshelvedReference.UnshelvedReferenceReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private UnshelvedReferenceSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public UnshelvedReferenceReport(ReportTypes reportType, UnshelvedReferenceSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.UnshelvedReference.UnshelvedReferenceReport> GetData()
        {
            Query.UnshelvedReference.Report query = new Query.UnshelvedReference.Report();
            IEnumerable<ViewModels.UnshelvedReference.UnshelvedReferenceReport> data = query.ArrivalListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.UnshelvedReference.UnshelvedReferenceReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.UnshelvedReference.UnshelvedReferenceReport>(resourceKey: "RPT_ARRIVAL_UNSHELVED");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.UnshelvedReference.UnshelvedReferenceReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_ARRIVAL_UNSHELVED,
                Profile.User.CenterId,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}