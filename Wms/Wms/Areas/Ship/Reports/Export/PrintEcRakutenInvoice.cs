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
    public class PrintEcRakutenInvoice : BaseExportReport<PrintNouhinEcRakuten>, IReportExportable<PrintNouhinEcRakuten>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PrintEcInvoiceConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PrintEcRakutenInvoice(ReportTypes reportType, PrintEcInvoiceConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<PrintNouhinEcRakuten> GetData()
        {
            Query.PrintEcInvoice.Report query = new Query.PrintEcInvoice.Report();
            IEnumerable<PrintNouhinEcRakuten> data = query.GetPrintNouhinEcRakutens(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<PrintNouhinEcRakuten> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<PrintNouhinEcRakuten>(resourceKey: "RPT_NOUHIN_EC_RAKUTEN");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<PrintNouhinEcRakuten>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_NOUHIN_EC_RAKUTEN,
                Profile.User.UserId,
                this._search.CenterId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}