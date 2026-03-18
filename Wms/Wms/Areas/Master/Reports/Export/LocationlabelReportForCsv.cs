namespace Wms.Areas.Master.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Master.ViewModels.Location;
    using Wms.Common;
    using Wms.Resources;

    public class LocationlabelReportForCsv : BaseExportReport<LocationLabelReportRowForCsv>, IReportExportable<LocationLabelReportRowForCsv>
    {
        private readonly LocationSearchCondition _condition;

        public LocationlabelReportForCsv(LocationSearchCondition condition) : base(ReportTypes.Csv)
        {
            _condition = condition;
        }

        public override IEnumerable<LocationLabelReportRowForCsv> GetData()
        {
            Query.Location.Report query = new Query.Location.Report();
            IEnumerable<LocationLabelReportRowForCsv> data = query.GetResultRowList(_condition);
            return data;
        }

        public override IReportWriter<LocationLabelReportRowForCsv> GetWriter()
        {
            return new CsvWriter<LocationLabelReportRowForCsv>();
        }

        public override string GetDownloadFileName()
        {

            return string.Format(ReportResource.RPT_LOCATION_LABEL,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}