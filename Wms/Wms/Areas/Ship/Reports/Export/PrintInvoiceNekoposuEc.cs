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
    public class PrintEcInvoiceNekoposu : BaseExportReport<ViewModels.PrintInvoice.PrintInvoiceNekoposu>, IReportExportable<ViewModels.PrintInvoice.PrintInvoiceNekoposu>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PrintEcInvoiceConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PrintEcInvoiceNekoposu(ReportTypes reportType, PrintEcInvoiceConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.PrintInvoice.PrintInvoiceNekoposu> GetData()
        {
            Query.PrintEcInvoice.Report query = new Query.PrintEcInvoice.Report();
            IEnumerable<ViewModels.PrintInvoice.PrintInvoiceNekoposu> data = query.GetPrintInvoiceNekoposu(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.PrintInvoice.PrintInvoiceNekoposu> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.PrintInvoice.PrintInvoiceNekoposu>(resourceKey: "RPT_INVOICE_NEKOPOSU");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.PrintInvoice.PrintInvoiceNekoposu>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_INVOICE_NEKOPOSU,
                Profile.User.UserId,
                this._search.CenterId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}