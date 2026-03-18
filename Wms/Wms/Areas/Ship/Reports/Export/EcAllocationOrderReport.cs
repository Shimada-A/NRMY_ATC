namespace Wms.Areas.Ship.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.EcAllocation;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class EcAllocationOrderReport : BaseExportReport<ViewModels.EcAllocation.EcAllocationOrderReport>, IReportExportable<ViewModels.EcAllocation.EcAllocationOrderReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private EcAllocationSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public EcAllocationOrderReport(ReportTypes reportType, EcAllocationSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.EcAllocation.EcAllocationOrderReport> GetData()
        {
            Query.EcAllocation.Report query = new Query.EcAllocation.Report();
            IEnumerable<ViewModels.EcAllocation.EcAllocationOrderReport> data = query.EcAllocationOrderListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.EcAllocation.EcAllocationOrderReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.EcAllocation.EcAllocationOrderReport>(resourceKey: "RPT_EC_ALLOCATION_ORDER");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.EcAllocation.EcAllocationOrderReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_EC_ALLOCATION_ORDER,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}