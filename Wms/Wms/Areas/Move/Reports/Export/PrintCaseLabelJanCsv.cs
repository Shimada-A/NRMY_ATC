namespace Wms.Areas.Move.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Move.ViewModels.InputTransfer;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class PrintCaseLabelJanCsv : BaseExportReport<PrintCaseLabelCsv>, IReportExportable<PrintCaseLabelCsv>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private long _seq;
        private string _centerId;
        private List<string> _boxNos;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public PrintCaseLabelJanCsv(long seq, string centerId) : base(ReportTypes.Csv)
        {
            _seq = seq;
            _centerId = centerId;
        }

        public PrintCaseLabelJanCsv(List<string> boxNos, string centerId) : base(ReportTypes.Csv)
        {
            _boxNos = boxNos;
            _centerId = centerId;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<PrintCaseLabelCsv> GetData()
        {
            var query = new Query.InputTransfer.Report();
            IEnumerable<PrintCaseLabelCsv> data;

            if (_seq > 0)
            {
                data = query.PrintCaseLabelJanListing(_seq, _centerId);
            }
            else
            {
                data = query.PrintCaseLabelJanListing2(_boxNos, _centerId);
            }
            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<PrintCaseLabelCsv> GetWriter()
        {
            return new CsvWriter<PrintCaseLabelCsv>();
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_ARRIVAL_CASE_LABEL,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}