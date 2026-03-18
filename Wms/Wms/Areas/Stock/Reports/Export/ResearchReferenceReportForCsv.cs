using Share.Common;
using Share.Reports.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Stock.Query.ResearchReference;
using Wms.Areas.Stock.ViewModels.ResearchReference;
using Wms.Common;
using Wms.Resources;

namespace Wms.Areas.Stock.Reports.Export
{
    public class ResearchReferenceReportForCsv : BaseExportReport<ResearchReferenceReportRowForCsv>, IReportExportable<ResearchReferenceReportRowForCsv>
    {
        private readonly ResearchReferenceSearchConditions _condition;

        public ResearchReferenceReportForCsv(ResearchReferenceSearchConditions condition) : base(ReportTypes.Csv)
        {
            _condition = condition;
            _condition.PageSize = int.MaxValue;
            _condition.ClearPageInformation();
        }

        public override IEnumerable<ResearchReferenceReportRowForCsv> GetData()
        {
            var ret = new List<ResearchReferenceReportRowForCsv>();
            var resultRowList = ResearchReferenceQuery.GetResultReportRowList(_condition);
            var centerName = Profile.User.GetSelectCenterListItems().ToList().Find((val) => val.Value == _condition.CenterId)?.Text;
            //発行フラグ更新
            var result = ResearchReferenceQuery.UpdateFlg(resultRowList);

            foreach (var resultRow in resultRowList)
            {
                var csvRow = new ResearchReferenceReportRowForCsv(resultRow)
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

        public override IReportWriter<ResearchReferenceReportRowForCsv> GetWriter()
        {
            return new CsvWriter<ResearchReferenceReportRowForCsv>();
        }

        public override string GetDownloadFileName()
        {

            return string.Format(ReportResource.RPT_RESEARCH_REFERENCE_LIST,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}