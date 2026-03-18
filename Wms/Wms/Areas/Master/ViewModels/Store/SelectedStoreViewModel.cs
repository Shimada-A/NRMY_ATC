namespace Wms.Areas.Master.ViewModels.Store
{
    using Wms.Common;

    /// <summary>
    /// Store which is posted from view
    /// </summary>
    public class SelectedStoreViewModel
    {
        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 荷主ID
        /// </summary>
        /// <remarks>主キーの最後に荷主IDを指定したいのでOrder=99とする</remarks>
        public string ShipperId { get; set; }

        /// <summary>
        /// 店舗ID
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }
    }
}