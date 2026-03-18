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
    public class PackageUnshelvedReferenceReport : BaseExportReport<ViewModels.UnshelvedReference.PackageUnshelvedReferenceReport>, IReportExportable<ViewModels.UnshelvedReference.PackageUnshelvedReferenceReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private UnshelvedReferenceSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PackageUnshelvedReferenceReport(ReportTypes reportType, UnshelvedReferenceSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.UnshelvedReference.PackageUnshelvedReferenceReport> GetData()
        {
            Query.UnshelvedReference.Report query = new Query.UnshelvedReference.Report();
            IEnumerable<ViewModels.UnshelvedReference.PackageUnshelvedReferenceReport> data = query.PackageArrivalListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.UnshelvedReference.PackageUnshelvedReferenceReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.UnshelvedReference.PackageUnshelvedReferenceReport>(resourceKey: "RPT_ARRIVAL_UNSHELVED");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.UnshelvedReference.PackageUnshelvedReferenceReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_PACKAGE_ARRIVAL_UNSHELVED,
                Profile.User.CenterId,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}