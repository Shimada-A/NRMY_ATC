namespace Wms.Areas.Master.Reports.Import
{
    using System.Collections.Generic;
    using System.Web;
    using Share.Common;
    using Share.Reports.Import;
    using Wms.Areas.Master.Query.LocTransporter;

    public class LocTransporterReport : BaseImportReport<ViewModels.LocTransporter.Report>, IReportImportable<ViewModels.LocTransporter.Report>
    {
        public LocTransporterReport(ReportTypes reportType, HttpPostedFileBase file, string guid) : base(reportType, file, guid)
        {
        }

        /// <summary>
        /// ワークID
        /// </summary>
        public long _seq;

        public override IReportReader<ViewModels.LocTransporter.Report> GetReader()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new LocTransporterExcelReader<ViewModels.LocTransporter.Report>();
            }

            return null;
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <param name="reportData">レポート読み取りデータ</param>
        public override void Import(IEnumerable<ViewModels.LocTransporter.Report> reportData)
        {
            var query = new Report();
            _seq = query.InsertWwMasLocTransporters(reportData);
        }
    }
}