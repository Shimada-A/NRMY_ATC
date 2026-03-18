namespace Wms.Areas.Move.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Move.ViewModels.TransferReference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class TransferReference01Report : BaseExportReport<ViewModels.TransferReference.TransferReference01Report>, IReportExportable<ViewModels.TransferReference.TransferReference01Report>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private TransferReference01SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public TransferReference01Report(ReportTypes reportType, TransferReference01SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.TransferReference.TransferReference01Report> GetData()
        {
            Query.TransferReference.Report query = new Query.TransferReference.Report();
            IEnumerable<ViewModels.TransferReference.TransferReference01Report> data = query.TransferReference01Listing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.TransferReference.TransferReference01Report> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.TransferReference.TransferReference01Report>(resourceKey: "RPT_TRANSFER_REFERENCE01");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.TransferReference.TransferReference01Report>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_TRANSFER_REFERENCE01,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}