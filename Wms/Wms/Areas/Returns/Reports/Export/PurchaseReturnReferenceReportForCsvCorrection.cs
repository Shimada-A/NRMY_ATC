namespace Wms.Areas.Returns.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Returns.ViewModels.PurchaseCorrection;
    using Wms.Areas.Returns.ViewModels.PurchaseReturnReference;
    using Wms.Common;
    using Wms.Resources;

    public class PurchaseReturnReferenceReportForCsvCorrection : BaseExportReport<PurchaseCorrectionReport>, IReportExportable<PurchaseCorrectionReport>
    {
        private readonly PurchaseReturnReference01SearchConditions _condition;

        public PurchaseReturnReferenceReportForCsvCorrection(PurchaseReturnReference01SearchConditions condition) : base(ReportTypes.Csv)
        {
            _condition = condition;
        }

        public override IEnumerable<PurchaseCorrectionReport> GetData()
        {
            Query.PurchaseReturnReference.Report query = new Query.PurchaseReturnReference.Report();
            IEnumerable<PurchaseCorrectionReport> data = query.GetResultRowListCorrection(_condition);
            return data;
        }

        public override IReportWriter<PurchaseCorrectionReport> GetWriter()
        {
            return new CsvWriter<PurchaseCorrectionReport>();
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