using Share.Common;
using Share.Reports.Export;
using System;
using System.Collections.Generic;
using Wms.Areas.Stock.Query.InOutReference;
using Wms.Areas.Stock.ViewModels.InOutReference;
using Wms.Common;
using Wms.Resources;

namespace Wms.Areas.Stock.Reports.Export
{
    public class InOutReferencePackageStockReport : BaseExportReport<InOutReferencePackageStockReportRow>, IReportExportable<InOutReferencePackageStockReportRow>
    {
        private string TemplateExcelFileName => "RPT_INOUTREFERENCE_PACKAGESTOCK";

        private InOutReferenceSearchConditions SearchConditions { get; set; }

        public InOutReferencePackageStockReport(ReportTypes reportType, InOutReferenceSearchConditions searchConditions) : base(reportType)
        {
            SearchConditions = searchConditions;
        }

        public override IEnumerable<InOutReferencePackageStockReportRow> GetData()
        {
            return new InOutReferenceQuery().GetPackageStockReportData(SearchConditions);
        }

        public override IReportWriter<InOutReferencePackageStockReportRow> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<InOutReferencePackageStockReportRow>(resourceKey: TemplateExcelFileName);
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<InOutReferencePackageStockReportRow>();
            }

            return null;
        }

        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.ResourceManager.GetString(TemplateExcelFileName), DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT), Profile.User.CenterId, Profile.User.UserId);
        }
    }
}