namespace Wms.Areas.Ship.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.PrintEcInvoice;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// 出力クラス
    /// </summary>
    public class PrintEcInvoiceYamato : BaseExportReport<ViewModels.PrintInvoice.PrintInvoiceYamato>, IReportExportable<ViewModels.PrintInvoice.PrintInvoiceYamato>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PrintEcInvoiceConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PrintEcInvoiceYamato(ReportTypes reportType, PrintEcInvoiceConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.PrintInvoice.PrintInvoiceYamato> GetData()
        {
            Query.PrintEcInvoice.Report query = new Query.PrintEcInvoice.Report();
            IEnumerable<ViewModels.PrintInvoice.PrintInvoiceYamato> data = query.GetPrintInvoiceYamatos(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.PrintInvoice.PrintInvoiceYamato> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.PrintInvoice.PrintInvoiceYamato>(resourceKey: "RPT_INVOICE_YAMATO");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.PrintInvoice.PrintInvoiceYamato>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_INVOICE_YAMATO,
                Profile.User.UserId,
                this._search.CenterId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}