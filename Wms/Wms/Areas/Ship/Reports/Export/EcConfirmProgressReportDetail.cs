namespace Wms.Areas.Ship.Reports.Export
{
    using System;
    using System.Collections.Generic;
    using Share.Common;
    using Share.Reports.Export;
    using Wms.Areas.Ship.ViewModels.EcConfirmProgress;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// サンプル出力クラス
    /// </summary>
    public class EcConfirmProgressReportDetail : BaseExportReport<ViewModels.EcConfirmProgress.EcConfirmProgressReportDetail>, IReportExportable<ViewModels.EcConfirmProgress.EcConfirmProgressReportDetail>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private EcConfirmProgress01SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public EcConfirmProgressReportDetail(ReportTypes reportType, EcConfirmProgress01SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.EcConfirmProgress.EcConfirmProgressReportDetail> GetData()
        {
            Query.EcConfirmProgress.Report query = new Query.EcConfirmProgress.Report();
            IEnumerable<ViewModels.EcConfirmProgress.EcConfirmProgressReportDetail> data = query.EcConfirmProgress01ListingDetail(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.EcConfirmProgress.EcConfirmProgressReportDetail> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.EcConfirmProgress.EcConfirmProgressReportDetail>(resourceKey: "RPT_EC_CONFIRM_PROGRESS_DETAIL");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.EcConfirmProgress.EcConfirmProgressReportDetail>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_DC_REFERENCE,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}