namespace Wms.Areas.Arrival.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Arrival.ViewModels.PurchaseReference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class PurchaseReference02DetailReport : BaseExportReport<ViewModels.PurchaseReference.PurchaseReference02DetailReport>, IReportExportable<ViewModels.PurchaseReference.PurchaseReference02DetailReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PurchaseReference02SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PurchaseReference02DetailReport(ReportTypes reportType, PurchaseReference02SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.PurchaseReference.PurchaseReference02DetailReport> GetData()
        {
            Query.PurchaseReference.Report query = new Query.PurchaseReference.Report();
            IEnumerable<ViewModels.PurchaseReference.PurchaseReference02DetailReport> data = query.PurchaseReference02DetailListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.PurchaseReference.PurchaseReference02DetailReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.PurchaseReference.PurchaseReference02DetailReport>(resourceKey: "RPT_PURCHASE_REFERENCE02");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.PurchaseReference.PurchaseReference02DetailReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_PURCHASE_REFERENCE02,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}