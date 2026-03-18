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
using static Wms.Areas.Stock.ViewModels.AdjustReference.AdjustReferenceSearchConditions;

namespace Wms.Areas.Stock.Reports.Export
{
    public class AdjustReferenceReportForCsv : BaseExportReport<AdjustReferenceReportRowForCsv>, IReportExportable<AdjustReferenceReportRowForCsv>
    {
        private readonly AdjustReferenceSearchConditions _condition;

        public AdjustReferenceReportForCsv(AdjustReferenceSearchConditions condition) : base(ReportTypes.Csv)
        {
            _condition = condition;
            _condition.PageSize = int.MaxValue;
            _condition.ClearPageInformation();
        }

        public override IEnumerable<AdjustReferenceReportRowForCsv> GetData()
        {
            var ret = new List<AdjustReferenceReportRowForCsv>();
            var resultRowList = AdjustReferenceQuery.GetResultRowList(_condition);
            var centerName = Profile.User.GetSelectCenterListItems().ToList().Find((val) => val.Value == _condition.CenterId)?.Text;

            foreach (var resultRow in resultRowList)
            {
                var csvRow = new AdjustReferenceReportRowForCsv(resultRow)
                {
                    CenterId = _condition.CenterId,
                    CenterName = centerName,
                    IssuerCode = Profile.User.UserId,
                    IssuerName = Profile.User.UserName
                };

                ret.Add(csvRow);
            }

            return ret;
        }

        public override IReportWriter<AdjustReferenceReportRowForCsv> GetWriter()
        {
            return new CsvWriter<AdjustReferenceReportRowForCsv>();
        }

        public override string GetDownloadFileName()
        {

            return string.Format(ReportResource.RPT_ADJUST_REFERENCE_LIST,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}