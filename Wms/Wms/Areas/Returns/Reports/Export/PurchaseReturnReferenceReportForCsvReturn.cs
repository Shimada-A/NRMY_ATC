namespace Wms.Areas.Returns.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Returns.ViewModels.PurchaseReturnReference;
    using Wms.Areas.Returns.ViewModels.PurchaseReturns;
    using Wms.Common;
    using Wms.Resources;

    public class PurchaseReturnReferenceReportForCsvReturn : BaseExportReport<PurchaseReturnsReport>, IReportExportable<PurchaseReturnsReport>
    {
        private readonly PurchaseReturnReference01SearchConditions _condition;

        public PurchaseReturnReferenceReportForCsvReturn(PurchaseReturnReference01SearchConditions condition) : base(ReportTypes.Csv)
        {
            _condition = condition;
        }

        public override IEnumerable<PurchaseReturnsReport> GetData()
        {
            Query.PurchaseReturnReference.Report query = new Query.PurchaseReturnReference.Report();
            IEnumerable<PurchaseReturnsReport> data = query.GetResultRowListReturn(_condition);
            return data;
        }

        public override IReportWriter<PurchaseReturnsReport> GetWriter()
        {
            return new CsvWriter<PurchaseReturnsReport>();
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