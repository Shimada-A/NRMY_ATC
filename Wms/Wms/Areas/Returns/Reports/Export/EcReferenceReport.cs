namespace Wms.Areas.Returns.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Returns.ViewModels.EcReference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class EcReferenceReport : BaseExportReport<ViewModels.EcReference.EcReferenceReport>, IReportExportable<ViewModels.EcReference.EcReferenceReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private EcReference01SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public EcReferenceReport(ReportTypes reportType, EcReference01SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.EcReference.EcReferenceReport> GetData()
        {
            Query.EcReference.Report query = new Query.EcReference.Report();
            IEnumerable<ViewModels.EcReference.EcReferenceReport> data = query.EcReferenceListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.EcReference.EcReferenceReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.EcReference.EcReferenceReport>(resourceKey: "RPT_EC_REFERENCE");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.EcReference.EcReferenceReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_EC_REFERENCE1,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}