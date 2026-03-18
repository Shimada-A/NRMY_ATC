namespace Wms.Areas.Master.ViewModels.ShipFrontage
{
    /// <summary>
    /// ShipFrontage which is posted from view
    /// </summary>
    public class SelectedShipFrontageViewModel
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
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        public string CenterId { get; set; }

        /// <summary>
        /// レーンNo (LANE_NO)
        /// </summary>
        public int LaneNo { get; set; }

        /// <summary>
        /// 間口No (FRONTAGE_NO)
        /// </summary>
        public int FrontageNo { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }
    }
}