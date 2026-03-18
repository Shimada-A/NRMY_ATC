namespace Wms.Areas.Master.ViewModels.Location
{
    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class SelectedLocationViewModel
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
        /// 倉庫ID
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// ロケーションコード
        /// </summary>
        public string LocationCd { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }
    }
}