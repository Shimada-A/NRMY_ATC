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
    public class PrintEcWaKsnapInvoice : BaseExportReport<PrintNouhinEcWaKsnap>, IReportExportable<PrintNouhinEcWaKsnap>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PrintEcInvoiceConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PrintEcWaKsnapInvoice(ReportTypes reportType, PrintEcInvoiceConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<PrintNouhinEcWaKsnap> GetData()
        {
            Query.PrintEcInvoice.Report query = new Query.PrintEcInvoice.Report();
            IEnumerable<PrintNouhinEcWaKsnap> data = query.GetPrintNouhinEcWaKsnaps(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<PrintNouhinEcWaKsnap> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<PrintNouhinEcWaKsnap>(resourceKey: "RPT_NOUHIN_EC_WAKSNAP");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<PrintNouhinEcWaKsnap>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_NOUHIN_EC_WAKSNAP,
                Profile.User.UserId,
                this._search.CenterId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}