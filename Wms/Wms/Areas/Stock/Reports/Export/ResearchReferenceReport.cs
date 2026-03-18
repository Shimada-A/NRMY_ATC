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
    public class ResearchReferenceReport : BaseExportReport<ResearchReferenceReportRow>, IReportExportable<ResearchReferenceReportRow>
    {
        protected const string ResourceKey = "RPT_RESEARCH_REFERENCE";

        private readonly ResearchReferenceSearchConditions _condition;

        public ResearchReferenceReport(ResearchReferenceSearchConditions condition) : base(ReportTypes.Excel)
        {
            _condition = condition;
            _condition.PageSize = int.MaxValue;
            _condition.ClearPageInformation();
        }

        public override IEnumerable<ResearchReferenceReportRow> GetData()
        {
            var ret = new List<ResearchReferenceReportRow>();
            var resultRowList = ResearchReferenceQuery.GetResultRowList(_condition,true);
            var resultSlipNo = string.Empty;

            foreach (var resultRow in resultRowList)
            {
                //同一のヘッダ情報の場合、表示しない
                if(resultSlipNo == resultRow.SlipNo)
                {
                    resultRow.SlipNo = string.Empty;
                    resultRow.OccurredDateTime = null;
                    resultRow.RegistClass = null;
                    resultRow.RegistUserId = string.Empty;
                    resultRow.BatchNo = string.Empty;
                    resultRow.GasBatchNo = string.Empty;
                    resultRow.LocationCd = string.Empty;
                    resultRow.FrontageNo = string.Empty;
                    resultRow.BoxNo = string.Empty;
                    resultRow.Sku = string.Empty;
                    resultRow.Jan = string.Empty;
                    resultRow.ItemId = string.Empty;
                    resultRow.ColorId = string.Empty;
                    resultRow.SizeId = string.Empty;
                    resultRow.GradeId = string.Empty;
                    resultRow.DiffQuantity = null;
                    resultRow.InvoiceNo = string.Empty;
                    resultRow.ShippingStoreId = string.Empty;
                    resultRow.ItemName = string.Empty;
                    resultRow.ColorName = string.Empty;
                    resultRow.SizeName = string.Empty;
                    resultRow.GradeName = string.Empty;
                    resultRow.RegistClassName = string.Empty;
                    resultRow.ListPrintFlagName = string.Empty;
                }
                ret.Add(new ResearchReferenceReportRow(resultRow));
                resultSlipNo = resultRow.SlipNo;
            }

            return ret;
        }

        public override IReportWriter<ResearchReferenceReportRow> GetWriter()
        {
            return new ExcelWriter<ResearchReferenceReportRow>(ResourceKey);
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