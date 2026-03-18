namespace Wms.Areas.Master.Reports.Import
{
    using System.Collections.Generic;
    using System.Web;
    using Share.Common;
    using Share.Reports.Import;
    using Wms.Areas.Master.Query.ShipFrontage;

    public class ShipFrontageReport : BaseImportReport<ViewModels.ShipFrontage.Report>, IReportImportable<ViewModels.ShipFrontage.Report>
    {
        public ShipFrontageReport(ReportTypes reportType, HttpPostedFileBase file, string guid) : base(reportType, file, guid)
        {
        }

        /// <summary>
        /// ワークID
        /// </summary>
        public long _seq;

        public override IReportReader<ViewModels.ShipFrontage.Report> GetReader()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ShipFrontageExcelReader<ViewModels.ShipFrontage.Report>();
            }

            return null;
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <param name="reportData">レポート読み取りデータ</param>
        public override void Import(IEnumerable<ViewModels.ShipFrontage.Report> reportData)
        {
            var query = new Report();
            _seq = query.InsertWwMasShipFrontages(reportData);
        }
    }
}