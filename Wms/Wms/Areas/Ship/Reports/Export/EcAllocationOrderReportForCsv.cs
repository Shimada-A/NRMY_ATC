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
    public class EcAllocationOrderReportForCsv : BaseExportReport<ViewModels.EcAllocation.EcAllocationOrderForCsv>, IReportExportable<ViewModels.EcAllocation.EcAllocationOrderForCsv>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private EcAllocationSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public EcAllocationOrderReportForCsv(ReportTypes reportType, EcAllocationSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.EcAllocation.EcAllocationOrderForCsv> GetData()
        {
            Query.EcAllocation.Report query = new Query.EcAllocation.Report();
            return query.EcAllocationOrderListingForCsv(this._search);
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.EcAllocation.EcAllocationOrderForCsv> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.EcAllocation.EcAllocationOrderForCsv>(resourceKey: "RPT_EC_ALLOCATION_ORDER");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.EcAllocation.EcAllocationOrderForCsv>();
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