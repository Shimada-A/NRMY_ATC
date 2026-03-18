namespace Wms.Areas.Master.Reports.Import
{
    using System.Collections.Generic;
    using System.Web;
    using OfficeOpenXml;
    using Share.Reports.Import;
    using Wms.Areas.Master.ViewModels.Location;

    public class LocationExcelReader<T> : IReportReader<T> where T : Report
    {
        /// <summary>
        /// Excelのデータを読み取る
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IEnumerable<T> Read(HttpPostedFileBase file)
        {
            IEnumerable<T> reportData;

            using (var excel = new ExcelPackage(file.InputStream))
            {
                var workSheet = excel.Workbook.Worksheets[1];
                var endColumn = workSheet.Dimension.End.Column;
                var endRow = LastRow(workSheet, endColumn);

                var data = new List<Report>();

                for (var row = 2; row <= endRow; row++)
                {
                    // データの追加
                    data.Add(CreateReportInstance(workSheet, row));
                }

                reportData = data as IEnumerable<T>;

                return reportData;
            }
        }

        /// <summary>
        /// WorkSheetのデータからReportのインスタンスを作成する
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private Report CreateReportInstance(ExcelWorksheet workSheet, int row)
        {
            // ロケーション設定情報を読み出す
            var locationInfo = new Report
            {
                // エリア
                Locsec_1 = workSheet.Cells[row, 1].Text,

                // 棚列
                Locsec_2 = workSheet.Cells[row, 2].Text,

                // 棚番
                Locsec_3 = workSheet.Cells[row, 3].Text,

                // 段
                Locsec_4 = workSheet.Cells[row, 4].Text,

                // 間口
                Locsec_5 = workSheet.Cells[row, 5].Text,

                // ロケーション区分
                LocationClass = workSheet.Cells[row, 6].Text,

                // 格付ID
                GradeId = workSheet.Cells[row, 7].Text,

                // 引当優先順位
                AllocPriority = workSheet.Cells[row, 8].Text,

                // ピッキンググループNo
                PickingGroupNo = workSheet.Cells[row, 9].Text
            };

            return locationInfo;
        }

        /// <summary>
        /// 値が入っている最終行を取得
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="endColumn"></param>
        /// <returns></returns>
        private int LastRow(ExcelWorksheet workSheet, int endColumn)
        {
            var endRow = workSheet.Dimension.End.Row;

            for (; endRow > 1; endRow--)
            {
                var exists = false;
                for (int i = 1; i <= endColumn; i++)
                {
                    if (!string.IsNullOrEmpty(workSheet.Cells[endRow, i].Text))
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists) break;
            }

            return endRow;
        }
    }
}