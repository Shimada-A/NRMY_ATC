namespace Wms.Areas.Master.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Master.ViewModels.ShipFrontage;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class ShipFrontageReport : BaseExportReport<Report>, IReportExportable<Report>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private ShipFrontageSearchCondition _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public ShipFrontageReport(ReportTypes reportType, ShipFrontageSearchCondition search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<Report> GetData()
        {
            Query.ShipFrontage.Report query = new Query.ShipFrontage.Report();
            IEnumerable<ViewModels.ShipFrontage.Report> data = query.ShipFrontageListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.ShipFrontage.Report> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                ExcelWriter < ViewModels.ShipFrontage.Report > export = new ExcelWriter<ViewModels.ShipFrontage.Report>(resourceKey: "RPT_SHIP_FRONTAGE");
                return export;
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.ShipFrontage.Report>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_SHIP_FRONTAGE,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}