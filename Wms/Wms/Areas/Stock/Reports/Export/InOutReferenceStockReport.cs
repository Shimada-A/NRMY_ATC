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
    public class InOutReferenceStockReport : BaseExportReport<InOutReferenceStockReportRow>, IReportExportable<InOutReferenceStockReportRow>
    {
        private string TemplateExcelFileName => "RPT_INOUTREFERENCE_STOCK";

        private InOutReferenceSearchConditions SearchConditions { get; set; }

        public InOutReferenceStockReport(ReportTypes reportType, InOutReferenceSearchConditions searchConditions) : base(reportType)
        {
            SearchConditions = searchConditions;
        }

        public override IEnumerable<InOutReferenceStockReportRow> GetData()
        {
            return new InOutReferenceQuery().GetStockReportData(SearchConditions);
        }

        public override IReportWriter<InOutReferenceStockReportRow> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<InOutReferenceStockReportRow>(resourceKey: TemplateExcelFileName);
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<InOutReferenceStockReportRow>();
            }

            return null;
        }

        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.ResourceManager.GetString(TemplateExcelFileName), DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT), Profile.User.CenterId, Profile.User.UserId);
        }
    }
}