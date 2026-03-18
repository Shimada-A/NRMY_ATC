namespace Wms.Areas.Master.Reports.Import
{
    using System.Collections.Generic;
    using System.Web;
    using Share.Common;
    using Share.Reports.Import;
    using Wms.Areas.Master.Query.Location;

    public class LocationReport : BaseImportReport<ViewModels.Location.Report>, IReportImportable<ViewModels.Location.Report>
    {
        public LocationReport(ReportTypes reportType, HttpPostedFileBase file, string guid) : base(reportType, file, guid)
        {
        }

        /// <summary>
        /// ワークID
        /// </summary>
        public long _seq;

        public override IReportReader<ViewModels.Location.Report> GetReader()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new LocationExcelReader<ViewModels.Location.Report>();
            }

            return null;
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <param name="reportData">レポート読み取りデータ</param>
        public override void Import(IEnumerable<ViewModels.Location.Report> reportData)
        {
            var query = new Report();
            _seq = query.InsertWwMasLocations(reportData);
        }
    }
}