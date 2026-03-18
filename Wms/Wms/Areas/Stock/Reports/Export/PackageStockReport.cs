namespace Wms.Areas.Stock.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Stock.ViewModels.Reference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class PackageStockReport : BaseExportReport<ViewModels.Reference.ReferenceReportPackage>, IReportExportable<ViewModels.Reference.ReferenceReportPackage>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private ReferenceSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PackageStockReport(ReportTypes reportType, ReferenceSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.Reference.ReferenceReportPackage> GetData()
        {
            Query.Reference.Report query = new Query.Reference.Report();
            IEnumerable<ViewModels.Reference.ReferenceReportPackage> data = query.PackageStockListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.Reference.ReferenceReportPackage> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.Reference.ReferenceReportPackage>(resourceKey: "RPT_PACKAGE_STOCK");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.Reference.ReferenceReportPackage>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_PACKAGE_STOCK,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}