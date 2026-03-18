using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Share.Common;
using Share.Reports.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Ship.ViewModels.PrintInvoice;
using Wms.Common;
using Wms.Resources;

namespace Wms.Areas.Ship.Reports.Export
{
    public class PrintInvoiceWorldSupply : BaseExportReport<ViewModels.PrintInvoice.PrintInvoiceWorldSupply>, IReportExportable<ViewModels.PrintInvoice.PrintInvoiceWorldSupply>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PrintInvoiceConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PrintInvoiceWorldSupply(ReportTypes reportType, PrintInvoiceConditions search) : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.PrintInvoice.PrintInvoiceWorldSupply> GetData()
        {
            return new Query.PrintInvoice.Report().GetPrintInvoiceWorldSupply(this._search);
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.PrintInvoice.PrintInvoiceWorldSupply> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.PrintInvoice.PrintInvoiceWorldSupply>(resourceKey: "RPT_INVOICE_WORLDSUPPLY");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.PrintInvoice.PrintInvoiceWorldSupply>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_INVOICE_WORLDSUPPLY,
                Profile.User.UserId,
                this._search.CenterId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }

    }
}