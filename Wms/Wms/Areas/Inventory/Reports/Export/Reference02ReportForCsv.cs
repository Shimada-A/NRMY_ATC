namespace Wms.Areas.Inventory.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Inventory.ViewModels.Reference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class Reference02ReportForCsv : BaseExportReport<ViewModels.Reference.ReferenceReportForCsv>, IReportExportable<ViewModels.Reference.ReferenceReportForCsv>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private Reference02SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public Reference02ReportForCsv(ReportTypes reportType, Reference02SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.Reference.ReferenceReportForCsv> GetData()
        {
            Query.Reference.Report query = new Query.Reference.Report();
            return query.Reference02ListingForCsv(this._search);
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.Reference.ReferenceReportForCsv> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.Reference.ReferenceReportForCsv>(resourceKey: "RPT_INV_REFERENCE");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.Reference.ReferenceReportForCsv>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_INV_REFERENCE,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT),
                Profile.User.CenterId,
                Profile.User.UserId);
        }
    }
}