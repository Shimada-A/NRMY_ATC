namespace Wms.Areas.Arrival.ViewModels.UnshelvedReference
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 仕入入荷未棚付
    /// </summary>
    public class UnshelvedReferenceSum
    {
        #region プロパティ

        /// <summary>
        /// SKU数
        /// </summary>
        public int ItemSkuSum { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        public int CaseQtySum { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        public int StockQtySum { get; set; }

        /// <summary>
        /// バラ数
        /// </summary>
        public int TotalQtySum { get; set; }

        #endregion
    }
}
