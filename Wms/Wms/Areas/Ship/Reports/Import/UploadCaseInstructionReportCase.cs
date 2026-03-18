namespace Wms.Areas.Ship.Reports.Import
{
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Web;
    using Share.Common;
    using Share.Reports.Import;
    using Wms.Areas.Ship.Query.UploadCaseInstruction;

    public class UploadCaseInstructionReportCase : BaseImportReport<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase>, IReportImportable<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase>
    {
        string _ShipInstructName = string.Empty;
        string _CenterId = string.Empty;
        public UploadCaseInstructionReportCase(ReportTypes reportType, HttpPostedFileBase file, string guid,string ShipInstructName, string CenterId) : base(reportType, file, guid)
        {
            _ShipInstructName = ShipInstructName;
            _CenterId = CenterId;
        }

        /// <summary>
        /// ワークID
        /// </summary>
        public long _seq;
        public string _message;

        public override IReportReader<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase> GetReader()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new UploadCaseInstructionExcelReaderCase<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase>();
            }

            return null;
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <param name="reportData">レポート読み取りデータ</param>
        public override void Import(IEnumerable<ViewModels.UploadCaseInstruction.UploadCaseInstructionReportCase> reportData)
        {
            var query = new Report();
            query.InsertWW_INPORT_CASE_INS(reportData,out _message,out _seq, _ShipInstructName, _CenterId);
        }
    }
}