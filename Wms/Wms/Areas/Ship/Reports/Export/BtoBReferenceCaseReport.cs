using Share.Common;
using Share.Reports.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Ship.ViewModels.BtoBReference;
using Wms.Common;
using Wms.Resources;

namespace Wms.Areas.Ship.Reports.Export
{
    public class BtoBReferenceCaseReport : BaseExportReport<ViewModels.BtoBReference.BtoBReferenceCaseReport>, IReportExportable<ViewModels.BtoBReference.BtoBReferenceCaseReport>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        private BtoBReference01SearchConditions _search;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="search">出力データの検索条件</param>
        public BtoBReferenceCaseReport(ReportTypes reportType, BtoBReference01SearchConditions search)
            : base(reportType)
        {
            this._search = search;
        }

        /// <summary>
        /// 出力データ取得
        /// </summary>
        /// <returns>出力データ</returns>
        public override IEnumerable<ViewModels.BtoBReference.BtoBReferenceCaseReport> GetData()
        {
            Query.BtoBReference.Report query = new Query.BtoBReference.Report();
            IEnumerable<ViewModels.BtoBReference.BtoBReferenceCaseReport> data = query.BtoBReferenceCaseListing(this._search);

            return data;
        }

        /// <summary>
        /// レポート書き込みクラス生成
        /// </summary>
        /// <returns>レポート書き込みクラス</returns>
        /// <remarks>共通のWriterが使用できない場合はIReportReaderインターフェースを使ったクラスを作成してください</remarks>
        public override IReportWriter<ViewModels.BtoBReference.BtoBReferenceCaseReport> GetWriter()
        {
            if (ReportType == ReportTypes.Excel)
            {
                return new ExcelWriter<ViewModels.BtoBReference.BtoBReferenceCaseReport>(resourceKey: "RPT_BTO_B_REFERENCE_CASE");
            }
            else if (ReportType == ReportTypes.Csv)
            {
                return new CsvWriter<ViewModels.BtoBReference.BtoBReferenceCaseReport>();
            }

            return null;
        }

        /// <summary>
        /// ダウンロードファイル名取得
        /// </summary>
        /// <returns>ダウンロードファイル名（拡張子なし）</returns>
        public override string GetDownloadFileName()
        {
            return string.Format(ReportResource.RPT_BTO_B_REFERENCE_CASE,
                Profile.User.CenterId,
                Profile.User.UserId,
                DateTime.Now.ToLocalTime().ToString(ReportResource.FILE_NAME_DATE_FORMAT));
        }
    }
}