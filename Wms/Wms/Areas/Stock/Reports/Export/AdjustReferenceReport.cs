using Share.Common;
using Share.Reports.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Stock.Query.AdjustReference;
using Wms.Areas.Stock.ViewModels.AdjustReference;
using Wms.Common;
using Wms.Resources;

namespace Wms.Areas.Stock.Reports.Export
{
    public class AdjustReferenceReport : BaseExportReport<AdjustReferenceReportRow>, IReportExportable<AdjustReferenceReportRow>
    {
        protected const string ResourceKey = "RPT_ADJUST_REFERENCE";

        private readonly AdjustReferenceSearchConditions _condition;

        public AdjustReferenceReport(AdjustReferenceSearchConditions condition) : base(ReportTypes.Excel)
        {
            _condition = condition;
            _condition.PageSize = int.MaxValue;
        }

        public override IEnumerable<AdjustReferenceReportRow> GetData()
        {
            var ret = new List<AdjustReferenceReportRow>();
            var resultRowList = AdjustReferenceQuery.GetResultRowList(_condition);

            foreach(var resultRow in resultRowList)
            {
                ret.Add(new AdjustReferenceReportRow(resultRow));
            }

            return ret;
        }

        public override IReportWriter<AdjustReferenceReportRow> GetWriter()
        {
            return new ExcelWriter<AdjustReferenceReportRow>(ResourceKey);
        }

        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_ADJUST_REFERENCE,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}