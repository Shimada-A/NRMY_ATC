namespace Wms.Areas.Returns.Reports.Export.PurchaseCorrection
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Returns.Query.PurchaseCorrection;
    using Wms.Areas.Returns.ViewModels.PurchaseCorrection;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class PurchaseCorrectionReportForCsv : BaseExportReport<PurchaseCorrectionReport>, IReportExportable<PurchaseCorrectionReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PurchaseCorrectionSearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PurchaseCorrectionReportForCsv(ReportTypes reportType, PurchaseCorrectionSearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<PurchaseCorrectionReport> GetData()
        {
            Report query = new Report();
            return query.ReturnListingForCsv(this._search);
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<PurchaseCorrectionReport> GetWriter()
        {
            if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<PurchaseCorrectionReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_PURCHASE_CORRECTION,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                this._search.CenterId,
                Profile.User.UserId);
        }
    }
}