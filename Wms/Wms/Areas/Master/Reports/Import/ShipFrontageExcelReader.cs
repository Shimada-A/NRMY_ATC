namespace Wms.Areas.Master.Reports.Import
{
    using System.Collections.Generic;
    using System.Web;
    using OfficeOpenXml;
    using Share.Reports.Import;
    using Wms.Areas.Master.ViewModels.ShipFrontage;

    public class ShipFrontageExcelReader<T> : IReportReader<T> where T : Report
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
            // 出荷レーン間口情報を読み出す
            var shipFrontageInfo = new Report
            {
                // ブランドID
                BrandId = workSheet.Cells[row,1].Text,

                // ブランド名

                BrandName= workSheet.Cells[row,2].Text,

                // レーンNo
                LaneNo = workSheet.Cells[row, 3].Text,

                // 間口No
                FrontageNo = workSheet.Cells[row, 4].Text,

                // 出荷先ID
                StoreId = workSheet.Cells[row, 5].Text
            };

            return shipFrontageInfo;
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