namespace Wms.Areas.Ship.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.BtoBInstructionReference;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class BtoBInstructionReferenceDetailReport : BaseExportReport<ViewModels.BtoBInstructionReference.BtoBInstructionReferenceDetailReport>, IReportExportable<ViewModels.BtoBInstructionReference.BtoBInstructionReferenceDetailReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private BtoBInstructionReference01SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public BtoBInstructionReferenceDetailReport(ReportTypes reportType, BtoBInstructionReference01SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.BtoBInstructionReference.BtoBInstructionReferenceDetailReport> GetData()
        {
            Query.BtoBInstructionReference.Report query = new Query.BtoBInstructionReference.Report();
            IEnumerable<ViewModels.BtoBInstructionReference.BtoBInstructionReferenceDetailReport> data = query.BtoBInstructionReferenceDetailListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.BtoBInstructionReference.BtoBInstructionReferenceDetailReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.BtoBInstructionReference.BtoBInstructionReferenceDetailReport>(resourceKey: "RPT_BTO_B_INSTRUCTION_REFERENCE2");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.BtoBInstructionReference.BtoBInstructionReferenceDetailReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_BTO_B_INSTRUCTION_REFERENCE2,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}