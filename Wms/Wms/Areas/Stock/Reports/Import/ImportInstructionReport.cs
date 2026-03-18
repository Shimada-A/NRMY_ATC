namespace Wms.Areas.Stock.Reports.Import
{
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Web;
    using Share.Common;
    using Share.Reports.Import;
    using Wms.Areas.Stock.Query.ImportInstruction;

    public class ImportInstructionReport : BaseImportReport<ViewModels.ImportInstruction.ImportInstructionOutputReport>, IReportImportable<ViewModels.ImportInstruction.ImportInstructionOutputReport>
    {
        string _SortInstructName = string.Empty;
        string _CenterId = string.Empty;
        public ImportInstructionReport(ReportTypes reportType, HttpPostedFileBase file, string guid,string SortInstructName,string CenterId) : base(reportType, file, guid)
        {
            _SortInstructName = SortInstructName;
            _CenterId = CenterId;
        }

        /// <summary>
        /// ワークID
        /// </summary>
        public long _seq;
        public string _message;

        public override IReportReader<ViewModels.ImportInstruction.ImportInstructionOutputReport> GetReader()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ImportInstructionExcelReader<ViewModels.ImportInstruction.ImportInstructionOutputReport>();
            }

            return null;
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <param name="reportData">レポート読み取りデータ</param>
        public override void Import(IEnumerable<ViewModels.ImportInstruction.ImportInstructionOutputReport> reportData)
        {
            var query = new Report();
            query.InsertWW_SORT_STOCK_INS(reportData,out _message,out _seq, _SortInstructName, _CenterId);
        }
    }
}