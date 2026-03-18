namespace Wms.Areas.Ship.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.BtoBInstructionInput;
    using Wms.Common;
    using Wms.Resources;
    public class BtoBInstructionInputReport : BaseExportReport<ViewModels.BtoBInstructionInput.BtoBInstructionInputReport>, IReportExportable<ViewModels.BtoBInstructionInput.BtoBInstructionInputReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private BtoBInstructionInput01SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public BtoBInstructionInputReport(ReportTypes reportType, BtoBInstructionInput01SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.BtoBInstructionInput.BtoBInstructionInputReport> GetData()
        {
            Query.BtoBInstructionInput.Report query = new Query.BtoBInstructionInput.Report();
            IEnumerable<ViewModels.BtoBInstructionInput.BtoBInstructionInputReport> data = query.BtoBInstructionInputListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.BtoBInstructionInput.BtoBInstructionInputReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.BtoBInstructionInput.BtoBInstructionInputReport>(resourceKey: "RPT_BTO_B_INSTRUCTION_INPUT");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.BtoBInstructionInput.BtoBInstructionInputReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_BTO_B_INSTRUCTION_INPUT,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}