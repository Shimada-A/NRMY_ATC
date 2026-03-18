namespace Share.Reports.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using OfficeOpenXml;
    using Share.Extensions.Classes;

    /// <summary>
    /// Excelリーダー
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcelReader<T> : IReportReader<T> where T : new()
    {
        /// <summary>
        /// レポート読み込み
        /// </summary>
        /// <returns>読み込みデータ</returns>
        /// <remarks>
        ///
        /// </remarks>
        public IEnumerable<T> Read(HttpPostedFileBase file)
        {
            IEnumerable<T> reportData;

            // byte[] fileBytes = new byte[file.ContentLength];
            // file.InputStream.Read(fileBytes, 0, file.ContentLength);
            // MemoryStream excelFileStream = new MemoryStream(fileBytes);
            // NPOI.SS.UserModel.IWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(excelFileStream);
            using (var excel = new ExcelPackage(file.InputStream))
            {
                var workSheet = excel.Workbook.Worksheets[1];
                reportData = workSheet.ConvertSheetToObjects<T>();
                return reportData?.ToList();
            }
        }

        /// <summary>
        /// 日付/時刻チェック
        /// </summary>
        /// <param name="type">日付/時刻フラグ 0：日付；1：時刻</param>
        /// <param name="datetime">日付/時刻</param>
        /// <returns></returns>
        public bool CheckDateTime(int type, string datetime)
        {
            if (!string.IsNullOrEmpty(datetime))
            {
                string autoLinkageTimeFrom = datetime.Replace("-", string.Empty).Replace("/", string.Empty).Replace(":", string.Empty);
                if (!string.IsNullOrEmpty(autoLinkageTimeFrom))
                {
                    DateTime dtDate;
                    if (type == 1)
                    {
                        if (autoLinkageTimeFrom.Length != 4)
                        {
                            return false;
                        }

                        return DateTime.TryParse(DateTime.Now.ToString("yyyy/MM/dd") + " " + autoLinkageTimeFrom.Substring(0, 2) + ":" + autoLinkageTimeFrom.Substring(2, 2), out dtDate);
                    }
                    else
                    {
                        if (autoLinkageTimeFrom.Length != 8)
                        {
                            return false;
                        }

                        return DateTime.TryParse(autoLinkageTimeFrom.Substring(0, 4) + "/" + autoLinkageTimeFrom.Substring(4, 2) + "/" + autoLinkageTimeFrom.Substring(6, 2), out dtDate);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 数値チェック
        /// </summary>
        /// <param name="field">数値</param>
        /// <param name="intFlag">整数フラグ</param>
        /// <returns></returns>
        public bool CheckNumber(string field, bool intFlag = true)
        {
            if (!string.IsNullOrEmpty(field))
            {
                if (intFlag)
                {
                    return int.TryParse(field, out int number);
                }
                else
                {
                    return decimal.TryParse(field, out decimal del);
                }
            }

            return true;
        }

        /// <summary>
        /// 小数チェック
        /// </summary>
        /// <param name="field">数値</param>
        /// <param name="precision">整数</param>
        /// <param name="scale">小数</param>
        /// <returns></returns>
        public bool CheckDeciaml(string field, int precision = 0, int scale = 0)
        {
            if (!string.IsNullOrEmpty(field))
            {
                if (decimal.TryParse(field, out decimal del))
                {
                    string pattern = @"(^\d{1," + precision + "}";
                    if (scale > 0)
                    {
                        pattern += @"\.\d{0," + scale + "}$)|" + pattern;
                    }

                    pattern += "$)";
                    return Regex.IsMatch(field, pattern);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 数値範囲チェック
        /// </summary>
        /// <param name="field">数値</param>
        /// <returns></returns>
        public bool CheckNumberRange(string field, int start, int end)
        {
            if (!string.IsNullOrEmpty(field))
            {
                if (int.Parse(field) < start || int.Parse(field) > end)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 緯度、経度チェック
        /// </summary>
        /// <param name="s">数値</param>
        /// <param name="precision">整数</param>
        /// <param name="scale">小数</param>
        /// <returns></returns>
        public bool IsNumber(string s, int precision, int scale, bool latitude = true)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (latitude)
                {
                    if (decimal.Parse(s) < -90 || decimal.Parse(s) > 90)
                    {
                        return false;
                    }
                }
                else
                {
                    if (decimal.Parse(s) < -180 || decimal.Parse(s) > 180)
                    {
                        return false;
                    }
                }

                if ((precision == 0) && (scale == 0))
                {
                    return false;
                }

                string pattern = @"(^\d{1," + precision + "}";
                if (scale > 0)
                {
                    pattern += @"\.\d{0," + scale + "}$)|" + pattern;
                }

                pattern += "$)";
                return Regex.IsMatch(s, pattern);
            }

            return true;
        }

        /// <summary>
        /// 禁則文字チェック
        /// </summary>
        /// <param name="field">数値</param>
        /// <returns></returns>
        public bool CheckErrWord(string field, string word)
        {
            bool rtn = true;
            if (!string.IsNullOrWhiteSpace(field))
            {
                rtn = !field.Contains(word);
            }

            return rtn;
        }
    }
}