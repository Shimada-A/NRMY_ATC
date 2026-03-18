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
    public class ShipFrontageReportTemp : BaseExportReport<ReportTemp>, IReportExportable<ReportTemp>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private ShipFrontageSearchCondition _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public ShipFrontageReportTemp(ReportTypes reportType, ShipFrontageSearchCondition search)
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
            Query.ShipFrontage.Report query = new Query.ShipFrontage.Report();
            IEnumerable<ViewModels.ShipFrontage.ReportTemp> data = query.ShipFrontageListingTemp(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.ShipFrontage.ReportTemp> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                ExcelWriter < ViewModels.ShipFrontage.ReportTemp> export = new ExcelWriter<ViewModels.ShipFrontage.ReportTemp>(resourceKey: "RPT_SHIP_FRONTAGE_TEMP");
                return export;
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.ShipFrontage.ReportTemp>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_SHIP_FRONTAGE_TEMP,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}