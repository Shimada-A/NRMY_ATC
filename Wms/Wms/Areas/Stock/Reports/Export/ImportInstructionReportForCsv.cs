using Share.Common;
using Share.Reports.Export;
using System;
using System.Collections.Generic;
using Wms.Areas.Stock.ViewModels.ImportInstruction;
using Wms.Common;
using Wms.Resources;

namespace Wms.Areas.Stock.Reports.Export
{
    public class ImportInstructionReportForCsv : BaseExportReport<ImportInstructionReportRowForCsv>, IReportExportable<ImportInstructionReportRowForCsv>
    {
        private ImportInstructionSearchConditions Conditions { get; }

        public ImportInstructionReportForCsv(ImportInstructionSearchConditions conditions) : base(ReportTypes.Csv)
        {
            Conditions = conditions;
        }

        public override IEnumerable<ImportInstructionReportRowForCsv> GetData()
        {
            return new Query.ImportInstruction.Report().GetImportInstructionListingForCsv(Conditions);
        }

        public override IReportWriter<ImportInstructionReportRowForCsv> GetWriter()
        {
            return new CsvWriter<ImportInstructionReportRowForCsv>();
        }

        public override string GetDownloadFileName()
        {

            return string.Format(ReportResource.RPT_IMPORTINSTRUCTION,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}