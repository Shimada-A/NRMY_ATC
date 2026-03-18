namespace Wms.Areas.Master.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Master.ViewModels.User;
    using Wms.Common;
    using Wms.Resources;

    public class UserlabelReportForCsv : BaseExportReport<UserLabelReportRowForCsv>, IReportExportable<UserLabelReportRowForCsv>
    {
        private readonly UserSearchCondition _condition;

        public UserlabelReportForCsv(UserSearchCondition condition) : base(ReportTypes.Csv)
        {
            _condition = condition;
        }

        public override IEnumerable<UserLabelReportRowForCsv> GetData()
        {
            Query.User.Report query = new Query.User.Report();
            IEnumerable<UserLabelReportRowForCsv> data = query.GetResultRowList(_condition);
            return data;
        }

        public override IReportWriter<UserLabelReportRowForCsv> GetWriter()
        {
            return new CsvWriter<UserLabelReportRowForCsv>();
        }

        public override string GetDownloadFileName()
        {

            return string.Format(ReportResource.RPT_USER_LABEL,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}