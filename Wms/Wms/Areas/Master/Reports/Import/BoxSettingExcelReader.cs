namespace Wms.Areas.Master.Reports.Import
{
    using System.Collections.Generic;
    using System.Web;
    using OfficeOpenXml;
    using Share.Reports.Import;
    using Wms.Areas.Master.ViewModels.BoxSetting;

    public class BoxSettingExcelReader<T> : IReportReader<T> where T : Report
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
            // ケース閾値設定情報を読み出す
            var boxSettingInfo = new Report
            {
                // CategoryId1
                CategoryId1 = workSheet.Cells[row, 1].Text,

                // CategoryId2
                CategoryId2 = workSheet.Cells[row, 2].Text,

                // CategoryId3
                CategoryId3 = workSheet.Cells[row, 3].Text,

                // CategoryId4
                CategoryId4 = workSheet.Cells[row, 4].Text,

                // ItemId
                ItemId = workSheet.Cells[row, 5].Text,

                // ThresholdClass
                ThresholdClass = workSheet.Cells[row, 6].Text,

                // Threshold
                Threshold = workSheet.Cells[row, 7].Text
            };

            return boxSettingInfo;
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