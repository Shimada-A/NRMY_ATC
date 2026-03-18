namespace Wms.Areas.Returns.ViewModels.AcceptArrival
{
    using System.ComponentModel.DataAnnotations;

    public class AcceptArrival01ResultRow
    {

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JANスキャン回数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ScanQty { get; set; } = 1;
    }
}