namespace Wms.Areas.Inventory.Reports.Import
{
    using System.Collections.Generic;
    using System.Web;
    using Share.Common;
    using Share.Reports.Import;
    using Wms.Areas.Inventory.Query.Start;

    public class StartReport : BaseImportReport<ViewModels.Start.StartReport>, IReportImportable<ViewModels.Start.StartReport>
    {
        public StartReport(ReportTypes reportType, HttpPostedFileBase file, string guid) : base(reportType, file, guid)
        {
        }

        /// <summary>
        /// ワークID
        /// </summary>
        public long _seq;
        public string _message;

        public override IReportReader<ViewModels.Start.StartReport> GetReader()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new StartExcelReader<ViewModels.Start.StartReport>();
            }

            return null;
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <param name="reportData">レポート読み取りデータ</param>
        public override void Import(IEnumerable<ViewModels.Start.StartReport> reportData)
        {
            var query = new Report();
            query.InsertWW_INV_START_01(reportData,out _message,out _seq);
        }
    }
}