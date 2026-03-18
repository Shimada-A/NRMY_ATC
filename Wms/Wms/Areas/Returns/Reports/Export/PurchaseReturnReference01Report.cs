namespace Wms.Areas.Returns.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Returns.ViewModels.PurchaseReturnReference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class PurchaseReturnReference01Report : BaseExportReport<ViewModels.PurchaseReturnReference.PurchaseReturnReference01Report>, IReportExportable<ViewModels.PurchaseReturnReference.PurchaseReturnReference01Report>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private PurchaseReturnReference01SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PurchaseReturnReference01Report(ReportTypes reportType, PurchaseReturnReference01SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.PurchaseReturnReference.PurchaseReturnReference01Report> GetData()
        {
            Query.PurchaseReturnReference.Report query = new Query.PurchaseReturnReference.Report();
            IEnumerable<ViewModels.PurchaseReturnReference.PurchaseReturnReference01Report> data = query.PurchaseReturnReference01Listing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.PurchaseReturnReference.PurchaseReturnReference01Report> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.PurchaseReturnReference.PurchaseReturnReference01Report>(resourceKey: "RPT_RET_PURCHASE_REFERENCE01");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.PurchaseReturnReference.PurchaseReturnReference01Report>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_RET_PURCHASE_REFERENCE01,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}