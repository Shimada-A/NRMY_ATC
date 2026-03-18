namespace Wms.Areas.Move.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Move.ViewModels.TransferReference;
    using Wms.Common;
    using Wms.Resources;

    public class TransferreferenceReportForCsv : BaseExportReport<TransferReferenceReportRowForCsv>, IReportExportable<TransferReferenceReportRowForCsv>
    {
        private readonly TransferReference02SearchConditions _condition;

        public TransferreferenceReportForCsv(TransferReference02SearchConditions condition) : base(ReportTypes.Csv)
        {
            _condition = condition;
        }

        public override IEnumerable<TransferReferenceReportRowForCsv> GetData()
        {
            Query.TransferReference.Report query = new Query.TransferReference.Report();
            IEnumerable<TransferReferenceReportRowForCsv> data = query.GetResultRowList(_condition);
            return data;
        }

        public override IReportWriter<TransferReferenceReportRowForCsv> GetWriter()
        {
            return new CsvWriter<TransferReferenceReportRowForCsv>();
        }

        public override string GetDownloadFileName()
        {

            return string.Format(ReportResource.TRANSFER_REFERENCE,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}