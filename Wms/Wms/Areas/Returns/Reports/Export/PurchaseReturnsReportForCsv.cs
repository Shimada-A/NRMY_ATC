namespace Wms.Areas.Returns.Reports.Export.PurchaseReturns
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Returns.Query.PurchaseReturns;
    using Wms.Areas.Returns.ViewModels.PurchaseReturns;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class PurchaseReturnsReportForCsv : BaseExportReport<PurchaseReturnsReport>, IReportExportable<PurchaseReturnsReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PurchaseReturnsSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PurchaseReturnsReportForCsv(ReportTypes reportType, PurchaseReturnsSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<PurchaseReturnsReport> GetData()
        {
            Report query = new Report();
            return query.ReturnListingForCsv(this._search);
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<PurchaseReturnsReport> GetWriter()
        {
            if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<PurchaseReturnsReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_PURCHASE_RETURN,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                this._search.CenterId,
                Profile.User.UserId);
        }
    }
}