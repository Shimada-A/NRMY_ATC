namespace Wms.Areas.Ships.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.EcConfirmProgress;
    using Wms.Areas.Ships.ViewModels.EcConfirmProgress;
    using Wms.Common;
    using Wms.Resources;

    public class EcConfirmProgressReportForCsv : BaseExportReport<EcConfirmProgressReportRowForCsv>, IReportExportable<EcConfirmProgressReportRowForCsv>
    {
        private readonly EcConfirmProgress02SearchConditions _condition;

        public EcConfirmProgressReportForCsv(EcConfirmProgress02SearchConditions condition) : base(ReportTypes.Csv)
        {
            _condition = condition;
        }

        public override IEnumerable<EcConfirmProgressReportRowForCsv> GetData()
        {
            Ship.Query.EcConfirmProgress.Report query = new Ship.Query.EcConfirmProgress.Report();
            IEnumerable<EcConfirmProgressReportRowForCsv> data = query.GetResultRowList(_condition);
            return data;
        }

        public override IReportWriter<EcConfirmProgressReportRowForCsv> GetWriter()
        {
            return new CsvWriter<EcConfirmProgressReportRowForCsv>();
        }

        public override string GetDownloadFileName()
        {

            return string.Format(ReportResource.RPT_EC_CONFIRMPROGRESS,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}