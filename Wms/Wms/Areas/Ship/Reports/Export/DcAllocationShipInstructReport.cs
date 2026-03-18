namespace Wms.Areas.Ship.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.DcAllocation;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class DcAllocationShipInstructReport : BaseExportReport<ViewModels.DcAllocation.DcAllocationShipInstructReport>, IReportExportable<ViewModels.DcAllocation.DcAllocationShipInstructReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private DcAllocationSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public DcAllocationShipInstructReport(ReportTypes reportType, DcAllocationSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.DcAllocation.DcAllocationShipInstructReport> GetData()
        {
            Query.DcAllocation.Report query = new Query.DcAllocation.Report();
            IEnumerable<ViewModels.DcAllocation.DcAllocationShipInstructReport> data = query.DcAllocationShipInstructListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.DcAllocation.DcAllocationShipInstructReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.DcAllocation.DcAllocationShipInstructReport>(resourceKey: "RPT_DC_ALLOCATION_INSTRUCT_ID");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.DcAllocation.DcAllocationShipInstructReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_DC_ALLOCATION_INSTRUCT_ID,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}