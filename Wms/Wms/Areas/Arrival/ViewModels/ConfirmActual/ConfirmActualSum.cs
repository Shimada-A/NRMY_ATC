namespace Wms.Areas.Arrival.ViewModels.ConfirmActual
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 仕入入荷未棚付
    /// </summary>
    public class ConfirmActualSum
    {
        #region プロパティ

        /// <summary>
        /// SKU数
        /// </summary>
        public int ItemSkuSum { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        public int ArrivePlanQtySum { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        public int ResultQtySum { get; set; }

        /// <summary>
        /// 予定伝票数
        /// </summary>
        public int ArrivePlanSlipQtySum { get; set; }

        /// <summary>
        /// 実績伝票数
        /// </summary>
        public int ResultSlipQtySum { get; set; }

        #endregion
    }
}
