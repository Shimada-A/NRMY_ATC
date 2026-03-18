namespace Wms.Areas.Styleguide.Models
{
    using System;

    public class DatePicker
    {
        // 初期値を今日にしたい場合
        public DateTime DateTime1 { get; set; } = DateTime.Now;

        // 初期値を空白にしたい場合
        public DateTime? DateTime2 { get; set; } = null;

        public DateTime DateTime3 { get; set; } = DateTime.Now.AddDays(10);
    }
}