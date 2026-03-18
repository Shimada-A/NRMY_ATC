namespace Wms.Areas.Ship.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.BtoBReference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class BtoBReferenceReportJan : BaseExportReport<ViewModels.BtoBReference.BtoBReferenceReportJan>, IReportExportable<ViewModels.BtoBReference.BtoBReferenceReportJan>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private BtoBReference02SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public BtoBReferenceReportJan(ReportTypes reportType, BtoBReference02SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.BtoBReference.BtoBReferenceReportJan> GetData()
        {
            Query.BtoBReference.Report query = new Query.BtoBReference.Report();
            IEnumerable<ViewModels.BtoBReference.BtoBReferenceReportJan> data = query.GetDetailJanData(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.BtoBReference.BtoBReferenceReportJan> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.BtoBReference.BtoBReferenceReportJan>(resourceKey: "RPT_BTO_B_REFERENCE_JAN");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.BtoBReference.BtoBReferenceReportJan>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_BTO_B_REFERENCE_JAN,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}