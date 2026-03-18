namespace Wms.Areas.Master.Reports.Import
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using OfficeOpenXml;
    using Share.Reports.Import;
    using Wms.Areas.Master.ViewModels.LocTransporter;

    public class LocTransporterExcelReader<T> : IReportReader<T> where T : Report
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
            int col = 1;
            var LocTransporterInfo = new Report();

            LocTransporterInfo.CenterId = workSheet.Cells[row, col].Text;
            col += 1;
            LocTransporterInfo.ShipToStoreId = workSheet.Cells[row, col].Text;
            col += 1;
            LocTransporterInfo.ShipToStoreClass = workSheet.Cells[row, col].Text;
            col += 1;
            LocTransporterInfo.StartDate = workSheet.Cells[row, col].Text;
            col += 1;
            LocTransporterInfo.TransporterId = workSheet.Cells[row, col].Text;
            col += 1;
            LocTransporterInfo.LeadTimes = workSheet.Cells[row, col].Text;
            col += 1;
            //LocTransporterInfo.TransporterIdMon = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.LeadTimesMon = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.TransporterIdTue = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.LeadTimesTue = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.TransporterIdWed = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.LeadTimesWed = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.TransporterIdThu = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.LeadTimesThu = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.TransporterIdFri = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.LeadTimesFri = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.TransporterIdSat = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.LeadTimesSat = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.TransporterIdSun = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.LeadTimesSun = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.TransporterIdHol = workSheet.Cells[row, col].Text;
            //col += 1;
            //LocTransporterInfo.LeadTimesHol = workSheet.Cells[row, col].Text;
            //col += 1;
            LocTransporterInfo.ClientCd = workSheet.Cells[row, col].Text;
            col += 1;

            LocTransporterInfo.ControlId = workSheet.Cells[row, col].Text;
            col += 1;

            LocTransporterInfo.ConsignorId = workSheet.Cells[row, col].Text;
            col += 1;

            return LocTransporterInfo;
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