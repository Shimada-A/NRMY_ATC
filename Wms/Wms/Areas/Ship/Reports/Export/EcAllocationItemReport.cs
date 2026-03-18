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
    public class EcAllocationItemReport : BaseExportReport<ViewModels.EcAllocation.EcAllocationItemReport>, IReportExportable<ViewModels.EcAllocation.EcAllocationItemReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private EcAllocationSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public EcAllocationItemReport(ReportTypes reportType, EcAllocationSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.EcAllocation.EcAllocationItemReport> GetData()
        {
            Query.EcAllocation.Report query = new Query.EcAllocation.Report();
            IEnumerable<ViewModels.EcAllocation.EcAllocationItemReport> data = query.EcAllocationItemListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.EcAllocation.EcAllocationItemReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.EcAllocation.EcAllocationItemReport>(resourceKey: "RPT_EC_ALLOCATION_ITEM");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.EcAllocation.EcAllocationItemReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_EC_ALLOCATION_ITEM,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}