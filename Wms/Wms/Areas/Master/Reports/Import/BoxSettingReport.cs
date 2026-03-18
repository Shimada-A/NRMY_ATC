namespace Wms.Areas.Master.Reports.Import
{
    using System.Collections.Generic;
    using System.Web;
    using Share.Common;
    using Share.Reports.Import;
    using Wms.Areas.Master.Query.BoxSetting;

    public class BoxSettingReport : BaseImportReport<ViewModels.BoxSetting.Report>, IReportImportable<ViewModels.BoxSetting.Report>
    {
        public BoxSettingReport(ReportTypes reportType, HttpPostedFileBase file, string guid) : base(reportType, file, guid)
        {
        }

        /// <summary>
        /// ワークID
        /// </summary>
        public long _seq;

        public override IReportReader<ViewModels.BoxSetting.Report> GetReader()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new BoxSettingExcelReader<ViewModels.BoxSetting.Report>();
            }

            return null;
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <param name="reportData">レポート読み取りデータ</param>
        public override void Import(IEnumerable<ViewModels.BoxSetting.Report> reportData)
        {
            var query = new Report();
            _seq = query.InsertWwMasBoxSettings(reportData);
        }
    }
}