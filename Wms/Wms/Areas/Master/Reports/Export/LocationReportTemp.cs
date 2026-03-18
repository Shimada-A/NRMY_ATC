namespace Wms.Areas.Master.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Master.ViewModels.Location;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class LocationReportTemp : BaseExportReport<ReportTemp>, IReportExportable<ReportTemp>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private LocationSearchCondition _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public LocationReportTemp(ReportTypes reportType, LocationSearchCondition search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ReportTemp> GetData()
        {
            Query.Location.Report query = new Query.Location.Report();
            IEnumerable<ViewModels.Location.ReportTemp> data = query.LocationListingTemp(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.Location.ReportTemp> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                ExcelWriter < ViewModels.Location.ReportTemp> export = new ExcelWriter<ViewModels.Location.ReportTemp>(resourceKey: "RPT_LOCATION_TEMP");
                return export;
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.Location.ReportTemp>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_LOCATION_TEMP,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}