namespace Share.Extensions.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OfficeOpenXml;
    using Share.Helpers;

    /// <summary>
    /// EPPLusのシートからオブジェクトへのマッピング拡張メソッド
    /// </summary>
    /// <remarks>
    /// 参考URL
    /// https://stackoverflow.com/questions/33436525/how-to-parse-excel-rows-back-to-types-using-epplus
    /// </remarks>
    public static class EPPLusExtensions
    {
        public static IEnumerable<T> ConvertSheetToObjects<T>(this ExcelWorksheet worksheet) where T : new()
        {
            // Display属性のOrder値をもとにカラム情報を取得する
            var columns = typeof(T)
                    .GetProperties()
                    .Select(p => new
                    {
                        Property = p,
                        OrderNo = MetaDataHelper.GetOrderNo(typeof(T), p.Name)
                    }).ToList();

            var rows = worksheet.Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);

            // Create the collection container
            var collection = rows.Skip(1)
                .Select(row =>
                {
                    Console.WriteLine(row);

                    var tnew = new T();
                    columns.ForEach(col =>
                    {
                        var val = worksheet.Cells[row, col.OrderNo];
                        if (val.Value == null)
                        {
                            col.Property.SetValue(tnew, null);
                            return;
                        }

                        var type = col.Property.PropertyType;
                        if (type == typeof(string))
                        {
                            col.Property.SetValue(tnew, val.GetValue<string>());
                            return;
                        }

                        if (type == typeof(int) || type == typeof(int?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<int>());
                            return;
                        }

                        if (type == typeof(decimal) || type == typeof(decimal?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<decimal>());
                            return;
                        }

                        if (type == typeof(bool) || type == typeof(bool?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<bool>());
                            return;
                        }

                        if (type == typeof(DateTime) || type == typeof(DateTime?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<DateTime>());
                            return;
                        }

                        if (type == typeof(DateTimeOffset) || (type == typeof(DateTimeOffset?)))
                        {
                            // col.Property.SetValue(tnew, null);
                            col.Property.SetValue(tnew, DateTimeOffset.Parse(val.Text));
                            return;
                        }

                        if (type == typeof(string))
                        {
                            col.Property.SetValue(tnew, val.GetValue<string>());
                            return;
                        }

                        // 列挙型(当アプリではenumをbyteで定義している)
                        // 2018/02/21時点ではnullableの列挙型は対応していない
                        if (type.IsSubclassOf(typeof(Enum)))
                        {
                            col.Property.SetValue(tnew, Enum.Parse(type, val.GetValue<string>(), true));
                            return;
                        }

                        // 該当なし
                        col.Property.SetValue(tnew, null);
                    });

                    return tnew;
                });

            // Send it back
            return collection;
        }
    }
}