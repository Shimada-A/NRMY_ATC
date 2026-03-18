namespace Wms.ViewModels.Home
{
    using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Resources;

    public class Arrive
    {
        /// <summary>
        /// 入荷予定日
        /// </summary>
        [Display(Name = nameof(HomeResource.ArrivePlanDate), ResourceType = typeof(HomeResource))]
        public string ArrivePlanDate { get; set; }

        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(HomeResource.CenterId), ResourceType = typeof(HomeResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷進捗
        /// </summary>
        [Display(Name = nameof(HomeResource.ArriveProgress), ResourceType = typeof(HomeResource))]
        public TcdcChart ArriveProgress { get; set; }

        /// <summary>
        /// TC入荷仕分進捗
        /// </summary>
        [Display(Name = nameof(HomeResource.TcArrivePartProgress), ResourceType = typeof(HomeResource))]
        public TcdcChart TcArrivePartProgress { get; set; }

        /// <summary>
        /// DC棚付進捗
        /// </summary>
        [Display(Name = nameof(HomeResource.DcShelvesProgress), ResourceType = typeof(HomeResource))]
        public TcdcChart DcShelvesProgress { get; set; }

        /// <summary>
        /// 当日日付
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd（ddd）}")]
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public class TcdcChart
    {
        public string Title { get; set; }

        public int TcQty { get; set; }

        public int DcQty { get; set; }
    }
}