using Share.Common;
using Share.Reports.Export;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Wms.Areas.Ship.ViewModels.TransferReference;
using Wms.Common;
using Wms.Resources;

namespace Wms.Areas.Ship.Reports.Export
{
    public class PickInfoCsv : BaseExportReport<ViewModels.TransferReference.PickInfoResult>, IReportExportable<ViewModels.TransferReference.PickInfoResult>
    {
        private TransferReferenceSearchConditions condition;

        public PickInfoCsv(ReportTypes reportType,TransferReferenceSearchConditions condition) : base(reportType)
        {
            this.condition = condition;
        }

        public override IEnumerable<PickInfoResult> GetData()
        {
            Query.TransferReference.Report query = new Query.TransferReference.Report();
            var data = query.GetPickInfo(condition);
            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<PickInfoResult> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<PickInfoResult>(resourceKey: "RPT_TRANSFER_REFERENCE03");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new InnerCsvWriter();
            }

            return null;
        }

        private class InnerCsvWriter : CsvWriter<PickInfoResult>
        {
            public override byte[] GetReportStream(IEnumerable<PickInfoResult> data)
            {
                var map = this.GetClassMap();
                var stream = new MemoryStream();
                using (TextWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(true)))
                using (var writer = new CsvHelper.CsvWriter(streamWriter, CultureInfo.CurrentCulture))
                {
                    writer.Configuration.Quote = '\t';
                    writer.Configuration.RegisterClassMap(map);
                    writer.WriteRecords(data);
                    writer.Flush();
                }

                // BOMを付けてCSVファイルをExcelで表示可能にする
                byte[] bom = { 0xEF, 0xBB, 0xBF };
                return bom.Concat(stream.ToArray()).ToArray();

            }
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_TRANSFER_REFERENCE03,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}